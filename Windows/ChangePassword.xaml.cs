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
    public partial class ChangePassword : Window
    {
        public ChangePassword(string userLogin)
        {
            InitializeComponent();

            UserLogin = userLogin;
        }

        private string UserLogin { get; set; }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void OpenNewPersonalAreaWindow()
        {
            PersonalArea personalArea = new(UserLogin);
            personalArea.Show();
            Close();
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            using var db = new UsersContext();

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

            if (!db.IsPasswordCorrectForUser(UserLogin, OldPasswordBox.Password))
            {
                OldPasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }
            else
            {
                if (db.ChangeUserPassword(UserLogin, NewPasswordBox.Password))
                {
                    OpenNewPersonalAreaWindow();
                };
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OpenNewPersonalAreaWindow();
        }
    }
}