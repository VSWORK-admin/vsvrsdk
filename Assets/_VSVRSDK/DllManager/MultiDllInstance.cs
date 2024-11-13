using ILRuntime.Runtime.Enviorment;
using ILRuntimeAdapter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Video;
using com.ootii.Messages;

using UnityEngine.Timeline;
using LitJson;
using System.Linq;

public class MultiDllInstance : MonoBehaviour
{
    public string DllName = string.Empty;

    public TextAsset DllAsset;
    public TextAsset PdbAsset;

    public ExtralData[] ExtralDatas;

    private void Awake()
    {
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.06f);
        if (DllAsset != null && PdbAsset != null)
        {
            MultiDllManager.Instance.LoadOnlineDllAssembly(new MultiDllInstanceData(DllName), MultiDllManager.MultiDllLoadType.None);
            MultiDllManager.Instance.PrepareBindSceneAssemblyStart(DllName, DllAsset, PdbAsset, ExtralDatas);
        }
    }

    private void OnDestroy()
    {
        if(MultiDllManager.Instance != null)
            MultiDllManager.Instance.UnloadAssemblyByName(DllName);

        StopAllCoroutines();
    }

}
