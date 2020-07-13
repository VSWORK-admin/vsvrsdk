using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{


	[ActionCategory("VRActions")]
	public class SendSystemWsMessages : FsmStateAction
	{
		public FsmEvent Recieve;
        public FsmString id;
        public FsmString name;
        public FsmString kind;
        public FsmString changenum;
		public FsmString a;
		public FsmString b;
		public FsmString c;
		public FsmString d;
		public FsmString e;

		// Code that runs on entering the state.
		public override void OnEnter()
		{
			WsChangeInfo wsinfo = new WsChangeInfo(){
				id = id.Value,
				name = name.Value,
				kind = kind.Value,
				changenum = changenum.Value,
				a = a.Value,
				b = b.Value,
				c = c.Value,
				d = d.Value,
				e = e.Value,
			};

			MessageDispatcher.SendMessage(this, WsMessageType.SendChangeObj.ToString(), wsinfo, 0);
			Finish();	
		}

		
	}

}
