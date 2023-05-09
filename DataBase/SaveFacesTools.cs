using Emgu.CV;

namespace AuthFaceIDModernUI.DataBase
{
    public static class SaveFacesTools
    {
        public static void SaveFacesByLogin(string userLogin, List<Mat> faces)
        {
            string userFacesPath = System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), userLogin.ToString());
            System.IO.Directory.CreateDirectory(userFacesPath);

            for (int i = 0; i < faces.Count; i++) 
            {
                string imagePath = userFacesPath + @"\" + i.ToString() + ".png";
                Mat face = faces[i];
                face.Save(imagePath);
            }
        }

        public static void DeleteFacesByLogin(string userLogin)
        {
            string userFacesPath = System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), userLogin.ToString());
            System.IO.Directory.Delete(userFacesPath, true);
        }
    }
}