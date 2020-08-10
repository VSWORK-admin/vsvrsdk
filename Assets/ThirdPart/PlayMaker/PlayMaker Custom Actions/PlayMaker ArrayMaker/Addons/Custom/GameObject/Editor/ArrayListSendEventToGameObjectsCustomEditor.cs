//	(c) Jean Fabre, 2011-2014 All rights reserved.
//	http://www.fabrejean.net


using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;


[CustomActionEditor(typeof(ArrayListSendEventToGameObjects))]
public class ArrayListSendEventToGameObjectsCustomEditor : CustomActionEditor
{

    public override bool OnGUI()
    {
		EditField("gameObject");
		EditField("reference");
		EditField("sendEvent");
		EditField("excludeSelf");
		EditField("sendToChildren");
		
		return GUI.changed;
    }


}
