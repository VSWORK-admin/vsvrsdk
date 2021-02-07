using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using InputTouches;

public class DragDetector : MonoBehaviour {

	private List<int> fingerIndex=new List<int>();
	private List<int> mouseIndex=new List<int>();
	
	public int minDragDistance=15;
	public bool enableMultiDrag=false;
	
	public bool fireOnDraggingWhenNotMoving=false;

	//private Vector2 lastPos;
	//private bool dragging=false;
	//private bool draggingInitiated=false;

	// Use this for initialization
	void Start () {
	
	}
	
	
	
	private int multiDragCount=0;
	IEnumerator MultiDragRoutine(int count){
		if(count<=1) yield break;
		
		bool dragStarted=false;
		
		Vector2 startPos=Vector2.zero;
		for(int i=0; i<Input.touchCount; i++){
			startPos+=Input.touches[i].position;
		}
		startPos/=Input.touchCount;
		Vector2 lastPos=startPos;
		
		float timeStart=Mathf.Infinity;
		
		#if UNITY_ANDROID
		Vector2[] lastPoss=new Vector2[count];
		for(int i=0; i<count; i++){
			lastPoss[i]=Input.touches[i].position;
		}
		#endif
		
		while(Input.touchCount==count){
			Vector2 curPos=Vector2.zero;
			Vector2[] allPos=new Vector2[count];
			bool moving=true;
			for(int i=0; i<count; i++){
				Touch touch=Input.touches[i];
				curPos+=touch.position;
				allPos[i]=touch.position;
				if(touch.phase!=TouchPhase.Moved) moving=false;
			}
			curPos/=count;
			
			bool sync=true;
			if(moving){
				for(int i=0; i<count-1; i++){
					#if UNITY_ANDROID
					Vector2 v1=(Input.touches[i].position+lastPoss[i]).normalized;
					Vector2 v2=(Input.touches[i+1].position+lastPoss[i+1]).normalized;
					#else
					Vector2 v1=Input.touches[i].deltaPosition.normalized;
					Vector2 v2=Input.touches[i+1].deltaPosition.normalized;
					#endif
					if(Vector2.Dot(v1, v2)<0.85f) sync=false;
				}
			}
			
			if(moving && sync){
				if(!dragStarted){
					if(Vector2.Distance(curPos, startPos)>minDragDistance*IT_Gesture.GetDPIFactor()){
						dragStarted=true;
						Vector2 delta=curPos-startPos;
						DragInfo dragInfo=new DragInfo(curPos, delta, count);
						IT_Gesture.DraggingStart(dragInfo);
						
						timeStart=Time.realtimeSinceStartup;
					}
				}
				else{
					if(curPos!=lastPos){
						Vector2 delta=curPos-lastPos;
						DragInfo dragInfo=new DragInfo(curPos, delta, count);
						IT_Gesture.Dragging(dragInfo);
					}
				}
			}
			else if(dragStarted && fireOnDraggingWhenNotMoving){
				DragInfo dragInfo=new DragInfo(curPos, Vector2.zero, count);
				IT_Gesture.Dragging(dragInfo);
			}
			
			lastPos=curPos;
			#if UNITY_ANDROID
			for(int i=0; i<count; i++){
				lastPoss[i]=Input.touches[i].position;
			}
			#endif
			
			yield return null;
		}
		
		if(dragStarted){
			bool isFlick=false;
			if(Time.realtimeSinceStartup-timeStart<0.5f) isFlick=true;
			
			Vector2 delta=lastPos-startPos;
			DragInfo dragInfo=new DragInfo(lastPos, delta, count, isFlick);
			IT_Gesture.DraggingEnd(dragInfo);
		}
		
	}
	
	IEnumerator MouseRoutine(int index){
		mouseIndex.Add(index);
		
		bool dragStarted=false;
		
		Vector2 startPos=Input.mousePosition;
		Vector2 lastPos=startPos;
		
		float timeStart=Mathf.Infinity;
		
		while(mouseIndex.Contains(index)){
			
			Vector2 curPos=Input.mousePosition;
			
			if(!dragStarted){
				if(Vector3.Distance(curPos, startPos)>minDragDistance*IT_Gesture.GetDPIFactor()){
					dragStarted=true;
					Vector2 delta=curPos-startPos;
					DragInfo dragInfo=new DragInfo(curPos, delta, 1, index, true);
					IT_Gesture.DraggingStart(dragInfo);
					
					timeStart=Time.realtimeSinceStartup;
				}
			}
			else{
				if(curPos!=lastPos){
					Vector2 delta=curPos-lastPos;
					DragInfo dragInfo=new DragInfo(curPos, delta, 1, index, true);
					IT_Gesture.Dragging(dragInfo);
				}
				else if(fireOnDraggingWhenNotMoving){
					DragInfo dragInfo=new DragInfo(curPos, Vector2.zero, 1, index, true);
					IT_Gesture.Dragging(dragInfo);
				}
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		if(dragStarted){
			bool isFlick=false;
			if(Time.realtimeSinceStartup-timeStart<0.5f) isFlick=true;
			
			Vector2 delta=lastPos-startPos;
			DragInfo dragInfo=new DragInfo(lastPos, delta, 1, index, isFlick, true);
			
			IT_Gesture.DraggingEnd(dragInfo);
		}
	}
	
	IEnumerator TouchRoutine(int index){
		fingerIndex.Add(index);
		
		bool dragStarted=false;
		
		Vector2 startPos=IT_Gesture.GetTouch(index).position;
		Vector2 lastPos=startPos;
		
		float timeStart=Mathf.Infinity;
		
		while((enableMultiDrag && Input.touchCount>0) || (!enableMultiDrag && Input.touchCount==1)){
			
			Touch touch=IT_Gesture.GetTouch(index);
			if(touch.position==Vector2.zero) break;
			
			Vector2 curPos=touch.position;
			
			if(touch.phase==TouchPhase.Moved){
				if(!dragStarted){
					if(Vector3.Distance(curPos, startPos)>minDragDistance*IT_Gesture.GetDPIFactor()){
						dragStarted=true;
						Vector2 delta=curPos-startPos;
						DragInfo dragInfo=new DragInfo(curPos, delta, 1, index, false);
						IT_Gesture.DraggingStart(dragInfo);
						
						timeStart=Time.realtimeSinceStartup;
					}
				}
				else{
					if(curPos!=lastPos){
						Vector2 delta=curPos-lastPos;
						DragInfo dragInfo=new DragInfo(curPos, delta, 1, index, false);
						IT_Gesture.Dragging(dragInfo);
					}
				}
				
				lastPos=curPos;
				
			}
			else if(dragStarted && fireOnDraggingWhenNotMoving){
				DragInfo dragInfo=new DragInfo(curPos, Vector2.zero, 1, index, false);
				IT_Gesture.Dragging(dragInfo);
			}
			
			yield return null;
		}
		
		if(dragStarted){
			bool isFlick=false;
			if(Time.realtimeSinceStartup-timeStart<0.5f) isFlick=true;
			
			Vector2 delta=lastPos-startPos;
			DragInfo dragInfo=new DragInfo(lastPos, delta, 1, index, isFlick, false);
			IT_Gesture.DraggingEnd(dragInfo);
		}
		
		fingerIndex.Remove(index);
	}
		
	
	// Update is called once per frame
	void Update () {
		
		if(Input.touchCount<=1) multiDragCount=1;
		
		//drag event detection goes here
		if(Input.touchCount>0){
			if(enableMultiDrag){
				//foreach(Touch touch in Input.touches){
				for(int i=0; i<Input.touches.Length; i++){
					Touch touch=Input.touches[i];
					if(fingerIndex.Count==0 || !fingerIndex.Contains(touch.fingerId)){
						StartCoroutine(TouchRoutine(touch.fingerId));
					}
				}
			}
			else{
				if(Input.touchCount==1){
					if(fingerIndex.Count==0){
						StartCoroutine(TouchRoutine(Input.touches[0].fingerId));
					}
				}
			}
			
			if(Input.touchCount>1 && Input.touchCount!=multiDragCount){
				multiDragCount=Input.touchCount;
				StartCoroutine(MultiDragRoutine(Input.touchCount));
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
			
			if(Input.GetMouseButtonDown(2)){
				if(!mouseIndex.Contains(2)) StartCoroutine(MouseRoutine(2)); 
			}
			else if(Input.GetMouseButtonUp(2)){
				if(mouseIndex.Contains(2)) mouseIndex.Remove(2); 
			}
			
			if(Input.GetMouseButtonDown(3)){
				if(!mouseIndex.Contains(3)) StartCoroutine(MouseRoutine(3)); 
			}
			else if(Input.GetMouseButtonUp(3)){
				if(mouseIndex.Contains(3)) mouseIndex.Remove(3); 
			}
		}
		
	}
	
	
}


