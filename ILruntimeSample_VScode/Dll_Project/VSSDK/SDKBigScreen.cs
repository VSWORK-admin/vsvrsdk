using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;
using UnityEngine.UI;

namespace Dll_Project
{
    public class SDKBigScreen : DllGenerateBase
    {
        string imgurl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=8470gceHBNdy3Gzo0AhHLBfPkhOZMA1JU5ZEadoh0tRZL7gTSgPph3Na2bkK7SgclC_EqAvIa1D5VunIyYG3KQsF1rsXnQGetIZ5FJFb8vwTj55DscI9cryU_cL-diLZxg&file_name=/%E6%9C%88%E5%AE%AB.png";
        string videourl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=391cpRuQtqnv0TA1-3SIan3EctbqKsGcYBEQ5Qx1sEKg3QGl3Ln06KUl-aIFii7dS9OD4HroRdID1i-jAxSgYmBXKh-cqzIaJ28TG9u9O8WSJJRoyWv2DE7BRm27vn5uTLsxnZ8cNSvKXm22_Oc&file_name=/11_m4v%E6%A0%BC%E5%BC%8F%E8%A7%86%E9%A2%91%20.m4v";
        string rtcurl = "rtmp://r2.vzan.com/v/slowlive_896208599916464437?zbid=2112941245&tpid=207420930";
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
            VSEngine.Instance.OnEventBigScreenShowImage += OnBigScreenShowImage;
            VSEngine.Instance.OnEventBigScreenPrepareVideo += OnBigScreenPrepareVideo;
            VSEngine.Instance.OnEventBigScreenRecieveRTSP += OnBigScreenRecieveRTSP;
            VSEngine.Instance.OnEventBigScreenVideoFrameReady += OnBigScreenShowVideoFrame;
            VSEngine.Instance.OnEventBigScreenUpdateImage += OnBigScreenUpdateImage;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventBigScreenShowImage -= OnBigScreenShowImage;
            VSEngine.Instance.OnEventBigScreenPrepareVideo -= OnBigScreenPrepareVideo;
            VSEngine.Instance.OnEventBigScreenRecieveRTSP -= OnBigScreenRecieveRTSP;
            VSEngine.Instance.OnEventBigScreenVideoFrameReady -= OnBigScreenShowVideoFrame;
            VSEngine.Instance.OnEventBigScreenUpdateImage -= OnBigScreenUpdateImage;
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
                VSEngine.Instance.ShowBigScreen(true);
                VSEngine.Instance.SetBigScreenProperty(Vector3.zero,Quaternion.Euler(Vector3.zero),Vector3.one*1.2f,30,true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.SetBigScreenSize(1920, 1080);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    url = imgurl,
                    name = "TestImage",
                    fileMd5 = "b741abdadf94ce087607d344d168567d",
                };
                VSEngine.Instance.SetBigScreenShowImage(file);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                VSEngine.Instance.SetBigScreenPrepareVideo(videourl);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                VSEngine.Instance.SetBigScreenShowRTSP(rtcurl);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
               // VSEngine.Instance.OpenBigScreenWebView("www.baidu.com", true, true,false,true, 1920, 1080, false,Vector2.zero);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
               // VSEngine.Instance.CloseBigScreenWebView();
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    url = imgurl,
                    name = "TestImage",
                    fileMd5 = "b741abdadf94ce087607d344d168567d",
                };
                VSEngine.Instance.LoadImageFile(file, (res) =>
                {
                    VSEngine.Instance.SetBigScreenShowImage(res.img, true);
                }, (p) =>
                {
                    Debug.Log("Load Image Progress file.name " + file.url + " progress " + p);
                });
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnBigScreenShowImage(Texture2D tex)
        {
            Debug.Log("VSEngine OnBigScreenShowImage tex " + tex.width + "x"+ tex.height);
        }
        void OnBigScreenPrepareVideo(string videopath)
        {
            Debug.Log("VSEngine OnBigScreenPrepareVideo videopath " + videopath);
        }
        void OnBigScreenRecieveRTSP(string url)
        {
            Debug.Log("VSEngine OnBigScreenRecieveRTSP url " + url);
        }
        void OnBigScreenShowVideoFrame(Texture2D tex)
        {
            if (tex != null)
                Debug.Log("VSEngine OnBigScreenShowVideoFrame tex " + tex.width + "x" + tex.height);
        }
        void OnBigScreenUpdateImage(Texture2D tex)
        {
            if (tex != null)
                Debug.Log("VSEngine OnBigScreenUpdateImage tex " + tex.width + "x" + tex.height);
        }
    }
}
