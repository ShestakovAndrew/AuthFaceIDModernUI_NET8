using AuthFaceIDModernUI.VoiceID.AudioEngine.MP3RecorderImpl;
using AuthFaceIDModernUI.VoiceID.Model;
using NAudio.CoreAudioApi;

namespace AuthFaceIDModernUI.VoiceID.AudioEngine
{
    public class MP3Recorder : IRecorder
    {
        private readonly RecordingState m_recordingState;
        private readonly StoppedState m_stoppedState;

        private MP3RecorderState m_activeState;

        private readonly InternalData m_data;

        public string OutFileName
        {
            get => m_data.OutFileName!;
            set => m_data.OutFileName = value;
        }

        public List<IDevice> Devices
        {
            get => m_data.Devices!;
            set => m_data.Devices = value;
        }

        public IDevice ActiveDevice
        {
            get => m_data.ActiveDevice!;
            set => m_data.ActiveDevice = value;
        }

        public MP3Recorder()
        {
            m_data = new InternalData();
            m_recordingState = new RecordingState(m_data);
            m_stoppedState = new StoppedState(m_data);

            m_activeState = m_stoppedState;

            CollectMicrophones();
            SelectDefaultDevice();
        }

        public void StartRecording()
        {
            m_activeState.StartRecording();
            m_activeState = m_recordingState;
        }

        public void StopRecording()
        {
            m_activeState.StopRecording();
            m_activeState = m_stoppedState;
        }

        public bool IsActive()
        {
            return m_activeState.IsActive();
        }
        
        private void CollectMicrophones()
        {
            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            Devices = new List<IDevice>();

            foreach (var device in devices)
            {
                Devices.Add(new Microphone(device));
            }
        }

        private void SelectDefaultDevice()
        {
            var enumerator = new MMDeviceEnumerator();
            var defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
            ActiveDevice = new Microphone(defaultDevice);
        }
    }
}
