// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Apply relative movement to current path position of a navMesh Agent. \nIf the agent has a path it will be adjusted. \nNOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class AgentMove : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The relative move vector.")]
		public FsmVector3 offset;
		
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
			offset = null;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoMove();

			Finish();		
		}
		
		void DoMove()
		{
			if (_agent == null) 
			{
				return;
			}
			
			_agent.Move(offset.Value);
		}

	}
}