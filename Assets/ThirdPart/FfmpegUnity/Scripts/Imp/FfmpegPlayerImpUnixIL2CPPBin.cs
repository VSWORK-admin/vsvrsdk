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
    public class FfmpegPlayerImpUnixIL2CPPBin : FfmpegPlayerImpBase
    {
        TextReader reader_;

        Dictionary<int, string> pipeFileNames_ = new Dictionary<int, string>();

        string dataDir_;

        [DllImport("__Internal")]
        static extern int unity_system(string command);
        [DllImport("__Internal")]
        static extern IntPtr unity_popen(string command, string type);
        [DllImport("__Internal")]
        static extern int unity_pclose(IntPtr stream);
        [DllImport("__Internal")]
        static extern IntPtr unity_fgets(IntPtr s, int n, IntPtr stream);

        public FfmpegPlayerImpUnixIL2CPPBin(FfmpegPlayerCommand playerCommand) : base(playerCommand)
        {
            dataDir_ = Application.temporaryCachePath;
        }

        public override TextReader OpenFfprobeReader(string inputPathAll)
        {
            string fileName = "ffprobe";
#if (UNITY_STANDALONE_OSX && !FFMPEG_UNITY_USE_OUTER_MAC) || (UNITY_STANDALONE_LINUX && !FFMPEG_UNITY_USE_OUTER_LINUX)
            fileName = Application.streamingAssetsPath + "/_FfmpegUnity_temp/ffprobe";
#endif
            IntPtr stdOutFp = unity_popen("\"" + fileName + "\" -i \"" + inputPathAll + "\" -show_streams", "r");
            string outputStr = "";
            IntPtr bufferHandler = Marshal.AllocHGlobal(1024);
            for (; ; )
            {
                IntPtr retPtr = unity_fgets(bufferHandler, 1024, stdOutFp);
                if (retPtr == IntPtr.Zero)
                {
                    break;
                }

                outputStr += Marshal.PtrToStringAuto(bufferHandler);
            }
            Marshal.FreeHGlobal(bufferHandler);
            unity_pclose(stdOutFp);

            reader_ = new StringReader(outputStr);
            return reader_;
        }

        public override void CloseFfprobeReader()
        {
            if (reader_ != null)
            {
                reader_.ReadToEnd();
                reader_.Dispose();
                reader_ = null;
            }
        }

        public override string BuildBeginOptions(string path)
        {
            string ret = "";

            if (!PlayerCommand.SyncFrameRate)
            {
                ret += " -fflags nobuffer ";
            }

            return ret + " -i \"" + path + "\" ";
        }

        public override string BuildVideoOptions(int streamId, int width, int height)
        {
            string fileNameFifo = "/tmp/FfmpegUnity_" + Guid.NewGuid().ToString();
            pipeFileNames_.Add(streamId, fileNameFifo);
            unity_system("mkfifo \"" + fileNameFifo + "\"");
            return " -f rawvideo -pix_fmt rgba \"" + fileNameFifo + "\"";
        }

        public override string BuildAudioOptions(int streamId, int sampleRate, int channels)
        {
            string fileNameFifo = "/tmp/FfmpegUnity_" + Guid.NewGuid().ToString();
            pipeFileNames_.Add(streamId, fileNameFifo);
            unity_system("mkfifo \"" + fileNameFifo + "\"");
            return " -f f32le \"" + fileNameFifo + "\"";
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
                StreamReadVideo(reader, streamId, width, height);
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
                StreamReadAudio(reader, streamId);
            }

            File.Delete(fileName);
        }
    }
}

#endif