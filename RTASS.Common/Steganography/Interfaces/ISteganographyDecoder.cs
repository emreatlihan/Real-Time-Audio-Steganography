namespace RTASS.Common.Steganography.Interfaces
{
    /// <summary>
    /// Steganografi decoder arayüzü
    /// </summary>
    public interface ISteganographyDecoder
    {
        byte[] Decode(byte[] stegoAudioData);
    }
}
