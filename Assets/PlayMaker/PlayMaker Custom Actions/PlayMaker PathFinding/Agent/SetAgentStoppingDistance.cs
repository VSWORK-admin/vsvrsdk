// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Sets the Stopping distance from the destination position of a NavMesh Agent. " +
		"\nNOTE: The Game Object must have a NavMeshAgentcomponent attached.")]
	public class SetAgentStoppingDistance : FsmStateAction
	{

		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The Stopping distance from the destination position")]
		public FsmFloat stoppingDistance;
		
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
			stoppingDistance = null;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoSetStoppingDistance();
			
			Finish();		
		}
		

		void DoSetStoppingDistance()
		{
			if (stoppingDistance == null || _agent == null)
			{
				return;
			}
			
			_agent.stoppingDistance = stoppingDistance.Value;
		}

	}
}