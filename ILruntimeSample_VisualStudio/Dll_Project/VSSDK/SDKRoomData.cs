using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;
using VSWorkSDK.Data;

namespace Dll_Project
{
    public class SDKRoomData : DllGenerateBase
    {
        public override void Init()
        {
            base.Init();
        }
        public override void Awake()
        {
            base.Awake();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            VSEngine.Instance.OnEventReceiveRoomSavedData += OnReceiveRoomSavedData;
            VSEngine.Instance.OnEventReceiveRoomSyncData += OnReceiveRoomSyncData;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventReceiveRoomSavedData -= OnReceiveRoomSavedData;
            VSEngine.Instance.OnEventReceiveRoomSyncData -= OnReceiveRoomSyncData;
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                VSEngine.Instance.SaveDataToRoom("TestSaveData","I AM OK");
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.RequestRoomSavedData("TestSaveData",(value)=> {
                    Debug.Log("VSEngine OnReceiveRoomSavedData 1 value " + JsonMapper.ToJson(value));
                });
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                VSEngine.Instance.RequestRoomSavedData("TestSaveDataNull", (value) => {
                    Debug.Log("VSEngine OnReceiveRoomSavedData 1 value " + JsonMapper.ToJson(value));
                });
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                RoomSycnData info = new RoomSycnData();
                info.a = "Test";
                info.b = "I AM OK";
                VSEngine.Instance.SendRoomSyncData(info);
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnReceiveRoomSavedData(Dictionary<string,string> data)
        {
            Debug.Log("VSEngine OnReceiveRoomSavedData data " + JsonMapper.ToJson(data));
        }
        void OnReceiveRoomSyncData(RoomSycnData data)
        {
            Debug.Log("VSEngine OnReceiveRoomSyncData data " + JsonMapper.ToJson(data));
        }
    }
}
