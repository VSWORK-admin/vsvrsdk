using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegWriteFromTexturesCommand : FfmpegCommand
    {
        public string InputOptions = "";
        public string OutputOptions = "";

        FfmpegBytesInputs bytesInputs_;

        Texture2D tempTexture_ = null;

        public bool IsEmpty
        {
            get
            {
                return bytesInputs_.IsEmpty;
            }
        }

        public IEnumerator WriteTexture(Texture inputTexture)
        {
            while (bytesInputs_ == null)
            {
                yield return null;
            }

            Texture2D texture2D = null;

            if (inputTexture is Texture2D)
            {
                if (((Texture2D)inputTexture).format == TextureFormat.RGBA32)
                {
                    texture2D = (Texture2D)inputTexture;
                }
                else
                {
                    if (tempTexture_ == null)
                    {
                        tempTexture_ = new Texture2D(inputTexture.width, inputTexture.height, TextureFormat.RGBA32, false);
                    }
                    tempTexture_.SetPixels(((Texture2D)inputTexture).GetPixels());
                    tempTexture_.Apply();
                    texture2D = tempTexture_;
                }
            }
            else if (inputTexture is RenderTexture)
            {
                if (tempTexture_ == null)
                {
                    tempTexture_ = new Texture2D(inputTexture.width, inputTexture.height, TextureFormat.RGBA32, false);
                }

                var tempTextureActive = RenderTexture.active;

                RenderTexture.active = (RenderTexture)inputTexture;

                tempTexture_.ReadPixels(new Rect(0, 0, tempTexture_.width, tempTexture_.height), 0, 0);
                tempTexture_.Apply();

                RenderTexture.active = tempTextureActive;

                texture2D = tempTexture_;
            }

            var videoBuffer = texture2D.GetRawTextureData<byte>().ToArray();
            var newVideoBuffer = new byte[texture2D.width * texture2D.height * 4];
            for (int y = 0; y < texture2D.height; y++)
            {
                Array.Copy(videoBuffer, y * texture2D.width * 4,
                    newVideoBuffer, (texture2D.height - y - 1) * texture2D.width * 4,
                    texture2D.width * 4);
            }

            while (!bytesInputs_.IsEmpty)
            {
                yield return null;
            }

            bytesInputs_.AddInputBytes(newVideoBuffer);
        }

        protected override void Build()
        {
            RunOptions = "";
            bytesInputs_ = FfmpegBytesInputs.GetNewInstance(new[] { " -f rawvideo -pix_fmt rgba " + InputOptions }, this);
            RunOptions += bytesInputs_.BuildAndStart();

            RunOptions += " " + OutputOptions;

            IsFinishedBuild = true;
        }

        protected override void Clean()
        {
            if (bytesInputs_ != null)
            {
                bytesInputs_.Dispose();
                bytesInputs_ = null;
            }

            if (tempTexture_ != null)
            {
                Destroy(tempTexture_);
                tempTexture_ = null;
            }
        }
    }
}
