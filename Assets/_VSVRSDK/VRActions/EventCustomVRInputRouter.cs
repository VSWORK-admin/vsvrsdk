using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("VRActions")]
	public class EventCustomVRInputRouter : FsmStateAction
	{
		public CommonVREventType InputType;
		public VRPointObjEventType PointEventType;
		public FsmGameObject MessageObject;
		public FsmString Fsm2DAxisName;
		public FsmString Fsm1DAxisName;
		public FsmString FsmEventName;
		
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
			
			if(msg.Type == CommonVREventType.VR_LefStickAxis.ToString() || msg.Type == CommonVREventType.VR_RightStickAxis.ToString()){
				MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmVector2(Fsm2DAxisName.Value).Value = (Vector2)msg.Data;
			}else if(msg.Type == CommonVREventType.VR_LeftTriggerAxis.ToString() || msg.Type == CommonVREventType.VR_RightTriggerAxis.ToString() || msg.Type == CommonVREventType.VR_LeftGrabAxis.ToString() || msg.Type == CommonVREventType.VR_RightGrabAxis.ToString()){
				MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmFloat(Fsm1DAxisName.Value).Value = (float)msg.Data;
			}
			MessageObject.Value.GetComponent<PlayMakerFSM>().SendEvent(FsmEventName.Value);

		}
	}

}
