using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Util;

namespace AuthFaceIDModernUI.DataBase
{
    public static class FacesRecognizerTool
    {
        public static void SaveRecognizerByLogin(string userLogin, List<Mat> faces)
        {
            string userFaceRecognizersPath = System.IO.Path.Join(Config.FaceRecognizersPath, userLogin.ToString());
            System.IO.Directory.CreateDirectory(userFaceRecognizersPath);

            EigenFaceRecognizer faceRecognition = new(faces.Count);

            List<Mat> facesGray = new();
            
            foreach (Mat face in faces)
            {
                CvInvoke.CvtColor(face, face, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray, 0);
                facesGray.Add(face);
            }

            faceRecognition.Train(  
                new VectorOfMat(facesGray.ToArray()),
                new VectorOfInt(Enumerable.Repeat(1, faces.Count).ToArray())
            );

            faceRecognition.Write(System.IO.Path.Join(userFaceRecognizersPath, "trainedData.yml"));
        }

        public static void DeleteRecognizerByLogin(string userLogin)
        {
            string userFaceRecognizersPath = System.IO.Path.Join(Config.FaceRecognizersPath, userLogin.ToString());

            if (System.IO.Directory.Exists(userFaceRecognizersPath))
            {
                System.IO.Directory.Delete(userFaceRecognizersPath, true);
            }
        }

        public static EigenFaceRecognizer GetRecognizerByLogin(string userLogin)
        {
            EigenFaceRecognizer faceRecognition = new();
            faceRecognition.Read(
                System.IO.Path.Join(System.IO.Path.Join(Config.FaceRecognizersPath, userLogin.ToString()), "trainedData.yml")
            );
            return faceRecognition;
        }
    }
}