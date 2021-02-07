using UnityEngine;
using System.Collections;
using System.Collections.Generic;



	public class Tap{
		public Vector2 posInitial;
		public Vector2 pos;
		public int count;
		
		public int fingerCount=1;
		public Vector2[] positions=new Vector2[1];
		
		public bool isMouse=false;
		public int index=0;
		public int[] indexes=new int[1];
		
		public Tap(Vector2 p):this(p, p, 1, 0, false){}
		public Tap(Vector2 p, int c):this(p, p, c, 0, false){}
		public Tap(Vector2 p, int c, int ind):this(p, p, c, ind, false){}
		public Tap(Vector2 p1, Vector2 p2, int c, int ind):this(p1, p2, c, ind, false){}
		public Tap(Vector2 p,  int c, int ind, bool im):this(p, p, c, ind, im){}
		public Tap(Vector2 p1, Vector2 p2, int c, int ind, bool im){
			posInitial=p1;
			pos=p2;
			count=c;
			index=ind;
			isMouse=im;
			
			positions[0]=pos;
			indexes[0]=index;
		}
		public Tap(int c, int fc, Vector2[] ps, int[] inds){
			count=c;
			fingerCount=fc;
			positions=ps;
			indexes=inds;
			
			Vector2 pos=Vector2.zero;
			
			//foreach(Vector2 p in positions){
			//	pos+=p;
			//}
			for(int i=0; i<positions.Length; i++){
				pos+=positions[i];
			}
			
			pos/=positions.Length;
		}
	}

	public class ChargedInfo{
		public float percent=0;
		public Vector2 pos;
		
		public int fingerCount=1;
		public Vector2[] positions=new Vector2[1];
		
		public bool isMouse=false;
		public int index=0;
		public int[] indexes=new int[1];
		
		//obsolete member
		public Vector2 pos1;
		public Vector2 pos2;
		
		public ChargedInfo(Vector2 p, float val):this(p, val, 0, false) { }
		public ChargedInfo(Vector2 p, float val, int ind, bool im){
			pos=p;
			percent=val;
			index=ind;
			isMouse=im;
			
			positions[0]=pos;
			indexes[0]=ind;
		}
		
		public ChargedInfo(Vector2 p, Vector2[] posL, float val, int[] inds){
			pos=p;
			positions=posL;
			percent=val;
			indexes=inds;
			fingerCount=indexes.Length;
		}
		
		//obsolete constructor
		public ChargedInfo(Vector2 p, float val, Vector2 p1, Vector2 p2):this(p, val, p1, p2, 0, false){}
		public ChargedInfo(Vector2 p, float val, Vector2 p1, Vector2 p2, int ind, bool im){
			pos=p;
			percent=val;
			pos1=p1;
			pos2=p2;
			index=ind;
			isMouse=im;
		}
	}

	public class DragInfo{
		public Vector2 pos;
		public Vector2 delta;
		
		public int fingerCount=1;
		
		public bool isFlick=false;
		
		public bool isMouse;
		public int index;
		
		public DragInfo(Vector2 p, Vector2 dir, int fCount):this(p, dir, fCount, 0, false, false) { }
		public DragInfo(Vector2 p, Vector2 dir, int fCount, bool iFlick):this(p, dir, fCount, 0, iFlick, false) { }
		//public DragInfo(Vector2 p, Vector2 dir, int ind, bool im):this(p, dir, 1, ind, false, im) { }
		public DragInfo(Vector2 p, Vector2 dir, int fCount, int ind, bool im):this(p, dir, fCount, ind, false, im){}
		public DragInfo(Vector2 p, Vector2 dir, int fCount, int ind, bool iFlick, bool im){
			pos=p;
			delta=dir;
			fingerCount=fCount;
			index=ind;
			isFlick=iFlick;
			isMouse=im;
		}
	}

	public class SwipeInfo{
		public Vector2 startPoint;
		public Vector2 endPoint;
		
		public Vector2 direction;
		public float angle;
		
		public float duration;
		public float speed;
		
		public int index=0;
		public bool isMouse=false;
		
		public SwipeInfo(Vector2 p1, Vector2 p2, Vector2 dir, float startT, int ind, bool im){
			startPoint=p1;
			endPoint=p2;
			direction=dir;
			angle=IT_Gesture.VectorToAngle(dir);
			duration=Time.realtimeSinceStartup-startT;
			speed=dir.magnitude/duration;
			index=ind;
			isMouse=im;
		}
		
		public float GetMagnitude(){
			return (endPoint-startPoint).magnitude;
		}
	}


	public class RotateInfo{
		public float magnitude;
		public Vector2 pos1;
		public Vector2 pos2;
		
		public RotateInfo(float mag, Vector2 p1, Vector2 p2){
			magnitude=mag;
			pos1=p1;
			pos2=p2;
		}
	}

	public class PinchInfo{
		public float magnitude;
		public Vector2 pos1;
		public Vector2 pos2;
		
		public PinchInfo(float mag, Vector2 p1, Vector2 p2){
			magnitude=mag;
			pos1=p1;
			pos2=p2;
		}
	}


//}

