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
    public bool bListenAwake = false;
    private void Awake()
    {
        if (bListenAwake)
            MessageDispatcher.AddListener("GeneralDllBehaviorAwake", OnDllAwake,true);
    }
    private void OnDisable()
    {
        if (bListenAwake)
            MessageDispatcher.RemoveListener("GeneralDllBehaviorAwake", OnDllAwake, true);
    }
    private void OnDestroy()
    {
        OnDisable();
    }
    void OnDllAwake(IMessage msg)
    {
        var scripts = gameObject.GetComponents<GeneralDllBehavior>();

        if (scripts == null) return;

        for (int i = 0; i < scripts.Length; i++)
        {
            if (scripts[i].ScriptClassName.Equals(ScriptClassName))
            {
                return;
            }
        }

        var DllBehavior = gameObject.GetComponent<GeneralDllBehavior>();
        if (DllBehavior == null)
        {
            gameObject.AddComponent<GeneralDllBehavior>();
        }
    }
}
