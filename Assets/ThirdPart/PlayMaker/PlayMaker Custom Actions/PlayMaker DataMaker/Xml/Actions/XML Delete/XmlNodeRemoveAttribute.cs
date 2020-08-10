// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
//

using UnityEngine;

using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Remove an attribute from a node.")]
	public class XmlNodeRemoveAttribute : DataMakerXmlActions
	{

		[Tooltip("The Reference of the node to delete.")]
		public FsmString xmlReference;

		public FsmString attribute;

		[Tooltip("Event Fired if the node reference is null")]
		public FsmEvent failureEvent;

		public override void Reset ()
		{
			xmlReference = null;
			attribute = null;
			failureEvent = null;
		}

		public override void OnEnter ()
		{
			XmlNode _node = DataMakerXmlUtils.XmlRetrieveNode(xmlReference.Value);

			if (_node!=null)
			{
				_node.Attributes.RemoveNamedItem(attribute.Value);

			}else{
				Fsm.Event(failureEvent);
			}
			  
			Finish ();
		}
		
	}
}