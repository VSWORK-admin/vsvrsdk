using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity.Sample
{
    public class BatchTexture : MonoBehaviour
    {
        IEnumerator Start()
        {
            var videoTexture = GetComponent<FfmpegPlayerVideoTexture>();
            var writeCommand = GetComponent<FfmpegWriteFromTexturesCommand>();
            var readCommand = GetComponent<FfmpegGetTexturePerFrameCommand>();

            while (videoTexture.VideoTexture == null || !readCommand.IsRunning || !writeCommand.IsRunning)
            {
                yield return null;
            }

            int frameCount = 0;
            while (!readCommand.IsEOF)
            {
                yield return readCommand.GetNextFrame();
                if (readCommand.IsEOF || !readCommand.IsRunning)
                {
                    break;
                }

                Texture texture = videoTexture.VideoTexture;

                // Insert processing.

                yield return writeCommand.WriteTexture(texture);

                frameCount++;
            }

            readCommand.StopFfmpeg();
            writeCommand.StopFfmpeg();
        }
    }
}
