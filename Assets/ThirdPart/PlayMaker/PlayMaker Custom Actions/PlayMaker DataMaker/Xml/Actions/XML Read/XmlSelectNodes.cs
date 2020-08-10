// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.
//
// © 2012 Jean Fabre http://www.fabrejean.net
//
// To Learn about xPath syntax: http://msdn.microsoft.com/en-us/library/ms256471.aspx
//
using UnityEngine;

using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Gets nodes a xml text asset and an xpath query. Properties are referenced from the node itself, so a '.' is prepended if you use xpath within the property string like ")]
	public class XmlSelectNodes : DataMakerXmlActions
	{
		
		[ActionSection("XML Source")]
		
		public FsmXmlSource xmlSource;
		
		[ActionSection("xPath Query")]
		
		public FsmXpathQuery xPath;
		
		[ActionSection("Result")]
		
		[Tooltip("The result of the xPathQuery, wrapped into a 'result' node, so that it's resuable and a valid xml")]
		[UIHint(UIHint.Variable)]
		public FsmString xmlResult;
		
		[Tooltip("The result of the xPathQuery stored in memory. More efficient if you want to process the result further")]
		public FsmString storeReference;
		
		
		[Tooltip("The number of entries found for the xPathQuery")]
		[UIHint(UIHint.Variable)]
		public FsmInt nodeCount;
		
		[ActionSection("Feedback")]
		[UIHint(UIHint.Variable)]
		public FsmBool found;
		public FsmEvent foundEvent;
		public FsmEvent notFoundEvent;
		public FsmEvent errorEvent;
		
		public override void Reset ()
		{
			xmlSource = null;
			xPath = new FsmXpathQuery();
			
			nodeCount = null;
			
			xmlResult = null;

			found = null;
			foundEvent = null;
			notFoundEvent = null;
			errorEvent = null;
			
		}

		public override void OnEnter ()
		{
			SelectNodeList();

			Finish ();
		}
		
		
		private void SelectNodeList ()
		{

			nodeCount.Value = 0;

			if (xmlSource.Value ==null)
			{
				Debug.LogWarning("XMl source is empty, or likely invalid");
				Fsm.Event (errorEvent);
				return;
			}
			
			string xPathQueryString = xPath.ParseXpathQuery(this.Fsm);
			
			XmlNodeList nodeList =null;
			
			try{
				nodeList = xmlSource.Value.SelectNodes(xPathQueryString,DataMakerXmlUtils.CreateNamespaceManager(xmlSource.Value.OwnerDocument));
			}catch(XPathException e)
			{
				Debug.LogWarning(e.Message);
				Fsm.Event (errorEvent);
				return;
			}
			
			if (nodeList != null) {
				
				nodeCount.Value = nodeList.Count;
				
				if (nodeList.Count==0)
				{
					found.Value = false;
					Fsm.Event (notFoundEvent);
					return;
				}
				
				if (!xmlResult.IsNone)
				{
					xmlResult.Value = DataMakerXmlUtils.XmlNodeListToString(nodeList);
				//	Debug.Log(xmlResult.Value);
				}
				found.Value = true;
				Fsm.Event (foundEvent);
			} else {
				found.Value = false;
				Fsm.Event (notFoundEvent);
			}
			
			if (!string.IsNullOrEmpty(storeReference.Value))
			{
				DataMakerXmlUtils.XmlStoreNodeList(nodeList,storeReference.Value);
			}

			
		}
		
	}
}