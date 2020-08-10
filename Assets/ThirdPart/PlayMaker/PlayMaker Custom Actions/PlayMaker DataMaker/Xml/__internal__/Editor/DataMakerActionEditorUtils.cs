using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;

using HutongGames.PlayMaker;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HutongGames.PlayMaker.Ecosystem.Utils;

public class DataMakerActionEditorUtils {

	
	public static void EditHashSetField(Fsm fsm,FsmString key,FsmString content)
	{
		key = VariableEditor.FsmStringField(new GUIContent("Property"),fsm,key,null);
		content = VariableEditor.FsmStringField(new GUIContent("Value"),fsm,content,null);
	}
	
	
	public static bool EditFsmXpathQueryField(Fsm fsm,FsmXpathQuery target)
	{
		bool edited =false;
		
		target._foldout = FsmEditorGUILayout.BoldFoldout(target._foldout,new GUIContent("xPath Query"));
		
		if (target.xPathQuery==null)
		{
			target.xPathQuery = new FsmString();
		}
		
		if (target._foldout)
		{
			#if PLAYMAKER_1_8_OR_NEWER
			PlayMakerInspectorUtils.SetActionEditorVariableSelectionContext(target,target.GetType().GetField("xPathQuery"));
			#endif


			target.xPathQuery = VariableEditor.FsmStringField(new GUIContent("xPath Query"),fsm,target.xPathQuery,null);
		}
		
		if (string.IsNullOrEmpty(target.xPathQuery.Value))
		{
			
		}else{
			if (target.xPathVariables==null || target.xPathVariables.Length==0)
			{
				if (!target._foldout)
				{
					EditorGUILayout.LabelField("xPath Query",target.xPathQuery.Value);
				}
			}else{
				EditorGUILayout.LabelField("xPath Query parsed",target.ParseXpathQuery(fsm));
			}
		}
		if (target._foldout)
		{	
			edited = edited || EditFsmXpathQueryVariablesProperties(fsm,target);
		}

		
		return edited;
	}
	
	public static bool EditFsmXpathQueryVariablesProperties(Fsm fsm,FsmXpathQuery target)
	{
	
		if (target==null)
		{
			target = new FsmXpathQuery();
		}
		
		bool edited = false;
		
		int count = 0;
		
		if (target.xPathVariables !=null)
		{
			count = target.xPathVariables.Length;
			
			for(int i=0;i<count;i++)
			{
				
					
				GUILayout.BeginHorizontal();

				bool fsmVariableChangedFlag;
				target.xPathVariables[i] = PlayMakerInspectorUtils.EditorGUILayout_FsmVarPopup("Variable _"+i+"_",fsm.Variables.GetAllNamedVariables(),target.xPathVariables[i],out fsmVariableChangedFlag);

				// PlayMaker api is now not working on 1.8 and shoudl become private
				//target.xPathVariables[i] = VariableEditor.FsmVarPopup(new GUIContent("Variable _"+i+"_"),fsm,target.xPathVariables[i]);

				edited = edited || fsmVariableChangedFlag;

				if (i+1==count)
				{
					if (FsmEditorGUILayout.DeleteButton())
					{
						ArrayUtility.RemoveAt(ref target.xPathVariables,i);
						return true; // we must not continue, an entry is going to be deleted so the loop is broken here. next OnGui, all will be well.
					}
				}else{
					GUILayout.Space(21);
				}
				GUILayout.EndHorizontal();
				
			}	
		}
		
		string _addButtonLabel = "Add a variable";
		
		if (count>0)
		{
			_addButtonLabel = "Add another variable";
		}
		
		GUILayout.BeginHorizontal();
			GUILayout.Space(154);
		
			if ( GUILayout.Button(_addButtonLabel) )
			{		
				
				if (target.xPathVariables==null)
				{
					target.xPathVariables = new FsmVar[0];
				}
				
				
				ArrayUtility.Add<FsmVar>(ref target.xPathVariables, new FsmVar());
				edited = true;	
			}
			GUILayout.Space(21);
		GUILayout.EndHorizontal();
		
		return edited || GUI.changed;
	}
	

	
	public static bool EditFsmXmlSourceField(Fsm fsm,FsmXmlSource source)
	{
		
		source.sourceSelection = EditorGUILayout.Popup("Source selection",source.sourceSelection,source.sourceTypes);
			
		if (source.sourceString==null)
		{
			source.sourceString = new FsmString();
		}

		//force use variable if necessary
		if (!source.sourceString.UseVariable && source.sourceSelection==2)
		{
			source.sourceString.UseVariable = true;
		}

		bool showPreview = false;
		string preview = "";
		
		if (source.sourceSelection==0)
		{

			source._sourceEdit = EditorGUILayout.Foldout(source._sourceEdit,new GUIContent("Edit"));
			if (source._sourceEdit)
			{
				source.sourceString.Value = EditorGUILayout.TextArea(source.sourceString.Value,GUILayout.Height(200));
			}
			
		}else if (source.sourceSelection==1)
		{
			source.sourcetextAsset = (TextAsset)EditorGUILayout.ObjectField("TextAsset Object",source.sourcetextAsset,typeof(TextAsset),false);
			if (source.sourcetextAsset!=null)
			{
				source._sourcePreview = EditorGUILayout.Foldout(source._sourcePreview,new GUIContent("Preview"));
				showPreview = source._sourcePreview;
				preview = source.sourcetextAsset.text;
			}
		}else if (source.sourceSelection==2)
		{
		
			#if PLAYMAKER_1_8_OR_NEWER
				PlayMakerInspectorUtils.SetActionEditorVariableSelectionContext(source,source.GetType().GetField("sourceString"));
			#endif

			source.sourceString = VariableEditor.FsmStringField(new GUIContent("Fsm String"),fsm,source.sourceString,null);
		
			if (!source.sourceString.UseVariable)
			{
				source.sourceSelection=0;
				return true;
			}

			if (!source.sourceString.IsNone)
			{
				source._sourcePreview = EditorGUILayout.Foldout(source._sourcePreview,new GUIContent("Preview"));
				showPreview = source._sourcePreview;
				preview = source.sourceString.Value;
			}


		}else if (source.sourceSelection==3)
		{
			if (source.sourceProxyGameObject ==null)
			{
				source.sourceProxyGameObject = new FsmGameObject();
				source.sourceProxyReference = new FsmString();
			}

			#if PLAYMAKER_1_8_OR_NEWER
				PlayMakerInspectorUtils.SetActionEditorVariableSelectionContext(source,source.GetType().GetField("sourceProxyGameObject"));
			#endif
			source.sourceProxyGameObject = VariableEditor.FsmGameObjectField(new GUIContent("GameObject"),fsm,source.sourceProxyGameObject);

			#if PLAYMAKER_1_8_OR_NEWER
				PlayMakerInspectorUtils.SetActionEditorVariableSelectionContext(source,source.GetType().GetField("sourceProxyReference"));
			#endif

			source.sourceProxyReference = VariableEditor.FsmStringField(new GUIContent("Reference"),fsm,source.sourceProxyReference,null);
			
			if (source.sourceProxyGameObject!=null)
			{

				DataMakerXmlProxy proxy =  DataMakerCore.GetDataMakerProxyPointer(typeof(DataMakerXmlProxy), source.sourceProxyGameObject.Value, source.sourceProxyReference.Value, true) as DataMakerXmlProxy;
				if (proxy!=null)
				{
					if (proxy.XmlTextAsset!=null)
					{
						source._sourcePreview = EditorGUILayout.Foldout(source._sourcePreview,new GUIContent("Preview"));
						showPreview = source._sourcePreview;
						preview = proxy.XmlTextAsset.text;
					}else{
						//oupss...
					}
				}else{
					//oupss..
				}
			}
		}else if (source.sourceSelection ==4)
		{
			if (source.inMemoryReference==null)
			{
				source.inMemoryReference = new FsmString();
			}

			#if PLAYMAKER_1_8_OR_NEWER
				PlayMakerInspectorUtils.SetActionEditorVariableSelectionContext(source,source.GetType().GetField("inMemoryReference"));
			#endif
			source.inMemoryReference = VariableEditor.FsmStringField(new GUIContent("Memory Reference"),fsm,source.inMemoryReference,null);
			
			if (!string.IsNullOrEmpty(source.inMemoryReference.Value) )
			{
				source._sourcePreview = EditorGUILayout.Foldout(source._sourcePreview,new GUIContent("Preview"));
				showPreview = source._sourcePreview;
				preview = DataMakerXmlUtils.XmlNodeToString(DataMakerXmlUtils.XmlRetrieveNode(source.inMemoryReference.Value));
			}
		}


		if (showPreview)
		{
			if (string.IsNullOrEmpty(preview))
			{
				GUILayout.Label("-- empty --");
			}else{
				source._scroll = DataMakerEditorGUILayoutUtils.StringContentPreview(source._scroll,preview);

				/*
				source._scroll = GUILayout.BeginScrollView(source._scroll,"box", GUILayout.Height (200));
				GUI.skin.box.alignment = TextAnchor.UpperLeft;
				GUILayout.Box(preview,"Label",null);
				GUILayout.EndScrollView();
				*/
			}
		}
		
		
		return false;
	}
	
	
	public static bool EditFsmPropertiesStorage(Fsm fsm,FsmXmlPropertiesStorage target)
	{
		
		FsmEditorGUILayout.LightDivider();

		
		bool edited = false;
		
		int count = 0;
		
		if (target !=null && target.properties !=null &&  target.propertiesVariables !=null)
		{
			count = target.properties.Length;
			
		
			for(int i=0;i<count;i++)
			{
				
				GUILayout.BeginHorizontal();
				
					GUILayout.Label("Property item "+i);
					GUILayout.FlexibleSpace();
		
				
					if (FsmEditorGUILayout.DeleteButton())
					{
						ArrayUtility.RemoveAt(ref target.properties,i);
						ArrayUtility.RemoveAt(ref target.propertiesVariables,i);
						return true; // we must not continue, an entry is going to be deleted so the loop is broken here. next OnGui, all will be well.
					}
				
				GUILayout.EndHorizontal();

				#if PLAYMAKER_1_8_OR_NEWER
//				PlayMakerInspectorUtils.SetActionEditorArrayVariableSelectionContext(target,i,target.GetType().GetField("properties").GetType());
				#endif

			

				target.properties[i] = VariableEditor.FsmStringField(new GUIContent("Property"),fsm,target.properties[i],null);
				if (target.properties[i].UseVariable)
				{
					DataMakerEditorGUILayoutUtils.feedbackLabel("Using variables not supported, Check changeLog for infos",DataMakerEditorGUILayoutUtils.labelFeedbacks.ERROR);

				}
			//	target.propertiesVariables[i] = VariableEditor.FsmVarPopup(new GUIContent("Value"),fsm,target.propertiesVariables[i]);
				bool fsmVariableChangedFlag =false;
				target.propertiesVariables[i] = PlayMakerInspectorUtils.EditorGUILayout_FsmVarPopup("Value",fsm.Variables.GetAllNamedVariables(),target.propertiesVariables[i],out fsmVariableChangedFlag);

			}	
		}
		
		string _addButtonLabel = "Get a Property";
		
		if (count>0)
		{
			_addButtonLabel = "Get another Property";
		}
		
		GUILayout.BeginHorizontal();
			GUILayout.Space(154);
		
			if ( GUILayout.Button(_addButtonLabel) )
			{		
				
				if (target.properties==null)
				{
					target.properties = new FsmString[0];
					target.propertiesVariables = new FsmVar[0];
				}
				
				
				ArrayUtility.Add<FsmString>(ref target.properties, new FsmString());
				ArrayUtility.Add<FsmVar>(ref target.propertiesVariables,new FsmVar());
				edited = true;	
			}
			GUILayout.Space(21);
		GUILayout.EndHorizontal();
		
		return edited || GUI.changed;
	}
	
	public static bool EditFsmXmlPropertiesTypes(Fsm fsm,FsmXmlPropertiesTypes target)
	{
		
		FsmEditorGUILayout.LightDivider();

		
		bool edited = false;
		
		int count = 0;
		
		if (target.properties !=null &&  target.propertiesTypes !=null)
		{
			count = target.properties.Length;

			
			#if PLAYMAKER_1_8_OR_NEWER
			// TODO: TOFIX: pointing to a fsm variable to define a property is not working.
			//FieldInfo _fsmStringArray_FieldInfo = target.GetType().GetField("properties");

			#endif

		
			for(int i=0;i<count;i++)
			{
				
				GUILayout.BeginHorizontal();
				
					GUILayout.Label("Property item "+i);
					GUILayout.FlexibleSpace();
		
				
					if (FsmEditorGUILayout.DeleteButton())
					{
						ArrayUtility.RemoveAt(ref target.properties,i);
						ArrayUtility.RemoveAt(ref target.propertiesTypes,i);
						return true; // we must not continue, an entry is going to be deleted so the loop is broken here. next OnGui, all will be well.
					}
				
				GUILayout.EndHorizontal();

				#if PLAYMAKER_1_8_OR_NEWER
					
				//	PlayMakerInspectorUtils.SetActionEditorArrayVariableSelectionContext(target,i,_fsmStringArray_FieldInfo);
				#endif

				target.properties[i] = VariableEditor.FsmStringField(new GUIContent("Property"),fsm,target.properties[i],null);
				target.propertiesTypes[i] = (VariableType)EditorGUILayout.EnumPopup(new GUIContent("Type"),target.propertiesTypes[i]);
			}	
		}
		
		string _addButtonLabel = "Define a Property";
		
		if (count>0)
		{
			_addButtonLabel = "Define another Property";
		}
		
		GUILayout.BeginHorizontal();
			GUILayout.Space(154);
		
			if ( GUILayout.Button(_addButtonLabel) )
			{		
				
				if (target.properties==null)
				{
					target.properties = new FsmString[0];
					target.propertiesTypes = new VariableType[0];
				}
				
				
				ArrayUtility.Add<FsmString>(ref target.properties, new FsmString());
				ArrayUtility.Add<VariableType>(ref target.propertiesTypes,VariableType.Float);
				edited = true;	
			}
			GUILayout.Space(21);
		GUILayout.EndHorizontal();
		
		return edited || GUI.changed;
	}
	
}
