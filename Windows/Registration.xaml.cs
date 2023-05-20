using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using AuthFaceIDModernUI.VoiceID;
using AuthFaceIDModernUI.VoiceID.Audio;
using Emgu.CV;
using ModernLoginWindow;
using NAudio.Wave;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AuthFaceIDModernUI.Windows
{
    public partial class Registration : Window
    {
        private string m_voiceFilePath;

        public Registration()
        {
            InitializeComponent();

            m_voiceFilePath = string.Empty;
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

            UsersDataBase db = new();

            if (db.IsLoginExistInDB(NewLoginTextBox.Text))
            {
                NewLoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (db.AddNewUser(NewLoginTextBox.Text, NewPasswordBox.Password))
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

        private async void AddFaceAndVoiceOnAccountButton_Click(object sender, RoutedEventArgs e)
        {
            await FaceIDTool.ResetSpaceWithFaces();

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

            UsersDataBase db = new();

            if (db.IsLoginExistInDB(NewLoginTextBox.Text))
            {
                NewLoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                await VoiceTool.TextToSpeech("Пользователь с таким логином уже зарегистрирован в системе");
                return;
            }

            FaceCamera faceCamera = new(null);
            faceCamera.TurnOn();
            Thread.Sleep(3000);
            faceCamera.TurnOff();

            foreach (Mat frameWithFaces in faceCamera.GetUserFaces())
            {
                if (await FaceIDTool.RecornizeIDOnImage(frameWithFaces) != -1)
                {
                    await VoiceTool.TextToSpeech("Данный пользователь уже был зарегистрирован в системе");
                    return;
                }
            }

            if (
                db.AddNewUser(NewLoginTextBox.Text, NewPasswordBox.Password) &&
                await FaceIDTool.SetPersonID(faceCamera.GetUserFaces()[0], db.GetIDByLogin(NewLoginTextBox.Text))
            )
            {
                await VoiceTool.TextToSpeech("Для завершения регистрации скажите секретное слово");

                do
                {
                    RecordVoice recordVoice = new();

                    if (recordVoice.ShowDialog() == true)
                    {
                        m_voiceFilePath = recordVoice.m_outFilePath;
                        m_voiceFilePath = VoiceTool.ConvertStereoToMonoWav(recordVoice, m_voiceFilePath);

                        string textFromUser = await VoiceTool.SpeechToText(m_voiceFilePath);

                        MessageBoxResult boxResult = MessageBox.Show($"Подтвердить секретное слово - {textFromUser}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        
                        if (boxResult == MessageBoxResult.Yes)
                        {
                            db.AddSecretWordForUser(NewLoginTextBox.Text, textFromUser);
                        }
                        else
                        {
                            await VoiceTool.TextToSpeech("Секретное слово не подтверждено, нужно повторить запись");
                        }
                    }

                } while (!db.IsUserHasSecretWord(NewLoginTextBox.Text));

                await VoiceTool.TextToSpeech("Пользователь с биометрическими данными был успешно добавлен в систему");
                OpenNewLoginWindow();
            }
            else
            {
                db.DeleteUserByID(db.GetIDByLogin(NewLoginTextBox.Text));
                await VoiceTool.TextToSpeech($"Биометрические данные для пользователя с логином {NewLoginTextBox.Text} не были добавлены");
            }
        }
    }
}