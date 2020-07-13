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

	[CustomActionEditor(typeof(SetAgentAreaMask))]
	public class SetAgentAreaMaskCustomEditor : CustomActionEditor
	{

		private PlayMakerNavMeshAreaMaskField _maskField;

	    public override bool OnGUI()
	    {
		
			SetAgentAreaMask _target = (SetAgentAreaMask)target;
			
			bool edited = false;
			
			
			
			EditField("gameObject");

			if (_target.NavMeshAreaMask ==null)
			{
				_target.NavMeshAreaMask =  new FsmInt();
				_target.NavMeshAreaMask.Value = -1;
			}
			
			LayerMask _mask = _target.NavMeshAreaMask.Value;

			if (_maskField==null)
			{
				_maskField = new PlayMakerNavMeshAreaMaskField();
			}
			LayerMask _newMask = _maskField.AreaMaskField("Area Mask",_mask,true);

			if (_newMask!=_mask)
			{
				edited = true;
				_target.NavMeshAreaMask.Value = _newMask.value;
			}

			return GUI.changed || edited;
	    }

	}
}
