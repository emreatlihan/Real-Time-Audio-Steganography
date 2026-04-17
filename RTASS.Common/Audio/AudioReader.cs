using NAudio.Wave;
using RTASS.Common.Constants;

namespace RTASS.Common.Audio
{
    /// <summary>
    /// WAV dosyalarını okuma işlemleri
    /// </summary>
    public class AudioReader
    {
        public float[] ReadWavFile(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                if (reader.WaveFormat.SampleRate != AppConstants.DefaultSampleRate)
                {
                }


                var buffer = new float[reader.Length / 4];
                int read = reader.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }
    }
}
