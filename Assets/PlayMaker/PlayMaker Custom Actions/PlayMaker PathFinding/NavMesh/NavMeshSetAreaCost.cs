// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.



namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMesh)]
	[Tooltip("Sets the cost for finding path over geometry of the area type on all agents.\n" +
	         "This will replace any custom area costs on all agents, and set the default cost for new agents that are created after calling the function. The cost must be larger than 1.0.")]
	public class NavMeshSetLayerCost : FsmStateAction
	{

		[Tooltip("The area index.")]
		public FsmInt area;

		[Tooltip("OR The area name.")]
		public FsmString orAreaName;
		
		[Tooltip("The Layer Cost")]
		public FsmFloat cost;
		
		
		public override void Reset()
		{
			area = null;
			orAreaName = new FsmString(){UseVariable=true};
			cost = null;
		}

		public override void OnEnter()
		{
			DoSetAreaCost();

			Finish();		
		}
		
		void DoSetAreaCost()
		{
			
			int areaId = area.Value;
			if (orAreaName.Value!=""){
				areaId = UnityEngine.AI.NavMesh.GetAreaFromName(orAreaName.Value);
			}
			UnityEngine.AI.NavMesh.SetAreaCost(areaId,cost.Value);
			
		}
		
		public override string ErrorCheck()
		{
			
			if (orAreaName.Value!="")
			{
				int areaId = UnityEngine.AI.NavMesh.GetAreaFromName(orAreaName.Value);
				if (areaId==-1){
					return "Area Name '"+orAreaName.Value+"' doesn't exists";
				}else if(area.Value != 0){
					if (areaId == area.Value){
						return "Area reference redundancy. Use 'Area' OR 'Area Name', not both at the same time.";
					}else{
						return "Area conflict, area name '"+orAreaName.Value+"' will be used";
					}
					
				}
			}
			
			return "";
		}

		
	}
}