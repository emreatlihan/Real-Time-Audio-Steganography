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

                // Mevcut buffer'daki bitleri encoder'a gönderiyoruz.
                // Encoder, ses chunk'ının boyutuna (ve block size değerine) göre 
                // bu chunk'a kaç bit sığdırabileceğini hesaplar ve gömer.
                bool[] bitsToEncode = _bitBuffer.ToArray();

                // Encoder'ın bu chunk içerisine başarıyla gömebildiği bit sayısını geri alıyoruz.
                float[] encodedAudio = _encoder.Encode(audioData, bitsToEncode, parameters);
                int bitsEncoded = bitsToEncode.Length; 

                if (bitsEncoded > 0)
                {
                    // Başarıyla gömülen bitleri kuyruktan temizle, geri kalanı bir sonraki chunk'a kalsın
                    _bitBuffer.RemoveRange(0, Math.Min(bitsEncoded, _bitBuffer.Count));
                }
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
