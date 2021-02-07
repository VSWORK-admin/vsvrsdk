using UnityEngine;
using System.Collections;

public class RTSCam : MonoBehaviour {

	private float dist;
	
	private float orbitSpeedX;
	private float orbitSpeedY;
	private float zoomSpeed;
	
	public float rotXSpeedModifier=0.25f;
	public float rotYSpeedModifier=0.25f;
	public float zoomSpeedModifier=5;
	
	public float minRotX=-20;
	public float maxRotX=70;
	
	//public float minRotY=45;
	//public float maxRotY=315;
	
	public float minZoom=3;
	public float maxZoom=15;
	
	public float panSpeedModifier=1f;
	
	// Use this for initialization
	void Start () {
		dist=transform.localPosition.z;
		
		DemoSceneUI.SetSceneTitle("RTS camera, the camera will orbit around a pivot point but the rotation in z-axis is locked");
		
		string instInfo="";
			instInfo+="- swipe or drag on screen to rotate the camera\n";
			instInfo+="- pinch or using mouse wheel to zoom in/out\n";
			instInfo+="- swipe or drag on screen with 2 fingers to move around\n";
			instInfo+="- single finger interaction can be simulate using left mosue button\n";
			instInfo+="- two fingers interacion can be simulate using right mouse button";
		DemoSceneUI.SetSceneInstruction(instInfo);
	}
	
	void OnEnable(){
		IT_Gesture.onDraggingE += OnDragging;
		IT_Gesture.onMFDraggingE += OnMFDragging;
		
		IT_Gesture.onPinchE += OnPinch;
		
		orbitSpeedX=0;
		orbitSpeedY=0;
		zoomSpeed=0;
	}
	
	void OnDisable(){
		IT_Gesture.onDraggingE -= OnDragging;
		IT_Gesture.onMFDraggingE -= OnMFDragging;
		
		IT_Gesture.onPinchE -= OnPinch;
	}

	
	// Update is called once per frame
	void Update () {
		//get the current rotation
		float x=transform.parent.rotation.eulerAngles.x;
		float y=transform.parent.rotation.eulerAngles.y;
		
		//make sure x is between -180 to 180 so we can clamp it propery later
		if(x>180) x-=360;
		
		//calculate the x and y rotation
		//Quaternion rotationY=Quaternion.Euler(0, Mathf.Clamp(y+orbitSpeedY, minRotY, maxRotY), 0);
		Quaternion rotationY=Quaternion.Euler(0, y+orbitSpeedY, 0);
		Quaternion rotationX=Quaternion.Euler(Mathf.Clamp(x+orbitSpeedX, minRotX, maxRotX), 0, 0);
		
		//apply the rotation
		transform.parent.rotation=rotationY*rotationX;
		
		//calculate the zoom and apply it
		dist+=Time.deltaTime*zoomSpeed*0.01f;
		dist=Mathf.Clamp(dist, -maxZoom, -minZoom);
		transform.localPosition=new Vector3(0, 0, dist);
		
		//reduce all the speed
		orbitSpeedX*=(1-Time.deltaTime*12);
		orbitSpeedY*=(1-Time.deltaTime*3);
		zoomSpeed*=(1-Time.deltaTime*4);
		
		//use mouse scroll wheel to simulate pinch, sorry I sort of cheated here
		zoomSpeed+=Input.GetAxis("Mouse ScrollWheel")*500*zoomSpeedModifier;
	}
	
	//called when one finger drag are detected
	void OnDragging(DragInfo dragInfo){
		//if the drag is perform using mouse2, use it as a two finger drag
		if(dragInfo.isMouse && dragInfo.index==1) OnMFDragging(dragInfo);
		//else perform normal orbiting
		else{
			//apply the DPI scaling
			dragInfo.delta/=IT_Gesture.GetDPIFactor();
			//vertical movement is corresponded to rotation in x-axis
			orbitSpeedX=-dragInfo.delta.y*rotXSpeedModifier;
			//horizontal movement is corresponded to rotation in y-axis
			orbitSpeedY=dragInfo.delta.x*rotYSpeedModifier;
		}
	}
	
	//called when pinch is detected
	void OnPinch(PinchInfo pinfo){
		zoomSpeed-=pinfo.magnitude*zoomSpeedModifier/IT_Gesture.GetDPIFactor();
	}
	
	//called when a dual finger or a right mouse drag is detected
	void OnMFDragging(DragInfo dragInfo){
		//apply the DPI scaling
		dragInfo.delta/=IT_Gesture.GetDPIFactor();
		
		//make a new direction, pointing horizontally at the direction of the camera y-rotation
		Quaternion direction=Quaternion.Euler(0, transform.parent.rotation.eulerAngles.y, 0);
		
		//calculate forward movement based on vertical input
		Vector3 moveDirZ=transform.parent.InverseTransformDirection(direction*Vector3.forward*-dragInfo.delta.y);
		//calculate sideway movement base on horizontal input
		Vector3 moveDirX=transform.parent.InverseTransformDirection(direction*Vector3.right*-dragInfo.delta.x);
		
		//move the cmera 
		transform.parent.Translate(moveDirZ * panSpeedModifier * Time.deltaTime);
		transform.parent.Translate(moveDirX * panSpeedModifier * Time.deltaTime);
	}
	
	
	
	//for the camera to auto rotate and focus on a predefined position
	/*
	public float targetRotX;
	public float targetRotY;
	public float targetRotZ;
	public Vector3 targetPos;
	public float targetZoom;
	
	IEnumerator LerpToPoint(){
		Quaternion startRot=transform.parent.rotation;
		Quaternion endRot=Quaternion.Euler(targetRotX, targetRotY, targetRotZ);
		
		Vector3 startPos=transform.parent.position;
		Vector3 startZoom=transform.localPosition;
		
		float duration=0;
		while(duration<1){
			transform.parent.rotation=Quaternion.Lerp(startRot, endRot, duration);
			transform.parent.position=Vector3.Lerp(startPos, targetPos, duration);
			transform.localPosition=Vector3.Lerp(startZoom, new Vector3(0, 0, -targetZoom), duration);
			duration+=Time.deltaTime;
			yield return null;
		}
		
		transform.parent.rotation=endRot;
		transform.parent.position=targetPos;
		transform.localPosition=new Vector3(0, 0, -targetZoom);
	}
	*/
	
	
	/*
	private bool instruction=false;
	void OnGUI(){
		string title="RTS camera, the camera will orbit around a pivot point but the rotation in z-axis is locked.";
		GUI.Label(new Rect(150, 10, 400, 60), title);
		
		if(!instruction){
			if(GUI.Button(new Rect(10, 55, 130, 35), "Instruction On")){
				instruction=true;
			}
		}
		else{
			if(GUI.Button(new Rect(10, 55, 130, 35), "Instruction Off")){
				instruction=false;
			}
			
			GUI.Box(new Rect(10, 100, 400, 100), "");
			
			string instInfo="";
			instInfo+="- swipe or drag on screen to rotate the camera\n";
			instInfo+="- pinch or using mouse wheel to zoom in/out\n";
			instInfo+="- swipe or drag on screen with 2 fingers to move around\n";
			instInfo+="- single finger interaction can be simulate using left mosue button\n";
			instInfo+="- two fingers interacion can be simulate using right mouse button";
			
			GUI.Label(new Rect(15, 105, 390, 90), instInfo);
		}
	}
	*/
	
}
