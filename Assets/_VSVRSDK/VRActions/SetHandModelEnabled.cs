using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{


	[ActionCategory("VRActions")]
	public class SetHandModelEnabled : FsmStateAction
	{
		public HandModelType tp;
		public FsmBool enabled;
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			MessageDispatcher.SendMessage(this, tp.ToString(), enabled.Value, 0);
			Finish();
		}

		
	}

}
