using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FfmpegUnity
{
    [CustomEditor(typeof(FfmpegCommand))]
    public class FfmpegCommandEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Parameter Change");

            EditorGUI.BeginChangeCheck();

            FfmpegCommand ffmpegCommand = (FfmpegCommand)target;

            ffmpegCommand.ExecuteOnStart = EditorGUILayout.Toggle("Execute On Start", ffmpegCommand.ExecuteOnStart);
            EditorGUILayout.LabelField("Options");
            ffmpegCommand.Options = EditorGUILayout.TextArea(ffmpegCommand.Options);
            ffmpegCommand.PrintStdErr = EditorGUILayout.Toggle("Print StdErr", ffmpegCommand.PrintStdErr);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(ffmpegCommand);
            }
        }
    }
}
