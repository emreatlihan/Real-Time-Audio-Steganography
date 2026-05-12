using RTASS.Common.Constants;
using RTASS.Common.Helpers;
using RTASS.Common.Steganography.EchoHiding;
using RTASS.Common.Steganography.Interfaces;
using RTASS.Common.Enums;

namespace RTASS.Common.Business
{
    public class DecodingBusiness
    {
        private readonly ISteganographyDecoder _decoder;
        private readonly List<bool> _bitBuffer = new List<bool>();
        private DecoderState _state = DecoderState.SearchingSync;
        private int _expectedMessageLength = 0;

        // Mesaj tam olarak çözüldüğünde tetiklenen olay.
        public event Action<string>? OnMessageReceived;

        public DecodingBusiness(ISteganographyDecoder decoder)
        {
            _decoder = decoder;
        }

        // Gelen ses parçası üzerinden decoding işlemini yürütür.
        public void ProcessAudioChunk(float[] audioData, EchoParameters parameters)
        {
            bool[] bits = _decoder.Decode(audioData, parameters);
            
            _bitBuffer.AddRange(bits);

            ProcessBuffer();
        }

        private void ProcessBuffer()
        {
            bool continueProcessing = true;
            while (continueProcessing && _bitBuffer.Count > 0)
            {
                switch (_state)
                {
                    case DecoderState.SearchingSync:
                        continueProcessing = SearchForSync();
                        break;
                    case DecoderState.ReadingLength:
                        continueProcessing = ReadLength();
                        break;
                    case DecoderState.ReadingMessage:
                        continueProcessing = ReadMessage();
                        break;
                }
            }
        }

        private bool SearchForSync()
        {
            var pattern = AppConstants.SyncPattern;
            if (_bitBuffer.Count < pattern.Length) return false;

            // Kayan pencere ile Sync Pattern ara
            for (int i = 0; i <= _bitBuffer.Count - pattern.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (_bitBuffer[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    // Sync bulundu: Öncesindeki çöpleri ve sync'i sil, sonraki aşamaya geç
                    _bitBuffer.RemoveRange(0, i + pattern.Length);
                    _state = DecoderState.ReadingLength;
                    return true;
                }
            }

            // Sync bulunamadıysa, son pattern boyutu kadar bit bırakıp gerisini temizle
            if (_bitBuffer.Count > pattern.Length)
            {
                _bitBuffer.RemoveRange(0, _bitBuffer.Count - pattern.Length + 1);
            }

            return false;
        }

        private bool ReadLength()
        {
            if (_bitBuffer.Count < AppConstants.MessageLengthHeaderBits) return false;

            // İlk 32 biti al ve sayıya çevir
            bool[] lengthBits = _bitBuffer.Take(AppConstants.MessageLengthHeaderBits).ToArray();
            _expectedMessageLength = BitHelper.BitsToInt(lengthBits);
            
            // Okunan kısmı temizle ve mesaj okuma moduna geç
            _bitBuffer.RemoveRange(0, AppConstants.MessageLengthHeaderBits);
            _state = DecoderState.ReadingMessage;
            
            return true;
        }

        private bool ReadMessage()
        {
            if (_bitBuffer.Count < _expectedMessageLength) return false;

            // Belirlenen uzunlukta biti al ve string'e çevir
            bool[] messageBits = _bitBuffer.Take(_expectedMessageLength).ToArray();
            string message = BitHelper.BitsToString(messageBits);
            
            // Kuyruğu temizle ve tekrar yeni mesaj aramaya (Sync) dön
            _bitBuffer.RemoveRange(0, _expectedMessageLength);
            _state = DecoderState.SearchingSync;

            // Olayı tetikle
            OnMessageReceived?.Invoke(message);
            
            return true;
        }
    }
}
