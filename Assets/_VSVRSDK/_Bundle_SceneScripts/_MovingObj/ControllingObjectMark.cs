using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
public class ControllingObjectMark : MonoBehaviour
{
    public Vector3 startscale;
    public Quaternion startrotation;
    public Vector3 startposition;
    void Start()
    {
        MessageDispatcher.AddListener(WsMessageType.RecieveMovingObj.ToString(), RecieveMovingObj);
        SaveRTS();
    }

    public void SaveRTS(){
        startscale = transform.localScale;
        startrotation = transform.localRotation;
        startposition = transform.localPosition;
    }
    void OnDestroy()
    {
        MessageDispatcher.RemoveListener(WsMessageType.RecieveMovingObj.ToString(), RecieveMovingObj);
    }

    public void resetscale(){
        transform.localScale = startscale;
    }
    public void resetrotation(){
        transform.localRotation = startrotation;
    }

    public void resetposition(){
        transform.localPosition = startposition;
    }

    public void resetall(){
        resetscale();
        resetrotation();
        resetposition();
    }

    void RecieveMovingObj(IMessage msg)
    {
        WsMovingObj newMovingObj = msg.Data as WsMovingObj;
        if (mStaticThings.I == null) { return; }
        if (newMovingObj.id != mStaticThings.I.mAvatarID)
        {
            if (newMovingObj.name == gameObject.name)
            {
                if (newMovingObj.mark == "i")
                {
                    if (newMovingObj.islocal)
                    {
                        transform.localPosition = newMovingObj.position;
                        transform.localRotation = newMovingObj.rotation;
                    }
                    else
                    {
                        transform.position = newMovingObj.position;
                        transform.rotation = newMovingObj.rotation;
                    }
                    transform.localScale = newMovingObj.scale;
                    
                }
                else if (newMovingObj.mark == "s")
                {
                    if(GetComponent<Rigidbody>()){
                        GetComponent<Rigidbody>().useGravity = false;
                    }
                }
                else if (newMovingObj.mark == "e")
                {
                    if(GetComponent<Rigidbody>()){
                        GetComponent<Rigidbody>().useGravity = true;
                    }
                }
            }
        }
    }
}
