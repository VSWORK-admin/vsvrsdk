// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Assign path to NavMesh Agent. Uses FsmNavMeshPath component. \nNOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class AgentSetPath : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("The Game Object holding the path. NOTE: The Game Object must have a FsmNavMeshPath component attached.")]
		[CheckForComponent(typeof(FsmNavMeshPath))]
		public FsmOwnerDefault path;
		
		[Tooltip("True if succesfully assigned.")]
		public FsmBool pathAssigned;
		
		[Tooltip("Trigger event if path assigned.")]
		public FsmEvent pathAssignedEvent;

		[Tooltip("Trigger event if path not assigned.")]
		public FsmEvent pathNotAssignedEvent;			

		private UnityEngine.AI.NavMeshAgent _agent;
		private FsmNavMeshPath _pathProxy;
		
		private void _getAgent()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_agent =  go.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}	

		private void _getPathProxy()
		{
			GameObject go = path.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : path.GameObject.Value;
			if (go == null) 
			{
				return;
			}
			
			_pathProxy =  go.GetComponent<FsmNavMeshPath>();
		}	
		
		public override void Reset()
		{
			gameObject = null;
			path = null;

		}

		public override void OnEnter()
		{
			_getAgent();
			_getPathProxy();
			
			DoSetPath();

			Finish();		
		}
		
		void DoSetPath()
		{
			if (_pathProxy == null || _agent == null) 
			{
				return;
			}
			
			
			bool _ok = _agent.SetPath(_pathProxy.path);
			
			pathAssigned.Value = _ok;
			
			if (_ok)
			{
				if ( ! FsmEvent.IsNullOrEmpty(pathAssignedEvent) ){
					Fsm.Event(pathAssignedEvent);
				}
			}else
			{
				if (! FsmEvent.IsNullOrEmpty(pathNotAssignedEvent) ){
					Fsm.Event(pathNotAssignedEvent);
				}
			}
		}

	}
}