namespace AuthFaceIDModernUI
{
    public static class Config
    {
        private static string DataBaseName = "users.db";
        private static string TempFolderName = "Temp";

        public static string DataBasePath = System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DataBaseName);
        public static string TempFolderPath = System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), TempFolderName);
        public static string HaarCascadePath = System.IO.Path.Join(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\FaceID\\haarcascade_frontalface_default.xml");
        public static string BaseDirectoryPath = System.IO.Path.Join(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..");

        public static string HostVisionAPI = "https://smarty.mail.ru/api/v1/persons/";
        public static string HostASRAPI = "https://voice.mcs.mail.ru/asr";
        public static string HostTTSAPI = "https://voice.mcs.mail.ru/tts";

        public static string OAuthProvider = "mcs";

        //set your tokens from VK Cloud
        public static string OAuthTokenFaceID = "LGeEtP6piyaeBVDe1EdJGpzSGJ24XPGP52t4FmrHLuk3Sogtx";
        public static string OAuthTokenSpeechSynthesis = "2QaHcfs7WTbWsGuZxm7jJaCzMLNzRnrxTsUgVLeHHpZKrn6xoR";
        public static string OAuthTokenAudioFileRecognition = "CCiPVUB3Rp2c7v2ow2aszosNF1pcaUh86ThieQjaz55rH9pwC";
    }
}
