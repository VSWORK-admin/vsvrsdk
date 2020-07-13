// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMesh)]
	[Tooltip("True if the OffMeshLink component will try to match endpoint transforms specified by startTransform and endTransform . See also UpdatePositions. \n" +
	         "NOTE: The Game Object must have an OffMeshLink component attached.")]
	public class GetOffMeshLinkAutoUpdatePositions : FsmStateAction
	{
		
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have an OffMeshLink component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.OffMeshLink))]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[Tooltip("True if the OffMeshLink component will try to match endpoint transforms specified by startTransform and endTransform")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[Tooltip("Runs every frame.")]
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
			
			DoGetAutoUpdatePositions();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoGetAutoUpdatePositions();
		}
		
		void DoGetAutoUpdatePositions()
		{
			if (storeResult == null || _offMeshLink == null) 
			{
				return;
			}
			
			storeResult.Value = _offMeshLink.autoUpdatePositions;
		}
		
	}
}