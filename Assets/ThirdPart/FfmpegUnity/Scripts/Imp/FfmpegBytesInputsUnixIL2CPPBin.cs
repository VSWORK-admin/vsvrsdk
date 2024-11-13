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
    public class FfmpegBytesInputsUnixIL2CPPBin : FfmpegBytesInputs
    {
        [DllImport("__Internal")]
        static extern int unity_system(string command);

        public FfmpegBytesInputsUnixIL2CPPBin(string[] inputOptions, FfmpegCommand command) : base(inputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            string fileName = "/tmp/FfmpegUnity_" + Guid.NewGuid().ToString();

            unity_system("mkfifo \"" + fileName + "\"");

            return fileName;
        }

        protected override void Write(string pipeFileName, int streamId)
        {
            while (!File.Exists(pipeFileName))
            {
                Thread.Sleep(1);
            }

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