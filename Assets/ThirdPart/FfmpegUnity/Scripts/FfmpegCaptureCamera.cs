using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegCaptureCamera : MonoBehaviour
    {
        public FfmpegCaptureCommand CaptureCommand
        {
            get;
            set;
        } = null;

        public int CaptureId
        {
            get;
            set;
        } = -1;

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (CaptureCommand != null)
            {
                CaptureCommand.CaptureCameraRenderImage(src, CaptureId);
            }

            Graphics.Blit(src, dest);
        }
    }
}
