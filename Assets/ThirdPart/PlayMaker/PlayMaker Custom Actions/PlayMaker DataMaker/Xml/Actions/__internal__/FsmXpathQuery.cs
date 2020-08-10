using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;


public class FsmXpathQuery : FsmStateAction {
	

	public FsmString xPathQuery;
	public FsmVar[] xPathVariables;
	
	// I am hosting editor values cause I can't find a way to serialize them inside my editor since I am generating that editor gui with a static util class
	public bool _foldout = true;
	
	public string parsedQuery;
	
	public string ParseXpathQuery(Fsm fsm)
	{

		parsedQuery = xPathQuery.Value;
		
		if (xPathVariables!=null)
		{
			int i = 0;
			foreach (FsmVar xPathVar in xPathVariables) {
				 
				if (! xPathVar.IsNone)
				{
					parsedQuery = parsedQuery.Replace ("_" + i + "_", PlayMakerUtils.ParseFsmVarToString(fsm,xPathVar)) ;
				}
				i++;
			}
		}
		
		return parsedQuery;
	}


}