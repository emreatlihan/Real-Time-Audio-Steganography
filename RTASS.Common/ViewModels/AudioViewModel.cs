namespace RTASS.Common.ViewModels
{
    /// <summary>
    /// Ses dosyası bilgilerini taşıyan ViewModel
    /// </summary>
    public class AudioViewModel : BaseViewModel
    {
        public string? FilePath { get; set; }
        public int SampleRate { get; set; }
        public int Channels { get; set; }
        public int BitDepth { get; set; }
        public double Duration { get; set; }
    }
}
