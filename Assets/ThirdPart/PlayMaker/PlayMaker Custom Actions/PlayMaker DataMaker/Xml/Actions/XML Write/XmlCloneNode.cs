// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.
//
//
using UnityEngine;

using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Clone a node. Use an xml reference to store it.")]
	public class XmlCloneNode : DataMakerXmlActions
	{
		[Tooltip("The node")]
		public FsmString nodeReference;

		[Tooltip("true to recursively clone the subtree under the specified node; false to clone only the node itself. ")]
		public FsmBool deepClone;

		[ActionSection("Store Clone Reference")]
		[RequiredField]
		public FsmString storeCloneReference;
		
		[ActionSection("Feedback")]
		public FsmEvent errorEvent;


		XmlNode _node;
		XmlNode _clonedNode;
		
		public override void Reset ()
		{
			nodeReference = null;

			storeCloneReference = null;
			
			errorEvent = null;
		}

		public override void OnEnter ()
		{
			CloneNode();

			Finish ();
		}

		void CloneNode()
		{
			_node = DataMakerXmlUtils.XmlRetrieveNode(nodeReference.Value);

			if (_node==null)
			{
				Fsm.Event(errorEvent);
				return;
			}

			_clonedNode = _node.CloneNode (deepClone.Value);
			if (_clonedNode== null)
			{
				Fsm.Event(errorEvent);
				return;
			}

			if (! string.IsNullOrEmpty(storeCloneReference.Value))
			{
				DataMakerXmlUtils.XmlStoreNode(_clonedNode,storeCloneReference.Value);
			}
		}
	}
}