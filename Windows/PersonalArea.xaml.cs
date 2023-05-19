using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using AuthFaceIDModernUI.VoiceID;
using AuthFaceIDModernUI.VoiceID.AudioEngine;
using ModernLoginWindow;
using NAudio.Wave;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Text;
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
            m_voiceFilePath = string.Empty;        
        }

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
                m_voiceFilePath = recordVoice.m_outFilePath;
                ListenRecordingButton.IsEnabled = true;

                ConvertStereoToMonoWav(recordVoice);
                SetTextFromFile();
                VoiceExistBlock.Text = "Аудио распознано!";
            }
            else
            {
                ListenRecordingButton.IsEnabled = false;
                TextFromRecordTextBox.Clear();
            }
        }

        private void ConvertStereoToMonoWav(RecordVoice recordVoice)
        {
            var waveFileReader = new WaveFileReader(m_voiceFilePath);
            var outFormat = new WaveFormat(waveFileReader.WaveFormat.SampleRate, 1);
            var resampler = new MediaFoundationResampler(waveFileReader, outFormat);

            recordVoice.UpdateFileName();
            m_voiceFilePath = recordVoice.m_outFilePath;
            WaveFileWriter.CreateWaveFile(m_voiceFilePath, resampler);
        }

        private async void SetTextFromFile()
        {
            TextFromRecordTextBox.Text = await VoiceTool.SpeechToText(m_voiceFilePath);
        }

        private void ListenRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            ListenRecordingButton.IsEnabled = false;
            SoundPlayer player = new() { SoundLocation = m_voiceFilePath };
            player.PlaySync();
            ListenRecordingButton.IsEnabled = true;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Login login = new();
            login.Show();
            Close();
        }
    }
}