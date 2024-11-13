using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegUnity
{
    [RequireComponent(typeof(AudioSource))]
    public class FfmpegPlayerAudio : MonoBehaviour
    {
        public int StreamId
        {
            set;
            get;
        }

        public FfmpegPlayerCommand Player
        {
            set;
            get;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            Player.OnAudioFilterReadFromPlayerAudio(data, channels, StreamId);
        }
    }
}
