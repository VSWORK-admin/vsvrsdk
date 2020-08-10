using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
class DataMakerEditorUtils
{
	static DataMakerEditorUtils ()
	{

		#if ! DATAMAKER
			Debug.Log("Setting Up ArrayMaker Scripting define symbol 'DATAMAKER'"); 
			PlayMakerEditorUtils.MountScriptingDefineSymbolToAllTargets("DATAMAKER");
		#endif

	}
	

}