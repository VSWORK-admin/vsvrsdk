using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	private float dist;
	
	private float orbitSpeedX;
	private float orbitSpeedY;
	private float zoomSpeed;
	
	public float rotXSpeedModifier=0.25f;
	public float rotYSpeedModifier=0.25f;
	public float zoomSpeedModifier=5;
	
	public float minRotX=-20;
	public float maxRotX=70;
	
	public float panSpeedModifier=1f;
	
	// Use this for initialization
	void Start () {
		dist=transform.localPosition.z;
	}
	
	void OnEnable(){
		IT_Gesture.onDraggingE += OnDragging;
		//IT_Gesture.onMFDraggingE += OnMFDragging;
		
		//~ IT_Gesture.onPinchE += OnPinch;
		
		orbitSpeedX=0;
		orbitSpeedY=0;
		zoomSpeed=0;
	}
	
	void OnDisable(){
		IT_Gesture.onDraggingE -= OnDragging;
		//~ IT_Gesture.onMFDraggingE -= OnMFDragging;
		
		//~ IT_Gesture.onPinchE -= OnPinch;
	}

	
	// Update is called once per frame
	void Update () {
		//get the current rotation
		float x=transform.rotation.eulerAngles.x;
		float y=transform.rotation.eulerAngles.y;
		
		//make sure x is between -180 to 180 so we can clamp it propery later
		//~ if(x>180) x-=360;
		
		//calculate the x and y rotation
		Quaternion rotationY=Quaternion.Euler(0, y, 0)*Quaternion.Euler(0, orbitSpeedY, 0);
		Quaternion rotationX=Quaternion.Euler(Mathf.Clamp(x+orbitSpeedX, minRotX, maxRotX), 0, 0);
		
		//apply the rotation
		transform.rotation=rotationY*rotationX;
		
		//calculate the zoom and apply it
		dist+=Time.deltaTime*zoomSpeed*0.01f;
		dist=Mathf.Clamp(dist, -15, -3);
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
			//vertical movement is corresponded to rotation in x-axis
			orbitSpeedX=-dragInfo.delta.y*rotXSpeedModifier;
			//horizontal movement is corresponded to rotation in y-axis
			orbitSpeedY=dragInfo.delta.x*rotYSpeedModifier;
		}
	}
	
	//called when pinch is detected
	void OnPinch(PinchInfo pinfo){
		zoomSpeed-=pinfo.magnitude*zoomSpeedModifier;
	}
	
	//called when a dual finger or a right mouse drag is detected
	void OnMFDragging(DragInfo dragInfo){
		
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
	
}
