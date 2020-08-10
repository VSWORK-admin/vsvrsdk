using UnityEditor;
using UnityEngine;
using System.Collections;

using System;
using System.Xml;
using System.Xml.XPath;


[CustomEditor(typeof(DataMakerXmlNodeProxy))]
public class DataMakerXmlNodeProxyEditor : Editor {

	private Vector2 _scroll;
	
	private XmlNode nodeCache;
	private string content;
	
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		DataMakerXmlNodeProxy _target = target as DataMakerXmlNodeProxy;
		
		if (_target._FsmXmlNode!=null)
		{
			
			if (!_target._FsmXmlNode.Value.Equals(nodeCache) ){
				nodeCache = _target._FsmXmlNode.Value;
				content = DataMakerXmlUtils.XmlNodeToString(nodeCache);
			}
			DataMakerEditorGUILayoutUtils.feedbackLabel("Xml Source Valid",DataMakerEditorGUILayoutUtils.labelFeedbacks.OK);
			
			
			_scroll = DataMakerEditorGUILayoutUtils.StringContentPreview(_scroll,content);
			
		}else{
			DataMakerEditorGUILayoutUtils.feedbackLabel("Xml Source Invalid",DataMakerEditorGUILayoutUtils.labelFeedbacks.ERROR);

		}

	
	}
	
}
