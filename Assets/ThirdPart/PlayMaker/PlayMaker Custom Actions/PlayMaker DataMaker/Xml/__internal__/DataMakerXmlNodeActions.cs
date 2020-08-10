// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.
//
// © 2012 Jean Fabre http://www.fabrejean.net
//
//
using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	public abstract class DataMakerXmlNodeActions: FsmStateAction
	{
		
		internal DataMakerXmlNodeProxy proxy;
		
		/*
		protected DataMakerXmlNodeProxy GetDataMakerXmlNodeProxyPointer (GameObject aProxy, string nameReference, bool silent)
		{
					
			if (aProxy == null) {
				if (!silent)
					Debug.LogError ("Null Proxy");
				return null;
			}
				
			
			DataMakerXmlNodeProxy[] proxies = aProxy.GetComponents<DataMakerXmlNodeProxy> ();
			if (proxies.Length > 1) {
				
				if (nameReference == "") {
					if (!silent)
						Debug.LogError ("Several DataMaker Xml Node Proxies coexists on the same GameObject and no reference is given to find the expected DataMaker Xml Node Proxy");
				}
					
				foreach (DataMakerXmlNodeProxy iProxy in proxies) {
					if (iProxy.referenceName == nameReference) {
						return iProxy;
					}
				}
	
				if (nameReference != "") {
					if (!silent)
						LogError ("DataMaker Xml Node Proxy not found for reference <" + nameReference + ">");
					return null;
				}
						
			} else if (proxies.Length > 0) {
				if (nameReference != "" && nameReference != proxies [0].referenceName) {
					if (!silent)
						Debug.LogError ("DataMaker Xml node Proxy reference do not match");
					return null;
				}
					
				return proxies [0];
						
			}
				
			if (!silent) {
				LogError ("XmlMaker proxy not found");
			}
			return null;
		}// GetDataMakerXmlProxyPointer
		
		*/
		
		protected bool SetUpDataMakerXmlNodeProxyPointer (GameObject aProxyGO, string nameReference)
		{
				
			if (aProxyGO == null) {
				return false;
			}
			
			proxy = DataMakerCore.GetDataMakerProxyPointer(typeof(DataMakerXmlNodeProxy), aProxyGO, nameReference, false) as DataMakerXmlNodeProxy;

			return proxy != null;
		}

		public bool isProxyValid ()
		{
						
			if (proxy == null) {
				LogError ("DataMaker Xml Node proxy is null");
				return false;
			}
				
			return true;
		}// isProxyValid
		
	}
}