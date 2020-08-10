// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using System.Collections;

using System.Xml;

namespace HutongGames.PlayMaker.Actions
{
	public abstract class DataMakerXmlActions: FsmStateAction
	{
		
		public static string GetNodeProperty(XmlNode node,string property)
		{
			if (property.StartsWith("@"))
			{
				property = property.Remove(0,1);
				
				XmlAttribute att = node.Attributes[property];
				if (att != null) {
					return att.InnerText;
				} else {
					//Debug.LogWarning (property + " attribute not found");
				
				}
			}else if (property.StartsWith("/") || property.StartsWith("."))
			{
					if (property.StartsWith("/"))
					{
						property = "." + property;
					}
					
					XmlNode subNode = node.SelectSingleNode(property,
						DataMakerXmlUtils.CreateNamespaceManager(node.OwnerDocument));
					if (subNode != null)
					{
						return subNode.InnerText;
					}
					else
					{
						Debug.LogWarning(property + " not found");
					}
	
			}else
			{
				if (property == "Name()")
				{
					return node.Name;

				}
				else
				{

					XmlNode innerNode = node[property];
					if (innerNode != null)
					{
						return innerNode.InnerText;
					}
					else
					{
						return node.InnerText;
						//Debug.LogWarning (property + " not found");
					}
				}
			}
			
			return "";
		}

		public static void SetNodeProperty(XmlNode node,string property,string propertyValue)
		{
			if (property.StartsWith("@"))
			{
				property = property.Remove(0,1);
				
				XmlAttribute att = node.Attributes[property];
				if (att == null) {
					att = node.OwnerDocument.CreateAttribute(property);
					node.Attributes.Append(att);
				}
				att.InnerText = propertyValue;
				
			}else if (property.StartsWith("/") || property.StartsWith("."))
			{
				if (property.StartsWith("/"))
				{
					property =  "."+property;
				}
				
				XmlNode subNode = node.SelectSingleNode(property,DataMakerXmlUtils.CreateNamespaceManager(node.OwnerDocument));
				if (subNode != null) {
					subNode.InnerText = propertyValue;
				} else {
					Debug.LogWarning (property + " not found");
				}
			
			}else
			{
				XmlNode innerNode = node[property];
				if (innerNode != null) {
					innerNode.InnerText = propertyValue;
				} else {
					//Debug.LogWarning (property + " not found");
				}
			}

		}
		
	}
}