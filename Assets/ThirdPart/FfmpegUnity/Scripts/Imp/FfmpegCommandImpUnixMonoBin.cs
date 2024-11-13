using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegCommandImpUnixMonoBin : FfmpegCommandImpBase
    {
        StreamReader stdErr_ = null;
        Thread stdErrThread_ = null;
        List<string> stdErrList_ = new List<string>();
        Process process_ = null;

        bool sendQCommand_ = false;

        int returnCode_ = 0;
        public override int ReturnCode
        {
            get
            {
                return returnCode_;
            }
        }

        public FfmpegCommandImpUnixMonoBin(FfmpegCommand command) : base(command)
        {
        }

        public override IEnumerator StartFfmpegCoroutine(string options)
        {
            options = ParseOptions(options);

            string fileName = "ffmpeg";
#if UNITY_EDITOR_OSX
#if !FFMPEG_UNITY_USE_OUTER_MAC
            fileName = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Mac/ffmpeg");
#else
            if (File.Exists("/usr/local/bin/ffmpeg"))
            {
                fileName = "/usr/local/bin/ffmpeg";
            }
#endif
#elif UNITY_EDITOR_LINUX
#if !FFMPEG_UNITY_USE_OUTER_LINUX
            fileName = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Linux/ffmpeg");
#endif
#elif (UNITY_STANDALONE_OSX && !FFMPEG_UNITY_USE_OUTER_MAC) || (UNITY_STANDALONE_LINUX && !FFMPEG_UNITY_USE_OUTER_LINUX)
            fileName = Application.streamingAssetsPath + "/_FfmpegUnity_temp/ffmpeg";
#endif

            ProcessStartInfo psInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                Arguments = options,
            };

            process_ = Process.Start(psInfo);
            stdErr_ = process_.StandardError;
            stdErrThread_ = new Thread(() =>
            {
                while (!LoopExit)
                {
                    try
                    {
                        Task<string> strTask = stdErr_.ReadLineAsync();
                        while (!strTask.IsCompleted && LoopExit)
                        {
                            Thread.Sleep(16);
                        }
                        if (LoopExit)
                        {
                            break;
                        }
                        string str = strTask.Result;
                        if (str == null)
                        {
                            LoopExit = true;
                            break;
                        }
                        if (str.StartsWith("Press [q] to stop"))
                        {
                            sendQCommand_ = true;
                        }
                        stdErrList_.Add(str);
                    }
                    catch (Exception)
                    {
                        LoopExit = true;
                        break;
                    }
                }
            });
            stdErrThread_.Start();
            CommandObj.StartCoroutine(setReturnCode());

            yield break;
        }

        IEnumerator setReturnCode()
        {
            while (!process_.HasExited)
            {
                yield return null;
            }

            returnCode_ = process_.ExitCode;
        }

        public override void StopFfmpeg()
        {
            if (process_ != null)
            {
                if (!process_.HasExited && sendQCommand_)
                {
                    process_.StandardInput.Write("q");
                    process_.WaitForExit();
                }
                if (!process_.HasExited)
                {
                    process_.CloseMainWindow();
                    process_.WaitForExit();
                }
                returnCode_ = process_.ExitCode;
                process_.Dispose();
                process_ = null;
            }

            if (stdErrThread_ != null)
            {
                stdErrThread_.Join();
                stdErrThread_ = null;
            }

            if (stdErr_ != null)
            {
                stdErr_.Dispose();
                stdErr_ = null;
            }

            sendQCommand_ = false;
        }

        public override bool IsRunning
        {
            get
            {
                return !LoopExit && IsAlreadyBuild;
            }
        }

        public override string StdErrLine(List<string> stdErrListForGetLine)
        {
            string ret;
            if (stdErrList_ == null)
            {
                return null;
            }
            lock (stdErrList_)
            {
                if (stdErrList_.Count <= 0)
                {
                    return null;
                }
                ret = stdErrList_[0];
                stdErrList_.RemoveAt(0);
            }
            stdErrListForGetLine.Add(ret);
            return ret;
        }
    }
}
