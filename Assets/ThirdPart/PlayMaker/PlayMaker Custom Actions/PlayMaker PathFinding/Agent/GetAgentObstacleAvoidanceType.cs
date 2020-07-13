// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.NavMeshAgent)]
	[Tooltip("Gets the level of quality of avoidance of a NavMesh Agent. \n" +
		"Store as a string or as an int: Range: no:(0), low:(1), medium:(2), good(3), high(4). \n" +
		"NOTE: The Game Object must have a NavMeshAgentcomponent attached.")]
	public class GetAgentObstacleAvoidanceType : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a NavMeshAgent component attached.")]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Store the agent level of quality of avoidance. Range: no,low,medium,good,high")]
		[UIHint(UIHint.Variable)]
		public FsmString storeQualityAsString;
		
		[Tooltip("Store the agent level of quality of avoidance. Range: no:(0), low:(1), medium:(2), good(3), high(4)")]
		[UIHint(UIHint.Variable)]		
		public FsmInt storeQualityAsInt;
		
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
			storeQualityAsString = null;
			storeQualityAsInt = null;
		}

		public override void OnEnter()
		{
			_getAgent();
			
			DoGetbstacleAvoidanceType();

			Finish();		
		}

		void DoGetbstacleAvoidanceType()
		{
			if (_agent==null)
			{
				return;
			}
			
			string levelAsString = "";
			int levelAsInt = 0;
			
			
			switch(_agent.obstacleAvoidanceType)
			{
				case UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance:
					levelAsString = "no";
					levelAsInt = 0;
					break;
				case UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance:
					levelAsString = "low";
					levelAsInt = 1;
					break;	
				case UnityEngine.AI.ObstacleAvoidanceType.MedQualityObstacleAvoidance:
					levelAsString = "medium";
					levelAsInt = 2;
					break;
				case UnityEngine.AI.ObstacleAvoidanceType.GoodQualityObstacleAvoidance:
					levelAsString = "good";
					levelAsInt = 3;
					break;
				case UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance:
					levelAsString = "high";
					levelAsInt = 4;
					break;
			}
			
			if (storeQualityAsString!=null){
				storeQualityAsString.Value = levelAsString;
			}
			if (storeQualityAsInt!=null){
				storeQualityAsInt.Value = levelAsInt;
			}						
		}

	}
}