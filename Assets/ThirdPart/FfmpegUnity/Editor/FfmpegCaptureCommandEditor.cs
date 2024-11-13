using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FfmpegUnity
{
    [CustomEditor(typeof(FfmpegCaptureCommand))]
    public class FfmpegCaptureCommandEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Parameter Change");

            EditorGUI.BeginChangeCheck();

            FfmpegCaptureCommand ffmpegCaptureCommand = (FfmpegCaptureCommand)target;

            ffmpegCaptureCommand.ExecuteOnStart = EditorGUILayout.Toggle("Execute On Start", ffmpegCaptureCommand.ExecuteOnStart);

            EditorGUILayout.LabelField("Capture Sources");

            for (int loop = 0; loop < ffmpegCaptureCommand.CaptureSources.Length; loop++)
            {
                ffmpegCaptureCommand.CaptureSources[loop].Type = (FfmpegCaptureCommand.CaptureSource.SourceType)
                    EditorGUILayout.EnumPopup("#" + loop, ffmpegCaptureCommand.CaptureSources[loop].Type);


                switch (ffmpegCaptureCommand.CaptureSources[loop].Type)
                {
                    case FfmpegCaptureCommand.CaptureSource.SourceType.Video_GameView:
                        {
                            Vector2Int videoSize = EditorGUILayout.Vector2IntField("Video Size",
                                new Vector2Int(ffmpegCaptureCommand.CaptureSources[loop].Width, ffmpegCaptureCommand.CaptureSources[loop].Height));
                            ffmpegCaptureCommand.CaptureSources[loop].Width = videoSize.x;
                            ffmpegCaptureCommand.CaptureSources[loop].Height = videoSize.y;

                            ffmpegCaptureCommand.CaptureSources[loop].FrameRate =
                                EditorGUILayout.FloatField("Frame Rate", ffmpegCaptureCommand.CaptureSources[loop].FrameRate);
                        }
                        break;
                    case FfmpegCaptureCommand.CaptureSource.SourceType.Video_Camera:
                        {
                            ffmpegCaptureCommand.CaptureSources[loop].SourceCamera =
                                (Camera)EditorGUILayout.ObjectField("Camera", ffmpegCaptureCommand.CaptureSources[loop].SourceCamera, typeof(Camera), true);

                            Vector2Int videoSize = EditorGUILayout.Vector2IntField("Video Size",
                                new Vector2Int(ffmpegCaptureCommand.CaptureSources[loop].Width, ffmpegCaptureCommand.CaptureSources[loop].Height));
                            ffmpegCaptureCommand.CaptureSources[loop].Width = videoSize.x;
                            ffmpegCaptureCommand.CaptureSources[loop].Height = videoSize.y;

                            ffmpegCaptureCommand.CaptureSources[loop].FrameRate =
                                EditorGUILayout.FloatField("Frame Rate", ffmpegCaptureCommand.CaptureSources[loop].FrameRate);
                        }
                        break;
                    case FfmpegCaptureCommand.CaptureSource.SourceType.Video_RenderTexture:
                        {
                            ffmpegCaptureCommand.CaptureSources[loop].SourceRenderTexture =
                                (RenderTexture)EditorGUILayout.ObjectField(
                                    "Render Texture", ffmpegCaptureCommand.CaptureSources[loop].SourceRenderTexture, typeof(RenderTexture), true);

                            ffmpegCaptureCommand.CaptureSources[loop].FrameRate =
                                EditorGUILayout.FloatField("Frame Rate", ffmpegCaptureCommand.CaptureSources[loop].FrameRate);
                        }
                        break;
                    case FfmpegCaptureCommand.CaptureSource.SourceType.Audio_AudioSource:
                        ffmpegCaptureCommand.CaptureSources[loop].SourceAudio =
                            (AudioSource)EditorGUILayout.ObjectField(
                                "Audio Source", ffmpegCaptureCommand.CaptureSources[loop].SourceAudio, typeof(AudioSource), true);
                        ffmpegCaptureCommand.CaptureSources[loop].Mute = EditorGUILayout.Toggle("Mute on Game", ffmpegCaptureCommand.CaptureSources[loop].Mute);
                        break;
                }

                if (GUILayout.Button("Delete"))
                {
                    var captureList = ffmpegCaptureCommand.CaptureSources.ToList();
                    captureList.RemoveAt(loop);
                    ffmpegCaptureCommand.CaptureSources = captureList.ToArray();
                }
            }

            if (GUILayout.Button("Add"))
            {
                var captureList = ffmpegCaptureCommand.CaptureSources.ToList();
                captureList.Add(new FfmpegCaptureCommand.CaptureSource());
                ffmpegCaptureCommand.CaptureSources = captureList.ToArray();
            }

            EditorGUILayout.LabelField("Options");
            ffmpegCaptureCommand.CaptureOptions = EditorGUILayout.TextArea(ffmpegCaptureCommand.CaptureOptions);

            ffmpegCaptureCommand.PrintStdErr = EditorGUILayout.Toggle("Print StdErr", ffmpegCaptureCommand.PrintStdErr);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(ffmpegCaptureCommand);
            }
        }
    }
}
