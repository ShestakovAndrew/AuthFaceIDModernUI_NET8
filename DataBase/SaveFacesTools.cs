using Emgu.CV;

namespace AuthFaceIDModernUI.DataBase
{
    public static class SaveFacesTools
    {
        public static void SaveFacesByLogin(string userLogin, List<Mat> faces)
        {
            string userFacesPath = System.IO.Path.Join(Config.UserPhotosPath, userLogin.ToString());
            System.IO.Directory.CreateDirectory(userFacesPath);

            for (int i = 0; i < faces.Count; i++) 
            {
                Mat face = faces[i];
                face.Save(userFacesPath + @"\" + i.ToString() + ".png");
            }
        }

        public static void DeleteFacesByLogin(string userLogin)
        {
            string userFacesPath = System.IO.Path.Join(Config.UserPhotosPath, userLogin.ToString());

            if (System.IO.Directory.Exists(userFacesPath))
            {
                System.IO.Directory.Delete(userFacesPath, true);
            }
        }
    }
}