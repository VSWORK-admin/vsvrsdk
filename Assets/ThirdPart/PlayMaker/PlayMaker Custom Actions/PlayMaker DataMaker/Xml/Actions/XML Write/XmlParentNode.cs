// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
//
//
using UnityEngine;

using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Parent a node.")]
	public class XmlParentNode : DataMakerXmlActions
	{
		public enum InsertNodeType {AppendChild,PrependChild,BeforeChild,AfterChild}
		[Tooltip("The parent node")]
		public FsmString parentNodeReference;
		
		public InsertNodeType insert;
		
		[Tooltip("The child node to use for insertion rule")]
		public FsmString childNodeReference;
		
		[ActionSection("XML Node")]
		[RequiredField]
		public FsmString nodeReference;
		
		[ActionSection("Feedback")]
		public FsmEvent errorEvent;

		XmlNode _parentNode;
		XmlNode _node;
		
		public override void Reset ()
		{
			parentNodeReference = null;
			
			insert = InsertNodeType.PrependChild;
			
			childNodeReference = null;
			
			nodeReference = null;
			errorEvent = null;
		}

		public override void OnEnter ()
		{
			ExecuteAction ();
			Finish ();
		}

		void ExecuteAction()
		{
			_parentNode = DataMakerXmlUtils.XmlRetrieveNode(parentNodeReference.Value);
			_node = DataMakerXmlUtils.XmlRetrieveNode(nodeReference.Value);

			if (_parentNode == null || _node == null)
			{
				Fsm.Event(errorEvent);
				return;
			}

			if (insert == InsertNodeType.AfterChild)
			{
				XmlNode refChild = DataMakerXmlUtils.XmlRetrieveNode(childNodeReference.Value);
				_parentNode.InsertAfter(_node,refChild);
				
			}else if (insert == InsertNodeType.BeforeChild)
			{
				XmlNode refChild = DataMakerXmlUtils.XmlRetrieveNode(childNodeReference.Value);
				_parentNode.InsertBefore(_node,refChild);
				
			}else if (insert == InsertNodeType.PrependChild)
			{
				_parentNode.PrependChild(_node);
			}else if (insert == InsertNodeType.AppendChild)
			{
				_parentNode.AppendChild(_node);
			}

			Finish ();
		}

	}
}