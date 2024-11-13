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
    public class FfmpegPlayerImpAndroidMemory : FfmpegPlayerImpAndroidBase
    {
        Dictionary<int, string> pipeFileNames_ = new Dictionary<int, string>();
        List<IntPtr> handles_ = new List<IntPtr>();

        [DllImport("ffmpegkit")]
        static extern IntPtr unitybuf_get_handle_dll(string uri);
        [DllImport("ffmpegkit")]
        static extern int unitybuf_read_dll(IntPtr handle, IntPtr buf, int size);
        [DllImport("ffmpegkit")]
        static extern int unitybuf_clear_dll(IntPtr handle);
        [DllImport("ffmpegkit")]
        static extern int unitybuf_count_dll(IntPtr handle);

        public FfmpegPlayerImpAndroidMemory(FfmpegPlayerCommand playerCommand) : base(playerCommand)
        {
        }

        public override bool IsEOF
        {
            get;
            protected set;
        } = false;

        public override string BuildBeginOptions(string path)
        {
            return " -re " + base.BuildBeginOptions(path);
        }

        public override string BuildVideoOptions(int streamId, int width, int height)
        {
            string fileName = "unitybuf:" + (width * height * 4) + "/2/" + Guid.NewGuid().ToString();
            if (pipeFileNames_.ContainsKey(streamId))
            {
                pipeFileNames_.Remove(streamId);
            }
            pipeFileNames_.Add(streamId, fileName);

            unitybuf_get_handle_dll(fileName); // Dummy

            return " -f rawvideo -pix_fmt rgba \"" + fileName + "\" ";
        }

        public override string BuildAudioOptions(int streamId, int sampleRate, int channels)
        {
            //string fileName = "unitybuf:" + (sampleRate * channels * 4 / 50) + "/0/" + Guid.NewGuid().ToString();
            string fileName = "unitybuf:0/0/" + Guid.NewGuid().ToString();
            if (pipeFileNames_.ContainsKey(streamId))
            {
                pipeFileNames_.Remove(streamId);
            }
            pipeFileNames_.Add(streamId, fileName);

            unitybuf_get_handle_dll(fileName); // Dummy

            return " -f f32le \"" + fileName + "\" ";
        }

        public override void CleanBuf()
        {
            foreach (var handle in handles_)
            {
                unitybuf_clear_dll(handle);
            }
            handles_.Clear();

            pipeFileNames_.Clear();
        }

        public override void ReadVideo(int streamId, int width, int height)
        {
            byte[] buf = new byte[width * height * 4];

            int elementSize = Marshal.SizeOf(typeof(byte));
            IntPtr bufPtr = Marshal.AllocHGlobal(buf.Length * elementSize);

            while (!PlayerCommand.IsRunning && !IsEnd)
            {
                Thread.Sleep(1);
            }

            while (!PlayerCommand.IsAlreadyStart && !IsEnd)
            {
                Thread.Sleep(1);
            }

            IntPtr handle = IntPtr.Zero;
            while (handle == IntPtr.Zero && !IsEnd && PlayerCommand.IsRunning && PlayerCommand.IsAlreadyStart)
            {
                handle = unitybuf_get_handle_dll(pipeFileNames_[streamId]);
                if (handle == IntPtr.Zero)
                {
                    Thread.Sleep(1);
                }
            }

            if (handle != IntPtr.Zero)
            {
                handles_.Add(handle);
            }

            while (!IsEnd && PlayerCommand.IsRunning && PlayerCommand.IsAlreadyStart)
            {
                /*
                if (unitybuf_count_dll(handle) <= 0)
                {
                    Thread.Sleep(1);
                    continue;
                }
                */

                int readSize = unitybuf_read_dll(handle, bufPtr, buf.Length);
                if (readSize == -('E' | ('O' << 8) | ('F' << 16) | (' ' << 24)))
                {
                    break;
                }
                else if (readSize <= 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                Marshal.Copy(bufPtr, buf, 0, readSize);

                var newVideoBuffer = new byte[buf.Length];
                for (int y = 0; y < height; y++)
                {
                    Array.Copy(buf, y * width * 4,
                        newVideoBuffer, (height - y - 1) * width * 4,
                        width * 4);
                }

                PlayerCommand.SetVideoBuffer(streamId, newVideoBuffer);

                PlayerCommand.StopPerFrameFunc(streamId);
            }

            Marshal.FreeHGlobal(bufPtr);

            IsEOF = true;
        }

        public override void ReadAudio(int streamId, int sampleRate, int channels)
        {
            int size = 1024; //sampleRate * channels * 4 / 50;
            int elementSizeFloat = Marshal.SizeOf(typeof(float));
            IntPtr bufPtr = Marshal.AllocHGlobal(size);

            while (!PlayerCommand.IsRunning && !IsEnd)
            {
                Thread.Sleep(1);
            }

            while (!PlayerCommand.IsAlreadyStart && !IsEnd)
            {
                Thread.Sleep(1);
            }

            IntPtr handle = IntPtr.Zero;
            while (handle == IntPtr.Zero && !IsEnd && PlayerCommand.IsRunning && PlayerCommand.IsAlreadyStart)
            {
                handle = unitybuf_get_handle_dll(pipeFileNames_[streamId]);
                if (handle == IntPtr.Zero)
                {
                    Thread.Sleep(1);
                }
            }

            if (handle != IntPtr.Zero)
            {
                handles_.Add(handle);
            }

            while (!IsEnd && PlayerCommand.IsRunning && PlayerCommand.IsAlreadyStart)
            {
                int readSize = unitybuf_read_dll(handle, bufPtr, size);

                if (readSize == -('E' | ('O' << 8) | ('F' << 16) | (' ' << 24)))
                {
                    break;
                }
                else if (readSize <= 0)
                {
                    Thread.Sleep(1);
                }
                else
                {
                    float[] floatBuffer = new float[readSize / elementSizeFloat];
                    Marshal.Copy(bufPtr, floatBuffer, 0, readSize / elementSizeFloat);
                    PlayerCommand.AddAudioBuffer(streamId, floatBuffer);
                }
            }

            Marshal.FreeHGlobal(bufPtr);

            IsEOF = true;
        }
    }
}

#endif