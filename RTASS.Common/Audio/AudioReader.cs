using NAudio.Wave;
using RTASS.Common.Constants;

namespace RTASS.Common.Audio
{
    /// <summary>
    /// WAV (veya MP3) dosyalarını okuma işlemleri
    /// </summary>
    public class AudioReader
    {
        public float[] ReadWavFile(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                var floats = new List<float>();
                // 1 saniyelik parçalar halinde oku
                float[] buffer = new float[reader.WaveFormat.SampleRate * reader.WaveFormat.Channels]; 
                int read;

                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (reader.WaveFormat.Channels == 2)
                    {
                        // Stereo (Çift kanal) ise Mono'ya (Tek kanala) dönüştür
                        for (int i = 0; i < read; i += 2)
                        {
                            floats.Add((buffer[i] + buffer[i + 1]) / 2f);
                        }
                    }
                    else
                    {
                        // Zaten Mono ise doğrudan al
                        for (int i = 0; i < read; i++)
                        {
                            floats.Add(buffer[i]);
                        }
                    }
                }

                if (reader.WaveFormat.SampleRate != AppConstants.DefaultSampleRate)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[Uyarı] Ses dosyasının Sample Rate değeri {reader.WaveFormat.SampleRate}Hz.");
                    Console.WriteLine($"Sistem varsayılan olarak {AppConstants.DefaultSampleRate}Hz kullanıyor.");
                    Console.WriteLine("Sesin hızı/tonu biraz değişebilir, ancak şifreleme çalışacaktır.");
                    Console.ResetColor();
                }

                return floats.ToArray();
            }
        }
    }
}
