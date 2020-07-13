// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.



namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMesh)]
	[Tooltip("Locate the closest NavMesh edge from a point close to the NavMesh. \nYou can dispatch events If terminated before reaching the target position or not. \nYou can then store information about the location (navMeshHit).")]
	public class NavMeshFindClosestEdge : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The origin of the distance query.")]
		public FsmVector3 sourcePosition;
		
		[RequiredField]
		[Tooltip("The mask specifying which NavMesh layers can be passed when finding the nearest edge.")]
		public FsmInt passableMask;
		
		[ActionSection("Result")]
		
		[Tooltip("True if a nearest edge is found.")]
		[UIHint(UIHint.Variable)]
		public FsmBool nearestEdgeFound;
		
		[Tooltip("Trigger event if a nearest edge is found.")]
		public FsmEvent nearestEdgeFoundEvent;

		[Tooltip("Trigger event if a nearest edge is NOT found.")]
		public FsmEvent nearestEdgeNotFoundEvent;
		
		[ActionSection("Hit information (of the found edge)")]
		
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
			
			passableMask = -1; // so that by default mask is "everything"
			
			nearestEdgeFound = null;
			nearestEdgeFoundEvent = null;
			nearestEdgeNotFoundEvent = null;
			
			position = null;
			normal = null;
			distance = null;
			mask = null;
			hit = null;
			
		}

		public override void OnEnter()
		{
			DoGetDistancetoEdge();

			Finish();		
		}
		
		void DoGetDistancetoEdge()
		{
			UnityEngine.AI.NavMeshHit _NavMeshHit;
			bool _nearestEdgeFound = UnityEngine.AI.NavMesh.FindClosestEdge(sourcePosition.Value,out _NavMeshHit,passableMask.Value);
			nearestEdgeFound.Value = _nearestEdgeFound;
			
			position.Value = _NavMeshHit.position;
			normal.Value = _NavMeshHit.normal;
			distance.Value = _NavMeshHit.distance;
			mask.Value = _NavMeshHit.mask;
			hit.Value = _NavMeshHit.hit;
			
			if (_nearestEdgeFound)
			{
				if ( ! FsmEvent.IsNullOrEmpty(nearestEdgeFoundEvent) ){
					Fsm.Event(nearestEdgeFoundEvent);
				}
			}else
			{
				if (! FsmEvent.IsNullOrEmpty(nearestEdgeNotFoundEvent) ){
					Fsm.Event(nearestEdgeNotFoundEvent);
				}
			}
			
			
		}
		

	}
}