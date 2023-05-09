using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace AuthFaceIDModernUI.FaceID
{
    public class FaceCamera
    {
        private CascadeClassifier m_faceDetected;
        private VideoCapture? m_capture;
        private System.Windows.Controls.Image m_imageControl;

        private bool m_isStartSaveFaces;
        private int m_countFacesToLearn;
        private System.Windows.Controls.Button m_buttonControl;
        private System.Windows.Controls.ProgressBar m_progressBarControl;
        public List<Mat> m_userFaces { get; set; }

        public FaceCamera(
            System.Windows.Controls.Image imageControl, 
            System.Windows.Controls.Button startGetFacesButton,
            System.Windows.Controls.ProgressBar progressBarControl,
            int countFacesToLearn
        )
        {
            m_imageControl = imageControl;
            m_buttonControl = startGetFacesButton;
            m_countFacesToLearn = countFacesToLearn;
            m_progressBarControl = progressBarControl;
            m_isStartSaveFaces = false;

            m_faceDetected = new CascadeClassifier(@"D:\CourseWork\FaceID\haarcascade_frontalface_default.xml");
            m_userFaces = new List<Mat>();
        }

        public void TurnOn()
        {
            CvInvoke.UseOpenCL = false;

            try
            {
                m_capture = new VideoCapture();
                m_capture.ImageGrabbed += ProcessFrame!;
                m_capture.Start();
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        public void TurnOff()
        {
            m_capture?.Stop();
            m_capture?.Dispose();
        }

        public void StartSaveFaces()
        {
            m_userFaces.Clear();
            m_isStartSaveFaces = true;
        }

        private Mat GetCurrentFrame()
        {
            Mat? newFrame = new();

            if (m_capture != null && m_capture.Ptr != IntPtr.Zero)
            {
                m_capture.Retrieve(newFrame, 0);
            }

            return newFrame;
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (m_capture != null && m_capture.Ptr != IntPtr.Zero)
            {
                Mat? currentFrame = GetCurrentFrame();

                if (currentFrame != null)
                {
                    Rectangle faceOnFrame = m_faceDetected.DetectMultiScale(currentFrame).FirstOrDefault();

                    if (!faceOnFrame.IsEmpty)
                    {
                        Mat faceImage = new(currentFrame, faceOnFrame);

                        CvInvoke.Resize(faceImage, faceImage, new System.Drawing.Size(150, 150));
                        CvInvoke.Rectangle(currentFrame, faceOnFrame, new Bgr(System.Drawing.Color.DarkGreen).MCvScalar);

                        if (m_isStartSaveFaces)
                        {
                            if (m_userFaces.Count < m_countFacesToLearn)
                            {
                                m_userFaces.Add(faceImage);

                                m_buttonControl.Dispatcher.Invoke(() =>
                                {
                                    m_buttonControl.Content = m_userFaces.Count.ToString() + " / " + m_countFacesToLearn;
                                });
                            }
                            else
                            {
                                m_buttonControl.Dispatcher.Invoke(() =>
                                {
                                    m_buttonControl.Content = "Сохранить";
                                    m_buttonControl.IsEnabled = true;
                                    m_buttonControl.Background = new SolidColorBrush(Colors.Green);
                                });

                                m_progressBarControl.Dispatcher.Invoke(() =>
                                {
                                    m_progressBarControl.IsIndeterminate = false;
                                });
                            }
                        }
                        else
                        {
                            m_buttonControl.Dispatcher.Invoke(() =>
                            {
                                m_buttonControl.Content = "Cбор данных";
                                m_buttonControl.IsEnabled = true;
                            });
                        }
                    }
                }

                m_imageControl.Dispatcher.Invoke(() =>
                {
                    m_imageControl.Source = BitmapSourceExtension.ToBitmapSource(currentFrame);
                });
            }
        }
    }
}
