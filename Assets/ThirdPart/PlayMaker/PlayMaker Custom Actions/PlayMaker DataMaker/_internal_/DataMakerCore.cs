using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataMakerCore {
	
	// Generic function to get a DataMaker proxy.
	static public DataMakerProxyBase GetDataMakerProxyPointer<T>(T type,GameObject aProxy, string nameReference, bool silent)
	{
		if (aProxy == null) {
			if (!silent)
				Debug.LogError ("Null Proxy");
			return null;
		}
			
		// get all proxies regardless of the specific type
		// since T can not be used with GetComponents as it expectd a MonoType.
		DataMakerProxyBase[] proxies = aProxy.GetComponents<DataMakerProxyBase>();
		
		// now filter by type T
		List<DataMakerProxyBase> proxies_T = new List<DataMakerProxyBase>();
		
		foreach(DataMakerProxyBase proxy in proxies)
		{
			if (proxy.GetType().Equals(type))
			{
				proxies_T.Add(proxy);
			}
		}
		
		// and back as an array. Now we have only proxies of type T
		proxies = proxies_T.ToArray();
		
		if (proxies.Length > 1) {
			
			if (nameReference == "") {
				if (!silent)
					Debug.LogError ("Several "+type+" coexists on the same GameObject and no reference is given to find the expected "+type);
			}
				
			foreach (DataMakerProxyBase iProxy in proxies) {
				if (iProxy.referenceName == nameReference) {
					return iProxy;
				}
			}

			if (nameReference != "") {
				if (!silent)
					Debug.LogError (type+" not found for reference <" + nameReference + ">");
				return null;
			}
					
		} else if (proxies.Length > 0) {
			if (nameReference != "" && nameReference != proxies [0].referenceName) {
				if (!silent)
					Debug.LogError (type+" reference do not match");
				return null;
			}
				
			return proxies [0];
					
		}
			
		if (!silent) {
			Debug.LogError (type+" not found");
		}
		return null;
	}

}
