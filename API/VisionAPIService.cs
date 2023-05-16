using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace AuthFaceIDModernUI.API
{
    public enum Method
    {
        set,
        recognize,
        delete,
        truncate,
    }

    public static class VisionAPIService
    {
        public static async Task<DeleteUserFaceResponse?> Delete(int personID)
        {
            using var httpClient = new HttpClient();

            var multipartContent = new MultipartFormDataContent
            {
                { new StringContent($"{{ \"space\": \"1\", \"images\": [ {{ \"name\": \"file\", \"person_id\": {personID} }} ] }}"), "meta" }
            };

            string responseString = await (await httpClient.PostAsync(
                $"{Config.HostVisionAPI}{Method.delete}?oauth_token={Config.OAuthToken}&oauth_provider={Config.OAuthProvider}",
                multipartContent
            )).Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DeleteUserFaceResponse>(responseString);
        }

        public static async Task<RecognizeUserFaceResponse?> Recognize(string fileNamePath)
        {
            using var httpClient = new HttpClient();

            var file = new ByteArrayContent(File.ReadAllBytes(fileNamePath));
            file.Headers.Add("Content-Type", "image/jpeg");

            var multipartContent = new MultipartFormDataContent
                {
                    { file, "file", Path.GetFileName(fileNamePath) },
                    { new StringContent($"{{ \"space\": \"1\", \"create_new\": false, \"images\": [ {{ \"name\": \"file\" }} ] }}"), "meta" }
                };

            string responseString = await (await httpClient.PostAsync(
                $"{Config.HostVisionAPI}{Method.recognize}?oauth_token={Config.OAuthToken}&oauth_provider={Config.OAuthProvider}",
                multipartContent
            )).Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<RecognizeUserFaceResponse>(responseString);
        }

        public static async Task<SetUserFaceResponse?> Set(string fileNamePath, int personID)
        {
            using var httpClient = new HttpClient();

            var file = new ByteArrayContent(File.ReadAllBytes(fileNamePath));
            file.Headers.Add("Content-Type", "image/jpeg");

            var multipartContent = new MultipartFormDataContent
            {
                { file, "file", Path.GetFileName(fileNamePath) },
                { new StringContent($"{{ \"space\": \"1\", \"images\": [ {{ \"name\": \"file\", \"person_id\": {personID} }} ] }}"), "meta" }
            };

            string responseString = await (await httpClient.PostAsync(
                $"{Config.HostVisionAPI}{Method.set}?oauth_token={Config.OAuthToken}&oauth_provider={Config.OAuthProvider}",
                multipartContent
            )).Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SetUserFaceResponse>(responseString);
        }
        public static async Task<TruncateUserFaceResponse?> Truncate()
        {
            using var httpClient = new HttpClient();

            var multipartContent = new MultipartFormDataContent
            {
                { new StringContent("{ \"space\": \"1\" }"), "meta" }
            };

            string responseString = await (await httpClient.PostAsync(
                $"{Config.HostVisionAPI}{Method.truncate}?oauth_token={Config.OAuthToken}&oauth_provider={Config.OAuthProvider}",
                multipartContent
            )).Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TruncateUserFaceResponse>(responseString);
        }
    }

    public class DeleteUserFaceResponse
    {
        public Body body { get; set; } = new Body();

        public bool htmlencoded { get; set; }

        public int last_modified { get; set; }

        public int status { get; set; }

        public class Body
        {
            public List<Object> objects { get; set; } = new List<Object>();
        }

        public class Object
        {
            public string name { get; set; } = string.Empty;
            public int status { get; set; }
        }
    }

    public class RecognizeUserFaceResponse
    {
        public Body body { get; set; } = new Body();

        public bool htmlencoded { get; set; }

        public int last_modified { get; set; }

        public int status { get; set; }

        public class Body
        {
            public List<Object> objects { get; set; } = new List<Object>();
        }

        public class Object
        {
            public string name { get; set; } = string.Empty;
            public List<Person> persons { get; set; } = new List<Person>();
            public int status { get; set; }
        }

        public class Person
        {
            public int age { get; set; }
            public double arousal { get; set; }
            public double awesomeness { get; set; }
            public double confidence { get; set; }
            public List<int> coord { get; set; } = new List<int>();
            public string emotion { get; set; } = string.Empty;
            public double frontality { get; set; }
            public string sex { get; set; } = string.Empty;
            public double similarity { get; set; }
            public string tag { get; set; } = string.Empty;
            public double valence { get; set; }
            public double visibility { get; set; }
        }
    }

    public class SetUserFaceResponse
    {
        public Body body { get; set; } = new Body();

        public bool htmlencoded { get; set; }

        public int last_modified { get; set; }

        public int status { get; set; }

        public class Body
        {
            public List<Object>? objects { get; set; }
        }

        public class Object
        {
            public string name { get; set; } = string.Empty;
            public int status { get; set; }
        }
    }

    public class TruncateUserFaceResponse
    {
        public Body? body { get; set; }

        public bool htmlencoded { get; set; }

        public int last_modified { get; set; }

        public int status { get; set; }

        public class Body
        {
        }
    }
}
