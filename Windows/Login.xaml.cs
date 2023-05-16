using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using AuthFaceIDModernUI.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ModernLoginWindow
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private async void ScanFaceButton_Click(object sender, RoutedEventArgs e)
        {
            UsersDataBase db = new();

            if (!db.IsLoginExistInDB(LoginTextBox.Text))
            {
                LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (!db.IsPasswordCorrectForUser(LoginTextBox.Text, PasswordBox.Password))
            {
                PasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            SetUserFaceID setUserFaceID = new();

            if (setUserFaceID.ShowDialog() == true)
            {
                if (await FaceIDTool.RecornizeIDOnImage(setUserFaceID.faceToCheck!) == db.GetIDByLogin(LoginTextBox.Text))
                {
                    PersonalArea personalArea = new(LoginTextBox.Text);
                    personalArea.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Ошибка распознования. Повторите попытку.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Лицо для распознания не выбрано.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new();
            registration.Show();
            Close();
        }

        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox.BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}