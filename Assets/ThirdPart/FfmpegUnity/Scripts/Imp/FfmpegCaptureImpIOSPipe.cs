#if UNITY_IOS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegCaptureImpIOSPipe : FfmpegCaptureImpBase
    {
        [DllImport("__Internal")]
        static extern void ffmpeg_mkpipe(IntPtr output, int outputLength);
        [DllImport("__Internal")]
        static extern void ffmpeg_closePipe(string pipeName);

        public FfmpegCaptureImpIOSPipe(FfmpegCaptureCommand captureCommand) : base(captureCommand)
        {
        }

        protected override string GenerateCaptureFileName()
        {
            IntPtr hglobalPipe = Marshal.AllocHGlobal(1024);
            ffmpeg_mkpipe(hglobalPipe, 1024);
            string fileName = Marshal.PtrToStringAuto(hglobalPipe);
            Marshal.FreeHGlobal(hglobalPipe);

            return fileName;
        }

        public override bool Reverse
        {
            get
            {
                return false;
            }
        }

        public override void WriteVideo(int streamId, string pipeFileName, int width, int height, Dictionary<int, byte[]> videoBuffers)
        {
            try
            {
                using (var stream = File.OpenWrite(pipeFileName))
                using (var writer = new BinaryWriter(stream))
                {
                    StreamWriteVideo(writer, streamId, videoBuffers);
                }
            }
            catch (IOException)
            {
                IsEnd = true;
            }

            File.Delete(pipeFileName);
        }

        public override void WriteAudio(int streamId, string pipeFileName, int sampleRate, int channels, Dictionary<int, List<float>> audioBuffers)
        {
            try
            {
                using (var stream = File.OpenWrite(pipeFileName))
                using (var writer = new BinaryWriter(stream))
                {
                    StreamWriteAudio(writer, streamId, audioBuffers);
                }
            }
            catch (IOException)
            {
                IsEnd = true;
            }

            File.Delete(pipeFileName);
        }
    }
}

#endif