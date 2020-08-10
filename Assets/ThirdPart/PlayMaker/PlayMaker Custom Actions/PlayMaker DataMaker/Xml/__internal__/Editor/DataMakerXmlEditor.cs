//	(c) Jean Fabre, 2012-2013 All rights reserved.
//	http://www.fabrejean.net
//
// Version Alpha 0.1
//

using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.IO;

public class DataMakerXmlEditor : Editor {
	
	
	#region Memory Slots
	
	//private Dictionary<stringXmlNode>
	
	public static void XmlSaveNode(FsmXmlNode node,string reference)
	{
		
	}
	
	#endregion Memory Slots
	
	#region Menus
	
	

	// let the user easily add an DataMaker Xml source Proxy
    [MenuItem ("PlayMaker/Addons/DataMaker/Xml/Add Xml Proxy to selected Objects")]
    static void AddDataMakerXmlProxyComponent () {
		 foreach (Transform transform in Selection.transforms) {
			Undo.AddComponent<DataMakerXmlProxy>(transform.gameObject);
			//Undo.RecordObjects(Selection.transforms,"DataMakerXmlProxy Additions");
            //Undo.RegisterUndo(Selection.transforms,"DataMakerXmlProxy Additions");
           // transform.gameObject.AddComponent("DataMakerXmlProxy");
        }
    }
	
	/*
	// let the user easily add an DataMaker Xml node proxy to store XPath results and further process it
    [MenuItem ("PlayMaker Add ons/DataMaker/Xml/Add Xml Node Proxy to selected Objects")]
    static void AddDataMakerXmlNodeProxyComponent () {
		
		 foreach (Transform transform in Selection.transforms) {
            Undo.RegisterUndo(Selection.transforms,"DataMakerXmlNodeProxy Addition");
            transform.gameObject.AddComponent("DataMakerXmlNodeProxy");
        }
    }

	// let the user easily add an DataMaker Xml node List proxy to store XPath results and further work with it
    [MenuItem ("PlayMaker Add ons/DataMaker/Xml/Add Xml Node List Proxy to selected Objects")]
    static void AddDataMakerXmlNodeListProxyComponent () {
		
		 foreach (Transform transform in Selection.transforms) {
            Undo.RegisterUndo(Selection.transforms,"DataMakerXmlNodeListProxy Addition");
            transform.gameObject.AddComponent("DataMakerXmlNodeListProxy");
        }
    }
    */
	
	[MenuItem("Assets/Create/DataMaker/Xml/Create Xml File", false, 0)]
	static void CreateXmlFile()
	{
		string path = "Assets";
		
		if (Selection.activeObject!=null)
		{
			string SelectionPath = AssetDatabase.GetAssetPath(Selection.activeObject);
			
			if (File.Exists(SelectionPath))
	        {
	            path = Path.GetDirectoryName(SelectionPath);
	        }else{
				// it's a folder
				if (SelectionPath.StartsWith("Assets"))
				{
					path = SelectionPath;
				}
			}
		}
		
		path = AssetDatabase.GenerateUniqueAssetPath (path + "/XmlContent.xml");
		StreamWriter _sm =	File.CreateText(path);
		_sm.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<root>\n</root>");
		_sm.Close();
		EditorUtility.FocusProjectWindow();
		AssetDatabase.Refresh();
		Selection.activeObject = AssetDatabase.LoadAssetAtPath(path,typeof(TextAsset));
	}
	
	#endregion Menus
}