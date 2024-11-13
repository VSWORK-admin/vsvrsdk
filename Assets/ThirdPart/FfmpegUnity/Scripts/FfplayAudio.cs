using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegUnity
{
    [RequireComponent(typeof(AudioSource))]
    public class FfplayAudio : MonoBehaviour
    {
        public FfplayCommand Ffplay
        {
            set;
            get;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            Ffplay.OnAudioFilterReadFromFfplayAudio(data, channels);
        }
    }
}
