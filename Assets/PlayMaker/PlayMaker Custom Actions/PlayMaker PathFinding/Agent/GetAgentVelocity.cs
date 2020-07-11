// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the velocity of a NavMesh Agent and stores it in a float Variable ( the magnitude) or in a vector3 directly. \n" +
		"NOTE: The Game Object must have a NavMeshAgent.")]
	public class GetAgentVelocity : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
	
		[Tooltip("The current velocity of the navMesh Agent.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 velocity;	
		
		[Tooltip("The current velocity magnitude  of the navMesh Agent.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat velocityMagnitude;
		
		[Tooltip("The current squared velocity magnitude of the navMesh Agent. NOTE: Faster than getting the actual magnitude")]
		[UIHint(UIHint.Variable)]
		public FsmFloat velocitySqrMagnitude;	
		
		// we could also design something that gives the velocity ranging from 0 to 1, 1 being the maximum speed of the agent, 
		// which could give us good starting points for switching animation or know easily when an agent is at full speed.

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
		
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
			velocity = null;
			velocityMagnitude = null;
			velocitySqrMagnitude = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoGetVelocity();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			DoGetVelocity();
		}

		void DoGetVelocity()
		{

			if (_agent == null) 
			{
				return;
			}
			
			if (velocity!=null){
				velocity.Value = _agent.velocity;
			}
			
			if (velocityMagnitude !=null){
				velocityMagnitude.Value = _agent.velocity.magnitude;
			}
			
			if (velocitySqrMagnitude !=null){
				velocitySqrMagnitude.Value = _agent.velocity.sqrMagnitude;
			}			
		}

	}
}