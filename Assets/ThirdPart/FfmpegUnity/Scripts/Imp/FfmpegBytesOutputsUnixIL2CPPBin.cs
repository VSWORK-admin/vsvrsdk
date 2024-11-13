#if ((UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && FFMPEG_UNITY_USE_BINARY_MAC) || ((UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX) && FFMPEG_UNITY_USE_BINARY_LINUX)

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
    public class FfmpegBytesOutputsUnixIL2CPPBin : FfmpegBytesOutputs
    {
        [DllImport("__Internal")]
        static extern int unity_system(string command);

        public FfmpegBytesOutputsUnixIL2CPPBin(string[] outputOptions, FfmpegCommand command) : base(outputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            string fileNameFifo = "/tmp/FfmpegUnity_" + Guid.NewGuid().ToString();
            unity_system("mkfifo \"" + fileNameFifo + "\"");
            return fileNameFifo;
        }

        protected override void Read(string pipeFileName, int streamId)
        {
            string fileName = pipeFileName;

            while (!File.Exists(fileName))
            {
                Thread.Sleep(1);
            }

            using (var stream = File.OpenRead(fileName))
            using (var reader = new BinaryReader(stream))
            {
                StreamRead(reader, streamId);
            }

            File.Delete(fileName);
        }
    }
}

#endif