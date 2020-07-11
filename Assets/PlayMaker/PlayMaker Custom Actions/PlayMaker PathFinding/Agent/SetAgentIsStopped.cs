// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

#if UNITY_5_5_OR_NEWER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Sets the stop or resume condition of the NavMesh agent. If set to True, the NavMesh agent's movement will be stopped along its current path. If set to False after the NavMesh agent has stopped, it will resume moving along its current path.")]
	public class SetAgentIsStopped : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The stop or resume condition of the NavMesh agent")]
		public FsmBool isStopped;

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
			isStopped = null;

		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoSetIsStopped();

			Finish();		
		}
		
		void DoSetIsStopped()
		{
			if (isStopped == null || _agent == null) 
			{
				return;
			}
			
			_agent.isStopped = isStopped.Value;
		}

	}
}

#endif