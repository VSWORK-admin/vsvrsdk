// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Json")]
	[Tooltip("Convert an json string into a Xml node")]
	public class ConvertJsonStringToXmlNode : DataMakerXmlNodeActions
	{
		
		[Tooltip("The json string")]
		public FsmString jsonSource;

		[Tooltip("The optional root element Name. Leave to none for no effect on the json source hierearchy. WARNING, json can have multiple root, and so as a safety measure, it should be set if you are unsure of the incoming content hierarchy")]//. If json has multiple root, this action will automatically inject a 'root' or use this root element name property")]
		public FsmString rootElementName;

		//[Tooltip("If true, rootElementName property will be forced as the xml root, and will use 'root' node name is rootElementName property is none or empty.")]
		//public FsmBool forceRootElementName;


		[ActionSection("Result")]
		
		[Tooltip("Save as xml reference")]
		public FsmString storeReference;
		
		[Tooltip("Save in DataMaker Xml Proxy component")]
		[CheckForComponent(typeof(DataMakerXmlProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the DataMaker Xml Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;
		
		[Tooltip("Save as string")]
		public FsmString xmlString;
		
		public FsmEvent errorEvent;

		public override void Reset ()
		{
			jsonSource = null;
			rootElementName =  "root";
			//forceRootElementName = true;

			storeReference = new FsmString(){UseVariable=true};
			gameObject = null;
			reference = new FsmString(){UseVariable=true};
			
			xmlString = new FsmString(){UseVariable=true};
			
		}
		
		public override void OnEnter ()
		{
			ConvertFromJsonString();
			
			Finish();
		}
		
		void ConvertFromJsonString()
		{
			string _jsonSource = jsonSource.Value;

			/* fail to check if json root is a list of a single object... odd
			// check if json has unique root or not.
			try{

				System.Object _json = (System.Object)JsonConvert.DeserializeObject(_jsonSource, typeof(System.Object));
			}catch(Exception e)
			{
				Debug.LogError(e.Message);
				Fsm.EventData.StringData = e.Message;
				Fsm.Event(errorEvent);
				return;
			}

			bool jsonHasRoot = _jsonSource.Length==1;

			string ForcedRootName = string.IsNullOrEmpty(rootElementName.Value)?"root":rootElementName.Value;

	*/

			XmlDocument _document = new XmlDocument();
			try{
				//if(!jsonHasRoot || !string.IsNullOrEmpty(rootElementName.Value) || forceRootElementName.Value)

				if(!string.IsNullOrEmpty(rootElementName.Value) )
				{
					_document = JsonConvert.DeserializeXmlNode(_jsonSource,rootElementName.Value);
				}else{
					_document = JsonConvert.DeserializeXmlNode(_jsonSource);
				}
			}catch(Exception e)
			{
				Debug.LogError(e.Message);
				Fsm.EventData.StringData = e.Message;
				Fsm.Event(errorEvent);
				return;
			}



			if (_document==null)
			{
				Debug.LogError(DataMakerXmlUtils.lastError);
				Fsm.EventData.StringData = DataMakerXmlUtils.lastError;
				Fsm.Event(errorEvent);
				return;
			}

			if(!storeReference.IsNone)
			{
				if (_document.DocumentElement.GetType() == typeof(XmlNodeList) )
				{
					//DataMakerXmlUtils.XmlStoreNodeList(_document.DocumentElement as XmlNodeList,storeReference.Value);
				}else{
					DataMakerXmlUtils.XmlStoreNode(_document.DocumentElement as XmlNode ,storeReference.Value);
				}
			}
			
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			DataMakerXmlProxy proxy = DataMakerCore.GetDataMakerProxyPointer(typeof(DataMakerXmlProxy), go, reference.Value, false) as DataMakerXmlProxy;
			
			if (proxy!=null) {

				if (_document.DocumentElement.GetType() ==  typeof(XmlNodeList) )
				{
				//	proxy.InjectXmlNodeList(_document.DocumentElement as XmlNode);
				}else{
					proxy.InjectXmlNode(_document.DocumentElement as XmlNode);
				}
			}

			proxy.RefreshStringVersion();

			if (!xmlString.IsNone) {
				
				xmlString.Value= proxy.content;
			}

			Finish ();
		}
		
	}
}