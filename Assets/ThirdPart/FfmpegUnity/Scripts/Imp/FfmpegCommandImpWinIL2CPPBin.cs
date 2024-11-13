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
    public class FfmpegCommandImpWinIL2CPPBin : FfmpegCommandImpBase
    {
        StreamReader stdErr_ = null;
        Thread stdErrThread_ = null;
        List<string> stdErrList_ = new List<string>();
        public FfmpegExecuteIL2CPPWin ExecuteObj
        {
            get;
            private set;
        } = null;
        List<FfmpegExecuteIL2CPPWin.PipeOption> pipeOptionsList_ = new List<FfmpegExecuteIL2CPPWin.PipeOption>();
        public void AddPipeOptions(FfmpegExecuteIL2CPPWin.PipeOption pipeOption)
        {
            pipeOptionsList_.Add(pipeOption);
        }
        public int PipeOptionsCount
        {
            get
            {
                return pipeOptionsList_.Count;
            }
        }
        List<FfmpegExecuteIL2CPPWin.PipeOption> lastPipeOptions_ = new List<FfmpegExecuteIL2CPPWin.PipeOption>();
        public void AddLastPipeOptions(FfmpegExecuteIL2CPPWin.PipeOption pipeOption)
        {
            lastPipeOptions_.Add(pipeOption);
        }
        public int LastPipeOptionsCount
        {
            get
            {
                return lastPipeOptions_.Count;
            }
        }

        public FfmpegCommandImpWinIL2CPPBin(FfmpegCommand command) : base(command)
        {
        }

        public override IEnumerator StartFfmpegCoroutine(string options)
        {
            options = ParseOptions(options);

            ExecuteObj = new FfmpegExecuteIL2CPPWin();
            //if (IsGetStdErr)
            {
                var pipeOption = new FfmpegExecuteIL2CPPWin.PipeOption();
                pipeOption.BlockSize = -1;
                pipeOption.BufferSize = 1024;
                pipeOption.PipeName = "FfmpegUnity_" + Guid.NewGuid().ToString();
                pipeOption.StdMode = 2;
                pipeOptionsList_.Add(pipeOption);
            }
            int streamPosId = pipeOptionsList_.Count - 1;
            if (lastPipeOptions_.Count > 0)
            {
                pipeOptionsList_.AddRange(lastPipeOptions_);
            }
#if FFMPEG_UNITY_USE_OUTER_WIN
            const bool useBuiltIn = false;
#else
            const bool useBuiltIn = true;
#endif
            ExecuteObj.Execute(useBuiltIn, "ffmpeg", options, pipeOptionsList_.ToArray());
            while (ExecuteObj.GetStream(streamPosId) == null)
            {
                yield return null;
            }
            stdErr_ = new StreamReader(ExecuteObj.GetStream(streamPosId));
            stdErrThread_ = new Thread(() =>
            {
                while (!LoopExit)
                {
                    Task<string> strTask = stdErr_.ReadLineAsync();
                    while (!strTask.IsCompleted && !LoopExit)
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
                    stdErrList_.Add(str);
                }
            });
            stdErrThread_.Start();
        }

        public override void StopFfmpeg()
        {
            if (ExecuteObj != null)
            {
                ExecuteObj.Dispose();
                ExecuteObj = null;
            }
            pipeOptionsList_.Clear();
            lastPipeOptions_.Clear();
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
