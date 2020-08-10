// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
//
//
using UnityEngine;

using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Create a node. Use an xml reference to store it.")]
	public class XmlCreateNode : DataMakerXmlActions
	{
		[Tooltip("The parent node")]
		public FsmString parentNodeReference;
		
		
		[ActionSection("XML Node")]
		[RequiredField]
		public FsmString nodeName;
		
		public FsmString nodeInnerText;
		
		[CompoundArray("Node Attributes", "Attribute", "Value")]
		public FsmString[] attributes;
		
		public FsmString[] attributesValues;
		
		[ActionSection("Store Reference")]
		
		public FsmString storeReference;
		
		[ActionSection("Feedback")]
		public FsmEvent errorEvent;
		
		XmlNode _node;
		
		public override void Reset ()
		{
			parentNodeReference = null;
			
			
			nodeName = null;
			nodeInnerText = null;
			
			attributes = null;
			attributesValues = null;
			
			storeReference = null;
			
			errorEvent = null;
		}

		public override void OnEnter ()
		{
			CreateNode();

			Finish ();
		}

		void CreateNode()
		{
			XmlNode parentNode = DataMakerXmlUtils.XmlRetrieveNode(parentNodeReference.Value);

			if (parentNode==null)
			{
				Fsm.EventData.StringData = "parentNode is null";
				Fsm.Event(errorEvent);
				return;
			}

			_node = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element,nodeName.Value,null);

			if (_node== null)
			{
				Fsm.Event(errorEvent);
				return;
			}

			if (! string.IsNullOrEmpty(storeReference.Value))
			{
				DataMakerXmlUtils.XmlStoreNode(_node,storeReference.Value);
			}

			SetAttributes();
			
			parentNode.AppendChild(_node);
			_node.InnerText = nodeInnerText.Value;
		}
		
		private void SetAttributes ()
		{
			int att_i = 0;
			foreach (FsmString att in attributes) {
				SetNodeProperty(_node,att.Value,attributesValues[att_i].Value);
				att_i++;
			}
		}
		
	}
}