// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;

using HutongGames.PlayMaker;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace HutongGames.PlayMakerEditor
{
	
	[CustomActionEditor(typeof(NavMeshSamplePosition))]
	public class NavMeshSamplePositionCustomEditor : CustomActionEditor
	{
		
		private PlayMakerNavMeshAreaMaskField _maskField;
		
		
		public override bool OnGUI()
		{
			
			NavMeshSamplePosition _target = (NavMeshSamplePosition)target;
			
			bool edited = false;
			
			EditField("sourcePosition");
			
			edited = EditMaskField(_target);

			EditField("maxDistance");
		
			EditField("nearestPointFound");
			EditField("nearestPointFoundEvent");
			EditField("nearestPointNotFoundEvent");
			
			EditField("position");
			EditField("normal");
			EditField("distance");
			EditField("mask");
			EditField("hit");
			
			
			return GUI.changed || edited;
		}

		bool EditMaskField(NavMeshSamplePosition _target)
		{
			bool edited = false;

			if (_target.allowedMask ==null)
			{
				_target.allowedMask =  new FsmInt();
				_target.allowedMask.Value = -1;
			}
			
			if (_target.allowedMask.UseVariable)
			{
				EditField("allowedMask");
				
			}else{
				
				GUILayout.BeginHorizontal();
				
				LayerMask _mask = _target.allowedMask.Value;
				
				if (_maskField==null)
				{
					_maskField = new PlayMakerNavMeshAreaMaskField();
				}
				LayerMask _newMask = _maskField.AreaMaskField("Allowed Mask",_mask,true);

				if (_newMask!=_mask)
				{
					edited = true;
					_target.allowedMask.Value = _newMask.value;
				}
				
				if (PlayMakerEditor.FsmEditorGUILayout.MiniButtonPadded(PlayMakerEditor.FsmEditorContent.VariableButton))
				{
					_target.allowedMask.UseVariable = true;
				}
				GUILayout.EndHorizontal();
			}

			return edited;
		}
		
	}
}