using AuthFaceIDModernUI.VoiceID.Model;
using NAudio.Wave;

namespace AuthFaceIDModernUI.VoiceID.AudioEngine.MP3RecorderImpl
{
    internal class InternalData
    {
        public string? OutFileName { get; set; } = null;

        public List<IDevice>? Devices { get; set; } = null;

        public IDevice? ActiveDevice { get; set; } = null;

        public WaveFileWriter? Writer { get; set; } = null;

        public IWaveIn? WaveIn { get; set; } = null;
    }
}
