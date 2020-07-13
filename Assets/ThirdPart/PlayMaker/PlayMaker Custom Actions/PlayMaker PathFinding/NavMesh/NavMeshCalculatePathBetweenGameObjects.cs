// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
// TODO: implement FsmNavMeshPath properly.
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMesh)]
	[Tooltip("Calculate a path between two GameObjects and store the resulting path.")]
	public class NavMeshCalculatePathBetweenGameObjects : FsmStateAction
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The mask specifying which NavMesh layers can be passed when calculating the path.")]
		public FsmInt passableMask;
		
		[RequiredField]
		[Tooltip("The initial position of the path requested.")]
		public FsmOwnerDefault sourceGameObject;

		[RequiredField]
		[Tooltip("The final position of the path requested.")]
		public FsmGameObject targetGameObject;
		
		[ActionSection("Result")]
		

		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.Vector3)]
		[Tooltip("Store the calculated path corners")]
		public FsmArray calculatedPathCorners;

		[Tooltip("The Fsm NavMeshPath proxy component to hold the resulting path")]
		[UIHint(UIHint.Variable)]
		[CheckForComponent(typeof(FsmNavMeshPath))]
		public FsmOwnerDefault calculatedPath;
		
		
		[Tooltip("True If a resulting path is found.")]
		[UIHint(UIHint.Variable)]
		public FsmBool resultingPathFound;
		
		[Tooltip("Trigger event if resulting path found.")]
		public FsmEvent resultingPathFoundEvent;

		[Tooltip("Trigger event if no path could be found.")]
		public FsmEvent resultingPathNotFoundEvent;	
		
		
		
		private FsmNavMeshPath _NavMeshPathProxy;
		
		private void _getNavMeshPathProxy()
		{
			GameObject go = calculatedPath.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : calculatedPath.GameObject.Value;
			if (go == null) 
			{
				return;
			}
			
			_NavMeshPathProxy =  go.GetComponent<FsmNavMeshPath>();
		}	
		
		public override void Reset()
		{

			calculatedPath = new FsmOwnerDefault();
			calculatedPath.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
			calculatedPath.GameObject = new FsmGameObject () { UseVariable=true };

			calculatedPathCorners = null;

			passableMask = -1; // so that by default mask is "everything"
			sourceGameObject = null;
			targetGameObject = null;
			resultingPathFound = null;
			resultingPathFoundEvent = null;
			resultingPathNotFoundEvent = null;
		}

		public override void OnEnter()
		{	
			DoCalculatePath();

			Finish();		
		}
		
		void DoCalculatePath()
		{
			
			GameObject _sourceGameObject = Fsm.GetOwnerDefaultTarget(sourceGameObject);
			if (_sourceGameObject == null) 
			{
				return;
			}
			GameObject _targetGameObject = targetGameObject.Value;
			if (_targetGameObject == null)
			{
				return;
			}

			_getNavMeshPathProxy();
			
			UnityEngine.AI.NavMeshPath _path = new UnityEngine.AI.NavMeshPath();
			
			bool _found = UnityEngine.AI.NavMesh.CalculatePath(_sourceGameObject.transform.position,_targetGameObject.transform.position,passableMask.Value,_path);
			
			if (_NavMeshPathProxy !=null)
			{
				_NavMeshPathProxy.path = _path;
			}

			if (!calculatedPathCorners.IsNone)
			{
				calculatedPathCorners.Resize (_path.corners.Length);
				for (int i = 0; i < calculatedPathCorners.Length; i++)
				{
					calculatedPathCorners.Set (i, _path.corners [i]);
				}

				calculatedPathCorners.SaveChanges();
			}
			
			resultingPathFound.Value = _found;
			
			if (_found)
			{
				if ( ! FsmEvent.IsNullOrEmpty(resultingPathFoundEvent) ){
					Fsm.Event(resultingPathFoundEvent);
				}
			}else
			{
				if (! FsmEvent.IsNullOrEmpty(resultingPathNotFoundEvent) ){
					Fsm.Event(resultingPathNotFoundEvent);
				}
			}
			
			
		}

	}
}