using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class VRSendLocalCustomMessage : MonoBehaviour
{
    public void SendCustomMessage(string CustomMessage){
        MessageDispatcher.SendMessage(this, VrDispMessageType.CustomLocalMessage.ToString(), CustomMessage, 0);
    }
}
