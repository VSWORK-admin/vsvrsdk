using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegCaptureImpUnixMonoBin : FfmpegCaptureImpBase
    {
        public FfmpegCaptureImpUnixMonoBin(FfmpegCaptureCommand captureCommand) : base(captureCommand)
        {
        }

        protected override string GenerateCaptureFileName()
        {
            string fileName = Application.temporaryCachePath + "/FfmpegUnity_" + Guid.NewGuid().ToString();
            ProcessStartInfo psInfoMkFifo = new ProcessStartInfo()
            {
                FileName = "mkfifo",
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = "\"" + fileName + "\"",
            };
            using (Process process = Process.Start(psInfoMkFifo))
            {
                process.WaitForExit();
            }
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