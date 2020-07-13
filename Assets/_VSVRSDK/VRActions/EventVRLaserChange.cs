using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("VRActions")]
	public class EventVRLaserChange : FsmStateAction
	{
		public VRPointObjEventType PointEventType;
		public FsmEvent PointEvent;
		public FsmGameObject PointedObj;
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
			PointedObj.Value = pobj;
			Fsm.Event(PointEvent);
		}
	}

}
