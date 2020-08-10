using UnityEngine;
using System.Collections;
using System.Linq;
using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Sorts the sequence of gameobjects by names in a PlayMaker ArrayList Proxy component")]
	public class ArrayListSortGameObjects : ArrayListActions
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
		}
		
		public override void OnEnter()
		{
			if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				DoArrayListSort();
			
			Finish();
		}

		
		public void DoArrayListSort()
		{
			if (! isProxyValid()) 
				return;
				

			IComparer myComparer = new myGameObjectSorter();

			GameObject[] _temp = (GameObject[]) proxy.arrayList.ToArray (typeof(GameObject));

			Array.Sort(_temp, myComparer);

			proxy.arrayList.Clear();

			proxy.arrayList.AddRange(_temp);	

		}

		public class myGameObjectSorter : IComparer  {
			
			// Calls CaseInsensitiveComparer.Compare on the monster name string.
			int IComparer.Compare( System.Object x, System.Object y )  {
				return( (new CaseInsensitiveComparer()).Compare( ((GameObject)x).name, ((GameObject)y).name) );
			}
			
		}

	}
}