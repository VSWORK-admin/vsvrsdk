// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.
//
// © 2012 Jean Fabre http://www.fabrejean.net
//
//
using System;
using UnityEngine;
using System.Collections;

using HutongGames.PlayMaker;

using System.Xml;


public class DataMakerXmlProxy : DataMakerProxyBase {
	
	static public bool delegationActive = true;

	/// <summary>
	/// If defined, the xml of this proxy will be available in memory.
	/// </summary>
	public string storeInMemory ="";

	public bool useSource;

	public TextAsset XmlTextAsset;
	
	private XmlNode _xmlNode;
	
	[HideInInspector]
	public XmlNode xmlNode
	{
		get{
			return _xmlNode;
		}
		set{
			_xmlNode = value;
		}
	}

	[HideInInspector]
	[NonSerialized]
	public bool isDirty;

	[HideInInspector]
	[NonSerialized]
	public string content;
	
	public PlayMakerFSM FsmEventTarget;
	
	void Awake () {
		if (useSource && XmlTextAsset!=null)
		{
			InjectXmlString(XmlTextAsset.text);
		}
		
		RegisterEventHandlers();
	}

	public void RefreshStringVersion()
	{
		//Debug.Log("RefreshStringVersion");
		this.content = DataMakerXmlUtils.XmlNodeToString(xmlNode);
		//Debug.Log(this.content);
		isDirty = true;
	}
	
	public void InjectXmlNode(XmlNode node)
	{
		xmlNode = node;
		RegisterEventHandlers();
	}
	
	public void InjectXmlNodeList(XmlNodeList nodeList)
	{
		XmlDocument doc = new XmlDocument();
		xmlNode =  doc.CreateElement("root");
		foreach(XmlNode _node in nodeList)
		{
			xmlNode.AppendChild(_node);
		}

		if (!string.IsNullOrEmpty (storeInMemory)) {
			DataMakerXmlUtils.XmlStoreNode(xmlNode,storeInMemory);
		}

		RegisterEventHandlers();
	}
	
	public void InjectXmlString(string source)
	{
		//Debug.Log("InjectXmlString :"+source);
		xmlNode = DataMakerXmlUtils.StringToXmlNode(source);

		if (!string.IsNullOrEmpty (storeInMemory)) {
			DataMakerXmlUtils.XmlStoreNode(xmlNode,storeInMemory);
		}

		RegisterEventHandlers();
	}
	
	
	private void UnregisterEventHandlers()
	{
		// Is it required? since the xmlnode is going to be garbage collected??!
		
		//	xmlNode.OwnerDocument.NodeChanged = null; new XmlNodeChangedEventHandler(NodeTouchedHandler);
		//	xmlNode.OwnerDocument.NodeInserted += new XmlNodeChangedEventHandler(NodeTouchedHandler);
		//	xmlNode.OwnerDocument.NodeRemoved += new XmlNodeChangedEventHandler(NodeTouchedHandler);
	}
	
	private void RegisterEventHandlers()
	{
		if (xmlNode!=null)
		{
			xmlNode.OwnerDocument.NodeChanged += new XmlNodeChangedEventHandler(NodeTouchedHandler);
			xmlNode.OwnerDocument.NodeInserted += new XmlNodeChangedEventHandler(NodeTouchedHandler);
			xmlNode.OwnerDocument.NodeRemoved += new XmlNodeChangedEventHandler(NodeTouchedHandler);
		}
	}
	
	//Define the event handler.
	void NodeTouchedHandler(object src, XmlNodeChangedEventArgs args)
	{
		//Debug.Log("Node " + args.Node.Name + " action:"+args.Action);
		
		if (FsmEventTarget==null || ! delegationActive)
		{
			return;
		}
		
		if(args.Action == XmlNodeChangedAction.Insert)
		{
			PlayMakerUtils.SendEventToGameObject(FsmEventTarget,FsmEventTarget.gameObject,"XML / INSERTED");
		}else if(args.Action == XmlNodeChangedAction.Change)
		{
			PlayMakerUtils.SendEventToGameObject(FsmEventTarget,FsmEventTarget.gameObject,"XML / CHANGED");
		}else if(args.Action == XmlNodeChangedAction.Remove)
		{
			PlayMakerUtils.SendEventToGameObject(FsmEventTarget,FsmEventTarget.gameObject,"XML / REMOVED");
		}
	}
	
}
