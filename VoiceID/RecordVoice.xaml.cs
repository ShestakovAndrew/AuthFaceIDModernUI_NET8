using AuthFaceIDModernUI.VoiceID.AudioEngine;
using System.IO;
using System.Windows;

namespace AuthFaceIDModernUI.VoiceID
{
    public partial class RecordVoice
    {
        private MP3Recorder m_MP3Recorder;

        public string m_outFilePath;

        public RecordVoice()
        {
            InitializeComponent();

            m_MP3Recorder = new MP3Recorder();
            m_outFilePath = string.Empty;

            StartRecordButton.IsEnabled = true;
            StopRecordButton.IsEnabled = false;
            SaveAndCloseButton.IsEnabled = false;

            AudioTextBlock.Text = "Нажмите старт!";
        }

        private void StartRecordButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateFileName();
            UpdateRecordButtons();

            SaveAndCloseButton.IsEnabled = false;   
            m_MP3Recorder.OutFileName = m_outFilePath;
            m_MP3Recorder.StartRecording();
            AudioTextBlock.Text = "Идёт запись микрофона";
        }

        private void StopRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_MP3Recorder.IsActive())
            {
                m_MP3Recorder.StopRecording();
                AudioTextBlock.Text = "Аудио записано!";
                UpdateRecordButtons();
                SaveAndCloseButton.IsEnabled = true;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SaveAndCloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void UpdateFileName()
        {
            m_outFilePath = Path.Join(Config.BaseDirectoryPath, $"VoiceID\\AudioFiles\\FromUser\\{DateTime.Now.ToString("dddd_dd_MMMM_yyyy_HH_mm_ss")}.mp3");
        }

        private void UpdateRecordButtons()
        {
            StartRecordButton.IsEnabled = !StartRecordButton.IsEnabled;
            StopRecordButton.IsEnabled = !StopRecordButton.IsEnabled;
        }
    }
}