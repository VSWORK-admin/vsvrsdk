using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace FfmpegUnity
{
    public class InitTargetWin : IActiveBuildTargetChanged
    {
        public int callbackOrder => 0;

        public readonly static string WarningImportFile =
            "<color=yellow><b>Please import windows assets from: </b></color>https://github.com/NON906/ffmpeg-windows-build-helpers/releases/download/1.0/FfmpegUnity_WindowsBinaries.unitypackage";

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
#if (UNITY_EDITOR_WIN || (UNITY_STANDALONE_WIN && UNITY_EDITOR)) && FFMPEG_UNITY_USE_BINARY_WIN && !FFMPEG_UNITY_USE_OUTER_WIN
            if (string.IsNullOrEmpty(FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Windows/ffmpeg.exe", false))
                || string.IsNullOrEmpty(FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Windows/ffprobe.exe", false)))
            {
                Debug.LogWarning(WarningImportFile);
            }
#endif
        }
    }
}
