using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;

namespace AuthFaceIDModernUI.Windows
{
    public partial class SetUserFaceID : Window
    {
        private CascadeClassifier m_faceDetected;

        private EigenFaceRecognizer m_faceRecognizer;

        private VideoCapture? m_capture;

        private Mat m_grayFrame;

        private Mat m_frame;

        public SetUserFaceID()
        {
            InitializeComponent();

            try
            {
                m_capture = new VideoCapture();
                m_capture.ImageGrabbed += ProcessFrame;
                m_capture.Start();
            }
            catch(NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            m_faceDetected = new CascadeClassifier(@"D:\CourseWork\FaceID\haarcascade_frontalface_default.xml");
            m_faceRecognizer = new EigenFaceRecognizer();
            m_grayFrame = new Mat();
            m_frame = new Mat();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (m_capture != null && m_capture.Ptr != IntPtr.Zero)
            {
                m_capture.Retrieve(m_frame, 0);
                CvInvoke.CvtColor(m_frame, m_grayFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                var faces = m_faceDetected.DetectMultiScale(m_grayFrame);

                foreach (var face in faces)
                {
                    var faceImage = new Mat(m_grayFrame, face);

                    CvInvoke.Resize(faceImage, faceImage, new System.Drawing.Size(100, 100));
                    CvInvoke.Rectangle(m_frame, face, new MCvScalar(0, 0, 255));

                    /*
                    var prediction = m_faceRecognizer.Predict(faceImage);

                    if (prediction.Label != -1)
                    {
                        var name = "name";
                        CvInvoke.PutText(m_frame, name, new System.Drawing.Point(face.X, face.Y - 10), Emgu.CV.CvEnum.FontFace.HersheyPlain, 1.0, new MCvScalar(0, 0, 255));
                    }
                    */
                }

                Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new Action(() => CameraImages.Source = BitmapSourceExtension.ToBitmapSource(m_frame))
                );
            }
        }

        private void SaveFaceImageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}