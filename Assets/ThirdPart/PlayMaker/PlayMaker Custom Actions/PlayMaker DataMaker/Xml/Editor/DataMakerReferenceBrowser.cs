using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DataMakerReferenceBrowser : EditorWindow {

	public static DataMakerReferenceBrowser Instance;

	List<string> xmlNodePreviews = new List<string>();
	Dictionary<string,Vector2> xmlNodePreviewsScroll = new Dictionary<string, Vector2>();

	List<string> xmlNodeListPreviews = new List<string>();
	Dictionary<string,Vector2> xmlNodeListPreviewsScroll = new Dictionary<string, Vector2>();

	Vector2 scroll =  Vector2.zero;

	static GUIStyle _BigTitle;

	public bool LiveUpdate;

	[MenuItem ("PlayMaker/Addons/DataMaker/Xml/Reference Browser",false,1)]
	static void Init () {

		
		// Get existing open window or if none, make a new one:
		Instance = (DataMakerReferenceBrowser)EditorWindow.GetWindow (typeof (DataMakerReferenceBrowser));
		
		Instance.position = new Rect(100,100, 430,600);
		Instance.minSize = new Vector2(200,200);
		#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0
		Instance.title = "DataMaker";
		#else

		Instance.titleContent = new GUIContent("DataMaker","DataMaker Reference Browser");
		#endif

	}

	void OnInspectorUpdate() {

		if (DataMakerXmlUtils.IsDirty || LiveUpdate ) {
			DataMakerXmlUtils.IsDirty = false;
			Repaint ();
		}

	}

	void OnGUI () { wantsMouseMove = true;


		if (Event.current.type == EventType.MouseMove) Repaint ();

		GUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.FlexibleSpace();
		LiveUpdate = GUILayout.Toggle(LiveUpdate,"Live Update",EditorStyles.toolbarButton);

		GUILayout.EndHorizontal ();

		scroll = GUILayout.BeginScrollView(scroll);

		OnGUI_Title("XmlNode References");

		if (DataMakerXmlUtils.xmlNodeLUT != null) {
			GUILayout.BeginVertical ();
			foreach (KeyValuePair<string,XmlNode> entry in DataMakerXmlUtils.xmlNodeLUT) {
				GUILayout.BeginHorizontal ();

				if (GUILayout.Button (entry.Key)) {
					ToggleXmlNodePreview (entry.Key);
				}

				if (GUILayout.Button ("Copy", GUILayout.Width (50))) {
					PlayMakerEditorUtils.CopyTextToClipboard (DataMakerXmlUtils.XmlNodeToString (entry.Value));
				}

				if (GUILayout.Button ("x", GUILayout.Width (30))) {
					DataMakerXmlUtils.DeleteXmlNodeReference (entry.Key);
					GUIUtility.ExitGUI ();
					return;
				}
				
				GUILayout.EndHorizontal ();

				if (xmlNodePreviews.Contains (entry.Key)) {
					OnGUI_XmlNodeReferencePreview (entry.Key);
				}

			}
			GUILayout.EndVertical ();

		} else {
			GUILayout.Label ("- nothing yet -");
		}

		OnGUI_Title("XmlNodeList References");

		if (DataMakerXmlUtils.xmlNodeListLUT != null) 
		{
			GUILayout.BeginVertical();
			foreach(KeyValuePair<string,XmlNodeList> entry in DataMakerXmlUtils.xmlNodeListLUT)
			{
				GUILayout.BeginHorizontal();

				if (GUILayout.Button(entry.Key))
				{
					ToggleXmlNodeListPreview(entry.Key);
				}

				if (GUILayout.Button("Copy",GUILayout.Width(50)))
				{
					PlayMakerEditorUtils.CopyTextToClipboard(DataMakerXmlUtils.XmlNodeListToString (entry.Value));
				}

				if (GUILayout.Button("x",GUILayout.Width(30)))
				{
					DataMakerXmlUtils.DeleteXmlNodeReference(entry.Key);
					GUIUtility.ExitGUI ();
					return;
				}
			
				GUILayout.EndHorizontal();

				if (xmlNodeListPreviews.Contains(entry.Key))
				{
					OnGUI_XmlNodeListReferencePreview(entry.Key);
				}
				
			}
			GUILayout.EndVertical();
			
		} else {
			GUILayout.Label ("- nothing yet -");
		}


		GUILayout.EndScrollView();
	}

	void ToggleXmlNodePreview(string reference)
	{
		if (xmlNodePreviews.Contains (reference)) {
			xmlNodePreviews.Remove(reference);
			xmlNodePreviewsScroll.Remove(reference);
		} else {
			xmlNodePreviews.Add(reference);
		}
	}
	
	
	void OnGUI_XmlNodeReferencePreview(string reference)
	{
		string preview = DataMakerXmlUtils.XmlNodeToString (DataMakerXmlUtils.XmlRetrieveNode (reference));
		
		if (string.IsNullOrEmpty (preview)) {
			GUILayout.Label ("-- empty --");
		} else {
			if (!xmlNodePreviewsScroll.ContainsKey(reference))
			{
				xmlNodePreviewsScroll.Add(reference,Vector2.zero);
			}
			xmlNodePreviewsScroll [reference] = DataMakerEditorGUILayoutUtils.StringContentPreview (xmlNodePreviewsScroll [reference], preview);
		}
	}

	void ToggleXmlNodeListPreview(string reference)
	{
		if (xmlNodeListPreviews.Contains (reference)) {
			xmlNodeListPreviews.Remove(reference);
			xmlNodeListPreviewsScroll.Remove(reference);
		} else {
			xmlNodeListPreviews.Add(reference);
		}
	}


	void OnGUI_XmlNodeListReferencePreview(string reference)
	{
		string preview = DataMakerXmlUtils.XmlNodeListToString (DataMakerXmlUtils.XmlRetrieveNodeList (reference));

		if (string.IsNullOrEmpty (preview)) {
			GUILayout.Label ("-- empty --");
		} else {
			if (!xmlNodeListPreviewsScroll.ContainsKey(reference))
			{
				xmlNodeListPreviewsScroll.Add(reference,Vector2.zero);
			}
			xmlNodeListPreviewsScroll [reference] = DataMakerEditorGUILayoutUtils.StringContentPreview (xmlNodeListPreviewsScroll [reference], preview);
		}
	}

	void OnGUI_Title(string title)
	{
		if (_BigTitle == null) {
			_BigTitle = GUI.skin.FindStyle ("IN BigTitle");
		}

		GUILayout.BeginHorizontal (_BigTitle,GUILayout.ExpandWidth(true));
		GUILayout.Label (title,EditorStyles.boldLabel,GUILayout.ExpandWidth(true));
		GUILayout.EndHorizontal ();

	}


}
