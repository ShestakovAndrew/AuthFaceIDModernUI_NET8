namespace AuthFaceIDModernUI.VoiceID.Model
{
    public class EventData : EventArgs { }

    public enum EPlayStatus
    {
        Playing = 0,
        Stopped,
    };

    public interface IPlayer
    {
        string FileName { set; get; }

        event EventHandler<EventData> PlayingStoppedEvent;

        void Play();

        void Stop();
        
        bool IsActive();
    }
}
