using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

public class FsmXmlPropertiesTypes : FsmStateAction {
	
	public FsmString[] properties;
	public VariableType[] propertiesTypes;
	
	private Dictionary<string,VariableType> _cache;
	
	public void cacheTypes()
	{
		_cache = new Dictionary<string, VariableType>();
		
		int i = 0;
		foreach(FsmString _prop in properties)
		{
			_cache.Add(_prop.Value,propertiesTypes[i]);
			i++;
		}
	}
	
	public VariableType GetPropertyType(string property)
	{
		
		if (_cache==null)
		{
			cacheTypes();
		}
		
		if (_cache.ContainsKey(property))
		{
			return _cache[property];
		}
		
		return VariableType.Unknown;
	}
	
}