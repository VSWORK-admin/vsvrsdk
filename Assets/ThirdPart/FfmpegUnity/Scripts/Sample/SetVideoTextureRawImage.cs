using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FfmpegUnity.Sample
{
    [RequireComponent(typeof(FfmpegPlayerVideoTexture))]
    [RequireComponent(typeof(RawImage))]
    public class SetVideoTextureRawImage : MonoBehaviour
    {
        void Update()
        {
            GetComponent<RawImage>().texture = GetComponent<FfmpegPlayerVideoTexture>().VideoTexture;
        }
    }
}
