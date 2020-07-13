using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class RecieveSystemWsMessages : FsmStateAction
    {
        // Code that runs on entering the state.
        public FsmEvent Recieve;
        public FsmString id;
        public FsmString name;
        public FsmString kind;
        public FsmString changenum;
        public FsmString a;
        public FsmString b;
        public FsmString c;
        public FsmString d;
        public FsmString e;

        public override void OnEnter()
        {
            MessageDispatcher.AddListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj);
        }

        // Code that runs when exiting the state.
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj);
        }
        void RecieveChangeObj(IMessage msg)
        {
            WsChangeInfo rinfo = msg.Data as WsChangeInfo;
            id = rinfo.id;
            name = rinfo.name;
            kind = rinfo.kind;
            changenum = rinfo.changenum;
            a.Value = rinfo.a;
            b.Value = rinfo.b;
            c.Value = rinfo.c;
            d.Value = rinfo.d;
            e.Value = rinfo.e;
            Fsm.Event(Recieve);
        }
    }
}
