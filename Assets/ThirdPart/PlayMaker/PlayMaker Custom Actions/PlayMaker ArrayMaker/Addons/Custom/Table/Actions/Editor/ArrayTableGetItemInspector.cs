//(c) Jean Fabre, 2011-2018 All rights reserved.


using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;


[CustomActionEditor(typeof(ArrayTableGetItem))]
public class ArrayTableGetItemInspector : CustomActionEditor
{
	
	public override bool OnGUI()
	{ 
		ArrayTableGetItem _target = target as ArrayTableGetItem;
		
		EditField("gameObject");
		EditField("UseColumnHeader");

		if (!_target.UseColumnHeader)
		{
			EditField ("atColumnIndex");
		} else {
			EditField ("atColumn");
		}
		
		EditField ("atRowIndex");

		EditField("value");
		EditField("failureEvent");
		
		return GUI.changed;
	}
	
	
}
