using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using DG.Tweening;
public class RoomHideOtherController : MonoBehaviour
{
    bool pointed = false;

    public GameObject Pointobj;
    public GameObject ColoredObj;
    public GameObject ShowObj;
    public GameObject HideObj;
    public Color entercolor;
    Color startcolor;
    void Start()
    {
        MessageDispatcher.AddListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj);
        MessageDispatcher.AddListener(VRPointObjEventType.VRPointEnter.ToString(), VRPointEnter);
        MessageDispatcher.AddListener(VRPointObjEventType.VRPointExit.ToString(), VRPointExit);
        startcolor = ColoredObj.transform.GetComponent<MeshRenderer>().material.color;
        MessageDispatcher.AddListener(CommonVREventType.VRCommitButtonClick.ToString(), VRClick);
    }

    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj);
        MessageDispatcher.RemoveListener(VRPointObjEventType.VRPointEnter.ToString(), VRPointEnter);
        MessageDispatcher.RemoveListener(VRPointObjEventType.VRPointExit.ToString(), VRPointExit);
        MessageDispatcher.RemoveListener(CommonVREventType.VRCommitButtonClick.ToString(), VRClick);
    }

    void RecieveChangeObj(IMessage msg)
    {
        WsChangeInfo newtchangeinfo = (WsChangeInfo)msg.Data;
        //Debug.LogWarning("ChangeObj :  " + newtchangeinfo.name + "   To : " + newtchangeinfo.changenum);
        if (newtchangeinfo.id != mStaticThings.I.mAvatarID)
        {
            if (newtchangeinfo.name == Pointobj.name)
            {
                if (newtchangeinfo.kind == "show")
                {
                    HideObj.SetActive(false);
                    ShowObj.SetActive(true);
                }
            }
        }
    }


    void VRClick(IMessage msg)
    {
        if (pointed)
        {
            string nowchange = "0";
            //Debug.LogWarning("sssssssssssssssssssss");
            HideObj.SetActive(false);
            ShowObj.SetActive(true);

            WsChangeInfo newchangeinfo = new WsChangeInfo
            {
                id = mStaticThings.I.mAvatarID,
                name = Pointobj.name,
                kind = "show",
                changenum = nowchange
            };
            MessageDispatcher.SendMessage(this, WsMessageType.SendChangeObj.ToString(), newchangeinfo, 0);
        }
    }

    void VRPointEnter(IMessage msg)
    {
        if ((GameObject)msg.Data == Pointobj)
        {
            pointed = true;
            ColoredObj.transform.GetComponent<MeshRenderer>().material.color = entercolor;
        }
    }

    void VRPointExit(IMessage msg)
    {
        if ((GameObject)msg.Data == Pointobj)
        {
            pointed = false;
            ColoredObj.transform.GetComponent<MeshRenderer>().material.color = startcolor;
        }
    }
}
