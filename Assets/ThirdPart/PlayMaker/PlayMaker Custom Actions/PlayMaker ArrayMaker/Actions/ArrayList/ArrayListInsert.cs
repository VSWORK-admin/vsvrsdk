using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Insert item at a specified index of a PlayMaker ArrayList Proxy component")]
	public class ArrayListInsert : ArrayListActions
	{
			[ActionSection("Set up")]
			
			[RequiredField]
			[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
			[CheckForComponent(typeof(PlayMakerArrayListProxy))]
			public FsmOwnerDefault gameObject;
			
			[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
			public FsmString reference;
			
			[UIHint(UIHint.FsmInt)]
			[Tooltip("The index to remove at")]
			public FsmInt index;
			
			[ActionSection("Data")]
			
			[RequiredField]
			[Tooltip("The variable to add.")]
			public FsmVar variable;
			
			[Tooltip("Ints can be stored as bytes, useful when serializing over network for efficiency")]
			public bool convertIntToByte;

			[ActionSection("Result")]
			
			[UIHint(UIHint.FsmEvent)]
			[Tooltip("The event to trigger if the removeAt throw errors")]
			public FsmEvent failureEvent;
			
			
			public override void Reset()
			{
				gameObject = null;
				reference = null;
				variable = null;
				failureEvent = null;
				index = null;
			}
			
			
			// Code that runs on entering the state.
			public override void OnEnter()
			{
				if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				doArrayListInsert();
				
				Finish();
			}
			
			public void doArrayListInsert()
			{
				if (! isProxyValid()) 
					return;
				
				try
				{
				var _value = PlayMakerUtils.GetValueFromFsmVar(Fsm,variable);
				
				if (variable.Type == VariableType.Int && convertIntToByte)
				{
					proxy.arrayList.Insert(index.Value, System.Convert.ToByte(_value));
				}else{
					proxy.arrayList.Insert(index.Value, _value);
				}

				}catch(System.Exception e){
					Debug.LogError(e.Message);
					Fsm.Event(failureEvent);
				}
				
			}
	
	
	}
	
}
