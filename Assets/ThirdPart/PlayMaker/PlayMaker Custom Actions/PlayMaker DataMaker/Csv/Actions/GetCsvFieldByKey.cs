// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Get a Csv field by key. Use ReadCsv first with the hasKey option set to true.")]
	public class GetCsvFieldByKey : FsmStateAction
	{
		
		[Tooltip("The csv reference defined in ReadCsv action")]
		public FsmString reference;
		
		[Tooltip("The record index")]
		public FsmInt record;

		[Tooltip("The column key")]
		public FsmString key;

		[Tooltip("if true, indexing starts at 0, else first index is 1")]
		public bool zeroBasedIndexing;

		[ActionSection("Result")]
		
		[Tooltip("The field at record index and key for the csv reference")]
		[UIHint(UIHint.Variable)]
		public FsmString field;

		[Tooltip("Event sent if an error ocurred")]
		public FsmEvent errorEvent;

		public override void Reset ()
		{
			reference = null;
			record =  null;
			key = null;
			field = null;
			zeroBasedIndexing = true;

			errorEvent = null;
		}
		
		public override void OnEnter ()
		{
			GetCsvFields();
			
			Finish();
		}
		
		void GetCsvFields()
		{
			
			CsvData _data =  CsvData.GetReference(reference.Value);

			if (_data==null)
			{
				Fsm.Event(errorEvent);
				field.Value = string.Empty;
				return;
			}

			if (!_data.HasHeader)
			{
				LogError("Csv Data ("+reference.Value+") has no header");
				Fsm.Event(errorEvent);
				field.Value = string.Empty;
				return;
			}

			int _record = zeroBasedIndexing?record.Value:record.Value-1;

			if (_data.RecordCount<=_record)
			{
				LogError("Csv Data '"+reference.Value+"' doesn't have "+(_record+1) +" records, only "+_data.RecordCount);
				
				Fsm.Event(errorEvent);
				field.Value = string.Empty;
				return;
			}

			int _column = _data.HeaderKeys.IndexOf(key.Value);
			
			if (_data.ColumnCount<=_column)
			{
				LogError("Csv Data '"+reference.Value+"' doesn't have "+(_column+1)+" columns based on key "+key.Value+", only "+_data.ColumnCount);
				Fsm.Event(errorEvent);
				field.Value = string.Empty;
				return;
			}



			field.Value = _data.GetFieldAt(record.Value,key.Value);
		}
		
	}
}