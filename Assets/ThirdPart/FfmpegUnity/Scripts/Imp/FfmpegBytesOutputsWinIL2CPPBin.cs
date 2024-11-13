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
    public class FfmpegBytesOutputsWinIL2CPPBin : FfmpegBytesOutputs
    {
        public FfmpegBytesOutputsWinIL2CPPBin(string[] outputOptions, FfmpegCommand command) : base(outputOptions, command)
        {
        }

        protected override string GenerateFileName()
        {
            FfmpegCommandImpWinIL2CPPBin commandImp = (FfmpegCommandImpWinIL2CPPBin)CommandObj.CommandImp;

            var pipeOption2 = new FfmpegExecuteIL2CPPWin.PipeOption();
            pipeOption2.BlockSize = BLOCK_SIZE;
            pipeOption2.BufferSize = BUFFER_SIZE;
            pipeOption2.PipeName = "FfmpegUnity_" + Guid.NewGuid().ToString();
            pipeOption2.StdMode = 0;
            commandImp.AddPipeOptions(pipeOption2);
            return @"\\.\pipe\" + pipeOption2.PipeName;
        }

        protected override void Read(string pipeFileName, int streamId)
        {
            FfmpegCommandImpWinIL2CPPBin commandImp = (FfmpegCommandImpWinIL2CPPBin)CommandObj.CommandImp;

            while (commandImp.ExecuteObj == null || commandImp.ExecuteObj.GetStream(streamId) == null || !commandImp.ExecuteObj.GetStream(streamId).CanRead)
            {
                Thread.Sleep(1);
                commandImp = (FfmpegCommandImpWinIL2CPPBin)CommandObj.CommandImp;
            }

            var stream = commandImp.ExecuteObj.GetStream(streamId);
            using (var reader = new BinaryReader(stream))
            {
                StreamRead(reader, streamId);
            }
        }
    }
}
