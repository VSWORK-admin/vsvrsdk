using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace FfmpegUnity
{
    public class InitTargetLinux : IActiveBuildTargetChanged
    {
        public int callbackOrder => 0;

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            MainProcess();
        }

        public static void MainProcess()
        {
#if UNITY_EDITOR_LINUX || (UNITY_STANDALONE_LINUX && UNITY_EDITOR)
            string[] commands = new[]
            {
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Linux/ffmpeg", false),
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Linux/ffprobe", false)
            };

            foreach (var command in commands)
            {
                if (string.IsNullOrEmpty(command))
                {
                    continue;
                }

                ProcessStartInfo psInfo = new ProcessStartInfo()
                {
                    FileName = "chmod",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = "a+x \"" + command + "\"",
                };
                using (Process process = Process.Start(psInfo))
                {
                    process.WaitForExit();
                }
            }
#endif
        }

        [InitializeOnLoad]
        public static class InitTargetLinuxOnLoad
        {
            static InitTargetLinuxOnLoad()
            {
                MainProcess();
            }
        }
    }
}
