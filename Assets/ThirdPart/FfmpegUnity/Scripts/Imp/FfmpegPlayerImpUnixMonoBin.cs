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
    public class FfmpegPlayerImpUnixMonoBin : FfmpegPlayerImpBase
    {
        Process process_;

        Dictionary<int, string> pipeFileNames_ = new Dictionary<int, string>();

        string dataDir_;

        public FfmpegPlayerImpUnixMonoBin(FfmpegPlayerCommand playerCommand) : base(playerCommand)
        {
            dataDir_ = Application.temporaryCachePath;
        }

        public override TextReader OpenFfprobeReader(string inputPathAll)
        {
            string fileName = "ffprobe";
#if UNITY_EDITOR_OSX
#if !FFMPEG_UNITY_USE_OUTER_MAC
            fileName = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Mac/ffprobe");
#else
            if (File.Exists("/usr/local/bin/ffprobe"))
            {
                fileName = "/usr/local/bin/ffprobe";
            }
#endif
#elif UNITY_EDITOR_LINUX
#if !FFMPEG_UNITY_USE_OUTER_LINUX
            fileName = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Linux/ffprobe");
#endif
#elif (UNITY_STANDALONE_OSX && !FFMPEG_UNITY_USE_OUTER_MAC) || (UNITY_STANDALONE_LINUX && !FFMPEG_UNITY_USE_OUTER_LINUX)
            fileName = Application.streamingAssetsPath + "/_FfmpegUnity_temp/ffprobe";
#endif

            ProcessStartInfo psInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = "-i \"" + inputPathAll + "\" -show_streams",
            };

            process_ = Process.Start(psInfo);
            return process_.StandardOutput;
        }

        public override void CloseFfprobeReader()
        {
            if (process_ != null)
            {
                process_.WaitForExit();
                process_.Dispose();
                process_ = null;
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
            string fileNameFifo = dataDir_ + "/FfmpegUnity_" + Guid.NewGuid().ToString();
            pipeFileNames_[streamId] = fileNameFifo;

            ProcessStartInfo psInfoMkFifo = new ProcessStartInfo()
            {
                FileName = "mkfifo",
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = "\"" + fileNameFifo + "\"",
            };
            using (Process process = Process.Start(psInfoMkFifo))
            {
                process.WaitForExit();
            }

            return " -f rawvideo -pix_fmt rgba \"" + fileNameFifo + "\"";
        }

        public override string BuildAudioOptions(int streamId, int sampleRate, int channels)
        {
            string fileNameFifo = dataDir_ + "/FfmpegUnity_" + Guid.NewGuid().ToString();
            pipeFileNames_[streamId] = fileNameFifo;

            ProcessStartInfo psInfoMkFifo = new ProcessStartInfo()
            {
                FileName = "mkfifo",
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = "\"" + fileNameFifo + "\"",
            };
            using (Process process = Process.Start(psInfoMkFifo))
            {
                process.WaitForExit();
            }

            return " -f f32le \"" + fileNameFifo + "\" ";
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
