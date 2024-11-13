using System;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class ExtralData
{
    public string OtherData;
    public Transform Target;
    public ExtralData[] Info;
}
[System.Serializable]
public class ExtralDataObj
{
    public string OtherData;
    public UnityEngine.Object Target;
    public ExtralDataObj[] Info;
}
[System.Serializable]
public class ExtralDataInfo
{
    public string OtherData;
    public string stringData;
    public int intData;
    public float floatData;
    public Vector3[] vector3Data;
    public AnimationCurve[] animationCurveData;
    public Color[] colorData;

    public ExtralDataInfo[] Info;
}
public class HFExtralData : MonoBehaviour
{
    public string OtherData;
    public ExtralData[] ExtralDatas;
    public ExtralDataObj[] ExtralDataObjs;
    public ExtralDataInfo[] ExtralDataInfos;
}


