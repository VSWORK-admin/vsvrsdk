// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Get a Csv Column by key and save it in an array of string. Use ReadCsv first.")]
	public class GetCsvColumnByKey : FsmStateAction
	{
		
		[Tooltip("The csv reference defined in ReadCsv action")]
		public FsmString reference;
		
		[Tooltip("The column Key")]
		public FsmString key;

		[ActionSection("Result")]
		
		[Tooltip("All values at column the csv reference")]
		[ArrayEditor(VariableType.String)]
		[UIHint(UIHint.Variable)]
		public FsmArray fields;

		[Tooltip("Event sent if an error ocurred")]
		public FsmEvent errorEvent;

		public override void Reset ()
		{
			reference = null;
			key =  null;
			fields = null;
			
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
				fields.Resize(0);
				return;
			}

			
			if (!_data.HasHeader)
			{
				LogError("Csv Data '"+reference.Value+"' has no header");
				Fsm.Event(errorEvent);
				fields.Resize(0);
				return;
			}

			int _column = _data.HeaderKeys.IndexOf(key.Value);

			if (_data.ColumnCount<=_column)
			{
				LogError("Csv Data '"+reference.Value+"' doesn't have "+(_column+1)+" columns based on key "+key.Value+", only "+_data.ColumnCount);
				Fsm.Event(errorEvent);
				fields.Resize(0);
				return;
			}

			fields.Resize(_data.RecordCount);
			for(int i=0;i<_data.RecordCount;i++)
			{
				fields.Set(i,_data.GetFieldAt(i,_column,false));
			}

			fields.SaveChanges();
		}
		
	}
}