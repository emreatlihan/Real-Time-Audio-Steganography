namespace RTASS.Common.Helpers
{
    /// <summary>
    /// Dosya işlemleri yardımcı metotları
    /// </summary>
    public static class FileHelper
    {
        public static byte[] GetFileBytes(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Dosya bulunamadı: {filePath}");

            return File.ReadAllBytes(filePath);
        }

        public static void SaveFileBytes(string filePath, byte[] data)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("Kaydedilecek veri boş olamaz.");

            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllBytes(filePath, data);
        }
    }
}
