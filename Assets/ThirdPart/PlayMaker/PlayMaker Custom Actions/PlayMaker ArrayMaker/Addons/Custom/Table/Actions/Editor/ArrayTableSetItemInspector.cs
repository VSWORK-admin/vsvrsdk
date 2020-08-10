//(c) Jean Fabre, 2011-2018 All rights reserved.


using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;


[CustomActionEditor(typeof(ArrayTableSetItem))]
public class ArrayTableSetItemInspector : CustomActionEditor
{
	
	public override bool OnGUI()
	{ 
		ArrayTableSetItem _target = target as ArrayTableSetItem;
		
		EditField("gameObject");
		EditField("UseColumnHeader");

		if (!_target.UseColumnHeader)
		{
			EditField ("atColumnIndex");
		} else {
			EditField ("atColumn");
		}

		EditField("value");
		EditField("failureEvent");
		
		return GUI.changed;
	}
	
	
}
