using NAudio.Wave;
using RTASS.Common.Constants;
using RTASS.Common.Helpers;

namespace RTASS.Common.Audio
{
    /// <summary>
    /// WAV dosyalarını yazma işlemleri
    /// </summary>
    public class AudioWriter
    {
        public void WriteWavFile(string filePath, float[] audioData, int sampleRate = 44100, int channels = 1)
        {
            var format = new WaveFormat(sampleRate, 16, channels);
            using (var writer = new WaveFileWriter(filePath, format))
            {
                byte[] byteData = AudioHelper.ToByteArray(audioData);
                writer.Write(byteData, 0, byteData.Length);
            }
        }
    }
}
