//(c) Jean Fabre, 2011-2015 All rights reserved.


using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;


[CustomActionEditor(typeof(ArrayListInsert))]
public class ArrayListInsertCustomEditor : CustomActionEditor
{
	
	public override bool OnGUI()
	{ 
		ArrayListInsert _target = target as ArrayListInsert;

		EditField("gameObject");
		EditField("reference");

		EditField("index");


		EditField("variable");

		if (_target.variable!=null && _target.variable.Type == HutongGames.PlayMaker.VariableType.Int)
		{
			EditField("convertIntToByte");
		}

		EditField("failureEvent");

		return GUI.changed;
	}
	
	
}
