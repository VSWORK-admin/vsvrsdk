using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.IO;

public class VRPublishSettingController : MonoBehaviour
{
    public bool bMultiInstance = false;

    public static int nowMultiID = 0;

    private static VRPublishSettingController instance;
    public static VRPublishSettingController I
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        //LocalFileCache.Open();
        instance = this;
        mStaticThings.I.movingmarklist.Add(false);
        mStaticThings.I.movingmarklist.Add(false);

        if (GraphicsSettings.renderPipelineAsset)
        {
            mStaticThings.I.isurp = true;
        }
        else
        {
            mStaticThings.I.isurp = false;
        }
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
        {
            mStaticThings.I.ismobile = false;
        }
        else
        {
            mStaticThings.I.ismobile = true;

        }
    }

#if UNITY_STANDALONE_WIN
    private void Start()
    {

    }

    private void OnApplicationQuit()
    {
        
    }
#endif
}




