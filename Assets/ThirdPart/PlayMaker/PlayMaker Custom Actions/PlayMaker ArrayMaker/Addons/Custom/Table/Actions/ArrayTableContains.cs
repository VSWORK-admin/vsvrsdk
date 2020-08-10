// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Check if an item is contained in an ArrayTable")]
	public class ArrayTableContains : ComponentAction<ArrayListTable>
	{

		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayListTable Proxy component")]
		[CheckForComponent(typeof(ArrayListTable))]
		public FsmOwnerDefault gameObject;

		
		[ActionSection("Value")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVar value;

		[ActionSection("Result")]
		
		[Tooltip("Store in a bool wether it contains or not that element (described below)")]
		[UIHint(UIHint.Variable)]
		public FsmBool isContained;	
		
		[Tooltip("Event sent if this arraList contains that element ( described below)")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent isContainedEvent;	
		
		[Tooltip("Event sent if this arraList does not contains that element ( described below)")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent isNotContainedEvent;
		
		[Tooltip("Get The Row Index Result from the variable")]
		[UIHint(UIHint.Variable)]
		public FsmInt rowIndexResult;

		[Tooltip("Get The column Index Result from the variable")]
		[UIHint(UIHint.Variable)]
		public FsmInt columnIndexResult;

		[Tooltip("Get The column Name from the variable")]
		[UIHint(UIHint.Variable)]
		public FsmString columnNameResult;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		public FsmEvent failureEvent;
		
		
		public override void Reset()
		{

			gameObject = null;
			
			failureEvent = null;
			
			value = null;

			isContained = null;
			isContainedEvent = null;
			isNotContainedEvent = null;

			rowIndexResult = null;
			columnIndexResult = null;
			columnNameResult = null;
		}
		
		
		
		public override void OnEnter()
		{
			ExecuteAction();


			Finish();
		}
		
		
		
		public void ExecuteAction()
		{
			
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject)))
			{
				Debug.Log("ArrayListTable not found for "+PlayMakerUtils.LogFullPathToAction(this));
				Fsm.Event(failureEvent);
				return;
			}

			ArrayListTable _at = this.cachedComponent as ArrayListTable;

			PlayMakerUtils.RefreshValueFromFsmVar(this.Fsm,value);

			int _column_i = 0;
			foreach (PlayMakerArrayListProxy _p in _at.ColumnData)
			{
				if (_p.arrayList.Contains(value.GetValue()))
				{

					if (!rowIndexResult.IsNone)
					{
						rowIndexResult.Value = _p.arrayList.IndexOf(value.GetValue());
					}
					if (!columnIndexResult.IsNone)
					{
						columnIndexResult.Value = _column_i;
					}
					if (!columnNameResult.IsNone)
					{
						columnNameResult.Value = _at.GetColumnHeader(_column_i);
					}

					if (!isContained.IsNone)
					{
						isContained.Value = true;
					}

					Fsm.Event(isContainedEvent);

					return;
				}
				_column_i++;
			}

				if (!isContained.IsNone)
				{
					isContained.Value = true;
				}

				Fsm.Event(isNotContainedEvent);

		}
	}
}