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
    /// <summary>
    /// Логика взаимодействия для PersonalArea.xaml
    /// </summary>
    public partial class PersonalArea : Window
    {

        public PersonalArea(string userLogin)
        {
            InitializeComponent();

            UserLogin = userLogin;
            TitleTextBlock.Text += UserLogin;
            IsFaceIDEnable = false;

            UpdateFaceIDSettings();
        }

        private string UserLogin { get; set; }

        private bool IsFaceIDEnable { get; set; }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void FaceIDToggleButton_Click(object sender, RoutedEventArgs e)
        {
            IsFaceIDEnable = !IsFaceIDEnable;

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
            ChangeFaceIDButton.IsEnabled = IsFaceIDEnable;
            DeleteFaceIDButton.IsEnabled = IsFaceIDEnable;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changePassword = new(UserLogin);
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