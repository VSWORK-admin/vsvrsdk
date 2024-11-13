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
    public abstract class FfmpegBytesInputs : IDisposable
    {
        protected const int BUFFER_SIZE = 5000000;
        protected const int BLOCK_SIZE = 1024;

        public static FfmpegBytesInputs GetNewInstance(string[] inputOptions, FfmpegCommand command)
        {
#if UNITY_EDITOR_WIN && FFMPEG_UNITY_USE_BINARY_WIN
            return new FfmpegBytesInputsWinMonoBin(inputOptions, command);
#elif UNITY_EDITOR_WIN && !FFMPEG_UNITY_USE_BINARY_WIN || UNITY_EDITOR_LINUX && !FFMPEG_UNITY_USE_BINARY_LINUX
            return new FfmpegBytesInputsWinLib(inputOptions, command);
#elif (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC
            return new FfmpegBytesInputsIOSMemory(inputOptions, command);
#elif UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            return new FfmpegBytesInputsUnixMonoBin(inputOptions, command);
#elif UNITY_STANDALONE_WIN && !FFMPEG_UNITY_USE_BINARY_WIN || UNITY_STANDALONE_LINUX && !FFMPEG_UNITY_USE_BINARY_LINUX
            return new FfmpegBytesInputsWinLib(inputOptions, command);
#elif UNITY_STANDALONE_WIN && !ENABLE_IL2CPP
            return new FfmpegBytesInputsWinMonoBin(inputOptions, command);
#elif UNITY_STANDALONE_WIN && ENABLE_IL2CPP
            return new FfmpegBytesInputsWinIL2CPPBin(inputOptions, command);
#elif (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && !ENABLE_IL2CPP
            return new FfmpegBytesInputsUnixMonoBin(inputOptions, command);
#elif (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && ENABLE_IL2CPP
            return new FfmpegBytesInputsUnixIL2CPPBin(inputOptions, command);
#elif UNITY_ANDROID //&& FFMPEG_UNITY_USE_PIPE
            return new FfmpegBytesInputsAndroidPipe(inputOptions, command);
//#elif UNITY_ANDROID && !FFMPEG_UNITY_USE_PIPE
//            return new FfmpegBytesInputsAndroidMemory(inputOptions, command);
#elif UNITY_IOS //&& FFMPEG_UNITY_USE_PIPE
            return new FfmpegBytesInputsIOSPipe(inputOptions, command);
//#elif UNITY_IOS && !FFMPEG_UNITY_USE_PIPE
//            return new FfmpegBytesInputsIOSMemory(inputOptions, command);
#else
            return null;
#endif
        }

        public bool IsEmpty
        {
            get
            {
                if (InputBytes == null)
                {
                    return true;
                }

                for (int loop = 0; loop < InputBytes.Length; loop++)
                {
                    if (InputBytes[loop].Count > 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        string[] inputOptions_;

        protected List<byte[]>[] InputBytes
        {
            get;
            set;
        } = null;

        protected bool IsStop
        {
            get;
            set;
        } = false;

        protected List<Thread> Threads
        {
            get;
            private set;
        } = new List<Thread>();

        protected FfmpegCommand CommandObj
        {
            get;
            private set;
        }

        public FfmpegBytesInputs(string[] inputOptions, FfmpegCommand command)
        {
            inputOptions_ = inputOptions;
            CommandObj = command;

            resetInputBytes();
        }

        void resetInputBytes()
        {
            InputBytes = new List<byte[]>[inputOptions_.Length];
            for (int loop = 0; loop < InputBytes.Length; loop++)
            {
                InputBytes[loop] = new List<byte[]>();
            }
        }

        public void AddInputBytes(byte[] bytes, int inputNo = 0)
        {
            //lock (inputBytes_[inputNo])
            {
                InputBytes[inputNo].Add(bytes);
            }
        }

        protected abstract string GenerateFileName();

        public string BuildAndStart()
        {
            string options = "";

            for (int loop = 0; loop < inputOptions_.Length; loop++)
            {
                options += " " + inputOptions_[loop];

                string fileName = GenerateFileName();

                options += " -i \"" + fileName + "\" ";

                int index = loop;
                var thread = new Thread(() => {
                    string fileName2 = fileName;
                    int index2 = index;
                    Write(fileName2, index2);
                });
                thread.Start();
                Threads.Add(thread);
            }

            return options;
        }

        public string Restart()
        {
            IsStop = true;

            foreach (var thread in Threads)
            {
                thread.Join();
            }
            Threads.Clear();

            IsStop = false;

            string ret = BuildAndStart();

            return ret;
        }

        public void Dispose()
        {
            IsStop = true;

            foreach (var thread in Threads)
            {
                /*
                bool exited = thread.Join(1);
                while (!exited && CommandObj.IsRunning)
                {
                    exited = thread.Join(1);
                }
                if (!exited && !CommandObj.IsRunning)
                {
                    thread.Abort();
                }
                */
                thread.Join();
            }
            Threads.Clear();
        }

        protected void StreamWrite(BinaryWriter writer, int streamId)
        {
            while (!IsStop)
            {
                if (InputBytes[streamId].Count <= 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                byte[] buffer;
                //lock (inputBytes_[streamId])
                {
                    buffer = InputBytes[streamId][0];
                    InputBytes[streamId].RemoveAt(0);
                }

                try
                {
                    if (writer.BaseStream.CanWrite && buffer != null)
                    {
                        writer.Write(buffer);
                    }
                }
                catch (IOException)
                {
                    IsStop = true;
                    break;
                }
            }
        }

        protected abstract void Write(string pipeFileName, int streamId);

        public interface IInputControl
        {
            void AddInputBytes(byte[] bytes, int inputNo = 0);

            bool InputBytesIsEmpty { get; }
        }
    }
}
