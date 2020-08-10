// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Get a Csv field by index. Use ReadCsv first.")]
	public class GetCsvFieldByIndex : FsmStateAction
	{
		
		[Tooltip("The csv reference defined in ReadCsv action")]
		public FsmString reference;
		
		[Tooltip("If the csv first line is a header check this, it will allow you to use keys to access columns instead of indexes")]
		public FsmInt record;

		[Tooltip("If the csv first line is a header check this, it will allow you to use keys to access columns instead of indexes")]
		public FsmInt column;

		[Tooltip("if true, indexing starts at 0, else first index is 1")]
		public bool zeroBasedIndexing;


		[ActionSection("Result")]
		
		[Tooltip("The field at row and column for the csv reference")]
		[UIHint(UIHint.Variable)]
		public FsmString field;

		[Tooltip("Event sent if an error ocurred")]
		public FsmEvent errorEvent;

		public override void Reset ()
		{
			reference = null;
			record =  null;
			column = null;
			field = null;
			zeroBasedIndexing =true;

			errorEvent = null;
		}
		
		public override void OnEnter ()
		{
			GetCsvEntry();
			
			Finish();
		}
		
		void GetCsvEntry()
		{
			
			CsvData _data =  CsvData.GetReference(reference.Value);

			if (_data==null)
			{
				Fsm.Event(errorEvent);
				field.Value = string.Empty;
				return;
			}


			int _record = zeroBasedIndexing?record.Value:record.Value-1;
			int _column = zeroBasedIndexing?column.Value:column.Value-1;

			if (_data.RecordCount<=_record)
			{
				LogError("Csv Data '"+reference.Value+"' doesn't have "+(_record+1) +" records, only "+_data.RecordCount);
				
				Fsm.Event(errorEvent);
				field.Value = string.Empty;
				return;
			}

			if (_data.ColumnCount<=_column)
			{
				LogError("Csv Data '"+reference.Value+"' doesn't have "+(_column+1)+" columns, only "+_data.ColumnCount);
				Fsm.Event(errorEvent);
				field.Value = string.Empty;
				return;
			}

			field.Value = _data.GetFieldAt(_record,_column);
		}
		
	}
}