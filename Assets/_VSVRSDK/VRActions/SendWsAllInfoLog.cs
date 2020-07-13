using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SendWsAllInfoLog : FsmStateAction
    {
        public FsmBool mEnabled;
        public FsmString Info;
        public InfoColor infocolor;
        public FsmFloat lasttime;
        public FsmBool SendToOther;
        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if (mEnabled.Value)
            {
                if (mStaticThings.I == null) { return; }
                WsChangeInfo wsinfo = new WsChangeInfo()
                {
                    id = mStaticThings.I.mAvatarID,
                    name = "InfoLog",
                    a = Info.Value,
                    b = infocolor.ToString(),
                    c = lasttime.Value.ToString(),
                };
                if (SendToOther.Value)
                {
                    MessageDispatcher.SendMessage(this, VrDispMessageType.SendAllInfolog.ToString(), wsinfo, 0);
                }
                else
                {
                    MessageDispatcher.SendMessage(this, VrDispMessageType.SendInfolog.ToString(), wsinfo, 0);
                }
            }

            Finish();
        }
    }

}
