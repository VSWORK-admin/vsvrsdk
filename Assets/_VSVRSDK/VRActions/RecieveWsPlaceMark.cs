using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class RecieveWsPlaceMark : FsmStateAction
    {
        // Code that runs on entering the state.
        public FsmString wsGroupName;
        public FsmString wsMyPlaceName;
        public FsmEvent RecieveSameGroupName;
        public FsmEvent RecieveMySamePlaceaName;
        public FsmString RecievedGroupName;
        public FsmString RecievedPlaceName;
        public override void OnEnter()
        {
            RecievedGroupName.Value = "";
            RecievedPlaceName.Value = "";
            MessageDispatcher.AddListener(WsMessageType.RecievePlaceMark.ToString(), RecievePlaceMark);
            MessageDispatcher.AddListener(VrDispMessageType.AllPlaceTo.ToString(), AllPlaceTo);
            MessageDispatcher.AddListener(VrDispMessageType.SelfPlaceTo.ToString(), SelfPlaceTo);
        }

        // Code that runs when exiting the state.
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(WsMessageType.RecievePlaceMark.ToString(), RecievePlaceMark);
            MessageDispatcher.RemoveListener(VrDispMessageType.AllPlaceTo.ToString(), AllPlaceTo);
            MessageDispatcher.RemoveListener(VrDispMessageType.SelfPlaceTo.ToString(), SelfPlaceTo);
        }

        void SelfPlaceTo(IMessage msg)
        {
            string dname = (string)msg.Data;
            RecievedPlaceName.Value = dname;
            if (dname == wsMyPlaceName.Value)
            {
                Fsm.Event(RecieveMySamePlaceaName);
            }
        }
        void AllPlaceTo(IMessage msg)
        {
            string gname = (string)msg.Data;
            RecievedGroupName.Value = gname;
            if (gname == wsGroupName.Value)
            {
                Fsm.Event(RecieveSameGroupName);
            }
        }
        void RecievePlaceMark(IMessage msg)
        {
            WsPlaceMarkList newwpmlist = (WsPlaceMarkList)msg.Data;
            if (newwpmlist.kind == WsPlaycePortKind.all)
            {
                RecievedGroupName.Value = newwpmlist.gname;
                if (newwpmlist.gname == wsGroupName.Value)
                {
                    Fsm.Event(RecieveSameGroupName);
                }
            }
        }
    }
}
