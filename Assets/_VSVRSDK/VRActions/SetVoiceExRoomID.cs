using UnityEngine;
using com.ootii.Messages;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVoiceExRoomID : FsmStateAction
    {
        public FsmString ExRoomID;

        // Code that runs on entering the state.this
        public override void OnEnter()
        {
            MessageDispatcher.SendMessage(this, VoiceDispMessageType.ConnectVoiceExRoom.ToString(), ExRoomID.Value, 0);
            Finish();
        }
    }

}
