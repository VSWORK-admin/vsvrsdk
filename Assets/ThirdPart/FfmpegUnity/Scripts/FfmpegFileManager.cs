#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Linq;

namespace FfmpegUnity
{
    public static class FfmpegFileManager
    {
        [Serializable]
        public class FileItemJson
        {
            public string path = "";
            public string guid = "";
        }

        [Serializable]
        public class FileManagerJson
        {
            public FileItemJson[] items = null;
        }

        static void addFile(string originalPath)
        {
            FileManagerJson fileManagerJson;
            try
            {
                string jsonStr = File.ReadAllText("Assets/FfmpegUnity/Jsons/ffmpegFiles.json");
                fileManagerJson = JsonUtility.FromJson<FileManagerJson>(jsonStr);
            }
            catch (FileNotFoundException)
            {
                fileManagerJson = new FileManagerJson();
                fileManagerJson.items = new FileItemJson[0];
            }

            string guid = AssetDatabase.AssetPathToGUID(originalPath);

            var file = fileManagerJson.items.FirstOrDefault(x => x.path == originalPath);
            if (file == null)
            {
                var itemsList = fileManagerJson.items.ToList();
                var appendItem = new FileItemJson();
                appendItem.path = originalPath;
                appendItem.guid = guid;
                itemsList.Add(appendItem);
                fileManagerJson.items = itemsList.ToArray();
                string outputStr = JsonUtility.ToJson(fileManagerJson);
                File.WriteAllText("Assets/FfmpegUnity/Jsons/ffmpegFiles.json", outputStr);
                AssetDatabase.Refresh();
            }
            else
            {
                if (file.guid == guid)
                {
                    return;
                }
                file.guid = guid;
                string outputStr = JsonUtility.ToJson(fileManagerJson);
                File.WriteAllText("Assets/FfmpegUnity/Jsons/ffmpegFiles.json", outputStr);
                AssetDatabase.Refresh();
            }
        }

        public static string GetManagedFilePath(string path, bool logError = true)
        {
            string originalPath = path;
            if (path.StartsWith(Application.dataPath))
            {
                originalPath = "Assets" + path.Substring(Application.dataPath.Length);
            }

            if (File.Exists(originalPath))
            {
                addFile(originalPath);
                return path;
            }

            string jsonStr;
            try
            {
                jsonStr = File.ReadAllText("Assets/FfmpegUnity/Jsons/ffmpegFiles.json");
            }
            catch (Exception)
            {
                Debug.LogError("\"Assets/FfmpegUnity/Jsons/ffmpegFiles.json\" is not found: " + originalPath);
                return null;
            }
            FileManagerJson json = JsonUtility.FromJson<FileManagerJson>(jsonStr);

            var file = json.items.FirstOrDefault(x => x.path == originalPath);
            if (file == null)
            {
                if (logError)
                {
                    Debug.LogError("File not found: " + originalPath);
                }
                return null;
            }

            string ret = AssetDatabase.GUIDToAssetPath(file.guid);
            if (ret != null && ret.Length >= "Assets".Length && !ret.StartsWith(Application.dataPath))
            {
                ret = Application.dataPath + ret.Substring("Assets".Length);
            }
            return ret;
        }
    }
}

#endif