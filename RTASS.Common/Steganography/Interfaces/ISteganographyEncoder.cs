namespace RTASS.Common.Steganography.Interfaces
{
    /// <summary>
    /// Steganografi encoder arayüzü
    /// </summary>
    public interface ISteganographyEncoder
    {
        byte[] Encode(byte[] audioData, byte[] secretData);
    }
}
