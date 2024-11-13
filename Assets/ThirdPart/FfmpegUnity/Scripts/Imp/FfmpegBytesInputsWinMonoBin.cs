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
    public class FfmpegBytesInputsWinMonoBin : FfmpegBytesInputs
    {
        public FfmpegBytesInputsWinMonoBin(string[] inputOptions, FfmpegCommand command) : base(inputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            return @"\\.\pipe\FfmpegUnity_" + Guid.NewGuid().ToString();
        }

        protected override void Write(string pipeFileName, int streamId)
        {
            using (var stream = new NamedPipeServerStream(pipeFileName.Replace(@"\\.\pipe\", ""),
                PipeDirection.Out,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.WriteThrough,
                BUFFER_SIZE,
                BUFFER_SIZE))
            {
                Thread thread = new Thread(() =>
                {
                    while (!IsStop && !stream.IsConnected)
                    {
                        Thread.Sleep(1);
                    }
                    if (!stream.IsConnected)
                    {
                        using (var dummyStream = new NamedPipeClientStream(".", pipeFileName.Replace(@"\\.\pipe\", ""), PipeDirection.In))
                        {
                            dummyStream.Connect();
                        }
                    }
                });
                thread.Start();
                Threads.Add(thread);

                stream.WaitForConnection();

                try
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        StreamWrite(writer, streamId);
                    }
                }
                catch (IOException)
                {
                    IsStop = true;
                }
            }
        }
    }
}
