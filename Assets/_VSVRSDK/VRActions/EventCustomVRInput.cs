using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("VRActions")]
	public class EventCustomVRInput : FsmStateAction
	{
		public CommonVREventType InputType;
		public FsmEvent InputEvent;
		public FsmVector2 Rcieved2DAxis;
		public FsmFloat Rcieved1DAxis;
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			MessageDispatcher.AddListener(InputType.ToString(), GetInput);
		}

		// Code that runs when exiting the state.
		public override void OnExit()
		{
			MessageDispatcher.RemoveListener(InputType.ToString(), GetInput);
		}
		void GetInput(IMessage msg){
			Fsm.Event(InputEvent);
			if(msg.Type == CommonVREventType.VR_LefStickAxis.ToString() || msg.Type == CommonVREventType.VR_RightStickAxis.ToString()){
				Rcieved2DAxis.Value = (Vector2)msg.Data;
			}else if(msg.Type == CommonVREventType.VR_LeftTriggerAxis.ToString() || msg.Type == CommonVREventType.VR_RightTriggerAxis.ToString() || msg.Type == CommonVREventType.VR_LeftGrabAxis.ToString() || msg.Type == CommonVREventType.VR_RightGrabAxis.ToString()){
				Rcieved1DAxis.Value = (float)msg.Data;
			}
		}
	}

}
