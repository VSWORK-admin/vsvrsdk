#if UNITY_IOS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegBytesOutputsIOSPipe : FfmpegBytesOutputs
    {
        [DllImport("__Internal")]
        static extern void ffmpeg_mkpipe(IntPtr output, int outputLength);
        [DllImport("__Internal")]
        static extern void ffmpeg_closePipe(string pipeName);

        public FfmpegBytesOutputsIOSPipe(string[] outputOptions, FfmpegCommand command) : base(outputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            IntPtr hglobalPipe = Marshal.AllocHGlobal(1024);
            ffmpeg_mkpipe(hglobalPipe, 1024);
            string fileNameFifo = Marshal.PtrToStringAuto(hglobalPipe);
            Marshal.FreeHGlobal(hglobalPipe);

            return fileNameFifo;
        }

        protected override void Read(string pipeFileName, int streamId)
        {
            string fileName = pipeFileName;

            using (var stream = File.OpenRead(fileName))
            using (var reader = new BinaryReader(stream))
            {
                StreamRead(reader, streamId);
            }

            ffmpeg_closePipe(fileName);
        }
    }
}

#endif