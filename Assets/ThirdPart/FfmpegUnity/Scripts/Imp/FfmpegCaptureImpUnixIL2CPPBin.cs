#if ((UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && FFMPEG_UNITY_USE_BINARY_MAC) || ((UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX) && FFMPEG_UNITY_USE_BINARY_LINUX)

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
    public class FfmpegCaptureImpUnixIL2CPPBin : FfmpegCaptureImpBase
    {
        [DllImport("__Internal")]
        static extern int unity_system(string command);

        public FfmpegCaptureImpUnixIL2CPPBin(FfmpegCaptureCommand captureCommand) : base(captureCommand)
        {
        }

        protected override string GenerateCaptureFileName()
        {
            string fileName = "/tmp/FfmpegUnity_" + Guid.NewGuid().ToString();
            unity_system("mkfifo \"" + fileName + "\"");
            return fileName;
        }

        public override bool Reverse
        {
            get
            {
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
                return true;
#else
                return false;
#endif
            }
        }

        public override void WriteVideo(int streamId, string pipeFileName, int width, int height, Dictionary<int, byte[]> videoBuffers)
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
            while (!File.Exists(pipeFileName))
            {
                Thread.Sleep(1);
            }

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