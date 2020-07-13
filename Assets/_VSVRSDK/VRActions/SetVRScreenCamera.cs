using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVRScreenCamera : FsmStateAction
    {
        public PCScreenCameraMark CameraMark;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            MessageDispatcher.SendMessage(this, VrDispMessageType.CameraScreenSettingEnd.ToString(), CameraMark, 0);
            Finish();
        }
    }
}
