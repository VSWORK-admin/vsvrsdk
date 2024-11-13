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
    public class FfmpegBytesInputsIOSPipe : FfmpegBytesInputs
    {
        [DllImport("__Internal")]
        static extern void ffmpeg_mkpipe(IntPtr output, int outputLength);
        [DllImport("__Internal")]
        static extern void ffmpeg_closePipe(string pipeName);

        public FfmpegBytesInputsIOSPipe(string[] inputOptions, FfmpegCommand command) : base(inputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            IntPtr hglobalPipe = Marshal.AllocHGlobal(1024);
            ffmpeg_mkpipe(hglobalPipe, 1024);
            string fileName = Marshal.PtrToStringAuto(hglobalPipe);
            Marshal.FreeHGlobal(hglobalPipe);

            return fileName;
        }

        protected override void Write(string pipeFileName, int streamId)
        {
            try
            {
                using (var stream = File.OpenWrite(pipeFileName))
                using (var writer = new BinaryWriter(stream))
                {
                    StreamWrite(writer, streamId);
                }
            }
            catch (IOException)
            {
                IsStop = true;
            }

            File.Delete(pipeFileName);
        }
    }
}

#endif