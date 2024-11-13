using com.ootii.Messages;
using com.ootii.Utilities.Debug;
using Kurisu.AkiBT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AkiInfo("Decorator : Templete")]
[AkiLabel("GeneratedDllDecorator : Templete")]
//[AkiGroup("GeneratedDllDecorator")]
public class GeneratedDllDecorator : Kurisu.AkiBT.Decorator
{
    public Kurisu.AkiBT.BehaviorTree BaseTree 
    { 
        get 
        {
            Kurisu.AkiBT.BehaviorTree temp = Tree as Kurisu.AkiBT.BehaviorTree;
            if (temp != null)
            {
                return temp;
            }
            return null;
        }
    }
    public GameObject gameObject
    {
        get { return GameObject; }
    }

    [SerializeField]
    public SharedString DllName;
    [SerializeField]
    public SharedString ScriptClassName;
    [SerializeField]
    public List<SharedString> strDatas;
    [SerializeField]
    public List<SharedInt> intDatas;
    [SerializeField]
    public List<SharedFloat> floatDatas;
    [SerializeField]
    public List<SharedBool> boolDatas;
    [SerializeField]
    public List<SharedVector3> vector3Datas;
    [SerializeField]
    public List<SharedObject> objectDatas;
    [SerializeField]
    public SharedTObject<HFExtralData> ExtralDatas;

    public DllDecoratorBase DllClass
    {
        get
        {
            InitVariable(DllName);
            InitVariable(ScriptClassName);
            InitVariable(ExtralDatas);
            foreach (var data in strDatas) { InitVariable(data); }
            foreach (var data in intDatas) { InitVariable(data); }
            foreach (var data in floatDatas) { InitVariable(data); }
            foreach (var data in boolDatas) { InitVariable(data); }
            foreach (var data in vector3Datas) { InitVariable(data); }
            foreach (var data in objectDatas) { InitVariable(data); }
            try
            {
                if (DllManager.appdomain == null && !string.IsNullOrEmpty(DllName.Value))
                {
                    if (MultiDllManager.Instance != null && MultiDllManager.Instance.AllDllInstance.ContainsKey(DllName.Value))
                    {
                        if (GenClass == null && !string.IsNullOrEmpty(ScriptClassName.Value))
                        {
                            GenClass = MultiDllManager.Instance.AllDllInstance[DllName.Value].appdomain.Instantiate<DllDecoratorBase>(ScriptClassName.Value);
                        }

                        if (GenClass != null)
                        {
                            GenClass.BaseAction = this;

                            //if (!bInit)
                            {
                                GenClass.Init();

                                //bInit = true;
                            }
                        }

                        return GenClass;
                    }
                    else
                    {
                        Debug.LogError("Please init appdomain first !");
                        return null;
                    }
                }
                else if (!string.IsNullOrEmpty(DllName.Value) && MultiDllManager.Instance != null && MultiDllManager.Instance.AllDllInstance.ContainsKey(DllName.Value))
                {
                    if (GenClass == null && !string.IsNullOrEmpty(ScriptClassName.Value))
                    {
                        GenClass = MultiDllManager.Instance.AllDllInstance[DllName.Value].appdomain.Instantiate<DllDecoratorBase>(ScriptClassName.Value);
                    }

                    if (GenClass != null)
                    {
                        GenClass.BaseAction = this;

                        //if (!bInit)
                        {
                            GenClass.Init();

                            //bInit = true;
                        }
                    }

                    return GenClass;
                }
                else
                {
                    if (GenClass == null && !string.IsNullOrEmpty(ScriptClassName.Value))
                    {
                        GenClass = DllManager.appdomain.Instantiate<DllDecoratorBase>(ScriptClassName.Value);
                    }

                    if (GenClass != null)
                    {
                        GenClass.BaseAction = this;

                        //if (!bInit)
                        {
                            GenClass.Init();

                            //bInit = true;
                        }
                    }

                    return GenClass;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error Init " + "DllName : " + DllName + "ScriptClassName : " + ScriptClassName + " gameObject.name : " + gameObject.name + "\r\n" + e.ToString());
                return null;
            }
        }
    }

    private DllDecoratorBase GenClass = null;
    ~GeneratedDllDecorator()
    {
        if (GenClass != null)
        {
            GenClass = null;
            MessageDispatcher.SendMessageData(VrDispMessageType.SDKScriptDestroyed.ToString(), ScriptClassName.Value);
        }
    }
    public override void Abort()
    {
        base.Abort();
    }

    protected override void OnAwake()
    {
        InitVariable(DllName);
        InitVariable(ScriptClassName);
        InitVariable(ExtralDatas);
        foreach (var data in strDatas) { InitVariable(data); }
        foreach (var data in intDatas) { InitVariable(data); }
        foreach (var data in floatDatas) { InitVariable(data); }
        foreach (var data in boolDatas) { InitVariable(data); }
        foreach (var data in vector3Datas) { InitVariable(data); }
        foreach (var data in objectDatas) { InitVariable(data); }
        try
        {
            if (DllManager.appdomain == null && !string.IsNullOrEmpty(DllName.Value))
            {
                if (MultiDllManager.Instance != null && MultiDllManager.Instance.AllDllInstance.ContainsKey(DllName.Value))
                {
                    if (GenClass == null && !string.IsNullOrEmpty(ScriptClassName.Value))
                    {
                        GenClass = MultiDllManager.Instance.AllDllInstance[DllName.Value].appdomain.Instantiate<DllDecoratorBase>(ScriptClassName.Value);
                    }

                    if (GenClass != null)
                    {
                        GenClass.BaseAction = this;

                        //if (!bInit)
                        {
                            GenClass.Init();

                            //bInit = true;
                        }

                        GenClass.Awake();
                    }
                }
                else
                {
                    Debug.LogError("Please init appdomain first !");
                    return;
                }
            }
            else if (!string.IsNullOrEmpty(DllName.Value) && MultiDllManager.Instance != null && MultiDllManager.Instance.AllDllInstance.ContainsKey(DllName.Value))
            {
                if (GenClass == null && !string.IsNullOrEmpty(ScriptClassName.Value))
                {
                    GenClass = MultiDllManager.Instance.AllDllInstance[DllName.Value].appdomain.Instantiate<DllDecoratorBase>(ScriptClassName.Value);
                }

                if (GenClass != null)
                {
                    GenClass.BaseAction = this;

                    //if (!bInit)
                    {
                        GenClass.Init();

                        //bInit = true;
                    }

                    GenClass.Awake();
                }
            }
            else
            {
                if (GenClass == null && !string.IsNullOrEmpty(ScriptClassName.Value))
                {
                    GenClass = DllManager.appdomain.Instantiate<DllDecoratorBase>(ScriptClassName.Value);
                }

                if (GenClass != null)
                {
                    GenClass.BaseAction = this;

                    //if (!bInit)
                    {
                        GenClass.Init();

                        //bInit = true;
                    }

                    GenClass.Awake();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error Awake " + "DllName : " + DllName + "ScriptClassName : " + ScriptClassName + " gameObject.name : " + gameObject.name + "\r\n" + e.ToString());
        }
    }

    public override bool CanUpdate()
    {
        if(GenClass != null)
        {
            return GenClass.CanUpdate();
        }
        return false;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    protected override void OnStart()
    {
        if(GenClass != null)
        {
            GenClass.Start();
        }
    }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override Status OnUpdate()
    {
        if (GenClass != null)
        {
            return GenClass.OnUpdate();
        }
        return Status.Failure;
    }
    /// <summary>
    /// 装饰子结点返回值
    /// </summary>
    /// <param name="childStatus"></param>
    /// <returns></returns>
    protected override Status OnDecorate(Status childStatus)
    {
        if (GenClass != null)
        {
            return GenClass.OnDecorate(childStatus);
        }
        return childStatus;
    }
    /// <summary>
    /// 装饰子判断结点(Conditional)的CanUpdate返回值
    /// </summary>
    /// <param name="childCanUpdate"></param>
    /// <returns></returns>
    protected override bool OnDecorate(bool childCanUpdate)
    {
        if (GenClass != null)
        {
            return GenClass.OnDecorate(childCanUpdate);
        }
        return childCanUpdate;
    }
}
