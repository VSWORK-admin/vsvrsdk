// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Gets an item from an ArrayTable")]
	public class ArrayTableGetItem : ComponentAction<ArrayListTable>
	{

		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayListTable Proxy component")]
		[CheckForComponent(typeof(ArrayListTable))]
		public FsmOwnerDefault gameObject;

		[ActionSection("Column")]
		public bool UseColumnHeader;

		[Tooltip("The colum index to retrieve the item from")]
		public FsmInt atColumnIndex;

		[Tooltip("The colum name to retrieve the item from")]
		public FsmString atColumn;

		[UIHint(UIHint.FsmInt)]
		[Tooltip("The row index to retrieve the item from")]
		public FsmInt atRowIndex;
		
		[ActionSection("Value")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVar value;
		
		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		public FsmEvent failureEvent;
		
		
		public override void Reset()
		{
			atColumnIndex = null;
			atColumn = null;
			UseColumnHeader = false;

			atRowIndex = null;

			gameObject = null;
			
			failureEvent = null;
			
			value = null;
		}
		
		
		
		public override void OnEnter()
		{
			GetItemAtIndex();
			
			Finish();
		}
		
		
		
		public void GetItemAtIndex(){
			
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject)))
			{
				Debug.Log("ArrayListTable not found for "+PlayMakerUtils.LogFullPathToAction(this));
				Fsm.Event(failureEvent);
				return;
			}

			ArrayListTable _at = this.cachedComponent as ArrayListTable;

			object element = null;

			string _error = string.Empty;

			int index =0;

			try{

				if (UseColumnHeader)
				{
					if (_at.HeaderProxy == null)
					{
						_error = "Header Proxy not defined";
						throw new UnityException(_error+" for "+PlayMakerUtils.LogFullPathToAction(this));
					}

					index = _at.HeaderProxy.arrayList.IndexOf(atColumn.Value);

					if (index <0 || (index+1) > _at.ColumnData.Length)
					{
						_error = "Header Column index out of range";
						throw new UnityException(_error+" for "+PlayMakerUtils.LogFullPathToAction(this));
					}

				}else{
					index = atColumnIndex.Value;

					if (index <0 || (index+1) > _at.ColumnData.Length)
					{
						_error = "Column index out of range";
						throw new UnityException(_error+" for "+PlayMakerUtils.LogFullPathToAction(this));
					}
				}

				if (atRowIndex.Value <0 || (atRowIndex.Value +1) > _at.ColumnData[atColumnIndex.Value].arrayList.Count)
				{
					_error = "Row index out of range";
					throw new UnityException(_error+" for "+PlayMakerUtils.LogFullPathToAction(this));
				}

				PlayMakerArrayListProxy _column = _at.ColumnData[index];
				if (_column == null)
				{
					_error = "Column index not found";
					throw new UnityException(_error+" for "+PlayMakerUtils.LogFullPathToAction(this));
				}

				element = _column._arrayList[atRowIndex.Value];
			}catch(System.Exception e){
				Debug.Log(e.Message);
				Fsm.Event(failureEvent);
				return;
			}
			
			PlayMakerUtils.ApplyValueToFsmVar(Fsm,value,element);
		}
	}
}