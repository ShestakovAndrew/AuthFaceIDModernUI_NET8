using AuthFaceIDModernUI.VoiceID.Model;
using NAudio.Wave;

namespace AuthFaceIDModernUI.VoiceID.AudioEngine.MP3PlayerImpl
{
    internal abstract class AMP3PlayerState
    {
        protected InternalData Data;

        protected AMP3PlayerState(InternalData data)
        {
            Data = data;
        }

        public abstract void Play();

        public abstract void Stop();

        public abstract bool IsActive();
    }

    internal class PlayingState : AMP3PlayerState
    {
        public PlayingState(InternalData data) : base(data)
        {
        }

        public override void Play()
        {
            throw new InvalidOperationException("ERROR: The Playing is already started.");
        }

        public override void Stop()
        {
            Data.WaveOut?.Stop();
            Data.WaveOut?.Dispose();
            Data.WaveOut = null;
            Data.Reader?.Close();
            Data.Reader = null;
        }

        public override bool IsActive()
        {
            return true;
        }
    }

    internal class StoppedState : AMP3PlayerState
    {
        public StoppedState(InternalData data) : base(data)
        {
        }

        public override void Play()
        {
            InitPlayer();
            Data.WaveOut?.Play();
        }

        public override void Stop()
        {
        }

        public override bool IsActive()
        {
            return false;
        }

        private void InitPlayer()
        {
            if (Data.Reader != null) { throw new InvalidOperationException("ERROR: Data.Reader should be null."); }
            Data.Reader = new Mp3FileReader(Data.FileName);

            if (Data.WaveOut != null) { throw new InvalidOperationException("ERROR: Data.WaveOut should be null."); }
            Data.WaveOut = new WaveOut();

            Data.WaveOut.Init(Data.Reader);
            Data.WaveOut.PlaybackStopped += PlaybackStoppedHandler!;
        }

        private void PlaybackStoppedHandler(object sender, StoppedEventArgs e)
        {
            Stop();
            Data.InvokePlayingStoppedEventListeners(this, new EventData());
        }
    }
}
