// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using UnityEngine;

using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Insert a node. Use an xml reference to store it.")]
	public class XmlReplaceNode : DataMakerXmlActions
	{

		[Tooltip("The current child node to be replaced by the new child")]
		public FsmString childNodeReference;

		[Tooltip("The new child node to replace the current child")]
		public FsmString newChildNodeReference;

		[ActionSection("Feedback")]
		public FsmEvent errorEvent;

		XmlNode _parent;

		XmlNode _oldChild;
		XmlNode _newChild;
		XmlNode _resultChild;
		public override void Reset ()
		{
			childNodeReference = null;
			newChildNodeReference = null;
			
			errorEvent = null;
		}

		public override void OnEnter ()
		{

			Execute ();

			Finish ();
		}

		void Execute()
		{
			_oldChild = DataMakerXmlUtils.XmlRetrieveNode(childNodeReference.Value);
			_newChild = DataMakerXmlUtils.XmlRetrieveNode(newChildNodeReference.Value);
			
			if (_oldChild == null) {
				Fsm.EventData.StringData = "'childNodeReference' not found";
				Fsm.Event (errorEvent);
				return;
			}
			if (_newChild == null) {
				Fsm.EventData.StringData = "'newChildNodeReference' not found";
				Fsm.Event (errorEvent);
				return;
			}
			if (_oldChild.ParentNode == null) {
				Fsm.EventData.StringData = "'childNodeReference' parent not found";
				Fsm.Event (errorEvent);
				return;
			}
	
			try{
				_oldChild.ParentNode.ReplaceChild(_newChild,_oldChild);
			}catch(XmlException e)
			{
				Fsm.EventData.StringData = e.Message;
				Fsm.Event (errorEvent);
			}

		}
	}
}