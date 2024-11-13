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
    public class FfmpegPlayerImpIOSPipe : FfmpegPlayerImpIOSBase
    {
        Dictionary<int, string> pipeFileNames_ = new Dictionary<int, string>();

        [DllImport("__Internal")]
        static extern IntPtr ffmpeg_ffprobeExecuteAsync(string command);
        [DllImport("__Internal")]
        static extern bool ffmpeg_isRunnning(IntPtr session);
        [DllImport("__Internal")]
        static extern int ffmpeg_getOutputLength(IntPtr session);
        [DllImport("__Internal")]
        static extern void ffmpeg_getOutput(IntPtr session, int startIndex, IntPtr output, int outputLength);
        [DllImport("__Internal")]
        static extern void ffmpeg_mkpipe(IntPtr output, int outputLength);
        [DllImport("__Internal")]
        static extern void ffmpeg_closePipe(string pipeName);

        public FfmpegPlayerImpIOSPipe(FfmpegPlayerCommand playerCommand) : base(playerCommand)
        {
        }

        public override string BuildVideoOptions(int streamId, int width, int height)
        {
            IntPtr hglobalPipe = Marshal.AllocHGlobal(1024);
            ffmpeg_mkpipe(hglobalPipe, 1024);
            string fileNameFifo = Marshal.PtrToStringAuto(hglobalPipe);
            Marshal.FreeHGlobal(hglobalPipe);

            pipeFileNames_[streamId] = fileNameFifo;

            return " -frame_drop_threshold 5 -f rawvideo -pix_fmt rgba \"" + fileNameFifo + "\"";
        }

        public override string BuildAudioOptions(int streamId, int sampleRate, int channels)
        {
            IntPtr hglobalPipe = Marshal.AllocHGlobal(1024);
            ffmpeg_mkpipe(hglobalPipe, 1024);
            string fileNameFifo = Marshal.PtrToStringAuto(hglobalPipe);
            Marshal.FreeHGlobal(hglobalPipe);

            pipeFileNames_[streamId] = fileNameFifo;

            return " -f f32le \"" + fileNameFifo + "\"";
        }

        public override void ReadVideo(int streamId, int width, int height)
        {
            string fileName = pipeFileNames_[streamId];

            using (var stream = File.OpenRead(fileName))
            using (var reader = new BinaryReader(stream))
            {
                StreamReadVideo(reader, streamId, width, height);
            }

            ffmpeg_closePipe(fileName);
        }

        public override void ReadAudio(int streamId, int sampleRate, int channels)
        {
            string fileName = pipeFileNames_[streamId];

            using (var stream = File.OpenRead(fileName))
            using (var reader = new BinaryReader(stream))
            {
                StreamReadAudio(reader, streamId);
            }

            ffmpeg_closePipe(fileName);
        }
    }
}

#endif