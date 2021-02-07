using UnityEngine;
using System.Collections;


[RequireComponent (typeof (BasicDetector))]
[RequireComponent (typeof (DragDetector))]
[RequireComponent (typeof (TapDetector))]
[RequireComponent (typeof (SwipeDetector))]
[RequireComponent (typeof (DualFingerDetector))]


public class IT_Gesture : MonoBehaviour {

	
	public static IT_Gesture instance;
	
	void Awake(){
		instance=this;
	}
	
	//to convert editor screen to actual screen pos
	//no in use, at all, ever
	public static Vector2 ConvertToCurrentResolution(Vector2 inPos){
		Vector2 outPos=Vector2.zero;
		outPos.x=(int)(inPos.x*(float)Screen.currentResolution.width/(float)Screen.width);
		outPos.y=(int)(inPos.y*(float)Screen.currentResolution.height/(float)Screen.height);
		return outPos;
	}
	
	
	
	public bool useDPIScaling=true;
	public float defaultDPI=441;	//this is tested on GalaxyS4 hence the default DPI used is based on GalaxyS4
	public static float GetDefaultDPI(){ return instance.defaultDPI; }
	public static float GetCurrentDPI(){ return Screen.dpi!=0 ? Screen.dpi : GetDefaultDPI(); }
	public static float GetDPIFactor(){ return !instance.useDPIScaling ? 1 : GetCurrentDPI()/GetDefaultDPI(); }
	
	
	
	
	//*****************************************************************************//
	//standard tap event
	public delegate void MultiTapHandler(Tap tap); 
	public static event MultiTapHandler onMultiTapE;
	
	
	public delegate void LongTapHandler(Tap tap); 
	public static event LongTapHandler onLongTapE;
	
	public delegate void ChargeStartHandler(ChargedInfo cInfo); 
	public static event ChargeStartHandler onChargeStartE;
	
	public delegate void ChargingHandler(ChargedInfo cInfo); 
	public static event ChargingHandler onChargingE;
	
	public delegate void ChargeEndHandler(ChargedInfo cInfo); 
	public static event ChargeEndHandler onChargeEndE;
	
	
	//Multi-Finger Standard tap event
	public delegate void MFMultiTapHandler(Tap tap); 
	public static event MFMultiTapHandler onMFMultiTapE;
	
	public delegate void MFLongTapHandler(Tap tap); 
	public static event MFLongTapHandler onMFLongTapE;
	
	public delegate void MFChargeStartHandler(ChargedInfo cInfo); 
	public static event MFChargeStartHandler onMFChargeStartE;
	
	public delegate void MFChargingHandler(ChargedInfo cInfo); 
	public static event MFChargingHandler onMFChargingE;
	
	public delegate void MFChargeEndHandler(ChargedInfo cInfo); 
	public static event MFChargeEndHandler onMFChargeEndE;
	
	
	//*****************************************************************************//
	//dragging
	public delegate void DraggingStartHandler(DragInfo dragInfo);
	public static event DraggingStartHandler onDraggingStartE;
	
	public delegate void DraggingHandler(DragInfo dragInfo);
	public static event DraggingHandler onDraggingE;
	
	public delegate void DraggingEndHandler(DragInfo dragInfo); 
	public static event DraggingEndHandler onDraggingEndE;
	
	public delegate void MFDraggingStartHandler(DragInfo dragInfo); 
	public static event MFDraggingStartHandler onMFDraggingStartE;
	
	public delegate void MFDraggingHandler(DragInfo dragInfo); 
	public static event MFDraggingHandler onMFDraggingE;
	
	public delegate void MFDraggingEndHandler(DragInfo dragInf); 
	public static event MFDraggingEndHandler onMFDraggingEndE;
	
	
	//*****************************************************************************//
	//special event swipe/pinch/rotate
	public delegate void SwipeStartHandler(SwipeInfo sw); 
	public static event SwipeStartHandler onSwipeStartE;
	
	public delegate void SwipingHandler(SwipeInfo sw); 
	public static event SwipingHandler onSwipingE;
	
	public delegate void SwipeEndHandler(SwipeInfo sw); 
	public static event SwipeEndHandler onSwipeEndE;
	
	public delegate void SwipeHandler(SwipeInfo sw); 
	public static event SwipeHandler onSwipeE;
	
	public delegate void PinchHandler(PinchInfo PI); 
	public static event PinchHandler onPinchE;
	
	public delegate void RotateHandler(RotateInfo RI); 
	public static event RotateHandler onRotateE;
	
	
	
	//*****************************************************************************//
	//native input down/on/up event
	public delegate void TouchDownPosHandler(Vector2 pos); 
	public static event TouchDownPosHandler onTouchDownPosE;
	
	public delegate void TouchUpPosHandler(Vector2 pos); 
	public static event TouchUpPosHandler onTouchUpPosE;
	
	public delegate void TouchPosHandler(Vector2 pos); 
	public static event TouchPosHandler onTouchPosE;
	
	public delegate void TouchDownHandler(Touch touch); 
	public static event TouchDownHandler onTouchDownE;
	
	public delegate void TouchUpHandler(Touch touch); 
	public static event TouchUpHandler onTouchUpE;
	
	public delegate void TouchHandler(Touch touch); 
	public static event TouchHandler onTouchE;
	
	public delegate void Mouse1DownHandler(Vector2 pos); 
	public static event Mouse1DownHandler onMouse1DownE;
	
	public delegate void Mouse1UpHandler(Vector2 pos); 
	public static event Mouse1UpHandler onMouse1UpE;
	
	public delegate void Mouse1Handler(Vector2 pos); 
	public static event Mouse1Handler onMouse1E;
	
	public delegate void Mouse2DownHandler(Vector2 pos); 
	public static event Mouse2DownHandler onMouse2DownE;
	
	public delegate void Mouse2UpHandler(Vector2 pos); 
	public static event Mouse2UpHandler onMouse2UpE;
	
	public delegate void Mouse2Handler(Vector2 pos); 
	public static event Mouse2Handler onMouse2E;
	
	
	//~ //the rotating direction
	//~ private var rotateDirection:int=0;
	
	//~ //set a rotate direction when a drag is detected
	//~ function onDraggingE(info:DragInfo){
		//~ if(info.delta.x>0) rotateDirection=1;
		//~ else if(info.delta.x<0) rotateDirection=-1;
	//~ }
	
	//~ //while there's a valid rotate direction, rotate the player
	//~ //onTouchE will be called as long as there's a finger on the screen
	//~ function onTouchE(pos:Vector2){
		//~ if(rotateDirection>0) PlayerRotateRight();
		//~ else if(rotateDirection<0) playerRotateLeft();
	//~ }
	
	//~ //clear the rotate direction so the player stop rotating in the next touch event
	//~ //onTouchUpE will be called when a finger is lifted from the screen
	//~ function onTouchUpE(pos:Vector2){
		//~ rotateDirection=0;
	//~ }
	
	
	private bool enableMultiTapFilter=false;
	private int tapExisted=0;
	private int MFtapExisted=0;
	private int maxMultiTapCount=2;
	private float maxMultiTapInterval=0.35f;
	
	public static void SetMultiTapFilter(bool flag){
		if(instance!=null) instance.enableMultiTapFilter=flag;
	}
	public static void SetMaxMultiTapCount(int val){
		if(instance!=null) instance.maxMultiTapCount=val;
	}
	public static void SetMaxMultiTapInterval(float val){
		if(instance!=null) instance.maxMultiTapInterval=val;
	}
	
	void CheckMultiTap(Tap tap){
		tapExisted+=1;
		
		if(tap.count==maxMultiTapCount){
			tapExisted=0;
			if(onMultiTapE!=null) onMultiTapE(tap);
		}
		else{
			StartCoroutine(TapCoroutine(tap));
		}
	}
	
	IEnumerator TapCoroutine(Tap tap){
		yield return new WaitForSeconds(maxMultiTapCount*maxMultiTapInterval);
		
		if(tapExisted==tap.count){
			tapExisted=0;
			if(onMultiTapE!=null) onMultiTapE(tap);
		}
	}
	
	void CheckMFMultiTap(Tap tap){
		MFtapExisted+=1;
		
		if(tap.count==maxMultiTapCount){
			MFtapExisted=0;
			if(onMFMultiTapE!=null) onMFMultiTapE(tap);
		}
		else{
			StartCoroutine(MFTapCoroutine(tap));
		}
	}
	
	IEnumerator MFTapCoroutine(Tap tap){
		yield return new WaitForSeconds(maxMultiTapCount*maxMultiTapInterval);
		
		if(MFtapExisted==tap.count){
			MFtapExisted=0;
			if(onMFMultiTapE!=null) onMFMultiTapE(tap);
		}
	}
	
	
	//*****************************************************************************//
	//standard tap event
	public static void MultiTap(Tap tap){
		if(tap.fingerCount==1){
			if(tap.count==1){
				if(onShortTapE!=null) onShortTapE(tap.pos);
			}
			else if(tap.count==2){
				if(onDoubleTapE!=null) onDoubleTapE(tap.pos);
			}
			
			if(instance.enableMultiTapFilter) instance.CheckMultiTap(tap);
			else if(onMultiTapE!=null) onMultiTapE(tap);
		}
		else{
			if(tap.fingerCount==2){
				if(tap.count==1){
					DFShortTap(tap.pos);
				}
				else if(tap.count==2){
					DFDoubleTap(tap.pos);
				}
			}
			
			if(instance.enableMultiTapFilter) instance.CheckMFMultiTap(tap);
			else if(onMFMultiTapE!=null) onMFMultiTapE(tap);
		}
	}
	
	public static void LongTap(Tap tap){
		if(tap.fingerCount>1){
			if(tap.fingerCount==2){
				if(onDFLongTapE!=null) onDFLongTapE(tap.pos);
			}
			if(onMFLongTapE!=null) onMFLongTapE(tap);
		}
		else{
			if(onLongTapE!=null) onLongTapE(tap);
		}
	}
	
	
	//*****************************************************************************//
	//charge
	public static void ChargeStart(ChargedInfo cInfo){
		if(cInfo.fingerCount>1){
			if(onMFChargeStartE!=null) onMFChargeStartE(cInfo);
		}
		else{
			if(onChargeStartE!=null) onChargeStartE(cInfo);
		}
	}
	
	public static void Charging(ChargedInfo cInfo){
		if(cInfo.fingerCount>1){
			if(cInfo.fingerCount==2) DFCharging(cInfo);
			if(onMFChargingE!=null) onMFChargingE(cInfo);
		}
		else{
			if(onChargingE!=null) onChargingE(cInfo);
		}
	}
	
	public static void ChargeEnd(ChargedInfo cInfo){
		if(cInfo.fingerCount>1){
			if(cInfo.fingerCount==2) DFChargingEnd(cInfo);
			if(onMFChargeEndE!=null) onMFChargeEndE(cInfo);
		}
		else{
			if(onChargeEndE!=null) onChargeEndE(cInfo);
		}
	}
	


	
	//*****************************************************************************//
	//dragging
	public static void DraggingStart(DragInfo dragInfo){
		if(dragInfo.fingerCount>1){
			if(onMFDraggingStartE!=null) onMFDraggingStartE(dragInfo);
		}
		else{
			if(onDraggingStartE!=null) onDraggingStartE(dragInfo);
		}
	}
	
	public static void Dragging(DragInfo dragInfo){
		if(dragInfo.fingerCount>1){
			if(dragInfo.fingerCount==2) DFDragging(dragInfo); //obsolete function call
			if(onMFDraggingE!=null) onMFDraggingE(dragInfo);
		}
		else{
			if(onDraggingE!=null) onDraggingE(dragInfo);
		}
	}
	
	public static void DraggingEnd(DragInfo dragInfo){
		if(dragInfo.fingerCount>1){
			if(dragInfo.fingerCount==2) DFDraggingEnd(dragInfo); //obsolete function call
			if(onMFDraggingEndE!=null) onMFDraggingEndE(dragInfo);
		}
		else{
			if(onDraggingEndE!=null) onDraggingEndE(dragInfo);
		}
	}
	
	
	
	
	

	//*****************************************************************************//
	//special event swipe/pinch/rotate
	public static void SwipeStart(SwipeInfo sw){
		if(onSwipeStartE!=null) onSwipeStartE(sw);
	}
	
	public static void Swiping(SwipeInfo sw){
		if(onSwipingE!=null) onSwipingE(sw);
	}
	
	public static void SwipeEnd(SwipeInfo sw){
		if(onSwipeEndE!=null) onSwipeEndE(sw);
	}
	
	public static void Swipe(SwipeInfo sw){
		if(onSwipeE!=null) onSwipeE(sw);
	}

	public static void Pinch(PinchInfo PI){
		if(onPinchE!=null) onPinchE(PI);
	}
	
	public static void Rotate(RotateInfo RI){
		if(onRotateE!=null) onRotateE(RI);
	}
	
	
	//*****************************************************************************//
	//native input down/on/up event
	//~ public static void OnTouchDown(Vector2 pos){
		//~ if(onTouchDownE!=null) onTouchDownE(pos);
	//~ }
	//~ public static void OnTouchUp(Vector2 pos){
		//~ if(onTouchUpE!=null) onTouchUpE(pos);
	//~ }
	//~ public static void OnTouch(Vector2 pos){
		//~ if(onTouchE!=null) onTouchE(pos);
	//~ }
	public static void OnTouchDown(Touch touch){
		if(onTouchDownPosE!=null) onTouchDownPosE(touch.position);
		if(onTouchDownE!=null) onTouchDownE(touch);
	}
	public static void OnTouchUp(Touch touch){
		if(onTouchUpPosE!=null) onTouchUpPosE(touch.position);
		if(onTouchUpE!=null) onTouchUpE(touch);
	}
	public static void OnTouch(Touch touch){
		if(onTouchPosE!=null) onTouchPosE(touch.position);
		if(onTouchE!=null) onTouchE(touch);
	}
	
	public static void OnMouse1Down(Vector2 pos){
		if(onMouse1DownE!=null) onMouse1DownE(pos);
	}
	public static void OnMouse1Up(Vector2 pos){
		if(onMouse1UpE!=null) onMouse1UpE(pos);
	}
	public static void OnMouse1(Vector2 pos){
		if(onMouse1E!=null) onMouse1E(pos);
	}
	
	public static void OnMouse2Down(Vector2 pos){
		if(onMouse2DownE!=null) onMouse2DownE(pos);
	}
	public static void OnMouse2Up(Vector2 pos){
		if(onMouse2UpE!=null) onMouse2UpE(pos);
	}
	public static void OnMouse2(Vector2 pos){
		if(onMouse2E!=null) onMouse2E(pos);
	}
	
	
	
	//utility for converting vector to angle
	public static float VectorToAngle(Vector2 dir){
		
		if(dir.x==0){
			if(dir.y>0) return 90;
			else if(dir.y<0) return 270;
			else return 0;
		}
		else if(dir.y==0){
			if(dir.x>0) return 0;
			else if(dir.x<0) return 180;
			else return 0;
		}
		
		float h=Mathf.Sqrt(dir.x*dir.x+dir.y*dir.y);
		float angle=Mathf.Asin(dir.y/h)*Mathf.Rad2Deg;
		
		if(dir.y>0){
			if(dir.x<0)  angle=180-angle;
		}
		else{
			if(dir.x>0)  angle=360+angle;
			if(dir.x<0)  angle=180-angle;
		}
		
		//Debug.Log(angle);
		return angle;
	}
	
	
	//utility for tracking a finger input based on fingerId
	public static Touch GetTouch(int ID){
		Touch touch=new Touch();
		
		if(Input.touchCount>0){
			//foreach(Touch touchTemp in Input.touches){
			for(int i=0; i<Input.touches.Length; i++){
				Touch touchTemp=Input.touches[i];
				if(touchTemp.fingerId==ID){
					touch=touchTemp;
					break;
				}
			}
		}
		
		return touch;
	}
	
	
	
	//***************************************************************************//
	//obsolete but still in use, possibly remove by the next update
	//standard tap event
	public delegate void ShortTapHandler(Vector2 pos); 
	public static event ShortTapHandler onShortTapE;
	
	//public delegate void LongTapHandler(Vector2 pos); 
	//public static event LongTapHandler onLongTapE;
	
	public delegate void DoubleTapHandler(Vector2 pos); 
	public static event DoubleTapHandler onDoubleTapE;
	
	
	//Dual-Finger Standard tap event 
	public delegate void DFShortTapHandler(Vector2 pos); 
	public static event DFShortTapHandler onDFShortTapE;
	
	public delegate void DFLongTapHandler(Vector2 pos); 
	public static event DFLongTapHandler onDFLongTapE;
	
	public delegate void DFDoubleTapHandler(Vector2 pos); 
	public static event DFDoubleTapHandler onDFDoubleTapE;
	
	public delegate void DFChargingHandler(ChargedInfo cInfo); 
	public static event DFChargingHandler onDFChargingE;
	
	public delegate void DFChargeEndHandler(ChargedInfo cInfo); 
	public static event DFChargeEndHandler onDFChargeEndE;
	
	
	//Dual-Finger dragging
	public delegate void DFDraggingHandler(DragInfo dragInfo);
	public static event DFDraggingHandler onDualFDraggingE;
	
	public delegate void DFDraggingEndHandler(Vector2 pos); 
	public static event DFDraggingEndHandler onDualFDraggingEndE;
	
	
	//*****************************************************************************//
	//standard
	public static void ShortTap(Vector2 pos){
		if(onShortTapE!=null) onShortTapE(pos);
	}
	
	//public static void LongTap(Vector2 pos){
	//	if(onLongTapE!=null) onLongTapE(pos);
	//}
	
	public static void DoubleTap(Vector2 pos){
		if(onDoubleTapE!=null) onDoubleTapE(pos);
	}
	
	
	//Dual Finger standard tap event
	public static void DFShortTap(Vector2 pos){
		if(onDFShortTapE!=null) onDFShortTapE(pos);
	}
	
	public static void DFLongTap(Vector2 pos){
		if(onDFLongTapE!=null) onDFLongTapE(pos);
	}
	
	public static void DFDoubleTap(Vector2 pos){
		if(onDFDoubleTapE!=null) onDFDoubleTapE(pos);
	}
	
	public static void DFCharging(ChargedInfo cInfo){
		if(onDFChargingE!=null) onDFChargingE(cInfo);
	}
	
	public static void DFChargingEnd(ChargedInfo cInfo){
		if(onDFChargeEndE!=null) onDFChargeEndE(cInfo);
	}
	
	//dual finger drag event
	public static void DFDragging(DragInfo dragInfo){
		if(onDualFDraggingE!=null) onDualFDraggingE(dragInfo);
	}
	
	public static void DFDraggingEnd(DragInfo dragInfo){
		if(onDualFDraggingEndE!=null) onDualFDraggingEndE(dragInfo.pos);
	}
	
	
}


