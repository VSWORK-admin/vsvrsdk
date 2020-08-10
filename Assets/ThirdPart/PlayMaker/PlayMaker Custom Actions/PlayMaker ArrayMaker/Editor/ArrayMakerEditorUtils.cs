using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
class ArrayMakerEditorUtils
{
	static ArrayMakerEditorUtils ()
	{

		#if ! ARRAYMAKER
			Debug.Log("Setting Up ArrayMaker Scripting define symbol 'ARRAYMAKER'"); 
			PlayMakerEditorUtils.MountScriptingDefineSymbolToAllTargets("ARRAYMAKER");
		#endif

	}
	

}