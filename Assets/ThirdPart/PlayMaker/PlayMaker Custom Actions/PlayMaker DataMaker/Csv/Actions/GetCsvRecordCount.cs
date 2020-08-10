// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Get a Csv Records count. Use ReadCsv first.")]
	public class GetCsvRecordCount : FsmStateAction
	{
		
		[Tooltip("The csv reference defined in ReadCsv action")]
		public FsmString reference;

		[ActionSection("Result")]
		
		[Tooltip("The number of records")]
		[UIHint(UIHint.Variable)]
		public FsmInt recordCount;

		[Tooltip("Event sent if an error ocurred")]
		public FsmEvent errorEvent;

		public override void Reset ()
		{
			reference = null;
			recordCount = null;
			errorEvent = null;
		}
		
		public override void OnEnter ()
		{
			DoGetCsvRecordCount();
			
			Finish();
		}
		
		void DoGetCsvRecordCount()
		{
			
			CsvData _data =  CsvData.GetReference(reference.Value);

			if (_data==null)
			{
				Fsm.Event(errorEvent);
				recordCount.Value = 0;
				return;
			}

			recordCount.Value = _data.RecordCount;
		}
		
	}
}