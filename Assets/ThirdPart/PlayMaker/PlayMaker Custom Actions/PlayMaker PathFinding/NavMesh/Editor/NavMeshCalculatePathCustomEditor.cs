// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;

using HutongGames.PlayMaker;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace HutongGames.PlayMakerEditor
{
	
	[CustomActionEditor(typeof(NavMeshCalculatePath))]
	public class NavMeshCalculatePathCustomEditor : CustomActionEditor
	{
		
		private PlayMakerNavMeshAreaMaskField _maskField;
		
		
		public override bool OnGUI()
		{
			
			NavMeshCalculatePath _target = (NavMeshCalculatePath)target;
			
			bool edited = false;

			edited = EditMaskField(_target);

			EditField("sourcePosition");
			EditField("targetPosition");

			EditField("calculatedPathCorners");
			EditField("calculatedPath");

			EditField("resultingPathFound");
			EditField("resultingPathFoundEvent");
			EditField("resultingPathNotFoundEvent");
			
			
			return GUI.changed || edited;
		}
		
		bool EditMaskField(NavMeshCalculatePath _target)
		{
			bool edited = false;
			
			if (_target.passableMask ==null)
			{
				_target.passableMask =  new FsmInt();
				_target.passableMask.Value = -1;
			}
			
			if (_target.passableMask.UseVariable)
			{
				EditField("passableMask");
				
			}else{
				
				GUILayout.BeginHorizontal();
				
				LayerMask _mask = _target.passableMask.Value;
				
				if (_maskField==null)
				{
					_maskField = new PlayMakerNavMeshAreaMaskField();
				}
				LayerMask _newMask = _maskField.AreaMaskField("Passable Mask",_mask,true);
				
				
				if (_newMask!=_mask)
				{
					edited = true;
					_target.passableMask.Value = _newMask.value;
				}
				
				if (PlayMakerEditor.FsmEditorGUILayout.MiniButtonPadded(PlayMakerEditor.FsmEditorContent.VariableButton))
				{
					_target.passableMask.UseVariable = true;
				}
				GUILayout.EndHorizontal();
			}
			
			return edited;
		}
	}
}