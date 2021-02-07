using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class Turret : MonoBehaviour {

	//public GUITexture crosshair;
	public Transform crosshair;
	
	//public GUITexture bar;
	public Slider powerbar;
	public Image barImg;
	
	[Space(10)]
	public Transform turret;
	private Vector2 turretPos; //turret's screen position
	
	//the minimum and maximum elevation of the turret
	public float minElevation=10;
	public float maxElevation=80;
	
	public float maxForce=200; //maximum force, used when the charge is released at maximum
	public GameObject bullet;
	
	
	// Use this for initialization
	void Start () {
		//initiate the turret position on screen
		turretPos = Camera.main.WorldToScreenPoint(turret.position);
		
		//fake a tap event to initiate the turret rotation and cursor
		Tap tap=new Tap(new Vector2(Screen.width/2, Screen.height/2));
		OnTap(tap);
		
		DemoSceneUI.SetSceneTitle("Shoot the target!!");
		DemoSceneUI.SetSceneInstruction("- tap on on screen to set the aim\n- hold down 2 fingers on screen to charge up a fire\n- right click to simulate 2 fingers charge");
	}
	
	void OnEnable(){
		IT_Gesture.onMultiTapE += OnTap;
		
		IT_Gesture.onChargingE += OnCharging;
		IT_Gesture.onChargeEndE += OnChargeEnd;
		
		IT_Gesture.onMFChargingE += OnMFCharging;
		IT_Gesture.onMFChargeEndE += OnMFChargeEnd;
	}
	
	void OnDisable(){
		IT_Gesture.onMultiTapE -= OnTap;
		
		IT_Gesture.onChargingE -= OnCharging;
		IT_Gesture.onChargeEndE -= OnChargeEnd;
		
		IT_Gesture.onMFChargingE -= OnMFCharging;
		IT_Gesture.onMFChargeEndE -= OnMFChargeEnd;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//called when a tap is detected
	void OnTap(Tap tap){
		//if the tap is triggered by mouse, we want only left mouse button
		if(tap.isMouse && tap.index!=0) return;
		
		//get the relative vector between the turret and the tapPos
		Vector2 tempVector=tap.pos-turretPos;
		
		bool recalculateVector=false;
		
		//calculate the angle based on the relative vector
		float angle=Mathf.Asin(tempVector.y/tempVector.magnitude)*Mathf.Rad2Deg;
		
		//if the angle exceed what is permmited
		if(angle<minElevation || angle>maxElevation){
			//clamp the angle to what's within the limited value
			angle=Mathf.Clamp(angle, minElevation, maxElevation);
			//set this flag to true so we know we need to adjust the relative vector to fit the angle
			recalculateVector=true;
		}
		
		//set the turret rotation according to the angle,
		turret.rotation=Quaternion.Euler(-angle, turret.rotation.eulerAngles.y, turret.rotation.eulerAngles.z);
		
		//if the angle value is modified earlier, now modified the relative vector value to fit the angle
		if(recalculateVector){
			tempVector.x=Mathf.Cos(angle*Mathf.Deg2Rad);
			tempVector.y=Mathf.Sin(angle*Mathf.Deg2Rad);
		}
		
		//calculate the cursor position based on the turretPos, 
		//which is basically 100 pixels away from turretPos in the direction of the relative vector
		//Vector2 pos=turretPos+tempVector.normalized*100;
		//crosshair default pixel inset is (-35, -35, 70, 70) and it's transform is positioned at (0, 0, 0)
		//crosshair.pixelInset=new Rect(pos.x-35, pos.y-35, 70, 70);
		
		crosshair.rotation=Quaternion.identity;
	}
	
	//triggered when mouse/single-finger charging event is on-going
	//this is just to simulate 2-fingers charge event using right-mouse-click
	void OnCharging(ChargedInfo cInfo){
		if(cInfo.isMouse){
			if(cInfo.index==1){
				//if this is triggered by right-mouse-button, modified the fingerCount and call OnMFCharging with the same chargeInfo
				cInfo.fingerCount=2;
				OnMFCharging(cInfo);
			}
		}
	}
	
	//triggered when mouse/single-finger charging event is ended
	void OnChargeEnd(ChargedInfo cInfo){
		if(cInfo.isMouse){
			if(cInfo.index==1){
				//if this is triggered by right-mouse-button, modified the fingerCount and call OnMFChargeEnd with the same chargeInfo
				cInfo.fingerCount=2;
				OnMFChargeEnd(cInfo);
			}
		}
	}
	
	//triggered when a multiple finger charge event is on-going
	void OnMFCharging(ChargedInfo cInfo){
		if(cInfo.fingerCount==2){
			//adjust the length of the indicator bar accordingly to the percent
			//bar.pixelInset=new Rect(bar.pixelInset.x, bar.pixelInset.y, cInfo.percent*150, bar.pixelInset.height);
			//adjust the color on the bar
			//bar.color=new Color(cInfo.percent, 1-cInfo.percent, 0);
			
			//adjust the length of the indicator bar accordingly to the percent
			powerbar.value=cInfo.percent;
			//adjust the color on the bar
			barImg.color=new Color(cInfo.percent, 1-cInfo.percent, 0);
		}
	}
	
	//triggered when a multiple finger charge event has ended
	void OnMFChargeEnd(ChargedInfo cInfo){
		if(cInfo.fingerCount==2){
			//reset the bullet
			//adjust the position os it's at the tip of the barrel
			bullet.transform.position=turret.TransformPoint(new Vector3(0, 0, 1.0f));
			//match the bullet rotation to turret's, so that when force is applied, the bullet head in the right direction
			bullet.transform.rotation=turret.rotation;
			//cancel current force on bullet
			Rigidbody rigidbody=bullet.GetComponent<Rigidbody>();
			rigidbody.velocity=Vector3.zero;
			
			//shoot the bullet based on the charged percent
			rigidbody.AddForce(cInfo.percent*maxForce*bullet.transform.forward);
			
			//clear the charge indicator bar
			powerbar.value=0;
			//bar.pixelInset=new Rect(bar.pixelInset.x, bar.pixelInset.y, 0, bar.pixelInset.height);
		}
	}
	
	
	
	/*
	private bool instruction=false;
	void OnGUI(){
		string title="Shoot the target!!";
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
			
			GUI.Box(new Rect(10, 100, 300, 65), "");
			
			GUI.Label(new Rect(15, 105, 290, 65), "tap on on screen to set the aim\nhold down 2 fingers on screen to charge up a fire\nright click to simulate 2 fingers charge");
		}
	}
	*/
	
}






