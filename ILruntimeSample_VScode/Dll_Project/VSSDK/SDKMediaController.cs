using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKMediaController : DllGenerateBase
    {
        private string pdfurl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=4fb1JSicWrIAa057S346VmNqJD7ZYj7Q58iMbw_sUi0zS_4Muyu93h9wKAmQmAw2-R8ZjDCw2x341H0srw6gudpMdvXG5fQD1d8eGJu7SMpQYlKihw_E6ae2ZB9i-CC0y8lEDhdviFCN4SNhrzsCjjIZdXZbJ4bTrQbIyTw&file_name=/VSWORK%E5%85%83%E5%AE%87%E5%AE%99%E5%BC%95%E6%93%8E-%E5%85%AC%E5%8F%B8%E4%BB%8B%E7%BB%8D.pdf";
        private string videourl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=1fcd4C18J3d7_gfHe2ClQN_oiwOInYQfZn5fIZIPpPv1sXZNczqhq2vzeskA9lUILe6BVhDzrFlcZBpBZ-V6CF4Bhlh5jNZLV9jeM3zQMplQord1X2ao6STRbofGNLZ_RnR9N3Q3p7xbcpf3QabrQA&file_name=/0-vswork%E5%AE%A3%E4%BC%A0%E7%89%87720P.mp4";
        private GameObject clicked;
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
            VSEngine.Instance.OnEventReceivePdfPageCount += OnReceivePageCount;
            VSEngine.Instance.OnEventReceivePdfRenderTexture += OnReceivePdfRenderTexture;
            VSEngine.Instance.OnEventReceiveVideoFirstFrameReady += OnReceiveVideoFirstFrame;
            VSEngine.Instance.OnEventReceiveVideoCurrentTime += OnReceiveVideoCurrentTime;
            VSEngine.Instance.OnEventReceiveVideoInfo += OnReceiveVideoInfo;
            VSEngine.Instance.OnEventReceiveVideoFinish += OnReceiveVideoFinish;
            VSEngine.Instance.OnEventPointClickHandler += OnPointClick;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventReceivePdfPageCount -= OnReceivePageCount;
            VSEngine.Instance.OnEventReceivePdfRenderTexture -= OnReceivePdfRenderTexture;
            VSEngine.Instance.OnEventReceiveVideoFirstFrameReady -= OnReceiveVideoFirstFrame;
            VSEngine.Instance.OnEventReceiveVideoCurrentTime -= OnReceiveVideoCurrentTime;
            VSEngine.Instance.OnEventReceiveVideoInfo -= OnReceiveVideoInfo;
            VSEngine.Instance.OnEventReceiveVideoFinish -= OnReceiveVideoFinish;
            VSEngine.Instance.OnEventPointClickHandler -= OnPointClick;
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.F1))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                WsMediaFile file = new WsMediaFile()
                {
                    url = pdfurl,
                    name = "MediaTestPDF",
                    fileMd5 = "",
                    mtime = ""
                };
                VSEngine.Instance.DownloadAndCacheFile(file, (data) =>
                {
                    Debug.Log("SDKEngine DownloadAndCacheFile path " + data.localpath);
                    VSEngine.Instance.CreatePdfRenderPlayer(VSEngine.Instance.GetBigScreenViewRoot().gameObject, data.localpath);
                });
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.ShowPdfRenderPlayerPage(VSEngine.Instance.GetBigScreenViewRoot().gameObject, 2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                VSEngine.Instance.RequestPdfRenderPlayerPageCount(VSEngine.Instance.GetBigScreenViewRoot().gameObject,(r,count)=> {
                    Debug.Log("VSEngine pdf count " + count);
                });
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (clicked != null)
                {
                    VSEngine.Instance.InitVideoPlayer(clicked, VideoPlayerKind.Ffmpeg, videourl, new GameObject[] { clicked });
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                VSEngine.Instance.PlayVideo(clicked, VideoPlayerKind.Ffmpeg);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                VSEngine.Instance.PauseVideo(clicked, VideoPlayerKind.Ffmpeg);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                VSEngine.Instance.RequestVideoInfo(clicked, VideoPlayerKind.Ffmpeg, (c, info) =>
                {
                    VSEngine.Instance.SeekVideoPos(clicked, VideoPlayerKind.Ffmpeg,(float)info.DurationTime/2);
                });
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                VSEngine.Instance.StopVideo(clicked, VideoPlayerKind.Ffmpeg);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                VSEngine.Instance.SetVideoVolume(clicked, VideoPlayerKind.Ffmpeg, 50);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                VSEngine.Instance.SetVideoLoop(clicked, VideoPlayerKind.Ffmpeg, true);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
            {
                VSEngine.Instance.SetVideoMuteAudio(clicked, VideoPlayerKind.Ffmpeg, true);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M))
            {
                VSEngine.Instance.RequestVideoTexture(clicked, VideoPlayerKind.Ffmpeg, (c, tex) => {
                    Debug.Log("VSEngine RequestVideoTexture tex " + tex.width + "x" + tex.height);
                });
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.G))
            {
                VSEngine.Instance.RequestVideoCurrentTime(clicked, VideoPlayerKind.Ffmpeg, (c, time) => {
                    Debug.Log("VSEngine RequestVideoTexture tex currenttime " + time);
                });
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnReceivePageCount(GameObject render,int pagecont)
        {
            Debug.Log("VSEngine OnReceivePageCount pagecount " + pagecont);
        }
        void OnReceivePdfRenderTexture(GameObject render,Texture2D tex)
        {
            Debug.Log("VSEngine OnReceivePdfRenderTexture tex " + tex.width + "x" + tex.height);
            VSEngine.Instance.SetBigScreenShowImage(tex);
        }
        void OnReceiveVideoFirstFrame(GameObject controller,Texture tex)
        {
            Debug.Log("VSEngine OnReceiveVideoFirstFrame tex " + tex.width + "x" + tex.height);
            VSEngine.Instance.SetBigScreenShowImage((Texture2D)tex);
        }
        void OnReceiveVideoCurrentTime(GameObject controller,double time)
        {
            Debug.Log("VSEngine OnReceiveVideoCurrentTime time " + time);
        }
        void OnReceiveVideoInfo(GameObject controller,CustomVideoPlayer info)
        {
            Debug.Log("VSEngine OnReceiveVideoInfo length " + info.DurationTime + " url " + info.url);
        }
        void OnReceiveVideoFinish(GameObject controller,string str)
        {
            Debug.Log("VSEngine OnReceiveVideoFinish ");
        }
        void OnPointClick(GameObject click)
        {
            clicked = click;
            Debug.Log("VSEngine OnPointClick name " + clicked.name);
        }
    }
}
