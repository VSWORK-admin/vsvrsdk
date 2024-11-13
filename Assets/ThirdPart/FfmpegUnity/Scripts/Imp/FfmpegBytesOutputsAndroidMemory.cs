#if false//UNITY_ANDROID

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
    public class FfmpegBytesOutputsAndroidMemory : FfmpegBytesOutputs
    {
        [DllImport("ffmpegkit")]
        static extern IntPtr unitybuf_get_handle_dll(string uri);
        [DllImport("ffmpegkit")]
        static extern int unitybuf_read_dll(IntPtr handle, byte[] buf, int size);
        [DllImport("ffmpegkit")]
        static extern int unitybuf_clear_dll(IntPtr handle);

        public FfmpegBytesOutputsAndroidMemory(string[] outputOptions, FfmpegCommand command) : base(outputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            return "unitybuf:0/0/" + Guid.NewGuid().ToString();
        }

        protected override void Read(string pipeFileName, int streamId)
        {
            byte[] bytes = new byte[BLOCK_SIZE];

            Thread.Sleep(50);

            IntPtr handle;
            do
            {
                handle = unitybuf_get_handle_dll(pipeFileName);
                if (handle == IntPtr.Zero)
                {
                    Thread.Sleep(1);
                }
            } while (handle == IntPtr.Zero && !IsEnd);

            while (!IsEnd)
            {
                int readSize = unitybuf_read_dll(handle, bytes, bytes.Length);

                if (readSize == -('E' | ('O' << 8) | ('F' << 16) | (' ' << 24)))
                {
                    break;
                }
                else if (readSize <= 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                lock (OutputBytes[streamId])
                {
                    OutputBytes[streamId].Add(bytes.ToList().GetRange(0, readSize).ToArray());
                }
            }

            unitybuf_clear_dll(handle);
        }
    }
}

#endif