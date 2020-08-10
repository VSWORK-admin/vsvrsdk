// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// Original Action by DudeBxl 
// Keywords: Get All Child Of GameObject

using UnityEngine;
using System.Collections;
using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Store all (all levels) childs of GameObject (active and/or inactive) from a parent.")]
	public class ArrayListGetAllChildOfGameObject : ArrayListActions
	{
		
		[ActionSection("Array Setup")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		public FsmString reference;
		
		[ActionSection("Setup")]
		[Tooltip("The parent gameObject")]
		[RequiredField]
		public FsmGameObject parent;
		
		[ActionSection("Option")]
		public FsmBool includeInactive;

		public FsmBool includeParent;
		
		private GameObject go;
		private Transform[] childs;
		
		public override void Reset()
		{
			gameObject = null;
			reference = null;
			parent = null;
			includeInactive = true;
			includeParent = false;
		}
		
		
		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) ){
				getAllChilds(parent.Value);
			}

			Finish();
		}
		
		
		public void getAllChilds(GameObject parent)
		{
			if (! isProxyValid()) 
				return;

			childs = parent.GetComponentsInChildren<Transform>(includeInactive.Value);

			proxy.arrayList.Clear();

			foreach(Transform trans in childs) {
				if ( !includeParent.Value && trans.gameObject == parent)
				{
					continue;
				}

				proxy.arrayList.Add(trans.gameObject);
			}

		}
	}
}
