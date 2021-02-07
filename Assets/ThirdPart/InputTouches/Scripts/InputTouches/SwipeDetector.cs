using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using InputTouches;

public class SwipeDetector : MonoBehaviour {

	private enum _SwipeState{None, Start, Swiping, End}

	private List<int> fingerIndex=new List<int>();
	private List<int> mouseIndex=new List<int>();
	
	
	public float maxSwipeDuration=0.25f;
	public float minSpeed=150;
	public float minDistance=15;
	public float maxDirectionChange=35;
	public bool onlyFireWhenLiftCursor=false;
	public bool enableMultiSwipe=true;
	
	
	public float minDurationBetweenSwipe=0.5f;
	private float lastSwipeTime=-10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.touchCount>0){
			if(enableMultiSwipe){
				//foreach(Touch touch in Input.touches){
				for(int i=0; i<Input.touches.Length; i++){
					Touch touch=Input.touches[i];
					if(fingerIndex.Count==0 || !fingerIndex.Contains(touch.fingerId)){
						StartCoroutine(TouchSwipeRoutine(touch.fingerId));
					}
				}
			}
			else{
				if(Input.touchCount==1 && fingerIndex.Count!=1){
					Touch touch=Input.touches[0];
					StartCoroutine(TouchSwipeRoutine(touch.fingerId));
				}
			}
		}
		else if(Input.touchCount==0){
			if(Input.GetMouseButtonDown(0)){
				if(!mouseIndex.Contains(0)) StartCoroutine(MouseSwipeRoutine(0)); 
			}
			else if(Input.GetMouseButtonUp(0)){
				if(mouseIndex.Contains(0)) mouseIndex.Remove(0); 
			}
			
			if(Input.GetMouseButtonDown(1)){
				if(!mouseIndex.Contains(1)) StartCoroutine(MouseSwipeRoutine(1)); 
			}
			else if(Input.GetMouseButtonUp(1)){
				if(mouseIndex.Contains(1)) mouseIndex.Remove(1); 
			}
		}
		
	}
	
	
	IEnumerator MouseSwipeRoutine(int index){
		//GameMessage.DisplayMessage("swipe routine started");
		//Debug.Log("swipe routine started........................");
		mouseIndex.Add(index);
		
		float timeStartSwipe=Time.realtimeSinceStartup;
		Vector2 initialPos;	//initial pos when mouse is down
		Vector2 startPos;
		Vector2 initVector=Vector2.zero;
		Vector2 lastPos=Vector2.zero;
		_SwipeState swipeState=_SwipeState.None;
		
		lastPos=Input.mousePosition;
		startPos=lastPos;
		initialPos=lastPos;
		
		int count=0;
		
		yield return null;
		
		while(mouseIndex.Contains(index)){
			
			Vector2 curPos=Input.mousePosition;
			Vector2 curVector=curPos-lastPos;
			
			float mag=curVector.magnitude;
			
			count+=1;
			
			if(swipeState==_SwipeState.None && mag>0){
				timeStartSwipe=Time.realtimeSinceStartup;
				//startPos=curPos;
				startPos=initialPos;
				swipeState=_SwipeState.Swiping;
				initVector=curVector;
				count=1;
				
				SwipeStart(startPos, curPos, timeStartSwipe, index, true);
			}
			else if(swipeState==_SwipeState.Swiping){
				if(count<3) initVector=(initVector+curVector)*0.5f;
				
				if(mag>0){
					Swiping(startPos, curPos, timeStartSwipe, index, true);
					
					if(curPos.x<0 || curPos.x>Screen.width || curPos.y<0 || curPos.y>Screen.height){
						//GameMessage.DisplayMessage("out of bound");
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, true);
						initialPos=curPos;
					}
					if(Time.realtimeSinceStartup-timeStartSwipe>maxSwipeDuration){
						//GameMessage.DisplayMessage("duration due");
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, true);
						initialPos=curPos;
					}
					//check angle
					if(Mathf.Abs(Vector2.Angle(initVector, curVector))>maxDirectionChange){
						//GameMessage.DisplayMessage("angle too wide");
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, true);
						initialPos=curPos;
					}
					//check speed
					if(Mathf.Abs((curPos-startPos).magnitude/(Time.realtimeSinceStartup-timeStartSwipe))<minSpeed){
						//GameMessage.DisplayMessage("too slow");
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, true);
						initialPos=curPos;
					}
				}
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		if(swipeState==_SwipeState.Swiping){
			swipeState=_SwipeState.None;
			SwipeEnd(startPos, lastPos, timeStartSwipe, index, true);
		}
		
		//GameMessage.DisplayMessage("swipe routine ended");
	}
	
	
	IEnumerator TouchSwipeRoutine(int index){
		//GameMessage.DisplayMessage("swipe routine started");
		fingerIndex.Add(index);
		
		float timeStartSwipe=Time.realtimeSinceStartup;
		Vector2 initialPos;
		Vector2 startPos;
		Vector2 initVector=Vector2.zero;
		Vector2 lastPos=Vector2.zero;
		_SwipeState swipeState=_SwipeState.None;
		
		lastPos=IT_Gesture.GetTouch(index).position;
		startPos=lastPos;
		initialPos=lastPos;
		
		int count=0;
		
		yield return null;
		
		while(Input.touchCount>0){
			if(!enableMultiSwipe && Input.touchCount>1) break;
			
			Touch touch=IT_Gesture.GetTouch(index);
			
			if(touch.position==Vector2.zero) break;
			
			Vector2 curPos=touch.position;
			Vector2 curVector=curPos-lastPos;
			
			float mag=curVector.magnitude;
			
			count+=1;
			
			if(swipeState==_SwipeState.None && mag>0){
				timeStartSwipe=Time.realtimeSinceStartup;
				//startPos=curPos;
				startPos=initialPos;
				swipeState=_SwipeState.Swiping;
				initVector=curVector;
				
				count=1;
				
				SwipeStart(startPos, curPos, timeStartSwipe, index, false);
			}
			else if(swipeState==_SwipeState.Swiping){
				if(count<3) initVector=(initVector+curVector)*0.5f;
				
				if(mag>0){
					Swiping(startPos, curPos, timeStartSwipe, index, false);
					
					if(curPos.x<0 || curPos.x>Screen.width || curPos.y<0 || curPos.y>Screen.height){
						//GameMessage.DisplayMessage("out of bound");
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, true);
						initialPos=curPos;
					}
					if(Time.realtimeSinceStartup-timeStartSwipe>maxSwipeDuration){
						//GameMessage.DisplayMessage("duration due");
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index);
						initialPos=curPos;
					}
					//check angle
					if(Mathf.Abs(Vector2.Angle(initVector, curVector))>maxDirectionChange){
						//GameMessage.DisplayMessage("angle is too wide "+initVector+"   "+curVector);
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index);
						initialPos=curPos;
					}
					//check speed
					if(Mathf.Abs((curPos-startPos).magnitude/(Time.realtimeSinceStartup-timeStartSwipe))<minSpeed){
						//GameMessage.DisplayMessage("too slow");
						swipeState=_SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index);
						initialPos=curPos;
					}
				}
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		if(swipeState==_SwipeState.Swiping){
			swipeState=_SwipeState.None;
			SwipeEnd(startPos, lastPos, timeStartSwipe, index);
		}
		
		fingerIndex.Remove(index);
		//GameMessage.DisplayMessage("swipe routine ended");
	}
	
	
	void SwipeStart(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index, bool isMouse){
		Vector2 swipeDir=endPos-startPos;
		SwipeInfo sw=new SwipeInfo(startPos, endPos, swipeDir, timeStartSwipe, index, isMouse);
		IT_Gesture.SwipeStart(sw);
	}
	
	void Swiping(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index, bool isMouse){
		Vector2 swipeDir=endPos-startPos;
		SwipeInfo sw=new SwipeInfo(startPos, endPos, swipeDir, timeStartSwipe, index, isMouse);
		IT_Gesture.Swiping(sw);
	}
	
	
	void SwipeEnd(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index){
		SwipeEnd(startPos, endPos, timeStartSwipe, index, false);
	}
		
	void SwipeEnd(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index, bool isMouse){
		if(onlyFireWhenLiftCursor){
			if(!isMouse){
				for(int i=0; i<Input.touchCount; i++){
					Touch touch=Input.touches[i];
					if(touch.fingerId==index){
						return;
					}
				}
			}
			else{
				if(mouseIndex.Contains(index)){
					//Debug.Log("mouse still down");
					return;
				}
				if(Time.realtimeSinceStartup-timeStartSwipe>maxSwipeDuration){
					//Debug.Log("too long   "+(Time.realtimeSinceStartup-timeStartSwipe));
					return;
				}
			}
		}
		
		
		
		Vector2 swipeDir=endPos-startPos;
		SwipeInfo sw=new SwipeInfo(startPos, endPos, swipeDir, timeStartSwipe, index, isMouse);
		
		IT_Gesture.SwipeEnd(sw);
		
		if((swipeDir).magnitude<minDistance*IT_Gesture.GetDPIFactor()) {
			//GameMessage.DisplayMessage("too short");
			return;
		}
		
		if(Time.time-lastSwipeTime<minDurationBetweenSwipe) return;
		lastSwipeTime=Time.time;
		
		IT_Gesture.Swipe(sw);
		
		//GameMessage.DisplayMessage("swiped");
	}
	
	
}
