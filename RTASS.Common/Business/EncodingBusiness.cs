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

        // Kuyrukta gönderilmeyi bekleyen bit olup olmadığını kontrol etmek için
        public bool HasPendingData => _bitBuffer.Count > 0;

        public EncodingBusiness(ISteganographyEncoder encoder)
        {
            _encoder = encoder;
        }

        /// <summary>
        /// Gönderilmek istenen metin mesajını Sync ve Uzunluk flagleri ile birlikte bit kuyruğuna ekler.
        /// </summary>
        public void PrepareMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            // 1. Metni bit dizisine çevir
            bool[] messageBits = BitHelper.StringToBits(message);

            // 2. Mesajın bit uzunluğunu 32 bitlik bir diziye çevir
            bool[] lengthBits = BitHelper.IntToBits(messageBits.Length);

            // 3. Decoding katmanının beklediği protokol sırasıyla buffer'a ekle:
            // [SYNC PATTERN (16 bit)] + [MESAJ UZUNLUĞU (32 bit)] + [ASIL MESAJ BİTLERİ]
            lock (_bitBuffer)
            {
                _bitBuffer.AddRange(AppConstants.SyncPattern);
                _bitBuffer.AddRange(lengthBits);
                _bitBuffer.AddRange(messageBits);
            }
        }
        /// Canlı hattan gelen ses parçasına (chunk) kuyruktaki bitleri gömer.
        /// <param name="audioData">Üzerine veri yazılacak canlı ses verisi</param>
        /// <param name="parameters">Echo Hiding parametreleri</param>
        public void ProcessAudioChunk(float[] audioData, EchoParameters parameters)
        {
            lock (_bitBuffer)
            {
                // Eğer gömülecek veri yoksa ses verisine dokunmadan çık
                if (_bitBuffer.Count == 0) return;

                // 1. Bu ses parçasına (chunk) EN FAZLA kaç bit sığabileceğini hesapla
                int maxBitsCapacity = audioData.Length / parameters.SegmentLength;
                
                if (maxBitsCapacity == 0) return;

                int bitsToTake = Math.Min(maxBitsCapacity, _bitBuffer.Count);
                bool[] bitsToEncode = _bitBuffer.Take(bitsToTake).ToArray();

                float[] encodedAudio = _encoder.Encode(audioData, bitsToEncode, parameters);

                Array.Copy(encodedAudio, audioData, audioData.Length);
                _bitBuffer.RemoveRange(0, bitsToTake);
            }
        }
        /// Gönderimi iptal etmek veya buffer'ı temizlemek gerekirse kullanılır.
        public void ClearBuffer()
        {
            lock (_bitBuffer)
            {
                _bitBuffer.Clear();
            }
        }
    }
}
