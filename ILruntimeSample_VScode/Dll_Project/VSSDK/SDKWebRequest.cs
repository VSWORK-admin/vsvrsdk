using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKWebRequest : DllGenerateBase
    {
        string imgurl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=a7b2jRPdODLl0LXLqJfLN33Bex0I9u636VDkyq_h_fbdvKJzPAV0AtSyooGd-8aU9YHbf9D8JFNhRViR-PsG1Yf5WTl2_kdRl3zqvp__w_B43KIewI2VJzMQk5hJ5bv-4SlswRKp-GWWLave&file_name=/1-%E5%86%88%E4%BB%81%E6%B3%A2%E9%BD%90%E5%B3%B0.jpg";
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
            VSEngine.Instance.OnEventReceiveStreamWebData += OnStreamWebData;
            VSEngine.Instance.OnEventReceiveStreamWebResult += OnStreamWebResult;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventReceiveStreamWebData -= OnStreamWebData;
            VSEngine.Instance.OnEventReceiveStreamWebResult -= OnStreamWebResult;
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
                WsMediaFile file = new WsMediaFile()
                {
                    url = imgurl,
                    name = "Test",
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.DownloadAndCacheFile(file,
                (res)=> 
                {
                    VSEngine.Instance.ShowRoomMarqueeLog("DownloadAndCacheFile Done File Local Path " + res.localpath,InfoColor.black,3,false);
                },null);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    url = imgurl,
                    name = "Test",
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.LoadImageFile(file,
                (res) =>
                {
                    VSEngine.Instance.ShowRoomMarqueeLog("LoadImageFile Done width*height " + res.img.width+"x"+res.img.height, InfoColor.black, 3, false);
                });
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WWWForm form = new WWWForm();
                form.AddField("", "");
                VSEngine.Instance.PostWebRequest("", form,
                (res) =>
                {
                    VSEngine.Instance.ShowRoomMarqueeLog("VSEngine Post ret " + res, InfoColor.black, 3, false);
                }, null);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                string url = "http://vsapitest.vswork.online/space/82803e52-2f8b-475b-bc87-801b682fd972/vsai/chat/stream";
                string content = "Count to 100 , . E.g., 1, 2, 3, ...";
                VSEngine.Instance.StartStreamChatWebRequest(url, content);
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnStreamWebData(string data)
        {
            Debug.Log("SDKEngine OnStreamWebData data " + data);
        }
        void OnStreamWebResult(string data)
        {
            Debug.Log("SDKEngine OnStreamWebResult data " + data);
        }
    }
}
