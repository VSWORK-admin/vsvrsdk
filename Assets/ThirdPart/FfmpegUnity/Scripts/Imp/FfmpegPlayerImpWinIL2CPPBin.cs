using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegPlayerImpWinIL2CPPBin : FfmpegPlayerImpBase
    {
        FfmpegExecuteIL2CPPWin execute_;
        TextReader reader_;

        Dictionary<int, string> pipeFileNames_ = new Dictionary<int, string>();

        public FfmpegPlayerImpWinIL2CPPBin(FfmpegPlayerCommand playerCommand) : base(playerCommand) {}

        public override IEnumerator OpenFfprobeReaderCoroutine(string inputPathAll)
        {
#if FFMPEG_UNITY_USE_OUTER_WIN
            bool useBuiltIn = false;
#else
            bool useBuiltIn = true;
#endif

            var pipeOption = new FfmpegExecuteIL2CPPWin.PipeOption();
            pipeOption.BlockSize = -1;
            pipeOption.BufferSize = 1024;
            pipeOption.PipeName = "FfmpegUnity_" + Guid.NewGuid().ToString();
            pipeOption.StdMode = 1;
            execute_ = new FfmpegExecuteIL2CPPWin();
            execute_.ExecuteSingle(useBuiltIn, "ffprobe", "-i \"" + inputPathAll + "\" -show_streams", pipeOption);
            while (execute_.GetStream(0) == null)
            {
                yield return null;
            }

            reader_ = new StreamReader(execute_.GetStream(0));
        }

        public override TextReader OpenFfprobeReader(string inputPathAll)
        {
            return reader_;
        }

        public override void CloseFfprobeReader()
        {
            if (reader_ != null)
            {
                try
                {
                    reader_.ReadToEnd();
                    reader_.Dispose();
                }
                catch (Exception)
                {

                }
                reader_ = null;
            }

            if (execute_ != null)
            {
                execute_.Dispose();
                execute_ = null;
            }
        }

        public override string BuildVideoOptions(int streamId, int width, int height)
        {
            string pipeName = "FfmpegUnity_" + Guid.NewGuid().ToString();
            string pipeFileName = @"\\.\pipe\" + pipeName;

            var pipeOption2 = new FfmpegExecuteIL2CPPWin.PipeOption();
            pipeOption2.BlockSize = width * height * 4;
            pipeOption2.BufferSize = width * height * 4;
            pipeOption2.PipeName = pipeName;
            pipeOption2.StdMode = 0;
            ((FfmpegCommandImpWinIL2CPPBin)PlayerCommand.CommandImp).AddPipeOptions(pipeOption2);

            pipeFileNames_[streamId] = pipeFileName;

            return " -f rawvideo -pix_fmt rgba \"" + pipeFileName + "\" ";
        }

        public override string BuildAudioOptions(int streamId, int sampleRate, int channels)
        {
            string pipeName = "FfmpegUnity_" + Guid.NewGuid().ToString();
            string pipeFileName = @"\\.\pipe\" + pipeName;

            var pipeOption2 = new FfmpegExecuteIL2CPPWin.PipeOption();
            pipeOption2.BlockSize = 1024;
            pipeOption2.BufferSize = sampleRate * 4;
            pipeOption2.PipeName = pipeName;
            pipeOption2.StdMode = 0;
            ((FfmpegCommandImpWinIL2CPPBin)PlayerCommand.CommandImp).AddPipeOptions(pipeOption2);

            pipeFileNames_[streamId] = pipeFileName;

            return " -f f32le \"" + pipeFileName + "\" ";
        }

        public override void ReadVideo(int streamId, int width, int height)
        {
            var commandImp = PlayerCommand.CommandImp as FfmpegCommandImpWinIL2CPPBin;
            var execute = commandImp.ExecuteObj;
            while (execute == null)
            {
                Thread.Sleep(1);
                execute = commandImp.ExecuteObj;
            }

            var stream = execute.GetStream(streamId);
            while (stream == null)
            {
                Thread.Sleep(1);
                stream = execute.GetStream(streamId);
            }

            using (var reader = new BinaryReader(stream))
            {
                StreamReadVideo(reader, streamId, width, height);
            }
        }

        public override void ReadAudio(int streamId, int sampleRate, int channels)
        {
            var commandImp = PlayerCommand.CommandImp as FfmpegCommandImpWinIL2CPPBin;
            var execute = commandImp.ExecuteObj;
            while (execute == null)
            {
                Thread.Sleep(1);
                execute = commandImp.ExecuteObj;
            }

            var stream = execute.GetStream(streamId);
            while (stream == null)
            {
                Thread.Sleep(1);
                stream = execute.GetStream(streamId);
            }

            using (var reader = new BinaryReader(stream))
            {
                StreamReadAudio(reader, streamId);
            }
        }
    }
}
