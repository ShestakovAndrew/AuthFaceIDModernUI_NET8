﻿using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;

using Emgu.CV;
using Emgu.CV.Structure;
using AuthFaceIDModernUI.DataBase;

namespace AuthFaceIDModernUI.Windows
{
    public partial class SetUserFaceID : Window
    {
        private CascadeClassifier m_faceDetected;

        private VideoCapture? m_capture;

        private string m_userLogin;

        private Mat? m_userFace;

        private UsersDataBase m_dataBase { get; set; }

        public SetUserFaceID(string userLogin)
        {
            InitializeComponent();

            try
            {
                m_capture = new VideoCapture();
                m_capture.ImageGrabbed += ProcessFrame;
                m_capture.Start();
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            m_faceDetected = new CascadeClassifier(@"D:\CourseWork\FaceID\haarcascade_frontalface_default.xml");
            m_dataBase = new UsersDataBase();
            m_userLogin = userLogin;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
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
                    var faceOnFrame = m_faceDetected.DetectMultiScale(currentFrame).FirstOrDefault();

                    if (!faceOnFrame.IsEmpty) 
                    {
                        var faceImage = new Mat(currentFrame, faceOnFrame);
                        CvInvoke.Resize(faceImage, faceImage, new System.Drawing.Size(150, 150));

                        CvInvoke.Rectangle(currentFrame, faceOnFrame, new Bgr(System.Drawing.Color.DarkGreen).MCvScalar);
                        CvInvoke.PutText(
                            currentFrame,
                            m_userLogin,
                            new System.Drawing.Point(faceOnFrame.X, faceOnFrame.Y - 10),
                            Emgu.CV.CvEnum.FontFace.HersheyComplex,
                            1.0,
                            new Bgr(System.Drawing.Color.DarkGreen).MCvScalar
                        );

                        m_userFace = faceImage;
                    }

                    Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        new Action(() => CameraImages.Source = BitmapSourceExtension.ToBitmapSource(currentFrame))
                    );
                }
            }
        }

        private void SaveFaceImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_userFace != null) 
            {
                SaveUserFace();
                OpenPersonalArea();
            }
            else
            {
                MessageBox.Show("Убедитесь, чтобы ваше лицо было определено и находилось в рамке.");
            }
        }

        private void SaveUserFace()
        {
            if (m_dataBase.SaveFaceByLogin(m_userFace!, m_userLogin))
            {

            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OpenPersonalArea();
        }

        private void OpenPersonalArea()
        {
            PersonalArea personalArea = new(m_userLogin);
            personalArea.Show();
            Close();
        }
    }
}