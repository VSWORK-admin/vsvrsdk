using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetCustomVRInput : FsmStateAction
    {
        public CommonVREventType InputType;
        public Vibrationinfo vibinfo;
        // Code that runs on entering the state.
        public override void OnEnter()
        {
            //MessageDispatcher.(InputType.ToString(), GetInput);
            if (InputType == CommonVREventType.ControllerVibration)
            {
                MessageDispatcher.SendMessage(this, InputType.ToString(), vibinfo, 0);
            }
            else
            {
                MessageDispatcher.SendMessage(this, InputType.ToString(), true, 0);
            }
            Finish();
        }
    }
}
