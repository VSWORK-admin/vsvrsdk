//	(c) Jean Fabre, 2011-2018

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	public abstract class HashTableActions : CollectionsActions  {
		
		internal PlayMakerHashTableProxy proxy;

		
		protected bool SetUpHashTableProxyPointer(GameObject aProxyGO,string nameReference){
			
		//	UnityEngine.Debug.Log(aProxyGO);
			
			if (aProxyGO == null){
				
				return false;
			}
			 proxy = GetHashTableProxyPointer(aProxyGO,nameReference,false);

			return proxy!=null;
		}
		
		// not clever enough to work out how to use <T> properly to have only one function for both hashtable and arrayList...
		protected bool SetUpHashTableProxyPointer(PlayMakerHashTableProxy aProxy,string nameReference){
			
			if (aProxy == null){
				return false;
			}
			 proxy = GetHashTableProxyPointer(aProxy.gameObject,nameReference,false);

			return proxy!=null;
		}
		
		
		protected bool isProxyValid()
		{
					
			if (proxy==null){
				Debug.LogError("HashTable proxy is null");
				return false;
			}			
			if (proxy.hashTable ==null){
				Debug.LogError("HashTable undefined");
				return false;
			}
			
			return true;
		}// isProxyValid
			
	}
}