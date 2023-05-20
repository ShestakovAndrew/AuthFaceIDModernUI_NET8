using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using ModernLoginWindow;
using System.Windows;
using System.Windows.Input;

namespace AuthFaceIDModernUI.Windows
{
    public partial class PersonalArea : Window
    {
        private bool m_isWindowLoading { get; set; }
        private string m_userLogin { get; set; }

        public PersonalArea(string userLogin)
        {
            InitializeComponent();

            m_isWindowLoading = true;

            m_userLogin = userLogin;
            TitleTextBlock.Text += userLogin;

            m_isWindowLoading = false;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
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
    }
}