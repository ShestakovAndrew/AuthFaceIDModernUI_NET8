using AuthFaceIDModernUI.VoiceID.Model;
using NAudio.Wave;

namespace AuthFaceIDModernUI.VoiceID.AudioEngine.MP3PlayerImpl
{
    internal class InternalData
    {
        public string? FileName { set; get; } = null;

        public event EventHandler<EventData>? PlayingStoppedEvent;

        public Mp3FileReader? Reader { get; set; } = null;

        public WaveOut? WaveOut { get; set; } = null;

        public void InvokePlayingStoppedEventListeners(object owner, EventData args)
        {
            PlayingStoppedEvent?.Invoke(owner, args);
        }
    }
}
