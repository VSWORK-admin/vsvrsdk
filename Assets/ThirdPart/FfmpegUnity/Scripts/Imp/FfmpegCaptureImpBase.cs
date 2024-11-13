using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public abstract class FfmpegCaptureImpBase
    {
        protected const float DELETE_TIME = 10f;

        public bool IsEnd
        {
            get;
            set;
        } = false;

        protected FfmpegCaptureCommand CaptureCommand
        {
            get;
            private set;
        } = null;

        public FfmpegCaptureImpBase(FfmpegCaptureCommand captureCommand)
        {
            CaptureCommand = captureCommand;

            if (AudioStreamId < 0)
            {
                for (int loop = 0; loop < CaptureCommand.CaptureSources.Length; loop++)
                {
                    if (CaptureCommand.CaptureSources[loop].Type == FfmpegCaptureCommand.CaptureSource.SourceType.Audio_AudioListener
                       || CaptureCommand.CaptureSources[loop].Type == FfmpegCaptureCommand.CaptureSource.SourceType.Audio_AudioSource)
                    {
                        AudioStreamId = loop;
                        break;
                    }
                }

                audioSampleRate_ = AudioSettings.outputSampleRate;
            }
        }

        protected virtual string GenerateCaptureFileName()
        {
            return null;
        }

        public virtual string GenerateCaptureFileNameVideo(int width, int height)
        {
            return GenerateCaptureFileName();
        }

        public virtual string GenerateCaptureFileNameAudio(int sampleRate, int channels)
        {
            return GenerateCaptureFileName();
        }

        public abstract bool Reverse
        {
            get;
        }

        protected int AudioStreamId
        {
            get;
            private set;
        } = -1;
        protected long AudioTotalSamples
        {
            get;
            private set;
        } = 0;
        protected void AddAudioTotalSamples(int streamId, int samples)
        {
            if (AudioStreamId == streamId)
            {
                AudioTotalSamples += samples;
            }
        }

        Dictionary<int, long> videoTotalFrames_ = new Dictionary<int, long>();
        protected void AddVideoTotalFrames(int streamId, int frames = 1)
        {
            if (!videoTotalFrames_.ContainsKey(streamId))
            {
                videoTotalFrames_[streamId] = frames;
                return;
            }

            videoTotalFrames_[streamId] += frames;
        }

        int audioSampleRate_;
        protected int AudioSampleRate
        {
            get
            {
                return audioSampleRate_;
            }
        }

        public int AudioChannels
        {
            get;
            set;
        } = 0;

        protected float DefaultFrameRate
        {
            get
            {
                foreach (var capture in CaptureCommand.CaptureSources)
                {
                    if (capture.Type == FfmpegCaptureCommand.CaptureSource.SourceType.Video_GameView
                        || capture.Type == FfmpegCaptureCommand.CaptureSource.SourceType.Video_Camera
                        || capture.Type == FfmpegCaptureCommand.CaptureSource.SourceType.Video_RenderTexture)
                    {
                        return capture.FrameRate;
                    }
                }
                return 0f;
            }
        }

        protected float VideoSyncWaitTime(int streamId)
        {
            if (AudioStreamId < 0)
            {
                return 0f;
            }

            float audioTime = AudioTotalSamples / (float)audioSampleRate_ / AudioChannels;
            float videoTime = 0f;
            if (videoTotalFrames_.ContainsKey(streamId) && CaptureCommand.CaptureSources.Length > streamId)
            {
                videoTime = videoTotalFrames_[streamId] / CaptureCommand.CaptureSources[streamId].FrameRate;
            }

            float ret = videoTime - audioTime;

            lock (videoTotalFrames_)
            {
                if (audioTime > DELETE_TIME * 2f)
                {
                    bool adjustFrag = true;
                    foreach (var videoKey in videoTotalFrames_.Keys.ToArray())
                    {
                        float videoTime2 = videoTotalFrames_[videoKey] / CaptureCommand.CaptureSources[videoKey].FrameRate;
                        if (videoTime2 <= DELETE_TIME * 2f)
                        {
                            adjustFrag = false;
                            break;
                        }
                    }
                    if (adjustFrag)
                    {
                        AudioTotalSamples -= (long)(audioSampleRate_ * AudioChannels * DELETE_TIME);
                        foreach (var videoKey in videoTotalFrames_.Keys.ToArray())
                        {
                            videoTotalFrames_[videoKey] -= (long)(CaptureCommand.CaptureSources[videoKey].FrameRate * DELETE_TIME);
                        }
                    }
                }
            }

            return ret;
        }

        protected void ResetVideoTime(int streamId)
        {
            float audioTime = AudioTotalSamples / (float)audioSampleRate_ / AudioChannels;
            videoTotalFrames_[streamId] = (long)(audioTime / CaptureCommand.CaptureSources[streamId].FrameRate);
        }

        public abstract void WriteVideo(int streamId, string pipeFileName, int width, int height, Dictionary<int, byte[]> videoBuffers);

        protected void StreamWriteVideo(BinaryWriter writer, int streamId, Dictionary<int, byte[]> videoBuffers)
        {
            float frameRate = CaptureCommand.CaptureSources[streamId].FrameRate;

            DateTime startTime = DateTime.Now;
            long frameCount = 0;

            while (!IsEnd)
            {
                if (!videoBuffers.ContainsKey(streamId))
                {
                    Thread.Sleep(1);
                    continue;
                }

                byte[] buffer;
                lock (videoBuffers)
                {
                    buffer = videoBuffers[streamId];
                }

                try
                {
                    /*
                    if (AudioStreamId >= 0)
                    {
                        while (!IsEnd && VideoSyncWaitTime(streamId) <= 0f)
                        {
                            if (writer.BaseStream.CanWrite)
                            {
                                writer.Write(buffer);
                                AddVideoTotalFrames(streamId);
                            }
                        }

                        while (!IsEnd && VideoSyncWaitTime(streamId) > 0f)
                        {
                            Thread.Sleep(1);
                        }
                    }
                    else
                    */
                    {
                        if (writer.BaseStream.CanWrite)
                        {
                            writer.Write(buffer);
                        }
                    }
                }
                catch (IOException)
                {
                    IsEnd = true;
                    break;
                }

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
                    if (sleepTime < 0.0)
                    {
                        sleepTime = 0.0;
                    }
                    Thread.Sleep((int)sleepTime);
                } while (!IsEnd && sleepTime > 0.0);
            }
        }

        public abstract void WriteAudio(int streamId, string pipeFileName, int sampleRate, int channels, Dictionary<int, List<float>> audioBuffers);

        protected void StreamWriteAudio(BinaryWriter writer, int streamId, Dictionary<int, List<float>> audioBuffers)
        {
            float frameRate = DefaultFrameRate;

            DateTime startTime = default;
            long frameCount = 0;

            while (!IsEnd)
            {
                if (!audioBuffers.ContainsKey(streamId) || audioBuffers[streamId] == null || audioBuffers[streamId].Count <= 0)
                {
                    Thread.Sleep(0);
                    continue;
                }

                lock (audioBuffers[streamId])
                {
                    int loopLength = audioBuffers[streamId].Count;
                    for (int loop = 0; loop < loopLength; loop++)
                    {
                        if (writer.BaseStream.CanWrite)
                        {
                            writer.Write(audioBuffers[streamId][loop]);
                        }
                    }
                    //AddAudioTotalSamples(streamId, loopLength);

                    audioBuffers[streamId].RemoveRange(0, loopLength);
                }

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
                    if (sleepTime < 0.0)
                    {
                        sleepTime = 0.0;
                    }
                    Thread.Sleep((int)sleepTime);
                } while (!IsEnd && sleepTime > 0.0);
            }
        }
    }
}