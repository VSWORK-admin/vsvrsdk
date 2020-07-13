using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class EventRoomConnectionChange : FsmStateAction
    {

        // Code that runs on entering the state.
        public FsmBool ByPass;
        public FsmBool IsRoomConnectReady;
        public FsmEvent ChangeReady;
        public FsmEvent ChangeUnReady;
        public override void OnEnter()
        {
            if (ByPass.Value)
            {
                IsRoomConnectReady.Value = true;
                Fsm.Event(ChangeReady);
            }
            else
            {
                if (mStaticThings.I == null)
                {
                    IsRoomConnectReady.Value = false;
                }
                else
                {
                    IsRoomConnectReady.Value = mStaticThings.I.WsAvatarIsReady;
                    MessageDispatcher.AddListener(VrDispMessageType.RoomConnected.ToString(), RoomConnected);
                    MessageDispatcher.AddListener(VrDispMessageType.RoomDisConnected.ToString(), RoomDisConnected);
                }
            }


        }
        public override void OnExit()
        {
            if (mStaticThings.I != null && !ByPass.Value)
            {
                MessageDispatcher.RemoveListener(VrDispMessageType.RoomConnected.ToString(), RoomConnected);
                MessageDispatcher.RemoveListener(VrDispMessageType.RoomDisConnected.ToString(), RoomDisConnected);
            }

        }
        void RoomConnected(IMessage msg)
        {
            IsRoomConnectReady.Value = true;
            Fsm.Event(ChangeReady);
        }
        void RoomDisConnected(IMessage msg)
        {
            IsRoomConnectReady.Value = false;
            Fsm.Event(ChangeUnReady);
        }

    }

}
