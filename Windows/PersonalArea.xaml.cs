using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using AuthFaceIDModernUI.VoiceID;
using ModernLoginWindow;
using System.Windows;
using System.Windows.Input;

namespace AuthFaceIDModernUI.Windows
{
    public partial class PersonalArea : Window
    {
        private string m_voiceFilePath;

        public PersonalArea(string userLogin)
        {
            InitializeComponent();

            TitleTextBlock.Text = (userLogin + TitleTextBlock.Text);
            m_voiceFilePath = "";        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void RecordVoiceButton_Click(object sender, RoutedEventArgs e)
        {
            RecordVoice recordVoice = new();

            if (recordVoice.ShowDialog() == true)
            {

            }
            else
            {

            }
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