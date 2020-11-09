using UnityEngine;
using com.ootii.Messages;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVRMicEnabled : FsmStateAction
    {
        
        public FsmBool SendMicEnabled;
        public FsmBool SendMicDisabled;
        public FsmBool SendMicOn;
        public FsmBool SendMicOff;
        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if(SendMicEnabled.Value){
                MessageDispatcher.SendMessage(VoiceDispMessageType.GmeMicEnalbe.ToString());
            }else if(SendMicDisabled.Value){
                MessageDispatcher.SendMessage(VoiceDispMessageType.GmeMicDisable.ToString());
            }

            if(SendMicOn.Value){
                MessageDispatcher.SendMessage(VoiceDispMessageType.GmemMicOn.ToString());
            }else if(SendMicOff.Value){
                MessageDispatcher.SendMessage(VoiceDispMessageType.GmeMicOff.ToString());
            }
            
            Finish();
        }
    }

}
