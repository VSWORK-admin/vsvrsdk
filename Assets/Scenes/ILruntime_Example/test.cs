using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject clickObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            com.ootii.Messages.MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointClick.ToString(), clickObj, 0);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            WsCChangeInfo rinfo = new WsCChangeInfo();

            rinfo.a = "hideitem";
            rinfo.b = "Sphere";

            com.ootii.Messages.MessageDispatcher.SendMessage(this, WsMessageType.RecieveCChangeObj.ToString(), rinfo, 0);
        }
    }
}
