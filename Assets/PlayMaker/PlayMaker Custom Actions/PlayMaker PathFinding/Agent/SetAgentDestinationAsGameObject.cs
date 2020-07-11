// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Set the destination of a NavMesh Agent to a gameObject. \nNOTE: The Game Object must have a NavMeshAgentcomponent attached.")]
	public class SetAgentDestinationAsGameObject : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("Set the destination to navigate towards.")]
		public FsmGameObject destination;
		
		[RequiredField]
		[Tooltip("Define the gameObject minimum movement distance to retrigger path computation.")]
		public FsmFloat triggerThreshold;
		
		private UnityEngine.AI.NavMeshAgent _agent;
		
		private Vector3 lastPosition;
		
		private float lastDistance;
		
		private float squareDistance;

		GameObject _lastGo;

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
			destination = null;
			triggerThreshold = 1;
			
		}

		public override void OnEnter()
		{
			// force refresh
			_lastGo = null;

			_getAgent();
			
			DoSetDestination();

		}

		public override void OnUpdate()
		{
			DoSetDestination();
		}

		void DoSetDestination()
		{
			if (destination.Value == null || _agent == null) 
			{
				return;
			}

			bool forceUpdate = false;
			if (_lastGo != destination.Value)
			{
				forceUpdate = true;
				_lastGo = destination.Value;
			}

			if (lastDistance!= triggerThreshold.Value){
				lastDistance = triggerThreshold.Value;
				squareDistance = lastDistance*lastDistance;
			}
		
			Vector3 newPosition = destination.Value.transform.position;
			Vector3 deltaPosition = newPosition-lastPosition;
			if (deltaPosition.sqrMagnitude>squareDistance || forceUpdate)
			{
				_agent.SetDestination(newPosition);
				lastPosition = destination.Value.transform.position;
			}

		}
	

	}
}