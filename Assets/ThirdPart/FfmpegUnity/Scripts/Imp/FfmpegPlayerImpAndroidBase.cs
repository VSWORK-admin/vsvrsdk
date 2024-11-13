using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace FfmpegUnity
{
    public abstract class FfmpegPlayerImpAndroidBase : FfmpegPlayerImpBase
    {
        protected string DataDir
        {
            get;
            private set;
        }

        TextReader reader_;

        string options_;

        FfmpegPlayerCommand playerCommand_;

        public FfmpegPlayerImpAndroidBase(FfmpegPlayerCommand playerCommand) : base(playerCommand)
        {
            playerCommand_ = playerCommand;

            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext"))
            using (AndroidJavaObject info = context.Call<AndroidJavaObject>("getApplicationInfo"))
            {
                DataDir = info.Get<string>("dataDir");
            }
        }

        public override IEnumerator OpenFfprobeReaderCoroutine(string inputPathAll)
        {
            if (inputPathAll.Contains(Application.streamingAssetsPath))
            {
                yield return PlayerCommand.StreamingAssetsCopyPath(inputPathAll);
                inputPathAll = PlayerCommand.PathInStreamingAssetsCopy;
            }

            string outputStr;

            using (AndroidJavaClass ffprobe = new AndroidJavaClass("com.arthenica.ffmpegkit.FFprobeKit"))
            using (AndroidJavaObject ffprobeSession = ffprobe.CallStatic<AndroidJavaObject>("execute", "-i \"" + inputPathAll + "\" -show_streams"))
            {
                outputStr = ffprobeSession.Call<string>("getOutput");
            }

            reader_ = new StringReader(outputStr);
        }

        public override TextReader OpenFfprobeReader(string inputPathAll)
        {
            return reader_;
        }

        public override void CloseFfprobeReader()
        {
            if (reader_ != null)
            {
                reader_.ReadToEnd();
                reader_.Dispose();
                reader_ = null;
            }
        }

        public override IEnumerator BuildBeginOptionsCoroutine(string path)
        {
            options_ = " -i \"";

            if (path.Contains(Application.streamingAssetsPath))
            {
                if (string.IsNullOrEmpty(PlayerCommand.PathInStreamingAssetsCopy))
                {
                    yield return PlayerCommand.StreamingAssetsCopyPath(path);
                }
                options_ += PlayerCommand.PathInStreamingAssetsCopy;
            }
            else
            {
                options_ += path;
            }

            options_ += "\" ";
        }

        public override string BuildBeginOptions(string path)
        {
            return options_;
        }

        public override bool IsGetTimeFromImp
        {
            get
            {
                return true;
            }
        }

        public override float Time
        {
            get
            {
                return playerCommand_.CommandImp.Time + StartTime;
            }
        }

        public override float StartTime
        {
            get;
            set;
        } = 0f;

        [DllImport("glNative")]
        static extern int openGLSetupNativeTextureRender(IntPtr textureId, int width, int height);

        [DllImport("glNative")]
        static extern void openGLCopyBuffer(int id, byte[] buf);

        [DllImport("glNative")]
        static extern void openGLFinishNativeTextureRender(int id);

        [DllImport("glNative")]
        static extern IntPtr openGLGetRenderEventFunc();

        Dictionary<Texture, int> openGLIds_ = new Dictionary<Texture, int>();

        public override void WriteTexture(Texture texture, byte[] data, int width, int height)
        {
            if (!(playerCommand_ is FfmpegGetTexturePerFrameCommand) && SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
            {
                if (!openGLIds_.ContainsKey(texture))
                {
                    openGLIds_[texture] = openGLSetupNativeTextureRender(texture.GetNativeTexturePtr(), width, height);
                }

                int id = openGLIds_[texture];

                openGLCopyBuffer(id, data);

                GL.IssuePluginEvent(openGLGetRenderEventFunc(), id);
            }
            else
            {
                base.WriteTexture(texture, data, width, height);
            }
        }

        public override void Clean()
        {
            foreach (var id in openGLIds_.Values)
            {
                openGLFinishNativeTextureRender(id);
            }
            openGLIds_.Clear();
        }
    }
}
