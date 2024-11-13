using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegUnity.Sample
{
    public class KeepScreen : MonoBehaviour
    {
        void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
