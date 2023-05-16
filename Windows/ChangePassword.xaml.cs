using AuthFaceIDModernUI.DataBase;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AuthFaceIDModernUI.Windows
{
    public partial class ChangePassword : Window
    {
        private string m_userLogin { get; set; }

        private UsersDataBase m_dataBase { get; set; }

        public ChangePassword(string userLogin)
        {
            InitializeComponent();

            m_userLogin = userLogin;
            m_dataBase = new UsersDataBase();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void OpenNewPersonalAreaWindow()
        {
            PersonalArea personalArea = new(m_userLogin);
            personalArea.Show();
            Close();
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (OldPasswordBox.Password.Length == 0)
            {
                OldPasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (NewPasswordBox.Password.Length == 0)
            {
                NewPasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (NewPasswordAgainBox.Password.Length == 0)
            {
                NewPasswordAgainBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (NewPasswordBox.Password != NewPasswordAgainBox.Password)
            {
                NewPasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                NewPasswordAgainBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (!m_dataBase.IsPasswordCorrectForUser(m_userLogin, OldPasswordBox.Password))
            {
                OldPasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (m_dataBase.ChangeUserPassword(m_userLogin, NewPasswordBox.Password))
            {
                OpenNewPersonalAreaWindow();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OpenNewPersonalAreaWindow();
        }
    }
}