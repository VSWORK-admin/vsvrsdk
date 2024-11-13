using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace FfmpegUnity
{
    public static class PostProcessBuildiOS
    {
        const string FRAMEWORK_ORIGIN_PATH = "Assets/FfmpegUnity/Plugins/iOS";
        const string FRAMEWORK_TARGET_PATH = "Frameworks/FfmpegUnity/Plugins/iOS";

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
#if UNITY_IOS
            string projectPath = PBXProject.GetPBXProjectPath(path);
            PBXProject pbxProject = new PBXProject();

            pbxProject.ReadFromString(File.ReadAllText(projectPath));

            string target = pbxProject.GetUnityFrameworkTargetGuid();

            pbxProject.AddFileToBuild(target, pbxProject.AddFile("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk));
            pbxProject.AddFileToBuild(target, pbxProject.AddFile("usr/lib/libbz2.tbd", "Frameworks/libbz2.tbd", PBXSourceTree.Sdk));
            pbxProject.AddFileToBuild(target, pbxProject.AddFile("System/Library/Frameworks/VideoToolbox.framework", "Frameworks/VideoToolbox.framework", PBXSourceTree.Sdk));

            string appTarget = pbxProject.GetUnityMainTargetGuid();

            foreach (var dir in Directory.GetDirectories(FRAMEWORK_ORIGIN_PATH))
            {
                if (dir.EndsWith(".framework"))
                {
                    string destPath = Path.Combine(FRAMEWORK_TARGET_PATH, Path.GetFileName(dir));
                    string fileGuid = pbxProject.FindFileGuidByProjectPath(destPath);
                    //UnityEditor.iOS.Xcode.Extensions.PBXProjectExtensions.AddFileToEmbedFrameworks(pbxProject, target, fileGuid);
                    pbxProject.AddFileToBuild(appTarget, fileGuid);
                    UnityEditor.iOS.Xcode.Extensions.PBXProjectExtensions.AddFileToEmbedFrameworks(pbxProject, appTarget, fileGuid);
                }
            }

            pbxProject.AddBuildProperty(appTarget, "FRAMEWORK_SEARCH_PATHS", $"$(PROJECT_DIR)/{FRAMEWORK_TARGET_PATH}");

            File.WriteAllText(projectPath, pbxProject.WriteToString());
#endif
        }
    }
}
