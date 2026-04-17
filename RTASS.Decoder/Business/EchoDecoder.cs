using RTASS.Common.Steganography.EchoHiding;
using RTASS.Common.Steganography.Interfaces;

namespace RTASS.Decoder.Business
{
    /// <summary>
    /// Echo Hiding yöntemi ile gizli veriyi çıkarma (decoding)
    /// </summary>
    public class EchoDecoder : ISteganographyDecoder
    {
        public bool[] Decode(float[] stegoAudioData, EchoParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}

