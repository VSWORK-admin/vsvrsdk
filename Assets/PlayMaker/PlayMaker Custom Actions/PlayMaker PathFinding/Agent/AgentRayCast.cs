// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Trace movement towards a target postion in the NavMesh, without moving the agent. \n" +
		"You can dispatch events If terminated before reaching the target position or not. \n" +
		"You can then store information about the location (navMeshHit). \n" +
		"NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class AgentRayCast : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The desired end position of movement.")]
		[UIHint(UIHint.FsmVector3)]
		public FsmVector3 targetPosition;
		
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
		
		private UnityEngine.AI.NavMeshAgent _agent;
		
		private void _getAgent()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_agent =  go.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}	
		
		public override void Reset()
		{
			gameObject = null;
			
			targetPosition = null;
			
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
			_getAgent();
			
			DoRaycast();

			Finish();		
		}
		
		void DoRaycast()
		{
			if (_agent == null) 
			{
				return;
			}
			
			UnityEngine.AI.NavMeshHit _NavMeshHit;
			bool _reachedBeforeTargetPosition = _agent.Raycast(targetPosition.Value,out _NavMeshHit);
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