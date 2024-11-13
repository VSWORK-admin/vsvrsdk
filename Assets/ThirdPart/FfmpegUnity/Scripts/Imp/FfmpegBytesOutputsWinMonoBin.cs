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
    public class FfmpegBytesOutputsWinMonoBin : FfmpegBytesOutputs
    {
        public FfmpegBytesOutputsWinMonoBin(string[] outputOptions, FfmpegCommand command) : base(outputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            return @"\\.\pipe\FfmpegUnity_" + Guid.NewGuid().ToString();
        }

        protected override void Read(string pipeFileName, int streamId)
        {
            using (var stream = new NamedPipeServerStream(pipeFileName.Replace(@"\\.\pipe\", ""),
                PipeDirection.In,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.WriteThrough,
                BUFFER_SIZE, BUFFER_SIZE))
            {
                Thread thread = new Thread(() =>
                {
                    while (!IsEnd && !stream.IsConnected)
                    {
                        Thread.Sleep(1);
                    }
                    if (!stream.IsConnected)
                    {
                        using (var dummyStream = new NamedPipeClientStream(".", pipeFileName.Replace(@"\\.\pipe\", ""), PipeDirection.Out))
                        {
                            dummyStream.Connect();
                        }
                    }
                });
                thread.Start();
                Threads.Add(thread);

                stream.WaitForConnection();

                using (var reader = new BinaryReader(stream))
                {
                    StreamRead(reader, streamId);
                }
            }
        }
    }
}
