//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net
//  contact: http://www.fabrejean.net/contact.htm
//
// Version Alpha 0.92

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable
using System;

using UnityEngine;
using System.Xml;


namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/HashTable")]
	[Tooltip("Parse a XmlNode properties and attributes inside a HashTable")]
	public class HashTableGetXmlNodeProperties : HashTableActions
	{
		
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;
		
	
		[ActionSection("XML Source")]
		
		public FsmXmlSource xmlSource;		
		
		public FsmXmlPropertiesTypes propertiesTypes;
		
		[ActionSection("Feedback")]
		
		public FsmEvent successEvent;
		public FsmEvent failureEvent;
		
		
		public override void Reset()
		{
			gameObject = null;
			reference = null;
			xmlSource = null;
			propertiesTypes = new FsmXmlPropertiesTypes();
			
			successEvent = null;
			failureEvent = null;
			
		}

		
		public override void OnEnter()
		{
			if ( SetUpHashTableProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
			{
				if (ParseNode())
				{
					 Fsm.Event(successEvent);
				}else{
					 Fsm.Event(failureEvent);
				}
			}
			
			Finish();
		}

		
		public bool ParseNode()
		{
			if (! isProxyValid()) 
				return false;
			
			
			if (xmlSource.Value ==null)
			{
				Debug.LogWarning("XMl source is empty, or likely invalid");
				return false;
			}
			
			// I am not clearing to allow for default values, could be handy, else you can always clear before using this action.
			//proxy.hashTable.Clear();
			
			// get attributes first:
			XmlAttributeCollection attCol = xmlSource.Value.Attributes;
			foreach(XmlAttribute _att in attCol)
			{
				//Debug.Log("_att: @"+_att.Name+" innerText="+_att.InnerText);
				if (!string.IsNullOrEmpty(_att.InnerText))
				{

					proxy.hashTable["@"+_att.Name] = PlayMakerUtils.ParseValueFromString(_att.InnerText,propertiesTypes.GetPropertyType("@"+_att.Name));
				}
			}
			
			// get the node properties
			foreach(XmlNode _childNode in xmlSource.Value.ChildNodes)
			{
				//Debug.Log("_childNode.Name: "+_childNode.Name+" innerText="+_childNode.InnerText);
				if (!string.IsNullOrEmpty(_childNode.InnerText))
				{
					proxy.hashTable[_childNode.Name] = PlayMakerUtils.ParseValueFromString(_childNode.InnerText,propertiesTypes.GetPropertyType(_childNode.Name));	
				}
			}
			
			return true;
		}
	}
}