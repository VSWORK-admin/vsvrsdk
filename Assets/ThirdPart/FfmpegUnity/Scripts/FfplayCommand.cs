using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace FfmpegUnity
{
    public class FfplayCommand : MonoBehaviour
    {
        const int SEEKING_AUDIO_FRAMES_START = 4;

        public bool ExecuteOnStart = true;
        public string Options = "";
        public FfmpegPath.DefaultPath DefaultPath = FfmpegPath.DefaultPath.NONE;
        public string InputPath = "";
        public FfmpegPlayerVideoTexture VideoTexture;
        public AudioSource AudioSourceComponent;
        public bool PrintStdErr = false;

        public Vector2Int RawVideoFrameSize
        {
            get;
            private set;
        } = Vector2Int.zero;

        public bool IsRunning
        {
            get
            {
                return ffplay_is_running(currentId_) != 0;
            }
        }

        double prevDuration_ = 0.0;
        public double Duration
        {
            get
            {
                double ret = ffplay_get_duration(currentId_);
                if (ret <= 0.0)
                {
                    ret = prevDuration_;
                }
                else
                {
                    prevDuration_ = ret;
                }
                return ret;
            }
        }

        double prevCurrentTime_ = 0.0;
        public double CurrentTime
        {
            get
            {
                double ret = ffplay_get_master_clock(currentId_);
                if (ret <= 0.0 || seekingAudioFrames_ > 0 || double.IsNaN(ret))
                {
                    ret = prevCurrentTime_;
                }
                else
                {
                    prevCurrentTime_ = ret;
                }
                return ret;
            }
        }

        bool prevPaused_ = false;
        public bool Paused
        {
            get
            {
                int ret = ffplay_get_paused(currentId_);
                if (ret < 0 || seekingAudioFrames_ > 0)
                {
                    return prevPaused_;
                }
                prevPaused_ = ret != 0;
                return prevPaused_;
            }
        }

        public bool Muted
        {
            get
            {
                return ffplay_get_muted(currentId_) != 0;
            }
        }

        public bool Loop
        {
            set
            {
                ffplay_set_loop(currentId_, value ? 0 : 1);
            }
            get
            {
                return ffplay_get_loop(currentId_) == 0;
            }
        }

        public bool HasChapter
        {
            get
            {
                return ffplay_has_chapter(currentId_) != 0;
            }
        }

        Thread thread_ = null;
        IntPtr videoBufferPtr_ = IntPtr.Zero;
        int videoBufferSize_;
        int width_;
        int height_;
        Texture tempTexture_;
        int channels_ = -1;
        int audioDataSize_ = -1;
        FfmpegCommand.StreamingAssetsCopyPathClass streamingAssetsCopyPathClass_ = new FfmpegCommand.StreamingAssetsCopyPathClass();
        int seekingAudioFrames_ = 0;

#if UNITY_EDITOR_WIN || !UNITY_EDITOR && UNITY_STANDALONE_WIN
        const string LIB_NAME = "libffmpegDll";
#elif UNITY_EDITOR_OSX || !UNITY_EDITOR && UNITY_STANDALONE_OSX
        const string LIB_NAME = "FfmpegUnityMacPlugin";
#elif UNITY_EDITOR_LINUX || !UNITY_EDITOR && UNITY_STANDALONE_LINUX 
        const string LIB_NAME = "ffmpegDll";
#elif !UNITY_EDITOR && UNITY_ANDROID
        const string LIB_NAME = "ffmpegkit";
#else
        const string LIB_NAME = "__Internal";
#endif
        [DllImport(LIB_NAME)]
        static extern int ffplay_start(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]string[] argv, int id, string file_path);
        [DllImport(LIB_NAME)]
        static extern void ffplay_stop(int id);
        [DllImport(LIB_NAME)]
        static extern int ffplay_get_audio(int id, short[] stream, int len);
        [DllImport(LIB_NAME)]
        static extern void ffplay_set_audio_spec_size(int size);
        [DllImport(LIB_NAME)]
        static extern void ffplay_set_audio_channels(int val);
        [DllImport(LIB_NAME)]
        static extern void ffplay_set_audio_sample_rate(int val);
        [DllImport(LIB_NAME)]
        static extern int ffplay_is_running(int id);
        [DllImport(LIB_NAME)]
        static extern void ffplay_set_video_buffer(int id, IntPtr buffer, int width, int height);
        [DllImport(LIB_NAME)]
        static extern int ffplay_get_frame_width(int id);
        [DllImport(LIB_NAME)]
        static extern int ffplay_get_frame_height(int id);
        [DllImport(LIB_NAME)]
        static extern void ffplay_set_filp_mode(int val);
        [DllImport(LIB_NAME)]
        static extern void ffplay_set_reset_audio(int val);

        [DllImport(LIB_NAME)]
        static extern double ffplay_get_master_clock(int id);
        [DllImport(LIB_NAME)]
        static extern int ffplay_toggle_pause(int id);
        [DllImport(LIB_NAME)]
        static extern int ffplay_toggle_mute(int id);
        [DllImport(LIB_NAME)]
        static extern int ffplay_stream_cycle_channel(int id, int codec_type);
        [DllImport(LIB_NAME)]
        static extern int ffplay_step_to_next_frame(int id);
        [DllImport(LIB_NAME)]
        static extern int ffplay_seek_incr_seconds(int id, double incr);
        [DllImport(LIB_NAME)]
        static extern int ffplay_seek(int id, double pos);
        [DllImport(LIB_NAME)]
        static extern double ffplay_get_duration(int id);
        [DllImport(LIB_NAME)]
        static extern int ffplay_has_chapter(int id);
        [DllImport(LIB_NAME)]
        static extern int ffplay_seek_chapter(int id, int incr);
        [DllImport(LIB_NAME)]
        static extern int ffplay_set_loop(int id, int val);
        [DllImport(LIB_NAME)]
        static extern int ffplay_get_paused(int id);
        [DllImport(LIB_NAME)]
        static extern int ffplay_get_muted(int id);
        [DllImport(LIB_NAME)]
        static extern int ffplay_get_loop(int id);

#if !UNITY_EDITOR && UNITY_ANDROID
        [DllImport("glNative")]
        static extern int openGLSetupNativeTextureRender(IntPtr textureId, int width, int height);
        [DllImport("glNative")]
        static extern void openGLCopyBuffer(int id, IntPtr buf);
        [DllImport("glNative")]
        static extern void openGLFinishNativeTextureRender(int id);
        [DllImport("glNative")]
        static extern IntPtr openGLGetRenderEventFunc();

        Dictionary<Texture, int> openGLIds_ = new Dictionary<Texture, int>();
#endif

#if !UNITY_EDITOR && UNITY_IOS
        [DllImport("__Internal")]
        static extern int metalSetupNativeTextureRender(IntPtr textureId);
        [DllImport("__Internal")]
        static extern void metalCopyBuffer(int eventId, IntPtr buf);
        [DllImport("__Internal")]
        static extern void metalFinishNativeTextureRender(int eventId);
        [DllImport("__Internal")]
        static extern IntPtr metalGetRenderEventFunc();

        Dictionary<Texture, int> metalIds_ = new Dictionary<Texture, int>();
        CommandBuffer commandBuffer_;
#endif

        int currentId_ = -1;
        static int nextId_ = 0;

        bool audioLock_ = true;
        List<float[]> audioDatas_ = new List<float[]>();
        int tempAudioDatasCount_ = 0;

        string logFilePath_ = null;
        StreamReader streamReader_ = null;

        string parseOptions(string runOptions)
        {
            string options = runOptions.Replace("{PERSISTENT_DATA_PATH}", Application.persistentDataPath)
                .Replace("{TEMPORARY_CACHE_PATH}", Application.temporaryCachePath)
                .Replace("\r\n", "\n").Replace("\\\n", " ").Replace("^\n", " ").Replace("\n", " ");
            options = options.Replace("{STREAMING_ASSETS_PATH}", Application.streamingAssetsPath);
            return options;
        }

        public void Init()
        {
            if (nextId_ == 0 || nextId_ == int.MaxValue)
            {
                nextId_ = UnityEngine.Random.Range(1, int.MaxValue);
            }
            currentId_ = nextId_;
            nextId_++;

            StartCoroutine(initCoroutine());
        }

        IEnumerator initCoroutine()
        {
            tempTexture_ = null;

            if (PrintStdErr)
            {
                logFilePath_ = Application.temporaryCachePath + "/" + Guid.NewGuid().ToString() + ".log";
            }
            else
            {
                logFilePath_ = null;
            }

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            ffplay_set_filp_mode(1);
#else
            ffplay_set_filp_mode(0);
#endif

            /*
#if UNITY_EDITOR_OSX || !UNITY_EDITOR && (UNITY_IOS || UNITY_STANDALONE_OSX)
            ffplay_set_reset_audio(1);
#else
            ffplay_set_reset_audio(0);
#endif
            */
            ffplay_set_reset_audio(1);

            string runOptions = parseOptions(Options);
            string[] splitedOptions = FfmpegCommand.CommandSplitStatic(runOptions);

            string filePath = FfmpegPath.PathWithDefault(DefaultPath, InputPath);
            if (filePath.StartsWith("jar:file://"))
            {
                yield return streamingAssetsCopyPathClass_.StreamingAssetsCopyPath(filePath);
                filePath = streamingAssetsCopyPathClass_.PathInStreamingAssetsCopy;
            }

            List<string> tempOptions = new List<string>();
            tempOptions.Add("ffplay");
            if (AudioSourceComponent == null)
            {
                tempOptions.Add("-an");
            }
            tempOptions.AddRange(splitedOptions);
            tempOptions.Add(filePath);

            if (AudioSourceComponent != null)
            {
                ffplay_set_audio_sample_rate(AudioSettings.outputSampleRate);

                int channels = 2;
                switch (AudioSettings.speakerMode)
                {
                    case AudioSpeakerMode.Mono:
                        channels = 1;
                        break;
                    case AudioSpeakerMode.Quad:
                        channels = 4;
                        break;
                    case AudioSpeakerMode.Surround:
                        channels = 5;
                        break;
                    case AudioSpeakerMode.Mode5point1:
                        channels = 6;
                        break;
                    case AudioSpeakerMode.Mode7point1:
                        channels = 8;
                        break;
                }

                AudioSourceComponent.clip = AudioClip.Create("", AudioSettings.outputSampleRate * 2, channels, AudioSettings.outputSampleRate, true);
                AudioSourceComponent.loop = true;
                //if (AudioSourceComponent.playOnAwake)
                {
                    AudioSourceComponent.Play();
                }

                FfplayAudio audio = AudioSourceComponent.GetComponent<FfplayAudio>();
                if (audio == null)
                {
                    audio = AudioSourceComponent.gameObject.AddComponent<FfplayAudio>();
                }
                audio.Ffplay = this;

                while (channels_ <= 0 || audioDataSize_ <= 0)
                {
                    yield return null;
                }

                ffplay_set_audio_channels(channels_);

                ffplay_set_audio_spec_size(audioDataSize_);
            }

            thread_ = new Thread(() =>
            {
                string[] allOptions = tempOptions.ToArray();
                ffplay_start(allOptions.Length, allOptions, currentId_, logFilePath_);

                if (videoBufferPtr_ != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(videoBufferPtr_);
                    videoBufferPtr_ = IntPtr.Zero;
                }

                if (streamReader_ != null)
                {
                    streamReader_.Dispose();
                    streamReader_ = null;
                }
                if (!string.IsNullOrEmpty(logFilePath_))
                {
                    try
                    {
                        File.Delete(logFilePath_);
                    }
                    catch (Exception)
                    {

                    }
                }
            });
            thread_.Start();

            StartCoroutine(outputLogCoroutine());

            RawVideoFrameSize = new Vector2Int(ffplay_get_frame_width(currentId_), ffplay_get_frame_height(currentId_));
            while (RawVideoFrameSize.x <= 0 || RawVideoFrameSize.y <= 0)
            {
                yield return null;
                RawVideoFrameSize = new Vector2Int(ffplay_get_frame_width(currentId_), ffplay_get_frame_height(currentId_));
            }

            if (VideoTexture != null)
            {
                if (VideoTexture.VideoTexture != null)
                {
                    width_ = VideoTexture.VideoTexture.width;
                    height_ = VideoTexture.VideoTexture.height;
                }
                else
                {
                    width_ = RawVideoFrameSize.x;
                    height_ = RawVideoFrameSize.y;

                    tempTexture_ = new Texture2D(width_, height_, TextureFormat.RGBA32, false);
                    VideoTexture.VideoTexture = tempTexture_;
                }
            }
            else
            {
                width_ = RawVideoFrameSize.x;
                height_ = RawVideoFrameSize.y;
            }

            videoBufferSize_ = width_ * height_ * 4 * Marshal.SizeOf(typeof(byte));
            videoBufferPtr_ = Marshal.AllocHGlobal(videoBufferSize_);

            ffplay_set_video_buffer(currentId_, videoBufferPtr_, width_, height_);

            audioLock_ = false;
        }

        IEnumerator outputLogCoroutine()
        {
            if (!string.IsNullOrEmpty(logFilePath_))
            {
                FileStream stream;
                do
                {
                    yield return null;
                    try
                    {
                        stream = new FileStream(logFilePath_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    }
                    catch (FileNotFoundException)
                    {
                        stream = null;
                    }
                } while (stream == null);

                streamReader_ = new StreamReader(stream);
                while (streamReader_ != null)
                {
                    if (streamReader_.EndOfStream)
                    {
                        yield return null;
                        continue;
                    }

                    UnityEngine.Debug.Log(streamReader_.ReadLine());
                }
            }
        }

        public void Play()
        {
            if (currentId_ < 0 || thread_ == null || thread_.ThreadState != ThreadState.Running)
            {
                Init();
            }
        }

        void Start()
        {
#if FFMPEG_UNITY_USE_BINARY_WIN || FFMPEG_UNITY_USE_BINARY_MAC || FFMPEG_UNITY_USE_BINARY_LINUX
            UnityEngine.Debug.LogWarning("FfplayCommand is for \"Library\" only.");
#endif

            if (ExecuteOnStart)
            {
                Play();
            }
        }

        void drawDefault()
        {
            Texture texture = VideoTexture.VideoTexture;
            Texture2D videoTexture;
            if (texture is RenderTexture)
            {
                videoTexture = new Texture2D(width_, height_, TextureFormat.RGBA32, false);
            }
            else
            {
                videoTexture = texture as Texture2D;
            }
            if (videoTexture == null)
            {
                return;
            }

            videoTexture.LoadRawTextureData(videoBufferPtr_, videoBufferSize_);
            videoTexture.Apply();

            if (texture is RenderTexture)
            {
                Graphics.Blit(videoTexture, texture as RenderTexture);
                UnityEngine.Object.Destroy(videoTexture);
            }
        }

        void Update()
        {
            if (currentId_ < 0 || ffplay_is_running(currentId_) == 0)
            {
                return;
            }

#if true//UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX || !UNITY_EDITOR && (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX)

#else
            if (audioDataSize_ > 0)
            {
                while (tempAudioDatasCount_ > 0)
                {
                    float[] floatData = new float[audioDataSize_];
                    getAudio(floatData);
                    audioDatas_.Add(floatData);

                    tempAudioDatasCount_--;
                }
            }
#endif

            if (VideoTexture == null || videoBufferSize_ <= 0 || videoBufferPtr_ == IntPtr.Zero)
            {
                return;
            }

#if !UNITY_EDITOR && UNITY_ANDROID
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
            {
                Texture texture = VideoTexture.VideoTexture;

                if (!openGLIds_.ContainsKey(texture))
                {
                    drawDefault();
                    openGLIds_[texture] = openGLSetupNativeTextureRender(texture.GetNativeTexturePtr(), width_, height_);
                }
                else
                {
                    int id = openGLIds_[texture];
                    openGLCopyBuffer(id, videoBufferPtr_);
                    GL.IssuePluginEvent(openGLGetRenderEventFunc(), id);
                }
            }
            else
            {
                drawDefault();
            }
#elif !UNITY_EDITOR && UNITY_IOS
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Metal)
            {
                Texture texture = VideoTexture.VideoTexture;

                if (!metalIds_.ContainsKey(texture))
                {
                    drawDefault();
                    metalIds_[texture] = metalSetupNativeTextureRender(texture.GetNativeTexturePtr());
                }
                else
                {
                    int id = metalIds_[texture];
                    metalCopyBuffer(id, videoBufferPtr_);
                    var callback = metalGetRenderEventFunc();
                    if (commandBuffer_ == null)
                    {
                        commandBuffer_ = new CommandBuffer();
                        commandBuffer_.name = "MetalTextureUpdate";
                        commandBuffer_.IssuePluginEvent(callback, id);
                    }
                    Graphics.ExecuteCommandBufferAsync(commandBuffer_, ComputeQueueType.Default);
                }
            }
            else
            {
                drawDefault();
            }
#else
            drawDefault();
#endif
        }

        public void Dispose()
        {
            if (thread_ == null || thread_.ThreadState != ThreadState.Running)
            {
                currentId_ = -1;
            }

#if !UNITY_EDITOR && UNITY_ANDROID
            foreach (var id in openGLIds_.Values)
            {
                openGLFinishNativeTextureRender(id);
            }
            openGLIds_.Clear();
#elif !UNITY_EDITOR && UNITY_IOS
            foreach (var id in metalIds_.Values)
            {
                metalFinishNativeTextureRender(id);
            }
            metalIds_.Clear();
#endif

            if (currentId_ > 0)
            {
                ffplay_stop(currentId_);
                currentId_ = -1;
            }

            streamingAssetsCopyPathClass_.ExecuteDeleteAssets();
        }

        void OnDestroy()
        {
            Dispose();

            if (tempTexture_ != null)
            {
                Destroy(tempTexture_);
                tempTexture_ = null;
            }
        }

        void getAudio(float[] data)
        {
            short[] shortData = new short[data.Length];
            if (ffplay_get_audio(currentId_, shortData, shortData.Length * 2) >= 0)
            {
                if (seekingAudioFrames_ <= 0)
                {
                    for (int loop = 0; loop < shortData.Length; loop++)
                    {
                        data[loop] = shortData[loop] / ((float)short.MaxValue + 1);
                    }
                }
                else
                {
                    seekingAudioFrames_--;
                }
            }
        }

        public void OnAudioFilterReadFromFfplayAudio(float[] data, int channels)
        {
            channels_ = channels;
            audioDataSize_ = data.Length;
            if (currentId_ < 0 || ffplay_is_running(currentId_) == 0)
            {
                return;
            }

            if (audioLock_)
            {
                return;
            }
            audioLock_ = true;

#if true//UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX || !UNITY_EDITOR && (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX)
            getAudio(data);
#else
            tempAudioDatasCount_++;
            if (audioDatas_.Count > 0)
            {
                Array.Copy(audioDatas_[0], data, data.Length);
                audioDatas_.RemoveAt(0);
            }
#endif

            audioLock_ = false;
        }

        public void TogglePause()
        {
            ffplay_toggle_pause(currentId_);
        }

        public void ToggleMuted()
        {
            ffplay_toggle_mute(currentId_);
        }

        public void Seek(double pos)
        {
            if (AudioSourceComponent != null)
            {
                seekingAudioFrames_ = SEEKING_AUDIO_FRAMES_START;
            }
            prevCurrentTime_ = pos * Duration;
            ffplay_seek(currentId_, pos);
        }

        public void SeekTime(double time)
        {
            if (AudioSourceComponent != null)
            {
                seekingAudioFrames_ = SEEKING_AUDIO_FRAMES_START;
            }
            prevCurrentTime_ = time;
            ffplay_seek(currentId_, time / Duration);
        }

        public void StreamCycleChannel(int codec_type)
        {
            ffplay_stream_cycle_channel(currentId_, codec_type);
        }

        public void StepToNextFrame()
        {
            ffplay_step_to_next_frame(currentId_);
        }

        public void SeekIncrSeconds(double incr)
        {
            if (AudioSourceComponent != null)
            {
                seekingAudioFrames_ = SEEKING_AUDIO_FRAMES_START;
            }
            ffplay_seek_incr_seconds(currentId_, incr);
        }

        public void SeekChapter(int incr)
        {
            if (AudioSourceComponent != null)
            {
                seekingAudioFrames_ = SEEKING_AUDIO_FRAMES_START;
            }
            ffplay_seek_chapter(currentId_, incr);
        }

#if false//UNITY_EDITOR
        static void onReload()
        {
            var commands = FindObjectsOfType<FfplayCommand>();
            foreach (var command in commands)
            {
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        static void OnScriptsReloaded()
        {
            onReload();
        }

        [InitializeOnLoad]
        public class Startup
        {
            static Startup()
            {
                AssemblyReloadEvents.beforeAssemblyReload += onReload;
            }
        }
#endif
    }
}
