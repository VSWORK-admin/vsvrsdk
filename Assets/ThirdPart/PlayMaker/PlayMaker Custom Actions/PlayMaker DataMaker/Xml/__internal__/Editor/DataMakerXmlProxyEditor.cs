using UnityEditor;
using UnityEngine;
using System.Collections;

using System;
using System.Xml;
using System.Xml.XPath;

using System.IO;

[CustomEditor(typeof(DataMakerXmlProxy))]
public class DataMakerXmlProxyEditor : Editor {
	
	private Vector2 _scroll;
	
	private XmlNode node = null;
	
	private bool valid = false;
	private bool empty = true;
	
	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector();
		DataMakerXmlProxy _target = target as DataMakerXmlProxy;
		
		_target.referenceName =  EditorGUILayout.TextField(new GUIContent("Reference"),_target.referenceName);
		
		_target.FsmEventTarget = EditorGUILayout.ObjectField(new GUIContent("Fsm Event Target"),_target.FsmEventTarget,typeof(PlayMakerFSM),true) as PlayMakerFSM;


		_target.storeInMemory = EditorGUILayout.TextField(new GUIContent("Store In Memory"),_target.storeInMemory);

		if (_target.isDirty)
		{
			Refresh();
		}


		if (!Application.isPlaying)
		{
			if (_target.useSource)
			{
				if(GUILayout.Button("Don't use Text Asset"))
				{
					_target.content = null;
					node = null;
					empty = true;
					_target.XmlTextAsset= null;
					_target.useSource = false;
					
				}

				TextAsset _asset = (TextAsset)EditorGUILayout.ObjectField(new GUIContent("Text Asset"),_target.XmlTextAsset,typeof(TextAsset),false);

				if (_asset!=_target.XmlTextAsset)
				{
					_target.XmlTextAsset = _asset;
					Refresh();
				}

				GUILayout.Label("This proxy also accepts runtime xml results.");
				GUILayout.Label("i.e Use actions like XMlSaveInProxy");
				
			}else{
				
				GUILayout.Label("This proxy accepts runtime xml results");
				GUILayout.Label("i.e Use actions like XMlSaveInProxy");
				GUILayout.Label("But you can also use this proxy as a source");
				if(GUILayout.Button("Use Text Asset"))
				{
					_target.content = null;
					node = null;
					empty = true;
					_target.useSource = true;
				}
			}
		}else{
			if (_target.useSource)
			{
				 TextAsset _asset = (TextAsset)EditorGUILayout.ObjectField(new GUIContent("Text Asset"),_target.XmlTextAsset,typeof(TextAsset),false);
	
				if (_asset!=_target.XmlTextAsset)
				{
					_target.XmlTextAsset = _asset;
					Refresh();
				}
			}else{
				empty = _target.xmlNode==null;

			}
		}

		if (empty)
		{
			DataMakerEditorGUILayoutUtils.feedbackLabel("No XML data",DataMakerEditorGUILayoutUtils.labelFeedbacks.WARNING);
		}else{
			if (!valid)
			{
				DataMakerEditorGUILayoutUtils.feedbackLabel("Xml Invalid",DataMakerEditorGUILayoutUtils.labelFeedbacks.ERROR);
			}else{
				DataMakerEditorGUILayoutUtils.feedbackLabel("Xml Valid",DataMakerEditorGUILayoutUtils.labelFeedbacks.OK);
			}
			_scroll = DataMakerEditorGUILayoutUtils.StringContentPreview(_scroll,_target.content);
			
			if (GUILayout.Button("Refresh"))
			{
				_target.RefreshStringVersion();
			}

			if (_target.XmlTextAsset!=null)
			{
				if (GUILayout.Button("Save back to Text Asset file: "+_target.XmlTextAsset.name))
				{
					string assetPath = AssetDatabase.GetAssetPath(_target.XmlTextAsset.GetInstanceID());
					
					string path = Application.dataPath +  assetPath.Substring(6);
					
					Debug.Log (path+"/n"+_target.content);
					
					File.WriteAllText(path,_target.content);
					AssetDatabase.LoadAssetAtPath(assetPath,typeof(TextAsset));
					AssetDatabase.Refresh();
				}
			}
			
		}
		
		
		if(GUI.changed)
		{
			EditorUtility.SetDirty(target); 		
		}
	}
	
	void OnEnable()
	{
		Refresh();
	}

	void Refresh()
	{
		//Debug.Log("refresh");


		DataMakerXmlProxy _target = target as DataMakerXmlProxy;

		_target.isDirty = false;

		if (_target.xmlNode!=null)
		{
			/*
			if (_target.XmlTextAsset!=null)
			{
				DataMakerEditorGUILayoutUtils.feedbackLabel("XML DATA CHANGED.",DataMakerEditorGUILayoutUtils.labelFeedbacks.WARNING);
			}
			*/
			
			if (node==null || (!node.Equals(_target.xmlNode)) ){
				node = _target.xmlNode;
				_target.content =DataMakerXmlUtils.XmlNodeToString(_target.xmlNode);
				empty = string.IsNullOrEmpty(_target.content);
				//Debug.Log("PARSING XML NODE");
				valid = DataMakerXmlUtils.StringToXmlNode(_target.content) != null;
			}

			return ;
		}

		if (_target.XmlTextAsset!=null)
		{
			empty = string.IsNullOrEmpty(_target.XmlTextAsset.text);
			if (!empty || _target.content==null || !_target.content.Equals(_target.XmlTextAsset.text) ){
				_target.content = _target.XmlTextAsset.text;
				
				//Debug.Log("PARSING TEXT ASSET: \n"+_target.XmlTextAsset.text);
				valid = DataMakerXmlUtils.StringToXmlNode(_target.content) != null;
			}
			
		}
	}
	
	
	
	
	
}
