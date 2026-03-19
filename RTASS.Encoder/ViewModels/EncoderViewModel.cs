using RTASS.Common.ViewModels;

namespace RTASS.Encoder.ViewModels
{
    /// <summary>
    /// Encoder ekranı ViewModel
    /// </summary>
    public class EncoderViewModel : BaseViewModel
    {
        public string? InputFilePath { get; set; }
        public string? OutputFilePath { get; set; }
        public string? SecretMessage { get; set; }
        public bool IsEncoding { get; set; }
        public double Progress { get; set; }
        public string? StatusMessage { get; set; }
    }
}
