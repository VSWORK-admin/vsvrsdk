using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("VRActions")]
	public class SendChangeAvatar : FsmStateAction
	{
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			MessageDispatcher.SendMessage(WsMessageType.SendChangeAvatar.ToString());
			Finish();
		}
	}

}
