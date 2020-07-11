// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the desired velocity of the agent including any potential contribution from avoidance. \n" +
	         "NOTE: The Game Object must have a NavMeshAgent.")]
	public class GetAgentDesiredVelocity : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("the relative vertical displacement of the owning GameObject")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 desiredVelocity;

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
			desiredVelocity = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			_getAgent();
			
			DoGetDesiredVelocity();
			
			if (!everyFrame)
				Finish();		
		}
		
		public override void OnUpdate()
		{
			DoGetDesiredVelocity();
		}
		
		void DoGetDesiredVelocity()
		{
			
			if (_agent == null) 
			{
				return;
			}
	
			desiredVelocity.Value = _agent.desiredVelocity;
					
		}
		
	}
}