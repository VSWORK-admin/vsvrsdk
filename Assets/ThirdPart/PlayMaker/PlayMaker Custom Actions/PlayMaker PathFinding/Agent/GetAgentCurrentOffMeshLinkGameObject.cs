// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the reference to the game object of current OffMeshLink of a NavMeshAgent. \n" +
		"NOTE: The Game Object must have a NavMeshAgentcomponent attached.")]
	public class GetAgentCurrentOffMeshLinkGameObject : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
	
		[RequiredField]	
		[Tooltip("The GameObject with the current OffMeshLink component attached")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeOffMeshLinkGameObject;
		
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
			storeOffMeshLinkGameObject = new FsmGameObject { UseVariable = true};
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoGetCurrentOffMeshLinkGameObject();

			Finish();		
		}

		void DoGetCurrentOffMeshLinkGameObject()
		{
			if (_agent == null)
			{
				return;
			}
			
			if (!_agent.isOnOffMeshLink)
			{
				return;
			}
			
			UnityEngine.AI.OffMeshLink comp = _agent.currentOffMeshLinkData.offMeshLink;// .GetCurrentOffMeshLinkGameObject();
			if (comp!=null){
				storeOffMeshLinkGameObject.Value = comp.gameObject;
			}
		
		}

	}
}