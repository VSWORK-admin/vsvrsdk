using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlipDir
{
    Down,
    Left,
    UP,
    Right,
}

[Serializable]
public class FlipPointData
{
    public FlipDir dir;

    public Transform jumpPoint;
}

[Serializable]
public class FlipStationData
{
    [HideInInspector]
    public FlipDir flipto;

    public FlipDir curdir;

    public List<FlipPointData> dicJumpPoint = new List<FlipPointData>();
}
