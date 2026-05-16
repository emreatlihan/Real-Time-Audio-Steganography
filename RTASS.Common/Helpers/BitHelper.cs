namespace RTASS.Common.Helpers
{
    /// <summary>
    /// Bit düzeyinde dönüşüm yardımcı metotları
    /// </summary>
    public static class BitHelper
    {
        public static bool[] StringToBits(string secretMessage)
        {
            if (string.IsNullOrEmpty(secretMessage)) return Array.Empty<bool>();

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(secretMessage);
            bool[] bits = new bool[bytes.Length * 8];

            for (int i = 0; i < bytes.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bits[i * 8 + j] = ((bytes[i] >> (7 - j)) & 1) != 0;
                }
            }

            return bits;
        }

        public static string BitsToString(bool[] bits)
        {
            int byteCount = bits.Length / 8;
            byte[] bytes = new byte[byteCount];

            for (int i = 0; i < byteCount; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bits[i * 8 + j])
                    {
                        bytes[i] |= (byte)(1 << (7 - j));
                    }
                }
            }
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static bool[] IntToBits(int value)
        {
            bool[] bits = new bool[32];
            for (int i = 0; i < 32; i++)
            {
                bits[i] = ((value >> (31 - i)) & 1) != 0;
            }
            return bits;
        }

        public static int BitsToInt(bool[] bits)
        {
            int value = 0;
            for (int i = 0; i < 32 && i < bits.Length; i++)
            {
                if (bits[i])
                {
                    value |= (1 << (31 - i));
                }
            }
            return value;
        }
    }
}

