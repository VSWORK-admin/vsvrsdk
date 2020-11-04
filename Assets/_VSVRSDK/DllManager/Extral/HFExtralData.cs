﻿using System;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class ExtralData
{
    public string OtherData;
    public Transform Target;

    public ExtralData[] Info;
}
public class HFExtralData : MonoBehaviour
{
    public string OtherData;
    public ExtralData[] ExtralDatas;
}
