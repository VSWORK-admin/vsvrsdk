#if UNITY_IOS || ((UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegCommandImpIOS : FfmpegCommandImpBase
    {
#if UNITY_IOS && !UNITY_EDITOR_OSX
        const string IMPORT_NAME = "__Internal";
#else
        const string IMPORT_NAME = "FfmpegUnityMacPlugin";
#endif

        [DllImport(IMPORT_NAME)]
        static extern void ffmpeg_setup();
        [DllImport(IMPORT_NAME)]
        static extern IntPtr ffmpeg_executeAsync(string command);
        [DllImport(IMPORT_NAME)]
        static extern void ffmpeg_cancel(IntPtr session);
        [DllImport(IMPORT_NAME)]
        static extern int ffmpeg_getOutputLength(IntPtr session);
        [DllImport(IMPORT_NAME)]
        static extern void ffmpeg_getOutput(IntPtr session, int startIndex, IntPtr output, int outputLength);
        [DllImport(IMPORT_NAME)]
        static extern bool ffmpeg_isRunnning(IntPtr session);
        [DllImport(IMPORT_NAME)]
        static extern float ffmpeg_getTime(IntPtr session);
        [DllImport(IMPORT_NAME)]
        static extern double ffmpeg_getSpeed(IntPtr session);
        [DllImport(IMPORT_NAME)]
        static extern int ffmpeg_getReturnCode(IntPtr session);

        IntPtr session_ = IntPtr.Zero;
        string[] outputAllLine_ = new string[0];
        int outputAllLinePos_ = 0;
        int stdErrPos_ = 0;

        public FfmpegCommandImpIOS(FfmpegCommand command) : base(command)
        {
        }

        public override IEnumerator StartFfmpegCoroutine(string options)
        {
            options = ParseOptions(options);

            ffmpeg_setup();

            session_ = ffmpeg_executeAsync(options);

            yield break;
        }

        public override void StopFfmpeg()
        {
            if (session_ != IntPtr.Zero)
            {
                if (IsRunning)
                {
                    ffmpeg_cancel(session_);
                    /*
                    while (IsRunning)
                    {
                        Thread.Sleep(1);
                    }
                    */
                }
                session_ = IntPtr.Zero;
            }
        }

        public override bool IsRunning
        {
            get
            {
                if (session_ == IntPtr.Zero)
                {
                    return false;
                }
                return ffmpeg_isRunnning(session_);
            }
        }

        void getStdErr()
        {
            if (outputAllLine_.Length - 1 <= outputAllLinePos_)
            {
                int allLength = ffmpeg_getOutputLength(session_);
                if (allLength <= stdErrPos_)
                {
                    return;
                }

                int allocSize = allLength + 1 - stdErrPos_;
                IntPtr hglobal = Marshal.AllocHGlobal(allocSize);

                ffmpeg_getOutput(session_, stdErrPos_, hglobal, allocSize);

                string outputStr = Marshal.PtrToStringAuto(hglobal);
                if (outputAllLine_.Length > 0)
                {
                    outputStr = outputAllLine_[outputAllLine_.Length - 1] + outputStr;
                }
                outputAllLine_ = outputStr.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                outputAllLinePos_ = 0;

                Marshal.FreeHGlobal(hglobal);
            }
        }

        public override string StdErrLine(List<string> stdErrListForGetLine)
        {
            if (session_ == IntPtr.Zero)
            {
                return null;
            }

            getStdErr();

            if (outputAllLine_.Length - 1 <= outputAllLinePos_)
            {
                return null;
            }

            string ret = outputAllLine_[outputAllLinePos_];
            outputAllLinePos_++;
            /*
            if (outputAllLine_.Length - 1 == outputAllLinePos_)
            {
                stdErrPos_ += ret.Length;
            }
            else
            */
            {
                stdErrPos_ += ret.Length + Environment.NewLine.Length;
            }

            stdErrListForGetLine.Add(ret);
            return ret;
        }

        public override bool IsGetStatusFromImp
        {
            get
            {
                return true;
            }
        }

        public override float Time
        {
            get
            {
                if (session_ == IntPtr.Zero)
                {
                    return -1f;
                }
                return ffmpeg_getTime(session_);
            }
        }

        public override double Speed
        {
            get
            {
                if (session_ == IntPtr.Zero)
                {
                    return -1.0;
                }
                return ffmpeg_getSpeed(session_);
            }
        }

        public override int ReturnCode
        {
            get
            {
                if (session_ == IntPtr.Zero)
                {
                    return 0;
                }
                return ffmpeg_getReturnCode(session_);
            }
        }
    }
}

#endif