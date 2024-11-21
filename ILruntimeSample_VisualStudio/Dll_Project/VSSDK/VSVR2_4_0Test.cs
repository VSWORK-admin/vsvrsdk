using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VSWorkSDK;

namespace Dll_Project
{
    public class VSVR2_4_0Test : DllGenerateBase
    {
        public Transform WebViewButton;
        public RectTransform WebViewPanel;
        public Button button1;
        public RectTransform WebViewPanelParent;
        public Transform FfmpegCaptuureButtons;
        public Camera CaptureCamera;
        private RawImage webLoadImage;
        string path = "https://vsmiddle.vswork.vip/file/dev/20230920/a33ac354eade4d7c99634a8f97bf381f.png?OSSAccessKeyId=LTAI5tK4VV8vvTdA18jE3z2B&Expires=1703701979&Signature=dQL071nsDQN779X7MgWpvv3kl2s%3D";
        public override void Init()
        {
            WebViewButton = BaseMono.ExtralDatas[0].Target;

            FfmpegCaptuureButtons = BaseMono.ExtralDatas[1].Info[0].Target;
            CaptureCamera = BaseMono.ExtralDatas[1].Info[1].Target.GetComponent<Camera>();

            webLoadImage = BaseMono.ExtralDatas[3].Target.GetComponent<RawImage>();
            RegisterEvent(FfmpegCaptuureButtons, 0, CaptureVideoStart);
            RegisterEvent(FfmpegCaptuureButtons, 1, CaptureVideoStartTexture);
            RegisterEvent(FfmpegCaptuureButtons, 2, CaptureVideoStop);
            RegisterEvent(FfmpegCaptuureButtons, 3, CollectAudioStart);
            RegisterEvent(FfmpegCaptuureButtons, 4, CollectAudioStop);
            RegisterEvent(FfmpegCaptuureButtons, 5, VideoCaptureRtmpStart);
            RegisterEvent(FfmpegCaptuureButtons, 6, VideoCaptureRtmpStartTexture);
            RegisterEvent(FfmpegCaptuureButtons, 7, VideoCaptureRtmpStop);

            VSEngine.Instance.OnEventGetCapturePath += (path) => { Debug.LogError("录制视频的地址" + path); };
            VSEngine.Instance.OnEventGetCollectAudioPath += (path) => { Debug.LogError("录制音频的地址" + path); };
            VSEngine.Instance.OnEventStopCaptureRtmpSucceed += () => { Debug.LogError("录制推流结束" ); };
            //WebViewPanel = BaseMono.ExtralDatas[0].Target as RectTransform;
            //WebViewPanelParent = BaseMono.ExtralDatas[1].Target as RectTransform;
            base.Init();
        }
        public void RegisterEvent(Transform Parent,int Index,Action action )
        {
            if (Index<Parent.childCount)
            {
                Parent.GetChild(Index).GetComponent<Button>().onClick.AddListener(() => {
                    Debug.LogError( " 按钮"+Parent.name + "  " + Index);
                    action?.Invoke(); });
            }
        }
        public void CaptureVideoStart()
        {
            VSEngine.Instance.VideoCaptureStart("nzw", 1920, 1080, 30, null, 0);
        }
        public void CaptureVideoStartTexture()
        {
            VSEngine.Instance.VideoCaptureStart("nzwt", 1920, 1080, 30, CaptureCamera.targetTexture, 0);
        }
        public void CaptureVideoStop()
        {
            VSEngine.Instance.VideoCaptureStop(0);
            
        }
        public void CollectAudioStart()
        {
            VSEngine.Instance.CollectAudioStart("nzwa", 0);
        }
        public void CollectAudioStop()
        {
            VSEngine.Instance.CollectAudioStop(0);

        }
        public void VideoCaptureRtmpStart()
        {
            string url = "https://s.vswork.space/open/api/app/live/url";
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("code", "nizhuwei");
            VSEngine.Instance.PostWebRequest(url, wwwForm,
                (res) =>
                {
                    VSEngine.Instance.ShowRoomMarqueeLog("VSEngine Post ret " + res, InfoColor.black, 3, false);
                }, null,new Dictionary<string, string> {
                    {"apikey" ,mStaticThings.apikey},
                    {"apitoken", mStaticThings.apitoken }
                });
           // VSEngine.Instance.VideoCaptureRtmpStart("rtmp://ustream.vswork.vip/vsvr/appstream?auth_key=1701324411-0-0-9faec51c5f38bb06a3a25e7522f8c6ef", 1920, 1080, 30, null, 0);
        }
        public void VideoCaptureRtmpStartTexture()
        {
            VSEngine.Instance.VideoCaptureRtmpStart("rtmp://ustream.vswork.vip/vsvr/appstream?auth_key=1701324411-0-0-9faec51c5f38bb06a3a25e7522f8c6ef", 1920, 1080, 30, CaptureCamera.targetTexture, 0);
        }
        public void VideoCaptureRtmpStop()
        {
            VSEngine.Instance.VideoCaptureRtmpStop(0);

        }
        public override void Awake()
        {
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
          BaseMono.  StartCoroutine(LoadTextureFromNet2(path));

            mStaticThings.bOpenAutoGC = true;
           
        }
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (mStaticThings.I.isCloudRender || mStaticThings.I.isCloudRenderMobile)
                {

                    WebViewParamterData paramterdata1 = new WebViewParamterData()
                    {
                        sync = true,
                        adminonly = false,
                        url = "https://baidu.com",
                        displayUI = true,
                        bfullscreen = false,
                        width = 100,
                        height = 100 + 20,
                        showtoolbar = false,
                        offset = new Vector2(700, 19),
                    };
                    VSEngine.Instance.SendSystemExpandEvent("OpenBigScreenWebInner", new List<object> {
                    paramterdata1
                    });
                    //  MessageDispatcher.SendMessage(false, "OpenBigScreenWebInner", paramterdata1, 0);
                }
                else
                {
                    WebViewParamterData paramterdata = new WebViewParamterData()
                    {
                        sync = true,
                        adminonly = false,
                        url = "https://baidu.com",
                        displayUI = true,
                        bfullscreen = false,
                        width = 300,
                        height = 300 + 20,
                        showtoolbar = false,
                        offset = new Vector2(700, 19),
                    };
                    Debug.LogError("打开网页");
                    VSEngine.Instance.SendSystemExpandEvent("OpenBigScreenWeb", new List<object> {
                    paramterdata
                    });
                    BaseMono.StartCoroutine(Delay(0.2f));

                }
            }
            base.Update();
        }
        public override void OnDisable()
        {
            base.OnDisable();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        IEnumerator Delay(float Time)
        {
            yield return new WaitForSeconds(Time);


            RectTransform rectTransformParent = mStaticThings.I.BigscreenObj.transform.Find("Canvas_webRoot/parent/canvasweb_main/WebViewParent") as RectTransform;
            rectTransformParent.offsetMax = WebViewPanelParent.offsetMax;
            rectTransformParent.offsetMin = WebViewPanelParent.offsetMin;
            rectTransformParent.anchoredPosition3D = WebViewPanelParent.anchoredPosition3D;
            rectTransformParent.pivot = WebViewPanelParent.pivot;
            rectTransformParent.sizeDelta = WebViewPanelParent.sizeDelta;
            rectTransformParent.anchorMax = WebViewPanelParent.anchorMax;
            rectTransformParent.anchoredPosition = WebViewPanelParent.anchoredPosition;

            rectTransformParent.anchorMin = WebViewPanelParent.anchorMin;

            RectTransform rectTransformPrefab = mStaticThings.I.BigscreenObj.transform.Find("Canvas_webRoot/parent/canvasweb_main/WebViewParent/CanvasWebViewPrefab") as RectTransform;


            rectTransformPrefab.offsetMax = WebViewPanel.offsetMax;
            rectTransformPrefab.offsetMin = WebViewPanel.offsetMin;
            rectTransformPrefab.anchoredPosition3D = WebViewPanel.anchoredPosition3D;
            rectTransformPrefab.pivot = WebViewPanel.pivot;
            rectTransformPrefab.sizeDelta = WebViewPanel.sizeDelta;
            rectTransformPrefab.anchorMax = WebViewPanel.anchorMax;
            rectTransformPrefab.anchoredPosition = WebViewPanel.anchoredPosition;

            rectTransformPrefab.anchorMin = WebViewPanel.anchorMin;

        }
        IEnumerator DownloadImage()
        {

            WWW www = new WWW("https://upload-images.jianshu.io/upload_images/5809200-a99419bb94924e6d.jpg?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240");
            yield return www;
            if (www.error != null)
            {
                Debug.LogError(www.error);
                yield return null;
            }
            webLoadImage.texture = www.texture;
        }
        IEnumerator LoadTextureFromNet2(string filePath)
        {
            // 创建一个UnityWebRequest对象并加载本地图片
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(filePath);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // 获取加载的纹理
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                //把贴图赋到RawImage
                webLoadImage.texture = texture;

               
            }
            else
            {
                Debug.LogError("下载失败：" + www.error);
            }
        }
    }
}
