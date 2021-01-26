using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetNormalScreenCamera : FsmStateAction
    {
        public PCScreenCameraMark CameraMark;
        public FsmBool everyframe;
        // Code that runs on entering the state.
        public override void OnEnter()
        {
            MessageDispatcher.SendMessage(this, VrDispMessageType.CameraScreenCameraSet.ToString(), CameraMark, 0);
            if (!everyframe.Value)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            if (everyframe.Value)
            {
                MessageDispatcher.SendMessage(this, VrDispMessageType.CameraScreenCameraSet.ToString(), CameraMark, 0);
            }
        }
    }
}
