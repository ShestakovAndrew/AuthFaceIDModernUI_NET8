using AuthFaceIDModernUI.VoiceID.Model;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFaceIDModernUI.VoiceID.AudioEngine.MP3RecorderImpl
{
    internal class Microphone : IDevice
    {
        public Microphone(MMDevice device)
        {
            Device = device;
        }

        public string Name
        {
            get => Device.FriendlyName;
            set => throw new NotImplementedException();
        }

        public MMDevice Device { get; private set; }
    }
}
