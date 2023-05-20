using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFaceIDModernUI.VoiceID.Audio.MP3RecorderImpl
{
    internal abstract class MP3RecorderState
    {
        protected InternalData Data;

        protected MP3RecorderState(InternalData data)
        {
            Data = data;
        }

        public abstract void StartRecording();

        public abstract void StopRecording();

        public abstract bool IsActive();

        protected void Dispose()
        {
            Data.WaveIn?.Dispose();
            Data.WaveIn = null;
            Data.Writer?.Close();
            Data.Writer = null;
        }
    }

    internal class RecordingState : MP3RecorderState
    {
        public RecordingState(InternalData data)
            : base(data)
        { }

        public override void StartRecording()
        {
            throw new InvalidOperationException("ERROR: The recording is already started.");
        }

        public override void StopRecording()
        {
            Data.WaveIn?.StopRecording();
            Dispose();
        }

        public override bool IsActive()
        {
            return true;
        }
    }

    internal class StoppedState : MP3RecorderState
    {
        public StoppedState(InternalData data)
            : base(data)
        { }

        public override void StartRecording()
        {
            InitRecorder();
            Data.WaveIn?.StartRecording();
        }

        public override void StopRecording()
        {
            throw new InvalidOperationException("ERROR: The recording is already Stopped.");
        }

        public override bool IsActive()
        {
            return false;
        }

        private void InitRecorder()
        {
            if (Data.WaveIn != null) { throw new InvalidOperationException("ERROR: Data.WaveIn should be null."); }
            Data.WaveIn = new WasapiCapture((Data.ActiveDevice as Microphone).Device);

            if (Data.Writer != null) { throw new InvalidOperationException("ERROR: Data.Writer should be null."); }
            Data.Writer = new WaveFileWriter(Data.OutFileName, Data.WaveIn.WaveFormat);

            Data.WaveIn.DataAvailable += AvailableDataHandler!;
            Data.WaveIn.RecordingStopped += StopRecordingHandler!;
        }

        private void AvailableDataHandler(object sender, WaveInEventArgs e)
        {
            Data.Writer?.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private static void StopRecordingHandler(object sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                throw new Exception($"ERROR: A problem was encountered during recording {e.Exception.Message}", e.Exception);
            }
        }
    }
}
