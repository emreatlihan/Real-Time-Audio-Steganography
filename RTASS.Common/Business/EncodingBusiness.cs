using RTASS.Common.Constants;
using RTASS.Common.Helpers;
using RTASS.Common.Steganography.EchoHiding;
using RTASS.Common.Steganography.Interfaces;
using RTASS.Common.Enums;

namespace RTASS.Common.Business
{
    public class EncodingBusiness
    {
        private readonly ISteganographyEncoder _encoder;
        private readonly List<bool> _bitBuffer = new List<bool>();

        public bool HasPendingData => _bitBuffer.Count > 0;

        public EncodingBusiness(ISteganographyEncoder encoder)
        {
            _encoder = encoder;
        }

        public void PrepareMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            bool[] messageBits = BitHelper.StringToBits(message);
            bool[] lengthBits = BitHelper.IntToBits(messageBits.Length);

            lock (_bitBuffer)
            {
                _bitBuffer.AddRange(AppConstants.SyncPattern);
                _bitBuffer.AddRange(lengthBits);
                _bitBuffer.AddRange(messageBits);
            }
        }

        public void ProcessAudioChunk(float[] audioData, EchoParameters parameters)
        {
            lock (_bitBuffer)
            {
                if (_bitBuffer.Count == 0) return;

                int maxBitsCapacity = audioData.Length / parameters.SegmentLength;
                if (maxBitsCapacity == 0) return;

                int bitsToTake = Math.Min(maxBitsCapacity, _bitBuffer.Count);
                bool[] bitsToEncode = _bitBuffer.Take(bitsToTake).ToArray();

                float[] encodedAudio = _encoder.Encode(audioData, bitsToEncode, parameters);

                Array.Copy(encodedAudio, audioData, audioData.Length);
                _bitBuffer.RemoveRange(0, bitsToTake);
            }
        }

        public void ClearBuffer()
        {
            lock (_bitBuffer)
            {
                _bitBuffer.Clear();
            }
        }
    }
}
