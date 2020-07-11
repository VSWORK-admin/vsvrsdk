// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the cost for path calculation when crossing area of a particular type.. \n" +
		"NOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class GetAgentAreaCost : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The Area index.")]
		public FsmInt area;

		[Tooltip("OR the Area name.")]
		public FsmString orAreaName;
		
		[ActionSection("Result")]
		
		[Tooltip("Store the Area Cost")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;
		
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
			area = null;
			orAreaName = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoGetLayerCost();

			Finish();		
		}
		
		void DoGetLayerCost()
		{
			if (_agent == null) 
			{
				return;
			}
			
			int areaId = area.Value;
			if (orAreaName.Value!=""){
				
				areaId = UnityEngine.AI.NavMesh.GetAreaFromName(orAreaName.Value);
			}
			
			storeResult.Value =	_agent.GetAreaCost(areaId);
		}
		
		public override string ErrorCheck()
		{
			if (orAreaName.Value!="")
			{
				int areaId = UnityEngine.AI.NavMesh.GetAreaFromName(orAreaName.Value);
				if (areaId==-1){
					return "Layer Name '"+orAreaName.Value+"' doesn't exists";
				}else if(area.Value != 0){
					if (areaId == area.Value){
						return "Area reference redundancy. Use 'Area' OR 'Area Name', not both at the same time..";
					}else{
						return "Area conflict, area name '"+orAreaName.Value+"' will be used";
					}
					
				}
			}
			
			return "";
		}

	}
}