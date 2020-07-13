// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the maximum movement speed of a NavMesh Agent. \n" +
		"WARNING, this is the maximum speed possible for the agent, not the actual current speed ( use getAgentVelocity for this). \n" +
		"NOTE: The Game Object must have a NavMeshAgentcomponent attached.")]
	public class GetAgentMaximumSpeed : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Store the agent maximum movement speed")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;
		
		
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
			storeResult = null;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoGetSpeed();

			Finish();		
		}

		void DoGetSpeed()
		{
			if (storeResult == null || _agent==null)
			{
				return;
			}
			
			storeResult.Value = _agent.speed;			
		}

	}
}