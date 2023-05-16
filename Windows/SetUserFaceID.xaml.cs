using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using Emgu.CV;

namespace AuthFaceIDModernUI.Windows
{
    public partial class SetUserFaceID
    {
        private FaceCamera m_faceCamera;
        private bool m_isFotoSet;
        public Mat? faceToCheck;

        public SetUserFaceID()
        {
            InitializeComponent();

            m_isFotoSet = false;
            m_faceCamera = new FaceCamera(CameraImages);
            m_faceCamera.TurnOn();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void DoFotoButton_Click(object sender, RoutedEventArgs e)
        {
            if (DoFotoButton.Content.ToString() == "Сделать фото")
            {
                m_faceCamera.TurnOff();
                faceToCheck = m_faceCamera.GetUserFace();

                CameraImages.Source = BitmapSourceExtension.ToBitmapSource(faceToCheck);

                m_isFotoSet = true;
                ChangeButtons();
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_isFotoSet)
            {
                m_isFotoSet = false;
                ChangeButtons();
                m_faceCamera.TurnOn();
            }
            else
            {
                DialogResult = false;
                Close();
            }
        }

        private void ChangeButtons()
        {
            DoFotoButton.Content = m_isFotoSet ? "Сохранить лицо" : "Сделать фото";
            BackButton.Content = m_isFotoSet ? "Назад" : "Закрыть";
        }
    }
}