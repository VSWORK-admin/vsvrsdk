using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKScreenShare : DllGenerateBase
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
            VSEngine.Instance.OnEventUserShareScreen += OnUserShareScreen;
            VSEngine.Instance.OnEventShareScreenFrameReady += OnScreenShareFrameReady;
            VSEngine.Instance.OnEventScreenShareTextureSizeChange += OnScreenShareTextureSizeChange;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventUserShareScreen -= OnUserShareScreen;
            VSEngine.Instance.OnEventShareScreenFrameReady -= OnScreenShareFrameReady;
            VSEngine.Instance.OnEventScreenShareTextureSizeChange -= OnScreenShareTextureSizeChange;
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
                VSEngine.Instance.StartShareScreen();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.StopShareScreen();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                VSEngine.Instance.ShareFirstScreen();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                VSEngine.Instance.EnableScreenShareSound(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                VSEngine.Instance.StartShareCamera();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                VSEngine.Instance.StopShareCamera();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                VSEngine.Instance.SetRoomMultiScreenShare(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SetupScreenShareView shareview = new SetupScreenShareView()
                {
                    shareuserid = VSEngine.Instance.GetMyAvatarID(),
                    panelname = "",
                    bshow = true,
                };
                VSEngine.Instance.SetUpScreenShareToViewPanel(shareview);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                VSEngine.Instance.CloseUserScreenShare(VSEngine.Instance.GetMyAvatarID());
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnUserShareScreen(UserScreenShareReqExData data)
        {
            Debug.Log("VSEngine OnUserShareScreen  data " + data.shareuserid);
        }
        void OnScreenShareFrameReady(string shareuserid)
        {
            Debug.Log("VSEngine OnScreenShareFrameReady  shareuser " + shareuserid);
        }
        void OnScreenShareTextureSizeChange(string viewpanel,int width,int height)
        {
            Debug.Log("VSEngine OnScreenShareTextureSizeChange  viewpanel " + viewpanel + " width " + width + " height " + height);
        }
    }
}
