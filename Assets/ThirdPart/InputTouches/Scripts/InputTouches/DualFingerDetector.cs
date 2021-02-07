using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using InputTouches;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

public class DualFingerDetector : MonoBehaviour {

	//for rotate
	private Vector2 initPos1=Vector3.zero;
	private Vector2 initPos2=Vector3.zero;
	private Vector2 initGradient;
	
	private Vector2 lastPos1;
	private Vector2 lastPos2;
	
	private bool firstTouch=true;
	
	private int currentStart=0;
	private List<float> rotVals=new List<float>();
	
	private float curAngle;
	private float prevAngle;
	
	private Vector2 lastTouchPos1;
	private Vector2 lastTouchPos2;
	
	public int rotationSmoothFrameCount=5;
	
	public enum _SmoothMethod{None, Average, WeightedAverage}
	public _SmoothMethod rotationSmoothing;
	
	void Update(){
		if(Input.touchCount==2){
			Touch touch1=Input.touches[0];
			Touch touch2=Input.touches[1];
			
			Vector2 pos1 = touch1.position; 
			Vector2 pos2 = touch2.position; 
			
			Vector2 delta1 = pos1-lastTouchPos1;
			Vector2 delta2 = pos2-lastTouchPos2;
			
			
			if(firstTouch){
				firstTouch=false;
				
				//for rotate
				initPos1=pos1;
				initPos2=pos2;
				initGradient=(pos1-pos2).normalized;
				
				float curX=pos1.x-pos2.x;
				float curY=pos1.y-pos2.y;
				prevAngle=IT_Gesture.VectorToAngle(new Vector2(curX, curY));				
			}
			
			
			if(touch1.phase==TouchPhase.Moved && touch2.phase==TouchPhase.Moved){
				
				float dot = Vector2.Dot(delta1, delta2);
				if(dot<.5f){
					Vector2 grad1=(pos1-initPos1).normalized;
					Vector2 grad2=(pos2-initPos2).normalized;
					
					float dot1=Vector2.Dot(grad1, initGradient);
					float dot2=Vector2.Dot(grad2, initGradient);
					
					//rotate				
					if(dot1<0.7f && dot2<0.7f){
						
						float curX=pos1.x-pos2.x;
						float curY=pos1.y-pos2.y;
						float curAngle=IT_Gesture.VectorToAngle(new Vector2(curX, curY));
						float val=Mathf.DeltaAngle(curAngle, prevAngle);
						
						if(rotationSmoothing==_SmoothMethod.None){
							RotateInfo rotateInfo=new RotateInfo(val, pos1, pos2);
                            IT_Gesture.Rotate(rotateInfo);
                        }
                        else{
                            if(Mathf.Abs(val)>0) AddRotVal(val);
                           
                            float valueAvg=GetAverageValue();
							if(valueAvg!=0){
								RotateInfo rotateInfo=new RotateInfo(valueAvg, pos1, pos2);
								IT_Gesture.Rotate(rotateInfo);
							}
                        }
						
						prevAngle=curAngle;
					}
					//pinch
					else{
						Vector2 curDist=pos1-pos2;
						Vector2 prevDist=(pos1-delta1)-(pos2-delta2);
						float pinch=prevDist.magnitude-curDist.magnitude;
						
						if(Mathf.Abs(pinch)>0.5f){
							PinchInfo pinchInfo=new PinchInfo(pinch, pos1, pos2);
							IT_Gesture.Pinch(pinchInfo);
						}
					}
				}
				
				
			}
			
			lastTouchPos1=pos1;
			lastTouchPos2=pos2;
			
		}
		else{
			if(!firstTouch){
                firstTouch=true;
                if(rotationSmoothing!=_SmoothMethod.None) ClearRotVal();
            }
		}
	}
	
	
	void AddRotVal(float val){
        if(rotVals.Count<rotationSmoothFrameCount){
            rotVals.Add(val);
            currentStart=rotVals.Count-1;
        }
        else{
            rotVals[currentStart]=val;
           
            currentStart+=1;
            if(currentStart>=rotVals.Count) currentStart=0;
        }
    }
   
    void ClearRotVal(){
        rotVals=new List<float>();
    }
   
    float GetAverageValue(){
		if(rotVals.Count==0) return 0;
		
        if(rotationSmoothing==_SmoothMethod.Average){
			float valTotal=0;
			//foreach(float val in rotVals){
			for(int i=0; i<rotVals.Count; i++){
				valTotal+=rotVals[i];
            }
           
            return valTotal/rotVals.Count;
        }
        if(rotationSmoothing==_SmoothMethod.WeightedAverage){
            if(weights.Length!=rotationSmoothFrameCount) InitWeigths();
           
			
			
            float valTotal=0;
            int count=0;
            for(int i=currentStart; i<rotVals.Count; i++){
                valTotal+=rotVals[i]*weights[count];
                count+=1;
            }
            for(int i=0; i<currentStart; i++){
               valTotal+=rotVals[i]*weights[count];
                count+=1;
            }
           
            //return valTotal/(rotVals.Count*weightTotal);
            return valTotal/(weightTotal);
        }
       
        return 0;
    }
   
    private float[] weights=new float[0];
    private float weightTotal;
    void InitWeigths(){
        weights=new float[rotationSmoothFrameCount];
        weightTotal=0;
        for(int i=0; i<weights.Length; i++){
            weights[i]=i+1;
            weightTotal+=i+1;
        }
    }

}
