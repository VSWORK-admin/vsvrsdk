using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FfmpegUnity
{
    [CustomEditor(typeof(FfmpegWriteFromTexturesCommand))]
    public class FfmpegWriteFromTexturesCommandEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Parameter Change");

            EditorGUI.BeginChangeCheck();

            FfmpegWriteFromTexturesCommand ffmpegCommand = (FfmpegWriteFromTexturesCommand)target;

            ffmpegCommand.ExecuteOnStart = EditorGUILayout.Toggle("Execute On Start", ffmpegCommand.ExecuteOnStart);
            EditorGUILayout.LabelField("Input Options");
            ffmpegCommand.InputOptions = EditorGUILayout.TextArea(ffmpegCommand.InputOptions);
            EditorGUILayout.LabelField("Output Options");
            ffmpegCommand.OutputOptions = EditorGUILayout.TextArea(ffmpegCommand.OutputOptions);
            ffmpegCommand.PrintStdErr = EditorGUILayout.Toggle("Print StdErr", ffmpegCommand.PrintStdErr);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(ffmpegCommand);
            }
        }
    }
}
