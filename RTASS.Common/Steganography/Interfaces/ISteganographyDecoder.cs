using RTASS.Common.Steganography.EchoHiding;

namespace RTASS.Common.Steganography.Interfaces
{
    /// <summary>
    /// Steganografi decoder arayüzü
    /// </summary>
    public interface ISteganographyDecoder
    {
        bool[] Decode(float[] stegoAudioData, EchoParameters parameters);
    }
}
