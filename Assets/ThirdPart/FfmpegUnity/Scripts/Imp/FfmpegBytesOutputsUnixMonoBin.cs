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
    public class FfmpegBytesOutputsUnixMonoBin : FfmpegBytesOutputs
    {
        public FfmpegBytesOutputsUnixMonoBin(string[] outputOptions, FfmpegCommand command) : base(outputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            string fileNameFifo = Application.temporaryCachePath + "/FfmpegUnity_" + Guid.NewGuid().ToString();

            ProcessStartInfo psInfoMkFifo = new ProcessStartInfo()
            {
                FileName = "mkfifo",
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = "\"" + fileNameFifo + "\"",
            };
            using (Process process = Process.Start(psInfoMkFifo))
            {
                process.WaitForExit();
            }

            return fileNameFifo;
        }

        protected override void Read(string pipeFileName, int streamId)
        {
            string fileName = pipeFileName;

            while (!File.Exists(fileName))
            {
                Thread.Sleep(1);
            }

            using (var stream = File.OpenRead(fileName))
            using (var reader = new BinaryReader(stream))
            {
                StreamRead(reader, streamId);
            }

            File.Delete(fileName);
        }
    }
}
