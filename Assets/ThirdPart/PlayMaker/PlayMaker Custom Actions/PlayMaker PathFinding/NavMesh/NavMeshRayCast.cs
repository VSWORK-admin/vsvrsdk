// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.



namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMesh)]
	[Tooltip("Trace a ray between two points on the NavMesh. \n" +
		"You can dispatch events If terminated before reaching the target position or not. \nYou can then store information about the location (navMeshHit). \n" +
		"NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class NavMeshRaycast : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The origin of the ray.")]
		public FsmVector3 sourcePosition;
		
		[RequiredField]
		[Tooltip("The end of the ray.")]
		public FsmVector3 targetPosition;
		
		[RequiredField]
		[Tooltip("The mask specifying which NavMesh layers can be passed when tracing the ray.")]
		public FsmInt passableMask;

		
		[ActionSection("Result")]
		
		[Tooltip("true If terminated before reaching target position.")]
		[UIHint(UIHint.Variable)]
		public FsmBool reachedBeforeTargetPosition;
		
		[Tooltip("Trigger event if sample reached before the target position.")]
		public FsmEvent reachedBeforeTargetPositionEvent;

		[Tooltip("Trigger event if sample reached after the target position.")]
		public FsmEvent reachedAfterTargetPositionEvent;
		
		
		[ActionSection("Hit information of the sample")]
		
		[Tooltip("Position of hit")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 position;
		
		[Tooltip("Normal at the point of hit")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 normal;
		
		[Tooltip("Distance to the point of hit")]
		[UIHint(UIHint.Variable)]
		public FsmFloat distance;

		[Tooltip("Mask specifying NavMeshLayers at point of hit.")]
		[UIHint(UIHint.Variable)]
		public FsmInt mask;

		[Tooltip("Flag when hit")]
		[UIHint(UIHint.Variable)]
		public FsmBool hit;
			
		
		public override void Reset()
		{
			sourcePosition = null;
			targetPosition = null;
			
			passableMask = -1; // so that by default mask is "everything"
			
			reachedBeforeTargetPosition  = null;
			reachedBeforeTargetPositionEvent = null;
			reachedAfterTargetPositionEvent = null;
			
			position = null;
			normal = null;
			distance = null;
			mask = null;
			hit = null;
			
		}

		public override void OnEnter()
		{
			DoRaycast();

			Finish();		
		}
		
		void DoRaycast()
		{
			UnityEngine.AI.NavMeshHit _NavMeshHit;
			bool _reachedBeforeTargetPosition = UnityEngine.AI.NavMesh.Raycast(sourcePosition.Value,targetPosition.Value,out _NavMeshHit,passableMask.Value);
		 	reachedBeforeTargetPosition.Value = _reachedBeforeTargetPosition;
			
			position.Value = _NavMeshHit.position;
			normal.Value = _NavMeshHit.normal;
			distance.Value = _NavMeshHit.distance;
			mask.Value = _NavMeshHit.mask;
			hit.Value = _NavMeshHit.hit;
			
			if (_reachedBeforeTargetPosition)
			{
				if ( ! FsmEvent.IsNullOrEmpty(reachedBeforeTargetPositionEvent) ){
					Fsm.Event(reachedBeforeTargetPositionEvent);
				}
			}else
			{
				if (! FsmEvent.IsNullOrEmpty(reachedAfterTargetPositionEvent) ){
					Fsm.Event(reachedAfterTargetPositionEvent);
				}
			}
			
			
		}
		

	}
}