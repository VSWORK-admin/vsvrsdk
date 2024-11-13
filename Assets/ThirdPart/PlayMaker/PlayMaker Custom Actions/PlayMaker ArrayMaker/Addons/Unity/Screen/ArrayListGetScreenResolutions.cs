//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable
using System;
using System.Linq;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Store all resolutions, Also remove duplicates and sort them by width then height then refresh rate")]
	public class ArrayListGetScreenResolutions : ArrayListActions
	{
	
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("If true, output will include refreshrates, else only width and height")]
		public bool includeRefreshRate;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;
		
		public override void Reset()
		{
			gameObject = null;
			reference = null;
			includeRefreshRate = true;
		}

		
		public override void OnEnter()
		{
			if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				getResolutions();
			
			Finish();
		}

		
		public void getResolutions()
		{
			if (! isProxyValid()) 
				return;
			
			proxy.arrayList.Clear();
			
			if (includeRefreshRate)
			{
				Resolution[] resolutions = Screen.resolutions.Distinct().OrderBy(x => x.width).ThenBy(x => x.height).ThenBy(x => x.refreshRate).ToArray();
				foreach (Resolution res in resolutions)
				{
					proxy.arrayList.Add(new Vector3(res.width, res.height, res.refreshRate));
				}
			}else
			{
				Resolution[] resolutions = Screen.resolutions.Distinct().OrderBy(x => x.width).ThenBy(x => x.height).ToArray();
				foreach (Resolution res in resolutions)
				{
					proxy.arrayList.Add(new Vector2(res.width, res.height));
				}
			}

		}
	}
}