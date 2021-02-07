using UnityEngine;
using System.Collections;

//using InputTouches;

public class BasicDetector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update(){
		if(Input.touchCount>0){
			//foreach(Touch touch in Input.touches){
			for(int i=0; i<Input.touches.Length; i++){
				Touch touch=Input.touches[i];
				//if(touch.phase==TouchPhase.Began) IT_Gesture.OnTouchDown(touch.position);
				//else if(touch.phase==TouchPhase.Ended) IT_Gesture.OnTouchUp(touch.position);
				//else IT_Gesture.OnTouch(touch.position);
				
				if(touch.phase==TouchPhase.Began) IT_Gesture.OnTouchDown(touch);
				else if(touch.phase==TouchPhase.Ended) IT_Gesture.OnTouchUp(touch);
				else IT_Gesture.OnTouch(touch);
				
				//IT_Gesture.OnTouch(touch);
			}
		}
		else if(Input.touchCount==0){
			//#if !(UNITY_ANDROID || UNITY_IPHONE) || UNITY_EDITOR
			if(Input.GetMouseButtonDown(0)) IT_Gesture.OnMouse1Down(Input.mousePosition);
			else if(Input.GetMouseButtonUp(0)) IT_Gesture.OnMouse1Up(Input.mousePosition);
			else if(Input.GetMouseButton(0)) IT_Gesture.OnMouse1(Input.mousePosition);
			
			if(Input.GetMouseButtonDown(1)) IT_Gesture.OnMouse2Down(Input.mousePosition);
			else if(Input.GetMouseButtonUp(1)) IT_Gesture.OnMouse2Up(Input.mousePosition);
			else if(Input.GetMouseButton(1)) IT_Gesture.OnMouse2(Input.mousePosition);
			//#endif
		}
	}
	

}


