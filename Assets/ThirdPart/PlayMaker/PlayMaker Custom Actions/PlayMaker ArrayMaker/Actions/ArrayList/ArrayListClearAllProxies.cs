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
	[Tooltip("Removes all elements from all PlayMaker ArrayList Proxies on a GameObject")]
	public class ArrayListClearAllProxies : ArrayListActions
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the multiple PlayMaker ArrayList Proxy components")]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		
		public override void OnEnter()
		{
            GameObject _go = Fsm.GetOwnerDefaultTarget(gameObject);

            foreach(PlayMakerArrayListProxy _p in  _go.GetComponents<PlayMakerArrayListProxy>())
            {
                _p.arrayList.Clear();
            }


            Finish();
		}

	}
}