using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegUnity.Sample
{
    [RequireComponent(typeof(FfmpegPlayerVideoTexture))]
    [RequireComponent(typeof(Renderer))]
    public class SetVideoTexture : MonoBehaviour
    {
        void Update()
        {
            GetComponent<Renderer>().material.mainTexture = GetComponent<FfmpegPlayerVideoTexture>().VideoTexture;
        }
    }
}
