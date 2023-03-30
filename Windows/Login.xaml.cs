using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using AuthFaceIDModernUI.Windows;

namespace ModernLoginWindow
{
    public partial class Login : Window
    {
        private UsersDataBase m_dataBase { get; set; }

        public Login()
        {
            InitializeComponent();

            m_dataBase = new UsersDataBase();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void LoginToPersonalArea(string login)
        {
            PersonalArea personalArea = new(login);
            personalArea.Show();
            Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!m_dataBase.IsLoginExistInDB(LoginTextBox.Text))
            {
                LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (!m_dataBase.IsPasswordCorrectForUser(LoginTextBox.Text, PasswordBox.Password))
            {
                PasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            LoginToPersonalArea(LoginTextBox.Text);
        }

        private void FaceIDButton_Click(object sender, RoutedEventArgs e)
        {
            if (!m_dataBase.IsLoginExistInDB(LoginTextBox.Text))
            {
                LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (!FaceRecognitionTools.FaceExistByLogin(LoginTextBox.Text))
            {
                PasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            LoginToPersonalArea(LoginTextBox.Text);
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new();
            registration.Show();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox.BorderBrush = new SolidColorBrush(Colors.Gray);
        }
    }
}