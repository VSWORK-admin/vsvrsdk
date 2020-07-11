// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Terminate current OffMeshLink of a NavMesh Agent. \nNOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class AgentCompleteOffMeshLink : FsmStateAction
	{

		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;

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

		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoCompleteOffMeshLink();

			Finish();		
		}
		
		void DoCompleteOffMeshLink()
		{
			if (_agent == null) 
			{
				return;
			}
			
			_agent.CompleteOffMeshLink();
		}

	}
}