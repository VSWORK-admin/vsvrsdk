// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Adds a new row to an ArrayTable")]
	public class ArrayTableAddRow : ComponentAction<ArrayListTable>
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayListTable Proxy component")]
		[CheckForComponent(typeof(ArrayListTable))]
		public FsmOwnerDefault gameObject;


		[RequiredField]
		[Tooltip("The new row data")]
		[UIHint(UIHint.Variable)]
		public FsmArray newRow;


		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		public FsmEvent failureEvent;

		ArrayListTable _at;

		string _error = string.Empty;

		public override void Reset()
		{
			gameObject = null;


			failureEvent = null;
		}
		

		public override void OnEnter()
		{
			AddNewRow();
			
			Finish();
		}
		
		
		
		public void AddNewRow(){
			
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject)))
			{
				Debug.Log("ArrayListTable not found for "+PlayMakerUtils.LogFullPathToAction(this));
				Fsm.Event(failureEvent);
				return;
			}

			_at = this.cachedComponent as ArrayListTable;
			_error = string.Empty;

			if (newRow.Length != _at.ColumnData.Length) {
				Debug.Log("Column count not matching the newRow count for "+PlayMakerUtils.LogFullPathToAction(this));
				Fsm.Event(failureEvent);
				return;
			}

			try{

				int i = 0;
				foreach(PlayMakerArrayListProxy _column in _at.ColumnData)
				{
					_column.arrayList.Add(newRow.Values[i]);
					i++;
				}

			}catch(System.Exception e){
				Debug.Log(e.Message);
				Fsm.EventData.StringData = _error;
				Fsm.Event(failureEvent);
				return;
			}

		}
	}
}