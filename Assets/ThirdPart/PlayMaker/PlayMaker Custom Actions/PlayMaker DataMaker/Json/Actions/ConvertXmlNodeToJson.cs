// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Json")]
	[Tooltip("Convert a Xml node into a json string ")]
	public class ConvertXmlNodeToJson : DataMakerXmlNodeActions
	{

		[Tooltip("The Reference of the node to Convert.")]
		public FsmString xmlReference;

		[ActionSection("Result")]

		[Tooltip("The json string")]
		public FsmString jsonResult;

		[Tooltip("Event sent if node reference is not found")]
		public FsmEvent errorEvent;

		public override void Reset ()
		{
			xmlReference = null;
			jsonResult = null;
			
		}
		
		public override void OnEnter ()
		{
			ConvertToJsonString();
			
			Finish();
		}
		
		void ConvertToJsonString()
		{
			XmlNode _node = DataMakerXmlUtils.XmlRetrieveNode(xmlReference.Value);
			
			if (_node!=null)
			{
				_node.ParentNode.RemoveChild(_node);
			}else{
				Fsm.Event(errorEvent);
			}

			jsonResult.Value = JsonConvert.SerializeXmlNode(_node);

			Finish ();
		}
		
	}
}