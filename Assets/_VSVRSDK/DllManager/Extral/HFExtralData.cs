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
public class HFExtralData : MonoBehaviour
{
    public string OtherData;
    public ExtralData[] ExtralDatas;
    public ExtralDataObj[] ExtralDataObjs;
}


