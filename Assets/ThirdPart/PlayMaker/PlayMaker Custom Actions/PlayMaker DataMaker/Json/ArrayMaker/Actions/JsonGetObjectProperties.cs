//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using Net.FabreJean.DataMaker;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("json")]
	[Tooltip("Get a one level json object values, subsquent levels are stored as string ( more json objects, or arrays), and can be in turn parsed.")]
	public class JsonGetObjectProperties : CollectionsActions
	{
        [Tooltip("The json raw string")]
		public FsmString jsonString;

		[RequiredField]
		[CompoundArray("Content", "Key", "Value")]

		public FsmString[] keys;
		
		[UIHint(UIHint.Variable)]
		public FsmVar[] values;
		
		
		public FsmEvent successEvent;
		public FsmEvent failureEvent;
		
		
		public override void Reset()
		{
			jsonString = "";
			keys = new FsmString[0];
			values = new FsmVar[0];
			
			successEvent= null;
			failureEvent = null;
		}
		
		public override void OnEnter()
		{

			if (string.IsNullOrEmpty(jsonString.Value))
	        {
	           	LogWarning("json raw string is empty");
				Fsm.Event(failureEvent);
				Finish();
	            return;
	        }
			
			 Hashtable jsonHash;



			try {
				jsonHash =  (Hashtable)JSON.JsonDecode(jsonString.Value);
				 if (jsonHash == null)
		        {
		            LogWarning("json content is null");
		            return;
		        }
				
				
			} catch (System.Exception e) {
				LogError("Json parsing error "+e.Message);
				Fsm.Event(failureEvent);
				Finish();
	            return;
			}
			
			if (jsonHash==null)
			{
				LogError("Json parsing failed ");
				Fsm.Event(failureEvent);
				Finish();
	            return;
			}
			
			
			int i = 0;
			
			foreach(FsmString _key in keys)
			{
				object _val = jsonHash[_key.Value];
				
				if (_val!=null)
				{
					//Debug.Log(_val.GetType().Name);
					
					if ( _val.GetType() == typeof(Hashtable) )
					{
						if (values[i].Type == VariableType.GameObject)
						{
							// we put it into a hashtable with a proper reference to it;
							
							PlayMakerHashTableProxy _proxy = GetHashTableProxyPointer(values[i].gameObjectValue,_key.Value,true);
							if (_proxy!=null)
							{
								Hashtable _table = (Hashtable)_val;
								foreach(DictionaryEntry _entry in _table)
								{
									if (_entry.Value.GetType() == typeof(Hashtable) || _entry.Value.GetType() == typeof(ArrayList) )
									{
										_proxy.hashTable[_entry.Key] =   JSON.JsonEncode(_entry.Value);
									}else{
										_proxy.hashTable[_entry.Key] = _entry.Value;
									}
								}
								
							}
							
						}else if (values[i].Type == VariableType.String)
						{
							PlayMakerUtils.ApplyValueToFsmVar(Fsm,values[i],JSON.JsonEncode(_val));
						}
					}else if ( _val.GetType() == typeof(ArrayList) )
					{
						if (values[i].Type == VariableType.GameObject)
						{
							// we put it into a arraylist with a proper reference to it;
					
							PlayMakerArrayListProxy _proxy = GetArrayListProxyPointer(values[i].gameObjectValue,"",true);
							if (_proxy!=null)
							{
									//	Debug.Log("We are in");
								ArrayList _list = (ArrayList)_val;
								foreach(object _entry in _list)
								{
										//	Debug.Log(_entry);
									if (_entry.GetType() == typeof(Hashtable) || _entry.GetType() == typeof(ArrayList) )
									{
										_proxy.arrayList.Add(JSON.JsonEncode(_entry));
									}else{
										_proxy.arrayList.Add(_entry);
									}
								}
								
							}
							
						}else if (values[i].Type == VariableType.String)
						{
							PlayMakerUtils.ApplyValueToFsmVar(Fsm,values[i],JSON.JsonEncode(_val));
						}
					}
					
					
					else{
						PlayMakerUtils.ApplyValueToFsmVar(Fsm,values[i],_val);
					}
				}
				
				i++;
			}
			
			Fsm.Event(successEvent);
			Finish();
		}


	}
}