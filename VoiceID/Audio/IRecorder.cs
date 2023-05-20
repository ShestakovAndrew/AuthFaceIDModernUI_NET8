using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFaceIDModernUI.VoiceID.Audio
{
    public interface IDevice
    {
        string Name { get; set; }
    }

    public enum RecordingStatus
    {
        Recording = 0,
        Stopped,
    };

    public interface IRecorder
    {
        string OutFileName { set; get; }

        List<IDevice> Devices { get; }

        IDevice ActiveDevice { get; set; }

        void StartRecording();

        void StopRecording();

        bool IsActive();
    }
}
