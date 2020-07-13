using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{


	[ActionCategory("VRActions")]
	public class SendCustomWsMessages : FsmStateAction
	{
		public FsmString a;
		public FsmString b;
		public FsmString c;
		public FsmString d;
		public FsmString e;
		public FsmString f;
		public FsmString g;
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			WsCChangeInfo wsinfo = new WsCChangeInfo(){
				a = a.Value,
				b = b.Value,
				c = c.Value,
				d = d.Value,
				e = e.Value,
				f = f.Value,
				g = g.Value,
			};

			MessageDispatcher.SendMessage(this, WsMessageType.SendCChangeObj.ToString(), wsinfo, 0);
			Finish();	
		}

		
	}

}
