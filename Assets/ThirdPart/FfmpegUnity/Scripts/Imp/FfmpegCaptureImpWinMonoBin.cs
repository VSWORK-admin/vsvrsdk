using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegCaptureImpWinMonoBin : FfmpegCaptureImpBase
    {
        public FfmpegCaptureImpWinMonoBin(FfmpegCaptureCommand captureCommand) : base(captureCommand)
        {
        }

        protected override string GenerateCaptureFileName()
        {
            return @"\\.\pipe\FfmpegUnity_" + Guid.NewGuid().ToString();
        }

        public override bool Reverse
        {
            get
            {
                return false;
            }
        }

        public override void WriteVideo(int streamId, string pipeFileName, int width, int height, Dictionary<int, byte[]> videoBuffers)
        {
            using (var stream = new NamedPipeServerStream(pipeFileName.Replace(@"\\.\pipe\", ""),
                PipeDirection.Out,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.WriteThrough,
                width * height * 4,
                width * height * 4))
            {
                stream.WaitForConnection();

                try
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        StreamWriteVideo(writer, streamId, videoBuffers);
                    }
                }
                catch (IOException)
                {
                    IsEnd = true;
                }
            }
        }

        public override void WriteAudio(int streamId, string pipeFileName, int sampleRate, int channels, Dictionary<int, List<float>> audioBuffers)
        {
            using (var stream = new NamedPipeServerStream(pipeFileName.Replace(@"\\.\pipe\", ""),
                PipeDirection.Out,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.WriteThrough,
                sampleRate * 4, sampleRate * 4))
            {
                stream.WaitForConnection();

                try
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        StreamWriteAudio(writer, streamId, audioBuffers);
                    }
                }
                catch (IOException)
                {
                    IsEnd = true;
                }
            }
        }
    }
}