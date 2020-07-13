using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVRBigScreen : FsmStateAction
    {
        public FsmBool enabled;
        public BigScreenSelectController ScreenMark;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if(mStaticThings.I == null){return;}
            WsBigScreen wbs = new WsBigScreen()
            {
                id = mStaticThings.I.mAvatarID,
                enabled = enabled.Value,
                angle = ScreenMark.ScreenAngle,
                position = ScreenMark.transform.position,
                rotation = ScreenMark.transform.rotation,
                scale = ScreenMark.startscale
            };
            MessageDispatcher.SendMessage(this,VrDispMessageType.BigScreenEndAnchor.ToString(),wbs,0);
            Finish();
        }
    }
}
