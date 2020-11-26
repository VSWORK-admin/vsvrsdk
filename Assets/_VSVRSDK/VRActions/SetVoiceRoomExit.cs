using UnityEngine;
using com.ootii.Messages;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVoiceRoomExit : FsmStateAction
    {
        // Code that runs on entering the state.this
        public override void OnEnter()
        {
            MessageDispatcher.SendMessage(VoiceDispMessageType.ExitVoiceRoom.ToString());
            Finish();
        }
    }

}
