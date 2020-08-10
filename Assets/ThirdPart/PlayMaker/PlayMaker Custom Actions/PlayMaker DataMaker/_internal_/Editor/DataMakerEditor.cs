//	(c) Jean Fabre, 2012 All rights reserved.
//	http://www.fabrejean.net


using UnityEditor;
using UnityEngine;
using System.Collections;

using System.IO;

public class DataMakerEditor : Editor {

	
	#region Menus
	
	[MenuItem("Assets/Create/DataMaker/Json/Create Json File", false, 0)]
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
		
		path = AssetDatabase.GenerateUniqueAssetPath (path + "/JsonContent.json");
		StreamWriter _sm =	File.CreateText(path);
		_sm.Write("{}");
		_sm.Close();
		EditorUtility.FocusProjectWindow();
		AssetDatabase.Refresh();
		Selection.activeObject = AssetDatabase.LoadAssetAtPath(path,typeof(TextAsset));
	}
	
	#endregion Menus

// Taken from uTomate
public static T CreateAssetOfType<T> (string preferredName) where T:ScriptableObject
{
	var name = string.IsNullOrEmpty (preferredName) ? typeof(T).Name : preferredName;
	
	
	var path = "Assets";
	

			/*
	foreach (UObject obj in Selection.GetFiltered(typeof(UObject), SelectionMode.Assets))
    {
        path = AssetDatabase.GetAssetPath(obj);
        if (File.Exists(path))
        {
            path = Path.GetDirectoryName(path);

        }
        break;
    }		
	*/
	
	path = AssetDatabase.GenerateUniqueAssetPath (path + "/" + name + ".asset");
	T item =  ScriptableObject.CreateInstance<T> ();
	AssetDatabase.CreateAsset (item, path);
	EditorUtility.FocusProjectWindow ();
	Selection.activeObject = item;
	return item;

}
	
	
}
