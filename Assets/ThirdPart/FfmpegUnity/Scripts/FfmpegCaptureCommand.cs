#define CAPTURE_VIDEO_LIMIT

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

namespace FfmpegUnity
{
    public class FfmpegCaptureCommand : FfmpegCommand
    {
        const int CAPTURE_VIDEO_LIMIT_PER_FRAME = 2;

        static int captureVideoLimitCount_ = 0;
        static int waitCaptureVideo_ = 0;

        [Serializable]
        public class CaptureSource
        {
            public enum SourceType
            {
                Video_GameView,
                Video_Camera,
                Video_RenderTexture,
                Audio_AudioListener,
                Audio_AudioSource,
            }

            public SourceType Type = SourceType.Video_GameView;
            public int Width = -1;
            public int Height = 480;
            public float FrameRate = 30;
            public Camera SourceCamera = null;
            public RenderTexture SourceRenderTexture = null;
            public AudioSource SourceAudio = null;
            public bool Mute = false;
        }

        public CaptureSource[] CaptureSources = new CaptureSource[]
        {
            new CaptureSource()
            {
                Type = CaptureSource.SourceType.Video_GameView,
            },
            new CaptureSource()
            {
                Type = CaptureSource.SourceType.Audio_AudioListener,
            },
        };

        public string CaptureOptions = "";

        Dictionary<int, byte[]> videoBuffers_ = new Dictionary<int, byte[]>();

        Dictionary<int, List<float>> audioBuffers_ = new Dictionary<int, List<float>>();
        Dictionary<int, int> audioChannels_ = new Dictionary<int, int>();

        List<Thread> threads_ = new List<Thread>();

        Dictionary<int, bool> reverse_ = new Dictionary<int, bool>();

        Dictionary<int, Vector2Int> texturesSize_ = new Dictionary<int, Vector2Int>();
        Dictionary<int, RenderTexture> renderTextures_ = new Dictionary<int, RenderTexture>();

        Dictionary<int, Texture2D> tempTextures_ = new Dictionary<int, Texture2D>();

        Shader flipShader_;
        Material flipMaterial_;

        FfmpegCaptureImpBase captureImp_;

        AudioListener audioListener_ = null;

        Dictionary<int, float> nextCaptureTime_ = new Dictionary<int, float>();

        bool isGetCameraRenderFormat_ = false;
        RenderTextureFormat cameraRenderFormat_;

        Dictionary<int, TextureFormat> syncGPUReadbackFormats_ = new Dictionary<int, TextureFormat>();

        List<FfmpegCaptureAudio> captureAudios_ = new List<FfmpegCaptureAudio>();
        bool captureAudioEnabled_ = false;

        protected override void Build()
        {
            StartCoroutine(captureCoroutine());
        }

        protected override void Clean()
        {
            if (captureImp_ != null)
            {
                captureImp_.IsEnd = true;
            }

            /*
            foreach (var thread in threads_)
            {
                if (thread != null && thread.IsAlive)
                {
                    bool exited = thread.Join(1);
                    while (!exited && IsRunning)
                    {
                        exited = thread.Join(1);
                    }
                    if (!exited && !IsRunning)
                    {
                        thread.Abort();
                    }
                }
            }
            */
            threads_ = new List<Thread>();

            texturesSize_ = new Dictionary<int, Vector2Int>();

            videoBuffers_ = new Dictionary<int, byte[]>();
            audioBuffers_ = new Dictionary<int, List<float>>();

            /*
            if (captureImp_ != null)
            {
                captureImp_.IsEnd = false;
            }
            */
            captureImp_ = null;

            captureAudioEnabled_ = false;
            foreach (var captureAudio in captureAudios_)
            {
                Destroy(captureAudio);
            }
            captureAudios_.Clear();
        }

        protected virtual void ByteStart()
        {

        }

        IEnumerator captureCoroutine()
        {
            flipShader_ = Resources.Load<Shader>("FfmpegUnity/Shaders/FlipShader");
            flipMaterial_ = new Material(flipShader_);

            audioListener_ = FindObjectOfType<AudioListener>();

#if UNITY_EDITOR_WIN && FFMPEG_UNITY_USE_BINARY_WIN
            captureImp_ = new FfmpegCaptureImpWinMonoBin(this);
#elif UNITY_EDITOR_WIN && !FFMPEG_UNITY_USE_BINARY_WIN || UNITY_EDITOR_LINUX && !FFMPEG_UNITY_USE_BINARY_LINUX
            captureImp_ = new FfmpegCaptureImpWinLib(this);
#elif (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC
            captureImp_ = new FfmpegCaptureImpIOSMemory(this);
#elif UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            captureImp_ = new FfmpegCaptureImpUnixMonoBin(this);
#elif UNITY_STANDALONE_WIN && !FFMPEG_UNITY_USE_BINARY_WIN || UNITY_STANDALONE_LINUX && !FFMPEG_UNITY_USE_BINARY_LINUX
            captureImp_ = new FfmpegCaptureImpWinLib(this);
#elif UNITY_STANDALONE_WIN && !ENABLE_IL2CPP
            captureImp_ = new FfmpegCaptureImpWinMonoBin(this);
#elif UNITY_STANDALONE_WIN && ENABLE_IL2CPP
            captureImp_ = new FfmpegCaptureImpWinIL2CPPBin(this);
#elif (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && !ENABLE_IL2CPP
            captureImp_ = new FfmpegCaptureImpUnixMonoBin(this);
#elif (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && ENABLE_IL2CPP
            captureImp_ = new FfmpegCaptureImpUnixIL2CPPBin(this);
#elif UNITY_ANDROID //&& FFMPEG_UNITY_USE_PIPE
            captureImp_ = new FfmpegCaptureImpAndroidPipe(this);
//#elif UNITY_ANDROID && !FFMPEG_UNITY_USE_PIPE
//            captureImp_ = new FfmpegCaptureImpAndroidMemory(this);
#elif UNITY_IOS //&& FFMPEG_UNITY_USE_PIPE
            captureImp_ = new FfmpegCaptureImpIOSPipe(this);
//#elif UNITY_IOS && !FFMPEG_UNITY_USE_PIPE
//            captureImp_ = new FfmpegCaptureImpIOSMemory(this);
#endif

            RunOptions = " -y -re ";
            IsGetStdErr = PrintStdErr || GetProgressOnScript;
            if (!IsGetStdErr)
            {
                RunOptions += " -loglevel quiet ";
            }

            for (int captureLoop = 0; captureLoop < CaptureSources.Length; captureLoop++)
            {
                string fileName = null;

                switch (CaptureSources[captureLoop].Type)
                {
                    case CaptureSource.SourceType.Video_GameView:
                        {
                            reverse_[captureLoop] = captureImp_.Reverse;

                            int width;
                            int height;
                            if (CaptureSources[captureLoop].Width <= 0 && CaptureSources[captureLoop].Height <= 0)
                            {
                                width = Screen.width;
                                height = Screen.height;
                            }
                            else if (CaptureSources[captureLoop].Width <= 0)
                            {
                                width = Screen.width * CaptureSources[captureLoop].Height / Screen.height;
                                height = CaptureSources[captureLoop].Height;
                            }
                            else if (CaptureSources[captureLoop].Height <= 0)
                            {
                                width = CaptureSources[captureLoop].Width;
                                height = Screen.height * CaptureSources[captureLoop].Width / Screen.width;
                            }
                            else
                            {
                                width = CaptureSources[captureLoop].Width;
                                height = CaptureSources[captureLoop].Height;
                            }
                            texturesSize_[captureLoop] = new Vector2Int(width, height);

                            fileName = captureImp_.GenerateCaptureFileNameVideo(width, height);

                            var captureId = captureLoop;
                            var thread = new Thread(() => { writeVideo(captureId, fileName, width, height); });
                            //thread.Start();
                            threads_.Add(thread);

                            RenderTexture checkTexture = RenderTexture.GetTemporary(2, 2);
                            RenderTextureFormat renderTextureFormat = checkTexture.format;
                            string textureFormat = "rgba";
                            syncGPUReadbackFormats_[captureLoop] = TextureFormat.RGBA32;
                            if (SystemInfo.supportsAsyncGPUReadback)
                            {
                                switch (checkTexture.format)
                                {
                                    case RenderTextureFormat.BGRA32:
                                        textureFormat = "bgra";
                                        syncGPUReadbackFormats_[captureLoop] = TextureFormat.BGRA32;
                                        break;
                                    case RenderTextureFormat.ARGB32:
                                        textureFormat = "argb";
                                        syncGPUReadbackFormats_[captureLoop] = TextureFormat.ARGB32;
                                        break;
                                }
                            }
                            RenderTexture.ReleaseTemporary(checkTexture);

                            RunOptions += " -r " + CaptureSources[captureLoop].FrameRate + " -f rawvideo -s " + width + "x" + height + " -pix_fmt " + textureFormat + " -i \"" + fileName + "\" ";
                        }
                        break;
                    case CaptureSource.SourceType.Video_Camera:
                        {
                            reverse_[captureLoop] = true;

                            int baseWidth = (int)(Screen.width * CaptureSources[captureLoop].SourceCamera.rect.width);
                            int baseHeight = (int)(Screen.height * CaptureSources[captureLoop].SourceCamera.rect.height);
                            int width;
                            int height;
                            if (CaptureSources[captureLoop].Width <= 0 && CaptureSources[captureLoop].Height <= 0)
                            {
                                width = baseWidth;
                                height = baseHeight;
                            }
                            else if (CaptureSources[captureLoop].Width <= 0)
                            {
                                width = baseWidth * CaptureSources[captureLoop].Height / baseHeight;
                                height = CaptureSources[captureLoop].Height;
                            }
                            else if (CaptureSources[captureLoop].Height <= 0)
                            {
                                width = CaptureSources[captureLoop].Width;
                                height = baseHeight * CaptureSources[captureLoop].Width / baseWidth;
                            }
                            else
                            {
                                width = CaptureSources[captureLoop].Width;
                                height = CaptureSources[captureLoop].Height;
                            }
                            texturesSize_[captureLoop] = new Vector2Int(width, height);

                            fileName = captureImp_.GenerateCaptureFileNameVideo(width, height);

                            var captureId = captureLoop;
                            var thread = new Thread(() => { writeVideo(captureId, fileName, width, height); });
                            //thread.Start();
                            threads_.Add(thread);

                            var camera = CaptureSources[captureLoop].SourceCamera;
                            var captureCamera = camera.gameObject.AddComponent<FfmpegCaptureCamera>();
                            captureCamera.CaptureId = captureLoop;
                            captureCamera.CaptureCommand = this;

                            string textureFormat = "rgba";
                            syncGPUReadbackFormats_[captureLoop] = TextureFormat.RGBA32;
                            if (SystemInfo.supportsAsyncGPUReadback)
                            {
                                while (!isGetCameraRenderFormat_)
                                {
                                    yield return null;
                                }

                                //UnityEngine.Debug.Log(cameraRenderFormat_);

                                switch (cameraRenderFormat_)
                                {
                                    case RenderTextureFormat.BGRA32:
                                        textureFormat = "bgra";
                                        syncGPUReadbackFormats_[captureLoop] = TextureFormat.BGRA32;
                                        break;
                                    case RenderTextureFormat.ARGB32:
                                        textureFormat = "argb";
                                        syncGPUReadbackFormats_[captureLoop] = TextureFormat.ARGB32;
                                        break;
                                    default:
                                        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal)
                                        {
                                            textureFormat = "bgra";
                                            syncGPUReadbackFormats_[captureLoop] = TextureFormat.BGRA32;
                                            cameraRenderFormat_ = RenderTextureFormat.BGRA32;
                                        }
                                        break;
                                }
                            }

                            RunOptions += " -r " + CaptureSources[captureLoop].FrameRate + " -f rawvideo -s " + width + "x" + height + " -pix_fmt " + textureFormat + " -i \"" + fileName + "\" ";
                        }
                        break;
                    case CaptureSource.SourceType.Video_RenderTexture:
                        {
                            reverse_[captureLoop] = true;

                            RenderTexture renderTexture = CaptureSources[captureLoop].SourceRenderTexture;
                            int width = renderTexture.width;
                            int height = renderTexture.height;
                            texturesSize_[captureLoop] = new Vector2Int(width, height);

                            fileName = captureImp_.GenerateCaptureFileNameVideo(width, height);

                            var captureId = captureLoop;
                            var thread = new Thread(() => { writeVideo(captureId, fileName, width, height); });
                            //thread.Start();
                            threads_.Add(thread);

                            string textureFormat = "rgba";
                            syncGPUReadbackFormats_[captureLoop] = TextureFormat.RGBA32;
                            if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal)
                            {
                                textureFormat = "bgra";
                                syncGPUReadbackFormats_[captureLoop] = TextureFormat.BGRA32;
                            }
                            else if (SystemInfo.supportsAsyncGPUReadback)
                            {
                                switch (renderTexture.format)
                                {
                                    case RenderTextureFormat.BGRA32:
                                        textureFormat = "bgra";
                                        syncGPUReadbackFormats_[captureLoop] = TextureFormat.BGRA32;
                                        break;
                                    case RenderTextureFormat.ARGB32:
                                        textureFormat = "argb";
                                        syncGPUReadbackFormats_[captureLoop] = TextureFormat.ARGB32;
                                        break;
                                }
                            }

                            RunOptions += " -r " + CaptureSources[captureLoop].FrameRate + " -f rawvideo -s " + width + "x" + height + " -pix_fmt " + textureFormat + " -i \"" + fileName + "\" ";
                        }
                        break;
                    case CaptureSource.SourceType.Audio_AudioListener:
                        {
                            var captureAudio = audioListener_.gameObject.AddComponent<FfmpegCaptureAudio>();
                            captureAudio.StreamId = captureLoop;
                            captureAudio.Capture = this;
                            captureAudio.Mute = false;
                            captureAudios_.Add(captureAudio);

                            while (!audioChannels_.ContainsKey(captureLoop))
                            {
                                yield return null;
                            }

                            fileName = captureImp_.GenerateCaptureFileNameAudio(AudioSettings.outputSampleRate, audioChannels_[captureLoop]);
                            captureImp_.AudioChannels = audioChannels_[captureLoop];

                            var captureId = captureLoop;
                            int sampleRate = AudioSettings.outputSampleRate;
                            int audioThreadChannels = audioChannels_[captureLoop];
                            var thread = new Thread(() => { writeAudio(captureId, fileName, sampleRate, audioThreadChannels); });
                            //thread.Start();
                            threads_.Add(thread);

                            RunOptions += " -f f32le -ar " + AudioSettings.outputSampleRate + " -ac " + audioChannels_[captureLoop] + " -i \"" + fileName + "\" ";
                        }
                        break;
                    case CaptureSource.SourceType.Audio_AudioSource:
                        {
                            var captureAudio = CaptureSources[captureLoop].SourceAudio.gameObject.AddComponent<FfmpegCaptureAudio>();
                            captureAudio.StreamId = captureLoop;
                            captureAudio.Capture = this;
                            captureAudio.Mute = CaptureSources[captureLoop].Mute;
                            captureAudios_.Add(captureAudio);

                            while (!audioChannels_.ContainsKey(captureLoop))
                            {
                                yield return null;
                            }

                            fileName = captureImp_.GenerateCaptureFileNameAudio(AudioSettings.outputSampleRate, audioChannels_[captureLoop]);
                            captureImp_.AudioChannels = audioChannels_[captureLoop];

                            var captureId = captureLoop;
                            int sampleRate = AudioSettings.outputSampleRate;
                            int audioThreadChannels = audioChannels_[captureLoop];
                            var thread = new Thread(() =>
                            {
                                writeAudio(captureId, fileName, sampleRate, audioThreadChannels);
                            });
                            //thread.Start();
                            threads_.Add(thread);

                            RunOptions += " -f f32le -ar " + AudioSettings.outputSampleRate + " -ac " + audioChannels_[captureLoop] + " -i \"" + fileName + "\" ";
                        }
                        break;
                }
            }

            foreach (Thread thread in threads_)
            {
                thread.Start();
            }

            yield return null;

            RunOptions += " " + CaptureOptions;

            ByteStart();

            IsFinishedBuild = true;

            yield return new WaitForEndOfFrame();

            while (captureImp_ != null && !captureImp_.IsEnd)
            {
                waitCaptureVideo_++;

                for (int captureLoop = 0; captureLoop < CaptureSources.Length; captureLoop++)
                {
                    if (CaptureSources[captureLoop].Type <= CaptureSource.SourceType.Video_RenderTexture)
                    {
                        if (CaptureSources[captureLoop].Type == CaptureSource.SourceType.Video_Camera && SystemInfo.supportsAsyncGPUReadback)
                        {
                            continue;
                        }

                        if (nextCaptureTime_.ContainsKey(captureLoop) && nextCaptureTime_[captureLoop] > Time.realtimeSinceStartup)
                        {
                            continue;
                        }

#if CAPTURE_VIDEO_LIMIT
                        while (captureVideoLimitCount_ > CAPTURE_VIDEO_LIMIT_PER_FRAME)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        captureVideoLimitCount_++;
                        if (captureVideoLimitCount_ > CAPTURE_VIDEO_LIMIT_PER_FRAME)
                        {
                            yield return new WaitForEndOfFrame();
                            captureVideoLimitCount_ = 1;
                        }
#endif

                        if (!texturesSize_.ContainsKey(captureLoop) /*|| !(renderTextures_.ContainsKey(captureLoop) && (CaptureSources[captureLoop].Type != CaptureSource.SourceType.Video_RenderTexture || reverse_[captureLoop]))*/)
                        {
                            continue;
                        }

                        RenderTexture srcTexture = null;
                        switch (CaptureSources[captureLoop].Type)
                        {
                            case CaptureSource.SourceType.Video_GameView:
                                {
                                    RenderTexture screenTexture = RenderTexture.GetTemporary(Screen.width, Screen.height);
                                    ScreenCapture.CaptureScreenshotIntoRenderTexture(screenTexture);
                                    srcTexture = new RenderTexture(texturesSize_[captureLoop].x, texturesSize_[captureLoop].y, 0, screenTexture.format);
                                    if (reverse_[captureLoop])
                                    {
                                        Graphics.Blit(screenTexture, srcTexture, flipMaterial_);
                                    }
                                    else
                                    {
                                        Graphics.Blit(screenTexture, srcTexture);
                                    }
                                    RenderTexture.ReleaseTemporary(screenTexture);
                                }
                                break;
                            case CaptureSource.SourceType.Video_Camera:
                                {
                                    srcTexture = renderTextures_[captureLoop];
                                }
                                break;
                            case CaptureSource.SourceType.Video_RenderTexture:
                                {
                                    srcTexture = CaptureSources[captureLoop].SourceRenderTexture;
                                    if (srcTexture == null)
                                    {
                                        UnityEngine.Debug.LogError("Error: SourceRenderTexture is not set.");
                                        yield break;
                                    }

                                    if (reverse_[captureLoop])
                                    {
                                        var srcTexture2 = new RenderTexture(srcTexture.width, srcTexture.height, 0);
                                        Graphics.Blit(srcTexture, srcTexture2, flipMaterial_);
                                        srcTexture = srcTexture2;
                                    }
                                }
                                break;
                        }

                        if (SystemInfo.supportsAsyncGPUReadback)
                        {
                            renderTextures_[captureLoop] = srcTexture;
                            int captureId = captureLoop;
                            if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal)
                            {
                                AsyncGPUReadback.Request(srcTexture, 0, (request) => captureReadbackCompleted(request, captureId));
                            }
                            else
                            {
                                AsyncGPUReadback.Request(srcTexture, 0, syncGPUReadbackFormats_[captureId], (request) => captureReadbackCompleted(request, captureId));
                            }
                        }
                        else
                        {
                            RenderTexture filpedTexture = srcTexture;

                            var tempTextureActive = RenderTexture.active;

                            RenderTexture.active = filpedTexture;

                            if (!tempTextures_.ContainsKey(captureLoop))
                            {
                                tempTextures_[captureLoop] = new Texture2D(srcTexture.width, srcTexture.height, TextureFormat.RGBA32, false);
                            }
                            var tempTexture2 = tempTextures_[captureLoop];
                            tempTexture2.ReadPixels(new Rect(0, 0, filpedTexture.width, filpedTexture.height), 0, 0);
                            tempTexture2.Apply();

                            RenderTexture.active = tempTextureActive;

                            var textureData = tempTexture2.GetRawTextureData<byte>().ToArray();
                            byte[] bufferData = new byte[tempTexture2.width * tempTexture2.height * 4];

                            Array.Copy(textureData, 0, bufferData, 0, bufferData.Length);
                            lock (videoBuffers_)
                            {
                                videoBuffers_[captureLoop] = bufferData;
                            }

                            if (CaptureSources[captureLoop].Type != CaptureSource.SourceType.Video_RenderTexture || reverse_[captureLoop])
                            {
                                Destroy(srcTexture);
                                if (renderTextures_.ContainsKey(captureLoop))
                                {
                                    renderTextures_.Remove(captureLoop);
                                }
                            }

                            captureAudioEnabled_ = true;
                        }

                        if (!nextCaptureTime_.ContainsKey(captureLoop))
                        {
                            nextCaptureTime_[captureLoop] = Time.realtimeSinceStartup;
                        }
                        nextCaptureTime_[captureLoop] += 1f / CaptureSources[captureLoop].FrameRate;
                    }
                }

#if CAPTURE_VIDEO_LIMIT
                waitCaptureVideo_--;
                yield return new WaitForEndOfFrame();
                while (waitCaptureVideo_ > 0)
                {
                    yield return new WaitForEndOfFrame();
                }
#else
                yield return new WaitForEndOfFrame();
#endif
            }

            nextCaptureTime_.Clear();

            var tempTexturesDestroy = tempTextures_.Values;
            foreach (var tempTexture3 in tempTexturesDestroy)
            {
                DestroyImmediate(tempTexture3);
            }
            tempTextures_.Clear();
        }

        public void CaptureCameraRenderImage(RenderTexture srcTexture, int captureId)
        {
            if (!isGetCameraRenderFormat_)
            {
                cameraRenderFormat_ = srcTexture.format;
                isGetCameraRenderFormat_ = true;
            }

            if (nextCaptureTime_.ContainsKey(captureId) && nextCaptureTime_[captureId] > Time.realtimeSinceStartup
                || !texturesSize_.ContainsKey(captureId) || renderTextures_.ContainsKey(captureId))
            {
                return;
            }

            var renderTexture = new RenderTexture(texturesSize_[captureId].x, texturesSize_[captureId].y, 0, cameraRenderFormat_);

            if (reverse_[captureId])
            {
                Graphics.Blit(srcTexture, renderTexture, flipMaterial_);
            }
            else
            {
                Graphics.Blit(srcTexture, renderTexture);
            }

            renderTextures_[captureId] = renderTexture;

            if (SystemInfo.supportsAsyncGPUReadback)
            {
                if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal)
                {
                    AsyncGPUReadback.Request(renderTexture, 0, (request) => captureReadbackCompleted(request, captureId));
                }
                else
                {
                    AsyncGPUReadback.Request(renderTexture, 0, syncGPUReadbackFormats_[captureId], (request) => captureReadbackCompleted(request, captureId));
                }

                if (!nextCaptureTime_.ContainsKey(captureId))
                {
                    nextCaptureTime_[captureId] = Time.realtimeSinceStartup;
                }
                nextCaptureTime_[captureId] += 1f / CaptureSources[captureId].FrameRate;
            }
        }

        void captureReadbackCompleted(AsyncGPUReadbackRequest request, int captureId)
        {
            if (CaptureSources[captureId].Type != CaptureSource.SourceType.Video_RenderTexture || reverse_[captureId])
            {
                if (renderTextures_.ContainsKey(captureId))
                {
                    DestroyImmediate(renderTextures_[captureId]);
                    renderTextures_.Remove(captureId);
                }
            }

            var imageBytes = request.GetData<byte>();
            lock (videoBuffers_)
            {
                /*
                if (syncGPUReadbackFormats_[captureId] == TextureFormat.RGBA64)
                {
                    var imageBytesArray = imageBytes.ToArray();
                    int loop = 0;
                    while (loop < imageBytesArray.Length)
                    {
                        float val = Mathf.HalfToFloat(BitConverter.ToUInt16(imageBytesArray, loop));
                        ushort half = (ushort)(val * 0x00007bfe);
                        imageBytesArray[loop] = (byte)(half & 0x00ff);
                        imageBytesArray[loop + 1] = (byte)((half & 0xff00) >> 8);
                        loop += 2;
                    }
                    videoBuffers_[captureId] = imageBytesArray;
                }
                else
                */
                {
                    videoBuffers_[captureId] = imageBytes.ToArray();
                }
            }

            captureAudioEnabled_ = true;
        }

        protected override void Update()
        {
            base.Update();

            if (captureImp_ != null && captureImp_.IsEnd)
            {
                StopFfmpeg();
                return;
            }
        }

        void writeVideo(int streamId, string pipeFileName, int width, int height)
        {
            captureImp_.WriteVideo(streamId, pipeFileName, width, height, videoBuffers_);
        }

        void writeAudio(int streamId, string pipeFileName, int sampleRate, int channels)
        {
            captureImp_.WriteAudio(streamId, pipeFileName, sampleRate, channels, audioBuffers_);
        }

        public void OnAudioFilterWriteToCaptureAudio(float[] data, int channels, int streamId)
        {
            audioChannels_[streamId] = channels;

            if (!audioBuffers_.ContainsKey(streamId))
            {
                audioBuffers_[streamId] = new List<float>();
            }
            if (captureAudioEnabled_)
            {
                lock (audioBuffers_[streamId])
                {
                    if (data != null && data.Length > 0 && audioBuffers_.ContainsKey(streamId))
                    {
                        audioBuffers_[streamId].AddRange(data);
                    }
                }
            }
            else
            {
                lock (audioBuffers_[streamId])
                {
                    if (data != null && data.Length > 0 && audioBuffers_.ContainsKey(streamId))
                    {
                        audioBuffers_[streamId].AddRange(new float[data.Length]);
                    }
                }
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                StopFfmpegMain();
            }
        }
    }
}
