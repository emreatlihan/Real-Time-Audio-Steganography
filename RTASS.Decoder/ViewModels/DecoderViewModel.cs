using RTASS.Common.ViewModels;

namespace RTASS.Decoder.ViewModels
{
    /// <summary>
    /// Decoder ekranı ViewModel
    /// </summary>
    public class DecoderViewModel : BaseViewModel
    {
        public string? InputFilePath { get; set; }
        public string? DecodedMessage { get; set; }
        public bool IsDecoding { get; set; }
        public double Progress { get; set; }
        public string? StatusMessage { get; set; }
    }
}
