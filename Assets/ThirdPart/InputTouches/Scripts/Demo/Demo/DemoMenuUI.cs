using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class DemoMenuUI : MonoBehaviour {
	
	public List<Button> buttonList=new List<Button>();
	
	void Start(){
		for(int i=0; i<buttonList.Count; i++){
			int idx=i;
			buttonList[i].onClick.AddListener(delegate { OnLevelButton(idx); });
		}
	}
	
	
	void OnLevelButton(int idx){
		if(idx==0)			SceneManager.LoadScene("RTSCam");
		else if(idx==1)	SceneManager.LoadScene("OrbitCam");
		else if(idx==2)	SceneManager.LoadScene("SwipeDemo");
		else if(idx==3)	SceneManager.LoadScene("TapDemo");
		else if(idx==4)	SceneManager.LoadScene("TurretDemo");
		else if(idx==5)	SceneManager.LoadScene("FlickDemo");
		else if(idx==6)	SceneManager.LoadScene("DPad");
	}
	
	
	/*
	void OnGUI(){
		int startX=Screen.width/2-210;
		int startY=Screen.height/2-70;
		int width=200;
		int height=40;
		int spaceY=50;
		
		if(GUI.Button(new Rect(startX, startY, width, height), "RTS Camera")){
			SceneManager.LoadScene("RTSCam");
		}
		if(GUI.Button(new Rect(startX, startY+=spaceY, width, height), "Orbit Camera")){
			SceneManager.LoadScene("OrbitCam");
		}
		if(GUI.Button(new Rect(startX, startY+=spaceY, width, height), "Swipe Example")){
			SceneManager.LoadScene("SwipeDemo");
		}
		if(GUI.Button(new Rect(startX, startY+=spaceY, width, height), "General Tap/Click")){
			SceneManager.LoadScene("TapDemo");
		}
		
		startY=Screen.height/2-70;
		startX=Screen.width/2+10;
		
		if(GUI.Button(new Rect(startX, startY, width, height), "Turret Example")){
			SceneManager.LoadScene("TurretDemo");
		}
		if(GUI.Button(new Rect(startX, startY+=spaceY, width, height), "Flick-Shoot Example")){
			SceneManager.LoadScene("FlickDemo");
		}
		if(GUI.Button(new Rect(startX, startY+=spaceY, width, height), "DPad Example")){
			SceneManager.LoadScene("DPad");
		}
		
		GUI.Label(new Rect(5, Screen.height-25, 500, 25), "Input.Touches version1.3 Demo by K.SongTan");
	}
	*/
	
}
