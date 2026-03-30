using RTASS.Common.Steganography.Interfaces;

namespace RTASS.Encoder.Business
{
    /// <summary>
    /// Echo Hiding yöntemi ile veri gizleme (encoding)
    /// </summary>
    public class EchoEncoder : ISteganographyEncoder
    {
        public byte[] Encode(byte[] audioData, byte[] secretData)
        {
            throw new NotImplementedException();
        }
    }
}
