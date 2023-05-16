using Emgu.CV;

namespace AuthFaceIDModernUI.DataBase
{
    public static class DirectoryTool
    {
        public static string SaveTempImage(Mat image)
        {
            string tempImagePath = System.IO.Path.Join(Config.TempFolderPath, System.IO.Path.GetRandomFileName() + ".png");
            image.Save(tempImagePath);
            return tempImagePath;
        }

        public static void DeleteTempImage(string filePath)
        {
            System.IO.File.Delete(filePath);
        }
    }
}
