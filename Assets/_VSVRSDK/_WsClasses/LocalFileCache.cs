using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using com.ootii.Messages;
using System.Net;

namespace STTXVR.Caches
{
    public class LocalFileCache
    {
        public static int TxtTimeOutSetting = 8;
        public static int ImgTimeOutSetting = 10;
        public static int FileTimeOutSetting = 120;
        public static bool DeleteOldAvatar = true;
        public static bool DeleteOldFile = true;
        public static string RootDirectory
        {
            get
            {
                if(VRPublishSettingController.I.bMultiInstance)
                {
                    return Application.persistentDataPath + "/" + VRPublishSettingController.nowMultiID.ToString() + "/FileCache/";
                }
                else
                {
                    return Application.persistentDataPath + "/FileCache/";
                }
            }
        }

        public static string SceneDirectory
        {
            get
            {
                if (VRPublishSettingController.I.bMultiInstance)
                {
                    return Application.persistentDataPath + "/" + VRPublishSettingController.nowMultiID.ToString() + "/SceneCache/";
                }
                else
                {
                    return Application.persistentDataPath + "/SceneCache/";
                }
            }
        }

        public static string AvatarDirectory
        {
            get
            {
                if (VRPublishSettingController.I.bMultiInstance)
                {
                    return Application.persistentDataPath + "/" + VRPublishSettingController.nowMultiID.ToString() + "/AvatarCache/";
                }
                else
                {
                    return Application.persistentDataPath + "/AvatarCache/";
                }
            }
        }

        public static string sysDirectory
        {
            get
            {
                if (VRPublishSettingController.I.bMultiInstance)
                {
                    return Application.persistentDataPath + "/" + VRPublishSettingController.nowMultiID.ToString() + "/sysCache/";
                }
                else
                {
                    return Application.persistentDataPath + "/sysCache/";
                }
            }
        }


        public static void CheckLocalPath(string sign, System.Action<bool> existed)
        {
        }
    }
}