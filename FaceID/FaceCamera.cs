using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Windows;

namespace AuthFaceIDModernUI.FaceID
{
    public class FaceCamera
    {
        private VideoCapture? m_capture;
        private CascadeClassifier m_faceClassifier;
        private System.Windows.Controls.Image? m_faceViewControl;
        private Mat m_userFace;

        public FaceCamera(
            System.Windows.Controls.Image imageControl
        )
        {
            if (imageControl != null) m_faceViewControl = imageControl;

            m_faceClassifier = new CascadeClassifier(Config.HaarCascadePath);

            m_userFace = new Mat();
        }

        public Mat GetUserFace()
        {
            return m_userFace;
        }

        public void TurnOff()
        {
            m_capture?.Stop();
            m_capture?.Dispose();
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

        private Mat? GetCurrentFrame()
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
                    Rectangle faceOnFrame = m_faceClassifier.DetectMultiScale(currentFrame).FirstOrDefault();

                    if (!faceOnFrame.IsEmpty)
                    {
                        Mat faceImage = new(currentFrame, faceOnFrame);

                        m_userFace = faceImage;

                        CvInvoke.Resize(faceImage, faceImage, new System.Drawing.Size(200, 200));
                        CvInvoke.Rectangle(currentFrame, faceOnFrame, new Bgr(System.Drawing.Color.DarkGreen).MCvScalar);
                    }
                }

                m_faceViewControl?.Dispatcher.Invoke(() =>
                {
                    m_faceViewControl.Source = BitmapSourceExtension.ToBitmapSource(currentFrame);
                });
            }
        }
    }
}