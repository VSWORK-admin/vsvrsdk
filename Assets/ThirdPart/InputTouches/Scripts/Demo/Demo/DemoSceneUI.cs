using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DemoSceneUI : MonoBehaviour {

	public Button buttonBack;
	public Button buttonInstruction;
	
	public Text labelSceneTitle;
	public Text labelInstruction;
	
	private static DemoSceneUI instance;
	
	void Awake(){
		instance=this;
	}
	
	// Use this for initialization
	void Start () {
		buttonBack.onClick.AddListener(delegate { OnBackButton(); });
		buttonInstruction.onClick.AddListener(delegate { OnInstructionButton(); });
		
		labelInstruction.enabled=false;
	}
	
	void OnBackButton(){
		SceneManager.LoadScene("DemoMenu");
	}
	
	private bool instruction=false;
	void OnInstructionButton(){
		instruction=!instruction;
		labelInstruction.enabled=instruction;
	}
	
	
	public static void SetSceneTitle(string text){
		if(instance!=null) instance.labelSceneTitle.text=text;
	}
	public static void SetSceneInstruction(string text){
		if(instance!=null) instance.labelInstruction.text=text;
	}
}
