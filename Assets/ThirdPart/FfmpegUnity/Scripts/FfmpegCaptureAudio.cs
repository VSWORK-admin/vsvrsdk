using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegUnity
{
    //[RequireComponent(typeof(AudioSource))]
    public class FfmpegCaptureAudio : MonoBehaviour
    {
        public int StreamId
        {
            set;
            get;
        }

        public FfmpegCaptureCommand Capture
        {
            set;
            get;
        }

        public bool Mute
        {
            set;
            get;
        } = false;

        void OnAudioFilterRead(float[] data, int channels)
        {
            Capture.OnAudioFilterWriteToCaptureAudio(data, channels, StreamId);

            if (Mute)
            {
                for (int loop = 0; loop < data.Length; loop++)
                {
                    data[loop] = 0f;
                }
            }
        }
    }
}
