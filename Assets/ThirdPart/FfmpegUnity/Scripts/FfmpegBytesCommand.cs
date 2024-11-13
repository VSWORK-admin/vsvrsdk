using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegBytesCommand : FfmpegCommand, FfmpegBytesInputs.IInputControl, FfmpegBytesOutputs.IOutputControl
    {
        public string[] InputOptions = new string[0];
        public string[] OutputOptions = new string[0];

        FfmpegBytesInputs bytesInputs_;
        FfmpegBytesOutputs bytesOutputs_;

        public bool InputBytesIsEmpty
        {
            get
            {
                if (bytesInputs_ == null)
                {
                    return true;
                }
                return bytesInputs_.IsEmpty;
            }
        }

        public int OutputOptionsCount
        {
            get
            {
                return OutputOptions.Length;
            }
        }

        public void AddInputBytes(byte[] bytes, int inputNo = 0)
        {
            StartCoroutine(addInputBytes(bytes, inputNo));
        }

        IEnumerator addInputBytes(byte[] bytes, int inputNo = 0)
        {
            while (bytesInputs_ == null)
            {
                yield return null;
            }

            bytesInputs_.AddInputBytes(bytes, inputNo);
        }

        public byte[] GetOutputBytes(int outputNo = 0)
        {
            if (bytesOutputs_ == null)
            {
                return null;
            }

            return bytesOutputs_.GetOutputBytes(outputNo);
        }

        protected override void Build()
        {
            RunOptions = "";

            if (InputOptions != null && InputOptions.Length > 0)
            {
                bytesInputs_ = FfmpegBytesInputs.GetNewInstance(InputOptions, this);
                RunOptions += bytesInputs_.BuildAndStart();
            }

            RunOptions += Options;

            if (OutputOptions != null && OutputOptions.Length > 0)
            {
                bytesOutputs_ = FfmpegBytesOutputs.GetNewInstance(OutputOptions, this);
                RunOptions += bytesOutputs_.BuildAndStart();
            }

            IsFinishedBuild = true;
        }

        protected override void Clean()
        {
            if (bytesOutputs_ != null)
            {
                bytesOutputs_.Dispose();
            }
            if (bytesInputs_ != null)
            {
                bytesInputs_.Dispose();
            }
        }
    }
}
