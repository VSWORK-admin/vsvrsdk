using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVRSystemMemuEnable : FsmStateAction
    {
        public FsmBool enabled;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if(mStaticThings.I == null){return;}
            MessageDispatcher.SendMessage(this,VrDispMessageType.SystemMenuEnable.ToString(),enabled.Value,0);
            Finish();
        }
    }
}
