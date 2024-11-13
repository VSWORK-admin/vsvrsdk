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
    public abstract class FfmpegBytesOutputs : IDisposable
    {
        protected const int BUFFER_SIZE = 5000000;
        protected const int BLOCK_SIZE = 1024;

        public static FfmpegBytesOutputs GetNewInstance(string[] outputOptions, FfmpegCommand command)
        {
#if UNITY_EDITOR_WIN && FFMPEG_UNITY_USE_BINARY_WIN
            return new FfmpegBytesOutputsWinMonoBin(outputOptions, command);
#elif UNITY_EDITOR_WIN && !FFMPEG_UNITY_USE_BINARY_WIN || UNITY_EDITOR_LINUX && !FFMPEG_UNITY_USE_BINARY_LINUX
            return new FfmpegBytesOutputsWinLib(outputOptions, command);
#elif (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC
            return new FfmpegBytesOutputsIOSMemory(outputOptions, command);
#elif UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            return new FfmpegBytesOutputsUnixMonoBin(outputOptions, command);
#elif UNITY_STANDALONE_WIN && !FFMPEG_UNITY_USE_BINARY_WIN || UNITY_STANDALONE_LINUX && !FFMPEG_UNITY_USE_BINARY_LINUX
            return new FfmpegBytesOutputsWinLib(outputOptions, command);
#elif UNITY_STANDALONE_WIN && !ENABLE_IL2CPP
            return new FfmpegBytesOutputsWinMonoBin(outputOptions, command);
#elif UNITY_STANDALONE_WIN && ENABLE_IL2CPP
            return new FfmpegBytesOutputsWinIL2CPPBin(outputOptions, command);
#elif (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && !ENABLE_IL2CPP
            return new FfmpegBytesOutputsUnixMonoBin(outputOptions, command);
#elif (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && ENABLE_IL2CPP
            return new FfmpegBytesOutputsUnixIL2CPPBin(outputOptions, command);
#elif UNITY_ANDROID //&& FFMPEG_UNITY_USE_PIPE
            return new FfmpegBytesOutputsAndroidPipe(outputOptions, command);
//#elif UNITY_ANDROID && !FFMPEG_UNITY_USE_PIPE
//            return new FfmpegBytesOutputsAndroidMemory(outputOptions, command);
#elif UNITY_IOS //&& FFMPEG_UNITY_USE_PIPE
            return new FfmpegBytesOutputsIOSPipe(outputOptions, command);
//#elif UNITY_IOS && !FFMPEG_UNITY_USE_PIPE
//            return new FfmpegBytesOutputsIOSMemory(outputOptions, command);
#else
            return null;
#endif
        }

        string[] outputOptions_;

        protected FfmpegCommand CommandObj
        {
            get;
            private set;
        }

        protected List<byte[]>[] OutputBytes
        {
            get;
            set;
        } = null;

        string[] outputPipeNames_;

        protected List<Thread> Threads
        {
            get;
            set;
        } = new List<Thread>();

        protected bool IsEnd
        {
            get;
            set;
        } = false;

        void resetOutput()
        {
            OutputBytes = new List<byte[]>[outputOptions_.Length];
            for (int loop = 0; loop < OutputBytes.Length; loop++)
            {
                OutputBytes[loop] = new List<byte[]>();
            }
        }

        public FfmpegBytesOutputs(string[] outputOptions, FfmpegCommand command)
        {
            outputOptions_ = outputOptions;
            CommandObj = command;

            resetOutput();
        }

        protected abstract string GenerateFileName();

        public string BuildAndStart()
        {
            string options = " ";

            outputPipeNames_ = new string[outputOptions_.Length];

            for (int loop = 0; loop < outputOptions_.Length; loop++)
            {
                outputPipeNames_[loop] = GenerateFileName();

                int streamId = loop;
                var thread = new Thread(() => { Read(outputPipeNames_[streamId], streamId); });
                thread.Start();
                Threads.Add(thread);
            }

            for (int loop = 0; loop < outputOptions_.Length; loop++)
            {
                options += " " + outputOptions_[loop] + " \"" + outputPipeNames_[loop] + "\" ";
            }

            return options;
        }

        protected void StreamRead(BinaryReader reader, int streamId)
        {
            while (!IsEnd)
            {
                try
                {
                    var bytes = reader.ReadBytes(BLOCK_SIZE);
                    lock (OutputBytes[streamId])
                    {
                        OutputBytes[streamId].Add(bytes);
                    }
                }
                catch (Exception)
                {
                    Thread.Sleep(1);
                    continue;
                }
            }
        }

        protected abstract void Read(string pipeFileName, int streamId);

        public byte[] GetOutputBytes(int outputNo = 0)
        {
            if (OutputBytes[outputNo].Count <= 0)
            {
                return null;
            }

            byte[] ret;
            lock (OutputBytes[outputNo])
            {
                ret = OutputBytes[outputNo][0];
                OutputBytes[outputNo].RemoveAt(0);
            }

            return ret;
        }

        public void Dispose()
        {
            IsEnd = true;

            foreach (var thread in Threads)
            {
                bool exited = thread.Join(1);
                while (!exited && CommandObj.IsRunning)
                {
                    exited = thread.Join(1);
                }
                if (!exited && !CommandObj.IsRunning)
                {
                    thread.Abort();
                }
            }
            Threads.Clear();
        }

        public interface IOutputControl
        {
            byte[] GetOutputBytes(int outputNo = 0);
            int OutputOptionsCount { get; }
        }
    }
}
