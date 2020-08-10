// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Removes a row from an ArrayTable")]
	public class ArrayTableRemoveRowAt : ComponentAction<ArrayListTable>
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayListTable Proxy component")]
		[CheckForComponent(typeof(ArrayListTable))]
		public FsmOwnerDefault gameObject;


		[RequiredField]
		[Tooltip("The row to remove. use -1 to remove the last row")]
		public FsmInt rowIndex;


		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		public FsmEvent failureEvent;

		ArrayListTable _at;
		string _error = string.Empty;

		int _index;

		public override void Reset()
		{
			gameObject = null;

			rowIndex = null;

			failureEvent = null;
		}
		

		public override void OnEnter()
		{
			RemoveRowAt();
			
			Finish();
		}
		
		
		
		public void RemoveRowAt(){
			
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject)))
			{
				Debug.Log("ArrayListTable not found for "+PlayMakerUtils.LogFullPathToAction(this));
				Fsm.Event(failureEvent);
				return;
			}

			_at = this.cachedComponent as ArrayListTable;
			_error = string.Empty;
			_index = rowIndex.Value >= 0 ? rowIndex.Value : _at.ColumnData [0].arrayList.Count;


			if (_index <0 || _index >= _at.ColumnData [0].arrayList.Count) {
				Debug.Log("Row index out of range for "+PlayMakerUtils.LogFullPathToAction(this));
				Fsm.Event(failureEvent);
				return;
			}

			try{

				int i = 0;
				foreach(PlayMakerArrayListProxy _column in _at.ColumnData)
				{
					_column.arrayList.RemoveAt(_index);
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