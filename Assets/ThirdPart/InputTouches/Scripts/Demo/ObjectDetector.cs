using UnityEngine;
using System.Collections;

//this is just a simple example to show how the function in IT_Utility can be used
public class ObjectDetector : MonoBehaviour {

	void OnEnable(){
		IT_Gesture.onMultiTapE += OnMultiTap;
	}
	void OnDisable(){
		IT_Gesture.onMultiTapE -= OnMultiTap;
	}
	
	void OnMultiTap(Tap tap){
		bool objDetected=false;
		
		//check if the tap has landed on any UIElement
		if(IT_Utility.IsCursorOnUI(tap.pos)){
			//get the UIElement gameobject the tap landed on
			GameObject objUI=IT_Utility.GetHoveredUIElement(tap.pos);
			Debug.Log("Cursor has landed on an UI element ("+objUI.name+")");
			objDetected=true;
		}
		
		//get the 2D sprite gameobject (uses 2DCollider) the tap landed on
		GameObject obj2D=IT_Utility.GetHovered2DObject(tap.pos);
		if(obj2D!=null){
			Debug.Log("Cursor has landed on a 2D object ("+obj2D.name+")");
			objDetected=true;
		}
		
		//get the 3D gameobject (uses default collider) the tap landed on
		GameObject obj3D=IT_Utility.GetHovered3DObject(tap.pos);
		if(obj3D!=null){
			Debug.Log("Cursor has landed on a 3D object ("+obj3D.name+")");
			objDetected=true;
		}
		
		if(!objDetected) Debug.Log("Cusror has landed on nothing");
	}
	
}
