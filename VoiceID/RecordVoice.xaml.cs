using Plugin.AudioRecorder;
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

namespace AuthFaceIDModernUI.VoiceID
{
    /// <summary>
    /// Логика взаимодействия для RecordVoice.xaml
    /// </summary>
    public partial class RecordVoice : Window
    {
        private AudioRecorderService audioRecorder;

        public RecordVoice()
        {
            InitializeComponent();

            audioRecorder = new AudioRecorderService();
        }

        private async Task StartRecordButton_Click(object sender, RoutedEventArgs e)
        {
            await audioRecorder.StartRecording();
        }

        private async Task StopRecordButton_Click(object sender, RoutedEventArgs e)
        {
            await audioRecorder.StopRecording();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveAndCloseButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
