//	(c) Jean Fabre, 2011-2019 All rights reserved.
//	http://www.fabrejean.net

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Gets a Random item weighted along a curve in PlayMaker ArrayList Proxy component")]
	public class ArrayListGetRandomCurvedWeighted : ArrayListActions
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;		

		[Tooltip("The weight Curve")]
		public FsmAnimationCurve weightCurve;
		
		[Tooltip("Can return the same item twice in a row.")]
		public FsmBool repeat;
		
		[ActionSection("Result")]
		
		[Tooltip("The random item data picked from the array")]
		[UIHint(UIHint.Variable)]
		public FsmVar randomItem;
		
		[Tooltip("The random item index picked from the array")]
		[UIHint(UIHint.Variable)]
		public FsmInt randomIndex;
		
		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		public FsmEvent failureEvent;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
		
		float[] _weights;
		
		public override void Reset()
		{
			gameObject = null;
			weightCurve = new FsmAnimationCurve();
			
			failureEvent = null;
			
			randomItem = null;
			randomIndex = null;
		}



		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				_weights = GetWeightsFromCurve(weightCurve.curve, proxy.arrayList.Count);
				GetRandomItem();
			}

			if(!everyFrame) Finish();

		}

		public override void OnUpdate()
		{
			GetRandomItem();

		}



		public void GetRandomItem()
		{

			if (! isProxyValid())
			{
				return;
			}
			
			int index = GetRandomWeightedIndex(_weights);


			object element = null;
			
			try{

				element = proxy.arrayList[index];
			}catch(System.Exception e){
				Debug.LogWarning(e.Message);
				Fsm.Event(failureEvent);
				return;
			}
			
			randomIndex.Value = index;
			
			bool ok = PlayMakerUtils.ApplyValueToFsmVar(Fsm,randomItem,element);
			
			if (!ok)
			{
				Debug.LogWarning("ApplyValueToFsmVar failed");
				Fsm.Event(failureEvent);
				return;
			}
		}
		
		public float[] GetWeightsFromCurve(AnimationCurve curve, int sampleCount)
		{
			float _totalTime = curve.keys[curve.keys.Length - 1].time;

			sampleCount = Mathf.Max(2, sampleCount);
			
			float[] weights = new float[sampleCount];
			
			float _interval = _totalTime / (sampleCount - 1);
			
			
			for (int i = 0; i < sampleCount; i++)
			{
				weights[i] = Mathf.Max(0,curve.Evaluate(_interval * i));
			}

			return weights;
		}
		
		/// <summary>
		/// Given an array of weights, returns a randomly selected index. 
		/// </summary>
		public int GetRandomWeightedIndex(float[] weights)
		{
			float totalWeight = 0;

			foreach (var t in weights)
			{
				totalWeight += t;
			}

			var random = Random.Range(0, totalWeight);

			for (var i = 0; i < weights.Length; i++)
			{
				if (random < weights[i])
				{
					return i;
				}

				random -= weights[i];
			}

			return -1;
		}
	}
}