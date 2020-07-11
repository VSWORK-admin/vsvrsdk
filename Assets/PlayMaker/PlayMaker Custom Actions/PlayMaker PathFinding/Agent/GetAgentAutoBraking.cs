// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the flag of NavMesh Agent avoids overshooting the destination point. \n" +
	         "NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class GetAgentAutoBraking : FsmStateAction
	{
		
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("Store flag of the agent that avoids overshooting the destination point.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[Tooltip("Runs every frame.")]
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
			storeResult = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			_getAgent();
			
			DoGetAutoBraking();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoGetAutoBraking();
		}
		
		void DoGetAutoBraking()
		{
			if (storeResult == null || _agent == null) 
			{
				return;
			}
			
			storeResult.Value = _agent.autoBraking;
		}
		
	}
}