using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("VRActions")]
	public class SendCommitOrder : FsmStateAction
	{
		public FsmString OrderString;
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			MessageDispatcher.SendMessage(this, VrDispMessageType.CommitOrder.ToString(), OrderString.Value, 0);
			Finish();
		}
	}

}
