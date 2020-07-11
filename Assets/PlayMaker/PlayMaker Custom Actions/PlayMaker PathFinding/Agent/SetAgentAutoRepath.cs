// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Set the flag to attempt to acquire a new path \n" +
		"if the existing path of a NavMesh Agent becomes invalid \n" +
		"or if the agent reaches the end of a partial and stale path. \n" +
		"NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class SetAgentAutoRepath : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("Flag to attempt to acquire a new path if the existing path of a NavMesh Agent becomes invalid or if the agent reaches the end of a partial and stale path.")]
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
			
			DoSetAutoRepath();

			Finish();		
		}
		
		void DoSetAutoRepath()
		{
			if (isStopped == null || _agent == null) 
			{
				return;
			}
			
			_agent.autoRepath = isStopped.Value;
		}

	}
}