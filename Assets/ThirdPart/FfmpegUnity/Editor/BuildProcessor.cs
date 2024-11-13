using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace FfmpegUnity
{
    public class BuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        bool isFirst_ = true;

        string[] pluginsPaths_ = null;

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!isFirst_)
            {
                return;
            }
            isFirst_ = false;

#if (UNITY_STANDALONE_WIN && FFMPEG_UNITY_USE_BINARY_WIN && !FFMPEG_UNITY_USE_OUTER_WIN) || (UNITY_STANDALONE_OSX && FFMPEG_UNITY_USE_BINARY_MAC && !FFMPEG_UNITY_USE_OUTER_MAC) || (UNITY_STANDALONE_LINUX && FFMPEG_UNITY_USE_BINARY_LINUX && !FFMPEG_UNITY_USE_OUTER_LINUX)

            if (!AssetDatabase.IsValidFolder("Assets/StreamingAssets"))
            {
                AssetDatabase.CreateFolder("Assets", "StreamingAssets");
            }
            if (!AssetDatabase.IsValidFolder("Assets/StreamingAssets/_FfmpegUnity_temp"))
            {
                AssetDatabase.CreateFolder("Assets/StreamingAssets", "_FfmpegUnity_temp");
            }

#if UNITY_STANDALONE_WIN && FFMPEG_UNITY_USE_BINARY_WIN && !FFMPEG_UNITY_USE_OUTER_WIN
            string ffmpegPath = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Windows/ffmpeg.exe");
            string ffprobePath = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Windows/ffprobe.exe");
            if (string.IsNullOrEmpty(ffmpegPath) || string.IsNullOrEmpty(ffprobePath))
            {
                throw new BuildFailedException(InitTargetWin.WarningImportFile);
            }

            File.Copy(ffmpegPath,
                "Assets/StreamingAssets/_FfmpegUnity_temp/ffmpeg.exe");
            File.Copy(ffprobePath,
                "Assets/StreamingAssets/_FfmpegUnity_temp/ffprobe.exe");
#elif UNITY_STANDALONE_OSX && FFMPEG_UNITY_USE_BINARY_MAC && !FFMPEG_UNITY_USE_OUTER_MAC
            File.Copy(FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Mac/ffmpeg"),
                "Assets/StreamingAssets/_FfmpegUnity_temp/ffmpeg");
            File.Copy(FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Mac/ffprobe"),
                "Assets/StreamingAssets/_FfmpegUnity_temp/ffprobe");
#elif UNITY_STANDALONE_LINUX && FFMPEG_UNITY_USE_BINARY_LINUX && !FFMPEG_UNITY_USE_OUTER_LINUX
            File.Copy(FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Linux/ffmpeg"),
                "Assets/StreamingAssets/_FfmpegUnity_temp/ffmpeg");
            File.Copy(FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Linux/ffprobe"),
                "Assets/StreamingAssets/_FfmpegUnity_temp/ffprobe");
#endif

#if UNITY_STANDALONE_WIN && FFMPEG_UNITY_USE_BINARY_WIN
            if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.IL2CPP)
            {
                string pipePath = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Windows/NamedPipeConnecter.exe");
                if (string.IsNullOrEmpty(pipePath))
                {
                    throw new BuildFailedException(InitTargetWin.WarningImportFile);
                }

                File.Copy(pipePath,
                    "Assets/StreamingAssets/_FfmpegUnity_temp/NamedPipeConnecter.exe");
            }
#endif

            AssetDatabase.Refresh();
#endif

#if UNITY_STANDALONE_WIN
            pluginsPaths_ = new string[]
            {
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Plugins/Windows/avcodec-59.dll"),
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Plugins/Windows/avdevice-59.dll"),
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Plugins/Windows/avfilter-8.dll"),
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Plugins/Windows/avformat-59.dll"),
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Plugins/Windows/avutil-57.dll"),
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Plugins/Windows/swresample-4.dll"),
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Plugins/Windows/swscale-6.dll"),
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Plugins/Windows/libffmpegDll.dll"),
                FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Plugins/Windows/libwinpthread-1.dll"),
            };

#if FFMPEG_UNITY_USE_BINARY_WIN
            foreach (string path in pluginsPaths_)
            {
                var importer = AssetImporter.GetAtPath(path.Replace(Application.dataPath, "Assets/")) as PluginImporter;
                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
            }
#else
            foreach (string path in pluginsPaths_)
            {
                var importer = AssetImporter.GetAtPath(path.Replace(Application.dataPath, "Assets/")) as PluginImporter;
                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, true);
            }
#endif
#endif

#if UNITY_ANDROID
            var files = Directory.GetFiles("Assets/StreamingAssets", "*", SearchOption.AllDirectories).Where(x => !x.EndsWith(".meta")).ToArray();
            for (int loop = 0; loop < files.Length; loop++)
            {
                var fileData = File.ReadAllBytes(files[loop]);
                SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();
                var sha256 = string.Join("", provider.ComputeHash(fileData).Select(x => $"{x:x2}"));
                files[loop] = sha256 + "\t" + files[loop].Replace("\\", "/").Replace("Assets/StreamingAssets", "");
            }
            File.WriteAllLines("Assets/StreamingAssets/_FfmpegUnity_files.txt", files);

            AssetDatabase.Refresh();
#endif

#if !((UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && ENABLE_IL2CPP) && !UNITY_2020
            AssetDatabase.MoveAsset("Assets/FfmpegUnity/Plugins/system.c", "Assets/FfmpegUnity/_temp_system.txt");
#endif

            EditorApplication.update += postProcess;
        }

        void postProcess()
        {
#if (UNITY_STANDALONE_WIN && FFMPEG_UNITY_USE_BINARY_WIN && !FFMPEG_UNITY_USE_OUTER_WIN) || (UNITY_STANDALONE_OSX && FFMPEG_UNITY_USE_BINARY_MAC && !FFMPEG_UNITY_USE_OUTER_MAC) || (UNITY_STANDALONE_LINUX && FFMPEG_UNITY_USE_BINARY_LINUX && !FFMPEG_UNITY_USE_OUTER_LINUX)
            AssetDatabase.DeleteAsset("Assets/StreamingAssets/_FfmpegUnity_temp");
#endif

#if UNITY_STANDALONE_WIN && FFMPEG_UNITY_USE_BINARY_WIN
            foreach (string path in pluginsPaths_)
            {
                var importer = AssetImporter.GetAtPath(path.Replace(Application.dataPath, "Assets/")) as PluginImporter;
                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, true);
            }
#endif

#if UNITY_ANDROID
            AssetDatabase.DeleteAsset("Assets/StreamingAssets/_FfmpegUnity_files.txt");
#endif

#if !((UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && ENABLE_IL2CPP) && !UNITY_2020
            AssetDatabase.MoveAsset("Assets/FfmpegUnity/_temp_system.txt", "Assets/FfmpegUnity/Plugins/system.c");
#endif

            EditorApplication.update -= postProcess;
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            postProcess();
        }

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
#if UNITY_STANDALONE_OSX
            string bundlePath = path + "/Contents/PlugIns/FfmpegUnityMacPlugin.bundle";
            Directory.Delete(bundlePath, true);

            ProcessStartInfo psInfo = new ProcessStartInfo()
            {
                FileName = "unzip",
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = "\"" + FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/PluginsOSX/FfmpegUnityMacPlugin.bundle.zip") + "\" -d \"" + Path.GetDirectoryName(bundlePath) + "\"",
            };
            using (Process process = Process.Start(psInfo))
            {
                process.WaitForExit();
            }
#endif
        }
    }
}
