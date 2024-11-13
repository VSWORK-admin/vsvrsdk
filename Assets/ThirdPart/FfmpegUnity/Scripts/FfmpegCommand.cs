using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace FfmpegUnity
{
    public class FfmpegCommand : MonoBehaviour
    {
        public bool ExecuteOnStart = true;
        public string Options = "";
        public bool PrintStdErr = false;

        public bool IsRunning
        {
            get
            {
                if (CommandImp == null)
                {
                    return false;
                }

                return CommandImp.IsRunning;
            }
        }

        public bool IsAlreadyStart
        {
            get;
            private set;
        } = false;

        public bool GetProgressOnScript
        {
            get;
            set;
        } = false;
        public TimeSpan DurationTime
        {
            get;
            private set;
        } = TimeSpan.Zero;
        TimeSpan currentTime_ = TimeSpan.Zero;
        public TimeSpan CurrentTime
        {
            get
            {
                if (CommandImp != null && CommandImp.IsGetStatusFromImp)
                {
                    return TimeSpan.FromSeconds(CommandImp.Time);
                }
                else
                {
                    return currentTime_;
                }
            }
            private set
            {
                currentTime_ = value;
            }
        }
        public float Progress
        {
            get
            {
                if (DurationTime.TotalSeconds <= 0.0)
                {
                    return 0f;
                }

                return (float)(CurrentTime.TotalSeconds / DurationTime.TotalSeconds);
            }
        }
        float speed_ = 0f;
        public float Speed
        {
            get
            {
                if (CommandImp != null && CommandImp.IsGetStatusFromImp)
                {
                    return (float)CommandImp.Speed;
                }
                else
                {
                    return speed_;
                }
            }
            private set
            {
                speed_ = value;
            }
        }
        public TimeSpan RemainingTime
        {
            get
            {
                if (Speed <= 0f)
                {
                    return TimeSpan.Zero;
                }

                return TimeSpan.FromSeconds((DurationTime - CurrentTime).TotalSeconds / Speed);
            }
        }

        public int ReturnCode
        {
            get
            {
                if (CommandImp == null)
                {
                    return 0;
                }

                return CommandImp.ReturnCode;
            }
        }

        public bool IsFinished
        {
            get;
            private set;
        } = false;

        public bool IsGetStdErr
        {
            get;
            protected set;
        } = false;
        protected string ParsedOptionInStreamingAssetsCopy = "";
        public string PathInStreamingAssetsCopy
        {
            get
            {
                return streamingAssetsCopyPathClass_.PathInStreamingAssetsCopy;
            }
        }

        bool isAlreadyBuild_ = false;

        List<string> deleteAssets_
        {
            get
            {
                return streamingAssetsCopyPathClass_.DeleteAssets;
            }
        }

        List<string> stdErrListForGetLine_ = new List<string>();

        //bool restopCoroutineStarted_ = false;

        List<Coroutine> coroutines_ = new List<Coroutine>();

        bool resetStart_ = false;

        bool startFfmpegExecuteFlag_ = false;
        bool stopFfmpegExecuteFlag_ = false;

        protected bool IsFinishedBuild
        {
            get;
            set;
        } = false;

        public string RunOptions
        {
            get;
            protected set;
        } = "";

        public FfmpegCommandImpBase CommandImp
        {
            get;
            private set;
        } = null;

        StreamingAssetsCopyPathClass streamingAssetsCopyPathClass_ = new StreamingAssetsCopyPathClass();

        public class StreamingAssetsCopyPathClass
        {
            public string PathInStreamingAssetsCopy
            {
                get;
                set;
            } = "";

            public List<string> DeleteAssets = new List<string>();

            public IEnumerator StreamingAssetsCopyPath(string path)
            {
                path = path.Replace(Application.streamingAssetsPath, "{STREAMING_ASSETS_PATH}").Replace("\"", "").Replace("'", "");

                string searchPath = Regex.Replace(path, @"\%[0\#\ \+\-]*[diouxXfeEgGcspn\%]", "*");
                searchPath = Regex.Escape(searchPath).Replace(@"\*", ".*");
                List<string> paths = new List<string>();
                List<string> hashs = new List<string>();
                using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(Application.streamingAssetsPath + "/_FfmpegUnity_files.txt"))
                {
                    yield return unityWebRequest.SendWebRequest();
                    string[] allLines = unityWebRequest.downloadHandler.text.Replace("\r\n", "\n").Split('\n');
                    foreach (string line in allLines)
                    {
                        string[] splitLine = line.Split('\t');
                        if (splitLine.Length == 2)
                        {
                            string hash = splitLine[0];
                            string singlePath = splitLine[1];
                            string addPath = "{STREAMING_ASSETS_PATH}" + singlePath.Replace("\\", "/");
                            if (Regex.IsMatch(addPath, searchPath))
                            {
                                paths.Add(addPath);
                                hashs.Add(hash);
                            }
                        }
                    }
                }

                for (int loop = 0; loop < paths.Count; loop++)
                {
                    var loopPath = paths[loop];

                    string streamingAssetPath = loopPath.Replace("{STREAMING_ASSETS_PATH}", Application.streamingAssetsPath);

                    string targetItem = loopPath.Replace("{STREAMING_ASSETS_PATH}", Application.temporaryCachePath + "/FfmpegUnity_temp/");

                    if (File.Exists(targetItem))
                    {
                        var fileData = File.ReadAllBytes(targetItem);
                        SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();
                        var sha256 = string.Join("", provider.ComputeHash(fileData).Select(x => $"{x:x2}"));
                        if (sha256 == hashs[loop])
                        {
                            continue;
                        }
                        File.Delete(targetItem);
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(targetItem));

                    byte[] data;
                    using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(streamingAssetPath))
                    {
                        yield return unityWebRequest.SendWebRequest();
                        data = unityWebRequest.downloadHandler.data;
                    }
                    File.WriteAllBytes(targetItem, data);

                    DeleteAssets.Add(targetItem);
                }

                PathInStreamingAssetsCopy = path.Replace("{STREAMING_ASSETS_PATH}", Application.temporaryCachePath + "/FfmpegUnity_temp/");
            }

            public void ExecuteDeleteAssets()
            {
                foreach (var file in DeleteAssets)
                {
                    File.Delete(file);
                }
                DeleteAssets.Clear();
            }
        }

        public IEnumerator StreamingAssetsCopyPath(string path)
        {
            yield return streamingAssetsCopyPathClass_.StreamingAssetsCopyPath(path);
        }

        protected virtual void Build()
        {
            RunOptions = Options;
            IsGetStdErr = PrintStdErr || GetProgressOnScript;
            IsFinishedBuild = true;
        }

        protected virtual void Clean()
        {

        }

        protected virtual void CleanBuf()
        {

        }

        // Get outputs from stderr.
        public string GetStdErrLine()
        {
            if (stdErrListForGetLine_.Count <= 0)
            {
                return null;
            }
            string ret = stdErrListForGetLine_[0];
            stdErrListForGetLine_.RemoveAt(0);
            return ret;
        }

        string stdErrLine()
        {
            if (CommandImp == null)
            {
                return null;
            }
            return CommandImp.StdErrLine(stdErrListForGetLine_);
        }

        IEnumerator Start()
        {
            yield return null;

            if (ExecuteOnStart)
            {
                StartFfmpeg();
            }
        }

        void OnDestroy()
        {
            StopFfmpegMain();

            foreach (var file in deleteAssets_)
            {
                File.Delete(file);
            }
            deleteAssets_.Clear();
        }

        // Start ffmpeg commands. (Continuous)
        // If you want stop commands, call StopFfmpeg().
        public void StartFfmpeg()
        {
            //stopFfmpegExecuteFlag_ = true;
            startFfmpegExecuteFlag_ = true;
        }

        static void connectSplitStr(List<string> optionsSplit, char targetChar)
        {
            int countChar(string baseStr, char c)
            {
                string s = baseStr.Replace("\\" + c.ToString(), "");
                return s.Length - s.Replace(c.ToString(), "").Length;
            }

            for (int loop = 0; loop < optionsSplit.Count - 1; loop++)
            {
                if (countChar(optionsSplit[loop], targetChar) % 2 == 1)
                {
                    int loop2;
                    for (loop2 = loop + 1;
                        loop2 < optionsSplit.Count - 1 && countChar(optionsSplit[loop2], targetChar) % 2 != 1;
                        loop2++)
                    {
                        if (optionsSplit[loop2] != null)
                        {
                            optionsSplit[loop] += " " + optionsSplit[loop2];
                        }
                    }
                    optionsSplit[loop] += " " + optionsSplit[loop2];

                    optionsSplit.RemoveRange(loop + 1, loop2 - loop);
                    optionsSplit[loop] = optionsSplit[loop].Replace("\\" + targetChar.ToString(), "\n")
                        .Replace(targetChar.ToString(), "")
                        .Replace("\n", "\\" + targetChar.ToString());
                }
            }
        }

        public static string[] CommandSplitStatic(string command)
        {
            var optionsSplit = command.Split().ToList();
            connectSplitStr(optionsSplit, '\"');
            connectSplitStr(optionsSplit, '\'');

            return optionsSplit.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }

        public string[] CommandSplit(string command)
        {
            return CommandSplitStatic(command);
        }

        public IEnumerator StreamingAssetsCopyOptions(string options)
        {
            options = options.Replace(Application.streamingAssetsPath, "{STREAMING_ASSETS_PATH}");

            var optionsSplit = CommandSplit(options);

            ParsedOptionInStreamingAssetsCopy = "";
            foreach (var optionItem in optionsSplit)
            {
                if (optionItem != null)
                {
                    string targetItem = optionItem;
                    if (optionItem.Contains("{STREAMING_ASSETS_PATH}"))
                    {
                        yield return StreamingAssetsCopyPath(optionItem);
                        targetItem = PathInStreamingAssetsCopy;
                    }

                    ParsedOptionInStreamingAssetsCopy += " " + targetItem;
                }
            }
        }

        IEnumerator startFfmpegCoroutine()
        {
            if (isAlreadyBuild_)
            {
                yield break;
            }

            isAlreadyBuild_ = true;

            IsFinished = false;

            /*
            while (CommandImp != null && CommandImp.IsRunning)
            {
                yield return null;
            }
            */
            if (CommandImp != null)
            {
                CommandImp.Dispose();
                CommandImp = null;
            }

#if UNITY_EDITOR_WIN && FFMPEG_UNITY_USE_BINARY_WIN
            CommandImp = new FfmpegCommandImpWinMonoBin(this);
#elif UNITY_EDITOR_WIN && !FFMPEG_UNITY_USE_BINARY_WIN || UNITY_EDITOR_LINUX && !FFMPEG_UNITY_USE_BINARY_LINUX
            CommandImp = new FfmpegCommandImpWinLib(this);
#elif (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC
            CommandImp = new FfmpegCommandImpIOS(this);
#elif UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            CommandImp = new FfmpegCommandImpUnixMonoBin(this);
#elif UNITY_STANDALONE_WIN && !FFMPEG_UNITY_USE_BINARY_WIN || UNITY_STANDALONE_LINUX && !FFMPEG_UNITY_USE_BINARY_LINUX
            CommandImp = new FfmpegCommandImpWinLib(this);
#elif UNITY_STANDALONE_WIN && !ENABLE_IL2CPP
            CommandImp = new FfmpegCommandImpWinMonoBin(this);
#elif UNITY_STANDALONE_WIN && ENABLE_IL2CPP
            CommandImp = new FfmpegCommandImpWinIL2CPPBin(this);
#elif (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX) && !ENABLE_IL2CPP
            CommandImp = new FfmpegCommandImpUnixMonoBin(this);
#elif UNITY_STANDALONE_OSX && ENABLE_IL2CPP
            CommandImp = new FfmpegCommandImpMacIL2CPPBin(this);
#elif UNITY_STANDALONE_LINUX && ENABLE_IL2CPP
            CommandImp = new FfmpegCommandImpLinuxIL2CPPBin(this);
#elif UNITY_ANDROID
            CommandImp = new FfmpegCommandImpAndroid(this);
#elif UNITY_IOS
            CommandImp = new FfmpegCommandImpIOS(this);
#endif

            CommandImp.IsAlreadyBuild = true;
            CommandImp.LoopExit = false;

            IsFinishedBuild = false;
            Build();
            while (!IsFinishedBuild)
            {
                yield return null;
            }

            if (string.IsNullOrWhiteSpace(RunOptions))
            {
                yield break;
            }

            yield return CommandImp.StartFfmpegCoroutine(RunOptions);

            IsAlreadyStart = true;
        }

        // Stop ffmpeg commands.
        public void StopFfmpeg()
        {
            stopFfmpegExecuteFlag_ = true;
        }

        protected void StopFfmpegMain()
        {
            if (CommandImp != null)
            {
                CommandImp.LoopExit = true;
            }

            Clean();

            if (CommandImp != null)
            {
                CommandImp.StopFfmpeg();
            }

            CleanBuf();

            streamingAssetsCopyPathClass_.PathInStreamingAssetsCopy = "";

            foreach (var coroutine in coroutines_)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            }
            coroutines_.Clear();

            isAlreadyBuild_ = false;
            if (CommandImp != null)
            {
                CommandImp.IsAlreadyBuild = false;
            }
            IsAlreadyStart = false;

            //CommandImp = null;

            IsFinished = true;
        }

        // Execute ffmpeg. (Once)
        // If you want to use StopFfmpeg(), call StartFfmpeg() instead.
        public void ExecuteFfmpeg()
        {
            StartFfmpeg();

            coroutines_.Add(StartCoroutine(executeFfmpegCoroutine()));
        }

        IEnumerator executeFfmpegCoroutine()
        {
            while (!IsRunning)
            {
                yield return null;
            }

            while (IsRunning)
            {
                yield return null;
            }

            StopFfmpeg();
        }

        protected void ExecuteStdErrLine()
        {
            string stdErrLoopResult;
            do
            {
                stdErrLoopResult = stdErrLine();
                if (PrintStdErr && stdErrLoopResult != null)
                {
                    UnityEngine.Debug.Log(stdErrLoopResult);
                }
                if (GetProgressOnScript && stdErrLoopResult != null)
                {
                    if (stdErrLoopResult.Contains("Duration: "))
                    {
                        string durationStr = stdErrLoopResult.Split(new[] { "Duration: " }, StringSplitOptions.None)[1].Split(',')[0];
                        TimeSpan result;
                        if (TimeSpan.TryParse(durationStr, out result))
                        {
                            DurationTime = result;
                        }
                    }
                    else if (stdErrLoopResult.Contains("time=") && stdErrLoopResult.Contains("speed="))
                    {
                        string str = stdErrLoopResult.Split(new[] { "time=" }, StringSplitOptions.None)[1].Split()[0];
                        TimeSpan result;
                        if (TimeSpan.TryParse(str, out result))
                        {
                            CurrentTime = result;
                        }

                        str = stdErrLoopResult.Split(new[] { "speed=" }, StringSplitOptions.None)[1].Split('x')[0];
                        float speed;
                        if (float.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out speed))
                        {
                            Speed = speed;
                        }
                    }
                }
            } while (stdErrLoopResult != null);
        }
        
        protected virtual void Update()
        {
            if (stopFfmpegExecuteFlag_ && IsAlreadyStart)
            {
                StopFfmpegMain();
                stopFfmpegExecuteFlag_ = false;
            }
            else if (startFfmpegExecuteFlag_)
            {
                coroutines_.Add(StartCoroutine(startFfmpegCoroutine()));
                startFfmpegExecuteFlag_ = false;
                stopFfmpegExecuteFlag_ = false;
            }

            ExecuteStdErrLine();

            if (CommandImp != null)
            {
                CommandImp.Update();
            }

            if (resetStart_)
            {
                resetStart_ = false;

                StopFfmpeg();
                StartFfmpeg();
            }
        }

        public void ResetFfmpeg()
        {
            resetStart_ = true;
        }

#if UNITY_EDITOR
        static void onReload()
        {
            var commands = FindObjectsOfType<FfmpegCommand>();
            foreach (var command in commands)
            {
                if (command != null && command.CommandImp != null)
                {
                    command.CommandImp.IsForceStop = true;
                    command.StopFfmpegMain();
                }
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        static void OnScriptsReloaded()
        {
            onReload();
        }

        [InitializeOnLoad]
        public class Startup
        {
            static Startup()
            {
                AssemblyReloadEvents.beforeAssemblyReload += onReload;
            }
        }
#endif
    }
}
