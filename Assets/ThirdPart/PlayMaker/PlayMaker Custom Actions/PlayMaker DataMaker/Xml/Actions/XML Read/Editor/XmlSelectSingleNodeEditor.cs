using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;

using HutongGames.PlayMaker;
using UnityEditor;
using UnityEngine;
using System;

[CustomActionEditor(typeof(XmlSelectSingleNode))]
public class XmlSelectSingleNodeEditor : XmlCustomActionEditor
{
	int _propCount = -1; // bug where action doesn't set dirty flag when length of array is edited without further edition on other properties

    public override bool OnGUI()
    {
		bool edited = false;
		XmlSelectSingleNode _target = (XmlSelectSingleNode)target;
	
		if (_target.xmlSource==null)
		{
			_target.xmlSource = new FsmXmlSource();
		}
	
		if (_target.xPath==null)
		{
			_target.xPath = new FsmXpathQuery();
		}
	
		edited = edited || DataMakerActionEditorUtils.EditFsmXmlSourceField(_target.Fsm,_target.xmlSource);
				
		edited = edited || DataMakerActionEditorUtils.EditFsmXpathQueryField(_target.Fsm,_target.xPath);
		
		EditField("xmlResult");
		EditField("storeReference");

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

		EditField("found");
		EditField("foundEvent");
		EditField("notFoundEvent");
		EditField("errorEvent");
		
		return GUI.changed || edited;
    }
	
}
