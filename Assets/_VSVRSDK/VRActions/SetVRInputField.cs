using UnityEngine;
using com.ootii.Messages;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVRInputField : FsmStateAction
    {
        public InputField input;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            MessageDispatcher.SendMessage(input, VrDispMessageType.InputFildClicked.ToString(), input.text, 0);
            Finish();
        }
    }

}
