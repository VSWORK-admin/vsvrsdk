#if false//UNITY_ANDROID

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegCaptureImpAndroidMemory : FfmpegCaptureImpBase
    {
        [DllImport("ffmpegkit")]
        static extern IntPtr unitybuf_get_handle_dll(string uri);
        [DllImport("ffmpegkit")]
        static extern int unitybuf_write_dll(IntPtr handle, byte[] buf, int size);
        [DllImport("ffmpegkit")]
        static extern int unitybuf_write_dll(IntPtr handle, IntPtr buf, int size);
        [DllImport("ffmpegkit")]
        static extern int unitybuf_count_dll(IntPtr handle);
        [DllImport("ffmpegkit")]
        static extern int unitybuf_clear_dll(IntPtr handle);

        public FfmpegCaptureImpAndroidMemory(FfmpegCaptureCommand captureCommand) : base(captureCommand)
        {
        }

        public override string GenerateCaptureFileNameVideo(int width, int height)
        {
            return "unitybuf:0/0/" + Guid.NewGuid().ToString();
        }

        public override string GenerateCaptureFileNameAudio(int sampleRate, int channels)
        {
            return "unitybuf:" + (sampleRate * channels * 4 / 50) + "/0/" + Guid.NewGuid().ToString();
        }

        public override bool Reverse
        {
            get
            {
#if UNITY_2020_1_OR_NEWER
                return SystemInfo.graphicsDeviceType != UnityEngine.Rendering.GraphicsDeviceType.Vulkan;
#else
                return false;
#endif
            }
        }

        public override void WriteVideo(int streamId, string pipeFileName, int width, int height, Dictionary<int, byte[]> videoBuffers)
        {
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
                if (!videoBuffers.ContainsKey(streamId) || unitybuf_count_dll(handle) > 1)
                {
                    Thread.Sleep(1);
                    continue;
                }

                byte[] buffer;
                lock (videoBuffers)
                {
                    buffer = videoBuffers[streamId];
                }

                unitybuf_write_dll(handle, buffer, buffer.Length);
            }

            unitybuf_clear_dll(handle);
        }

        public override void WriteAudio(int streamId, string pipeFileName, int sampleRate, int channels, Dictionary<int, List<float>> audioBuffers)
        {
            int blockSize = int.Parse(pipeFileName.Replace("unitybuf:", "").Split('/')[0]);
            int elementSize = Marshal.SizeOf(typeof(float));
            IntPtr bufferPtr = Marshal.AllocHGlobal(blockSize);

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
                if (!audioBuffers.ContainsKey(streamId) || audioBuffers[streamId] == null || audioBuffers[streamId].Count < blockSize / elementSize || unitybuf_count_dll(handle) > 1)
                {
                    Thread.Sleep(1);
                    continue;
                }

                lock (audioBuffers[streamId])
                {
                    float[] buffer = audioBuffers[streamId].GetRange(0, blockSize / elementSize).ToArray();

                    Marshal.Copy(buffer, 0, bufferPtr, blockSize / elementSize);

                    int removeLength = unitybuf_write_dll(handle, bufferPtr, blockSize);

                    if (removeLength > 0)
                    {
                        audioBuffers[streamId].RemoveRange(0, removeLength / elementSize);
                    }
                }
            }

            Marshal.FreeHGlobal(bufferPtr);

            unitybuf_clear_dll(handle);
        }
    }
}

#endif