using RTASS.Common.Constants;
using RTASS.Common.Steganography.EchoHiding;
using RTASS.Common.Steganography.Interfaces;

namespace RTASS.Encoder.Business
{
    /// <summary>
    /// Echo Hiding yöntemi ile veri gizleme (encoding)
    /// </summary>
    public class EchoEncoder : ISteganographyEncoder
    {
        public float[] Encode(float[] audioData, bool[] messageBits, EchoParameters parameters)
        {
            int sampleRate = AppConstants.DefaultSampleRate;
            int d1 = ConvertDelayToSamples(parameters.DelayOne, sampleRate);
            int d0 = ConvertDelayToSamples(parameters.DelayZero, sampleRate);
            float alpha = (float)parameters.DecayRate;

            float[] stego = (float[])audioData.Clone();

            for (int i = 0; i < messageBits.Length; i++)
            {
                int startIndex = i * parameters.SegmentLength;
                
                // Eğer ses verisi segment için yetersizse döngüden çık
                if (startIndex >= audioData.Length) break;

                int delay = messageBits[i] ? d1 : d0;

                ApplyEchoToSegment(audioData, stego, startIndex, parameters.SegmentLength, delay, alpha);
            }

            return stego;
        }

        private int ConvertDelayToSamples(double delayInSeconds, int sampleRate)
        {
            return (int)(delayInSeconds * sampleRate);
        }

        private void ApplyEchoToSegment(float[] source, float[] output, int startIndex, int segmentLength, int delaySamples, float decayRate)
        {
            for (int j = 0; j < segmentLength; j++)
            {
                int currentSampleIndex = startIndex + j;
                
                // Dizi sınırlarını kontrol et
                if (currentSampleIndex < output.Length && currentSampleIndex >= delaySamples)
                {
                    float echoValue = source[currentSampleIndex - delaySamples] * decayRate;
                    float newValue = source[currentSampleIndex] + echoValue;
                    
                    // Clipping'i önlemek için clamping uygula
                    output[currentSampleIndex] = Math.Clamp(newValue, -1f, 1f);
                }
            }
        }
    }
}

