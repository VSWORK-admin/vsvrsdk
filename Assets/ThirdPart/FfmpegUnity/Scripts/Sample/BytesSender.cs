using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegUnity.Sample
{
    public class BytesSender : MonoBehaviour
    {
        public FfmpegCommand FromCommand;
        public FfmpegCommand ToCommand;

        bool isStarted_ = false;

        void Update()
        {
            if (FromCommand.IsRunning)
            { 
                for (int loop = 0; loop < ((FfmpegBytesOutputs.IOutputControl)FromCommand).OutputOptionsCount; loop++)
                {
                    byte[] bytes;
                    do
                    {
                        bytes = ((FfmpegBytesOutputs.IOutputControl)FromCommand).GetOutputBytes(loop);
                        if (bytes != null && bytes.Length > 0)
                        {
                            ((FfmpegBytesInputs.IInputControl)ToCommand).AddInputBytes(bytes, loop);
                            isStarted_ = true;
                        }
                    } while (bytes != null && bytes.Length > 0);
                }
            }
            else if (isStarted_ && ((FfmpegBytesInputs.IInputControl)ToCommand).InputBytesIsEmpty)
            {
                ToCommand.StopFfmpeg();
            }
        }
    }
}