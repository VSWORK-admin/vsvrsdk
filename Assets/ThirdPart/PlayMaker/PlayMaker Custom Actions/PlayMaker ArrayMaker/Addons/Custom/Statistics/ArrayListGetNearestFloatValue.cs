//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Return the average value within an arrayList.")]
	public class ArrayListGetNearestFloatValue : ArrayListActions
	{

		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("The target Value")]
		public FsmFloat floatValue;

		[Tooltip("Performs every frame. WARNING, it could be affecting performances.")]
		public bool everyframe;
		
		[ActionSection("Result")]
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The index of the nearest Value")]
		public FsmInt nearestIndex;

		[UIHint(UIHint.Variable)]
		[Tooltip("The nearest Value")]
		public FsmFloat nearestValue;
		
		List<float> _floats;
		
		public override void Reset()
		{
			
			gameObject = null;
			reference = null;
			
			floatValue = null;

			nearestIndex = null;
			nearestValue = null;

			everyframe = true;
		}
		
		
		public override void OnEnter()
		{
			
			if (! SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
			{
				Finish();
			}
			
			DoGetNearestValue();
			
			if (!everyframe)
			{
				Finish();
			}
			
		}
		
		public override void OnUpdate()
		{
			DoGetNearestValue();
		}
		
		void DoGetNearestValue()
		{
			
			if (! isProxyValid())
			{
				return;
			}
			
			_floats = new List<float>();
			
			foreach(object _obj in proxy.arrayList)
			{
				try{
					_floats.Add( System.Convert.ToSingle(_obj) );
				}finally
				{
					
				}
				
			}

			float _target = floatValue.Value;

			if (_floats.Count>0)
			{
				var nearest = float.MaxValue;
				var minDifference = float.MaxValue;
				int _nearestIndex = 0;
				int i = 0;
				foreach (float element in _floats)
				{
					float difference = Mathf.Abs(element - _target);
					if (minDifference > difference)
					{
						minDifference = (float)difference;
						nearest = element;
						_nearestIndex = i;
					}

					i++;
				}

				nearestIndex.Value = _nearestIndex;

				nearestValue.Value = nearest;
			}
		}
		
	}
}