using AuthFaceIDModernUI.VoiceID.AudioEngine.MP3PlayerImpl;
using AuthFaceIDModernUI.VoiceID.Model;

namespace AuthFaceIDModernUI.VoiceID.AudioEngine
{
    public class MP3Player : IPlayer
    {
        private readonly PlayingState m_playingState;
        private readonly StoppedState m_stoppedState;

        private AMP3PlayerState m_activeState;

        private readonly InternalData m_data;

        public string FileName
        {
            get => m_data.FileName!;
            set => m_data.FileName = value;
        }

        public event EventHandler<EventData> PlayingStoppedEvent;

        public MP3Player()
        {
            m_data = new InternalData();
            m_playingState = new PlayingState(m_data);
            m_stoppedState = new StoppedState(m_data);

            m_activeState = m_stoppedState;

            m_data.PlayingStoppedEvent += (owner, args) => PlayingStoppedEvent?.Invoke(owner, args);
        }

        public void Play()
        {
            m_activeState.Play();
            m_activeState = m_playingState;

            m_data.PlayingStoppedEvent += ((sender, data) =>
            {
                m_activeState.Stop();
                m_activeState = m_stoppedState;
            });
        }

        public void Stop()
        {
            m_activeState.Stop();
            m_activeState = m_stoppedState;
        }

        public bool IsActive()
        {
            return m_activeState.IsActive();
        }
    }
}
