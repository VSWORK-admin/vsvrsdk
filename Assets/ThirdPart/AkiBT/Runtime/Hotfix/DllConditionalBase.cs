using Kurisu.AkiBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DllConditionalBase
{
    public GeneratedDllConditional BaseAction = null;
    public virtual void Abort()
    {
        
    }
    public virtual void Init()
    {
    }
    public virtual void Awake()
    {
    }

    public virtual bool CanUpdate()
    {
        return false;
    }

    public virtual void Start()
    {
    }

    public virtual Status OnUpdate()
    {
        return Status.Failure;
    }

    public virtual bool IsUpdatable()
    {
        return false;
    }
    public virtual void OnStart()
    {
    }
}
