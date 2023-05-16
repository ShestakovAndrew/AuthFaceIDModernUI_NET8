namespace AuthFaceIDModernUI
{
    public static class Config
    {
        private static string DataBaseName = "users.db";
        private static string TempFolderName = "Temp";

        public static string DataBasePath = System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DataBaseName);
        public static string TempFolderPath = System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), TempFolderName);
        public static string HaarCascadePath = System.IO.Path.Join(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\FaceID\\haarcascade_frontalface_default.xml");

        public static string HostVisionAPI = "https://smarty.mail.ru/api/v1/persons/";
        public static string OAuthProvider = "mcs";
        public static string OAuthToken = "your token"; //set your token from VK Cloud
    }
}
