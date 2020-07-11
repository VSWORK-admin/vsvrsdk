// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the current steering target - usually the next corner or end point of the current path. (Read Only) \n" +
	         "NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class GetAgentSteeringTarget : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Store the steering target.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeResult;

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
			storeResult = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			_getAgent();
			
			DoGetSteeringTarget();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoGetSteeringTarget();
		}
		
		
		void DoGetSteeringTarget()
		{
			if (storeResult == null || _agent == null) 
			{
				return;
			}
			
			storeResult.Value = _agent.steeringTarget;		
		}
		
	}
}