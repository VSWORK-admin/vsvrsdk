using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using HutongGames.PlayMaker;
using UnityEditor;
using UnityEngine;
using System;

[CustomActionEditor(typeof(XmlGetNextNodeListProperties))]
public class XmlGetNextNodeListPropertiesEditor : CustomActionEditor
{
	int _propCount = -1; // bug where action doesn't set dirty flag when length of array is edited without further edition on other properties

    public override bool OnGUI()
    {
		bool edited = false;
		XmlGetNextNodeListProperties _target = (XmlGetNextNodeListProperties)target;
		
		EditField("nodeListReference");

		EditField("reset");
		EditField("loopEvent");
		EditField("finishedEvent");
		EditField("index");


		if (_target.storeProperties == null || (_target.storeProperties!=null && _target.storeProperties.properties !=null && _target.storeProperties.properties.Length==0))
		{
			if (_target.storeNodeProperties!=null)
			{
				_propCount = _target.storeNodeProperties.Length;
			}
			
			EditField("storeNodeProperties");
			
			if (_target.storeNodeProperties!=null && _propCount != _target.storeNodeProperties.Length)
			{
				edited = true;
			}
		}else{

			edited = edited || DataMakerActionEditorUtils.EditFsmPropertiesStorage(_target.Fsm,_target.storeProperties);
		}
		
		return GUI.changed || edited;
    }
	
}
