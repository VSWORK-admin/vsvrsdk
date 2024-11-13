using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using com.ootii.Messages;
using DG.Tweening;
using UnityEngine.Networking;
using System.Net;
using System.IO;

namespace VSWorkSDK
{
    public class WebResponse : System.IDisposable
    {
        public string Url = "";
        public bool IsDone = false;
        public float DownloadProgress = 0;
        public string txt;
        public string Error = null;
        public bool islocalfile;
        public Texture2D img;
        public MemoryStream memoryStream;
        public AudioClip audioClip;
        public string localpath;

        public void Dispose()
        {
            txt = null;
            img = null;
            if (memoryStream != null)
            {
                memoryStream.Dispose();
            }
            audioClip = null;
            memoryStream = null;
            localpath = null;
        }
    }
    public delegate void WebResponseCallback(WebResponse response);
    public class BigScreenController : MonoBehaviour
    {
        private void Awake()
        {
            //Bigscreen.SetActive(false);

        }

        public GameObject ScreenRoot;

        public Canvas BigscreenCanvas;
        public GameObject Bigscreen;

        //public GameObject BigscreenControls;

        public Canvas WebscreenCanvas;
        public GameObject Webscreen;
        //public GameObject WebscreenControls;

        public RawImage screenimg;

        Vector3 nowscal;
        Texture2D bigscreentexture;
        Texture2D updatebigscreentexture;
        public int textruelevel = 1;

        public Collider grabhandel;
        public ControllingObjectMark controlmark;

        public WsMediaFile nowImgUrl = null;

        public int ScreenimagesizeConstract = 12000;

        void Start()
        {

            if (mStaticThings.I == null)
            {
                return;
            }
            mStaticThings.I.BigscreenRoot = transform;
            textruelevel = 1;
            MessageDispatcher.AddListener(VrDispMessageType.KODGetOneImage.ToString(), KODGetOneImage);
            MessageDispatcher.AddListener(VrDispMessageType.KODGetOneMov.ToString(), KODGetOneMov);
            MessageDispatcher.AddListener(VrDispMessageType.BigScreenEndAnchor.ToString(), BigScreenEndAnchor);
            MessageDispatcher.AddListener(WsMessageType.RecieveBigScreen.ToString(), RecieveBigScreen);
            MessageDispatcher.AddListener(VrDispMessageType.BigScreenSetPos.ToString(), BigScreenSetPos);
            MessageDispatcher.AddListener(VrDispMessageType.SceneChanged.ToString(), (msg) =>
            {
            //Bigscreen.SetActive(false);
            SetBigScreenActive(false);
                textruelevel = 1;
            }, true);
            MessageDispatcher.AddListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj);
            MessageDispatcher.AddListener(VrDispMessageType.BigScreenRecieveRTSP.ToString(), (msg) =>
            {

            });
            MessageDispatcher.AddListener(VrDispMessageType.CancelAllDownload.ToString(), CancelAllDownload);
            GetComponent<VRUISelectorProxy>().Init();
            BigscreenCanvas.worldCamera = mStaticThings.I.Maincamera.GetComponent<Camera>();


            MessageDispatcher.AddListener(VrDispMessageType.KODGetPDFPath.ToString(), (msg) =>
            {
                DoMarkDownloadSignNull();
            });

            MessageDispatcher.AddListener(VrDispMessageType.BigScreenRecieveRTSP.ToString(), (msg) =>
            {
                DoMarkDownloadSignNull();
            });


            MessageDispatcher.AddListener(VrDispMessageType.VRUserLeaveChanel.ToString(), (msg) =>
            {
                ClearBigscreenFrame();
            });

            SetBigScreenActive(false);
        }

        private void OnDestroy()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.KODGetOneImage.ToString(), KODGetOneImage);
            MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenEndAnchor.ToString(), RecieveBigScreen);
            MessageDispatcher.RemoveListener(WsMessageType.RecieveBigScreen.ToString(), RecieveBigScreen);
            MessageDispatcher.RemoveListener(VrDispMessageType.KODGetOneMov.ToString(), KODGetOneMov);
            MessageDispatcher.RemoveListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj);
            MessageDispatcher.RemoveListener(VrDispMessageType.CancelAllDownload.ToString(), CancelAllDownload);
        }


        public void SwitchScreenMode(BigScreenModeType tp)
        {
            switch (tp)
            {
                case BigScreenModeType.screen:
                    Bigscreen.SetActive(true);
                    Webscreen.SetActive(false);
                    //BigscreenControls.SetActive(true);
                    //WebscreenControls.SetActive(false);
                    break;
                case BigScreenModeType.web:
                    Bigscreen.SetActive(false);
                    Webscreen.SetActive(true);
                    //BigscreenControls.SetActive(false);
                    //WebscreenControls.SetActive(true);
                    break;
                default:
                    break;
            }
        }


        public void SendChangeScreenMode(int screentype)
        {
            if (screentype == 0)
            {
                WsChangeInfo cinfo = new WsChangeInfo
                {
                    a = "WsChangeScreenMode",
                    b = "screen"
                };
                MessageDispatcher.SendMessage(this, WsMessageType.SendChangeObj.ToString(), cinfo, 0);
            }
            else if (screentype == 1)
            {
                WsChangeInfo cinfo = new WsChangeInfo
                {
                    a = "WsChangeScreenMode",
                    b = "web"
                };
                MessageDispatcher.SendMessage(this, WsMessageType.SendChangeObj.ToString(), cinfo, 0);
            }
        }

        void BigScreenEndAnchor(IMessage msg)
        {
            RecieveBigScreen(msg);
            SendBigScreen();
        }

        public void BigscreenGrabEnabled(bool en)
        {
            grabhandel.enabled = en;
        }


        public void ClearBigscreenFrame()
        {
            if (bigscreentexture != null)
            {
                Destroy(bigscreentexture);
                bigscreentexture = null;
            }
            if (updatebigscreentexture != null)
            {
                Destroy(updatebigscreentexture);
                updatebigscreentexture = null;
            }
            screenimg.texture = null;
            EngineTestManager.Instance.sceneManager.CleanCaches();
        }

        public void EnableBigscreen(bool en)
        {
            //Bigscreen.SetActive(en);
            SetBigScreenActive(en);
            SendBigScreen();

        }

        void RecieveBigScreen(IMessage msg)
        {
            WsBigScreen bigscreennew = msg.Data as WsBigScreen;

            if (bigscreennew.enabled)
            {
                transform.position = bigscreennew.position;
                transform.rotation = bigscreennew.rotation;
                transform.DOScale(bigscreennew.scale, 0.3f).SetEase(Ease.Linear);
                nowscal = bigscreennew.scale;

                SetBigScreenActive(true);
            }
            else
            {
                SetBigScreenActive(true);
                transform.position = bigscreennew.position;
                transform.rotation = bigscreennew.rotation;
                transform.localScale = bigscreennew.scale;
                nowscal = bigscreennew.scale;

                SetBigScreenActive(false);
            }
        }

        public void BigScreenStartAnchor()
        {
            MessageDispatcher.SendMessage(this, VrDispMessageType.BigScreenStartAnchor.ToString(), 0, 0);
            //Bigscreen.SetActive(false);
            SetBigScreenActive(false);
        }

        string NowScreenImgSign;

        void KODGetOneImage(IMessage msg)
        {
            WsMediaFile newtexturefile = msg.Data as WsMediaFile;
            string sign = newtexturefile.fileMd5.Length != 32 ? VRUtils.GetMD5(newtexturefile.name + "_" + newtexturefile.mtime.ToString()) : newtexturefile.fileMd5;

            if (NowScreenImgSign == sign)
            {
                return;
            }
            else if (!newtexturefile.isupdate && NowScreenImgSign != "")
            {
                NowScreenImgSign = "";
            }
            UnityWebRequest request = UnityWebRequest.Get(newtexturefile.url);
            var response = new WebResponse();
            EngineTestManager.Instance.DoGetImg(request, response, (res2) =>
            {
                if (res2.Error == null)
                {
                    if (!newtexturefile.isupdate)
                    {
                        if (ScreenimagesizeConstract > Mathf.Max(res2.img.width, res2.img.height))
                        {
                            if (bigscreentexture != null)
                            {
                                Destroy(bigscreentexture);
                                bigscreentexture = null;
                            }
                            screenimg.texture = null;
                            EngineTestManager.Instance.sceneManager.CleanCaches();
                            bigscreentexture = res2.img;
                            MessageDispatcher.SendMessage(res2.Url.Replace("File://", ""), VrDispMessageType.BigScreenShowImage.ToString(), bigscreentexture, 0);

                            SetTextureLevel(textruelevel);
                            NowScreenMovieSign = "";
                        }
                        else
                        {
                            MessageDispatcher.SendMessage(res2.Url.Replace("File://", ""), VrDispMessageType.BigScreenShowImage.ToString(), res2.img, 0);
                        }
                    }
                    else
                    {
                        if (updatebigscreentexture != null)
                        {
                            Destroy(updatebigscreentexture);
                            updatebigscreentexture = null;
                        }
                        updatebigscreentexture = res2.img;
                        MessageDispatcher.SendMessage(res2.Url.Replace("File://", ""), VrDispMessageType.BigScreenUpdateImage.ToString(), updatebigscreentexture, 0);
                    }
                }
                else
                {
                    if (!newtexturefile.isupdate)
                    {
                        NowScreenImgSign = "";
                    }
                }
            });
        }

        public void UnloadScreenTextrue()
        {
            bigscreentexture = null;
        }


        void SetTextureLevel(int level)
        {
            if (bigscreentexture == null)
            {
                return;
            }
            screenimg.texture = null;
            screenimg.texture = bigscreentexture;
            SetBigScreenTextureWidth(bigscreentexture);
            textruelevel = level;
        }


        void CancelAllDownload(IMessage msg)
        {
            DoMarkDownloadSignNull();
        }

        void DoMarkDownloadSignNull()
        {
            NowScreenMovieSign = "";
            NowScreenImgSign = "";
        }

        string NowScreenMovieSign = "";
        public bool moveloadDirect = false;
        void KODGetOneMov(IMessage msg)
        {
            WsMediaFile newmov = msg.Data as WsMediaFile;
            string sign = newmov.fileMd5.Length != 32 ? VRUtils.GetMD5(newmov.name + "_" + newmov.mtime.ToString()) : newmov.fileMd5;

            if (NowScreenMovieSign == sign)
            {
                //GameManager.Instance.infoController.InfoLog(string.Format(LanguageDataBase.GetLanguageString("VRController_BigScreen", "BigScreenController_2"), newmov.name));
                //GameManager.Instance.infoController.InfoLog("视频" + newmov.name + " 正在下载中...");
                return;
            }
            else if (!newmov.isupdate && NowScreenMovieSign != "")
            {
                NowScreenMovieSign = "";
            }
            UnityWebRequest request = UnityWebRequest.Get(newmov.url);
            var response = new WebResponse();
            string vpath = Application.streamingAssetsPath + "/" + newmov.name;
            EngineTestManager.Instance.DoGetFile(request, response,vpath, (res) => {

                if (res.Error == null)
                {
                    if (newmov.isupdate)
                    {
                        return;
                    }
          
                    if (NowScreenMovieSign != "")
                    {
                        if (moveloadDirect)
                        {
                            MessageDispatcher.SendMessage(this, VrDispMessageType.BigScreenPrepareVideo.ToString(), vpath, 0);
                        }
                        else if (res.islocalfile)
                        {
                            NowScreenMovieSign = "";
                            MessageDispatcher.SendMessage(this, VrDispMessageType.BigScreenPrepareVideo.ToString(), vpath, 0);
                        }
                        else
                        {
                            NowScreenMovieSign = "";
                        }

                        NowScreenImgSign = "";
                    }
                }
                else
                {
                    if (!newmov.isupdate)
                    {
                        NowScreenMovieSign = "";
                    }

                    if (res.Error != "cancel")
                    {
                        //GameManager.Instance.infoController.InfoLog(string.Format(LanguageDataBase.GetLanguageString("VRController_BigScreen", "BigScreenController_5"), newmov.name), InfoColor.red, 6f);
                    }
                    //GameManager.Instance.infoController.InfoLog("视频:" + newmov.name + "缓存失败", InfoColor.red, 6f);
                }
            });

            if (!newmov.isupdate)
            {
                NowScreenMovieSign = sign;
            }
        }

        public void SetBigscreenWidth(float wh)
        {
            if (wh > 16f / 9f)
            {
                BigscreenCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(3840f, 3840f / wh);
            }
            else
            {
                BigscreenCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(2160f * wh, 2160f);
            }
        }


        void SetBigScreenTextureWidth(Texture2D tx)
        {
            SetBigscreenWidth((float)tx.width / (float)tx.height);
        }


        void BigScreenSetPos(IMessage msg)
        {
            WsBigScreen rcbigscreen = msg.Data as WsBigScreen;

            SetBigScreenActive(true);
            transform.position = rcbigscreen.position;
            transform.rotation = rcbigscreen.rotation;
            transform.localScale = rcbigscreen.scale;
            nowscal = rcbigscreen.scale;

            SetBigScreenActive(rcbigscreen.enabled);
        }


        void SetBigScreenActive(bool en)
        {
            ScreenRoot.SetActive(en);
            if (mStaticThings.I.isAdmin || mStaticThings.I.sadmin)
            {
                grabhandel.enabled = en;
            }
            controlmark.enabled = en;
        }

        void SendBigScreen()
        {
            WsBigScreen wbs = new WsBigScreen
            {
                id = mStaticThings.I.mAvatarID,
                enabled = ScreenRoot.activeSelf,
                position = transform.position,
                rotation = transform.rotation,
                scale = nowscal
            };
            MessageDispatcher.SendMessage(this, WsMessageType.SendBigScreen.ToString(), wbs, 0);
        }

        public void SetBigscreenScale(float num)
        {
            //Bigscreen.SetActive(true);
            SetBigScreenActive(true);
            float fix = transform.localScale.x * (1 + num);
            nowscal = new Vector3(fix, fix, fix);
            transform.DOScale(nowscal, 0.3f).SetEase(Ease.Linear);
            SendBigScreen();
        }

        public void SetAddBigscreenBend(int num)
        {
            //Bigscreen.SetActive(true);
            SetBigScreenActive(true);
            SendBigScreen();
        }

        public void SetBigscreenBend(int num)
        {
            SendBigScreen();
        }

        public void SetBigscreenPosX(float num)
        {
            SetBigScreenActive(true);
            float fix = transform.localPosition.x + (num);
            transform.localPosition = new Vector3(fix, transform.localPosition.y, transform.localPosition.z);
            SendBigScreen();
        }
        public void SetBigscreenPosY(float num)
        {
            SetBigScreenActive(true);
            float fix = transform.localPosition.y + (num);
            transform.localPosition = new Vector3(transform.localPosition.x, fix, transform.localPosition.z);
            SendBigScreen();
        }

        public void SetBigscreenPosZ(float num)
        {
            SetBigScreenActive(true);
            float fix = transform.localPosition.z + (num);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, fix);
            SendBigScreen();
        }
        public void SetBigscreenRotate(float num)
        {
            SetBigScreenActive(true);
            float fix = transform.rotation.y + (num);
            transform.Rotate(Vector3.up, num);
            SendBigScreen();
        }

        public void WsSetImageLevel(int i)
        {
            WsChangeInfo wsinfo = new WsChangeInfo()
            {
                id = mStaticThings.I.mAvatarID,
                a = "WsSetImageLevel",
                b = i.ToString()
            };
            MessageDispatcher.SendMessage(this, WsMessageType.SendChangeObj.ToString(), wsinfo, 0);
        }

        void RecieveChangeObj(IMessage msg)
        {
            WsChangeInfo wsinfo = msg.Data as WsChangeInfo;

            if (wsinfo.a == "WsSetImageLevel")
            {
                //GameManager.Instance.infoController.InfoLog("当前大屏图片Level ：" + wsinfo.b);
                SetTextureLevel(int.Parse(wsinfo.b));
            }
            else if (wsinfo.a == "WsChangeScreenMode")
            {
                if (wsinfo.b == "screen")
                {
                    SwitchScreenMode(BigScreenModeType.screen);
                }
                else if (wsinfo.b == "web")
                {
                    SwitchScreenMode(BigScreenModeType.web);
                }
            }
        }

 
    }
}