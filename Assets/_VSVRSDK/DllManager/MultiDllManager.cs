﻿using ILRuntime.Runtime.Enviorment;
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

public class MultiDllManager : MonoBehaviour
{
    public enum MultiDllLoadType
    {
        None = 0,
        OnlineMode,
        TestMode,
        InSceneMode,
    }
    /// <summary>
    /// 当前所有Dll实列（Key ：DllName）
    /// </summary>
    public Dictionary<string, MultiDllInstanceData> AllDllInstance = new Dictionary<string, MultiDllInstanceData>();

    private static MultiDllManager _Instatnce = null;
    public static MultiDllManager Instance
    {
        get { return _Instatnce; }
    }
    private static int portCount = 0;
    private void Awake()
    {
        _Instatnce = this;
        MessageDispatcher.AddListener(VrDispMessageType.VRUserLeaveChanel.ToString(), OnUserLeaveChanel, true);
    }

    private void OnUserLeaveChanel(IMessage rMessage)
    {
        UnloadAllAssembly();
#if ILHotFix && DEBUG && UNITY_STANDALONE_WIN
        if(VSVR_Debug.DebugManager.Instance != null)
        {
            VSVR_Debug.RtcMsgSingle rtcMsgSingle = new VSVR_Debug.RtcMsgSingle();
            rtcMsgSingle.rtcMsgId = (int)VSVR_Debug.RtcMsgType.UserLeaveChanel;
            VSVR_Debug.DebugManager.Instance.SendDebugMsg(JsonMapper.ToJson(rtcMsgSingle));
        }
#endif
    }

    void Start()
    {
    }

    private static int GetDebugPort()
    {
        portCount++;
        return 56001 + portCount;
    }
    public void PrepareBindSceneAssemblyStart(string dllName, TextAsset DllAsset, TextAsset PdbAsset, ExtralData[] ExtralDatas)
    {
        MultiDllInstanceData multiDllInstanceData;
        if (AllDllInstance.TryGetValue(dllName, out multiDllInstanceData))
        {
            multiDllInstanceData.DllAsset = DllAsset;
            multiDllInstanceData.PdbAsset = PdbAsset;
            multiDllInstanceData.ExtralDatas = ExtralDatas;
            multiDllInstanceData.bDefaultCallMain = true;

            multiDllInstanceData.Start();
        }
        else
        {
            Debug.LogError("PrepareBindSceneAssemblyStart error ! dllname : " + dllName);
        }
    }
    public void LoadOnlineDllAssembly(MultiDllInstanceData multiDllInstance, MultiDllLoadType loadType,bool callMainByDefault = true)
    {
        if (multiDllInstance == null) return;

        if (!AllDllInstance.ContainsKey(multiDllInstance.DllName))
        {
            multiDllInstance.DebugPort = GetDebugPort();
            AllDllInstance.Add(multiDllInstance.DllName, multiDllInstance);
        }
        else
        {
            switch (loadType)
            {
                case MultiDllLoadType.OnlineMode:
                    {
                        AllDllInstance[multiDllInstance.DllName].OnlineDll = multiDllInstance.OnlineDll;
                        AllDllInstance[multiDllInstance.DllName].OnlinePdb = multiDllInstance.OnlinePdb;
                    }
                    break;
                case MultiDllLoadType.TestMode:
                    {
                        AllDllInstance[multiDllInstance.DllName].TestDll = multiDllInstance.TestDll;
                        AllDllInstance[multiDllInstance.DllName].TestPdb = multiDllInstance.TestPdb;
                    }
                    break;
                default:
                    {
                        AllDllInstance[multiDllInstance.DllName] = multiDllInstance;
                    }
                    break;
            }
        }


        multiDllInstance.bDefaultCallMain = callMainByDefault;

        switch (loadType)
        {
            case MultiDllLoadType.OnlineMode:
                {
                    if(multiDllInstance.bTestMode)
                    {
                        multiDllInstance.OnTestModeLoaded();
                    }
                    else
                    {
                        multiDllInstance.LoadOnlineAssembly();
                    }
                }
                break;
            case MultiDllLoadType.TestMode:
                multiDllInstance.LoadTestAssembly();
                break;
        }
    }
    public void UnloadAllAssembly()
    {
        foreach(var item in AllDllInstance)
        {
            if(item.Value != null)
            {
                item.Value.UnLoadAssembly();
            }
        }
        AllDllInstance.Clear();
    }
    public void UnloadAssemblyByName(string dllName)
    {
        MultiDllInstanceData multiDllInstanceData;
        if (AllDllInstance.TryGetValue(dllName, out multiDllInstanceData))
        {
            multiDllInstanceData.UnLoadAssembly();

            AllDllInstance.Remove(dllName);
        }
        else
        {
            Debug.LogError("UnloadAssemblyByName error ! dllname : " + dllName);
        }
    }
    private void OnDestroy()
    {
        _Instatnce = null;
        MessageDispatcher.RemoveListener(VrDispMessageType.VRUserLeaveChanel.ToString(), OnUserLeaveChanel, true);
        StopAllCoroutines();
    }
}