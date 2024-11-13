using UnityEngine;
using System.Collections;

public class FrameRateLocker : MonoBehaviour
{
    void Start()
    {
        // 设置帧率锁定为60
        Application.targetFrameRate = 60;

        // 也可以设置LockFrameRate来锁定帧率
        //QualitySettings.lockFrameRate = true;
    }
}