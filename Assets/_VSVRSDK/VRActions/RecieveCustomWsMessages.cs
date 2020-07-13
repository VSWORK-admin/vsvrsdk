using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class RecieveCustomWsMessages : FsmStateAction
    {

        // Code that runs on entering the state.
        public FsmBool ByPass;
        public FsmEvent Recieve;
        public bool abcdefgFlow;
        public FsmString ws_a;
        public FsmEvent RecieveSame_a;
        public FsmString ws_b;
        public FsmEvent RecieveSame_b;
        public FsmString ws_c;
        public FsmEvent RecieveSame_c;
        public FsmString ws_d;
        public FsmEvent RecieveSame_d;
        public FsmString ws_e;
        public FsmEvent RecieveSame_e;
        public FsmString ws_f;
        public FsmEvent RecieveSame_f;
        public FsmString ws_g;
        public FsmEvent RecieveSame_g;

        public FsmString a;
        public FsmString b;
        public FsmString c;
        public FsmString d;
        public FsmString e;
        public FsmString f;
        public FsmString g;
        public override void OnEnter()
        {
            MessageDispatcher.AddListener(WsMessageType.RecieveCChangeObj.ToString(), RecieveCChangeObj);
            if (ByPass.Value)
            {
                Fsm.Event(Recieve);
                Fsm.Event(RecieveSame_a);
                Fsm.Event(RecieveSame_b);
                Fsm.Event(RecieveSame_c);
                Fsm.Event(RecieveSame_d);
                Fsm.Event(RecieveSame_e);
                Fsm.Event(RecieveSame_f);
                Fsm.Event(RecieveSame_g);
                Finish();
            }
        }

        // Code that runs when exiting the state.
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(WsMessageType.RecieveCChangeObj.ToString(), RecieveCChangeObj);
        }

        void RecieveCChangeObj(IMessage msg)
        {
            WsCChangeInfo rinfo = msg.Data as WsCChangeInfo;

            a.Value = rinfo.a;
            b.Value = rinfo.b;
            c.Value = rinfo.c;
            d.Value = rinfo.d;
            e.Value = rinfo.e;
            f.Value = rinfo.f;
            g.Value = rinfo.g;
            Fsm.Event(Recieve);
            if (abcdefgFlow)
            {
                if (rinfo.a == ws_a.Value)
                {
                    Fsm.Event(RecieveSame_a);
                }
                else if (rinfo.b == ws_b.Value)
                {
                    Fsm.Event(RecieveSame_b);
                }
                else if (rinfo.c == ws_c.Value)
                {
                    Fsm.Event(RecieveSame_c);
                }
                else if (rinfo.d == ws_d.Value)
                {
                    Fsm.Event(RecieveSame_d);
                }
                else if (rinfo.e == ws_e.Value)
                {
                    Fsm.Event(RecieveSame_e);
                }
                else if (rinfo.f == ws_f.Value)
                {
                    Fsm.Event(RecieveSame_f);
                }
                else if (rinfo.g == ws_g.Value)
                {
                    Fsm.Event(RecieveSame_g);
                }
            }
            else
            {
                if (rinfo.a == ws_a.Value)
                {
                    Fsm.Event(RecieveSame_a);
                }
                if (rinfo.b == ws_b.Value)
                {
                    Fsm.Event(RecieveSame_b);
                }
                if (rinfo.c == ws_c.Value)
                {
                    Fsm.Event(RecieveSame_c);
                }
                if (rinfo.d == ws_d.Value)
                {
                    Fsm.Event(RecieveSame_d);
                }
                if (rinfo.e == ws_e.Value)
                {
                    Fsm.Event(RecieveSame_e);
                }
                if (rinfo.f == ws_f.Value)
                {
                    Fsm.Event(RecieveSame_f);
                }
                if (rinfo.g == ws_g.Value)
                {
                    Fsm.Event(RecieveSame_g);
                }
            }

        }

    }

}
