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
    public class FfmpegCaptureImpAndroidPipe : FfmpegCaptureImpBase
    {
        public FfmpegCaptureImpAndroidPipe(FfmpegCaptureCommand captureCommand) : base(captureCommand)
        {
        }

        protected override string GenerateCaptureFileName()
        {
            string dataDir;

            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext"))
            using (AndroidJavaObject info = context.Call<AndroidJavaObject>("getApplicationInfo"))
            {
                dataDir = info.Get<string>("dataDir");
            }

            string fileName = dataDir + "/FfmpegUnity_" + Guid.NewGuid().ToString();

            using (AndroidJavaClass os = new AndroidJavaClass("android.system.Os"))
            {
                os.CallStatic("mkfifo", fileName, Convert.ToInt32("777", 8));
            }

            return fileName;
        }

        public override bool Reverse
        {
            get
            {
                return true;
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