using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AutoPathFindingRequestData
{
    public GameObject followtarget;  //ʱ�̸����Ŀ��
    public Vector3 targetpos;       //Ŀ���λѰ·

    public Vector3 offset;      //����ƫ��
    public int movetype;  // 0 walk 1 run
}

[Serializable]
public class AutoPathFindingRespondData
{
    public bool haspath;    //�Ƿ����
    public Vector3 targetpos;    //�յ�λ��
    public Quaternion targetrot;
}