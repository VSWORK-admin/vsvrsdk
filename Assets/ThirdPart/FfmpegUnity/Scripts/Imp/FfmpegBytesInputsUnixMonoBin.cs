using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegBytesInputsUnixMonoBin : FfmpegBytesInputs
    {
        public FfmpegBytesInputsUnixMonoBin(string[] inputOptions, FfmpegCommand command) : base(inputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            string fileName = Application.temporaryCachePath + "/FfmpegUnity_" + Guid.NewGuid().ToString();

            ProcessStartInfo psInfoMkFifo = new ProcessStartInfo()
            {
                FileName = "mkfifo",
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = "\"" + fileName + "\"",
            };
            using (Process process = Process.Start(psInfoMkFifo))
            {
                process.WaitForExit();
            }

            return fileName;
        }

        protected override void Write(string pipeFileName, int streamId)
        {
            try
            {
                using (var stream = File.OpenWrite(pipeFileName))
                using (var writer = new BinaryWriter(stream))
                {
                    StreamWrite(writer, streamId);
                }
            }
            catch (IOException)
            {
                IsStop = true;
            }

            File.Delete(pipeFileName);
        }
    }
}
