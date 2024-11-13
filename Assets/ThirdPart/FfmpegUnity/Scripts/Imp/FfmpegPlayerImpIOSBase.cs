#if UNITY_IOS || ((UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX) && !FFMPEG_UNITY_USE_BINARY_MAC)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

namespace FfmpegUnity
{
    public abstract class FfmpegPlayerImpIOSBase : FfmpegPlayerImpBase
    {
        TextReader reader_;

#if UNITY_IOS && !UNITY_EDITOR_OSX
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
        [DllImport(IMPORT_NAME)]
        static extern void ffmpeg_mkpipe(IntPtr output, int outputLength);
        [DllImport(IMPORT_NAME)]
        static extern void ffmpeg_closePipe(string pipeName);

        FfmpegPlayerCommand playerCommand_;

        public FfmpegPlayerImpIOSBase(FfmpegPlayerCommand playerCommand) : base(playerCommand)
        {
            playerCommand_ = playerCommand;
        }

        public override IEnumerator OpenFfprobeReaderCoroutine(string inputPathAll)
        {
            IntPtr ffprobeSession = ffmpeg_ffprobeExecuteAsync("-i \"" + inputPathAll + "\" -show_streams");

            while (ffmpeg_isRunnning(ffprobeSession))
            {
                yield return null;
            }

            int allocSize = ffmpeg_getOutputLength(ffprobeSession) + 1;
            IntPtr hglobal = Marshal.AllocHGlobal(allocSize);
            ffmpeg_getOutput(ffprobeSession, 0, hglobal, allocSize);
            string outputStr = Marshal.PtrToStringAuto(hglobal);
            Marshal.FreeHGlobal(hglobal);

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
                //reader_.ReadToEnd();
                reader_.Dispose();
                reader_ = null;
            }
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

#if UNITY_IOS && !UNITY_EDITOR_OSX
        [DllImport(IMPORT_NAME)]
        static extern int metalSetupNativeTextureRender(IntPtr textureId);

        [DllImport(IMPORT_NAME)]
        static extern void metalCopyBuffer(int eventId, byte[] buf);

        [DllImport(IMPORT_NAME)]
        static extern void metalFinishNativeTextureRender(int eventId);

        [DllImport(IMPORT_NAME)]
        static extern IntPtr metalGetRenderEventFunc();

        Dictionary<Texture, int> metalIds_ = new Dictionary<Texture, int>();
        CommandBuffer commandBuffer;

        public override void WriteTexture(Texture texture, byte[] data, int width, int height)
        {
            if (!(playerCommand_ is FfmpegGetTexturePerFrameCommand) && SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Metal)
            {
                if (!metalIds_.ContainsKey(texture))
                {
                    // Create texture first time
                    base.WriteTexture(texture, data, width, height);
                    // Setup the texture
                    metalIds_[texture] = metalSetupNativeTextureRender(texture.GetNativeTexturePtr());
                }
                else
                {
                    int id = metalIds_[texture];
                    metalCopyBuffer(id, data);
                    //GL.IssuePluginEvent(metalGetRenderEventFunc(), id);
                    var callback = metalGetRenderEventFunc();
                    if (commandBuffer == null)
                    {
                        commandBuffer = new CommandBuffer();
                        commandBuffer.name = "MetalTextureUpdate";
                        commandBuffer.IssuePluginEvent(callback, id);
                    }
                    // Execute Command
                    Graphics.ExecuteCommandBufferAsync(commandBuffer, ComputeQueueType.Default);
                }
            }
            else
            {
                base.WriteTexture(texture, data, width, height);
            }
        }

        public override void Clean()
        {
            foreach (var id in metalIds_.Values)
            {
                metalFinishNativeTextureRender(id);
            }
            metalIds_.Clear();
        }
#endif
    }
}

#endif