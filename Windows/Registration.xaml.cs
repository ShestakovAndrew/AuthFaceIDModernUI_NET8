using AuthFaceIDModernUI.DataBase;
using ModernLoginWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AuthFaceIDModernUI.Windows
{
    public partial class Registration : Window
    {
        private UsersDataBase m_dataBase { get; set; }

        public Registration()
        {
            InitializeComponent();

            m_dataBase = new UsersDataBase();
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

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewLoginTextBox.Text.Length == 0)
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

            if (m_dataBase.IsLoginExistInDB(NewLoginTextBox.Text))
            {
                NewLoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (m_dataBase.AddNewUser(NewLoginTextBox.Text, NewPasswordBox.Password))
            {
                OpenNewLoginWindow();
            };
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