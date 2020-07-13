// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the next offMeshLink data of a NavMesh Agent on the current path. In the case that the current path does not contain an OffMeshLink the OffMeshLinkData is marked as invalid. \nNOTE: The Game Object must have a NavMeshAgentcomponent attached.")]
	public class GetAgentNextOffMeshLinkData : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[ActionSection("Result")]
		
		[Tooltip("Link start world position")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 startPos;
		
		[Tooltip("Link start GameObject representing the link start position")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject startGameObject;
		
		[Tooltip("Link end world position")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 endPos;	
		
		[Tooltip("Link end GameObject representing the link end position")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject endGameObject;
		
		[Tooltip("Is Link activated")]
		[UIHint(UIHint.Variable)]
		public FsmBool activated;
		
		[Tooltip("Is Link valid")]
		[UIHint(UIHint.Variable)]
		public FsmBool valid;
		
		[Tooltip("Is Link occupied")]
		[UIHint(UIHint.Variable)]
		public FsmBool occupied; 
		
		[Tooltip("LinkTypeSpecifier: range: manual, dropDown, jumpAcross")]
		[UIHint(UIHint.Variable)]
		public FsmString OffMeshlinkType;
		
		[Tooltip("The pathfinding cost for the link")]
		[UIHint(UIHint.Variable)]
		public FsmFloat costOverride;
		
		[Tooltip("Can link be traversed in both directions")]
		[UIHint(UIHint.Variable)]
		public FsmBool biDirectional; 
		
		[Tooltip("Is Link automatically updating endpoints")]
		[UIHint(UIHint.Variable)]
		public FsmBool autoUpdatePositions; 
		
		[Tooltip("The Area for this OffMeshLink component")]
		[UIHint(UIHint.Variable)]
		public FsmInt navmeshArea; 
		
		
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
			startPos = new FsmVector3 { UseVariable = true};
			startGameObject = new FsmGameObject { UseVariable = true};
			endPos = new FsmVector3 { UseVariable = true};
			endGameObject = new FsmGameObject { UseVariable = true};
			activated = new FsmBool { UseVariable = true};
			valid = new FsmBool { UseVariable = true};
			occupied = new FsmBool { UseVariable = true};
			OffMeshlinkType = new FsmString { UseVariable = true};
			biDirectional = new FsmBool { UseVariable = true};
			autoUpdatePositions = new FsmBool { UseVariable = true};
			navmeshArea = new FsmInt { UseVariable = true};
			costOverride = new FsmFloat { UseVariable = true};
		}
		
		public override void OnEnter()
		{
			_getAgent();
			
			DoGetOffMeshLinkData();
			
			Finish();		
		}
		
		void DoGetOffMeshLinkData()
		{
			if (_agent == null)
			{
				return;
			}
			
			if (!_agent.isOnOffMeshLink)
			{
				return;
			}
			
			UnityEngine.AI.OffMeshLinkData link = new UnityEngine.AI.OffMeshLinkData();
			
			link = _agent.nextOffMeshLinkData;
			if(!startPos.IsNone)
			{
				startPos.Value = link.startPos;
			}
			
			if(!endPos.IsNone)
			{
				endPos.Value = link.endPos;
			}
			
			if(!activated.IsNone)
			{
				activated.Value = link.activated;
			}
			
			if(!valid.IsNone)
			{
				valid.Value = link.valid;
			}
			
			if(!OffMeshlinkType.IsNone)
			{
				switch(link.linkType)
				{
				case UnityEngine.AI.OffMeshLinkType.LinkTypeManual:
					OffMeshlinkType.Value = "manual";
					break;
				case UnityEngine.AI.OffMeshLinkType.LinkTypeDropDown:
					OffMeshlinkType.Value = "dropDown";
					break;
				case UnityEngine.AI.OffMeshLinkType.LinkTypeJumpAcross:
					OffMeshlinkType.Value = "jumpAcross";
					break;
				}
			}
			
			if(!costOverride.IsNone)
			{
				costOverride.Value = link.offMeshLink.costOverride;
			}
			
			if(!occupied.IsNone)
			{
				occupied.Value = link.offMeshLink.occupied;
			}
			
			if(!autoUpdatePositions.IsNone)
			{
				autoUpdatePositions.Value = link.offMeshLink.autoUpdatePositions;
			}
			
			if(!navmeshArea.IsNone)
			{
				navmeshArea.Value = link.offMeshLink.area;
			}
			
			if(!startGameObject.IsNone)
			{
				startGameObject.Value = link.offMeshLink.startTransform.gameObject;
			}
			
			if(!endGameObject.IsNone)
			{
				endGameObject.Value = link.offMeshLink.endTransform.gameObject;
			}
		}
		
	}
}