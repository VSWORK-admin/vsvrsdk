// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
//
// To Learn about xPath syntax: http://msdn.microsoft.com/en-us/library/ms256471.aspx
//
using UnityEngine;

using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

using System.Xml;

public class FsmXmlProperty : FsmStateAction {
	
	public FsmString property;
	[UIHint(UIHint.Variable)]
	public FsmVar variable;
	
	public static void StoreNodeProperties(Fsm fsm,XmlNode node,FsmXmlProperty[] properties)
	{
		
		int prop_i = 0;
		foreach (FsmXmlProperty prop in properties) {
			
			string _property = DataMakerXmlActions.GetNodeProperty(node,prop.property.Value);
			
			PlayMakerUtils.ApplyValueToFsmVar(
				fsm,
				prop.variable,
				PlayMakerUtils.ParseValueFromString(_property,prop.variable.Type)
				);
			
			prop_i++;
		}
	}
}	
