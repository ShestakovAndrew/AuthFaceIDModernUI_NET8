using ModernLoginWindow;
using System.Windows;
using System.Windows.Input;

namespace AuthFaceIDModernUI.Windows
{
    public partial class PersonalArea : Window
    {
        private string m_userLogin { get; set; }

        private bool m_isFaceIDEnable { get; set; }

        public PersonalArea(string userLogin)
        {
            InitializeComponent();

            m_userLogin = userLogin;
            TitleTextBlock.Text += m_userLogin;
            m_isFaceIDEnable = false;

            UpdateFaceIDSettings();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void FaceIDToggleButton_Click(object sender, RoutedEventArgs e)
        {
            m_isFaceIDEnable = !m_isFaceIDEnable;

            UpdateFaceIDSettings();
        }

        private void ChangeFaceIDButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFaceIDButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateFaceIDSettings()
        {
            ChangeFaceIDButton.IsEnabled = m_isFaceIDEnable;
            DeleteFaceIDButton.IsEnabled = m_isFaceIDEnable;
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