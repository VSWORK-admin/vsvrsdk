// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Get the next node properties in a nodelist. \nEach time this action is called it gets the next node." +
	         "This lets you quickly loop through all the nodelist to perform actions per nodes.")]
	public class XmlGetNextNodeListProperties : DataMakerXmlActions
	{
		[ActionSection("XML Source")]
		
		public FsmString nodeListReference;
		
		[ActionSection("Set up")]
		
		[Tooltip("Set to true to force iterating from the value of the index variable. This variable will be set to false as it carries on iterating, force it back to true if you want to renter this action back to the first item.")]
		[UIHint(UIHint.Variable)]
		public FsmBool reset;
		
		[Tooltip("Event to send for looping.")]
		public FsmEvent loopEvent;
		
		[Tooltip("Event to send when there are no more nodes.")]
		public FsmEvent finishedEvent;
		
		[ActionSection("Result")]
		
		[Tooltip("Save the node into memory")]
		public FsmString indexedNodeReference;
		
		[Tooltip("The index into the node List")]
		[UIHint(UIHint.Variable)]
		public FsmInt index;
		
		public FsmXmlPropertiesStorage storeProperties; // legacy, and only used in old projects
		
		[ActionSection("Properties Storage")]
		public FsmXmlProperty[] storeNodeProperties; // new version, automatically used on new projects and switched to if storeProperties is found to have no entries. transition is automatic
				
		
		// increment an index as we loop through items
		private int nextItemIndex;

		
		
		private XmlNodeList _nodeList;
		
		public override void Reset()
		{
			nodeListReference = null;
			
			storeProperties = null;
			storeNodeProperties = null;

			reset = null;
			
			finishedEvent = null;
			loopEvent = null;
			
			indexedNodeReference = new FsmString() {UseVariable=true};
			
			index = null;
		}
		
		public override void OnEnter()
		{
			
			if (reset.Value)
			{
				reset.Value =  false;
				if (index.IsNone)
				{
					nextItemIndex = 0;
				}else{
					nextItemIndex = index.Value;
				}
				_nodeList = null;
			}
			
			if (_nodeList==null)
			{
				_nodeList = DataMakerXmlUtils.XmlRetrieveNodeList(nodeListReference.Value);
				if (_nodeList==null)
				{
					Fsm.Event(finishedEvent);
					return;
				}
			}
			
			DoGetNextNode();
			
			Finish();
		}
		
		void DoGetNextNode()
		{		
			// no more items?
			
			int _count = _nodeList.Count;
			
			if (nextItemIndex >= _count)
			{
				nextItemIndex = 0;
				Fsm.Event(finishedEvent);
				return;
			}
			
			if (!string.IsNullOrEmpty(indexedNodeReference.Value))
			{
				DataMakerXmlUtils.XmlStoreNode(_nodeList[nextItemIndex],indexedNodeReference.Value);
			}
			
			// get next item properties
			index.Value = nextItemIndex;

			if (storeNodeProperties.Length>0)
			{
				FsmXmlProperty.StoreNodeProperties(this.Fsm,_nodeList[nextItemIndex],storeNodeProperties);
			}else{
				storeProperties.StoreNodeProperties(this.Fsm,_nodeList[nextItemIndex]);
			}
			
			// no more items?
			if (nextItemIndex >= _count)
			{
				Fsm.Event(finishedEvent);
				nextItemIndex = 0;
				return;
			}
			
			// iterate to next 
			nextItemIndex++;
			
			if (loopEvent!=null){
				Fsm.Event(loopEvent);
			}
		}
	}
}