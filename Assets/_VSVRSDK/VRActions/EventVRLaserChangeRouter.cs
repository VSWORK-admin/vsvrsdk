using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("VRActions")]
	public class EventVRLaserChangeRouter : FsmStateAction
	{
		public VRPointObjEventType PointEventType;
		public FsmGameObject MessageObject;
		public FsmString FsmValName;
		public FsmString FsmEventName;
		

		// Code that runs on entering the state.
		public override void OnEnter()
		{
			MessageDispatcher.AddListener(PointEventType.ToString(), GetPointEventType);
		}

		public override void OnExit()
		{
			MessageDispatcher.RemoveListener(PointEventType.ToString(), GetPointEventType);
		}

		void GetPointEventType(IMessage msg){
			GameObject pobj = msg.Data as GameObject;

			MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmGameObject(FsmValName.Value).Value = pobj;
            MessageObject.Value.GetComponent<PlayMakerFSM>().SendEvent(FsmEventName.Value);
		}
	}

}
