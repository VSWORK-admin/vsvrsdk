// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Get Csv Fields by Record index and save it in an array of string. Use ReadCsv first.")]
	public class GetCsvFieldsByRecord : FsmStateAction
	{
		
		[Tooltip("The csv reference defined in ReadCsv action")]
		public FsmString reference;
		
		[Tooltip("The record index.")]
		public FsmInt record;

		[Tooltip("if true, indexing starts at 0, else first index is 1")]
		public bool zeroBasedIndexing;

		[ActionSection("Result")]
		
		[Tooltip("All fields at record index the csv reference")]
		[ArrayEditor(VariableType.String)]
		[UIHint(UIHint.Variable)]
		public FsmArray fields;

		[Tooltip("Event sent if an error ocurred")]
		public FsmEvent errorEvent;

		public override void Reset ()
		{
			reference = null;
			record =  null;
			fields = null;
			zeroBasedIndexing = true;
			errorEvent = null;
		}
		
		public override void OnEnter ()
		{
			DoGetCsvFields();
			
			Finish();
		}
		
		void DoGetCsvFields()
		{
			
			CsvData _data =  CsvData.GetReference(reference.Value);

			if (_data==null)
			{
				Fsm.Event(errorEvent);
				fields.Resize(0);
				return;
			}

			int _record = zeroBasedIndexing?record.Value:record.Value-1;

			if (_data.RecordCount<=_record)
			{
				LogError("Csv Data '"+reference.Value+"' doesn't have "+(_record+1) +" records, only "+_data.RecordCount);

				Fsm.Event(errorEvent);
				fields.Resize(0);
				return;
			}



			fields.stringValues = _data.GetRecordAt(_record);
			fields.SaveChanges();
		}
		
	}
}