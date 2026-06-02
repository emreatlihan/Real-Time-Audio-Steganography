namespace RTASS.Common.Constants
{
    /// <summary>
    /// Uygulama genelinde kullanılan sabit değerler
    /// </summary>
    public static class AppConstants
    {
        public const int DefaultSampleRate = 48000;
        public const int DefaultBitDepth = 16;
        public const int DefaultChannels = 1;
        public const double DefaultDelayOne = 0.01;
        public const double DefaultDelayZero = 0.005;
        public const double DefaultDecayRate = 0.1; 
        public const double DefaultMixingRate = 0.7;
        public const int DefaultSegmentLength = 8192;
        public const float Epsilon = 1e-10f;

        public static readonly bool[] SyncPattern = { true, false, true, false, true, false, true, false, 
                                                      false, true, false, true, false, true, false, true }; // 10101010 01010101
        public const int MessageLengthHeaderBits = 32;
    }
}
