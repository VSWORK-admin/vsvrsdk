using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AutoPathFindingRequestData
{
    public GameObject followtarget;  //时刻跟随的目标
    public Vector3 targetpos;       //目标点位寻路

    public Vector3 offset;      //跟随偏移
    public int movetype;  // 0 walk 1 run
}

[Serializable]
public class AutoPathFindingRespondData
{
    public bool haspath;    //是否可行
    public Vector3 targetpos;    //终点位置
    public Quaternion targetrot;
}