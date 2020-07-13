using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{


    [ActionCategory("VRActions")]
    public class SetVRMovingObj : FsmStateAction
    {
        public FsmGameObject MovingObj;
        public FsmBool islocal;
        public FsmBool isSending;
        public FsmInt FrameSend;
        Vector3 nowpos;
        Quaternion nowrot;
        Vector3 nowscal;

        public override void OnEnter()
        {
            MessageDispatcher.AddListener(WsMessageType.RecieveMovingObj.ToString(), RecieveMovingObj);
        }

        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(WsMessageType.RecieveMovingObj.ToString(), RecieveMovingObj);
        }



        void RecieveMovingObj(IMessage msg)
        {
            WsMovingObj newMovingObj = msg.Data as WsMovingObj;
            if (mStaticThings.I == null) { return; }
            if (newMovingObj.id != mStaticThings.I.mAvatarID)
            {
                
                if (newMovingObj.name == MovingObj.Value.name)
                {
                    isSending.Value = false;
                    if (newMovingObj.islocal)
                    {
                        MovingObj.Value.transform.localPosition = newMovingObj.position;
                        MovingObj.Value.transform.localRotation = newMovingObj.rotation;
                    }
                    else
                    {
                        MovingObj.Value.transform.position = newMovingObj.position;
                        MovingObj.Value.transform.rotation = newMovingObj.rotation;
                    }
                    MovingObj.Value.transform.localScale = newMovingObj.scale;
                }
            }
        }

        public override void OnUpdate()
        {
            if (mStaticThings.I == null) { return; }
            if (isSending.Value)
            {
                if (Time.frameCount % FrameSend.Value == 0)
                {
                    Vector3 sendpos;
                    Quaternion sendnowrot;
                    Vector3 sendnowscal;
                    if (islocal.Value)
                    {
                        sendpos = MovingObj.Value.transform.localPosition;
                        sendnowrot = MovingObj.Value.transform.localRotation;
                    }
                    else
                    {
                        sendpos = MovingObj.Value.transform.position;
                        sendnowrot = MovingObj.Value.transform.rotation;
                    }
                    sendnowscal = MovingObj.Value.transform.localScale;

                    if (sendpos != nowpos || sendnowrot != nowrot || sendnowscal != nowscal)
                    {
                        nowpos = sendpos;
                        nowrot = sendnowrot;
                        nowscal = sendnowscal;
                        WsMovingObj sendMovingObj = new WsMovingObj()
                        {
                            id = mStaticThings.I.mAvatarID,
                            name = MovingObj.Value.name,
                            islocal = islocal.Value,
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
}
