using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

public class DataMakerXmlUtils {
	
	
	
		
	/// <summary>
	/// Returns an instance of XmlNamespaceManager with a specified XmlDocument.NameTable.  
	/// Returns null if no namespace is defined.
	/// https://blogs.msdn.microsoft.com/helloworld/2007/12/28/dynamically-getting-namespaces-in-an-xml/
	/// </summary>
	/// <param name="Doc">The XmlDocument</param>
	/// <returns>XmlNamespaceManager if there is at least one namespace, null if there is no namespace defined.</returns>
	public static XmlNamespaceManager CreateNamespaceManager(XmlDocument Doc)
	{
		//Create an instance of XPathNavigator at the root of the XmlDocument.
		XPathNavigator Nav = Doc.SelectSingleNode("/*").CreateNavigator();
		XmlNamespaceManager Result = null;
		
		//Move to the first namespace.
		if (Nav.MoveToFirstNamespace())
		{
			Result = new XmlNamespaceManager(Doc.NameTable);
			
			do
			{
				//Add namespaces to XmlNamespaceManager, if the Nav.Name is an empty string, it is the default
				//namespace.  Assign 'default' as the prefix.
				Result.AddNamespace(String.IsNullOrEmpty(Nav.Name)? "default" : Nav.Name, Nav.Value);
			} while (Nav.MoveToNextNamespace());
		}
		
		return Result;
	}

	
	#region Memory Slots
	
	public static Dictionary<string,XmlNode> xmlNodeLUT;
	public static Dictionary<string,XmlNodeList> xmlNodeListLUT;

	// For inspector, to avoid caching for checking if things have changed
	public static bool IsDirty;

	public static void XmlStoreNode(XmlNode node,string reference)
	{
	
		IsDirty = true;

		if (string.IsNullOrEmpty(reference))
		{
			Debug.LogWarning("empty reference.");
		}
		
		if (xmlNodeLUT==null)
		{
			xmlNodeLUT = new Dictionary<string, XmlNode>();
		}
		
		xmlNodeLUT[reference] = node;
	}
	
	public static XmlNode XmlRetrieveNode(string reference)
	{
		if (string.IsNullOrEmpty(reference))
		{
			Debug.LogWarning("empty reference.");
		}
		if (xmlNodeLUT==null)
		{
			return null;
		}
		
		if (!xmlNodeLUT.ContainsKey(reference))
		{
			return null;
		}
		return xmlNodeLUT[reference];
	}

	public static bool DeleteXmlNodeReference(string reference)
	{
		IsDirty = true;

		if (string.IsNullOrEmpty(reference))
		{
			Debug.LogWarning("empty reference.");
		}

		if (!xmlNodeLUT.ContainsKey(reference))
		{
			return false;
		}

		return xmlNodeLUT.Remove (reference);
	}

	public static bool DeleteXmlNodListeReference(string reference)
	{
		IsDirty = true;

		if (string.IsNullOrEmpty(reference))
		{
			Debug.LogWarning("empty reference.");
		}

		if (!xmlNodeListLUT.ContainsKey(reference))
		{
			return false;
		}
		
		return xmlNodeListLUT.Remove (reference);
	}

	public static void XmlStoreNodeList(XmlNodeList nodeList,string reference)
	{
		IsDirty = true;

		if (string.IsNullOrEmpty(reference))
		{
			Debug.LogWarning("empty reference.");
		}
		
		if (xmlNodeListLUT==null)
		{
			xmlNodeListLUT = new Dictionary<string, XmlNodeList>();
		}
		
		xmlNodeListLUT[reference] = nodeList;
	}
	
	public static XmlNodeList XmlRetrieveNodeList(string reference)
	{
		
		if (string.IsNullOrEmpty(reference))
		{
			Debug.LogWarning("empty reference.");
		}
		return xmlNodeListLUT[reference];
	}
	
	
	#endregion Memory Slots
	
	
	public static string lastError = "";
	
	public static XmlNode StringToXmlNode(string content)
	{
			XmlDocument xmlDoc = new XmlDocument();
			try{
				xmlDoc.LoadXml(content);
			}catch(XmlException e)
			{
				lastError = e.Message;
				return null;
			}
			return xmlDoc.DocumentElement as XmlNode;
	}
	
	public static string XmlNodeListToString(XmlNodeList nodeList)
	{
		return XmlNodeListToString(nodeList, 2);
	}
	
	public static string XmlNodeListToString(XmlNodeList nodeList, int indentation)
	{
		
		if (nodeList==null)
		{
			return "-- NULL --";
		}
		
	    using (var sw = new StringWriter())
	    {
	        using (var xw = new XmlTextWriter(sw))
	        {
	            xw.Formatting = Formatting.Indented;
	            xw.Indentation = indentation;
				xw.WriteRaw("<result>");
				
				foreach(XmlNode node in nodeList)
				{
	            	node.WriteTo(xw);
				}
				xw.WriteRaw("</result>");
	        }
	        return sw.ToString();
	    }
	}
	
	public static string XmlNodeToString(XmlNode node)
	{
		return XmlNodeToString(node, 2);
	}
	
	public static string XmlNodeToString(XmlNode node, int indentation)
	{
		if (node==null)
		{
			return "-- NULL --";
		}
	    using (var sw = new StringWriter())
	    {
	        using (var xw = new XmlTextWriter(sw))
	        {
	            xw.Formatting = Formatting.Indented;
	            xw.Indentation = indentation;
	            node.WriteTo(xw);
	        }
	        return sw.ToString();
	    }
	}
}
