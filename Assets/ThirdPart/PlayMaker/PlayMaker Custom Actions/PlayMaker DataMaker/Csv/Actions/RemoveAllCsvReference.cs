// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Remove All Csv References from runtime Memory")]
	public class RemoveAllCsvReference : FsmStateAction
	{
		public override void OnEnter ()
		{
			CsvData.RemoveAllReferences();
			
			Finish();
		}
	}
}