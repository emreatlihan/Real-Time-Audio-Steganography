namespace RTASS.Common.ViewModels
{
    /// <summary>
    /// Steganografi parametrelerini taşıyan ViewModel
    /// </summary>
    public class SteganographyViewModel : BaseViewModel
    {
        public string? SecretMessage { get; set; }
        public double DelayOne { get; set; }
        public double DelayZero { get; set; }
        public double DecayRate { get; set; }
        public double MixingRate { get; set; }
        public int SegmentLength { get; set; }
    }
}
