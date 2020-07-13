using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("VRActions")]
	public class EventVRSystemMenuChange : FsmStateAction
	{
		public FsmEvent SystemMenuOn;
		public FsmEvent SystemMenuOff;
		public FsmEvent SceneMenuOn;
		public FsmEvent SceneMenuOff;
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			MessageDispatcher.AddListener(VrDispMessageType.SystemMenuEvent.ToString(), SystemMenuAction);
			MessageDispatcher.AddListener(VrDispMessageType.SceneMenuEnable.ToString(), SceneMenuAction);
		}

		public override void OnExit()
		{
			MessageDispatcher.RemoveListener(VrDispMessageType.SystemMenuEvent.ToString(), SystemMenuAction);
			MessageDispatcher.RemoveListener(VrDispMessageType.SceneMenuEnable.ToString(), SceneMenuAction);
		}

		void SystemMenuAction(IMessage msg){
			bool ison = (bool)msg.Data;

			if(ison){
				Fsm.Event(SystemMenuOn);
			}else{
				Fsm.Event(SystemMenuOff);
			}
		}
		void SceneMenuAction(IMessage msg){
			bool ison = (bool)msg.Data;
			if(ison){
				Fsm.Event(SceneMenuOn);
			}else{
				Fsm.Event(SceneMenuOff);
			}
		}
	}
}
