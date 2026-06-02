using RTASS.Common.Constants;
using RTASS.Common.Helpers;
using RTASS.Common.Steganography.EchoHiding;
using RTASS.Common.Steganography.Interfaces;

namespace RTASS.Decoder.Business
{
    /// <summary>
    /// Echo Hiding yöntemi ile gizli veriyi çıkarma (decoding).
    /// Cepstrum analizi kullanarak her segmentteki yankı gecikmesini tespit eder
    /// ve gecikme süresine göre bit 0 veya bit 1 kararı verir.
    /// </summary>
    public class EchoDecoder : ISteganographyDecoder
    {
        public bool[] Decode(float[] stegoAudioData, EchoParameters parameters)
        {
            int sampleRate = AppConstants.DefaultSampleRate;
            int delayOneSamples = ConvertDelayToSamples(parameters.DelayOne, sampleRate);
            int delayZeroSamples = ConvertDelayToSamples(parameters.DelayZero, sampleRate);
            int segmentLength = parameters.SegmentLength;

            int totalSegments = stegoAudioData.Length / segmentLength;

            if (totalSegments == 0)
            {
                throw new ArgumentException("Ses verisi en az bir segment oluşturacak uzunlukta olmalıdır.");
            }

            bool[] decodedBits = new bool[totalSegments];

            for (int i = 0; i < totalSegments; i++)
            {
                int startIndex = i * segmentLength;

                float[] segment = new float[segmentLength];
                Array.Copy(stegoAudioData, startIndex, segment, 0, segmentLength);

                float[] cepstrum = ComputeCepstrum(segment);

                decodedBits[i] = DetectBit(cepstrum, delayOneSamples, delayZeroSamples);
            }

            return decodedBits;
        }

        /// <summary>
        /// Saniye cinsinden gecikmeyi sample sayısına çevirir.
        /// Örnek: 0.01 saniye * 44100 Hz = 441 sample
        /// </summary>
        private int ConvertDelayToSamples(double delayInSeconds, int sampleRate)
        {
            return (int)(delayInSeconds * sampleRate);
        }

        /// <summary>
        /// Tek bir ses segmentinden Cepstrum hesaplar.
        /// Adımlar: Pencereleme → FFT → Magnitude → Log → Ters FFT
        /// </summary>
        private float[] ComputeCepstrum(float[] segment)
        {
            int length = segment.Length;

            // Adım 1: Pencereleme — spectral leakage'ı önle
            WindowHelper.ApplyHannWindow(segment);

            // Adım 2: FFT — zaman → frekans dönüşümü
            float[] real = new float[length];
            float[] imaginary = new float[length];
            Array.Copy(segment, real, length);

            FftHelper.ForwardFFT(real, imaginary);

            // Adım 3: Güç spektrumu (magnitude)
            float[] magnitude = FftHelper.ComputeMagnitude(real, imaginary);

            // Adım 4: Logaritma — enerji farklarını belirginleştir
            for (int i = 0; i < magnitude.Length; i++)
            {
                magnitude[i] = MathF.Log(magnitude[i] + AppConstants.Epsilon);
            }

            // Adım 5: Ters FFT — log-magnitude'den cepstrum elde et
            float[] cepstrumReal = magnitude;
            float[] cepstrumImaginary = new float[length];

            FftHelper.InverseFFT(cepstrumReal, cepstrumImaginary);

            return cepstrumReal;
        }

        /// <summary>
        /// Cepstrum dizisindeki tepecik değerlerinin yerel belirginliğini (prominence) karşılaştırarak gömülen biti tespit eder.
        /// delayOne indeksindeki belirginlik daha büyükse → bit 1 (true)
        /// delayZero indeksindeki belirginlik daha büyükse → bit 0 (false)
        /// </summary>
        private bool DetectBit(float[] cepstrum, int delayOneSamples, int delayZeroSamples)
        {
            float prominenceOne = GetPeakProminence(cepstrum, delayOneSamples);
            float prominenceZero = GetPeakProminence(cepstrum, delayZeroSamples);

            return prominenceOne > prominenceZero;
        }

        /// <summary>
        /// Belirli bir gecikme indeksindeki tepeciğin yerel komşuluğuna göre ne kadar belirgin (prominent) olduğunu hesaplar.
        /// Bu, sesin kendi frekans karakteristiğinden kaynaklanan doğal cepstrum düşüşünü (decay) dengeler.
        /// </summary>
        private float GetPeakProminence(float[] cepstrum, int delayIndex)
        {
            int neighborhoodHalfWidth = 10; // Yerel baseline pencere genişliği (yarı çap)
            int excludeHalfWidth = 2;       // Tepeciğin kendisinin baseline'ı kirletmesini önlemek için hariç tutulan alan
            
            float sum = 0f;
            int count = 0;

            for (int i = -neighborhoodHalfWidth; i <= neighborhoodHalfWidth; i++)
            {
                if (i >= -excludeHalfWidth && i <= excludeHalfWidth)
                {
                    continue; // Tepeciğin kendisini ve hemen bitişiğindeki yayılımı baseline hesaplamasına katma
                }

                int idx = delayIndex + i;
                if (idx >= 0 && idx < cepstrum.Length)
                {
                    sum += MathF.Abs(cepstrum[idx]);
                    count++;
                }
            }

            if (count == 0) 
            {
                return MathF.Abs(cepstrum[delayIndex]);
            }

            float baseline = sum / count;
            
            // Tepecik değerinin yerel ortalamaya oranı (SNR benzeri metrik)
            return MathF.Abs(cepstrum[delayIndex]) / (baseline + 1e-9f);
        }
    }
}
