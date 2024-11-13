using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegCaptureImpWinIL2CPPBin : FfmpegCaptureImpBase
    {
        public FfmpegCaptureImpWinIL2CPPBin(FfmpegCaptureCommand captureCommand) : base(captureCommand)
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
            FfmpegCommandImpWinIL2CPPBin commandImp = (FfmpegCommandImpWinIL2CPPBin)CaptureCommand.CommandImp;

            var pipeOption = new FfmpegExecuteIL2CPPWin.PipeOption();
            pipeOption.BlockSize = width * height * 4;
            pipeOption.BufferSize = width * height * 4;
            pipeOption.PipeName = pipeFileName.Replace(@"\\.\pipe\", "");
            pipeOption.StdMode = 3;
            commandImp.AddPipeOptions(pipeOption);

            while (commandImp.ExecuteObj == null || commandImp.ExecuteObj.GetStream(streamId) == null || !commandImp.ExecuteObj.GetStream(streamId).CanWrite)
            {
                Thread.Sleep(1);
                commandImp = (FfmpegCommandImpWinIL2CPPBin)CaptureCommand.CommandImp;
            }

            try
            {
                using (var writer = new BinaryWriter(commandImp.ExecuteObj.GetStream(streamId)))
                {
                    StreamWriteVideo(writer, streamId, videoBuffers);
                }
            }
            catch (IOException)
            {
                IsEnd = true;
            }
        }

        public override void WriteAudio(int streamId, string pipeFileName, int sampleRate, int channels, Dictionary<int, List<float>> audioBuffers)
        {
            FfmpegCommandImpWinIL2CPPBin commandImp = (FfmpegCommandImpWinIL2CPPBin)CaptureCommand.CommandImp;

            var pipeOption = new FfmpegExecuteIL2CPPWin.PipeOption();
            pipeOption.BlockSize = 1024;
            pipeOption.BufferSize = 48000 * 4;
            pipeOption.PipeName = pipeFileName.Replace(@"\\.\pipe\", "");
            pipeOption.StdMode = 3;
            commandImp.AddPipeOptions(pipeOption);

            while (commandImp.ExecuteObj == null || commandImp.ExecuteObj.GetStream(streamId) == null || !commandImp.ExecuteObj.GetStream(streamId).CanWrite)
            {
                Thread.Sleep(1);
                commandImp = (FfmpegCommandImpWinIL2CPPBin)CaptureCommand.CommandImp;
            }

            try
            {
                using (var writer = new BinaryWriter(commandImp.ExecuteObj.GetStream(streamId)))
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