using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKRoomEvent : DllGenerateBase
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
            VSEngine.Instance.OnEventRoomConnected += OnRoomConnected;
            VSEngine.Instance.OnEventRoomConnectError += OnRoomConnectError;
            VSEngine.Instance.OnEventRoomDisConnected += OnRoomDisconnected;
            VSEngine.Instance.OnEventRoomConnectClose += OnRoomConnectClose;
            VSEngine.Instance.OnEventSelfLeaveRoom += OnSelfLeaveRoom;
            VSEngine.Instance.OnEventRoomConnectNewChannel += OnConnectNewChannel;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventRoomConnected -= OnRoomConnected;
            VSEngine.Instance.OnEventRoomConnectError -= OnRoomConnectError;
            VSEngine.Instance.OnEventRoomDisConnected -= OnRoomDisconnected;
            VSEngine.Instance.OnEventRoomConnectClose -= OnRoomConnectClose;
            VSEngine.Instance.OnEventSelfLeaveRoom -= OnSelfLeaveRoom;
            VSEngine.Instance.OnEventRoomConnectNewChannel -= OnConnectNewChannel;
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
                VSEngine.Instance.ConnectToNewChannel("d126cf5d-25f7-4fff-b353-44d232eb6ee0", "lbtimqyu");
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.ShowRoomMarqueeLog("I AM OK", InfoColor.black, 15, false);
            }

        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnRoomConnected(string roomserverurl)
        {
            Debug.Log("VSEngine OnRoomConnected serverurl " + roomserverurl);
        }
        void OnRoomConnectError(string roomserverurl)
        {
            Debug.Log("VSEngine OnRoomConnectError serverurl " + roomserverurl);
        }
        void OnRoomDisconnected(string serverurl)
        {
            Debug.Log("VSEngine OnRoomDisconnected serverurl " + serverurl);
        }
        void OnRoomConnectClose(string serverurl)
        {
            Debug.Log("VSEngine OnRoomConnectClose serverurl " + serverurl);
        }
        void OnSelfLeaveRoom()
        {
            Debug.Log("VSEngine OnSelfLeaveRoom ");
        }
        void OnConnectNewChannel(VRRootChanelRoom data)
        {
            Debug.Log("VSEngine OnConnectNewChannel data roomid " + data.roomid + " voiceid " + data.voiceid);
        }
    }
}
