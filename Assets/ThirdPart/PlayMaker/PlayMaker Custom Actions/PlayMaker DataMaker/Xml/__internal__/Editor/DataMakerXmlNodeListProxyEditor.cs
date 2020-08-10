using UnityEditor;
using UnityEngine;
using System.Collections;

using System;
using System.Xml;
using System.Xml.XPath;


[CustomEditor(typeof(DataMakerXmlNodeListProxy))]
public class DataMakerXmlNodeListProxyEditor : Editor {

	private Vector2 _scroll;
	
	private XmlNodeList nodeListCache;
	private string content;
	
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		DataMakerXmlNodeListProxy _target = target as DataMakerXmlNodeListProxy;
		
		if (_target._FsmXmlNodeList!=null)
		{
			
			if (!_target._FsmXmlNodeList.Value.Equals(nodeListCache) ){
				nodeListCache = _target._FsmXmlNodeList.Value;
				content = DataMakerXmlUtils.XmlNodeListToString(nodeListCache);
			}
			DataMakerEditorGUILayoutUtils.feedbackLabel("Xml Source Valid",DataMakerEditorGUILayoutUtils.labelFeedbacks.OK);
			
			
			_scroll = DataMakerEditorGUILayoutUtils.StringContentPreview(_scroll,content);
			
		}else{
			DataMakerEditorGUILayoutUtils.feedbackLabel("Xml Source Invalid",DataMakerEditorGUILayoutUtils.labelFeedbacks.ERROR);

		}
	
	}
	
}
