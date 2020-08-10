// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Get a Csv Header and save it in an array of string. Use ReadCsv first.")]
	public class GetCsvHeader : FsmStateAction
	{
		
		[Tooltip("The csv reference defined in ReadCsv action")]
		public FsmString reference;

		[ActionSection("Result")]
		
		[Tooltip("All header values the csv reference")]
		[ArrayEditor(VariableType.String)]
		[UIHint(UIHint.Variable)]
		public FsmArray header;

		[Tooltip("Event sent if an error ocurred")]
		public FsmEvent errorEvent;

		public override void Reset ()
		{
			reference = null;
			header = null;
			
			errorEvent = null;
		}
		
		public override void OnEnter ()
		{
			DoGetCsvHeader();
			
			Finish();
		}
		
		void DoGetCsvHeader()
		{
			CsvData _data =  CsvData.GetReference(reference.Value);

			if (_data==null)
			{
				Fsm.Event(errorEvent);
				header.Resize(0);
				return;
			}

			if (!_data.HasHeader)
			{
				LogError("Csv Data '"+reference.Value+"' has no header");
				Fsm.Event(errorEvent);
				header.Resize(0);
				return;
			}


			header.stringValues = _data.HeaderKeys.ToArray();
			header.SaveChanges();
		}
		
	}
}