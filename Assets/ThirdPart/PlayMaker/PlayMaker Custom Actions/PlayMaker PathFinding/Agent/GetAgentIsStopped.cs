// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

#if UNITY_5_5_OR_NEWER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the stop or resume condition of the NavMesh agent. The Game Object must have a NavMeshAgent component attached.")]
	public class GetAgentIsStopped : FsmStateAction
	{
		
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("Store the agent isStopped condition.")]
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

			DoGetIsPathStale();
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetIsPathStale();
		}

		void DoGetIsPathStale()
		{
			if (storeResult == null || _agent == null) 
			{
				return;
			}

			storeResult.Value = _agent.isStopped;
		}

	}
}

#endif