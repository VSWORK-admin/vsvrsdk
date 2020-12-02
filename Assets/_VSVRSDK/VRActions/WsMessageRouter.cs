using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class WsMessageRouter : FsmStateAction
    {
        // Code that runs on entering the state.
        public FsmGameObject MessageObject;
        public FsmString FsmEventName;
        public FsmString a_name;
        public FsmString b_name;
        public FsmString c_name;
        public FsmString d_name;
        public FsmString e_name;
        public FsmString f_name;
        public FsmString g_name;
        public override void OnEnter()
        {
            MessageDispatcher.AddListener(WsMessageType.RecieveCChangeObj.ToString(), RecieveCChangeObj);
        }

        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(WsMessageType.RecieveCChangeObj.ToString(), RecieveCChangeObj);
        }

        void RecieveCChangeObj(IMessage msg)
        {
            WsCChangeInfo rinfo = msg.Data as WsCChangeInfo;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(a_name.Value).Value = rinfo.a;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(b_name.Value).Value = rinfo.b;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(c_name.Value).Value = rinfo.c;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(d_name.Value).Value = rinfo.d;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(e_name.Value).Value = rinfo.e;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(f_name.Value).Value = rinfo.f;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(g_name.Value).Value = rinfo.g;
            MessageObject.Value.GetComponent<PlayMakerFSM>().SendEvent(FsmEventName.Value);
        }
    }
}
