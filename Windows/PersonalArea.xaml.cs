using AuthFaceIDModernUI.DataBase;
using ModernLoginWindow;
using System.Windows;
using System.Windows.Input;

namespace AuthFaceIDModernUI.Windows
{
    public partial class PersonalArea : Window
    {
        private bool m_isWindowLoading {  get; set; }

        private string m_userLogin { get; set; }

        public PersonalArea(string userLogin)
        {
            InitializeComponent();

            m_isWindowLoading = true;

            m_userLogin = userLogin;
            TitleTextBlock.Text += userLogin;


            UsersDataBase db = new();
            FaceIDToggleButton.IsChecked = db.IsExistFaceIDByLogin(m_userLogin);
            if (FaceIDToggleButton.IsChecked.Value)
            {
                FaceIDButtonsEnable();
            }
            else
            {
                FaceIDButtonsDisable();
            }

            m_isWindowLoading = false;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void ChangeFaceIDButton_Click(object sender, RoutedEventArgs e)
        {
            SetUserFaceID userFaceID = new(m_userLogin);
            userFaceID.ShowDialog();
        }

        private void DeleteFaceIDButton_Click(object sender, RoutedEventArgs e)
        {
            FaceIDToggleButton.IsChecked = false;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changePassword = new(m_userLogin);
            changePassword.Show();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Login login = new();
            login.Show();
            Close();
        }

        private void FaceIDToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!m_isWindowLoading)
            {
                SetUserFaceID userFaceID = new SetUserFaceID(m_userLogin);
                userFaceID.ShowDialog();
            }

            UsersDataBase db = new();
            if (db.IsExistFaceIDByLogin(m_userLogin))
            {
                FaceIDButtonsEnable();
            }
            else
            {
                FaceIDToggleButton.IsChecked = false;
            }
        }

        private void FaceIDToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            DeleteFaceID();
        }

        private void DeleteFaceID()
        {
            UsersDataBase db = new();
            if (db.DeleteFaceIDByLogin(m_userLogin))
            {
                FaceIDButtonsDisable();
            }
        }

        private void FaceIDButtonsDisable()
        {
            FaceIDToggleButton.IsChecked = false;
            ChangeFaceIDButton.IsEnabled = false;
            DeleteFaceIDButton.IsEnabled = false;
        }

        private void FaceIDButtonsEnable()
        {
            ChangeFaceIDButton.IsEnabled = true;
            DeleteFaceIDButton.IsEnabled = true;
        }
    }
}