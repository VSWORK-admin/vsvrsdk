using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using InputTouches;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

public enum _ChargeMode{Once, Clamp, Loop, PingPong}

public class MultiTapTracker{
	public int index;
	public int count=0;
	public float lastTapTime=-1;
	public Vector2 lastPos;
	
	public int fingerCount=1;
	 
	
	public MultiTapTracker(int ind){
		index=ind;
	}
}


public class TapDetector : MonoBehaviour {

	
	public static void SetChargeMode(_ChargeMode mode){ instance.chargeMode=mode; }
	public static _ChargeMode GetChargeMode(){ return instance!=null ? instance.chargeMode : _ChargeMode.Once ; }
	
	
	private List<int> fingerIndex=new List<int>();
	private List<int> mouseIndex=new List<int>();
	
	private MultiTapTracker[] multiTapMFTouch=new MultiTapTracker[5];
	private MultiTapTracker[] multiTapTouch=new MultiTapTracker[5];
	private MultiTapTracker[] multiTapMouse=new MultiTapTracker[2];

	private float tapStartTime=0;
	//private bool touched=false;
	
	private enum _DTapState{Clear, Tap1, Complete}
	private _DTapState dTapState=_DTapState.Clear;
	private Vector2 lastPos;
	
	private enum _ChargeState{Clear, Charging, Charged}
	//private _ChargeState chargeState=_ChargeState.Clear;
	//private float chargedValue=0;
	
	//private bool longTap;
	private Vector2 startPos;
	
	private bool posShifted;
	
	private float lastShortTapTime;
	private Vector2 lastShortTapPos;
	
	
	public bool enableMultiTapFilter=false;
	
	//public bool enableShortTap=true;
	public int maxTapDisplacementAllowance=5;
	public float shortTapTime=0.2f;
	public float longTapTime=0.8f;
	//public float maxLTapSpacing=10;
	
	public float multiTapInterval=0.35f;
	public float multiTapPosSpacing=20;
	public int maxMultiTapCount=2;
	
	public _ChargeMode chargeMode;
	public float minChargeTime=0.15f;
	public float maxChargeTime=2.0f;
	
	//the maximum pos deviation allowed while a touch is being held down
	public static float tapPosDeviation=10;
	
	private bool firstTouch=true;
	
	private static TapDetector instance;
	void Awake(){
		instance=this;
	}
	
	// Use this for initialization
	void Start () {
		
		if(enableMultiTapFilter){
			IT_Gesture.SetMultiTapFilter(enableMultiTapFilter);
			IT_Gesture.SetMaxMultiTapCount(maxMultiTapCount);
			IT_Gesture.SetMaxMultiTapInterval(multiTapInterval);
		}
		
		for(int i=0; i<multiTapMouse.Length; i++){
			multiTapMouse[i]=new MultiTapTracker(i);
		}
		for(int i=0; i<multiTapTouch.Length; i++){
			multiTapTouch[i]=new MultiTapTracker(i);
		}
		for(int i=0; i<multiTapMFTouch.Length; i++){
			multiTapMFTouch[i]=new MultiTapTracker(i);
		}
		
		StartCoroutine(CheckMultiTapCount());
		StartCoroutine(MultiFingerRoutine());
		
		//Debug.Log(IT_Gesture.GetTouch(0).position);
	}
	
	void CheckMultiTapMouse(int index, Vector2 startPos, Vector2 lastPos){
		if(multiTapMouse[index].lastTapTime>Time.realtimeSinceStartup-multiTapInterval){
			if(Vector2.Distance(startPos, multiTapMouse[index].lastPos)<multiTapPosSpacing*IT_Gesture.GetDPIFactor()){
				multiTapMouse[index].count+=1;
				multiTapMouse[index].lastPos=startPos;
				multiTapMouse[index].lastTapTime=Time.realtimeSinceStartup;
				
				IT_Gesture.MultiTap(new Tap(startPos, lastPos, multiTapMouse[index].count, index, true));
				
				if(multiTapMouse[index].count>=maxMultiTapCount) multiTapMouse[index].count=0;
			}
			else{
				multiTapMouse[index].count=1;
				multiTapMouse[index].lastPos=startPos;
				multiTapMouse[index].lastTapTime=Time.realtimeSinceStartup;
				
				IT_Gesture.MultiTap(new Tap(startPos, lastPos, 1, index, true));
			}
		}
		else{
			multiTapMouse[index].count=1;
			multiTapMouse[index].lastPos=startPos;
			multiTapMouse[index].lastTapTime=Time.realtimeSinceStartup;
			
			IT_Gesture.MultiTap(new Tap(startPos, lastPos, 1, index, true));
		}
	}
	
	void CheckMultiTapTouch(int index, Vector2 startPos, Vector2 lastPos){
		if(index>=multiTapTouch.Length) return;
		
		if(multiTapTouch[index].lastTapTime>Time.realtimeSinceStartup-multiTapInterval){
			if(Vector2.Distance(startPos, multiTapTouch[index].lastPos)<multiTapPosSpacing*IT_Gesture.GetDPIFactor()){
				multiTapTouch[index].count+=1;
				multiTapTouch[index].lastPos=startPos;
				multiTapTouch[index].lastTapTime=Time.realtimeSinceStartup;
				
				IT_Gesture.MultiTap(new Tap(startPos, lastPos, multiTapTouch[index].count, index, false));
				
				if(multiTapTouch[index].count>=maxMultiTapCount) multiTapTouch[index].count=0;
			}
			else{
				multiTapTouch[index].count=1;
				multiTapTouch[index].lastPos=startPos;
				multiTapTouch[index].lastTapTime=Time.realtimeSinceStartup;
				
				IT_Gesture.MultiTap(new Tap(startPos, lastPos, 1, index, false));
			}
		}
		else{
			multiTapTouch[index].count=1;
			multiTapTouch[index].lastPos=startPos;
			multiTapTouch[index].lastTapTime=Time.realtimeSinceStartup;
			
			IT_Gesture.MultiTap(new Tap(startPos, lastPos, 1, index, false));
		}
	}
	
	
	public void CheckMultiTapMFTouch(int fCount, Vector2[] posL, int[] indexes){
		Vector2 pos=Vector2.zero;
		//foreach(Vector2 p in posL){
		//	pos+=p;
		//}
		for(int i=0; i<posL.Length; i++){
			pos+=posL[i];
		}
		pos/=posL.Length;
		
		int index=0;
		bool match=false;
		//foreach(MultiTapTracker multiTap in multiTapMFTouch){
		for(int i=0; i<multiTapMFTouch.Length; i++){
			MultiTapTracker multiTap=multiTapMFTouch[i];
			if(multiTap.fingerCount==fCount){
				if(Vector2.Distance(pos, multiTap.lastPos)<multiTapPosSpacing*IT_Gesture.GetDPIFactor()){
					match=true;
					break;
				}
			}
			index+=1;
		}
		
		if(!match){
			index=0;
			//foreach(MultiTapTracker multiTap in multiTapMFTouch){
			for(int i=0; i<multiTapMFTouch.Length; i++){
				MultiTapTracker multiTap=multiTapMFTouch[i];
				if(multiTap.lastPos==Vector2.zero && multiTap.count==0){
					break;
				}
				index+=1;
			}
		}
		
		if(multiTapMFTouch[index].lastTapTime>Time.realtimeSinceStartup-multiTapInterval){
			multiTapMFTouch[index].count+=1;
			multiTapMFTouch[index].lastPos=pos;
			multiTapMFTouch[index].fingerCount=fCount;
			multiTapMFTouch[index].lastTapTime=Time.realtimeSinceStartup;
			
			IT_Gesture.MultiTap(new Tap(multiTapMFTouch[index].count, fCount, posL, indexes));
			
			if(multiTapMFTouch[index].count>=maxMultiTapCount) multiTapMFTouch[index].count=0;
		}
		else{
			multiTapMFTouch[index].count=1;
			multiTapMFTouch[index].lastPos=pos;
			multiTapMFTouch[index].fingerCount=fCount;
			multiTapMFTouch[index].lastTapTime=Time.realtimeSinceStartup;
			
			IT_Gesture.MultiTap(new Tap(multiTapMFTouch[index].count, fCount, posL, indexes));
		}
	}
	
	
	IEnumerator CheckMultiTapCount(){
		while(true){
			//foreach(MultiTapTracker multiTap in multiTapMouse){
			for(int i=0; i<multiTapMouse.Length; i++){
				MultiTapTracker multiTap=multiTapMouse[i];
				if(multiTap.count>0){
					if(multiTap.lastTapTime+multiTapInterval<Time.realtimeSinceStartup){
						multiTap.count=0;
						multiTap.lastPos=Vector2.zero;
					}
				}
			}
			//foreach(MultiTapTracker multiTap in multiTapTouch){
			for(int i=0; i<multiTapTouch.Length; i++){
				MultiTapTracker multiTap=multiTapTouch[i];
				if(multiTap.count>0){
					if(multiTap.lastTapTime+multiTapInterval<Time.realtimeSinceStartup){
						multiTap.count=0;
						multiTap.lastPos=Vector2.zero;
					}
				}
			}
			//foreach(MultiTapTracker multiTap in multiTapMFTouch){
			for(int i=0; i<multiTapMFTouch.Length; i++){
				MultiTapTracker multiTap=multiTapMFTouch[i];
				if(multiTap.count>0){
					if(multiTap.lastTapTime+multiTapInterval<Time.realtimeSinceStartup){
						multiTap.count=0;
						multiTap.lastPos=Vector2.zero;
						multiTap.fingerCount=1;
					}
				}
			}
			
			yield return null;
		}
	}
	
	IEnumerator FingerRoutine(int index){
		fingerIndex.Add(index);
		
		//init tap variables
		Touch touch=IT_Gesture.GetTouch(index);
		float startTime=Time.realtimeSinceStartup;
		Vector2 startPos=touch.position;
		Vector2 lastPos=startPos;
		bool longTap=false;
		
		//init charge variables
		_ChargeState chargeState=_ChargeState.Clear;
		int chargeDir=1;
		int chargeConst=0;
		float startTimeCharge=Time.realtimeSinceStartup;
		Vector2 startPosCharge=touch.position;
		
		//yield return null;
		
		bool inGroup=false;

		while(true){
			touch=IT_Gesture.GetTouch(index);
			if(touch.position==Vector2.zero) break;
			
			Vector2 curPos=touch.position;
			
			if(Time.realtimeSinceStartup-startTimeCharge>minChargeTime && chargeState==_ChargeState.Clear){
				chargeState=_ChargeState.Charging;
				float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/maxChargeTime), 0, 1);
				ChargedInfo cInfo=new ChargedInfo(curPos, chargedValue, index, false);
				IT_Gesture.ChargeStart(cInfo);
				
				startPosCharge=curPos;
			}
			else if(chargeState==_ChargeState.Charging){
				if(Vector3.Distance(curPos, startPosCharge)>tapPosDeviation){
					chargeState=_ChargeState.Clear;
					float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/maxChargeTime), 0, 1);
					ChargedInfo cInfo=new ChargedInfo(lastPos, chargedValue, index, false);
					IT_Gesture.ChargeEnd(cInfo);
				}
				else{
					float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/maxChargeTime), 0, 1);
					ChargedInfo cInfo=new ChargedInfo(curPos, chargedValue, index, false);
					
					if(chargeMode==_ChargeMode.PingPong){
						if(chargedValue==1 || chargedValue==0){
							chargeDir*=-1;
							if(chargeDir==1) chargeConst=0;
							else if(chargeDir==-1) chargeConst=1;
							startTimeCharge=Time.realtimeSinceStartup;
						}
						
						IT_Gesture.Charging(cInfo);
					}
					else{
						if(chargedValue<1.0f){
							IT_Gesture.Charging(cInfo);
						}
						else{
							cInfo.percent=1.0f;
							
							if(chargeMode==_ChargeMode.Once){
								chargeState=_ChargeState.Charged;
								IT_Gesture.ChargeEnd(cInfo);
								startTimeCharge=Mathf.Infinity;
								chargedValue=0;
							}
							else if(chargeMode==_ChargeMode.Clamp){
								chargeState=_ChargeState.Charged;
								IT_Gesture.Charging(cInfo);
							}
							else if(chargeMode==_ChargeMode.Loop){
								chargeState=_ChargeState.Clear;
								IT_Gesture.ChargeEnd(cInfo);
								startTimeCharge=Time.realtimeSinceStartup;
							}
						}
						
					}
				}
			}
			
			if(!longTap && Time.realtimeSinceStartup-startTime>longTapTime && Vector2.Distance(lastPos, startPos)<maxTapDisplacementAllowance*IT_Gesture.GetDPIFactor()){
				//new Tap(multiTapMFTouch[index].count, fCount, posL)
				//IT_Gesture.LongTap(new Tap(multiTapMFTouch[index].count, fCount, posL));
				IT_Gesture.LongTap(new Tap(curPos, 1, index, false));
				//IT_Gesture.LongTap(startPos);
				longTap=true;
			}
			
			lastPos=curPos;
			
			if(!inGroup) inGroup=IndexInFingerGroup(index);
			
			yield return null;
		}
		
		//check for shortTap
		if(!inGroup){
			if(Time.realtimeSinceStartup-startTime<=shortTapTime && Vector2.Distance(lastPos, startPos)<maxTapDisplacementAllowance*IT_Gesture.GetDPIFactor()){
				CheckMultiTapTouch(index, startPos, lastPos);
			}
		}
		
		//check for charge
		if(chargeState==_ChargeState.Charging || (chargeState==_ChargeState.Charged && chargeMode!=_ChargeMode.Once)){
			float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/maxChargeTime), 0, 1);
			ChargedInfo cInfo=new ChargedInfo(lastPos, chargedValue, index, false);
			IT_Gesture.ChargeEnd(cInfo);
		}
		
		fingerIndex.Remove(index);
	}
	
	
	IEnumerator MouseRoutine(int index){
		mouseIndex.Add(index);
		
		//init tap variables
		float startTime=Time.realtimeSinceStartup;
		Vector2 startPos=Input.mousePosition;
		Vector2 lastPos=startPos;
		bool longTap=false;
		
		//init charge variables
		_ChargeState chargeState=_ChargeState.Clear;
		int chargeDir=1;
		float chargeConst=0;
		float startTimeCharge=Time.realtimeSinceStartup;
		Vector2 startPosCharge=Input.mousePosition;
		
		yield return null;
		
		while(mouseIndex.Contains(index)){
			
			Vector2 curPos=Input.mousePosition;
			
			if(Time.realtimeSinceStartup-startTimeCharge>minChargeTime && chargeState==_ChargeState.Clear){
				chargeState=_ChargeState.Charging;
				float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/maxChargeTime), 0, 1);
				ChargedInfo cInfo=new ChargedInfo(curPos, chargedValue, index, true);
				IT_Gesture.ChargeStart(cInfo);
				
				startPosCharge=curPos;
			}
			else if(chargeState==_ChargeState.Charging){
				if(Vector3.Distance(curPos, startPosCharge)>tapPosDeviation){
					chargeState=_ChargeState.Clear;
					float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/maxChargeTime), 0, 1);
					ChargedInfo cInfo=new ChargedInfo(lastPos, chargedValue, index, true);
					IT_Gesture.ChargeEnd(cInfo);
				}
				else{
					float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/maxChargeTime), 0, 1);
					ChargedInfo cInfo=new ChargedInfo(curPos, chargedValue, index, true);
					
					if(chargeMode==_ChargeMode.PingPong){
						if(chargedValue==1 || chargedValue==0){
							chargeDir*=-1;
							if(chargeDir==1) chargeConst=0;
							else if(chargeDir==-1) chargeConst=1;
							startTimeCharge=Time.realtimeSinceStartup;
						}
						IT_Gesture.Charging(cInfo);
					}
					else{
						if(chargedValue<1.0f){
							IT_Gesture.Charging(cInfo);
						}
						else{
							cInfo.percent=1.0f;
							
							if(chargeMode==_ChargeMode.Once){
								chargeState=_ChargeState.Charged;
								IT_Gesture.ChargeEnd(cInfo);
								startTimeCharge=Mathf.Infinity;
								chargedValue=0;
							}
							else if(chargeMode==_ChargeMode.Clamp){
								chargeState=_ChargeState.Charged;
								IT_Gesture.Charging(cInfo);
							}
							else if(chargeMode==_ChargeMode.Loop){
								chargeState=_ChargeState.Clear;
								IT_Gesture.ChargeEnd(cInfo);
								startTimeCharge=Time.realtimeSinceStartup;
							}
						}
						
					}
				}
			}
			
			if(!longTap && Time.realtimeSinceStartup-startTime>longTapTime && Vector2.Distance(lastPos, startPos)<maxTapDisplacementAllowance*IT_Gesture.GetDPIFactor()){
				IT_Gesture.LongTap(new Tap(curPos, 1, index, true));
				longTap=true;
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		//check for shortTap
		if(Time.realtimeSinceStartup-startTime<=shortTapTime && Vector2.Distance(lastPos, startPos)<maxTapDisplacementAllowance*IT_Gesture.GetDPIFactor()){
			//IT_Gesture.ShortTap(startPos);
			CheckMultiTapMouse(index, startPos, lastPos);
		}
		
		//check for charge
		if(chargeState==_ChargeState.Charging || (chargeState==_ChargeState.Charged && chargeMode!=_ChargeMode.Once)){
			float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/maxChargeTime), 0, 1);
			ChargedInfo cInfo=new ChargedInfo(lastPos, chargedValue, index, true);
			IT_Gesture.ChargeEnd(cInfo);
		}
		
	}
	
	//private List<int> indexes=new List<int>();
	
	// Update is called once per frame
	void Update () {
		
		//InputEvent inputEvent=new InputEvent();
		
		if(Input.touchCount>0){
			
			if(fingerIndex.Count<Input.touchCount){
				//foreach(Touch touch in Input.touches){
				for(int i=0; i<Input.touches.Length; i++){
					Touch touch=Input.touches[i];
					if(!fingerIndex.Contains(touch.fingerId)){
						CheckFingerGroup(touch);
					}
				}
			}

			//foreach(Touch touch in Input.touches){
			for(int i=0; i<Input.touches.Length; i++){
				Touch touch=Input.touches[i];
				if(fingerIndex.Count==0 || (!fingerIndex.Contains(touch.fingerId))){
					//StartCoroutine(swipeRoutine(new InputEvent(touch)));
					StartCoroutine(FingerRoutine(touch.fingerId));
				}
			}
			
		}
		else if(Input.touchCount==0){
			if(Input.GetMouseButtonDown(0)){
				if(!mouseIndex.Contains(0)) StartCoroutine(MouseRoutine(0)); 
			}
			else if(Input.GetMouseButtonUp(0)){
				if(mouseIndex.Contains(0)) mouseIndex.Remove(0); 
			}
			
			if(Input.GetMouseButtonDown(1)){
				if(!mouseIndex.Contains(1)) StartCoroutine(MouseRoutine(1)); 
			}
			else if(Input.GetMouseButtonUp(1)){
				if(mouseIndex.Contains(1)) mouseIndex.Remove(1); 
			}
		}
		
	}
	
	private List<FingerGroup> fingerGroup=new List<FingerGroup>();
	IEnumerator MultiFingerRoutine(){
		while(true){
			if(fingerGroup.Count>0){
				for(int i=0; i<fingerGroup.Count; i++){
					if(fingerGroup[i].routineEnded){
						fingerGroup.RemoveAt(i);
						i--;
					}
				}
			}
			yield return null;
		}
	}
	
	public float maxFingerGroupDist=200;
	
	void CheckFingerGroup(Touch touch){
		//Debug.Log("Checking "+Time.realtimeSinceStartup);
		bool match=false;
		//foreach(FingerGroup group in fingerGroup){
		for(int i=0; i<fingerGroup.Count; i++){
			FingerGroup group=fingerGroup[i];
			if(Time.realtimeSinceStartup-group.triggerTime<shortTapTime/2){
				bool inRange=true;
				float dist=0;
				//foreach(int index in group.indexes){
				for(int j=0; j<group.indexes.Count; j++){
					int index=group.indexes[j];	
					dist=Vector2.Distance(IT_Gesture.GetTouch(index).position, touch.position);
					if(Vector2.Distance(IT_Gesture.GetTouch(index).position, touch.position)>maxFingerGroupDist*IT_Gesture.GetDPIFactor())
						inRange=false;
				}
				if(inRange){
					group.indexes.Add(touch.fingerId);
					group.positions.Add(touch.position);
					match=true;
					break;
				}
			}
		}
		
		if(!match){
			fingerGroup.Add(new FingerGroup(Time.realtimeSinceStartup, touch.fingerId, touch.position));
			StartCoroutine(fingerGroup[fingerGroup.Count-1].Routine(this));
		}
	}
	
	private bool IndexInFingerGroup(int index){
		for(int i=0; i<fingerGroup.Count; i++){
			if(fingerGroup[i].indexes.Count<2) continue;
			if(fingerGroup[i].indexes.Contains(index)) return true;
		}
		return false;
	}
	
}



public class FingerGroup{
	public List<int> indexes=new List<int>();
	public List<Vector2> positions=new List<Vector2>();
	public Vector2 posAvg;
	public float triggerTime;
	public bool routineEnded=false;
	public int count=0;
	public bool longTap=false;
	
	//charge related variable
	private enum _ChargeState{Clear, Charging, Charged}
	_ChargeState chargeState=_ChargeState.Clear;
	int chargeDir=1;
	int chargeConst=0;
	float startTimeCharge=Time.realtimeSinceStartup;
	
	public int[] indexList;
	private Vector2[] posList;
	
	private TapDetector tapDetector;

	public FingerGroup(float time, int index, Vector2 pos){
		indexes.Add(index);
		positions.Add(pos);
	}
	
	public IEnumerator Routine(TapDetector tapD){
		tapDetector=tapD;
		
		triggerTime=Time.realtimeSinceStartup;
		startTimeCharge=Time.realtimeSinceStartup;
		
		yield return new WaitForSeconds(0.075f);
		if(indexes.Count<2){
			routineEnded=true;
			yield break;
		}
		else{
			count=indexes.Count;

			posAvg=Vector2.zero;
			//foreach(Vector2 p in positions) posAvg+=p;
			for(int i=0; i<positions.Count; i++) posAvg+=positions[i];
			posAvg/=positions.Count;
			
			posList = new Vector2[positions.Count];
  			positions.CopyTo( posList );
  			indexList = new int[indexes.Count];
  			indexes.CopyTo( indexList );		
		}
		
		bool isOn=true;
		
		float liftTime=-1;
		
		int rand=Random.Range(0, 99999);
		
		while(isOn){
			for(int i=0; i<indexes.Count; i++){
				Touch touch=IT_Gesture.GetTouch(indexes[i]);
				if(touch.phase==TouchPhase.Moved) isOn=false;
				
				if(touch.position==Vector2.zero){
					if(indexes.Count==count){
						liftTime=Time.realtimeSinceStartup;
					}
					indexes.RemoveAt(i);
					i--;
				}
			}
			
			if(Time.realtimeSinceStartup-startTimeCharge>tapDetector.minChargeTime && chargeState==_ChargeState.Clear){
				chargeState=_ChargeState.Charging;
				float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/tapDetector.maxChargeTime), 0, 1);
				ChargedInfo cInfo=new ChargedInfo(posAvg, posList, chargedValue, indexList);
				IT_Gesture.ChargeStart(cInfo);
			}
			else if(chargeState==_ChargeState.Charging){

				float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/tapDetector.maxChargeTime), 0, 1);
				ChargedInfo cInfo=new ChargedInfo(posAvg, posList, chargedValue, indexList);
				
				if(tapDetector.chargeMode==_ChargeMode.PingPong){
					if(chargedValue==1 || chargedValue==0){
						chargeDir*=-1;
						if(chargeDir==1) chargeConst=0;
						else if(chargeDir==-1) chargeConst=1;
						startTimeCharge=Time.realtimeSinceStartup;
					}
					
					IT_Gesture.Charging(cInfo);
				}
				else{
					if(chargedValue<1.0f){
						IT_Gesture.Charging(cInfo);
					}
					else{
						cInfo.percent=1.0f;
						
						if(tapDetector.chargeMode==_ChargeMode.Once){
							chargeState=_ChargeState.Charged;
							IT_Gesture.ChargeEnd(cInfo);
							startTimeCharge=Mathf.Infinity;
							chargedValue=0;
						}
						else if(tapDetector.chargeMode==_ChargeMode.Clamp){
							chargeState=_ChargeState.Charged;
							IT_Gesture.Charging(cInfo);
						}
						else if(tapDetector.chargeMode==_ChargeMode.Loop){
							chargeState=_ChargeState.Clear;
							IT_Gesture.ChargeEnd(cInfo);
							startTimeCharge=Time.realtimeSinceStartup;
						}
					}
				}

			}
			
			if(!longTap && Time.realtimeSinceStartup-triggerTime>tapDetector.longTapTime){
				if(indexes.Count==count){
					Vector2[] posList = new Vector2[positions.Count];
  					positions.CopyTo( posList );
					
					Tap tap=new Tap(1, count, posList , indexList);
					IT_Gesture.LongTap(tap);
					longTap=true;
				}
			}
			
			if(indexes.Count<count){
				if(Time.realtimeSinceStartup-liftTime>0.075f || indexes.Count==0){
					if(indexes.Count==0){
						if(liftTime-triggerTime<tapDetector.shortTapTime+0.1f){
							Vector2[] posList = new Vector2[positions.Count];
		  					positions.CopyTo( posList );
							//Tap tap=new Tap(1, count, posList);
							//IT_Gesture.MFShortTap(tap);
							tapDetector.CheckMultiTapMFTouch(count, posList, indexList);
						}
					}
					isOn=false;
					break;
				}
			}
			
			yield return null;
		}
		
		if(chargeState==_ChargeState.Charging || (chargeState==_ChargeState.Charged && tapDetector.chargeMode!=_ChargeMode.Once)){
			float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.realtimeSinceStartup-startTimeCharge)/tapDetector.maxChargeTime), 0, 1);
			ChargedInfo cInfo=new ChargedInfo(posAvg, posList, chargedValue, indexList);
			IT_Gesture.ChargeEnd(cInfo);
		}
		
		routineEnded=true;
	}
}