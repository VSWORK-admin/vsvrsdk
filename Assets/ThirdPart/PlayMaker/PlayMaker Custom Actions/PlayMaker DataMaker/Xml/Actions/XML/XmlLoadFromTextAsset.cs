// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.
//
// © 2012 Jean Fabre http://www.fabrejean.net

using UnityEngine;

using System.Xml;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Xml")]
	[Tooltip("Load an xml string from a textAsset")]
	public class XmlLoadFromTextAsset : DataMakerXmlNodeActions
	{
		
		[Tooltip("The xml text")]
		public TextAsset source;
		
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
			source = null;

			storeReference = new FsmString(){UseVariable=true};
			gameObject = null;
			reference = new FsmString(){UseVariable=true};
			
			xmlString = new FsmString(){UseVariable=true};

		}

		public override void OnEnter ()
		{
			LoadFromText();

			Finish();
		}

		void LoadFromText()
		{
			XmlNode _node =	DataMakerXmlUtils.StringToXmlNode(source.text);

			if (_node==null)
			{
				Debug.LogError(DataMakerXmlUtils.lastError);
				Fsm.EventData.StringData = DataMakerXmlUtils.lastError;
				Fsm.Event(errorEvent);
				return;
			}

			if(!storeReference.IsNone)
			{
				DataMakerXmlUtils.XmlStoreNode(_node,storeReference.Value);
			}

			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			DataMakerXmlProxy proxy = DataMakerCore.GetDataMakerProxyPointer(typeof(DataMakerXmlProxy), go, reference.Value, false) as DataMakerXmlProxy;
			
			if (proxy!=null) {
				
				proxy.InjectXmlNode(_node);
			}
			
			if (!xmlString.IsNone) {
				
				xmlString.Value=source.text;
			}


			Finish ();
		}

	}
}