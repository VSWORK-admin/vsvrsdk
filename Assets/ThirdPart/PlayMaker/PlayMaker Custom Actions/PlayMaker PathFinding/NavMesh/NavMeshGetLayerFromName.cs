// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.



namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMesh)]
	[Tooltip("Gets the area index for a named area.")]
	public class NavMeshGetLayerFromName : FsmStateAction
	{	
		[Tooltip("The area Name")]
		public FsmString areaName;
		
		[ActionSection("Result")]
		
		[Tooltip("Store the area Index for this area Name")]
		[UIHint(UIHint.Variable)]
		public FsmInt areaIndex;

		
		public override void Reset()
		{
			areaName = null;
			areaIndex = null;
		}
		
		public override void OnEnter()
		{	
			DoGetAreaFromName();
			
			Finish();		
		}
		
		void DoGetAreaFromName()
		{
			areaIndex.Value = UnityEngine.AI.NavMesh.GetAreaFromName(areaName.Value);
		}
		

	}
}