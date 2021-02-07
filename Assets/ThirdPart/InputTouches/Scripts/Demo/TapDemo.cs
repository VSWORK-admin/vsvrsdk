using UnityEngine;
using System.Collections;

public class TapDemo : MonoBehaviour {

	public ParticleSystem Indicator;
	
	public Transform shortTapObj;
	public Transform longTapObj;
	public Transform doubleTapObj;
	public Transform chargeObj;
	
	public TextMesh chargeTextMesh;
	
	public Transform dragObj1;
	public TextMesh dragTextMesh1;
	
	public Transform dragObj2;
	public TextMesh dragTextMesh2;
	
	
	// Use this for initialization
	void Start () {
		DemoSceneUI.SetSceneTitle("");
		DemoSceneUI.SetSceneInstruction("interact with each object using the interaction stated on top of each of them");
	}
	
	void OnEnable(){
		//these events are obsolete, replaced by onMultiTapE, but it's still usable
		//IT_Gesture.onShortTapE += OnShortTap;
		//IT_Gesture.onDoubleTapE += OnDoubleTap;
		
		IT_Gesture.onMultiTapE += OnMultiTap;
		IT_Gesture.onLongTapE += OnLongTap;
		
		IT_Gesture.onChargingE += OnCharging;
		IT_Gesture.onChargeEndE += OnChargeEnd;
		
		IT_Gesture.onDraggingStartE += OnDraggingStart;
		IT_Gesture.onDraggingE += OnDragging;
		IT_Gesture.onDraggingEndE += OnDraggingEnd;
	}
	
	void OnDisable(){
		//these events are obsolete, replaced by onMultiTapE, but it's still usable
		//IT_Gesture.onShortTapE -= OnShortTap;
		//IT_Gesture.onDoubleTapE -= OnDoubleTap;
		
		IT_Gesture.onMultiTapE -= OnMultiTap;
		IT_Gesture.onLongTapE -= OnLongTap;
		
		IT_Gesture.onChargingE -= OnCharging;
		IT_Gesture.onChargeEndE -= OnChargeEnd;
		
		IT_Gesture.onDraggingStartE -= OnDraggingStart;
		IT_Gesture.onDraggingE -= OnDragging;
		IT_Gesture.onDraggingEndE -= OnDraggingEnd;
	}
	
	
	
	//called when a multi-Tap event is detected
	void OnMultiTap(Tap tap){
		//do a raycast base on the position of the tap
		Ray ray = Camera.main.ScreenPointToRay(tap.pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			//if the tap lands on the shortTapObj, then shows the effect.
			if(hit.collider.transform==shortTapObj){
				//place the indicator at the object position and assign a random color to it
				Indicator.transform.position=shortTapObj.position;
				
				var main = Indicator.main;
				main.startColor = GetRandomColor();	//Indicator.startColor=GetRandomColor();
				
				//emit a set number of particle
				Indicator.Emit(30);
			}
			//if the tap lands on the doubleTapObj
			else if(hit.collider.transform==doubleTapObj){
				//check to make sure if the tap count matches
				if(tap.count==2){
					//place the indicator at the object position and assign a random color to it
					Indicator.transform.position=doubleTapObj.position;
					
					var main = Indicator.main;
					main.startColor = GetRandomColor();	//Indicator.startColor=GetRandomColor();
					
					//emit a set number of particle
					Indicator.Emit(30);
				}
			}
		}
	}
	
	
	
	//called when a long tap event is ended
	void OnLongTap(Tap tap){
		//do a raycast base on the position of the tap
		Ray ray = Camera.main.ScreenPointToRay(tap.pos);
		RaycastHit hit;
		//if the tap lands on the longTapObj
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.collider.transform==longTapObj){
				//place the indicator at the object position and assign a random color to it
				Indicator.transform.position=longTapObj.position;
				
				var main = Indicator.main;
				main.startColor = GetRandomColor();	//Indicator.startColor=GetRandomColor();
				
				//emit a set number of particle
				Indicator.Emit(30);
			}
		}
	}
	
	//called when a double tap event is ended
	//this event is used for onDoubleTapE in v1.0, it's still now applicabl
	void OnDoubleTap(Vector2 pos){
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.collider.transform==doubleTapObj){
				//place the indicator at the object position and assign a random color to it
				Indicator.transform.position=doubleTapObj.position;
				var main = Indicator.main;
				main.startColor = GetRandomColor();	//Indicator.startColor=GetRandomColor();
				//emit a set number of particle
				Indicator.Emit(30);
			}
		}
	}
	
	
	
	//called when a charging event is detected
	void OnCharging(ChargedInfo cInfo){
		Ray ray = Camera.main.ScreenPointToRay(cInfo.pos);
		RaycastHit hit;
		//use raycast at the cursor position to detect the object
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.collider.transform==chargeObj){
				//display the charged percentage on screen
				chargeTextMesh.text="Charging "+(cInfo.percent*100).ToString("f1")+"%";
			}
		}
	}
	
	//called when a charge event is ended
	void OnChargeEnd(ChargedInfo cInfo){
		Ray ray = Camera.main.ScreenPointToRay(cInfo.pos);
		RaycastHit hit;
		//use raycast at the cursor position to detect the object
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.collider.transform==chargeObj){
				//place the indicator at the object position and assign a random color to it
				Indicator.transform.position=chargeObj.position;
				
				var main = Indicator.main;
				main.startColor = GetRandomColor();	//Indicator.startColor=GetRandomColor();
				
				//adjust the indicator speed with respect to the charged percent
				main.startSpeed=1+3*cInfo.percent;	//Indicator.startSpeed=1+3*cInfo.percent;
				
				//emit a set number of particles with respect to the charged percent
				Indicator.Emit((int)(10+cInfo.percent*75f));
				
				//reset the particle speed, since it's shared by other event
				StartCoroutine(ResumeSpeed());
			}
		}
		chargeTextMesh.text="HoldToCharge";
	}
	
	//reset the particle emission speed of the indicator
	IEnumerator ResumeSpeed(){
		var main = Indicator.main;
		yield return new WaitForSeconds(main.startLifetime.constant);
		main.startSpeed=2;
	}
	
	void Update(){
		//Debug.Log(currentDragIndex+"   "+dragByMouse);
	}
	
	/*
	bool cursorOnUIFlag=false;
	void OnDraggingStart(DragInfo dragInfo){
		if(IsCursorOnUI(dragInfo.pos)){
			cursorOnUIFlag=false;
			return;
		}
		
		cursorOnUIFlag=true;
	}
	void OnDraggingEnd(DragInfo dragInfo){
		if(!cursorOnUIFlag) return;
		if(IsCursorOnUI(dragInfo.pos)) return;
		
		//insert custom code
	}
	*/
	
	private Vector3 dragOffset1;
	private Vector3 dragOffset2;
	
	private int currentDragIndex1=-1;
	private int currentDragIndex2=-1;
	void OnDraggingStart(DragInfo dragInfo){
		//currentDragIndex=dragInfo.index;
		
		//if(currentDragIndex==-1){
			Ray ray = Camera.main.ScreenPointToRay(dragInfo.pos);
			RaycastHit hit;
			//use raycast at the cursor position to detect the object
			if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
				//if the drag started on dragObj1
				if(hit.collider.transform==dragObj1){
					Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(dragInfo.pos.x, dragInfo.pos.y, 30));
					dragOffset1=dragObj1.position-p;
					
					//change the scale of dragObj1, give the user some visual feedback
					dragObj1.localScale*=1.1f;
					//latch dragObj1 to the cursor, based on the index
					Obj1ToCursor(dragInfo);
					currentDragIndex1=dragInfo.index;
				}
				//if the drag started on dragObj2
				else if(hit.collider.transform==dragObj2){
					Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(dragInfo.pos.x, dragInfo.pos.y, 30));
					dragOffset2=dragObj2.position-p;
					
					//change the scale of dragObj2, give the user some visual feedback
					dragObj2.localScale*=1.1f;
					//latch dragObj2 to the cursor, based on the index
					Obj2ToCursor(dragInfo);
					currentDragIndex2=dragInfo.index;
				}
			}
		//}
	}
	
	//triggered on a single-finger/mouse dragging event is on-going
	void OnDragging(DragInfo dragInfo){
		//if the dragInfo index matches dragIndex1, call function to position dragObj1 accordingly
		if(dragInfo.index==currentDragIndex1){
			Obj1ToCursor(dragInfo);
		}
		//if the dragInfo index matches dragIndex2, call function to position dragObj2 accordingly
		else if(dragInfo.index==currentDragIndex2){
			Obj2ToCursor(dragInfo);
		}
	}
	
	//assign dragObj1 to the dragInfo position, and display the appropriate tooltip
	void Obj1ToCursor(DragInfo dragInfo){
		//~ LayerMask layer=~(1<<dragObj1.gameObject.layer);
		//~ Ray ray = Camera.main.ScreenPointToRay (dragInfo.pos);
		//~ RaycastHit hit;
		//~ if (Physics.Raycast (ray, out hit, Mathf.Infinity, layer)) {
			//~ dragObj1.position=hit.point;
		//~ }
		
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(dragInfo.pos.x, dragInfo.pos.y, 30));
		dragObj1.position=p+dragOffset1;
		
		if(dragInfo.isMouse){
			dragTextMesh1.text="Dragging with mouse"+(dragInfo.index+1);
		}
		else{
			dragTextMesh1.text="Dragging with finger"+(dragInfo.index+1);
		}
	}
	
	//assign dragObj2 to the dragInfo position, and display the appropriate tooltip
	void Obj2ToCursor(DragInfo dragInfo){
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(dragInfo.pos.x, dragInfo.pos.y, 30));
		dragObj2.position=p+dragOffset2;
		
		if(dragInfo.isMouse){
			dragTextMesh2.text="Dragging with mouse"+(dragInfo.index+1);
		}
		else{
			dragTextMesh2.text="Dragging with finger"+(dragInfo.index+1);
		}
	}
	
	void OnDraggingEnd(DragInfo dragInfo){
		
		//drop the dragObj being drag by this particular cursor
		if(dragInfo.index==currentDragIndex1){
			currentDragIndex1=-1;
			dragObj1.localScale*=10f/11f;
			
			Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(dragInfo.pos.x, dragInfo.pos.y, 30));
			dragObj1.position=p+dragOffset1;
			dragTextMesh1.text="DragMe";
		}
		else if(dragInfo.index==currentDragIndex2){
			currentDragIndex2=-1;
			dragObj2.localScale*=10f/11f;
			
			Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(dragInfo.pos.x, dragInfo.pos.y, 30));
			dragObj2.position=p+dragOffset2;
			dragTextMesh2.text="DragMe";
		}
		
	}
	
	
	//return a random color when called
	private Color GetRandomColor(){
		return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
	}
	

	/*
	private bool instruction=false;
	void OnGUI(){
		if(!instruction){
			if(GUI.Button(new Rect(10, 55, 130, 35), "Instruction On")){
				instruction=true;
			}
		}
		else{
			if(GUI.Button(new Rect(10, 55, 130, 35), "Instruction Off")){
				instruction=false;
			}
			
			GUI.Box(new Rect(10, 100, 200, 65), "");
			
			GUI.Label(new Rect(15, 105, 190, 65), "interact with each object using the interaction stated on top of each of them");
		}
	}
	*/
	
	
	//************************************************************************************************//
	//following function is used in v1.0 and is now obsolete
	
	//called when a short tap event is ended
	//this event is used for onShortTapE in v1.0, it's still now applicable
	void OnShortTap(Vector2 pos){
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.collider.transform==shortTapObj){
				//place the indicator at the object position and assign a random color to it
				Indicator.transform.position=shortTapObj.position;
				var main = Indicator.main;
				main.startColor=GetRandomColor();
				//emit a set number of particle
				Indicator.Emit(30);
			}
		}
	}

}
