using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using ModernLoginWindow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AuthFaceIDModernUI.Windows
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void OpenNewLoginWindow()
        {
            Login login = new();
            login.Show();
            Close();
        }

        private async void AddFaceButton_Click(object sender, RoutedEventArgs e)
        {
            UsersDataBase db = new();

            if (NewLoginTextBox.Text.Length == 0 || db.IsLoginExistInDB(NewLoginTextBox.Text))
            {
                NewLoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
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

            SetUserFaceID setUserFaceID = new();

            if (setUserFaceID.ShowDialog() == true)
            {
                if (
                    db.AddNewUser(NewLoginTextBox.Text, NewPasswordBox.Password) && 
                    await FaceIDTool.SetPersonID(setUserFaceID.faceToCheck!, db.GetIDByLogin(NewLoginTextBox.Text))
                    )
                {
                    OpenNewLoginWindow();
                }
                else
                {
                    MessageBox.Show("Ошибка добавления лица. Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Лицо для распознания не выбрано.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OpenNewLoginWindow();
        }

        private void NewLoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewLoginTextBox.BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            NewPasswordBox.BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void NewPasswordAgainBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            NewPasswordAgainBox.BorderBrush = new SolidColorBrush(Colors.Gray);
        }
    }
}