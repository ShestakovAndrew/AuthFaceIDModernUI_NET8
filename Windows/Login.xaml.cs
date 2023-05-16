using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.Windows;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Face;
using AuthFaceIDModernUI.FaceID;
using AuthFaceIDModernUI.API;
using System.Windows.Forms;

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

        private void LoginToPersonalArea(string login)
        {
            PersonalArea personalArea = new(login);
            personalArea.Show();
            Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
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

            LoginToPersonalArea(LoginTextBox.Text);
        }

        private async void FaceIDButton_Click(object sender, RoutedEventArgs e)
        {
            UsersDataBase db = new();

            if (LoginTextBox.Text.Length == 0 || !db.IsLoginExistInDB(LoginTextBox.Text))
            {
                LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }
            
            if (!db.IsExistFaceIDByLogin(LoginTextBox.Text))
            {
                FaceIDButton.BorderBrush = new SolidColorBrush(Colors.Red);
                System.Windows.MessageBox.Show("Для данного пользователя не настроен FaceID.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                SetUserFaceID setUserFaceID = new();

                if (setUserFaceID.ShowDialog() == true)
                {
                    if (await FaceIDTool.RecornizeIDOnImage(setUserFaceID.faceToCheck!) == db.GetIDByLogin(LoginTextBox.Text))
                    {
                        LoginToPersonalArea(LoginTextBox.Text);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Ошибка распознования. Повторите попытку.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Лицо для распознания не выбрано.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new();
            registration.Show();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Gray);
            FaceIDButton.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox.BorderBrush = new SolidColorBrush(Colors.Gray);
        }
    }
}