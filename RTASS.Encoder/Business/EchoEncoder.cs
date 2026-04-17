using RTASS.Common.Steganography.EchoHiding;
using RTASS.Common.Steganography.Interfaces;

namespace RTASS.Encoder.Business
{
    /// <summary>
    /// Echo Hiding yöntemi ile veri gizleme (encoding)
    /// </summary>
    public class EchoEncoder : ISteganographyEncoder
    {
        public float[] Encode(float[] audioData, bool[] messageBits, EchoParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}

