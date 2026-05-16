using RTASS.Common.Constants;
using RTASS.Common.Steganography.EchoHiding;
using RTASS.Decoder.Business;

namespace RTASS.Decoder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== EchoDecoder Test ===\n");

            // Test parametreleri
            var parameters = new EchoParameters
            {
                DelayOne = AppConstants.DefaultDelayOne,       // 0.01s → 441 sample
                DelayZero = AppConstants.DefaultDelayZero,     // 0.005s → 220 sample
                DecayRate = AppConstants.DefaultDecayRate,      // 0.5
                MixingRate = AppConstants.DefaultMixingRate,    // 0.7
                SegmentLength = AppConstants.DefaultSegmentLength // 8192
            };

            // Gömülecek test bitleri
            bool[] expectedBits = { true, false, true };
            int segmentLength = parameters.SegmentLength;
            int sampleRate = AppConstants.DefaultSampleRate;
            int totalSamples = expectedBits.Length * segmentLength;

            // 1. Sahte ses üret (440 Hz sinüs dalgası)
            Console.WriteLine("1. Sinüs dalgası üretiliyor...");
            float[] audio = new float[totalSamples];
            for (int n = 0; n < totalSamples; n++)
            {
                audio[n] = MathF.Sin(2 * MathF.PI * 440 * n / sampleRate);
            }

            // 2. Bilinen bitlerle elle yankı ekle (Encoder simülasyonu)
            Console.WriteLine("2. Bilinen bitlerle yankı ekleniyor (Encoder simülasyonu)...");
            float[] stegoAudio = new float[totalSamples];
            Array.Copy(audio, stegoAudio, totalSamples);

            double alpha = parameters.DecayRate;

            for (int i = 0; i < expectedBits.Length; i++)
            {
                int delay = expectedBits[i]
                    ? (int)(parameters.DelayOne * sampleRate)   // bit 1 → 441 sample
                    : (int)(parameters.DelayZero * sampleRate); // bit 0 → 220 sample

                int startIndex = i * segmentLength;

                for (int n = startIndex; n < startIndex + segmentLength; n++)
                {
                    if (n - delay >= 0)
                    {
                        stegoAudio[n] = audio[n] + (float)(alpha * audio[n - delay]);
                    }
                }
            }

            // 3. Decoder'ı çalıştır
            Console.WriteLine("3. EchoDecoder çalıştırılıyor...\n");
            var decoder = new EchoDecoder();
            bool[] decodedBits = decoder.Decode(stegoAudio, parameters);

            // 4. Sonuçları karşılaştır
            Console.WriteLine("Sonuçlar:");
            Console.WriteLine($"  Beklenen:  [{string.Join(", ", expectedBits.Select(b => b ? "1" : "0"))}]");
            Console.WriteLine($"  Çıkarılan: [{string.Join(", ", decodedBits.Select(b => b ? "1" : "0"))}]");
            Console.WriteLine();

            bool allMatch = true;
            for (int i = 0; i < expectedBits.Length; i++)
            {
                string status = expectedBits[i] == decodedBits[i] ? "+" : "-";
                Console.WriteLine($"  Bit {i}: Beklenen={expectedBits[i]}, Çıkan={decodedBits[i]} {status}");
                if (expectedBits[i] != decodedBits[i]) allMatch = false;
            }

            Console.WriteLine();
            Console.WriteLine(allMatch
                ? "SONUÇ: BAŞARILI — Tüm bitler doğru çıkarıldı!"
                : "SONUÇ: BAŞARISIZ — Bazı bitler yanlış çıkarıldı.");
        }
    }
}
