using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace FfmpegUnity.Sample
{
    public class BytesInputTest : MonoBehaviour
    {
        public string FileName;
        public FfmpegBytesCommand ToCommand;

        IEnumerator Start()
        {
            byte[] bytes;
            if (Application.streamingAssetsPath.Contains("://"))
            {
                UnityWebRequest request = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + FileName);
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    UnityEngine.Debug.LogError("Load file error.");
                    yield break;
                }
                bytes = request.downloadHandler.data;
            }
            else
            {
                bytes = File.ReadAllBytes(Application.streamingAssetsPath + "/" + FileName);
            }

            int startBytesPos = 0;
            for (int bytesPos = 1; bytesPos < bytes.Length - 2; bytesPos++)
            {
                if (bytes[bytesPos] == 0 && bytes[bytesPos + 1] == 0 && bytes[bytesPos + 2] == 1)
                {
                    byte[] addBytes = new byte[bytes.Length - startBytesPos - bytesPos - 1];
                    Array.Copy(bytes, startBytesPos, addBytes, 0, addBytes.Length);

                    ((FfmpegBytesInputs.IInputControl)ToCommand).AddInputBytes(addBytes, 0);

                    startBytesPos = bytesPos;
                    yield return null;
                    while (!ToCommand.InputBytesIsEmpty)
                    {
                        yield return null;
                    }
                }
            }
        }
    }
}