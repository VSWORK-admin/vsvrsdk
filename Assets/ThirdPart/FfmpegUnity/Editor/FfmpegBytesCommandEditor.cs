using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FfmpegUnity
{
    [CustomEditor(typeof(FfmpegBytesCommand))]
    public class FfmpegBytesCommandEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Parameter Change");

            EditorGUI.BeginChangeCheck();

            FfmpegBytesCommand ffmpegCommand = (FfmpegBytesCommand)target;

            SerializedObject so = new SerializedObject(target);

            ffmpegCommand.ExecuteOnStart = EditorGUILayout.Toggle("Execute On Start", ffmpegCommand.ExecuteOnStart);

            SerializedProperty inputsProperty = so.FindProperty("InputOptions");
            EditorGUILayout.PropertyField(inputsProperty, true);

            EditorGUILayout.LabelField("Options");
            ffmpegCommand.Options = EditorGUILayout.TextArea(ffmpegCommand.Options);

            SerializedProperty outputsProperty = so.FindProperty("OutputOptions");
            EditorGUILayout.PropertyField(outputsProperty, true);

            ffmpegCommand.PrintStdErr = EditorGUILayout.Toggle("Print StdErr", ffmpegCommand.PrintStdErr);

            so.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(ffmpegCommand);
            }
        }
    }
}
