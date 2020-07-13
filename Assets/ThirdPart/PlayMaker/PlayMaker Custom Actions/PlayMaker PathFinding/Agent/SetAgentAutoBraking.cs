// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Set the flag for the agent to brake automatically to avoid overshooting the destination point\n" +
	         "If the agent needs to land close to the destination point then it will typically need to brake to avoid overshooting or endless 'orbiting around the target zone." +
	         "If this property is set to true, the agent will brake automatically as it nears the destination." +
	         "NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class SetAgentAutoBraking : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("Flag to brake automatically to avoid overshooting the destination point")]
		public FsmBool autoBraking;
		
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
			autoBraking = null;
			
		}
		
		public override void OnEnter()
		{
			_getAgent();
			
			DoSetAutoBraking();
			
			Finish();		
		}
		
		void DoSetAutoBraking()
		{
			if (autoBraking == null || _agent == null) 
			{
				return;
			}
			
			_agent.autoBraking = autoBraking.Value;
		}
		
	}
}