using UnityEngine;
using System.Collections;

public class OrbitCam : MonoBehaviour {

	public Vector3 center=Vector3.zero;
	public float dist;
	
	private Vector2 orbitSpeed;
	private float rotateSpeed;
	private float zoomSpeed;
	
	public float orbitSpeedModifier=1;
	public float zoomSpeedModifier=5;
	public float rotateSpeedModifier=1;
	

	// Use this for initialization
	void Start () {
		dist=transform.position.z;
		
		DragInfo dInfo=new DragInfo(Vector2.zero, new Vector2(1000, 1000), 1);
		OnDragging(dInfo);
		
		orbitSpeed=Vector2.zero;
		
		
		DemoSceneUI.SetSceneTitle("Free orbit camera, the camera will orbit around the object");
		
		string instInfo="";
			instInfo+="- swipe or drag on screen to rotate the camera in x and y-axis\n";
			instInfo+="- pinch or using mouse wheel to zoom in/out\n";
			instInfo+="- rotate two fingers on screen to rotate the camera in z-axis\n";
			instInfo+="- single finger interaction can be simulate using left mosue button\n";
		DemoSceneUI.SetSceneInstruction(instInfo);
	}
	
	void OnEnable(){
		IT_Gesture.onDraggingE += OnDragging;
		IT_Gesture.onRotateE += OnRotate;
		IT_Gesture.onPinchE += OnPinch;
	}
	
	void OnDisable(){
		IT_Gesture.onDraggingE -= OnDragging;
		IT_Gesture.onRotateE += OnRotate;
		IT_Gesture.onPinchE += OnPinch;
	}
	
	// Update is called once per frame
	void Update () {
		//calculate the distance from center based on zoomSpeed
		dist+=Time.deltaTime*zoomSpeed*0.01f;
		//clamp the distance so it's within permmited range
		dist=Mathf.Clamp(dist, -15, -3);
		
		//set the camera rotation back to center, so we can calculate things from this point
		transform.position=center;
		//apply the rotation based on the current orbit and rotate speed
		transform.rotation*=Quaternion.Euler(-orbitSpeed.y, orbitSpeed.x, rotateSpeed);
		//apply the distance
		transform.position=transform.TransformPoint(new Vector3(0, 0, dist));
		
		//slow down all the speed
		orbitSpeed*=(1-Time.deltaTime*3);
		rotateSpeed*=(1-Time.deltaTime*4f);
		zoomSpeed*=(1-Time.deltaTime*4);
		
		//use mouse scroll wheel to simulate pinch, sorry I sort of cheated here
		zoomSpeed+=Input.GetAxis("Mouse ScrollWheel")*500*zoomSpeedModifier;
	}
	
	
	//rotate the object instead
	/*
	void Update(){
		Vector3 orbitS=new Vector3(orbitSpeed.y, -orbitSpeed.x, 0);
		transform.Rotate(orbitS, Space.World);
		
		//slow down all the speed
		orbitSpeed*=(1-Time.deltaTime*3);
		rotateSpeed*=(1-Time.deltaTime*4f);
		zoomSpeed*=(1-Time.deltaTime*4);
	}
	*/
	
	void OnDragging(DragInfo dragInfo){
		orbitSpeed=dragInfo.delta*orbitSpeedModifier/IT_Gesture.GetDPIFactor();
	}
	
	void OnRotate(RotateInfo rinfo){
		rotateSpeed=Mathf.Lerp(rotateSpeed, rinfo.magnitude*rotateSpeedModifier/IT_Gesture.GetDPIFactor(), 0.75f);
	}
	
	void OnPinch(PinchInfo pinfo){
		zoomSpeed-=pinfo.magnitude*zoomSpeedModifier/IT_Gesture.GetDPIFactor();
	}
	
	
	/*
	private bool instruction=false;
	void OnGUI(){
		string title="free orbit camera, the camera will orbit around the object";
		GUI.Label(new Rect(150, 15, 500, 40), title);
		
		if(!instruction){
			if(GUI.Button(new Rect(10, 55, 130, 35), "Instruction On")){
				instruction=true;
			}
		}
		else{
			if(GUI.Button(new Rect(10, 55, 130, 35), "Instruction Off")){
				instruction=false;
			}
			
			GUI.Box(new Rect(10, 100, 400, 80), "");
			
			string instInfo="";
			instInfo+="- swipe or drag on screen to rotate the camera in x and y-axis\n";
			instInfo+="- pinch or using mouse wheel to zoom in/out\n";
			instInfo+="- rotate two fingers on screen to rotate the camera in z-axis\n";
			instInfo+="- single finger interaction can be simulate using left mosue button\n";
			
			GUI.Label(new Rect(15, 105, 390, 70), instInfo);
		}
	}
	*/
	
}

