using AuthFaceIDModernUI.VoiceID.Audio;
using System.Windows;

namespace AuthFaceIDModernUI.VoiceID
{
    public partial class RecordVoice : Window
    {
        private MP3Recorder m_MP3Recorder;
        public string m_outFilePath;

        public RecordVoice()
        {
            InitializeComponent();

            m_MP3Recorder = new MP3Recorder();
            m_outFilePath = string.Empty;

            SaveAndCloseButton.IsEnabled = false;
        }

        private void StartStopRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_MP3Recorder.IsActive())
            {
                SaveAndCloseButton.IsEnabled = true; 
                m_MP3Recorder.StopRecording();

                StartStopRecordButton.Content = "Начать запись";
            }
            else
            {
                SaveAndCloseButton.IsEnabled = false;

                UpdateFileName();
                m_MP3Recorder.OutFileName = m_outFilePath;
                m_MP3Recorder.StartRecording();

                StartStopRecordButton.Content = "Закончить запись";
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

        public void UpdateFileName()
        {
            m_outFilePath = System.IO.Path.Join(Config.BaseDirectoryPath, $"VoiceID\\AudioFiles\\FromUser\\{DateTime.Now.ToString("dddd_dd_MMMM_yyyy_HH_mm_ss")}.wav");
        }
    }
}
