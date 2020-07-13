using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{


    [ActionCategory("VRActions")]
    public class SetVRControllingObj : FsmStateAction
    {
        public FsmGameObject ControllingObj;
        public FsmBool islocal;
        public FsmBool isSending;
        public FsmBool TriiggerStart;
        public FsmBool TriggerEnd;
        public FsmEvent TriggerEndEvent;
        public FsmInt FrameSend;
        Vector3 nowpos;
        Quaternion nowrot;
        Vector3 nowscal;
        public FsmEvent ControllStart;
        public FsmEvent Controlling;
        public FsmEvent ControllEnd;

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
                if (newMovingObj.name == ControllingObj.Value.name)
                {
                    isSending.Value = false;
                    if (newMovingObj.mark == "i")
                    {
                        if (newMovingObj.islocal)
                        {
                            ControllingObj.Value.transform.localPosition = newMovingObj.position;
                            ControllingObj.Value.transform.localRotation = newMovingObj.rotation;
                        }
                        else
                        {
                            ControllingObj.Value.transform.position = newMovingObj.position;
                            ControllingObj.Value.transform.rotation = newMovingObj.rotation;
                        }
                        ControllingObj.Value.transform.localScale = newMovingObj.scale;
                        Fsm.Event(Controlling);
                    }
                    else if (newMovingObj.mark == "s")
                    {
                        Fsm.Event(ControllStart);
                    }
                    else if (newMovingObj.mark == "e")
                    {
                        Fsm.Event(ControllEnd);
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            if (mStaticThings.I == null) { return; }
            if (TriiggerStart.Value)
            {
                WsMovingObj sendMovingObj = new WsMovingObj()
                {
                    id = mStaticThings.I.mAvatarID,
                    name = ControllingObj.Value.name,
                    islocal = islocal.Value,
                    mark = "s"
                };
                MessageDispatcher.SendMessage(this, WsMessageType.SendMovingObj.ToString(), sendMovingObj, 0);
                TriiggerStart.Value = false;
                return;
            }
            if (TriggerEnd.Value)
            {
                WsMovingObj sendMovingObj = new WsMovingObj()
                {
                    id = mStaticThings.I.mAvatarID,
                    name = ControllingObj.Value.name,
                    islocal = islocal.Value,
                    mark = "e"
                };
                MessageDispatcher.SendMessage(this, WsMessageType.SendMovingObj.ToString(), sendMovingObj, 0);
                TriggerEnd.Value = false;
                Fsm.Event(TriggerEndEvent);
                return;
            }

            if (isSending.Value)
            {
                if (Time.frameCount % FrameSend.Value == 0)
                {
                    Vector3 sendpos;
                    Quaternion sendnowrot;
                    Vector3 sendnowscal;
                    if (islocal.Value)
                    {
                        sendpos = ControllingObj.Value.transform.localPosition;
                        sendnowrot = ControllingObj.Value.transform.localRotation;
                    }
                    else
                    {
                        sendpos = ControllingObj.Value.transform.position;
                        sendnowrot = ControllingObj.Value.transform.rotation;
                    }
                    sendnowscal = ControllingObj.Value.transform.localScale;

                    if (sendpos != nowpos || sendnowrot != nowrot || sendnowscal != nowscal)
                    {
                        nowpos = sendpos;
                        nowrot = sendnowrot;
                        nowscal = sendnowscal;
                        WsMovingObj sendMovingObj = new WsMovingObj()
                        {
                            id = mStaticThings.I.mAvatarID,
                            name = ControllingObj.Value.name,
                            islocal = islocal.Value,
                            mark = "i",
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
