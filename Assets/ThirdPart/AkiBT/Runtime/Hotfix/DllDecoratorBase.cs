using Kurisu.AkiBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DllDecoratorBase
{
    public GeneratedDllDecorator BaseAction = null;
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
    /// <summary>
    /// 装饰子结点返回值
    /// </summary>
    /// <param name="childStatus"></param>
    /// <returns></returns>
    public virtual Status OnDecorate(Status childStatus)
    {
        return childStatus;
    }
    /// <summary>
    /// 装饰子判断结点(Conditional)的CanUpdate返回值
    /// </summary>
    /// <param name="childCanUpdate"></param>
    /// <returns></returns>
    public virtual bool OnDecorate(bool childCanUpdate)
    {
        return childCanUpdate;
    }
}
