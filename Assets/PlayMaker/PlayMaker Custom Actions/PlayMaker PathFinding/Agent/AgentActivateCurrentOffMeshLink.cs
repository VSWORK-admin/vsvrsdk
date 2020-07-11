// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Enables or disables the current link of a NavMesh Agent. \nNOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class AgentActivateCurrentOffMeshLink : FsmStateAction
	{

		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("Flag to Enables or disables the current link of the NavMesh Agent.")]
		[UIHint(UIHint.FsmFloat)]
		public FsmBool activate;

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
			activate = null;

		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoActivateCurrentOffMeshLink();

			Finish();		
		}
		
		void DoActivateCurrentOffMeshLink()
		{
			if (activate == null || _agent == null) 
			{
				return;
			}
			
			_agent.ActivateCurrentOffMeshLink(activate.Value);
		}

	}
}