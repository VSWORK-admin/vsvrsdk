using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVRGameObject : FsmStateAction
    {
        public FsmString aid;
        public FsmString name;
        public FsmBool hideavatar;
        public FsmBool SetColor;
        public FsmColor pencolor;
        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if(mStaticThings.I == null){
                return;
            }
            if(aid.Value != "" && aid.Value != null){
                mStaticThings.I.aid = aid.Value;
            }
            if(name.Value != "" && name.Value != null){
                mStaticThings.I.mNickName = name.Value;
            }
            if(SetColor.Value){
                mStaticThings.I.nowpencolor = "#" + ColorUtility.ToHtmlStringRGB(pencolor.Value);
            }
            mStaticThings.I.SendAvatar = !hideavatar.Value;
            Finish();
        }
    }

}
