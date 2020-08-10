// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.
//
// © 2012 Jean Fabre http://www.fabrejean.net
//
//
using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	public abstract class DataMakerXmlNodeListActions: FsmStateAction
	{
		
		internal DataMakerXmlNodeListProxy proxy;
		

		protected bool SetUpDataMakerXmlNodeListProxyPointer (GameObject aProxyGO, string nameReference)
		{
				
			if (aProxyGO == null) {
				return false;
			}
			
			proxy = DataMakerCore.GetDataMakerProxyPointer(typeof(DataMakerXmlNodeListProxy), aProxyGO, nameReference, false) as DataMakerXmlNodeListProxy;

			return proxy != null;
		}

		public bool isProxyValid ()
		{
						
			if (proxy == null) {
				LogError ("DataMaker Xml Node List proxy is null");
				return false;
			}
				
			return true;
		}// isProxyValid
		
	}
}