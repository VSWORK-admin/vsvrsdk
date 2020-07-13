using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVRVideoPlayer : FsmStateAction
    {
        public FsmGameObject ContorlObj;
        public GameObject[] RenderObj;
        public FsmString url;

        [Tooltip("vol value range:  0 - 100")]
        public FsmFloat vol;
        public FsmBool isloop;
        public FsmBool autostart;
        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if(mStaticThings.I == null){
                return;
            }
            CustomVideoPlayer cvp =new CustomVideoPlayer{
                ContorlObj = ContorlObj.Value,
                RenderObj = RenderObj,
                url = url.Value,
                vol = vol.Value,
                isloop = isloop.Value,
                autostart = autostart.Value
            };
            MessageDispatcher.SendMessage(this, VrDispMessageType.InitVideoPlayer.ToString(), cvp, 0);
            Finish();
        }
    }

}
