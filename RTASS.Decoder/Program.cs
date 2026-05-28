using RTASS.Common.Audio;
using RTASS.Common.Business;
using RTASS.Common.Constants;
using RTASS.Common.Steganography.EchoHiding;
using RTASS.Decoder.Business;

namespace RTASS.Decoder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("========================================");
            Console.WriteLine("||   RTASS DECODER - HACKER TERMINAL  ||");
            Console.WriteLine("========================================\n");
            Console.ResetColor();

            Console.WriteLine("[*] Dinleniyor... (audio_carrier.wav bekleniyor)\n");

            string sharedDir = @"C:\RTASS_Shared";
            string carrierPath = Path.Combine(sharedDir, "audio_carrier.wav");

            if (!File.Exists(carrierPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hata: Ses dosyası bulunamadı! Lütfen önce Encoder ile veri gömün.");
                Console.ResetColor();
                return;
            }

            var decoder = new EchoDecoder();
            var business = new DecodingBusiness(decoder);
            
            // Event tetiklendiğinde ekrana yazdırılacak işlem
            business.OnMessageReceived += (message) =>
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("!!! YENİ MESAJ YAKALANDI !!!");
                Console.ForegroundColor = ConsoleColor.Green;
                
                // Daktilo efekti
                Console.Write(">> ");
                foreach (char c in message)
                {
                    Console.Write(c);
                    Thread.Sleep(50); // 50ms gecikme
                }
                Console.WriteLine("\n");
                Console.ResetColor();
            };

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

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[>] Ses analizi başlatıldı...");
            Console.ResetColor();

            // Tüm sesi tek parça halinde veriyoruz (P4'te bu parça parça akacak)
            business.ProcessAudioChunk(audioData, parameters);

            Console.WriteLine("[+] Analiz tamamlandı. Çıkmak için Enter'a basın.");
            Console.ReadLine();
        }
    }
}
