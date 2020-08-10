using UnityEngine;
using System.Collections;

public class DataMakerEditorGUILayoutUtils {
	
	
	public enum labelFeedbacks { OK , WARNING , ERROR};

	public static void feedbackLabel(string message,labelFeedbacks type)
	{
		switch ( type)
		{
		case labelFeedbacks.OK:
			GUI.color = Color.green;
			break;
		case labelFeedbacks.WARNING:
			GUI.color = Color.yellow;
			break;
		case labelFeedbacks.ERROR:
			GUI.color = Color.red;
			break;
		}
		
		GUILayout.BeginHorizontal("box",GUILayout.ExpandWidth(true));
		GUI.color = Color.white;
		GUILayout.Label(message);	
		GUILayout.EndHorizontal();
	}
	
	
	public static Vector2 StringContentPreview(Vector2 scroll, string content)
	{

		string _preview = "";
		if(content !=null && content.Length > 10000)
		{
			_preview = content.Substring(0,9000);
			_preview = _preview + "/n <etc...>";
		}
		else
		{
			_preview = content;
		}


		scroll = GUILayout.BeginScrollView(scroll,"box", GUILayout.Height (200));
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		GUILayout.Box(_preview,"label",null);
		GUILayout.EndScrollView();
		
		return scroll;
	}
	
}
