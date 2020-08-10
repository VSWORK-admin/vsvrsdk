// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Read a Csv String into accessible data, use GetCsvXXX actions to use it")]
	public class ReadCsv : FsmStateAction
	{
		
		[Tooltip("The csv string")]
		[RequiredField]
		public FsmString csvSource;
		
		[Tooltip("If the csv first line is a headerm check this, it will allow you to use keys to access columns instead of indexes")]
		public FsmBool hasHeader;

		[Tooltip ("Custom delimiter, leave to none for no effect")]
		public FsmString delimiter;


		[Tooltip("Save as csv reference")]
		[RequiredField]
		public FsmString storeReference;

		[ActionSection("Result")]

		[Tooltip("The number of records")]
		[UIHint(UIHint.Variable)]
		public FsmInt recordCount;

		[Tooltip("The number of columns")]
		[UIHint(UIHint.Variable)]
		public FsmInt columnCount;

		public FsmEvent errorEvent;
		
		public override void Reset ()
		{
			csvSource = null;
			hasHeader =  null;

			delimiter = new FsmString () { UseVariable = true };

			recordCount = null;
			columnCount = null;

			storeReference = new FsmString(){UseVariable=true};

			errorEvent = null;
		}
		
		public override void OnEnter ()
		{
			ParseCsv();
			
			Finish();
		}
		
		void ParseCsv()
		{
			CsvData _data;
			if (!delimiter.IsNone) {
				_data = CsvReader.LoadFromString (csvSource.Value, hasHeader.Value,delimiter.Value[0]);
			} else {
				_data  = CsvReader.LoadFromString (csvSource.Value, hasHeader.Value);
			}

			CsvData.AddReference(_data,storeReference.Value);

			if (!recordCount.IsNone)
			{
				recordCount.Value = _data.RecordCount;
			}

			if (!columnCount.IsNone)
			{
				columnCount.Value = _data.ColumnCount;
			}

			Finish ();
		}
		
	}
}