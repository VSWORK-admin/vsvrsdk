#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegCommandImpWinLib : FfmpegCommandImpBase
    {
        [DllImport("libffmpegDll")]
        static extern int ffmpeg_start(int argc, IntPtr argv, int id, string file_path);
        [DllImport("libffmpegDll")]
        static extern void ffmpeg_stop(int id);
        [DllImport("libffmpegDll")]
        static extern void ffmpeg_force_stop(int id);
        [DllImport("libffmpegDll")]
        static extern int ffmpeg_is_running(int id);

        int currentId_ = -1;
        static int nextId_ = 0;

        string logFilePath_;

        Thread ffmpegThread_ = null;
        StreamReader streamReader_ = null;

        bool isForceStop_ = false;
        public override bool IsForceStop
        {
            get
            {
                return isForceStop_;
            }
            set
            {
                isForceStop_ = value;
            }
        }

        bool isRunning_ = false;
        bool isStop_ = false;

        int returnCode_ = 0;
        public override int ReturnCode
        {
            get
            {
                return returnCode_;
            }
        }

        public FfmpegCommandImpWinLib(FfmpegCommand command) : base(command)
        {
            isForceStop_ = command is FfmpegPlayerCommand;
        }

        public override IEnumerator StartFfmpegCoroutine(string options)
        {
            options = ParseOptions(options);

            if (nextId_ == 0 || nextId_ == int.MaxValue)
            {
                nextId_ = UnityEngine.Random.Range(0, int.MaxValue);
            }
            currentId_ = nextId_;
            nextId_++;
            string[] splitedOptions = CommandObj.CommandSplit(options);
            splitedOptions = (new[] { "ffmpeg" }).Concat(splitedOptions).ToArray();

            logFilePath_ = null;
            if (CommandObj.PrintStdErr || CommandObj.IsGetStdErr)
            {
                logFilePath_ = Application.temporaryCachePath + "/" + Guid.NewGuid().ToString() + ".log";
            }

            ffmpegThread_ = new Thread(() =>
            {
                int elementSize = Marshal.SizeOf(typeof(IntPtr));

                IntPtr splitedOptionsPtr = Marshal.AllocHGlobal(splitedOptions.Length * elementSize);

                List<IntPtr> splitedOptionsStrPtr = new List<IntPtr>();
                foreach (string str in splitedOptions)
                {
                    IntPtr strPtr = Marshal.StringToHGlobalAnsi(str.Replace("\"", "").Replace("\'", ""));
                    Marshal.WriteIntPtr(splitedOptionsPtr, splitedOptionsStrPtr.Count * elementSize, strPtr);
                    splitedOptionsStrPtr.Add(strPtr);
                }

                isRunning_ = true;
                returnCode_ = ffmpeg_start(splitedOptions.Length, splitedOptionsPtr, currentId_, logFilePath_);
                isStop_ = true;

                foreach (IntPtr strPtr in splitedOptionsStrPtr)
                {
                    Marshal.FreeHGlobal(strPtr);
                }

                Marshal.FreeHGlobal(splitedOptionsPtr);
            });
            ffmpegThread_.Start();

            if (logFilePath_ != null)
            {
                bool loopFlag;
                do
                {
                    yield return null;

                    loopFlag = false;
                    try
                    {
                        var stream = new FileStream(logFilePath_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        streamReader_ = new StreamReader(stream);
                    }
                    catch (IOException)
                    {
                        loopFlag = true;
                    }
                } while (loopFlag);
            }
        }

        public override void StopFfmpeg()
        {
            if (streamReader_ != null)
            {
                streamReader_.Dispose();
                streamReader_ = null;
            }

            if (currentId_ >= 0)
            {
                int id = currentId_;
                currentId_ = -1;
                if (ffmpeg_is_running(id) != 0)
                {
                    if (isForceStop_)
                    {
                        ffmpeg_force_stop(id);
                    }
                    else
                    {
                        ffmpeg_stop(id);
                    }
                }
            }

            if (ffmpegThread_ != null)
            {
                ffmpegThread_.Join();
                ffmpegThread_ = null;
            }
        }

        public override bool IsRunning
        {
            get
            {
                if (isRunning_)
                {
                    if (isStop_)
                    {
                        isRunning_ = false;
                        isStop_ = false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override string StdErrLine(List<string> stdErrListForGetLine)
        {
            if (streamReader_ != null)
            {
                string line = streamReader_.ReadLine();
                stdErrListForGetLine.Add(line);
                return line;
            }
            return null;
        }
    }
}

#endif