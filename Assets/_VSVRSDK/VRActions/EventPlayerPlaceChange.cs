using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class EventPlayerPlaceChange : FsmStateAction
    {
        // Code that runs on entering the state.
        public FsmString RecievedGroupName;
        public FsmString RecievedPlaceName;
        public FsmString RecievePlaceMeshName;
        public FsmEvent EventRecieveGroupName;
        public FsmEvent EventRecievePlaceaName;
        public FsmEvent EventRecievePlaceMeshName;
        public override void OnEnter()
        {
            MessageDispatcher.AddListener(WsMessageType.RecievePlaceMark.ToString(), RecievePlaceMark);
            MessageDispatcher.AddListener(VrDispMessageType.AllPlaceTo.ToString(), AllPlaceTo);
            MessageDispatcher.AddListener(VrDispMessageType.SelfPlaceTo.ToString(), SelfPlaceTo);
            MessageDispatcher.AddListener(VrDispMessageType.TelePortToMesh.ToString(), TelePortToMesh);
        }

        // Code that runs when exiting the state.
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(WsMessageType.RecievePlaceMark.ToString(), RecievePlaceMark);
            MessageDispatcher.RemoveListener(VrDispMessageType.AllPlaceTo.ToString(), AllPlaceTo);
            MessageDispatcher.RemoveListener(VrDispMessageType.SelfPlaceTo.ToString(), SelfPlaceTo);
            MessageDispatcher.RemoveListener(VrDispMessageType.TelePortToMesh.ToString(), TelePortToMesh);
        }


        void TelePortToMesh(IMessage msg){
            string dname = (string)msg.Data;
            RecievePlaceMeshName.Value = dname;
            Fsm.Event(EventRecievePlaceMeshName);
        }
        void SelfPlaceTo(IMessage msg)
        {
            string dname = (string)msg.Data;
            RecievedPlaceName.Value = dname;
            Fsm.Event(EventRecievePlaceaName);
        }
        void AllPlaceTo(IMessage msg)
        {
            string gname = (string)msg.Data;
            RecievedGroupName.Value = gname;
            Fsm.Event(EventRecieveGroupName);
        }
        void RecievePlaceMark(IMessage msg)
        {
            WsPlaceMarkList newwpmlist = (WsPlaceMarkList)msg.Data;
            if (newwpmlist.kind == WsPlaycePortKind.all)
            {
                RecievedGroupName.Value = newwpmlist.gname;
                Fsm.Event(EventRecieveGroupName);
            }
        }
    }
}
