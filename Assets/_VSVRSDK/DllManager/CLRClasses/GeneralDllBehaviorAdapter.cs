using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using System;
#if ILHotFix
[ILInject.ILInjectorAttribute(ILInject.InjectFlag.NoInject)]
#endif
public class GeneralDllBehaviorAdapter : MonoBehaviour
{
    public string ScriptClassName = string.Empty;

    public string OtherData;
    public ExtralData[] ExtralDatas;
    public ExtralDataObj[] ExtralDataObjs;
    public ExtralDataInfo[] ExtralDataInfos;
    public bool bListenAwake = true;
    private bool dllnameSetted = false;
    private string m_dllname;
    [HideInInspector]
    public string DllName
    {
        get
        {
            return m_dllname;
        }
        set
        {
            m_dllname = value;
            dllnameSetted = true;
        }
    }

    private void Awake()
    {
        if (bListenAwake)
        {
            MessageDispatcher.AddListener("GeneralDllBehaviorAwake", OnDllAwake, true);
        }
        else if (dllnameSetted)
        {
            OnLoadDllScript();
        }
    }
    private void OnDisable()
    {

    }
    private void OnDestroy()
    {
        if (bListenAwake)
            MessageDispatcher.RemoveListener("GeneralDllBehaviorAwake", OnDllAwake, true);
    }
    void OnDllAwake(IMessage msg)
    {
        string dllname = msg.Data != null ? (string)msg.Data : "";
        if (dllname == DllName)
        {
            Debug.Log($"GeneralDllBehaviorAdapter OnDllAwake dllname:{dllname} this.dllname:{DllName} scriptname:{ScriptClassName}");
            OnLoadDllScript();
        }
    }
    public void Instantiate(string dllname)
    {
        DllName = dllname;
        OnLoadDllScript();
    }
    public void Instantiate(string dllname, string scriptclassname)
    {
        DllName = dllname;
        ScriptClassName = scriptclassname;

        OnLoadDllScript();
    }

    void OnLoadDllScript()
    {
        if (string.IsNullOrEmpty(ScriptClassName)) return;

        var scripts = gameObject.GetComponents<GeneralDllBehavior>();

        if (scripts != null)
        {
            for (int i = 0; i < scripts.Length; i++)
            {
                if (scripts[i].ScriptClassName.Equals(ScriptClassName))
                {
                    return;
                }
            }
        }

        var DllBehavior = gameObject.GetComponent<GeneralDllBehavior>();
        if (DllBehavior == null)
        {
            GeneralDllBehavior dllbehavior = gameObject.AddComponent<GeneralDllBehavior>();
            dllbehavior.DllName = DllName;
            dllbehavior.ScriptClassName = ScriptClassName;
            dllbehavior.OtherData = OtherData;
            dllbehavior.ExtralDatas = ExtralDatas;
            dllbehavior.ExtralDataObjs = ExtralDataObjs;
            dllbehavior.ExtralDataInfos = ExtralDataInfos;
            //Debug.Log($"GeneralDllBehaviorAdapter LoadDllScript dllname:{DllName} scriptname:{ScriptClassName}");
            dllbehavior.Instantiate();
            if (dllbehavior.DllClass == null)
            {
                DestroyImmediate(dllbehavior);
            }
        }
    }
}
