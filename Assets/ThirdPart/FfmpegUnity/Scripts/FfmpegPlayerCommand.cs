using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegPlayerCommand : FfmpegCommand
    {
        public string InputOptions = "";

        public FfmpegPath.DefaultPath DefaultPath = FfmpegPath.DefaultPath.NONE;
        public string InputPath = "";

        public bool AutoStreamSettings = true;
        public FfmpegStream[] Streams = new FfmpegStream[] {
            new FfmpegStream()
            {
                CodecType = FfmpegStream.Type.VIDEO,
                Width = 640,
                Height = 480,
            },
            new FfmpegStream()
            {
                CodecType = FfmpegStream.Type.AUDIO,
                Channels = 2,
                SampleRate = 48000,
            },
        };
        public float FrameRate = 30f;

        public string PlayerOptions = "";

        public FfmpegPlayerVideoTexture[] VideoTextures;
        public AudioSource[] AudioSources;

        public bool SyncFrameRate = true;

        public float BufferTime = 0f;

        float time_ = 0f;
        bool addDeltaTime_ = false;

        // Current play time of video.
        // If you want to set, call SetTime().
        public float Time
        {
            get
            {
                if (time_ < targetTime_)
                {
                    return targetTime_;
                }
                if (time_ > Duration && Duration > 0f)
                {
                    return Duration;
                }
                return time_;
            }
            protected set
            {
                if (BufferTime >= 0f)
                {
                    time_ = value - BufferTime;
                }
                else
                {
                    time_ = value;
                }
                addDeltaTime_ = false;
            }
        }

        float duration_ = 0f;

        // Duration of video.
        public float Duration
        {
            get
            {
                return duration_ - 1f / FrameRate * 2f;
            }
            private set
            {
                duration_ = value;
            }
        }
        bool isPlayingPrev_ = true;
        public bool IsPlaying
        {
            get
            {
                if (!setTime_)
                {
                    isPlayingPrev_ = !isEnd_;
                }

                return isPlayingPrev_;
            }
        }

        public int Frames
        {
            get;
            private set;
        } = 0;

        public bool StopPerFrame
        {
            get;
            set;
        } = false;

        bool isEnd_ = true;
        List<Thread> threads_ = new List<Thread>();

        Dictionary<int, List<byte[]>> videoBuffers_ = new Dictionary<int, List<byte[]>>();
        public void SetVideoBuffer(int id, byte[] newBuffer)
        {
            lock (videoBuffers_)
            {
                if (!videoBuffers_.ContainsKey(id))
                {
                    videoBuffers_[id] = new List<byte[]>();
                }

                videoBuffers_[id].Add(newBuffer);

                if (!StopPerFrame)
                {
                    if (!frameTimes_.ContainsKey(id))
                    {
                        frameTimes_[id] = new List<float>();
                        frameTimes_[id].Add(BufferTime);
                        lastFrameTimes_[id] = BufferTime;
                    }
                    else
                    {
                        frameTimes_[id].Add(lastFrameTimes_[id] + (float)TimeBase);
                        lastFrameTimes_[id] += (float)TimeBase;
                    }
                }
            }
        }
        Dictionary<int, int> widths_ = new Dictionary<int, int>();
        Dictionary<int, int> heights_ = new Dictionary<int, int>();
        Dictionary<int, float> startWriteTime_ = new Dictionary<int, float>();
        Dictionary<int, List<float>> frameTimes_ = new Dictionary<int, List<float>>();
        Dictionary<int, float> lastFrameTimes_ = new Dictionary<int, float>();

        Dictionary<int, List<float>> audioBuffers_ = new Dictionary<int, List<float>>();
        public void AddAudioBuffer(int id, float[] buffer)
        {
            lock (audioBuffers_)
            {
                audioBuffers_[id].AddRange(buffer);
            }
        }
        Dictionary<int, int> sampleRates_ = new Dictionary<int, int>();
        Dictionary<int, int> channels_ = new Dictionary<int, int>();

        double timeBase_ = 0.0;
        public double TimeBase
        {
            get
            {
                if (timeBase_ > 0.0)
                {
                    return timeBase_;
                }
                else if (Frames != 0)
                {
                    return Duration / Frames;
                }
                else
                {
                    return 0.0;
                }
            }
            private set
            {
                timeBase_ = value;
            }
        }

        bool setTime_ = false;
        float targetTime_ = -1f;

        FfmpegPlayerImpBase playerImp_;

        string ffprobedPath_ = null;

        List<Coroutine> coroutines_ = new List<Coroutine>();

        float outputSampleRate_;

        bool isStartedVideo_ = false;
        int bufferLengthAdd_ = 0;

        void Awake()
        {
            outputSampleRate_ = AudioSettings.outputSampleRate;
        }

        public bool IsEOF
        {
            get
            {
                if (playerImp_ == null)
                {
                    return false;
                }

                return playerImp_.IsEOF;
            }
        }

        [Serializable]
        public class FfmpegStream
        {
            public enum Type
            {
                OTHER = -1,
                VIDEO,
                AUDIO,
                DATA
            }
            public Type CodecType;
            public int Width;
            public int Height;
            public int Channels;
            public int SampleRate;
        }

        // Set play time of video.
        public void SetTime(float val)
        {
            if (isEnd_)
            {
                Time = val;
                targetTime_ = val;
                return;
            }

            setTime_ = true;
            targetTime_ = val;
            StopFfmpeg();
            Time = val;
            StartFfmpeg();
        }

        protected override void Build()
        {
            RunOptions = "";

            if (AutoStreamSettings /*&& ffprobedPath_ != FfmpegPath.PathWithDefault(DefaultPath, InputPath)*/)
            {
                ffprobedPath_ = FfmpegPath.PathWithDefault(DefaultPath, InputPath);
                Streams = new FfmpegStream[0];
                coroutines_.Add(StartCoroutine(allCoroutine()));
            }
            else
            {
                coroutines_.Add(StartCoroutine(ResetCoroutine(false)));
            }
        }

        IEnumerator allCoroutine()
        {
            yield return FfprobeCoroutine(FfmpegPath.PathWithDefault(DefaultPath, InputPath));
            yield return ResetCoroutine(true);
        }

        protected IEnumerator FfprobeCoroutine(string inputPathAll)
        {
            List<FfmpegStream> ffmpegStreams = new List<FfmpegStream>();

            playerImp_ = FfmpegPlayerImpBase.GetNewInstance(this);

            yield return playerImp_.OpenFfprobeReaderCoroutine(inputPathAll);
            TextReader reader = playerImp_.OpenFfprobeReader(inputPathAll);

            Thread ffprobeThread = new Thread(() =>
            {
                bool breakFlag = false;
                do
                {
                    string readStr;
                    do
                    {
                        readStr = reader.ReadLine();
                        if (readStr == null)
                        {
                            breakFlag = true;
                        }
                    } while (!breakFlag && readStr.Trim() != "[STREAM]");

                    if (breakFlag)
                    {
                        break;
                    }

                    FfmpegStream ffmpegStream = new FfmpegStream();

                    bool innerBreakFlag = false;
                    do
                    {
                        readStr = reader.ReadLine().Trim();
                        if (readStr.StartsWith("codec_type="))
                        {
                            string type = readStr.Substring("codec_type=".Length);
                            if (string.IsNullOrWhiteSpace(type))
                            {
                                type = reader.ReadLine().Trim();
                            }
                            if (type == "audio")
                            {
                                ffmpegStream.CodecType = FfmpegStream.Type.AUDIO;
                            }
                            else if (type == "video")
                            {
                                ffmpegStream.CodecType = FfmpegStream.Type.VIDEO;
                            }
                            else if (type == "data")
                            {
                                ffmpegStream.CodecType = FfmpegStream.Type.DATA;
                            }
                            else
                            {
                                ffmpegStream.CodecType = FfmpegStream.Type.OTHER;
                            }
                        }
                        if (ffmpegStream.CodecType == FfmpegStream.Type.VIDEO || ffmpegStream.CodecType == FfmpegStream.Type.AUDIO)
                        {
                            if (readStr.StartsWith("width="))
                            {
                                int val;
                                if (!int.TryParse(readStr.Substring("width=".Length), out val))
                                {
                                    if (!int.TryParse(reader.ReadLine().Replace("\r\n", "").Replace("\n", ""), out val))
                                    {
                                    }
                                }
                                ffmpegStream.Width = val;
                            }
                            else if (readStr.StartsWith("height="))
                            {
                                int val;
                                if (!int.TryParse(readStr.Substring("height=".Length), out val))
                                {
                                    if (!int.TryParse(reader.ReadLine().Replace("\r\n", "").Replace("\n", ""), out val))
                                    {
                                    }
                                }
                                ffmpegStream.Height = val;
                            }
                            else if (readStr.StartsWith("channels="))
                            {
                                int val;
                                if (!int.TryParse(readStr.Substring("channels=".Length), out val))
                                {
                                    if (!int.TryParse(reader.ReadLine().Replace("\r\n", "").Replace("\n", ""), out val))
                                    {
                                    }
                                }
                                ffmpegStream.Channels = val;
                            }
                            else if (readStr.StartsWith("duration="))
                            {
                                float val;
                                if (!float.TryParse(readStr.Substring("duration=".Length), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out val))
                                {
                                    if (!float.TryParse(reader.ReadLine().Replace("\r\n", "").Replace("\n", ""), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out val))
                                    {
                                    }
                                }
                                if (Duration > val || Duration <= 0f)
                                {
                                    Duration = val;
                                }
                            }
                            else if (readStr.StartsWith("TAG:DURATION=") && Duration <= 0f)
                            {
                                bool isSet = true;
                                DateTime time;
                                if (!DateTime.TryParse(readStr.Substring("TAG:DURATION=".Length), CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                                {
                                    if (!DateTime.TryParse(reader.ReadLine().Replace("\r\n", "").Replace("\n", ""), CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                                    {
                                        isSet = false;
                                    }
                                }
                                if (isSet)
                                {
                                    Duration = (float)time.TimeOfDay.TotalSeconds;
                                }
                            }
                            else if (readStr.StartsWith("sample_rate="))
                            {
                                int val;
                                if (!int.TryParse(readStr.Substring("sample_rate=".Length), out val))
                                {
                                    if (!int.TryParse(reader.ReadLine().Replace("\r\n", "").Replace("\n", ""), out val))
                                    {
                                    }
                                }
                                ffmpegStream.SampleRate = val;
                            }
                            else if (readStr.StartsWith("r_frame_rate="))
                            {
                                if (ffmpegStream.CodecType == FfmpegStream.Type.VIDEO)
                                {
                                    string[] baseStr = readStr.Substring("r_frame_rate=".Length).Split('/');
                                    float numerator, denominator;
                                    if (float.TryParse(baseStr[0].Replace("\r\n", "").Replace("\n", ""), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out numerator))
                                    {
                                        if (float.TryParse(baseStr[1].Replace("\r\n", "").Replace("\n", ""), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out denominator))
                                        {
                                            TimeBase = denominator / numerator;
                                        }
                                    }
                                }
                            }
                            else if (readStr.StartsWith("nb_frames="))
                            {
                                int val;
                                if (!int.TryParse(readStr.Substring("nb_frames=".Length), out val))
                                {
                                    if (!int.TryParse(reader.ReadLine().Replace("\r\n", "").Replace("\n", ""), out val))
                                    {
                                    }
                                }
                                if (Frames > val || Frames <= 0)
                                {
                                    Frames = val;
                                }
                            }
                        }
                        if (readStr == "[/STREAM]")
                        {
                            innerBreakFlag = true;
                        }
                    } while (!innerBreakFlag);

                    ffmpegStreams.Add(ffmpegStream);

                } while (!breakFlag);

                playerImp_.CloseFfprobeReader();
                reader = null;
            });
            ffprobeThread.Start();

            while (ffprobeThread.IsAlive)
            {
                yield return null;
            }

            Streams = Streams.Concat(ffmpegStreams).ToArray();
        }

        protected IEnumerator ResetCoroutine(bool maintainPlayerImp)
        {
            if (!maintainPlayerImp)
            {
                playerImp_ = FfmpegPlayerImpBase.GetNewInstance(this);
            }

            isEnd_ = false;
            playerImp_.IsEnd = false;

            if (TimeBase <= 0.0 && FrameRate > 0f)
            {
                TimeBase = 1.0 / FrameRate;
            }

            RunOptions += " -y ";
            if (playerImp_.IsGetTimeFromImp)
            {
                IsGetStdErr = PrintStdErr || GetProgressOnScript;
                if (!IsGetStdErr)
                {
                    RunOptions += " -loglevel quiet ";
                }
            }
            else
            {
                RunOptions += " -dump ";
            }
            if (Time + 0.5f < Duration)
            {
                RunOptions += " -ss " + Time.ToString(CultureInfo.InvariantCulture) + " ";
                playerImp_.StartTime = Time;
            }
            else
            {
                playerImp_.StartTime = 0f;
            }
            RunOptions += " " + InputOptions + " ";

            if (!string.IsNullOrEmpty(InputPath))
            {
                yield return playerImp_.BuildBeginOptionsCoroutine(FfmpegPath.PathWithDefault(DefaultPath, InputPath));
                RunOptions += playerImp_.BuildBeginOptions(FfmpegPath.PathWithDefault(DefaultPath, InputPath));
            }

            RunOptions += " " + PlayerOptions + " ";

            int streamCount = 0;
            int videoStreamCount = 0;
            int audioStreamCount = 0;
            foreach (var ffmpegStream in Streams)
            {
                if (ffmpegStream.CodecType == FfmpegStream.Type.VIDEO)
                {
                    if (videoStreamCount >= VideoTextures.Length)
                    {
                        continue;
                    }
                    var videoTexture = VideoTextures[videoStreamCount];

                    RunOptions += playerImp_.BuildVideoOptions(streamCount, ffmpegStream.Width, ffmpegStream.Height);

                    if (videoTexture.VideoTexture == null)
                    {
                        videoTexture.VideoTexture = new Texture2D(ffmpegStream.Width, ffmpegStream.Height, TextureFormat.RGBA32, false);
                    }
                    else if (videoTexture.VideoTexture is Texture2D && (videoTexture.VideoTexture.width != ffmpegStream.Width || videoTexture.VideoTexture.height != ffmpegStream.Height))
                    {
                        Texture2D oldTexture = (Texture2D)videoTexture.VideoTexture;
                        videoTexture.VideoTexture = new Texture2D(ffmpegStream.Width, ffmpegStream.Height, oldTexture.format, false);
                        Destroy(oldTexture);
                    }

                    widths_[streamCount] = ffmpegStream.Width;
                    heights_[streamCount] = ffmpegStream.Height;

                    int streamId = streamCount;
                    var thread = new Thread(() => { readVideo(streamId); });
                    //thread.Start();
                    threads_.Add(thread);

                    videoStreamCount++;
                }
                else
                {
                    if (AudioSources == null || AudioSources.Length <= audioStreamCount)
                    {
                        continue;
                    }

                    var audioSource = AudioSources[audioStreamCount];

                    if (ffmpegStream.SampleRate != AudioSettings.outputSampleRate)
                    {
                        RunOptions += " -af asetrate=" + ffmpegStream.SampleRate + " -ar " + AudioSettings.outputSampleRate + " ";
                    }

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
                    RunOptions += " -ac " + channels + " ";

                    RunOptions += playerImp_.BuildAudioOptions(streamCount, ffmpegStream.SampleRate, ffmpegStream.Channels);

                    audioSource.clip = AudioClip.Create("", AudioSettings.outputSampleRate * 2, ffmpegStream.Channels, AudioSettings.outputSampleRate, true);
                    audioSource.loop = true;
                    if (audioSource.playOnAwake)
                    {
                        audioSource.Play();
                    }

                    audioBuffers_[streamCount] = new List<float>();
                    sampleRates_[streamCount] = ffmpegStream.SampleRate;
                    channels_[streamCount] = ffmpegStream.Channels;

                    var playerAudio = audioSource.GetComponent<FfmpegPlayerAudio>();
                    if (playerAudio == null)
                    {
                        playerAudio = audioSource.gameObject.AddComponent<FfmpegPlayerAudio>();
                    }
                    playerAudio.StreamId = streamCount;
                    playerAudio.Player = this;

                    int streamId = streamCount;
                    var thread = new Thread(() => { readAudio(streamId); });
                    //thread.Start();
                    threads_.Add(thread);

                    audioStreamCount++;
                }

                streamCount++;
            }

            foreach (var thread in threads_)
            {
                thread.Start();
            }

            IsGetStdErr = true;
            IsFinishedBuild = true;
            setTime_ = false;
            if (playerImp_.IsGetTimeFromImp)
            {
                yield return startReadTimeImp();
            }
            else
            {
                yield return startReadTime();
            }
        }

        protected override void Clean()
        {
            isEnd_ = true;
            if (playerImp_ != null)
            {
                playerImp_.IsEnd = true;
            }

            foreach (var thread in threads_)
            {
                /*
                bool exited = thread.Join(1);
                while (!exited && IsRunning)
                {
                    exited = thread.Join(1);
                }
                if (!exited && !IsRunning)
                {
                    thread.Abort();
                }
                */
                thread.Join();
            }
            threads_ = new List<Thread>();

            if (playerImp_ != null)
            {
                playerImp_.Clean();
            }
            playerImp_ = null;

            startWriteTime_ = new Dictionary<int, float>();
            frameTimes_ = new Dictionary<int, List<float>>();
            videoBuffers_ = new Dictionary<int, List<byte[]>>();
            isStartedVideo_ = false;
        }

        protected override void CleanBuf()
        {
            if (playerImp_ != null)
            {
                playerImp_.CleanBuf();
            }
            playerImp_ = null;

            foreach (var coroutine in coroutines_)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            }
            coroutines_.Clear();
        }

        void readVideo(int streamId)
        {
            playerImp_.ReadVideo(streamId, widths_[streamId], heights_[streamId]);
        }

        void readAudio(int streamId)
        {
            playerImp_.ReadAudio(streamId, sampleRates_[streamId], channels_[streamId]);
        }

        IEnumerator startReadTime()
        {
            string readStr;

            do
            {
                readStr = GetStdErrLine();
                if (readStr == null)
                {
                    yield return null;
                }
                if (isEnd_)
                {
                    yield break;
                }
            } while (readStr == null || !readStr.StartsWith("stream #0:"));

            while (!isEnd_)
            {
                readStr = GetStdErrLine();
                if (readStr == null)
                {
                    yield return null;
                    continue;
                }

                if (!readStr.StartsWith("stream #"))
                {
                    continue;
                }

                int streamNo = int.Parse(readStr.Substring("stream #".Length).Split(':')[0]);

                do
                {
                    readStr = GetStdErrLine();
                    if (readStr == null)
                    {
                        yield return null;
                    }
                    if (isEnd_)
                    {
                        yield break;
                    }
                } while (readStr == null || !readStr.StartsWith("  dts="));

                string timeStr = readStr.Substring("  dts=".Length).Split(new string[] { "  pts=" }, StringSplitOptions.None)[0];
                float time;
                if (float.TryParse(timeStr, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out time))
                {
                    /*
                    if (!frameTimes_.ContainsKey(streamNo))
                    {
                        frameTimes_[streamNo] = new List<float>();
                    }
                    frameTimes_[streamNo].Add(time);
                    */

                    if (streamNo == 0 && Time < time)
                    {
                        Time = time;
                    }
                }
            }
        }

        IEnumerator startReadTimeImp()
        {
            while (!isEnd_)
            {
                float time = playerImp_.Time;
                if (time >= 0f)
                {
                    Time = time;
                }
                yield return null;
            }
        }

        protected bool WriteNextTexture()
        {
            bool ret = false;
            lock (videoBuffers_)
            {
                if (videoBuffers_ == null || videoBuffers_.Count <= 0)
                {
                    return false;
                }

                int videoLoop = 0;
                List<int> delKeys = new List<int>();
                foreach (var videoBuffer in videoBuffers_)
                {
                    if (videoBuffer.Value.Count <= 0 || videoBuffer.Value[0].Length <= 0)
                    {
                        videoLoop++;
                        continue;
                    }

                    if (!startWriteTime_.ContainsKey(videoBuffer.Key))
                    {
                        startWriteTime_[videoBuffer.Key] = UnityEngine.Time.realtimeSinceStartup;
                        isStartedVideo_ = true;
                    }

                    if (BufferTime >= 0f)
                    {
                        if (!frameTimes_.ContainsKey(videoBuffer.Key) || frameTimes_[videoBuffer.Key].Count <= 0
                            || frameTimes_[videoBuffer.Key][0] > UnityEngine.Time.realtimeSinceStartup - startWriteTime_[videoBuffer.Key])
                        {
                            videoLoop++;
                            continue;
                        }
                    }

                    if (VideoTextures[videoLoop].VideoTexture == null)
                    {
                        continue;
                    }

                    playerImp_.WriteTexture(VideoTextures[videoLoop].VideoTexture, videoBuffer.Value[0], widths_[videoBuffer.Key], heights_[videoBuffer.Key]);

                    videoLoop++;

                    ret = true;

                    delKeys.Add(videoBuffer.Key);
                }

                foreach (var key in delKeys)
                {
                    if (BufferTime < 0f)
                    {
                        videoBuffers_[key].Clear();
                        if (frameTimes_.ContainsKey(key))
                        {
                            frameTimes_[key].Clear();
                        }
                    }
                    else
                    {
                        videoBuffers_[key].RemoveAt(0);
                        if (frameTimes_.ContainsKey(key))
                        {
                            frameTimes_[key].RemoveAt(0);

                            while (!(!frameTimes_.ContainsKey(key) || frameTimes_[key].Count <= 0
                                || frameTimes_[key][0] > UnityEngine.Time.realtimeSinceStartup - startWriteTime_[key]))
                            {
                                videoBuffers_[key].RemoveAt(0);
                                frameTimes_[key].RemoveAt(0);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        protected override void Update()
        {
            base.Update();

            if (StopPerFrame)
            {
                return;
            }

            if (!setTime_)
            {
                isPlayingPrev_ = !isEnd_;
            }

            if (time_ >= Duration - 1f / FrameRate * 2f && Duration > 0f)
            {
                StopFfmpeg();
                Time = 0f;
                targetTime_ = 0.0f;
                return;
            }

            if (isEnd_)
            {
                return;
            }

            if (!addDeltaTime_)
            {
                addDeltaTime_ = true;
            }
            else if (Duration > 0f)
            {
                //time_ += UnityEngine.Time.deltaTime;
            }

            if (!IsRunning)
            {
                return;
            }

            WriteNextTexture();
        }

        public void OnAudioFilterReadFromPlayerAudio(float[] data, int channels, int streamId)
        {
            /*
            if (!isStartedVideo_)
            {
                bufferLengthAdd_ += data.Length;
                return;
            }
            */

            lock (audioBuffers_)
            {
                float bufferTime = BufferTime >= 0f ? BufferTime : 0f;
                int bufferLength = (int)(outputSampleRate_ * channels * BufferTime) + bufferLengthAdd_;
                int length = audioBuffers_[streamId].Count - bufferLength < data.Length ? audioBuffers_[streamId].Count - bufferLength : data.Length;
                if (length <= 0)
                {
                    return;
                }

                try
                {
                    for (int loop = 0; loop < length; loop++)
                    {
                        data[loop] = audioBuffers_[streamId][loop];
                    }
                    if (length <= audioBuffers_[streamId].Count)
                    {
                        audioBuffers_[streamId].RemoveRange(0, length);
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    UnityEngine.Debug.LogError(e.ToString());
                }
            }
        }

        public void StopPerFrameFunc(int streamId)
        {
            if (StopPerFrame)
            {
                while (!isEnd_)
                {
                    lock (videoBuffers_)
                    {
                        if (videoBuffers_.ContainsKey(streamId) && videoBuffers_[streamId].Count <= 0)
                        {
                            break;
                        }
                    }

                    Thread.Sleep(1);
                }
            }
        }
    }
}
