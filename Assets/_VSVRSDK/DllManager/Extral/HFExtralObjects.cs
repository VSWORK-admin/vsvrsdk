using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HFExtralObjects : MonoBehaviour
{
    public string Intro;
    public ExtralObjects[] ObjList;
}

[System.Serializable]
public class ExtralObjects
{
    public string Intro;
    public UnityEngine.Object Target;
    public ExtralObjects[] Info;
}
