using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using DG.Tweening;
public class RoomAnimationController : MonoBehaviour
{
    bool selected = false;
    bool isopen = false;
    public DOTweenAnimation dta;
    Color startcolor;
    void Start()
    {
        MessageDispatcher.AddListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj);
        MessageDispatcher.AddListener(VRPointObjEventType.VRPointEnter.ToString(), VRPointEnter);
        MessageDispatcher.AddListener(VRPointObjEventType.VRPointExit.ToString(), VRPointExit);
        startcolor = transform.GetComponent<MeshRenderer>().material.color;
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
        Debug.LogWarning("ChangeObj :  " + newtchangeinfo.name + "   To : " + newtchangeinfo.changenum);
        if (newtchangeinfo.id != mStaticThings.I.mAvatarID)
        {
            if (newtchangeinfo.name == gameObject.name)
            {
                if (newtchangeinfo.kind == "ani")
                {
                    if (newtchangeinfo.changenum == "0")
                    {
                        dta.DOPlayBackwards();
                        isopen = false;
                    }
                    else if (newtchangeinfo.changenum == "1")
                    {
                        dta.DOPlayForward();
                        isopen = true;
                    }
                }
            }
        }
    }

    void VRClick(IMessage msg)
    {
        if(selected){
            string nowchange = "0";
            //Debug.LogWarning("sssssssssssssssssssss");
            if (!isopen)
            {
                dta.DOPlayForward();
                isopen = true;
                nowchange = "1";
            }
            else
            {
                dta.DOPlayBackwards();
                isopen = false;
                nowchange = "0";
            }

            WsChangeInfo newchangeinfo = new WsChangeInfo
            {
                id = mStaticThings.I.mAvatarID,
                name = gameObject.name,
                kind = "ani",
                changenum = nowchange
            };
            MessageDispatcher.SendMessage(this, WsMessageType.SendChangeObj.ToString(), newchangeinfo, 0);
        }
    }

    void VRPointEnter(IMessage msg)
    {
        if ((GameObject)msg.Data == gameObject)
        {
            selected = true;
            transform.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    void VRPointExit(IMessage msg)
    {
        if ((GameObject)msg.Data == gameObject)
        {
            selected = false;
            transform.GetComponent<MeshRenderer>().material.color = startcolor;
        }
    }
}
