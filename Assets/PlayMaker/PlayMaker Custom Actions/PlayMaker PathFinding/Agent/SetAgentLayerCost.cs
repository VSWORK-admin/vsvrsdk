// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Sets the cost for traversing over geometry of the layer type. \n " +
		"Cost should be between 1 and infinite. A cost of 3 means that walking 1 meter feels as walking 3 meter when cost is 1. So a higher value means 'more expensive'." +
		 "\nNOTE: The Game Object must have a NavMeshAgent component attached.")]
	public class SetAgentLayerCost : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The Area index.")]
		public FsmInt area;
		
		[Tooltip("OR the Area name.")]
		public FsmString orAreaName;

		[Tooltip("The Layer Cost. A cost of 3 means that walking 1 meter feels as walking 3 meter when cost is 1")]
		public FsmFloat cost;
		
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
			orAreaName = new FsmString(){UseVariable=true};
			cost = null;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoSetLayerCost();

			Finish();		
		}
		
		void DoSetLayerCost()
		{
			if (_agent == null) 
			{
				return;
			}
			
			int areaId = area.Value;
			if (orAreaName.Value!=""){
				areaId = UnityEngine.AI.NavMesh.GetAreaFromName(orAreaName.Value);
			}
			
			_agent.SetAreaCost(areaId,cost.Value);
			
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