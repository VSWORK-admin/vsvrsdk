using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegUnity.Sample
{
    [RequireComponent(typeof(FfmpegPlayerVideoTexture))]
    public class SetVideoSkybox : MonoBehaviour
    {
        public Skybox TargetSkybox;

        void Update()
        {
            TargetSkybox.material.SetTexture("_FrontTex", GetComponent<FfmpegPlayerVideoTexture>().VideoTexture);
        }
    }
}
