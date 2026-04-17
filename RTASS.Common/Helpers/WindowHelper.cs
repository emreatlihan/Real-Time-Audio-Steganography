namespace RTASS.Common.Helpers
{
    public static class WindowHelper
    {
        public static void ApplyHannWindow(float[] frame)
        {
            int N = frame.Length;
            for (int i = 0; i < N; i++)
            {
                // Hann Formülü: 0.5 * (1 - cos(2π * i / (N - 1)))
                double multiplier = 0.5 * (1 - Math.Cos(2 * Math.PI * i / (N - 1)));
                frame[i] *= (float)multiplier;
            }
        }

        public static void ApplyHammingWindow(float[] frame)
        {
            int N = frame.Length;
            for (int i = 0; i < N; i++)
            {
                // Hamming Formülü: 0.54 - 0.46 * cos(2π * i / (N - 1))
                double multiplier = 0.54 - 0.46 * Math.Cos(2 * Math.PI * i / (N - 1));
                frame[i] *= (float)multiplier;
            }
        }
    }
}
