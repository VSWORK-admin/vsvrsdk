using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity.Sample
{
    public class BytesOutputTest : MonoBehaviour
    {
        public FfmpegCommand FromCommand;
        public string FileName;

        IEnumerator Start()
        {
            bool isFinished = false;
            List<byte> bytesList = new List<byte>();

            Thread thread = new Thread(() =>
            {
                while (!FromCommand.IsRunning)
                {
                    Thread.Sleep(0);
                }

                byte[] bytes;

                while (FromCommand.IsRunning)
                {
                    bytes = ((FfmpegBytesOutputs.IOutputControl)FromCommand).GetOutputBytes(0);
                    if (bytes != null)
                    {
                        bytesList.AddRange(bytes);
                    }
                    Thread.Sleep(0);
                }

                for (; ; )
                {
                    bytes = ((FfmpegBytesOutputs.IOutputControl)FromCommand).GetOutputBytes(0);
                    if (bytes != null)
                    {
                        bytesList.AddRange(bytes);
                    }
                    else
                    {
                        break;
                    }
                }

                isFinished = true;
            });
            thread.Start();

            while (!isFinished)
            {
                yield return null;
            }

            File.WriteAllBytes(Application.persistentDataPath + "/" + FileName, bytesList.ToArray());
        }
    }
}