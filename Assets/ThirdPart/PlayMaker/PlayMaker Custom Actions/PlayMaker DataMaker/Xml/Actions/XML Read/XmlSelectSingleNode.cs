// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
//
// To Learn about xPath syntax: http://msdn.microsoft.com/en-us/library/ms256471.aspx
//
using UnityEngine;
using System;
using System.Xml;
using System.Xml.XPath;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Gets a node attributes and cdata from a xml text asset and an xpath query. Properties are referenced from the node itself, so a '.' is prepended if you use xpath within the property string like ")]
	public class XmlSelectSingleNode : DataMakerXmlActions
	{
		
		[ActionSection("Xml Source")]
		public FsmXmlSource xmlSource;
		
		[ActionSection("xPath Query")]
		
		public FsmXpathQuery xPath;
		
		[ActionSection("Result")]
		
		[Tooltip("The result of the xPathQuery as an xml string")]
		[UIHint(UIHint.Variable)]
		public FsmString xmlResult;
		
		[Tooltip("The result of the xPathQuery stored in memory. More efficient if you want to process the result further")]
		public FsmString storeReference;
		
		[ActionSection("Properties Storage")]
		public FsmXmlPropertiesStorage storeProperties; // legacy, and only used in old projects

		[ActionSection("Properties Storage")]
		public FsmXmlProperty[] storeNodeProperties; // new version, automatically used on new projects and switched to if storeProperties is found to have no entries. transition is automatic
		
		[ActionSection("Feedback")]
		[UIHint(UIHint.Variable)]
		public FsmBool found;
		public FsmEvent foundEvent;
		public FsmEvent notFoundEvent;
		public FsmEvent errorEvent;
		
		
		public override void Reset ()
		{
			xmlSource = null;
			
			xPath = null;

			xmlResult = null;
			storeReference = null;
			
			storeProperties = null;

			storeNodeProperties = null;

			found = null;
			foundEvent = null;
			notFoundEvent = null;
			errorEvent = null;
		}

		public override void OnEnter ()
		{

			SelectSingleNode();

			Finish ();
		}

		
		private void SelectSingleNode ()
		{
			
			if (xmlSource.Value ==null)
			{
				Debug.LogWarning("XMl source is empty, or likely invalid");
				
				Fsm.Event (errorEvent);
				return;
			}
			
			string xPathQueryString = xPath.ParseXpathQuery(this.Fsm);
			
			XmlNode node = null;
			
			try{
				node = xmlSource.Value.SelectSingleNode(xPathQueryString,DataMakerXmlUtils.CreateNamespaceManager(xmlSource.Value.OwnerDocument));
			}catch(XPathException e)
			{
				Debug.LogWarning(e.Message);
				Fsm.Event (errorEvent);
				return;
			}
			
			if (node != null) {

				if (!xmlResult.IsNone)
				{
					xmlResult.Value = DataMakerXmlUtils.XmlNodeToString(node);
				}

				if (storeNodeProperties.Length>0)
				{
					FsmXmlProperty.StoreNodeProperties(this.Fsm,node,storeNodeProperties);
				}else{
					storeProperties.StoreNodeProperties(this.Fsm,node);
				}


				found.Value = true;
				Fsm.Event (foundEvent);
			} else {
				found.Value = false;
				Fsm.Event (notFoundEvent);
			}
			
			if (!string.IsNullOrEmpty(storeReference.Value))
			{
				DataMakerXmlUtils.XmlStoreNode(node,storeReference.Value);
			}
			
			
			Finish ();
		}
		
	}
}