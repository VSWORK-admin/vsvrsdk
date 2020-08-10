// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Gets a full row from an ArrayTable and save it into an Hashtable. ArrayTable Header needs to be defined")]
	public class ArrayTableGetRowToHashtable : HashTableActions
	{

		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(ArrayListTable))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The row index to retrieve")]
		public FsmInt atRowIndex;

		[ActionSection("Result")]

		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component to copy the row to")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObjectTarget;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to copy the row to")]
		public FsmString referenceTarget;
		
		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		public FsmEvent failureEvent;
		
		ArrayListTable _at;

		public override void Reset()
		{
			gameObject = null;

			atRowIndex = null;
			gameObjectTarget = null;
			referenceTarget = null;
	
			
			failureEvent = null;

		}
		

		public override void OnEnter()
		{
			GetRowAtIndex();
			
			Finish();
		}
		
		
		
		public void GetRowAtIndex()
		{
			string _error = string.Empty;

			try{

				GameObject _go = Fsm.GetOwnerDefaultTarget (gameObject);

				if (_go!=null)
				{
					_at = _go.GetComponent<ArrayListTable>();
				}

				if (_at == null)
				{
					Debug.Log("ArrayListTable not found for "+PlayMakerUtils.LogFullPathToAction(this));
					Fsm.Event(failureEvent);
					return;
				}

				// now we check the target is defined as well
				if ( !SetUpHashTableProxyPointer(Fsm.GetOwnerDefaultTarget(gameObjectTarget),referenceTarget.Value) )
				{
					Debug.Log("ArrayList target not found for "+PlayMakerUtils.LogFullPathToAction(this));
					Fsm.Event(failureEvent);
					return;
				}
				
				if (! isProxyValid()) 
				{
					Debug.Log("ArrayList proxy  not valid for "+PlayMakerUtils.LogFullPathToAction(this));
					Fsm.Event(failureEvent);
					return;
				}

				if (atRowIndex.Value <0 || (atRowIndex.Value +1) > _at.ColumnData[0].arrayList.Count)
				{
					_error = "Row index out of range";
					throw new UnityException(_error+" for "+PlayMakerUtils.LogFullPathToAction(this));
				}


				proxy.hashTable.Clear();

				int i = 0;
				foreach(PlayMakerArrayListProxy _column in _at.ColumnData)
				{
					proxy.hashTable.Add(
						_at.HeaderProxy.arrayList[i],
						_column.arrayList[atRowIndex.Value]
						);
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