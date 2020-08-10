//(c) Jean Fabre, 2011-2015 All rights reserved.


using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;


[CustomActionEditor(typeof(ArrayListAdd))]
public class ArrayListAddCustomEditor : CustomActionEditor
{
	
	public override bool OnGUI()
	{ 
		ArrayListAdd _target = target as ArrayListAdd;

		EditField("gameObject");
		EditField("reference");

		EditField("variable");

		if (_target.variable!=null && _target.variable.Type == HutongGames.PlayMaker.VariableType.Int)
		{
			EditField("convertIntToByte");
		}

		EditField("index");

		return GUI.changed;
	}
	
	
}
