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
    public class FfmpegBytesInputsAndroidPipe : FfmpegBytesInputs
    {
        public FfmpegBytesInputsAndroidPipe(string[] inputOptions, FfmpegCommand command) : base(inputOptions, command)
        {
        }

        protected override string GenerateFileName()
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
