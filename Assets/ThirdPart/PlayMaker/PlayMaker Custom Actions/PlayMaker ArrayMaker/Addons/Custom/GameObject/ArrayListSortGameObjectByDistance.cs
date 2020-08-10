//	(c) Jean Fabre, 2011-2015 All rights reserved.
//	http://www.fabrejean.net

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable


// sorting algo based on http://answers.unity3d.com/questions/246781/sort-transforms-by-distance-to-player.html


using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Sort GameObjects within an arrayList based on the distance of a transform or position.")]
	public class ArrayListSortGameObjectByDistance : ArrayListActions
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;
		
		[Tooltip("Compare the distance of the items in the list to the position of this gameObject")]
		public FsmGameObject distanceFrom;
		
		[Tooltip("If DistanceFrom declared, use OrDistanceFromVector3 as an offset")]
		public FsmVector3 orDistanceFromVector3;
		
		public bool everyframe;
		
		
		public override void Reset()
		{
		
			gameObject = null;
			reference = null;
			distanceFrom = null;
			orDistanceFromVector3 = null;

			everyframe = true;
		}
		
		
		public override void OnEnter()
		{

			if (! SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
			{
				Finish();
			}
			
			DoSortByDistance();
			
			if (!everyframe)
			{
				Finish();
			}
			
		}
		
		public override void OnUpdate()
		{
			DoSortByDistance();
		}
		
		void DoSortByDistance()
		{
			
			if (! isProxyValid())
			{
				return;
			}
			
			Vector3 root = orDistanceFromVector3.Value;
			
			GameObject _rootGo = distanceFrom.Value;
			if (_rootGo!=null)
			{
				root += _rootGo.transform.position;
			}
			
			// bubble-sort transforms
			for ( int e = 0; e < proxy.arrayList.Count - 1; e ++ )
			{
				GameObject _item_e0 = (GameObject)proxy.arrayList[e + 0];
				GameObject _item_e1 = (GameObject)proxy.arrayList[e + 1];

				float sqrMag1  = ( _item_e0.transform.position - root ).sqrMagnitude;
				float sqrMag2 = ( _item_e1.transform.position - root ).sqrMagnitude;
				
				if ( sqrMag2 < sqrMag1 )
				{
					GameObject tempStore = (GameObject)proxy.arrayList[e];
					proxy.arrayList[e] = proxy.arrayList[e + 1];
					proxy.arrayList[e + 1] = tempStore;
					e = 0;
				}
			}

		}

	}
}