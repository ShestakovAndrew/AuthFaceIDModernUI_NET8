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

            EigenFaceRecognizer faceRecognition = new EigenFaceRecognizer(faces.Count, double.PositiveInfinity);

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
            /*
            Mat img5 = CvInvoke.Imread("C:\\Users\\Andrew\\Desktop\\andrew3.jpg");
            Mat img4 = CvInvoke.Imread("C:\\Users\\Andrew\\Desktop\\tan9.jpg");
            Mat img3 = CvInvoke.Imread("C:\\Users\\Andrew\\Desktop\\andrew2.jpg");
            Mat img2 = CvInvoke.Imread("C:\\Users\\Andrew\\Desktop\\andrew.jpg");
            Mat img1 = facesGray[30];

            CvInvoke.CvtColor(img2, img2, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray, 0);
            CvInvoke.CvtColor(img3, img3, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray, 0);
            CvInvoke.CvtColor(img4, img4, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray, 0);
            CvInvoke.CvtColor(img5, img5, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray, 0);
            CvInvoke.Resize(img5, img5, new System.Drawing.Size(150, 150), 2);


            FaceRecognizer.PredictionResult predictionResult1 = faceRecognition.Predict(img1);
            FaceRecognizer.PredictionResult predictionResult2 = faceRecognition.Predict(img2);
            FaceRecognizer.PredictionResult predictionResult3 = faceRecognition.Predict(img3);
            FaceRecognizer.PredictionResult predictionResult4 = faceRecognition.Predict(img4);
            FaceRecognizer.PredictionResult predictionResult5 = faceRecognition.Predict(img5);
            */
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