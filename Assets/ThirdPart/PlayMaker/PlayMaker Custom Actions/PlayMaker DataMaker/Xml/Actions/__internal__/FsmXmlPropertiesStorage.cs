using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Xml;

using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

public class FsmXmlPropertiesStorage : FsmStateAction {
	
	public FsmString[] properties;
	public FsmVar[] propertiesVariables;
	
	
	public void StoreNodeProperties(Fsm fsm,XmlNode node)
	{
		
			int prop_i = 0;
			foreach (FsmString prop in properties) {
				string _property = DataMakerXmlActions.GetNodeProperty(node,prop.Value);
				
				
				PlayMakerUtils.ApplyValueToFsmVar(
				fsm,
				propertiesVariables[prop_i],
				PlayMakerUtils.ParseValueFromString(_property,propertiesVariables[prop_i].Type)
				);
				
				prop_i++;
			}
	}
	
}