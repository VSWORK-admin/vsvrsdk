using DG.Tweening;
using System;
using UnityEngine;
using VSWorkSDK;
using VSWorkSDK.Data;

namespace Dll_Project
{
    public class ClickDemo : DllGenerateBase
    {
        public VRPointObjEventType PointEventType = VRPointObjEventType.VRPointClick;

        private Tweener tween;
        private bool isZoomIn = false;

        public override void Init()
        {
            Debug.Log("Click_Demo Init !");
        }

        public override void Awake()
        {
            Debug.Log("Click_Demo Awake !");

            VSEngine.Instance.OnEventPointClickHandler += OnPointClick;
            VSEngine.Instance.OnEventReceiveRoomSyncData += OnReceiveRoomSyncData;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            VSEngine.Instance.OnEventPointClickHandler -= OnPointClick;
            VSEngine.Instance.OnEventReceiveRoomSyncData -= OnReceiveRoomSyncData;
        }

        private void OnPointClick(GameObject obj)
        {
            if (obj.name.Contains("Cube"))
            {
                HandlePointedObject(obj);
            }
        }

        private void HandlePointedObject(GameObject obj)
        {
            string clickedName = $"{obj.name} has clicked!";
            Debug.Log($"VSEngine GetPointEventType + name = {obj.name}");

            VSEngine.Instance.ShowRoomMarqueeLog(clickedName, InfoColor.green, 5, false);
            LogEngineInfo();

            isZoomIn = !isZoomIn;
            RoomSycnData roomSycnData = new RoomSycnData()
            {
                a = "GameObjectScaleChange",
                b = obj.name,
                c = isZoomIn.ToString()
            };
            VSEngine.Instance.SendRoomSyncData(roomSycnData);
        }

        private void LogEngineInfo()
        {
            string version = VSEngine.Instance.GetApiVersion();
            string appname = VSEngine.Instance.GetAppName();
            Debug.Log($"VSEngine version {version} appname {appname}");
        }

        private void OnReceiveRoomSyncData(RoomSycnData data)
        {
            if (data.a == "GameObjectScaleChange" && GameObject.Find(data.b) != null)
            {
                HandleScaleChange(data);
            }
        }

        private void HandleScaleChange(RoomSycnData data)
        {
            GameObject gameObject = GameObject.Find(data.b);
            if (gameObject != null && bool.TryParse(data.c, out isZoomIn))
            {
                Transform transform = gameObject.GetComponent<Transform>();
                Vector3 targetScale = isZoomIn ? Vector3.one * 2 : Vector3.one;

                tween = transform.DOScale(targetScale, 0.5f).SetAutoKill(true);
            }
        }
    }
}