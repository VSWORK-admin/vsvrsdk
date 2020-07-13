// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMesh)]
	[Tooltip("Gets the area for this OffMeshLink component. \n" +
	         "NOTE: The Game Object must have an OffMeshLink component attached.")]
	public class GetOffMeshLinkNavMeshArea : FsmStateAction
	{
		
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have an OffMeshLink component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.OffMeshLink))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("Store the area for this OffMeshLink component")]
		[UIHint(UIHint.Variable)]
		public FsmInt storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
		
		private UnityEngine.AI.OffMeshLink _offMeshLink;
		
		private void _getOffMeshLink()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_offMeshLink =  go.GetComponent<UnityEngine.AI.OffMeshLink>();
		}
		
		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			_getOffMeshLink();
			
			DoGetArea();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoGetArea();
		}
		
		void DoGetArea()
		{
			if (storeResult == null || _offMeshLink == null) 
			{
				return;
			}
			
			storeResult.Value = _offMeshLink.area;
		}
		
	}
}