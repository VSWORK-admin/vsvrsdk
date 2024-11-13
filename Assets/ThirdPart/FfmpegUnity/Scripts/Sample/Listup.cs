using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FfmpegUnity.Sample
{
    public class Listup : MonoBehaviour
    {
        public Button EncodersButton;
        public Button DecodersButton;
        public Button FormatsButton;
        public Button CodecsButton;
        public Button ProtocolsButton;
        public Text LineTextPrefab;

#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !FFMPEG_UNITY_USE_BINARY_WIN || (UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX) && !FFMPEG_UNITY_USE_BINARY_LINUX
        [DllImport("libffmpegDll")]
        static extern void ffprobe_main(int argc, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]string[] argv, string file_path);
#endif

#if UNITY_EDITOR_WIN || (UNITY_EDITOR_OSX && FFMPEG_UNITY_USE_BINARY_MAC) || (UNITY_EDITOR_LINUX && FFMPEG_UNITY_USE_BINARY_LINUX)

#elif ((UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC) || UNITY_IOS
#if UNITY_IOS
        const string IMPORT_NAME = "__Internal";
#else
        const string IMPORT_NAME = "FfmpegUnityMacPlugin";
#endif
        [DllImport(IMPORT_NAME)]
        static extern IntPtr ffmpeg_ffprobeExecuteAsync(string command);
        [DllImport(IMPORT_NAME)]
        static extern bool ffmpeg_isRunnning(IntPtr session);
        [DllImport(IMPORT_NAME)]
        static extern int ffmpeg_getOutputLength(IntPtr session);
        [DllImport(IMPORT_NAME)]
        static extern void ffmpeg_getOutput(IntPtr session, int startIndex, IntPtr output, int outputLength);
#endif

#if ((UNITY_STANDALONE_OSX && FFMPEG_UNITY_USE_BINARY_MAC) || (UNITY_STANDALONE_LINUX && FFMPEG_UNITY_USE_BINARY_LINUX)) && ENABLE_IL2CPP
        [DllImport("__Internal")]
        static extern int unity_system(string command);
        [DllImport("__Internal")]
        static extern IntPtr unity_popen(string command, string type);
        [DllImport("__Internal")]
        static extern int unity_pclose(IntPtr stream);
        [DllImport("__Internal")]
        static extern IntPtr unity_fgets(IntPtr s, int n, IntPtr stream);
#endif

        void Start()
        {
            ClickEncodersButton();
        }

        IEnumerator setLineTexts(string option)
        {
            void buildText(StreamReader stdout)
            {
                string line;
                while (!stdout.EndOfStream)
                {
                    line = stdout.ReadLine();
                    if (line != null)
                    {
                        var lineObj = Instantiate(LineTextPrefab, transform);
                        lineObj.text = line;
                    }
                }
            }

            for (int loop = transform.childCount - 1; loop >= 0; loop--)
            {
                Destroy(transform.GetChild(loop).gameObject);
            }

#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !FFMPEG_UNITY_USE_BINARY_WIN || (UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX) && !FFMPEG_UNITY_USE_BINARY_LINUX
            string filePath = Application.temporaryCachePath + "/" + Guid.NewGuid().ToString() + ".txt";

            ffprobe_main(3, new string[] { "ffprobe", option, "-hide_banner" }, filePath);

            bool loopFlag;
            do
            {
                yield return null;

                loopFlag = false;
                try
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        for (int loop = transform.childCount - 1; loop >= 0; loop--)
                        {
                            Destroy(transform.GetChild(loop).gameObject);
                        }

                        buildText(reader);
                    }
                    File.Delete(filePath);
                }
                catch (IOException)
                {
                    loopFlag = true;
                }
            } while (loopFlag);
#elif UNITY_EDITOR_WIN || (UNITY_EDITOR_OSX && FFMPEG_UNITY_USE_BINARY_MAC) || (UNITY_EDITOR_LINUX && FFMPEG_UNITY_USE_BINARY_LINUX) || ((UNITY_STANDALONE_WIN || (UNITY_STANDALONE_OSX && FFMPEG_UNITY_USE_BINARY_MAC) || (UNITY_STANDALONE_LINUX && FFMPEG_UNITY_USE_BINARY_LINUX)) && !ENABLE_IL2CPP)
            string fileName = "ffmpeg";

#if UNITY_EDITOR_WIN
            fileName = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Windows/ffmpeg.exe");
#elif UNITY_EDITOR_OSX
#if !FFMPEG_UNITY_USE_OUTER_MAC
            fileName = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Mac/ffmpeg");
#else
            if (File.Exists("/usr/local/bin/ffmpeg"))
            {
                fileName = "/usr/local/bin/ffmpeg";
            }
#endif
#elif UNITY_EDITOR_LINUX
#if !FFMPEG_UNITY_USE_OUTER_LINUX
            fileName = FfmpegFileManager.GetManagedFilePath(Application.dataPath + "/FfmpegUnity/Bin/Linux/ffmpeg");
#endif
#elif UNITY_STANDALONE_WIN
            fileName = Application.streamingAssetsPath + "/_FfmpegUnity_temp/ffmpeg.exe";
#elif (UNITY_STANDALONE_OSX && FFMPEG_UNITY_USE_BINARY_MAC) || (UNITY_STANDALONE_LINUX && FFMPEG_UNITY_USE_BINARY_LINUX)
            fileName = Application.streamingAssetsPath + "/_FfmpegUnity_temp/ffmpeg";
#endif

            ProcessStartInfo psInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = option + " -hide_banner",
            };

            using (var process = Process.Start(psInfo))
            using (var stdout = process.StandardOutput)
            {
                buildText(stdout);
            }
#elif UNITY_STANDALONE_WIN && ENABLE_IL2CPP
            var execute = new FfmpegExecuteIL2CPPWin();
            var pipeOption = new FfmpegExecuteIL2CPPWin.PipeOption();
            pipeOption.BlockSize = -1;
            pipeOption.BufferSize = 1024;
            pipeOption.PipeName = "FfmpegUnity_" + Guid.NewGuid().ToString();
            pipeOption.StdMode = 1;
#if FFMPEG_UNITY_USE_OUTER_WIN
            execute.ExecuteSingle(false, "ffmpeg", option + " -hide_banner", pipeOption);
#else
            execute.ExecuteSingle(true, "ffmpeg", option + " -hide_banner", pipeOption);
#endif
            while (execute.GetStream(0) == null)
            {
                yield return null;
            }

            using (StreamReader reader = new StreamReader(execute.GetStream(0)))
            {
                buildText(reader);
            }

            execute.Dispose();
#elif UNITY_ANDROID
            string outputStr;

            using (AndroidJavaClass ffmpeg = new AndroidJavaClass("com.arthenica.ffmpegkit.FFmpegKit"))
            using (AndroidJavaObject ffmpegSession = ffmpeg.CallStatic<AndroidJavaObject>("executeAsync", option + " -hide_banner"))
            {
                using (AndroidJavaClass sessionState = new AndroidJavaClass("com.arthenica.ffmpegkit.SessionState"))
                using (AndroidJavaObject createdState = sessionState.GetStatic<AndroidJavaObject>("CREATED"))
                using (AndroidJavaObject runningState = sessionState.GetStatic<AndroidJavaObject>("RUNNING"))
                {
                    bool isRunning;
                    do
                    {
                        using (AndroidJavaObject state = ffmpegSession.Call<AndroidJavaObject>("getState"))
                        {
                            isRunning = state.Call<bool>("equals", createdState) || state.Call<bool>("equals", runningState);
                        }
                        if (isRunning)
                        {
                            yield return null;
                        }
                    } while (isRunning);
                }
                outputStr = ffmpegSession.Call<string>("getOutput");
            }

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(outputStr)))
            using (StreamReader reader = new StreamReader(stream))
            {
                buildText(reader);
            }
#elif ((UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC) || UNITY_IOS
            IntPtr ffprobeSession = ffmpeg_ffprobeExecuteAsync(option + " -hide_banner");

            while (ffmpeg_isRunnning(ffprobeSession))
            {
                yield return null;
            }

            int allocSize = ffmpeg_getOutputLength(ffprobeSession) + 1;
            IntPtr hglobal = Marshal.AllocHGlobal(allocSize);
            ffmpeg_getOutput(ffprobeSession, 0, hglobal, allocSize);
            string outputStr = Marshal.PtrToStringAuto(hglobal);
            Marshal.FreeHGlobal(hglobal);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(outputStr)))
            using (StreamReader reader = new StreamReader(stream))
            {
                buildText(reader);
            }
#elif ((UNITY_STANDALONE_OSX && FFMPEG_UNITY_USE_BINARY_MAC) || (UNITY_STANDALONE_LINUX && FFMPEG_UNITY_USE_BINARY_LINUX)) && ENABLE_IL2CPP
            string fileName = "ffmpeg";
#if !(UNITY_STANDALONE_OSX && FFMPEG_UNITY_USE_BINARY_MAC && !FFMPEG_UNITY_USE_OUTER_MAC || UNITY_STANDALONE_LINUX && FFMPEG_UNITY_USE_BINARY_LINUX && !FFMPEG_UNITY_USE_OUTER_LINUX)
            fileName = Application.streamingAssetsPath + "/_FfmpegUnity_temp/ffmpeg";
#endif
            IntPtr stdOutFp = unity_popen("\"" + fileName + "\" " + option + " -hide_banner", "r");
            string outputStr = "";
            IntPtr bufferHandler = Marshal.AllocHGlobal(1024);
            for (; ; )
            {
                IntPtr retPtr = unity_fgets(bufferHandler, 1024, stdOutFp);
                if (retPtr == IntPtr.Zero)
                {
                    break;
                }

                outputStr += Marshal.PtrToStringAuto(bufferHandler);
            }
            Marshal.FreeHGlobal(bufferHandler);
            unity_pclose(stdOutFp);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(outputStr)))
            using (StreamReader reader = new StreamReader(stream))
            {
                buildText(reader);
            }
#endif
            yield return null;
        }

        public void ClickEncodersButton()
        {
            EncodersButton.interactable = false;
            DecodersButton.interactable = true;
            FormatsButton.interactable = true;
            CodecsButton.interactable = true;
            ProtocolsButton.interactable = true;

            StartCoroutine(setLineTexts("-encoders"));
        }

        public void ClickDecodersButton()
        {
            EncodersButton.interactable = true;
            DecodersButton.interactable = false;
            FormatsButton.interactable = true;
            CodecsButton.interactable = true;
            ProtocolsButton.interactable = true;

            StartCoroutine(setLineTexts("-decoders"));
        }

        public void ClickFormatsButton()
        {
            EncodersButton.interactable = true;
            DecodersButton.interactable = true;
            FormatsButton.interactable = false;
            CodecsButton.interactable = true;
            ProtocolsButton.interactable = true;

            StartCoroutine(setLineTexts("-formats"));
        }

        public void ClickCodecsButton()
        {
            EncodersButton.interactable = true;
            DecodersButton.interactable = true;
            FormatsButton.interactable = true;
            CodecsButton.interactable = false;
            ProtocolsButton.interactable = true;

            StartCoroutine(setLineTexts("-codecs"));
        }

        public void ClickProtocolsButton()
        {
            EncodersButton.interactable = true;
            DecodersButton.interactable = true;
            FormatsButton.interactable = true;
            CodecsButton.interactable = true;
            ProtocolsButton.interactable = false;

            StartCoroutine(setLineTexts("-protocols"));
        }
    }
}
