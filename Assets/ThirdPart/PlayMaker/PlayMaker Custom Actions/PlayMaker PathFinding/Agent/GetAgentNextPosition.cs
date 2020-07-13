// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the next position on the current path of a NavMesh Agent. \n" +
		"NOTE: The Game Object must have a NavMeshAgent.")]
	public class GetAgentNextPosition : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The next position on the current path of the navMesh Agent.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 nextPosition;

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
			nextPosition = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoGetNextPosition();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
			DoGetNextPosition();
		}

		void DoGetNextPosition()
		{

			if (_agent == null) 
			{
				return;
			}
			
			if (nextPosition!=null){
				nextPosition.Value = _agent.nextPosition;
			}		
		}

	}
}