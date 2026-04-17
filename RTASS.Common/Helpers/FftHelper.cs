using MathNet.Numerics.IntegralTransforms;
using System.Numerics;

namespace RTASS.Common.Helpers
{
    public static class FftHelper
    {
        /// <summary>
        /// Zaman domainindeki veriyi Frekans domainine çevirir.
        /// </summary>
        public static void ForwardFFT(float[] real, float[] imaginary)
        {
            if (real.Length != imaginary.Length)
                throw new ArgumentException("Real ve Imaginary dizilerinin uzunlukları eşit olmalıdır.");

            int n = real.Length;

            Complex[] complexArray = new Complex[n];
            for (int i = 0; i < n; i++)
            {
                complexArray[i] = new Complex(real[i], imaginary[i]);
            }

            // 2. FFT İşlemi
            Fourier.Forward(complexArray, FourierOptions.Default);

            // 3. Sonuçları orijinal dizilere geri yazma kısmı
            for (int i = 0; i < n; i++)
            {
                real[i] = (float)complexArray[i].Real;
                imaginary[i] = (float)complexArray[i].Imaginary;
            }
        }

        /// <summary>
        /// Frekans domainindeki veriyi tekrar Zaman domainine (sese) çevirir.
        /// </summary>
        public static void InverseFFT(float[] real, float[] imaginary)
        {
            int n = real.Length;
            Complex[] complexArray = new Complex[n];

            for (int i = 0; i < n; i++)
                complexArray[i] = new Complex(real[i], imaginary[i]);

            // Ters FFT (Inverse) uygula
            Fourier.Inverse(complexArray, FourierOptions.Default);

            for (int i = 0; i < n; i++)
            {
                real[i] = (float)complexArray[i].Real;
                imaginary[i] = (float)complexArray[i].Imaginary;
            }
        }

        /// <summary>
        /// Karmaşık sayıların büyüklüğünü (enerjisini) hesaplar.
        /// Formül: sqrt(real^2 + imag^2)
        /// </summary>
        public static float[] ComputeMagnitude(float[] real, float[] imaginary)
        {
            int n = real.Length;
            float[] magnitude = new float[n];

            for (int i = 0; i < n; i++)
            {
                // Pisagor: Hipotenüs hesaplar gibi enerjiyi buluyoruz
                magnitude[i] = MathF.Sqrt(real[i] * real[i] + imaginary[i] * imaginary[i]);
            }

            return magnitude;
        }
    }
}
