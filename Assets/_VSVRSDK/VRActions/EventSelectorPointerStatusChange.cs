using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class EventSelectorPointerStatusChange : FsmStateAction
    {

        // Code that runs on entering the state.
        public FsmBool PointEnabled;
        public FsmEvent PointTrue;
        public FsmEvent PointFalse;
        public override void OnEnter()
        {
			if(mStaticThings.I != null){
				PointEnabled.Value = mStaticThings.I.SelectorEnabled;
			}
            MessageDispatcher.AddListener(VrDispMessageType.SelectorPointerStatusChange.ToString(), SelectorPointerStatusChange);
        }
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.SelectorPointerStatusChange.ToString(), SelectorPointerStatusChange);
        }
        void SelectorPointerStatusChange(IMessage msg)
        {
            bool ispointon = (bool)msg.Data;
			PointEnabled.Value = ispointon;
            if (ispointon)
            {
                Fsm.Event(PointTrue);
            }
            else
            {
                Fsm.Event(PointFalse);
            }
        }

    }

}
