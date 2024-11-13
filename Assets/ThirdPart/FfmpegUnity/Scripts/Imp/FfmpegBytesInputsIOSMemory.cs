//#if UNITY_IOS || ((UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC)
#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC

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
    public class FfmpegBytesInputsIOSMemory : FfmpegBytesInputs
    {
#if UNITY_IOS && !UNITY_EDITOR_OSX
        const string IMPORT_NAME = "__Internal";
#else
        const string IMPORT_NAME = "FfmpegUnityMacPlugin";
#endif

        [DllImport(IMPORT_NAME)]
        static extern IntPtr unitybuf_get_handle_dll(string uri);
        [DllImport(IMPORT_NAME)]
        static extern int unitybuf_write_dll(IntPtr handle, byte[] buf, int size);
        [DllImport(IMPORT_NAME)]
        static extern int unitybuf_count_dll(IntPtr handle);
        [DllImport(IMPORT_NAME)]
        static extern int unitybuf_clear_dll(IntPtr handle);

        public FfmpegBytesInputsIOSMemory(string[] inputOptions, FfmpegCommand command) : base(inputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            return "unitybuf:0/0/" + Guid.NewGuid().ToString();
        }

        protected override void Write(string pipeFileName, int streamId)
        {
            IntPtr handle;
            do
            {
                handle = unitybuf_get_handle_dll(pipeFileName);
                if (handle == IntPtr.Zero)
                {
                    Thread.Sleep(1);
                }
            } while (handle == IntPtr.Zero && !IsStop);

            while (!IsStop)
            {
                if (InputBytes[streamId].Count <= 0 || unitybuf_count_dll(handle) > 1)
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

                if (buffer != null)
                {
                    unitybuf_write_dll(handle, buffer, buffer.Length);
                }
            }

            unitybuf_clear_dll(handle);
        }
    }
}

#endif