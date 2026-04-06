namespace RTASS.Common.Helpers
{
    /// <summary>
    /// Ses işleme yardımcı metotları
    /// </summary>
    public static class AudioHelper
    {
        public static float[] ToFloatArray(byte[] byteArray, int bytesRead)
        {
            // Her 2 byte = 1 ses örneği (sample)
            float[] floatArray = new float[bytesRead / 2];

            for (int i = 0; i < floatArray.Length; i++)
            {
                // İki byte'ı birleştirip 16-bit tam sayı (short) yapıyoruz
                short sample = BitConverter.ToInt16(byteArray, i * 2);

                // Sayıyı -1.0 ile 1.0 arasına normalize ediyoruz (32768 = 2^15)
                floatArray[i] = sample / 32768f;
            }
            return floatArray;
        }

        // İşlenmiş Float dizisini tekrar 16-bit Byte dizisine çevirir (Sanal Kabloya göndermek için)
        public static byte[] ToByteArray(float[] floatArray)
        {
            byte[] byteArray = new byte[floatArray.Length * 2];

            for (int i = 0; i < floatArray.Length; i++)
            {
                // Sesi sınırda tut (Hoparlör patlamasın diye -1 ile 1 dışına çıkarma)
                float sample = Math.Clamp(floatArray[i], -1f, 1f);

                // Tekrar short (-32768 ile 32767 arası) yapıyoruz
                short shortSample = (short)(sample * 32767);

                byte[] bytes = BitConverter.GetBytes(shortSample);
                byteArray[i * 2] = bytes[0];
                byteArray[i * 2 + 1] = bytes[1];
            }
            return byteArray;
        }
    }
}
