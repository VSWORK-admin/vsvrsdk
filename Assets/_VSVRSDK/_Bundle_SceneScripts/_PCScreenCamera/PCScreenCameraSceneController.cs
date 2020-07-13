using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
public class PCScreenCameraSceneController : MonoBehaviour
{
    public GameObject startCameraMark;
    public GameObject[] CamScreenmarks;
    PCScreenCameraMark lastmark;
    // Start is called before the first frame update
    void Start()
    {
        setMarksEnable(false);
        if (mStaticThings.I == null) return;
        MessageDispatcher.AddListener(VrDispMessageType.CameraScreenSetFree.ToString(), CameraScreenSetFree);
        MessageDispatcher.AddListener(VrDispMessageType.CameraScreenSettingBegin.ToString(), CameraScreenSettingBegin);
        MessageDispatcher.AddListener(VrDispMessageType.CameraScreenSettingEnd.ToString(), CameraScreenSettingEnd);
        PCScreenCameraMark startmark;
        if (startCameraMark != null)
        {
            startmark = startCameraMark.GetComponent<PCScreenCameraMark>();
            lastmark = startmark;
            MessageDispatcher.SendMessage(this, VrDispMessageType.CameraScreenSceneStart.ToString(), startmark, 0);
        }
        else
        {
            PCScreenCameraMark psmk = new PCScreenCameraMark()
            {
                isfree = true
            };
            MessageDispatcher.SendMessage(this, VrDispMessageType.CameraScreenSceneStart.ToString(), psmk, 0);

        }

    }

    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener(VrDispMessageType.CameraScreenSetFree.ToString(), CameraScreenSetFree);
        MessageDispatcher.RemoveListener(VrDispMessageType.CameraScreenSettingBegin.ToString(), CameraScreenSettingBegin);
        MessageDispatcher.RemoveListener(VrDispMessageType.CameraScreenSettingEnd.ToString(), CameraScreenSettingEnd);
    }

    void CameraScreenSetFree(IMessage msg)
    {
        bool isfree = (bool)msg.Data;
        if (isfree)
        {
            if (lastmark != null)
            {
                MessageDispatcher.SendMessage(this, VrDispMessageType.CameraScreenSettingEnd.ToString(), lastmark, 0);
            }
            else
            {
                PCScreenCameraMark psmk = new PCScreenCameraMark()
                {
                    isfree = true
                };
                MessageDispatcher.SendMessage(this, VrDispMessageType.CameraScreenSettingEnd.ToString(), psmk, 0);
            }
        }
    }

    void CameraScreenSettingBegin(IMessage msg)
    {
        setMarksEnable(true);
    }

    void CameraScreenSettingEnd(IMessage msg)
    {
        PCScreenCameraMark nowmark = msg.Data as PCScreenCameraMark;
        lastmark = nowmark;
        setMarksEnable(false);
    }

    void setMarksEnable(bool en)
    {
        if (CamScreenmarks.Length == 0)
        {
            return;
        }
        foreach (var item in CamScreenmarks)
        {
            item.SetActive(en);
        }
    }
}
