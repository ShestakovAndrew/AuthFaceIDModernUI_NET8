using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFaceIDModernUI.VoiceID.Audio.MP3RecorderImpl
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
