using System.Windows;
using System.Windows.Input;

using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;

namespace AuthFaceIDModernUI.Windows
{
    public partial class SetUserFaceID : Window
    {
        private FaceCamera m_faceCamera;
        private string m_userLogin;
        private bool m_isFacesToChange;

        public SetUserFaceID(string userLogin, bool isFacesToChange)
        {
            InitializeComponent();

            m_userLogin = userLogin;
            m_isFacesToChange = isFacesToChange;

            m_faceCamera = new FaceCamera(
                CameraImages, 
                StartGetFacesButton,
                FaceProgressBar
            );

            m_faceCamera.TurnOn();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void StartGetFacesButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartGetFacesButton.Content.ToString() == "Сохранить")
            {
                UsersDataBase db = new();
                
                if (db.IsExistFaceIDByLogin(m_userLogin))
                {
                    SaveFacesTools.DeleteFacesByLogin(m_userLogin);
                    db.DeleteFacesByLogin(m_userLogin);
                }

                SaveFacesTools.SaveFacesByLogin(m_userLogin, m_faceCamera.m_userFaces);
                db.SaveFacesByLogin(m_userLogin);

                m_faceCamera.TurnOff();
                Close();
            }
            else
            {
                StartGetFacesButton.IsEnabled = false;
                m_faceCamera.StartSaveFaces();
                FaceProgressBar.IsIndeterminate = true;
            }    
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            m_faceCamera.TurnOff();

            if (!m_isFacesToChange)
            {
                SaveFacesTools.DeleteFacesByLogin(m_userLogin);
            }

            Close();
        }
    }
}