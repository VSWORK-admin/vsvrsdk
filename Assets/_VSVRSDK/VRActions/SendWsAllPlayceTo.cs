using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{


	[ActionCategory("VRActions")]
	public class SendWsAllPlayceTo : FsmStateAction
	{
		public FsmString GroupName;
		public FsmBool ToAll;
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			MessageDispatcher.SendMessage(ToAll.Value, VrDispMessageType.AllPlaceTo.ToString(), GroupName.Value, 0);
			Finish();
		}

		
	}

}
