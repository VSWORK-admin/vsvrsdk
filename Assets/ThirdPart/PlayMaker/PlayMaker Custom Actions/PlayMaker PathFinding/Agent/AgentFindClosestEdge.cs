// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Locate the closest NavMesh edge. \nYou can dispatch events if nearest edge found or not. \nYou can then store information about the location (navMeshHit). \nNOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class AgentFindClosestEdge : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[ActionSection("Result")]
		
		[Tooltip("True if a nearest edge is found.")]
		public FsmBool nearestEdgeFound;
		
		[Tooltip("Trigger event if a nearest edge is found.")]
		public FsmEvent nearestEdgeFoundEvent;

		[Tooltip("Trigger event if a nearest edge is NOT found.")]
		public FsmEvent nearestEdgeNotFoundEvent;
		
		
		[ActionSection("Hit information (of the found edge)")]
		
		[Tooltip("Position of hit")]
		public FsmVector3 position;
		
		[Tooltip("Normal at the point of hit")]
		public FsmVector3 normal;
		
		[Tooltip("Distance to the point of hit")]
		public FsmFloat distance;

		[Tooltip("Mask specifying NavMeshLayers at point of hit.")]
		public FsmInt mask;

		[Tooltip("Flag when hit")]
		public FsmBool hit;
		
		private UnityEngine.AI.NavMeshAgent _agent;
		
		private void _getAgent()
		{
			GameObject go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;
			if (go == null) 
			{
				return;
			}
			
			_agent =  go.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}	
		
		public override void Reset()
		{
			gameObject = null;
			
			nearestEdgeFound = new FsmBool { UseVariable = true };
			nearestEdgeFoundEvent = null;
			nearestEdgeNotFoundEvent = null;
			
			position = new FsmVector3 { UseVariable = true};
			normal = new FsmVector3 { UseVariable = true};
			distance = new FsmFloat { UseVariable = true};
			mask = new FsmInt { UseVariable = true};
			hit = new FsmBool { UseVariable = true};
			
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoFindClosestEdge();

			Finish();		
		}
		
		void DoFindClosestEdge()
		{
			if (_agent == null) 
			{
				return;
			}
			
			UnityEngine.AI.NavMeshHit _NavMeshHit;
			
			bool _nearestEdgeFound = _agent.FindClosestEdge(out _NavMeshHit);
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