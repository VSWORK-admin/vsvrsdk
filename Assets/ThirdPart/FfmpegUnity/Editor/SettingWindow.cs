using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FfmpegUnity
{
    public class SettingWindow : EditorWindow
    {
        enum BinaryToUse
        {
            BuiltInBinary = 1,
            InstalledBinary = 2,
        }

        enum BinaryOrLibraryToUse
        {
            Library = 0,
            BuiltInBinary = 1,
            InstalledBinary = 2,
        }

        enum IPCForMoblie
        {
            LibraryMemory,
            Pipe,
        }

        enum BinaryToUseLocal
        {
            Default = 0,
            BuiltInBinary = 1,
            InstalledBinary = 2,
        }

        BinaryOrLibraryToUse windowsUse_;
        BinaryOrLibraryToUse macUse_;
        BinaryOrLibraryToUse linuxUse_;
#if false
        IPCForMoblie androidIPC_;
        IPCForMoblie iosIPC_;
#endif

        [MenuItem("Tools/Ffmpeg for Unity/Open Setting Window")]
        static void openDialogWindow()
        {
            bool isOpenWindow = false;

            foreach (var window in Resources.FindObjectsOfTypeAll<EditorWindow>())
            {
                if (window.GetType() == typeof(SettingWindow))
                {
                    window.Focus();
                    isOpenWindow = true;
                }
            }

            if (!isOpenWindow)
            {
                GetWindow<SettingWindow>(true, "Ffmpeg for Unity", true);
            }
        }

        static void setScriptingDefineSymbol(BuildTargetGroup target, string symbol)
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            if (!symbols.Contains(symbol))
            {
                if (symbols == null || symbols == "")
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(target, symbol);
                }
                else
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(target, symbols + ";" + symbol);
                }
            }
        }

        static string deleteSymbol(string symbols, string symbol)
        {
            if (symbols.Contains(";" + symbol))
            {
                symbols = symbols.Replace(";" + symbol, "");
            }
            else if (symbols.Contains(symbol + ";"))
            {
                symbols = symbols.Replace(symbol + ";", "");
            }
            else
            {
                symbols = symbols.Replace(symbol, "");
            }

            return symbols;
        }

        static void deleteScriptingDefineSymbol(BuildTargetGroup target, string symbol)
        {
            string symbols;

            symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            symbols = deleteSymbol(symbols, symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, symbols);
        }

        void Awake()
        {
            var currentPosition = position;
            currentPosition.height = 210f;
            position = currentPosition;

            if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Contains("FFMPEG_UNITY_USE_BINARY_WIN"))
            {
                if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Contains("FFMPEG_UNITY_USE_OUTER_WIN"))
                {
                    windowsUse_ = BinaryOrLibraryToUse.InstalledBinary;
                }
                else
                {
                    windowsUse_ = BinaryOrLibraryToUse.BuiltInBinary;
                }
            }
            else
            {
                windowsUse_ = BinaryOrLibraryToUse.Library;
            }

            if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Contains("FFMPEG_UNITY_USE_BINARY_MAC"))
            {
                if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Contains("FFMPEG_UNITY_USE_OUTER_MAC"))
                {
                    macUse_ = BinaryOrLibraryToUse.InstalledBinary;
                }
                else
                {
                    macUse_ = BinaryOrLibraryToUse.BuiltInBinary;
                }
            }
            else
            {
                macUse_ = BinaryOrLibraryToUse.Library;
            }

            if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Contains("FFMPEG_UNITY_USE_BINARY_LINUX"))
            {
                if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Contains("FFMPEG_UNITY_USE_OUTER_LINUX"))
                {
                    linuxUse_ = BinaryOrLibraryToUse.InstalledBinary;
                }
                else
                {
                    linuxUse_ = BinaryOrLibraryToUse.BuiltInBinary;
                }
            }
            else
            {
                linuxUse_ = BinaryOrLibraryToUse.Library;
            }

#if false
            if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android).Contains("FFMPEG_UNITY_USE_PIPE"))
            {
                androidIPC_ = IPCForMoblie.Pipe;
            }
            else
            {
                androidIPC_ = IPCForMoblie.LibraryMemory;
            }

            if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS).Contains("FFMPEG_UNITY_USE_PIPE"))
            {
                iosIPC_ = IPCForMoblie.Pipe;
            }
            else
            {
                iosIPC_ = IPCForMoblie.LibraryMemory;
            }
#endif
        }

        void OnGUI()
        {
            GUILayout.Label("Binary / Library to Use (for Editor and Standalone)");

            var newWindowsUse = (BinaryOrLibraryToUse)EditorGUILayout.EnumPopup("Windows", windowsUse_);
            if (newWindowsUse != windowsUse_)
            {
                switch (newWindowsUse)
                {
                    case BinaryOrLibraryToUse.Library:
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_BINARY_WIN");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_OUTER_WIN");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_BINARY_WIN");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_OUTER_WIN");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_BINARY_WIN");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_OUTER_WIN");
                        break;
                    case BinaryOrLibraryToUse.BuiltInBinary:
                        setScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_BINARY_WIN");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_OUTER_WIN");
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_BINARY_WIN");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_OUTER_WIN");
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_BINARY_WIN");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_OUTER_WIN");
                        break;
                    case BinaryOrLibraryToUse.InstalledBinary:
                        setScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_BINARY_WIN");
                        setScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_OUTER_WIN");
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_BINARY_WIN");
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_OUTER_WIN");
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_BINARY_WIN");
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_OUTER_WIN");
                        break;
                }

                windowsUse_ = newWindowsUse;
            }

            var newMacUse = (BinaryOrLibraryToUse)EditorGUILayout.EnumPopup("Mac", macUse_);
            if (newMacUse != macUse_)
            {
                switch (newMacUse)
                {
                    case BinaryOrLibraryToUse.Library:
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_BINARY_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_BINARY_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_BINARY_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_OUTER_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_OUTER_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_OUTER_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_INNER_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_INNER_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_INNER_MAC");
                        break;
                    case BinaryOrLibraryToUse.BuiltInBinary:
                        setScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_BINARY_MAC");
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_BINARY_MAC");
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_BINARY_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_OUTER_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_OUTER_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_OUTER_MAC");
                        setScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_INNER_MAC");
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_INNER_MAC");
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_INNER_MAC");
                        break;
                    case BinaryOrLibraryToUse.InstalledBinary:
                        setScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_BINARY_MAC");
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_BINARY_MAC");
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_BINARY_MAC");
                        setScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_OUTER_MAC");
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_OUTER_MAC");
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_OUTER_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_INNER_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_INNER_MAC");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_INNER_MAC");
                        break;
                }

                macUse_ = newMacUse;
            }

            var newLinuxUse = (BinaryOrLibraryToUse)EditorGUILayout.EnumPopup("Linux", linuxUse_);
            if (newLinuxUse != linuxUse_)
            {
                switch (newLinuxUse)
                {
                    case BinaryOrLibraryToUse.Library:
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_BINARY_LINUX");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_BINARY_LINUX");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_BINARY_LINUX");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_OUTER_LINUX");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_OUTER_LINUX");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_OUTER_LINUX");
                        break;
                    case BinaryOrLibraryToUse.BuiltInBinary:
                        setScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_BINARY_LINUX");
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_BINARY_LINUX");
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_BINARY_LINUX");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_OUTER_LINUX");
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_OUTER_LINUX");
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_OUTER_LINUX");
                        break;
                    case BinaryOrLibraryToUse.InstalledBinary:
                        setScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_BINARY_LINUX");
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_BINARY_LINUX");
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_BINARY_LINUX");
                        setScriptingDefineSymbol(BuildTargetGroup.Standalone, "FFMPEG_UNITY_USE_OUTER_LINUX");
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_OUTER_LINUX");
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_OUTER_LINUX");
                        break;
                }

                linuxUse_ = newLinuxUse;
            }

#if false
            GUILayout.Label("IPC Mode (for Moblie)");

            var newAndroidIPC = (IPCForMoblie)EditorGUILayout.EnumPopup("Android", androidIPC_);
            if (newAndroidIPC != androidIPC_)
            {
                switch (newAndroidIPC)
                {
                    case IPCForMoblie.LibraryMemory:
                        deleteScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_PIPE");
                        break;
                    case IPCForMoblie.Pipe:
                        setScriptingDefineSymbol(BuildTargetGroup.Android, "FFMPEG_UNITY_USE_PIPE");
                        break;
                }

                androidIPC_ = newAndroidIPC;
            }

            var newIosIPC = (IPCForMoblie)EditorGUILayout.EnumPopup("iOS", iosIPC_);
            if (newIosIPC != iosIPC_)
            {
                switch (newIosIPC)
                {
                    case IPCForMoblie.LibraryMemory:
                        deleteScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_PIPE");
                        break;
                    case IPCForMoblie.Pipe:
                        setScriptingDefineSymbol(BuildTargetGroup.iOS, "FFMPEG_UNITY_USE_PIPE");
                        break;
                }

                iosIPC_ = newIosIPC;
            }
#endif

            GUILayout.Space(20);

            if (GUILayout.Button("Close"))
            {
                Close();
            }
        }
    }
}
