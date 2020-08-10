// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;

using HutongGames.PlayMaker.Ecosystem.DataMaker.CSV;


namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("DataMaker Csv")]
	[Tooltip("Convert an csv string into a Xml node")]
	public class ConvertCsvStringToXmlNode : DataMakerXmlNodeActions
	{
		
		[Tooltip("The Csv string")]
		[RequiredField]
		public FsmString csvSource;

		[Tooltip("If the csv first line is a headerm check this, it will allow you to use keys to access columns instead of indexes")]
		public FsmBool hasHeader;

		[Tooltip ("Custom delimiter, leave to none for no effect")]
		public FsmString delimiter;

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


		string[] _csvHeader;

		public override void Reset ()
		{
			csvSource = null;
			hasHeader = null;

			delimiter = new FsmString () { UseVariable = true };

			storeReference = new FsmString(){UseVariable=true};
			gameObject = null;
			reference = new FsmString(){UseVariable=true};
			
			xmlString = new FsmString(){UseVariable=true};
			
		}
		
		public override void OnEnter ()
		{
			ConvertFromCsvString();
			
			Finish();
		}
		
		void ConvertFromCsvString()
		{
			CsvData _data;

			if (!delimiter.IsNone) {
				_data = CsvReader.LoadFromString (csvSource.Value, hasHeader.Value, delimiter.Value [0]);
			} else {
				_data = CsvReader.LoadFromString (csvSource.Value, hasHeader.Value);
			}
		
			XmlDocument _document = new XmlDocument();
			XmlNode _root =	_document.AppendChild(_document.CreateElement("Root"));

			try{

				if (_data.HasHeader)
				{
					_csvHeader =_data.HeaderKeys.ToArray();
				}

				foreach(List<string> _row in _data.Data)
				{
					XmlNode _xmlRowNode = _document.CreateElement("Record");

					for(int i=0;i<_row.Count;i++)
					{
						XmlNode _Item = _document.CreateElement(_data.HasHeader?_csvHeader[i]:"Field");
						_Item.InnerText = _row[i];
						_xmlRowNode.AppendChild(_Item);
					}

					_root.AppendChild(_xmlRowNode);
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
//					DataMakerXmlUtils.XmlStoreNodeList(_document.DocumentElement as XmlNodeList,storeReference.Value);
				}else{
					DataMakerXmlUtils.XmlStoreNode(_document.DocumentElement as XmlNode ,storeReference.Value);
				}
			}
			
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go != null) {
				DataMakerXmlProxy proxy = DataMakerCore.GetDataMakerProxyPointer (typeof (DataMakerXmlProxy), go, reference.Value, false) as DataMakerXmlProxy;

				if (proxy != null) {

					if (_document.DocumentElement.GetType () == typeof (XmlNodeList)) {
						//	proxy.InjectXmlNodeList(_document.DocumentElement as XmlNode);
					} else {
						proxy.InjectXmlNode (_document.DocumentElement as XmlNode);
					}
				}

				proxy.RefreshStringVersion ();
			}

			if (!xmlString.IsNone) 
			{

				xmlString.Value = DataMakerXmlUtils.XmlNodeToString (_document.DocumentElement as XmlNode);
			}

			Finish ();
		}
		
	}
}