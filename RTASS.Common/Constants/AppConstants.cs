namespace RTASS.Common.Constants
{
    /// <summary>
    /// Uygulama genelinde kullanılan sabit değerler
    /// </summary>
    public static class AppConstants
    {
        public const int DefaultSampleRate = 44100;
        public const int DefaultBitDepth = 16;
        public const int DefaultChannels = 1;
        public const double DefaultDelayOne = 0.01;
        public const double DefaultDelayZero = 0.005;
        // Sesin bozulmasını (metalik yankı ve cızırtı) önlemek için %50'den %10'a düşürüldü
        public const double DefaultDecayRate = 0.1; 
        public const double DefaultMixingRate = 0.7;
        public const int DefaultSegmentLength = 8192;
        public const float Epsilon = 1e-10f;

        // P2 - Mesaj Protokolü Sabitleri (Senkronizasyon ve Uzunluk)
        public static readonly bool[] SyncPattern = { true, false, true, false, true, false, true, false, 
                                                      false, true, false, true, false, true, false, true }; // 10101010 01010101
        public const int MessageLengthHeaderBits = 32;
    }
}
