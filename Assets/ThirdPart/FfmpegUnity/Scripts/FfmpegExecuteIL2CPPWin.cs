using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public class FfmpegExecuteIL2CPPWin : IDisposable
    {
        string id_;
        NamedPipeClientStream[] streams_;

        public class PipeOption
        {
            public int BlockSize;
            public int BufferSize;
            public string PipeName;
            public int StdMode; // 0=‚È‚µ 1=stdout 2=stderr 3=‹t’ÊM
        }

        public void ExecuteSingle(bool useBuiltIn, string command, string options, PipeOption pipeOption)
        {
            clean();

            string tempPath = Application.temporaryCachePath;

            id_ = Guid.NewGuid().ToString();

            if (useBuiltIn)
            {
                command = "\"" + Application.streamingAssetsPath + "/_FfmpegUnity_temp/" + command + ".exe\"";
            }
            command = "\"" + Application.streamingAssetsPath + "/_FfmpegUnity_temp/NamedPipeConnecter.exe\" 1 \"" + 
                tempPath + "/FfmpegUnity_" + id_ + ".ready\" " + 
                pipeOption.BlockSize + " " + pipeOption.BufferSize + " \"" + pipeOption.PipeName + "\" " + pipeOption.StdMode + " " + 
                command + " " + options;
            command = command.Replace("\"", "\"\"");

            string vbs = "Set objWShell = CreateObject(\"WScript.Shell\")\r\nobjWShell.Run \"" +
                command + "\", 0, False";
            File.WriteAllText(tempPath + "/FfmpegUnity_" + id_ + ".vbs", vbs);

            Application.OpenURL(tempPath + "/FfmpegUnity_" + id_ + ".vbs");

            streams_ = new NamedPipeClientStream[1];
            Thread thread = new Thread(() =>
            {
                while (!File.Exists(tempPath + "/FfmpegUnity_" + id_ + ".ready"))
                {
                    Thread.Sleep(1);
                }

                NamedPipeClientStream stream;
                if (pipeOption.StdMode != 3)
                {
                    stream = new NamedPipeClientStream(".", pipeOption.PipeName + "_output", PipeDirection.In);
                }
                else
                {
                    stream = new NamedPipeClientStream(".", pipeOption.PipeName + "_input", PipeDirection.Out);
                }
                stream.Connect();
                streams_[0] = stream;
            });
            thread.Start();
        }

        public void Execute(bool useBuiltIn, string command, string options, PipeOption[] pipeOptions)
        {
            clean();

            string tempPath = Application.temporaryCachePath;

            id_ = Guid.NewGuid().ToString();

            if (useBuiltIn)
            {
                command = "\"" + Application.streamingAssetsPath + "/_FfmpegUnity_temp/" + command + ".exe\"";
            }
            string command2 = "\"" + Application.streamingAssetsPath + "/_FfmpegUnity_temp/NamedPipeConnecter.exe\" " + pipeOptions.Length + " " + tempPath + "/FfmpegUnity_" + id_ + ".ready ";
            foreach (var pipeOption in pipeOptions)
            {
                command2 += " " + pipeOption.BlockSize + " " + pipeOption.BufferSize + " \"" + pipeOption.PipeName + "\" " + pipeOption.StdMode + " ";
            }
            command2 += " " + command + " " + options;
            command = command2.Replace("\"", "\"\"");

            string vbs = "Set objWShell = CreateObject(\"WScript.Shell\")\r\nobjWShell.Run \"" +
                command + "\", 0, False";
            File.WriteAllText(tempPath + "/FfmpegUnity_" + id_ + ".vbs", vbs);

            Application.OpenURL(tempPath + "/FfmpegUnity_" + id_ + ".vbs");

            streams_ = new NamedPipeClientStream[pipeOptions.Length];
            for (int loop = 0; loop < pipeOptions.Length; loop++)
            {
                int streamId = loop;
                Thread thread = new Thread(() =>
                {
                    while (!File.Exists(tempPath + "/FfmpegUnity_" + id_ + ".ready"))
                    {
                        Thread.Sleep(1);
                    }

                    NamedPipeClientStream stream;
                    if (pipeOptions[streamId].StdMode != 3)
                    {
                        stream = new NamedPipeClientStream(".", pipeOptions[streamId].PipeName + "_output", PipeDirection.In);
                    }
                    else
                    {
                        stream = new NamedPipeClientStream(".", pipeOptions[streamId].PipeName + "_input", PipeDirection.Out);
                    }
                    stream.Connect();
                    streams_[streamId] = stream;
                });
                thread.Start();
            }
        }

        public NamedPipeClientStream GetStream(int index)
        {
            if (index < 0 || streams_ == null || index >= streams_.Length)
            {
                return null;
            }
            return streams_[index];
        }

        void clean()
        {
            File.WriteAllText(Application.temporaryCachePath + "/FfmpegUnity_" + id_ + ".ready.exit", "");

            if (!string.IsNullOrEmpty(id_))
            {
                File.Delete(Application.temporaryCachePath + "/FfmpegUnity_" + id_ + ".vbs");
                id_ = null;
            }

            if (streams_ != null)
            {
                foreach (var stream in streams_)
                {
                    stream.Dispose();
                }
                streams_ = null;
            }
        }

        public void Dispose()
        {
            clean();
        }
    }
}
