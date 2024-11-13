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
    public class FfmpegPlayerImpWinMonoBin : FfmpegPlayerImpBase
    {
        Process process_;
        TextReader reader_;

        Dictionary<int, string> pipeFileNames_ = new Dictionary<int, string>();

        public FfmpegPlayerImpWinMonoBin(FfmpegPlayerCommand playerCommand) : base(playerCommand) {}

        public override TextReader OpenFfprobeReader(string inputPathAll)
        {
            string fileName = "ffprobe";

#if !FFMPEG_UNITY_USE_OUTER_WIN
#if UNITY_EDITOR
            fileName = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Windows/ffprobe.exe");
#else
            fileName = Application.streamingAssetsPath + "/_FfmpegUnity_temp/ffprobe.exe";
#endif
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
            if (reader_ != null)
            {
                reader_.ReadToEnd();
                reader_.Dispose();
                reader_ = null;
            }

            if (process_ != null)
            {
                process_.WaitForExit();
                process_.Dispose();
                process_ = null;
            }
        }

        public override string BuildVideoOptions(int streamId, int width, int height)
        {
            string pipeFileName = @"\\.\pipe\FfmpegUnity_" + Guid.NewGuid().ToString();
            if (pipeFileNames_.ContainsKey(streamId))
            {
                pipeFileNames_.Remove(streamId);
            }
            pipeFileNames_.Add(streamId, pipeFileName);
            return " -f rawvideo -pix_fmt rgba \"" + pipeFileName + "\" ";
        }

        public override string BuildAudioOptions(int streamId, int sampleRate, int channels)
        {
            string pipeFileName = @"\\.\pipe\FfmpegUnity_" + Guid.NewGuid().ToString();
            if (pipeFileNames_.ContainsKey(streamId))
            {
                pipeFileNames_.Remove(streamId);
            }
            pipeFileNames_.Add(streamId, pipeFileName);
            return " -f f32le \"" + pipeFileName + "\" ";
        }

        public override void ReadVideo(int streamId, int width, int height)
        {
            string pipeName = pipeFileNames_[streamId].Replace(@"\\.\pipe\", "");

            using (var stream = new NamedPipeServerStream(pipeName,
                PipeDirection.In,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.WriteThrough,
                width * height * 4, width * height * 4))
            {
                Thread thread = new Thread(() =>
                {
                    while (!IsEnd && !stream.IsConnected)
                    {
                        Thread.Sleep(1);
                    }
                    if (!stream.IsConnected)
                    {
                        using (var dummyStream = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
                        {
                            dummyStream.Connect();
                        }
                    }
                });
                thread.Start();

                stream.WaitForConnection();

                using (var reader = new BinaryReader(stream))
                {
                    StreamReadVideo(reader, streamId, width, height);
                }

                bool exited = thread.Join(1);
                while (!exited && PlayerCommand.IsRunning)
                {
                    exited = thread.Join(1);
                }
                if (!exited && !PlayerCommand.IsRunning)
                {
                    thread.Abort();
                }
            }
        }

        public override void ReadAudio(int streamId, int sampleRate, int channels)
        {
            string pipeName = pipeFileNames_[streamId].Replace(@"\\.\pipe\", "");

            using (var stream = new NamedPipeServerStream(pipeName,
                PipeDirection.In,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.WriteThrough,
                sampleRate * 4, sampleRate * 4))
            {
                Thread thread = new Thread(() =>
                {
                    while (!IsEnd && !stream.IsConnected)
                    {
                        Thread.Sleep(1);
                    }
                    if (!stream.IsConnected)
                    {
                        using (var dummyStream = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
                        {
                            dummyStream.Connect();
                        }
                    }
                });
                thread.Start();

                stream.WaitForConnection();

                using (var reader = new BinaryReader(stream))
                {
                    StreamReadAudio(reader, streamId);
                }

                bool exited = thread.Join(1);
                while (!exited && PlayerCommand.IsRunning)
                {
                    exited = thread.Join(1);
                }
                if (!exited && !PlayerCommand.IsRunning)
                {
                    thread.Abort();
                }
            }
        }
    }
}
