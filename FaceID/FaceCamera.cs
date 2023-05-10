﻿using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace AuthFaceIDModernUI.FaceID
{
    public class FaceCamera
    {
        private CascadeClassifier m_faceClassifier;
        private VideoCapture? m_capture;

        private bool m_isStartSaveFaces;

        public List<Mat> m_userFaces { get; set; }
        public List<Mat> m_last10UserFaces { get; set; }

        private System.Windows.Controls.Button? m_buttonControl;
        private System.Windows.Controls.ProgressBar? m_progressBarControl;
        private System.Windows.Controls.Image? m_faceViewControl;

        public FaceCamera(
            System.Windows.Controls.Image? imageControl = null, 
            System.Windows.Controls.Button? buttonControl = null,
            System.Windows.Controls.ProgressBar? progressBarControl = null
        )
        {
            if (imageControl != null) m_faceViewControl = imageControl;
            if (buttonControl != null) m_buttonControl = buttonControl;
            if (progressBarControl != null) m_progressBarControl = progressBarControl;

            m_isStartSaveFaces = false;

            m_faceClassifier = new CascadeClassifier(Config.HaarCascadePath);
            m_userFaces = new List<Mat>();
            m_last10UserFaces = new List<Mat>();
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

                        CvInvoke.Resize(faceImage, faceImage, new System.Drawing.Size(300, 300));
                        CvInvoke.Rectangle(currentFrame, faceOnFrame, new Bgr(System.Drawing.Color.DarkGreen).MCvScalar);

                        AddFaceToLatest(faceImage);

                        if (m_isStartSaveFaces)
                        {
                            if (m_userFaces.Count < Config.CountFacesToLearn)
                            {
                                m_userFaces.Add(faceImage);

                                m_buttonControl?.Dispatcher.Invoke(() =>
                                {
                                    m_buttonControl.Content = m_userFaces.Count.ToString() + " / " + Config.CountFacesToLearn;
                                });
                            }
                            else
                            {
                                m_buttonControl?.Dispatcher.Invoke(() =>
                                {
                                    m_buttonControl.Content = "Сохранить";
                                    m_buttonControl.IsEnabled = true;
                                    m_buttonControl.Background = new SolidColorBrush(Colors.Green);
                                });

                                m_progressBarControl?.Dispatcher.Invoke(() =>
                                {
                                    m_progressBarControl.IsIndeterminate = false;
                                });
                            }
                        }
                        else
                        {
                            m_buttonControl?.Dispatcher.Invoke(() =>
                            {
                                m_buttonControl.Content = "Cбор данных";
                                m_buttonControl.IsEnabled = true;
                            });
                        }
                    }
                }

                m_faceViewControl?.Dispatcher.Invoke(() =>
                {
                    m_faceViewControl.Source = BitmapSourceExtension.ToBitmapSource(currentFrame);
                });
            }
        }

        private void AddFaceToLatest(Mat faceImage)
        {
            if (m_last10UserFaces.Count < 30)
            {
                m_last10UserFaces.Add(faceImage);
            }
            else
            {
                m_last10UserFaces.RemoveAt(0);
                m_last10UserFaces.Add(faceImage);
            }
        }
    }
}
