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
    [HideInInspector]
    public string DllName = string.Empty;

    public TextAsset DllAsset;
    public TextAsset PdbAsset;

    public ExtralData[] ExtralDatas;

    private MultiDllInstanceData instanceData;
    private void Awake()
    {
        DllName = gameObject.name;
        instanceData = new MultiDllInstanceData(DllName);
        MultiDllManager.Instance.RegisterDllInstance(instanceData);
        DllName = instanceData.DllName;
        SetScriptDllName();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.06f);
        if (DllAsset != null && PdbAsset != null)
        {
            MultiDllManager.Instance.LoadOnlineDllAssembly(instanceData, MultiDllManager.MultiDllLoadType.None);
            MultiDllManager.Instance.PrepareBindSceneAssemblyStart(DllName, DllAsset, PdbAsset, ExtralDatas);
        }
    }
    private void SetScriptDllName()
    {
        GeneralDllBehaviorAdapter[] dlladapters = gameObject.GetComponentsInChildren<GeneralDllBehaviorAdapter>(true);
        if (dlladapters != null)
        {
            foreach (var adapter in dlladapters)
            {
                adapter.DllName = DllName;
            }
        }
        GeneralDllBehavior[] dllbehaviors = gameObject.GetComponentsInChildren<GeneralDllBehavior>(true);
        if (dllbehaviors != null)
        {
            foreach (var behavior in dllbehaviors)
            {
                behavior.DllName = DllName;
            }
        }
    }
    private void OnDestroy()
    {
        if (MultiDllManager.Instance != null)
            MultiDllManager.Instance.UnloadAssemblyByName(DllName);

        StopAllCoroutines();
    }

}
