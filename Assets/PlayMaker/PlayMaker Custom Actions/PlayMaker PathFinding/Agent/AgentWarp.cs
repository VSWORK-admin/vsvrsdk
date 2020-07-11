// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Warps agent to the provided position. Send events base on result: Returns true if successful, otherwise returns false \n" +
	         "NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class AgentWarp : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;

		[Tooltip("New position to warp the agent to.")]
		public FsmVector3 newPosition;

		[ActionSection("Result")]

		[Tooltip("True if successful, otherwise returns false")]
		[UIHint(UIHint.Variable)]
		public FsmBool success;

		[Tooltip("Trigger this event Warp to new position is successful")]
		public FsmEvent successEvent;
		
		[Tooltip("Trigger this event when Warp to new position failed")]
		public FsmEvent failureEvent;

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
			newPosition = null;
			success = null;
			successEvent= null;
			failureEvent = null;
		}
		
		public override void OnEnter()
		{
			_getAgent();
			
			DoWarp();
			
			Finish();		
		}
		
		void DoWarp()
		{
			if (_agent == null) 
			{
				return;
			}
			
			bool ok =_agent.Warp(newPosition.Value);
			success.Value = ok;
			if (ok)
			{
			 Fsm.Event(successEvent);
			}else{
				Fsm.Event(failureEvent);
			}

		}
		
	}
}