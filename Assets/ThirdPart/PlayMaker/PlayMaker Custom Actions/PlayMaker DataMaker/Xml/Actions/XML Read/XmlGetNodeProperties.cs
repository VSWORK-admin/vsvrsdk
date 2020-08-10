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
	[Tooltip("Gets a node attributes and Properties, Properties are referenced from the node itself, so a '.' is prepended if you use xpath within the property string like ")]
	public class XmlGetNodeProperties : DataMakerXmlActions
	{

		[ActionSection("XML Source")]
		
		public FsmXmlSource xmlSource;
		
		[ActionSection("Result")]
		public FsmXmlPropertiesStorage storeProperties; // legacy, and only used in old projects

		[ActionSection("Result")]
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

			storeProperties = null;
			storeNodeProperties = null;

			found = null;
			foundEvent = null;
			notFoundEvent = null;
			errorEvent = null;
		}

		public override void OnEnter ()
		{
			GetNodeProps();

			Finish ();
		}
		
		
		private void GetNodeProps()
		{

			XmlNode node = null;
			
			try{
				node = xmlSource.Value;
			}catch(XPathException e)
			{
				Debug.LogWarning(e.Message);
				Fsm.Event (errorEvent);
				return;
			}
			
			if (node != null) {

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
			
			Finish ();
		}

	}
}