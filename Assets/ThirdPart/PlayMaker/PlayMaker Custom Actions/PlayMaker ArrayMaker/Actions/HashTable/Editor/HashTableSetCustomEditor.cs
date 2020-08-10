//(c) Jean Fabre, 2011-2015 All rights reserved.


using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;


[CustomActionEditor(typeof(HashTableSet))]
public class HashTableSetCustomEditor : CustomActionEditor
{
	
	public override bool OnGUI()
	{ 
		HashTableSet _target = target as HashTableSet;

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
