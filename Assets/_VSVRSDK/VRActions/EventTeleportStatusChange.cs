using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class EventTeleportStatusChange : FsmStateAction
    {

        // Code that runs on entering the state.
        public FsmEvent PointTrue;
        public FsmEvent PointFalse;
        public override void OnEnter()
        {
            MessageDispatcher.AddListener(VrDispMessageType.TeleporterStatusChange.ToString(), TeleporterStatusChange);
        }
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.TeleporterStatusChange.ToString(), TeleporterStatusChange);
        }
        void TeleporterStatusChange(IMessage msg)
        {
            bool ispointon = (bool)msg.Data;
            if (ispointon)
            {
                Fsm.Event(PointTrue);
            }
            else
            {
                Fsm.Event(PointFalse);
            }
        }

    }

}
