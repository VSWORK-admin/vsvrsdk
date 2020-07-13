using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
public class MovingObjectMark : MonoBehaviour
{
    public GameObject MovingObj;
    public bool islocal;
    public bool isSending;
    public int FrameSend;
    Vector3 nowpos;
    Quaternion nowrot;
    Vector3 nowscal;
    void Start()
    {
        MessageDispatcher.AddListener(WsMessageType.RecieveMovingObj.ToString(), RecieveMovingObj);
    }

    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener(WsMessageType.RecieveMovingObj.ToString(), RecieveMovingObj);
    }

    public void SetSending(bool send)
    {
        isSending = send;
    }

    void RecieveMovingObj(IMessage msg)
    {
        WsMovingObj newMovingObj = msg.Data as WsMovingObj;
        if (newMovingObj.id != mStaticThings.I.mAvatarID || !enabled)
        {
            isSending = false;
            if (newMovingObj.name == MovingObj.name)
            {
                if (newMovingObj.islocal)
                {
                    MovingObj.transform.localPosition = newMovingObj.position;
                    MovingObj.transform.localRotation = newMovingObj.rotation;
                }
                else
                {
                    MovingObj.transform.position = newMovingObj.position;
                    MovingObj.transform.rotation = newMovingObj.rotation;
                }
                MovingObj.transform.localScale = newMovingObj.scale;
            }
        }
    }

    public void LateUpdate()
    {
        if (isSending && enabled)
        {
            if (Time.frameCount % FrameSend == 0)
            {
                Vector3 sendpos;
                Quaternion sendnowrot;
                Vector3 sendnowscal;
                if (islocal)
                {
                    sendpos = MovingObj.transform.localPosition;
                    sendnowrot = MovingObj.transform.localRotation;
                }
                else
                {
                    sendpos = MovingObj.transform.position;
                    sendnowrot = MovingObj.transform.rotation;
                }
                sendnowscal = MovingObj.transform.localScale;

                if (sendpos != nowpos || sendnowrot != nowrot || sendnowscal != nowscal)
                {
                    nowpos = sendpos;
                    nowrot = sendnowrot;
                    nowscal = sendnowscal;
                    WsMovingObj sendMovingObj = new WsMovingObj()
                    {
                        id = mStaticThings.I.mAvatarID,
                        name = MovingObj.name,
                        islocal = islocal,
                        position = sendpos,
                        rotation = sendnowrot,
                        scale = sendnowscal
                    };
                    MessageDispatcher.SendMessage(this, WsMessageType.SendMovingObj.ToString(), sendMovingObj, 0);
                }
            }
        }
    }



}
