using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("VRActions")]
	public class SendCustomLocalMessage : FsmStateAction
	{
		public FsmString CustomMessage;
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			MessageDispatcher.SendMessage(this, VrDispMessageType.CustomLocalMessage.ToString(), CustomMessage.Value, 0);
			Finish();
		}
	}

}
