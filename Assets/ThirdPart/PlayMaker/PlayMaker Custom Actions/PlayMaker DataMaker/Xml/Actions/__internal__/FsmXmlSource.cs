using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

using System.Xml;
using System.Xml.XPath;

public class FsmXmlSource : FsmStateAction {
	
	public string[] sourceTypes =  {"Plain Text","Text Asset","Use Variable","Use Proxy","In Memory"};
	
	public int sourceSelection = 0;
	
	public TextAsset sourcetextAsset;
	
	public FsmString sourceString;
	
	public FsmGameObject sourceProxyGameObject;
	public FsmString sourceProxyReference;
	
	public FsmString inMemoryReference;
	
	
	// I am hosting editor values cause I can't find a way to serialize them inside my editor since I am generating that editor gui with a static util class
	public bool _minimized;
	public Vector2 _scroll;
	public bool _sourcePreview = true;
	public bool _sourceEdit = true;
	
	
	private XmlNode GetXmlNodeFromString(string source)
	{
		XmlDocument xmlDoc = new XmlDocument();
		try{
			xmlDoc.LoadXml(source);
		}catch(XmlException e)
		{
			Debug.Log(source);
			Debug.LogWarning(e.Message);
			return null;
		}
		
		return xmlDoc.DocumentElement as XmlNode;
	}
	
	public XmlNode Value
	{
		get{
			switch (sourceSelection)
			{
			case 0:
			case 2:
				return GetXmlNodeFromString(sourceString.Value);
			case 1:
				if (sourcetextAsset==null)
				{
					return null;
				}
				return GetXmlNodeFromString(sourcetextAsset.text);
			case 3:
				DataMakerXmlProxy proxy = DataMakerCore.GetDataMakerProxyPointer(typeof(DataMakerXmlProxy), sourceProxyGameObject.Value, sourceProxyReference.Value, false) as DataMakerXmlProxy;
				if (proxy!=null)
				{
					return proxy.xmlNode;
				}
				break;
			case 4:
				return DataMakerXmlUtils.XmlRetrieveNode(inMemoryReference.Value);
			default:
				break;
			}
			
			return null;
		}
	}

}