
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VSWorkSDK;
using VSWorkSDK.Data;

namespace Dll_Project
{
    public class ClickDemo : DllGenerateBase
    {
        public VRPointObjEventType PointEventType = VRPointObjEventType.VRPointClick;


        public GameObject ClickedObj;


        public string clickedName = string.Empty;

        public string Recieve_A = string.Empty;
        public string Recieve_B = string.Empty;

        public override void Init()
        {
            Debug.Log("Click_Demo Init !");
        }

        public override void Awake()
        {
            Debug.Log("Click_Demo Awake !");
    
            VSEngine.Instance.OnEventPointClickHandler += GetPointEventType;
            VSEngine.Instance.OnEventReceiveRoomSyncData += RecieveCChangeObj;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            VSEngine.Instance.OnEventPointClickHandler -= GetPointEventType;
            VSEngine.Instance.OnEventReceiveRoomSyncData -= RecieveCChangeObj;
        }

        public override void OnEnable()
        {
            Debug.Log("Click_Demo OnEnable !");
        }

        public override void OnDisable()
        {
        }
 
        void GetPointEventType(GameObject obj)
        {
            ClickedObj = obj;
            Debug.Log("VSEngine GetPointEventType + name = " + obj.name);
            HandleGetPointedObj();
        }
        void HandleGetPointedObj()
        {
            clickedName = ClickedObj.name + "has clicked !XXX";
            
            VSEngine.Instance.ShowRoomMarqueeLog(clickedName, InfoColor.green,5,false);
            string version = VSEngine.Instance.GetApiVersion();
            string appname = VSEngine.Instance.GetAppName();
            Debug.Log("VSEngine version " + version + " appname " + appname);

        }

        void RecieveCChangeObj(RoomSycnData rinfo)
        {
            Recieve_A = rinfo.a;
            Recieve_B = rinfo.b;

            HandleCChangeObj();
        }
        void HandleCChangeObj()
        {

        }
 
    }
}