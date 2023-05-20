using AuthFaceIDModernUI.API;
using NAudio.Wave;
using System.Windows;

namespace AuthFaceIDModernUI.VoiceID
{
    public static class VoiceTool
    {
        public static async Task TextToSpeech(string text)
        {
            try
            {
                await VoiceAPIService.TTS(text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static async Task<string> SpeechToText(string mp3FilePath)
        {
            try
            {
                STTUserVoiceResponse? response = await VoiceAPIService.STT(mp3FilePath);
                if ((response != null) && (response.result.texts.Count >= 1))
                {
                    return response.result.texts[0].text.ToLower();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            await TextToSpeech("Текст в айдиофайле не распознан");
            return "";
        }

        public static string ConvertStereoToMonoWav(RecordVoice recordVoice, string filePath)
        {
            var waveFileReader = new WaveFileReader(filePath);
            var outFormat = new WaveFormat(waveFileReader.WaveFormat.SampleRate, 1);
            var resampler = new MediaFoundationResampler(waveFileReader, outFormat);

            recordVoice.UpdateFileName();
            filePath = recordVoice.m_outFilePath;
            WaveFileWriter.CreateWaveFile(filePath, resampler);
            return filePath;
        }
    }
}
