//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Add an item to a PlayMaker Array List Proxy component")]
	public class ArrayListAdd : ArrayListActions
	{

		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;
		
		[ActionSection("Data")]
		
		[RequiredField]
		[Tooltip("The variable to add.")]
		public FsmVar variable;

		[Tooltip("Ints can be stored as bytes, useful when serializing over network for efficiency")]
		public bool convertIntToByte;

		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("The index it was added at")]
		public FsmInt index;


		
		
		
		public override void Reset()
		{
			gameObject = null;
			reference = null;
			variable = null;
			convertIntToByte = false;
			index = null;
		}
		
		
		public override void OnEnter()
		{

			if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				AddToArrayList();
			
			Finish();
		}

		public void AddToArrayList()
		{
			if (! isProxyValid() ) 
				return;

			var _value = PlayMakerUtils.GetValueFromFsmVar(Fsm,variable);

			if (variable.Type == VariableType.Int && convertIntToByte)
			{
				proxy.Add(System.Convert.ToByte(_value),variable.Type.ToString());
			}else{
				proxy.Add(_value,variable.Type.ToString());
			}

			index.Value = proxy.arrayList.Count -1;
		}
		
		
	}
}