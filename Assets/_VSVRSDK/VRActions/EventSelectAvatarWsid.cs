using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class EventSelectAvatarWsid : FsmStateAction
    {

        // Code that runs on entering the state.
        public FsmString SelectId;
        public FsmEvent EventSelectId;
        public override void OnEnter()
        {
            MessageDispatcher.AddListener(VrDispMessageType.SelectAvatarWsid.ToString(), SelectAvatarWsid, true);
        }
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.SelectAvatarWsid.ToString(), SelectAvatarWsid, true);
        }
        void SelectAvatarWsid(IMessage msg)
        {
            string selid = (string)msg.Data;
            SelectId.Value = selid;
            Fsm.Event(EventSelectId);
        }

    }

}
