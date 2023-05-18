using NAudio.Wave;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Media;

namespace AuthFaceIDModernUI.API
{
    public static class VoiceAPIService
    {
        public static async Task<STTUserVoiceResponse?> STT(string fileNamePath)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, Config.HostASRAPI);
            
            request.Headers.Add("Authorization", $"Bearer {Config.OAuthTokenAudioFileRecognition}");
            request.Content = new StreamContent(File.OpenRead(fileNamePath));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/wave");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<STTUserVoiceResponse>(await response.Content.ReadAsStringAsync());
        }

        public static async Task TTS(string textToSpeech)
        {
            if (textToSpeech.Length == 0) return;
            string outFilePath = $"{Path.Join(Config.BaseDirectoryPath, $"VoiceID\\FromVKCloud\\{BitConverter.ToString(Encoding.Default.GetBytes(textToSpeech.ToLower()))}.mp3")}";

            if (!File.Exists(outFilePath))
            {
                using var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, $"{Config.HostTTSAPI}?encoder=mp3");

                request.Headers.Add("Authorization", $"Bearer {Config.OAuthTokenSpeechSynthesis}");
                request.Content = new StringContent(textToSpeech, null, "text/plain");

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                using var reader = new Mp3FileReader(await response.Content.ReadAsStreamAsync());

                WaveFileWriter.CreateWaveFile(outFilePath, reader);
            }

            SoundPlayer player = new() { SoundLocation = outFilePath };
            player.PlaySync();
        }
    }

    public class STTUserVoiceResponse
    {
        public class Result
        {
            public List<Text> texts { get; set; }
        }

        public class Text
        {
            public string text { get; set; }
            public double confidence { get; set; }
            public string punctuated_text { get; set; }
        }

        public string qid { get; set; }
        public Result result { get; set; }
    }
}
