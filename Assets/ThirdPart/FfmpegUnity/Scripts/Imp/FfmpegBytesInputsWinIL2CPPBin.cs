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
    public class FfmpegBytesInputsWinIL2CPPBin : FfmpegBytesInputs
    {
        public FfmpegBytesInputsWinIL2CPPBin(string[] inputOptions, FfmpegCommand command) : base(inputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            return @"\\.\pipe\FfmpegUnity_" + Guid.NewGuid().ToString();
        }

        protected override void Write(string pipeFileName, int streamId)
        {
            FfmpegCommandImpWinIL2CPPBin commandImp = (FfmpegCommandImpWinIL2CPPBin)CommandObj.CommandImp;

            var pipeOption = new FfmpegExecuteIL2CPPWin.PipeOption();
            pipeOption.BlockSize = BLOCK_SIZE;
            pipeOption.BufferSize = BUFFER_SIZE;
            pipeOption.PipeName = pipeFileName.Replace(@"\\.\pipe\", "");
            pipeOption.StdMode = 3;
            commandImp.AddLastPipeOptions(pipeOption);

            while (commandImp.ExecuteObj == null || commandImp.ExecuteObj.GetStream(streamId + commandImp.PipeOptionsCount - commandImp.LastPipeOptionsCount) == null)
            {
                Thread.Sleep(1);
            }

            try
            {
                using (var writer = new BinaryWriter(commandImp.ExecuteObj.GetStream(streamId + commandImp.PipeOptionsCount - commandImp.LastPipeOptionsCount)))
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
