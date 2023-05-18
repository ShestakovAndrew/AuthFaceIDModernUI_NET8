using AuthFaceIDModernUI.API;
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
                    return response.result.texts[0].punctuated_text;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return "Текст не распознан.";
        }
    }
}