using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;

using HutongGames.PlayMaker;
using UnityEditor;
using UnityEngine;
using System;

[CustomActionEditor(typeof(HashTableGetXmlNodeProperties))]
public class HashTableGetXmlNodePropertiesEditor : CustomActionEditor
{

    public override bool OnGUI()
    {
		 
		HashTableGetXmlNodeProperties _target = (HashTableGetXmlNodeProperties)target;
		
		if (_target.xmlSource==null)
		{
			_target.xmlSource = new FsmXmlSource();
		}
		
		EditField("gameObject");
		EditField("reference");
		
		bool edited = DataMakerActionEditorUtils.EditFsmXmlSourceField(_target.Fsm,_target.xmlSource);
		
		edited = edited || DataMakerActionEditorUtils.EditFsmXmlPropertiesTypes(target.Fsm,_target.propertiesTypes);
		
		
		EditField("successEvent");
		EditField("failureEvent");
		
		return GUI.changed || edited;
    }

}
