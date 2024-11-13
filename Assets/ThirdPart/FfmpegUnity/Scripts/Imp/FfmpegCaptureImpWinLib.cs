#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX

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
    public class FfmpegCaptureImpWinLib : FfmpegCaptureImpBase
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        const string LIB_NAME = "avformat-59";
        [DllImport(LIB_NAME)]
        static extern IntPtr unitybuf_get_handle_dll(string uri);
        [DllImport(LIB_NAME)]
        static extern int unitybuf_write_dll(IntPtr handle, byte[] buf, int size);
        [DllImport(LIB_NAME)]
        static extern int unitybuf_write_dll(IntPtr handle, IntPtr buf, int size);
        [DllImport(LIB_NAME)]
        static extern int unitybuf_count_dll(IntPtr handle);
        [DllImport(LIB_NAME)]
        static extern int unitybuf_clear_dll(IntPtr handle);
#else
        const string LIB_NAME = "ffmpegDll";
        [DllImport(LIB_NAME, EntryPoint="unitybuf_get_handle_dll2")]
        static extern IntPtr unitybuf_get_handle_dll(string uri);
        [DllImport(LIB_NAME, EntryPoint="unitybuf_write_dll2")]
        static extern int unitybuf_write_dll(IntPtr handle, byte[] buf, int size);
        [DllImport(LIB_NAME, EntryPoint="unitybuf_write_dll2")]
        static extern int unitybuf_write_dll(IntPtr handle, IntPtr buf, int size);
        [DllImport(LIB_NAME, EntryPoint="unitybuf_count_dll2")]
        static extern int unitybuf_count_dll(IntPtr handle);
        [DllImport(LIB_NAME, EntryPoint="unitybuf_clear_dll2")]
        static extern int unitybuf_clear_dll(IntPtr handle);
#endif

        public FfmpegCaptureImpWinLib(FfmpegCaptureCommand captureCommand) : base(captureCommand)
        {
        }

        public override string GenerateCaptureFileNameVideo(int width, int height)
        {
            return "unitybuf:0/0/" + Guid.NewGuid().ToString();
        }

        public override string GenerateCaptureFileNameAudio(int sampleRate, int channels)
        {
            return "unitybuf:0/0/" + Guid.NewGuid().ToString();
        }

        public override bool Reverse
        {
            get
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                return false;
#else
                return SystemInfo.graphicsDeviceType != UnityEngine.Rendering.GraphicsDeviceType.Vulkan;
#endif
            }
        }

        public override void WriteVideo(int streamId, string pipeFileName, int width, int height, Dictionary<int, byte[]> videoBuffers)
        {
            float frameRate = CaptureCommand.CaptureSources[streamId].FrameRate;

            IntPtr handle;
            do
            {
                handle = unitybuf_get_handle_dll(pipeFileName);
                if (handle == IntPtr.Zero)
                {
                    Thread.Sleep(1);
                }
            } while (handle == IntPtr.Zero && !IsEnd);

            while (!IsEnd && !videoBuffers.ContainsKey(streamId))
            {
                Thread.Sleep(1);
            }

            DateTime startTime = DateTime.Now;
            long frameCount = 0;

            while (!IsEnd)
            {
                if (!videoBuffers.ContainsKey(streamId) /*|| unitybuf_count_dll(handle) > 0*/)
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

                frameCount++;
                /*
                AddVideoTotalFrames(streamId);
                if (AudioStreamId >= 0 && AudioTotalSamples > 0)
                {
                    float sleepTime;
                    do
                    {
                        sleepTime = VideoSyncWaitTime(streamId) * 1000f;
                        if (sleepTime < 0f)
                        {
                            sleepTime = 0f;
                        }
                        Thread.Sleep((int)sleepTime);
                    } while (!IsEnd && sleepTime > 0f);
                }
                else
                */
                {
                    double sleepTime;
                    do
                    {
                        sleepTime = (frameCount * 1000.0 / frameRate) - (DateTime.Now - startTime).TotalMilliseconds;
                        if (frameCount > frameRate * DELETE_TIME * 2f)
                        {
                            frameCount -= (int)(frameRate * DELETE_TIME);
                            startTime += TimeSpan.FromSeconds(DELETE_TIME);
                        }
                        //UnityEngine.Debug.Log("sleepTime: " + sleepTime);
                        if (sleepTime < 0.0)
                        {
                            sleepTime = 0.0;
                        }
                        Thread.Sleep((int)sleepTime);
                    } while (!IsEnd && sleepTime > 0.0);
                }
            }

            unitybuf_clear_dll(handle);
        }

        public override void WriteAudio(int streamId, string pipeFileName, int sampleRate, int channels, Dictionary<int, List<float>> audioBuffers)
        {
            float frameRate = DefaultFrameRate;
            int elementSize = Marshal.SizeOf(typeof(float));
            int bufferSize = (int)(sampleRate * channels * elementSize / frameRate);
            IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);

            IntPtr handle;
            do
            {
                handle = unitybuf_get_handle_dll(pipeFileName);
                if (handle == IntPtr.Zero)
                {
                    Thread.Sleep(0);
                }
            } while (handle == IntPtr.Zero && !IsEnd);

            DateTime startTime = default;
            long frameCount = 0;

            while (!IsEnd)
            {
                try
                {
                    if (!audioBuffers.ContainsKey(streamId) || audioBuffers[streamId] == null || audioBuffers[streamId].Count <= bufferSize / elementSize /*|| unitybuf_count_dll(handle) > 1*/)
                    {
                        Thread.Sleep(0);
                        continue;
                    }
                }
                catch (NullReferenceException)
                {
                    Thread.Sleep(0);
                    continue;
                }

                if (startTime == default)
                {
                    startTime = DateTime.Now;
                }

                /*
                int blockSize = audioBuffers[streamId].Count * elementSize;
                if (bufferSize < blockSize)
                {
                    Marshal.FreeHGlobal(bufferPtr);
                    bufferSize = blockSize;
                    bufferPtr = Marshal.AllocHGlobal(bufferSize);
                }
                */
                int blockSize = bufferSize;

                float[] buffer;
                lock (audioBuffers[streamId])
                {
                    buffer = audioBuffers[streamId].GetRange(0, blockSize / elementSize).ToArray();
                }

                Marshal.Copy(buffer, 0, bufferPtr, blockSize / elementSize);
                unitybuf_write_dll(handle, bufferPtr, blockSize);

                lock (audioBuffers[streamId])
                {
                    audioBuffers[streamId].RemoveRange(0, blockSize / elementSize);
                    if (audioBuffers[streamId].Count > bufferSize / elementSize)
                    {
                        audioBuffers[streamId].RemoveRange(0, audioBuffers[streamId].Count - bufferSize / elementSize);
                    }
                }

                //AddAudioTotalSamples(streamId, blockSize / elementSize);
                frameCount++;
                double sleepTime;
                do
                {
                    sleepTime = (frameCount * 1000.0 / frameRate) - (DateTime.Now - startTime).TotalMilliseconds;
                    if (frameCount > frameRate * DELETE_TIME * 2f)
                    {
                        frameCount -= (int)(frameRate * DELETE_TIME);
                        startTime += TimeSpan.FromSeconds(DELETE_TIME);
                    }
                    //UnityEngine.Debug.Log("sleepTime: " + sleepTime);
                    if (sleepTime < 0.0)
                    {
                        sleepTime = 0.0;
                    }
                    Thread.Sleep((int)sleepTime);
                } while (!IsEnd && sleepTime > 0.0);
            }

            Marshal.FreeHGlobal(bufferPtr);

            unitybuf_clear_dll(handle);
        }
    }
}

#endif