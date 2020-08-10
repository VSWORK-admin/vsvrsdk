//(c) Jean Fabre, 2011-2015 All rights reserved.


using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;


[CustomActionEditor(typeof(HashTableAdd))]
public class HashTableAddCustomEditor : CustomActionEditor
{
	
	public override bool OnGUI()
	{ 
		HashTableAdd _target = target as HashTableAdd;

		EditField("gameObject");
		EditField("reference");

		EditField("key");


		EditField("variable");

		if (_target.variable!=null && _target.variable.Type == HutongGames.PlayMaker.VariableType.Int)
		{
			EditField("convertIntToByte");
		}

		return GUI.changed;
	}
	
	
}
