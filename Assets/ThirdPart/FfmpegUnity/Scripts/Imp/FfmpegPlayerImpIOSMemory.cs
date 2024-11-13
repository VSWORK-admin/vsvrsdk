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
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegPlayerImpIOSMemory : FfmpegPlayerImpIOSBase
    {
        Dictionary<int, string> pipeFileNames_ = new Dictionary<int, string>();

#if UNITY_IOS && !UNITY_EDITOR_OSX
        const string IMPORT_NAME = "__Internal";
#else
        const string IMPORT_NAME = "FfmpegUnityMacPlugin";
#endif

        [DllImport(IMPORT_NAME)]
        static extern IntPtr unitybuf_get_handle_dll(string uri);
        [DllImport(IMPORT_NAME)]
        static extern int unitybuf_read_dll(IntPtr handle, IntPtr buf, int size);
        [DllImport(IMPORT_NAME)]
        static extern int unitybuf_clear_dll(IntPtr handle);
        [DllImport(IMPORT_NAME)]
        static extern int unitybuf_count_dll(IntPtr handle);

        public FfmpegPlayerImpIOSMemory(FfmpegPlayerCommand playerCommand) : base(playerCommand)
        {
        }

        public override bool IsEOF
        {
            get;
            protected set;
        } = false;

        public override string BuildBeginOptions(string path)
        {
            if (PlayerCommand.SyncFrameRate)
            {
                return " -re " + base.BuildBeginOptions(path);
            }
            return base.BuildBeginOptions(path);
        }

        public override string BuildVideoOptions(int streamId, int width, int height)
        {
            string fileName = "unitybuf:" + (width * height * 4) + "/2/" + Guid.NewGuid().ToString();
            lock (pipeFileNames_)
            {
                if (pipeFileNames_.ContainsKey(streamId))
                {
                    pipeFileNames_.Remove(streamId);
                }
                pipeFileNames_.Add(streamId, fileName);
            }
            return " -f rawvideo -pix_fmt rgba \"" + fileName + "\" ";
        }

        public override string BuildAudioOptions(int streamId, int sampleRate, int channels)
        {
            //string fileName = "unitybuf:" + (sampleRate * channels * 4 / 50) + "/0/" + Guid.NewGuid().ToString();
            string fileName = "unitybuf:0/0/" + Guid.NewGuid().ToString();
            lock (pipeFileNames_)
            {
                if (pipeFileNames_.ContainsKey(streamId))
                {
                    pipeFileNames_.Remove(streamId);
                }
                pipeFileNames_.Add(streamId, fileName);
            }
            return " -f f32le \"" + fileName + "\" ";
        }

        public override void CleanBuf()
        {
            foreach (var fileName in pipeFileNames_.Values)
            {
                var handle = unitybuf_get_handle_dll(fileName);
                if (handle != IntPtr.Zero)
                {
                    unitybuf_clear_dll(handle);
                }
            }
        }

        public override void ReadVideo(int streamId, int width, int height)
        {
            byte[] buf = new byte[width * height * 4];

            int elementSize = Marshal.SizeOf(typeof(byte));
            IntPtr bufPtr = Marshal.AllocHGlobal(buf.Length * elementSize);

            IntPtr handle;
            do
            {
                handle = unitybuf_get_handle_dll(pipeFileNames_[streamId]);
                if (handle == IntPtr.Zero)
                {
                    Thread.Sleep(1);
                }
            } while (handle == IntPtr.Zero && !IsEnd);

            int readPos = 0;

            while (!IsEnd)
            {
                /*
                int count = unitybuf_count_dll(handle);
                if (count <= 0)
                {
                    Thread.Sleep(1);
                    continue;
                }
                */

                bool error = false;
                bool eof = false;
                do
                {
                    int readSize = unitybuf_read_dll(handle, bufPtr + readPos * elementSize, buf.Length - readPos);
                    if (readSize == -('E' | ('O' << 8) | ('F' << 16) | (' ' << 24)))
                    {
                        eof = true;
                        break;
                    }
                    else if (readSize <= 0)
                    {
                        error = true;
                        break;
                    }
                    readPos += readSize;
                    if (readPos >= width * height * 4)
                    {
                        readPos -= width * height * 4;
                    }
                } while (readPos != 0 && !IsEnd);

                if (eof)
                {
                    break;
                }
                else if (error)
                {
                    Thread.Sleep(1);
                    continue;
                }

                Marshal.Copy(bufPtr, buf, 0, buf.Length);

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
            int size = 1024; // sampleRate * channels * 4 / 50;
            int elementSizeFloat = Marshal.SizeOf(typeof(float));
            IntPtr bufPtr = Marshal.AllocHGlobal(size);

            IntPtr handle;
            do
            {
                handle = unitybuf_get_handle_dll(pipeFileNames_[streamId]);
                if (handle == IntPtr.Zero)
                {
                    Thread.Sleep(1);
                }
            } while (handle == IntPtr.Zero && !IsEnd);

            while (!IsEnd)
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