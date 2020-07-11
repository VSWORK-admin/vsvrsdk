// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class FsmNavMeshPath : MonoBehaviour {
	
	//Corner points of path
	public Vector3[] corners;
	/*
	{
		get { 
			if (path== null)
			{
			 return null;
			}
			return path.corners;
		}
	}
	*/
	
	public NavMeshPathStatus status
	{
		get
		{ 
			if (path== null)
			{
			 return NavMeshPathStatus.PathInvalid;
			}	
		return path.status;
		}
	}

	NavMeshPath _path;

	public NavMeshPath path
	{
		set{
			_path = value;
			corners = _path.corners;
		}
		get{
			return _path;
		}
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	void ClearCorners()
	{
		path.ClearCorners();
	}
	
	public string GetStatusString()
	{
		if (path ==null){
			return "n/a";
		}else{
			return path.status.ToString();
		}
	}
	
}