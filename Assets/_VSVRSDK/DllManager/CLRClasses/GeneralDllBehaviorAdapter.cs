using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using System;

public class GeneralDllBehaviorAdapter : MonoBehaviour
{
    public string ScriptClassName = string.Empty;

    public string OtherData;
    public ExtralData[] ExtralDatas;
    public ExtralDataObj[] ExtralDataObjs;
    private void Awake()
    {
        MessageDispatcher.AddListener("GeneralDllBehaviorAwake", OnDllAwake,true);
    }

    public void OnDestroy()
    {
        MessageDispatcher.RemoveListener("GeneralDllBehaviorAwake", OnDllAwake, true);
    }

    void OnDllAwake(IMessage msg)
    {
        gameObject.AddComponent<GeneralDllBehavior>();
    }
}
