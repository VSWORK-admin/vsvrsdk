// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the path status or an Agent and dispatch events. \n" +
		"NOTE: The Game Object must have a NavMeshAgentcomponent attached.")]
	public class GetAgentPathStatus : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The path terminates at the destination.")]
		public FsmEvent pathCompleteEvent; // pathStatus
		
		[Tooltip("The path cannot reach the destination.")]
		public FsmEvent pathPartialEvent; // pathStatus
		
		[Tooltip("The path is invalid.")]
		public FsmEvent pathInvalidEvent; // pathStatus
		
		[Tooltip("Path is being computed, but not yet ready. ")]
		public FsmEvent pathPendingEvent; // pathPending flag
		
		[Tooltip("The agent does not currently have a path.")]
		public FsmEvent pathUndefinedEvent; // hasPath flag

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
			pathCompleteEvent = null;
			pathPartialEvent = null;
			pathInvalidEvent = null;
			pathPendingEvent = null;
			pathUndefinedEvent= null;
			
			everyFrame = false;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoGetPathStatus();
			
			if (!everyFrame)
			{
				Finish();
			}	
		}

		public override void OnUpdate()
		{
			DoGetPathStatus();
		}

		void DoGetPathStatus()
		{
			if (_agent ==null)
			{
				return;
			}
			
			if (! _agent.hasPath && pathUndefinedEvent!=null){
				Fsm.Event(pathUndefinedEvent);
				Finish();
			}
			
			if (! _agent.pathPending && pathPendingEvent!=null){
				Fsm.Event(pathPendingEvent);
				Finish();
			}
			
			if(_agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && pathCompleteEvent!=null){
				Fsm.Event(pathCompleteEvent);
				Finish();
			}
			
			if(_agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial && pathPartialEvent!=null){
				Fsm.Event(pathPartialEvent);
				Finish();
			}
			
			if(_agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid && pathInvalidEvent!=null){
				Fsm.Event(pathInvalidEvent);
				Finish();
			}
					
		}

	}
}