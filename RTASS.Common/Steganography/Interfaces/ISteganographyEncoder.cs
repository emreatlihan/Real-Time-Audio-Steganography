using RTASS.Common.Steganography.EchoHiding;

namespace RTASS.Common.Steganography.Interfaces
{
    /// <summary>
    /// Steganografi encoder arayüzü
    /// </summary>
    public interface ISteganographyEncoder
    {
        float[] Encode(float[] audioData, bool[] messageBits, EchoParameters parameters);
    }
}
