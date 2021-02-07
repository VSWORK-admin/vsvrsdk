using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class ChargeModeSwitcher : MonoBehaviour {

	private int type=0;
	
	public Text labelType;
	public Button buttonToggleMode;
	
	
	/*
	all the _ChargeMode available in TapDetector
		Once, 
		Clamp, 
		Loop, 
		PingPong,
	*/

	// Use this for initialization
	void Start () {
		buttonToggleMode.onClick.AddListener(delegate { OnToggleButton(); });
		
		type=(int)TapDetector.GetChargeMode();
		
		labelType.text=TapDetector.GetChargeMode().ToString();
	}
	
	
	void OnToggleButton(){
		type+=1;
		if(type>3)type=0;
		
		if(type==0) TapDetector.SetChargeMode(_ChargeMode.Once);
		else if(type==1) TapDetector.SetChargeMode(_ChargeMode.Clamp);
		else if(type==2) TapDetector.SetChargeMode(_ChargeMode.Loop);
		else if(type==3) TapDetector.SetChargeMode(_ChargeMode.PingPong);
		
		labelType.text=TapDetector.GetChargeMode().ToString();
	}
	
	/*
	void OnGUI(){
		
		//string displayText="";
		//if(type==0) displayText=
		
		if(GUI.Button(new Rect(Screen.width-150, 40, 130, 40), TapDetector.GetChargeMode().ToString())){
			type+=1;
			if(type>3)type=0;
			
			if(type==0) TapDetector.SetChargeMode(_ChargeMode.Once);
			else if(type==1) TapDetector.SetChargeMode(_ChargeMode.Clamp);
			else if(type==2) TapDetector.SetChargeMode(_ChargeMode.Loop);
			else if(type==3) TapDetector.SetChargeMode(_ChargeMode.PingPong);
		}
	}
	*/
	
}
