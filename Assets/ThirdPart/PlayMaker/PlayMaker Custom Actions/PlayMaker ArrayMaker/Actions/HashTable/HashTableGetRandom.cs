//	(c) Jean Fabre, 2011-2015 All rights reserved.
//	http://www.fabrejean.net
// original action by Terri: http://hutonggames.com/playmakerforum/index.php?topic=835.msg55473;topicseen#new

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/HashTable")]
	[Tooltip("Get a Random key and related value from an hashtable.")]
	public class HashTableGetRandom : HashTableActions
	{
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;
		
		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		public FsmEvent failureEvent;
			
		[ActionSection("Result")]
		
		[UIHint(UIHint.Variable)]
		public FsmString key;
		
		[UIHint(UIHint.Variable)]
		public FsmVar result;
		
		
		private ArrayList _keys;

		
		
		public override void Reset()
		{
		
			gameObject = null;
			reference = null;
	
			failureEvent = null;
			
			result = null;
			
		}
		
		
		public override void OnEnter()
		{

			if ( ! SetUpHashTableProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
			{
				Fsm.Event(failureEvent);	
				Finish();
			}
				
			_keys = new ArrayList(proxy.hashTable.Keys);
					
			DoGetRandom();
			
			Finish();
		}
		

		void DoGetRandom()
		{

			int index = Random.Range(0,_keys.Count);
			
			object element = null;
			
			try{
				element = proxy.hashTable[_keys[index]];
			}catch(System.Exception e){
				Debug.LogError(e.Message);
				Fsm.Event(failureEvent);
				return;
			}
			
			key.Value = (string)_keys[index];
			
			PlayMakerUtils.ApplyValueToFsmVar(Fsm,result,element);
			
		}
		
		
	}
}