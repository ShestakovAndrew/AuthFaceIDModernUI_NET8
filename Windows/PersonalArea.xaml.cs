using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using ModernLoginWindow;
using System.Windows;
using System.Windows.Input;

namespace AuthFaceIDModernUI.Windows
{
    public partial class PersonalArea : Window
    {
        public PersonalArea(string userLogin)
        {
            InitializeComponent();

            TitleTextBlock.Text = (userLogin + TitleTextBlock.Text);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void RecordVoiceButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ListenRecordingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Login login = new();
            login.Show();
            Close();
        }
    }
}