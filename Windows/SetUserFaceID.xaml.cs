using System.IO;
using System.Windows.Interop;
using System.Windows;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;

namespace AuthFaceIDModernUI.Windows
{
    public partial class SetUserFaceID : Window
    {
        private string m_facePath = AppDomain.CurrentDomain.BaseDirectory + "/Faces/Faces.txt";

        private CascadeClassifier m_faceDetected;

        private EigenFaceRecognizer m_faceRecognizer;

        private Mat m_frame;

        private VideoCapture m_camera;

        private Image<Gray, byte> m_result;

        private Image<Gray, byte> m_trainedFace;

        private Mat m_grayFace;

        private List<Image<Gray, byte>> m_trainingImages = new List<Image<Gray, byte>>();

        private List<string> m_labels;

        private List<string> m_users;

        int m_count, m_numLabels, m_t;

        string m_name, m_names;

        public SetUserFaceID()
        {
            InitializeComponent();

            m_faceDetected = new CascadeClassifier("HaarCascadeFrontalface.xml");
            m_faceRecognizer = new EigenFaceRecognizer();

            m_camera = new();
            m_labels = new();
            m_users = new();

            try
            {
                string labelsInfo = File.ReadAllText(m_facePath);
                string[] labels = labelsInfo.Split(',');

                m_numLabels = Convert.ToInt16(labels[0]);
                m_count = m_numLabels;

                string faceLoad;

                for (int i = 1; i < m_numLabels + 1; i++)
                {
                    faceLoad = "face" + i + ".bmp";
                    m_trainingImages.Add(new Image<Gray, byte>(m_facePath));
                    m_labels.Add(labels[i]);
                }
            }
            catch
            {
                MessageBox.Show("Empty");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DoFaceImageButton_Click(object sender, RoutedEventArgs e)
        {
            m_camera = new VideoCapture();
            m_camera.QueryFrame();
            ComponentDispatcher.ThreadIdle += new EventHandler(FrameProcedure);
        }

        private void FrameProcedure(object? sender, EventArgs e)
        {
            m_users.Add("");
             
            m_frame = m_camera.QueryFrame();

            CvInvoke.Resize(m_frame, m_frame, new System.Drawing.Size(600, 600));
            CvInvoke.CvtColor(m_frame, m_grayFace, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

            var faces = m_faceDetected.DetectMultiScale(m_grayFace);

            foreach (var face in faces)
            {
                var faceImage = new Mat(m_grayFace, face);
                CvInvoke.Resize(faceImage, faceImage, new System.Drawing.Size(150, 150));

                var prediction = m_faceRecognizer.Predict(faceImage);

                if (prediction.Label != -1)
                {
                    var name = "name";
                    CvInvoke.PutText(m_frame, name, new System.Drawing.Point(face.X, face.Y - 10), Emgu.CV.CvEnum.FontFace.HersheyPlain, 1.0, new MCvScalar(0, 0, 255));
                }

                CvInvoke.Rectangle(m_frame, face, new MCvScalar(0, 0, 255));
            }
            
            /*m_grayFace = m_frame.Convert<Gray, Byte>();

            //MCvAvgComp[][] facesDetectedNow = m_grayFace.DetectHaarCascade(m_faceDetected, 1.2, 10, Emgu.CV.CvEnum.HaarDetectionType.DoCannyPruning, new System.Drawing.Size(20, 20));

            //m_faceDetected.DetectMultiScale(m_frame, 1.2, 10, new System.Drawing.Size(20, 20));


            //foreach (MCvAvgComp face in facesDetectedNow[0])
            //{
            //    m_result = m_frame.Copy(face.Rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
            //    m_frame.Draw(face.Rect, new Bgr(System.Drawing.Color.Green), 3);

            //    if (m_trainingImages.ToArray().Length != 0)
            //    {
            //        MCvTermCriteria termCriterias = new(m_count, 0.001);
            //        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(m_trainingImages.ToArray(), m_labels.ToArray(), 1500, ref termCriterias);
            //        m_name = recognizer.Recognize(m_result).ToString();
            //    }
            //    m_users.Add("");
            }*/

            CameraImages.Source = BitmapSourceExtension.ToBitmapSource(m_frame);
            //m_names = "";
            //m_users.Clear();

        }

        private void ReDoFaceImageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveFaceImageButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}