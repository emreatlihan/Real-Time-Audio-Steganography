namespace RTASS.Common.Steganography.EchoHiding
{
    /// <summary>
    /// Echo Hiding parametreleri (delay, decay, mixing oranları vb.)
    /// </summary>
    public class EchoParameters
    {
        public double DelayOne { get; set; }
        public double DelayZero { get; set; }
        public double DecayRate { get; set; }
        public double MixingRate { get; set; }
        public int SegmentLength { get; set; }
    }
}
