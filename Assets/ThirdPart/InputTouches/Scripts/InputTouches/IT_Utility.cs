using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class IT_Utility : MonoBehaviour {

	private static float lastCastTime=0;
	private static List<RaycastResult> raycastResults = new List<RaycastResult>();
	
	//return true if the on screen position passed is hovering on top of a UI element, false if otherwise
	public static bool IsCursorOnUI(Vector2 cursorPos){
		if(EventSystem.current==null) return false;
		
		if(lastCastTime!=Time.time){
			lastCastTime=Time.time;
			
			PointerEventData pointer = new PointerEventData(EventSystem.current);
			pointer.position = cursorPos;
			EventSystem.current.RaycastAll(pointer, raycastResults);
		}
		
		return raycastResults.Count>0 ? true : false ;
	}
	
	//return the UI element current being hovered by the on screen position passed
	public static GameObject GetHoveredUIElement(Vector2 cursorPos){
		if(EventSystem.current==null) return null;
		
		if(lastCastTime!=Time.time){
			lastCastTime=Time.time;
			
			PointerEventData pointer = new PointerEventData(EventSystem.current);
			pointer.position = cursorPos;
			EventSystem.current.RaycastAll(pointer, raycastResults);
		}
		
		return raycastResults.Count>0 ? raycastResults[0].gameObject : null ;
	}
	
	
	public static GameObject GetHovered2DObject(Vector2 cursorPos, Camera camera=null){
		if(camera==null) camera=Camera.main;
		if(camera==null) return null;
		Ray ray = camera.ScreenPointToRay(cursorPos);
		RaycastHit2D hit2D=Physics2D.GetRayIntersection(ray); 
		return hit2D.collider!=null ? hit2D.collider.gameObject : null;
	}
	
	
	public static GameObject GetHovered3DObject(Vector2 cursorPos, Camera camera=null){
		if(camera==null) camera=Camera.main;
		if(camera==null) return null;
		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay(cursorPos);
        if(Physics.Raycast(ray, out hit)) return hit.collider.gameObject;
		return null;
	}
}
