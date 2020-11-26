using UnityEngine;
using com.ootii.Messages;
using UnityEngine.Video;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class RecieveKODTXTString : FsmStateAction
    {
        // Code that runs on entering the state.
        public FsmString txtstring;
        public FsmEvent RecieveTxtEvent;
        public override void OnEnter()
        {
            MessageDispatcher.AddListener(VrDispMessageType.KODGetTxtString.ToString(), KODGetTxtString);
        }
        // Code that runs when exiting the state.
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.KODGetTxtString.ToString(), KODGetTxtString);
        }
        void KODGetTxtString(IMessage msg)
        {
            string txt = msg.Data as string;
            txtstring.Value = txt;
            Fsm.Event(RecieveTxtEvent);
        }
    }

}
