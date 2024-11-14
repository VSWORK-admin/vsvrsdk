
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

        private Tweener tween;

        public string clickedName = string.Empty;

        public string Recieve_A = string.Empty;
        public string Recieve_B = string.Empty;

        private bool isZoomIn = false;
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

            isZoomIn = !isZoomIn;
            RoomSycnData roomSycnData = new RoomSycnData()
            {
                a = "GameObjectScaleChange",
                b = ClickedObj.name,
                c = isZoomIn.ToString()
            };
            VSEngine.Instance.SendRoomSyncData(roomSycnData);
        }

        void RecieveCChangeObj(RoomSycnData rinfo)
        {
           

            HandleCChangeObj(rinfo);
        }
        void HandleCChangeObj(RoomSycnData rinfo)
        {
            switch (rinfo.a)
            {
                case "GameObjectScaleChange":
                    GameObject gameObject = GameObject.Find(rinfo.b);
                    if (gameObject!=null)
                    {
                        if (bool.TryParse(rinfo.c,out isZoomIn))
                        {
                            if (isZoomIn)
                            {
                                tween = gameObject.GetComponent<Transform>().DOScale(Vector3.one * 2, 0.5f);

                                tween.SetAutoKill(true);
                            }
                            else
                            {
                                tween = gameObject.GetComponent<Transform>().DOScale(Vector3.one , 0.5f);

                                tween.SetAutoKill(true);
                            }
                        }
                       
                    }
                    break;
                default:
                    break;
            }
        }
 
    }
}