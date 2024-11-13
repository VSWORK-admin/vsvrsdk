using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegCaptureAudioListener : MonoBehaviour
    {
        public int StreamId
        {
            set;
            get;
        }

        public int Channels
        {
            set;
            get;
        }

        public int ReadCount
        {
            set;
            get;
        }

        public bool IsStart
        {
            get;
            private set;
        } = false;

        void OnAudioFilterRead(float[] data, int channels)
        {
            ReadCount += data.Length / channels;
        }

        public float[] Read()
        {
            if (ReadCount <= 0)
            {
                return new float[0];
            }

            int readCount = 1;
            while (ReadCount >= readCount)
            {
                readCount *= 2;
            }
            readCount /= 2;
            ReadCount -= readCount;

            float[] allSamples = new float[readCount * Channels];
            for (int loop = 0; loop < Channels; loop++)
            {
                float[] samples = new float[readCount];
                AudioListener.GetOutputData(samples, loop);
                for (int loop2 = 0; loop2 < readCount; loop2++)
                {
                    allSamples[loop2 * Channels + loop] = samples[loop2];
                }
            }

#if false//(UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN) && !FFMPEG_UNITY_USE_BINARY_WIN
            if (!IsStart)
            {
                int startPos = -1;
                for (int loop = 0; loop < allSamples.Length; loop++)
                {
                    if (allSamples[loop] != 0f)
                    {
                        startPos = loop;
                        break;
                    }
                }
                if (startPos >= 0)
                {
                    allSamples = allSamples.ToList().GetRange(startPos, allSamples.Length - startPos).ToArray();
                    IsStart = true;
                }
                else
                {
                    return new float[0];
                }
            }
#else
            IsStart = true;
#endif

            return allSamples;
        }
    }
}
