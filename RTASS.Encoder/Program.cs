namespace RTASS.Encoder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            public float[] Encode(float[] originalAudio, bool[] bitsToHide, EchoParameters parameters)
            {
                int sampleRate = AppConstants.DefaultSampleRate;
                int delayOnesSamples = ConvertDelayToSamples(parameters.DelayOne, sampleRate);
                int delayZerosSamples = ConvertDelayToSamples(parameters.DelayZero, sampleRate);
                int segmentLength = parameters.SegmentLength;
    
                // Yankı katsayısı (Duyulabilirlik vs Tespit edilebilirlik dengesi)
                float alpha = 0.3f; 

                float[] stegoAudio = (float[])originalAudio.Clone();

                for (int i = 0; i < bitsToHide.Length; i++)
                {
                    int startIndex = i * segmentLength;
                    int currentDelay = bitsToHide[i] ? delayOnesSamples : delayZerosSamples;
                    for (int j = 0; j < segmentLength; j++)
                    {
                        int currentIndex = startIndex + j;
                        int echoIndex = currentIndex - currentDelay;
                        // Yankı eklenecek indeksin sınır kontrolü
                        if (echoIndex >= 0)
                        {
                            stegoAudio[currentIndex] += alpha * originalAudio[echoIndex];
                        }
                    }
                }

                return stegoAudio;
            }
        }
    }
}
