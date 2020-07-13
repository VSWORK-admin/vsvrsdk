// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Watch the agent entering and leaving OffMeshLinks. Use this to send an event on change, or store a bool that can be used in other operations. \n" +
		"NOTE: The Game Object must have a NavMeshAgent.")]
	public class WatchAgentIsOnOffMeshLink : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[ActionSection("Result")]
		
		
		[Tooltip("Store flag if agent currently positioned on an OffMeshLink.")]
		[UIHint(UIHint.Variable)]
		public FsmBool isOnOffMeshLink;		
			
		[Tooltip("Trigger this event when isOnOffMeshLink switches to true. Essentially triggers when agent enters an offmeshlink")]
		public FsmEvent isOnOffMeshLinkEvent;

		[Tooltip("Trigger this event when isOnOffMeshLink switches to false. Essentially triggers when agent leaves an offmeshlink")]
		public FsmEvent isNotOnOffMeshLinkEvent;
		
		bool previousValue;
		
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
			isOnOffMeshLink = null;
			isOnOffMeshLinkEvent = null;
			isNotOnOffMeshLinkEvent = null;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			if ( _agent == null) 
			{
				return;
			}
			
			previousValue = _agent.isOnOffMeshLink;
		}

		public override void OnUpdate()
		{
			DoWatchIsOnOffMeshLink();
		}

		void DoWatchIsOnOffMeshLink()
		{

			if ( _agent == null) 
			{
				return;
			}
			
			if (!isOnOffMeshLink.IsNone)
			{
				isOnOffMeshLink.Value = _agent.isOnOffMeshLink;
			}
			
			if (previousValue != _agent.isOnOffMeshLink)
			{
				previousValue = _agent.isOnOffMeshLink;
				
				if(_agent.isOnOffMeshLink && isOnOffMeshLinkEvent != null)
				{
					Fsm.Event(isOnOffMeshLinkEvent);
					return;
				}
				
				if(!_agent.isOnOffMeshLink && isNotOnOffMeshLinkEvent != null)
				{
					Fsm.Event(isNotOnOffMeshLinkEvent);
					return;
				}
			}
						
		}

	}
}