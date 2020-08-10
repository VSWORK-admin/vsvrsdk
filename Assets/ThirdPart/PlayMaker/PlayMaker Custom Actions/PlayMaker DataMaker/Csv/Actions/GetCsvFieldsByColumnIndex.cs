// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Get a Csv Fields by Column index and save it in an array of string. Use ReadCsv first.")]
	public class GetCsvFieldsByColumnIndex : FsmStateAction
	{
		
		[Tooltip("The csv reference defined in ReadCsv action")]
		public FsmString reference;
		
		[Tooltip("The column Index")]
		public FsmInt column;

		[Tooltip("if true, first item is index 0, else first item is index 1")]
		public bool zeroBasedIndexing;

		[ActionSection("Result")]
		
		[Tooltip("All fields at column the csv reference")]
		[ArrayEditor(VariableType.String)]
		[UIHint(UIHint.Variable)]
		public FsmArray result;

		[Tooltip("Event sent if an error ocurred")]
		public FsmEvent errorEvent;

		public override void Reset ()
		{
			reference = null;
			column =  null;
			result = null;
			zeroBasedIndexing = true;

			errorEvent = null;
		}
		
		public override void OnEnter ()
		{
			DoGetCsvColumn();
			
			Finish();
		}
		
		void DoGetCsvColumn()
		{
			
			CsvData _data =  CsvData.GetReference(reference.Value);

			if (_data==null)
			{
				Fsm.Event(errorEvent);
				result.Resize(0);
				return;
			}

			int _column = zeroBasedIndexing?column.Value:column.Value-1;

			if (_data.ColumnCount<=_column)
			{
				LogError("Csv Data '"+reference.Value+"' doesn't have "+(_column)+" columns, only "+_data.ColumnCount);
				Fsm.Event(errorEvent);
				result.Resize(0);
				return;
			}

			result.Resize(_data.RecordCount);
			for(int i=0;i<_data.RecordCount;i++)
			{
				result.Set(i,_data.GetFieldAt(i,_column,false));
			}


			result.SaveChanges();
		}
		
	}
}