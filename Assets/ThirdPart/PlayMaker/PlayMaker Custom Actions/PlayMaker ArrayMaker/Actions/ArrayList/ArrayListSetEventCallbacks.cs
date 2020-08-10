//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Set the event callbacks for a PlayMaker array List component")]
	public class ArrayListSetEventCallbacks : ArrayListActions
	{
		
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

        [Tooltip("The enable events property")]
        public FsmBool EnableEvents;


		[Tooltip("The add event")]
		public FsmString addEvent;

        [Tooltip("The add event")]
        public FsmString setEvent;

        [Tooltip("The remove event")]
        public FsmString removeEvent;


        public override void Reset()
		{
			gameObject = null;
			reference = null;

            addEvent = new FsmString() { UseVariable = true };
            setEvent = new FsmString() { UseVariable = true };
            removeEvent = new FsmString() { UseVariable = true };
        }
		
        public override void OnEnter()
		{
            if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
            {
                execute();
            }
				Finish();
			
		}
		
	
		public void execute()
		{
			
			if (! isProxyValid() ) return;

            proxy.enablePlayMakerEvents = EnableEvents.Value;

          if (!addEvent.IsNone)
          {
            proxy.addEvent = addEvent.Value;
          }

            if (!setEvent.IsNone)
            {
                proxy.setEvent = setEvent.Value;
            }

            if (!removeEvent.IsNone)
            {
                proxy.removeEvent = removeEvent.Value;
            }
        }
		
		
	}
}