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
    public class FfmpegPlayerImpAndroidPipe : FfmpegPlayerImpAndroidBase
    {
        Dictionary<int, string> pipeFileNames_ = new Dictionary<int, string>();

        public FfmpegPlayerImpAndroidPipe(FfmpegPlayerCommand playerCommand) : base(playerCommand)
        {
        }

        public override string BuildVideoOptions(int streamId, int width, int height)
        {
            string fileName = DataDir + "/FfmpegUnity_" + Guid.NewGuid().ToString();
            pipeFileNames_.Add(streamId, fileName);

            using (AndroidJavaClass os = new AndroidJavaClass("android.system.Os"))
            {
                os.CallStatic("mkfifo", fileName, Convert.ToInt32("777", 8));
            }

            return " -f rawvideo -pix_fmt rgba \"" + fileName + "\"";
        }

        public override string BuildAudioOptions(int streamId, int sampleRate, int channels)
        {
            string fileName = DataDir + "/FfmpegUnity_" + Guid.NewGuid().ToString();
            pipeFileNames_.Add(streamId, fileName);

            using (AndroidJavaClass os = new AndroidJavaClass("android.system.Os"))
            {
                os.CallStatic("mkfifo", fileName, Convert.ToInt32("777", 8));
            }

            return " -f f32le \"" + fileName + "\"";
        }

        public override void ReadVideo(int streamId, int width, int height)
        {
            string fileName = pipeFileNames_[streamId];

            while (!File.Exists(fileName))
            {
                Thread.Sleep(1);
            }

            using (var stream = File.OpenRead(fileName))
            using (var reader = new BinaryReader(stream))
            {
                StreamReadVideo(reader, streamId, width, height, false);
            }

            File.Delete(fileName);
        }

        public override void ReadAudio(int streamId, int sampleRate, int channels)
        {
            string fileName = pipeFileNames_[streamId];

            while (!File.Exists(fileName))
            {
                Thread.Sleep(1);
            }

            using (var stream = File.OpenRead(fileName))
            using (var reader = new BinaryReader(stream))
            {
                StreamReadAudio(reader, streamId, false);
            }

            File.Delete(fileName);
        }
    }
}
