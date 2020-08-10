// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;

public class DataMakerCsvEditor : Editor {
	
	

	#region Menus
	
	
	[MenuItem("Assets/Create/DataMaker/Csv/Create Csv File", false, 0)]
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
		
		path = AssetDatabase.GenerateUniqueAssetPath (path + "/CsvContent.csv");
		StreamWriter _sm =	File.CreateText(path);
		_sm.Write("Header1,Header2\nValueA,ValueB\nValueC,ValueB");
		_sm.Close();
		EditorUtility.FocusProjectWindow();
		AssetDatabase.Refresh();
		Selection.activeObject = AssetDatabase.LoadAssetAtPath(path,typeof(TextAsset));
	}
	
	#endregion Menus
}