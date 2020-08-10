// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
//

using UnityEngine;

using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Removes all the child nodes and/or attributes of the targeted node.")]
	public class XmlNodeRemoveAll : DataMakerXmlActions
	{

		[Tooltip("The Reference of the node.")]
		public FsmString xmlReference;

		[Tooltip("Event Fired if the node reference is null")]
		public FsmEvent failureEvent;

		public override void Reset ()
		{
			xmlReference = null;
			failureEvent = null;
		}

		public override void OnEnter ()
		{
			XmlNode _node = DataMakerXmlUtils.XmlRetrieveNode(xmlReference.Value);

			if (_node!=null)
			{
				_node.RemoveAll();
			}else{
				Fsm.Event(failureEvent);
			}
			  
			Finish ();
		}
		
	}
}