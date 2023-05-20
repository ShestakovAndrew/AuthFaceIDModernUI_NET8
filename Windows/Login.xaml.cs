using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using AuthFaceIDModernUI.VoiceID;
using AuthFaceIDModernUI.Windows;
using Emgu.CV;
using NAudio.Wave;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ModernLoginWindow
{
    public partial class Login : Window
    {
        private string m_voiceFilePath;

        public Login()
        {
            InitializeComponent();

            m_voiceFilePath = string.Empty;
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
            FaceCamera faceCamera = new(null);

            faceCamera.TurnOn();
            Thread.Sleep(3000);
            faceCamera.TurnOff();

            UsersDataBase db = new();

            foreach (Mat frameWithFaces in faceCamera.GetUserFaces())
            {
                int id = await FaceIDTool.RecornizeIDOnImage(frameWithFaces);

                if (id != -1)
                {
                    if (await CheckSecretWord(id))
                    {
                        await VoiceTool.TextToSpeech("Авторизация прошла успешно");
                        LoginToPersonalArea(db.GetLoginByID(id));
                        return;
                    }
                    else
                    {
                        await VoiceTool.TextToSpeech("Секретное слово не верно, повторите попытку");
                    }
                }
            }

            await VoiceTool.TextToSpeech("Пользователь не распознан в системе");
        }

        private async Task<bool> CheckSecretWord(int userID)
        {
            await VoiceTool.TextToSpeech("Для завершения авторизации подтвердите секретное слово");

            RecordVoice recordVoice = new();

            if (recordVoice.ShowDialog() == true)
            {
                m_voiceFilePath = recordVoice.m_outFilePath;
                m_voiceFilePath = VoiceTool.ConvertStereoToMonoWav(recordVoice, m_voiceFilePath);

                string textFromUser = await VoiceTool.SpeechToText(m_voiceFilePath);
                
                UsersDataBase db = new();

                return textFromUser == db.GetSecretWordByID(userID);
            }

            return false;
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
            FaceIDButton.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox.BorderBrush = new SolidColorBrush(Colors.Gray);
        }
    }
}