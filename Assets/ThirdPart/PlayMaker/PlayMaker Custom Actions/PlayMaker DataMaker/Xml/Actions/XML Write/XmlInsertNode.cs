// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
//
//
using UnityEngine;

using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Insert a node. Use an xml reference to store it.")]
	public class XmlInsertNode : DataMakerXmlActions
	{
		public enum InsertNodeType {AppendChild,PrependChild,BeforeChild,AfterChild}
		[Tooltip("The parent node")]
		public FsmString parentNodeReference;
		
		public InsertNodeType insert;
		
		[Tooltip("The child node to use for insertion rule")]
		public FsmString childNodeReference;
		
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
			
			insert = InsertNodeType.PrependChild;
			
			childNodeReference = null;
			
			nodeName = null;
			nodeInnerText = null;
			
			attributes = null;
			attributesValues = null;
			
			storeReference = null;
			
			errorEvent = null;
		}

		public override void OnEnter ()
		{
			XmlNode parentNode = DataMakerXmlUtils.XmlRetrieveNode(parentNodeReference.Value);
			
			_node = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element,nodeName.Value,null);
			
			if (! string.IsNullOrEmpty(storeReference.Value))
			{
				DataMakerXmlUtils.XmlStoreNode(_node,storeReference.Value);
			}
			
			SetAttributes();
			
			if (insert == InsertNodeType.AfterChild)
			{
				XmlNode refChild = DataMakerXmlUtils.XmlRetrieveNode(childNodeReference.Value);
				parentNode.InsertAfter(_node,refChild);
				
			}else if (insert == InsertNodeType.BeforeChild)
			{
				XmlNode refChild = DataMakerXmlUtils.XmlRetrieveNode(childNodeReference.Value);
				parentNode.InsertBefore(_node,refChild);
				
			}else if (insert == InsertNodeType.PrependChild)
			{
				parentNode.PrependChild(_node);
			}else if (insert == InsertNodeType.AppendChild)
			{
				parentNode.AppendChild(_node);
			}
			_node.InnerText = nodeInnerText.Value;
			
			Finish ();
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