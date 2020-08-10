//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Intersect two arrayList proxy components.")]
	public class ArrayListIntersect : ArrayListActions
	{
		[ActionSection("Storage")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to store the concatenation ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;
		
		[ActionSection("ArrayLists to intersect")]
		
		[CompoundArray("ArrayLists", "ArrayList GameObject", "Reference")]
		
		[RequiredField]
		[Tooltip("The GameObject with the PlayMaker ArrayList Proxy component to copy to")]
		[ObjectType(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault arrayListGameObjectA;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to copy to ( necessary if several component coexists on the same GameObject")]
		public FsmString referenceTargetA;	

		[RequiredField]
		[Tooltip("The GameObject with the PlayMaker ArrayList Proxy component to copy to")]
		[ObjectType(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault arrayListGameObjectB;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to copy to ( necessary if several component coexists on the same GameObject")]
		public FsmString referenceTargetB;

		ArrayList	ArrayList_A = new ArrayList();
		ArrayList	ArrayList_B = new ArrayList();

		public override void Reset()
		{
			gameObject = null;
			reference = null;

		}

		
		public override void OnEnter()
		{

			if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
					DoArrayListConcat(proxy.arrayList);
			
			Finish();
		}

		
		public void DoArrayListConcat(ArrayList source)
		{
			if (! isProxyValid()) 
				return;
			

			ArrayList_A.Clear ();
			ArrayList_B.Clear ();
	
				if ( SetUpArrayListProxyPointer( Fsm.GetOwnerDefaultTarget(arrayListGameObjectA),referenceTargetA.Value) ){
						if (isProxyValid()){
							ArrayList_A = this.proxy._arrayList;	
					}
				}
			if ( SetUpArrayListProxyPointer( Fsm.GetOwnerDefaultTarget(arrayListGameObjectB),referenceTargetB.Value) ){
				if (isProxyValid()){
					ArrayList_B = this.proxy._arrayList;	
				}
			}

			source.Clear ();
			foreach (var _obj in ArrayList_A)
			{
				if (ArrayList_B.Contains(_obj))
					source.Add(_obj);
			}
				
		}
	}
}