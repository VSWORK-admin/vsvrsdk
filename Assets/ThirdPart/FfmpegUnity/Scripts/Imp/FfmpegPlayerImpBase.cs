using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public abstract class FfmpegPlayerImpBase
    {
        public static FfmpegPlayerImpBase GetNewInstance(FfmpegPlayerCommand playerCommand)
        {
            FfmpegPlayerImpBase playerImp = null;

#if UNITY_EDITOR_WIN && FFMPEG_UNITY_USE_BINARY_WIN
            playerImp = new FfmpegPlayerImpWinMonoBin(playerCommand);
#elif UNITY_EDITOR_WIN && !FFMPEG_UNITY_USE_BINARY_WIN || UNITY_EDITOR_LINUX && !FFMPEG_UNITY_USE_BINARY_LINUX
            playerImp = new FfmpegPlayerImpWinLib(playerCommand);
#elif (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC
            playerImp = new FfmpegPlayerImpIOSMemory(playerCommand);
#elif UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            playerImp = new FfmpegPlayerImpUnixMonoBin(playerCommand);
#elif UNITY_STANDALONE_WIN && !FFMPEG_UNITY_USE_BINARY_WIN || UNITY_STANDALONE_LINUX && !FFMPEG_UNITY_USE_BINARY_LINUX
            playerImp = new FfmpegPlayerImpWinLib(playerCommand);
#elif UNITY_STANDALONE_WIN && !ENABLE_IL2CPP
            playerImp = new FfmpegPlayerImpWinMonoBin(playerCommand);
#elif UNITY_STANDALONE_WIN && ENABLE_IL2CPP
            playerImp = new FfmpegPlayerImpWinIL2CPPBin(playerCommand);
#elif (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && !ENABLE_IL2CPP
            playerImp = new FfmpegPlayerImpUnixMonoBin(playerCommand);
#elif (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && ENABLE_IL2CPP
            playerImp = new FfmpegPlayerImpUnixIL2CPPBin(playerCommand);
#elif UNITY_ANDROID //&& FFMPEG_UNITY_USE_PIPE
            playerImp = new FfmpegPlayerImpAndroidPipe(playerCommand);
//#elif UNITY_ANDROID && !FFMPEG_UNITY_USE_PIPE
//            playerImp = new FfmpegPlayerImpAndroidMemory(playerCommand);
#elif UNITY_IOS //&& FFMPEG_UNITY_USE_PIPE
            playerImp = new FfmpegPlayerImpIOSPipe(playerCommand);
//#elif UNITY_IOS && !FFMPEG_UNITY_USE_PIPE
//            playerImp = new FfmpegPlayerImpIOSMemory(playerCommand);
#endif

            return playerImp;
        }

        public bool IsEnd
        {
            get;
            set;
        } = false;

        public virtual bool IsEOF
        {
            get
            {
                return IsEnd;
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }

        protected FfmpegPlayerCommand PlayerCommand
        {
            get;
            private set;
        } = null;

        public FfmpegPlayerImpBase(FfmpegPlayerCommand playerCommand)
        {
            PlayerCommand = playerCommand;
        }

        public virtual IEnumerator OpenFfprobeReaderCoroutine(string inputPathAll)
        {
            yield break;
        }

        public abstract TextReader OpenFfprobeReader(string inputPathAll);

        public abstract void CloseFfprobeReader();

        public virtual IEnumerator BuildBeginOptionsCoroutine(string path)
        {
            yield break;
        }

        public virtual string BuildBeginOptions(string path)
        {
            return " -i \"" + path + "\" "; 
        }

        public abstract string BuildVideoOptions(int streamId, int width, int height);

        public abstract string BuildAudioOptions(int streamId, int sampleRate, int channels);

        public virtual void Clean()
        {
        }

        public virtual void CleanBuf()
        {
        }

        protected void StreamReadVideo(BinaryReader reader, int streamId, int width, int height, bool checkIsRunning = true)
        {
            var stopWatch = new Stopwatch();
            double frameTime = 0.0;
            if (PlayerCommand.SyncFrameRate)
            {
                stopWatch.Start();
            }

            while (!IsEnd && (!checkIsRunning || PlayerCommand.IsRunning))
            {
                var videoBufferStart = reader.ReadBytes(width * height * 4);
                if (videoBufferStart == null)
                {
                    if (!IsEnd && (!checkIsRunning || PlayerCommand.IsRunning))
                    {
                        stopWatch.Stop();
                        return;
                    }
                    Thread.Sleep(1);
                    continue;
                }
                int pos = videoBufferStart.Length;
                byte[] videoBuffer;
                if (videoBufferStart.Length < width * height * 4)
                {
                    videoBuffer = new byte[width * height * 4];
                    Array.Copy(videoBufferStart, 0, videoBuffer, 0, videoBufferStart.Length);
                }
                else
                {
                    videoBuffer = videoBufferStart;
                }
                while (pos < width * height * 4)
                {
                    var addBuffer = reader.ReadBytes(width * height * 4 - pos);
                    if (addBuffer == null)
                    {
                        if (!IsEnd && (!checkIsRunning || PlayerCommand.IsRunning))
                        {
                            stopWatch.Stop();
                            return;
                        }
                        Thread.Sleep(1);
                        continue;
                    }
                    Array.Copy(addBuffer, 0, videoBuffer, pos, addBuffer.Length);
                    pos += addBuffer.Length;
                }

                var newVideoBuffer = new byte[videoBuffer.Length];
                for (int y = 0; y < height; y++)
                {
                    Array.Copy(videoBuffer, y * width * 4,
                        newVideoBuffer, (height - y - 1) * width * 4,
                        width * 4);
                }

                PlayerCommand.SetVideoBuffer(streamId, newVideoBuffer);

                if (PlayerCommand.SyncFrameRate)
                {
                    stopWatch.Stop();
                    double time = stopWatch.Elapsed.TotalSeconds;
                    stopWatch.Start();

                    frameTime += PlayerCommand.TimeBase;
                    if (time < frameTime)
                    {
                        Thread.Sleep((int)((frameTime - time) * 1000.0));
                    }
                }

                PlayerCommand.StopPerFrameFunc(streamId);
            }

            stopWatch.Stop();
        }

        public abstract void ReadVideo(int streamId, int width, int height);

        protected void StreamReadAudio(BinaryReader reader, int streamId, bool checkIsRunning = true)
        {
            float[] buffer = new float[2048];

            while (!IsEnd && (!checkIsRunning || PlayerCommand.IsRunning))
            {
                try
                {
                    for (int loop = 0; loop < buffer.Length; loop++)
                    {
                        buffer[loop] = reader.ReadSingle();
                    }
                    PlayerCommand.AddAudioBuffer(streamId, buffer);
                }
                catch (Exception)
                {
                    Thread.Sleep(1);
                    continue;
                }
            }
        }

        public abstract void ReadAudio(int streamId, int sampleRate, int channels);

        public virtual bool IsGetTimeFromImp
        {
            get
            {
                return false;
            }
        }

        public virtual float Time
        {
            get
            {
                return -1f;
            }
        }

        public virtual float StartTime
        {
            get
            {
                return 0f;
            }
            set
            {
                
            }
        }

        public virtual void WriteTexture(Texture texture, byte[] data, int width, int height)
        {
            Texture2D videoTexture;
            if (texture is RenderTexture)
            {
                videoTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            }
            else
            {
                videoTexture = texture as Texture2D;
            }
            if (videoTexture == null)
            {
                return;
            }

            videoTexture.LoadRawTextureData(data);
            videoTexture.Apply();

            if (texture is RenderTexture)
            {
                Graphics.Blit(videoTexture, texture as RenderTexture);
                UnityEngine.Object.Destroy(videoTexture);
            }
        }
    }
}
