// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Get a Csv Columns count. Use ReadCsv first.")]
	public class GetCsvColumnCount : FsmStateAction
	{
		
		[Tooltip("The csv reference defined in ReadCsv action")]
		public FsmString reference;

		[ActionSection("Result")]
		
		[Tooltip("The number of columns")]
		[UIHint(UIHint.Variable)]
		public FsmInt columnCount;

		[Tooltip("Event sent if an error ocurred")]
		public FsmEvent errorEvent;

		public override void Reset ()
		{
			reference = null;
			columnCount = null;
			errorEvent = null;
		}
		
		public override void OnEnter ()
		{
			DoGetCsvColumnCount();
			
			Finish();
		}
		
		void DoGetCsvColumnCount()
		{
			
			CsvData _data =  CsvData.GetReference(reference.Value);

			if (_data==null)
			{
				Fsm.Event(errorEvent);
				columnCount.Value = 0;
				return;
			}

			columnCount.Value = _data.ColumnCount;
		}
		
	}
}