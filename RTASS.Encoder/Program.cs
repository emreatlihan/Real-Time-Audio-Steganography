using RTASS.Common.Audio;
using RTASS.Common.Business;
using RTASS.Common.Constants;
using RTASS.Common.Steganography.EchoHiding;
using RTASS.Encoder.Business;

namespace RTASS.Encoder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("========================================");
            Console.WriteLine("||   RTASS ENCODER - HACKER TERMINAL  ||");
            Console.WriteLine("========================================\n");
            Console.ResetColor();

            string sharedDir = @"C:\RTASS_Shared";
            if (!Directory.Exists(sharedDir))
            {
                Directory.CreateDirectory(sharedDir);
            }

            string carrierPath = Path.Combine(sharedDir, "audio_carrier.wav");

            if (!File.Exists(carrierPath))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("[Uyarı] audio_carrier.wav bulunamadı. Test için 2 saniyelik boş ses oluşturuluyor...");
                CreateDummyWav(carrierPath);
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Gizlenecek Mesajı Girin: ");
            Console.ResetColor();
            string? message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hata: Mesaj boş olamaz!");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[>] Encoding Business başlatılıyor...");
            var encoder = new EchoEncoder();
            var business = new EncodingBusiness(encoder);
            
            business.PrepareMessage(message);

            Console.WriteLine("[>] Ses dosyası okunuyor ve mesaj gömülüyor...");
            
            var reader = new AudioReader();
            float[] audioData = reader.ReadWavFile(carrierPath);

            var parameters = new EchoParameters
            {
                DelayOne = AppConstants.DefaultDelayOne,
                DelayZero = AppConstants.DefaultDelayZero,
                DecayRate = AppConstants.DefaultDecayRate,
                MixingRate = AppConstants.DefaultMixingRate,
                SegmentLength = AppConstants.DefaultSegmentLength
            };

            business.ProcessAudioChunk(audioData, parameters);

            var writer = new AudioWriter();
            writer.WriteWavFile(carrierPath, audioData, AppConstants.DefaultSampleRate, 1);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[+] BAŞARILI: Mesaj sese enjekte edildi ve audio_carrier.wav dosyasına yazıldı.");
            Console.ResetColor();
        }

        private static void CreateDummyWav(string path)
        {
            int sampleRate = AppConstants.DefaultSampleRate;
            int totalSamples = sampleRate * 30; // 30 saniyelik ses (Mesaj kapasitesi için en az 20sn lazım)
            float[] audio = new float[totalSamples];
            for (int n = 0; n < totalSamples; n++)
            {
                audio[n] = (float)Math.Sin(2 * Math.PI * 440 * n / sampleRate) * 0.5f;
            }
            var writer = new AudioWriter();
            writer.WriteWavFile(path, audio, sampleRate, 1);
        }
    }
}