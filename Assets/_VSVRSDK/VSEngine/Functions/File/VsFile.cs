using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace VSEngine
{
    public class VsFile : BaseFunction
    {
        private static WsSceneInfo nowScene = new WsSceneInfo();

        private static Dictionary<string, LoadFileEvent> LoadUrlSceneEventDic = new Dictionary<string, LoadFileEvent>();

        private static Dictionary<string, LoadFileEvent> LoadUrlFileEventDic = new Dictionary<string, LoadFileEvent>();

        public static event System.Action<string> KODGetTxtStringEvent;
        private static Queue<System.Action<string>> KODGetTxtStringEventQueue = new Queue<Action<string>>();

        public static event System.Action<string, Texture2D> BigScreenShowImageEvent;
        private static Queue<System.Action<string, Texture2D>> BigScreenShowImageEventQueue = new Queue<System.Action<string, Texture2D>>();

        public static event System.Action<string, Texture2D> BigScreenUpdateImageEvent;
        private static Queue<System.Action<string, Texture2D>> BigScreenUpdateImageEventQueue = new Queue<System.Action<string, Texture2D>>();

        public static event System.Action<Texture> BigScreenShowVideoEvent;
        private static Queue<System.Action<Texture>> BigScreenShowVideoEventQueue = new Queue<System.Action<Texture>>();

        public static event System.Action<Texture2D> BigScreenShowVideoFrameEvent;
        private static Queue<System.Action<Texture2D>> BigScreenShowVideoFrameEventQueue = new Queue<System.Action<Texture2D>>();

        public static event System.Action<string> KODGetVideoPathEvent;
        public VsFile() : base(FunctionType.VsFile)
        {

        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        internal override void Awake()
        {
            base.Awake();

            MessageDispatcher.AddListener(VrDispMessageType.GetLocalCacheFile.ToString(), GetUrlCacheFile, true);
            MessageDispatcher.AddListener(VrDispMessageType.GetLocalCacheFile.ToString(), GetCacheFile, true);
            MessageDispatcher.AddListener(VrDispMessageType.GetLocalCacheFile.ToString(), GetUrlToLocalCache, true);

            MessageDispatcher.AddListener(VrDispMessageType.KODGetTxtString.ToString(), KODGetTxtString, true);

            MessageDispatcher.AddListener(VrDispMessageType.LoadGlbModelsDone.ToString(), LoadGlbModelsDone, true);

            MessageDispatcher.AddListener(VrDispMessageType.BigScreenShowImage.ToString(), BigScreenShowImage, true);
            MessageDispatcher.AddListener(VrDispMessageType.BigScreenUpdateImage.ToString(), BigScreenUpdateImage, true);

            MessageDispatcher.AddListener(VrDispMessageType.BigScreenShowVideo.ToString(), BigScreenShowVideo, true);
            MessageDispatcher.AddListener(VrDispMessageType.BigScreenShowVideoFrame.ToString(), BigScreenShowVideoFrame, true);

            MessageDispatcher.AddListener(VrDispMessageType.BigScreenPrepareVideo.ToString(), BigScreenPrepareVideo);
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();

            MessageDispatcher.RemoveListener(VrDispMessageType.GetLocalCacheFile.ToString(), GetUrlCacheFile, true);
            MessageDispatcher.RemoveListener(VrDispMessageType.GetLocalCacheFile.ToString(), GetCacheFile, true);
            MessageDispatcher.RemoveListener(VrDispMessageType.GetLocalCacheFile.ToString(), GetUrlToLocalCache, true);

            MessageDispatcher.RemoveListener(VrDispMessageType.KODGetTxtString.ToString(), KODGetTxtString, true);

            MessageDispatcher.RemoveListener(VrDispMessageType.LoadGlbModelsDone.ToString(), LoadGlbModelsDone, true);

            MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenShowImage.ToString(), BigScreenShowImage, true);
            MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenUpdateImage.ToString(), BigScreenUpdateImage, true);

            MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenShowVideo.ToString(), BigScreenShowVideo, true);
            MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenShowVideoFrame.ToString(), BigScreenShowVideoFrame, true);

            MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenPrepareVideo.ToString(), BigScreenPrepareVideo);

            try
            {
                KODGetTxtStringEvent = null;
                BigScreenShowImageEvent = null;
                BigScreenUpdateImageEvent = null;
                BigScreenShowVideoEvent = null;
                BigScreenShowVideoFrameEvent = null;
                KODGetVideoPathEvent = null;

                KODGetTxtStringEventQueue.Clear();
                BigScreenShowImageEventQueue.Clear();
                BigScreenUpdateImageEventQueue.Clear();
                BigScreenShowVideoEventQueue.Clear();
                BigScreenShowVideoFrameEventQueue.Clear();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        internal override void OnDisable()
        {
            base.OnDisable();
        }

        internal override void OnEnable()
        {
            base.OnEnable();
        }

        internal override void Start()
        {
            base.Start();
        }

        internal override void Update()
        {
            base.Update();
        }

        #region Event


        private void GetUrlCacheFile(IMessage msg)
        {
            LocalCacheFile sendfile = msg.Data as LocalCacheFile;

            string LocalPath = sendfile.path;
            string LocalUrl = "File://" + LocalPath;
            string GetedSign = sendfile.sign;

            if (!LoadUrlSceneEventDic.ContainsKey(GetedSign))
            {
                return;
            }

            var LoadUrlSceneEvent = LoadUrlSceneEventDic[GetedSign];

            if (sendfile.path != "")
            {
                if (LoadUrlSceneEvent.GetPathSucc != null)
                {
                    try
                    {
                        LoadUrlSceneEvent.GetPathSucc(LocalPath, LocalUrl, GetedSign);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        LoadUrlSceneEvent.GetPathSucc = null;
                    }       
                }
                WsMediaFile kodfile = new WsMediaFile
                {
                    roomurl = mStaticThings.I.nowRoomServerUrl,
                    preurl = mStaticThings.I.ThisKODfileUrl,
                    url = sendfile.path,
                    name = nowScene.name,
                    size = "11111",
                    ext = "scene",
                    mtime = sendfile.sign,
                    isupdate = false,
                    fileMd5 = ""
                };

                WsSceneInfo newscene = new WsSceneInfo
                {
                    id = nowScene.name,
                    scene = sendfile.path,
                    name = nowScene.name,
                    version = GetedSign,
                    isremote = true,
                    isupdate = false,
                    iskod = true,
                    icon = nowScene.icon,
                    kod = kodfile,
                    cryptAPI = nowScene.cryptAPI,
                    ckind = nowScene.ckind
                };
                MessageDispatcher.SendMessage(true, VrDispMessageType.LoadLocalPathScene.ToString(), newscene, 0);
            }
            else
            {
                if (LoadUrlSceneEvent.GetPathFail != null)
                {
                    try
                    {
                        LoadUrlSceneEvent.GetPathFail();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        LoadUrlSceneEvent.GetPathFail = null;
                    }         
                }
            }

            LoadUrlSceneEventDic.Remove(GetedSign);
        }

        private static Dictionary<string, LoadFileEvent> GetKODToLocalCacheFileEventDic = new Dictionary<string, LoadFileEvent>();
        private void GetCacheFile(IMessage msg)
        {
            LocalCacheFile sendfile = msg.Data as LocalCacheFile;

            string LocalPath = sendfile.path;
            string LocalUrl = "File://" + LocalPath;
            string GetedSign = sendfile.sign;

            if (!GetKODToLocalCacheFileEventDic.ContainsKey(GetedSign))
            {
                return;
            }

            var GetKODToLocalCacheFileEvent = GetKODToLocalCacheFileEventDic[GetedSign];

            if (sendfile.path != "")
            {
                if (GetKODToLocalCacheFileEvent.GetPathSucc != null)
                {
                    try
                    {
                        GetKODToLocalCacheFileEvent.GetPathSucc(LocalPath, LocalUrl, GetedSign);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        GetKODToLocalCacheFileEvent.GetPathSucc = null;
                    } 
                }
            }
            else
            {
                if (GetKODToLocalCacheFileEvent.GetPathFail != null)
                {
                    try
                    {
                        GetKODToLocalCacheFileEvent.GetPathFail();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        GetKODToLocalCacheFileEvent.GetPathFail = null;
                    } 
                }
            }

            GetKODToLocalCacheFileEventDic.Remove(GetedSign);
        }

        private void GetUrlToLocalCache(IMessage msg)
        {
            LocalCacheFile sendfile = msg.Data as LocalCacheFile;

            string LocalPath = sendfile.path;
            string LocalUrl = "File://" + LocalPath;
            string GetedSign = sendfile.sign;

            LoadFileEvent LoadUrlFileEvent = null;

            if (!LoadUrlFileEventDic.ContainsKey(GetedSign))
            {
                return;
            }
            LoadUrlFileEvent = LoadUrlFileEventDic[GetedSign];

            if (sendfile.path != "")
            {
                if (LoadUrlFileEvent.GetPathSucc != null)
                {
                    try
                    {
                        LoadUrlFileEvent.GetPathSucc(LocalPath, LocalUrl, GetedSign);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        LoadUrlFileEvent.GetPathSucc = null;
                    }
                }
            }
            else
            {
                if (LoadUrlFileEvent.GetPathFail != null)
                {
                    try
                    {
                        LoadUrlFileEvent.GetPathFail();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        LoadUrlFileEvent.GetPathFail = null;
                    }   
                }
            }

            LoadUrlFileEventDic.Remove(GetedSign);
        }

        private void KODGetTxtString(IMessage msg)
        {
            string txt = msg.Data as string;

            try
            {
                if (KODGetTxtStringEvent != null)
                    KODGetTxtStringEvent(txt);

                if (KODGetTxtStringEventQueue.Count > 0)
                {
                    var KODGetTxtStringEventInfo = KODGetTxtStringEventQueue.Dequeue();

                    if (KODGetTxtStringEventInfo != null)
                        KODGetTxtStringEventInfo(txt);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        private void BigScreenPrepareVideo(IMessage msg)
        {
            string vpath = (string)msg.Data;

            try
            {
                if (KODGetVideoPathEvent != null)
                    KODGetVideoPathEvent(vpath);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        private static Dictionary<string, System.Action<GameObject, List<string>>> LoadGlbModelsDoneEventDic = new Dictionary<string, Action<GameObject, List<string>>>();
        private void LoadGlbModelsDone(IMessage msg)
        {
            GlbSceneObjectFile newglb = msg.Data as GlbSceneObjectFile;

            if (!LoadGlbModelsDoneEventDic.ContainsKey(newglb.sign))
            {
                return;
            }

            var LoadGlbModelsDoneEvent = LoadGlbModelsDoneEventDic[newglb.sign];

            try
            {
                if (LoadGlbModelsDoneEvent != null)
                    LoadGlbModelsDoneEvent(newglb.glbobj, newglb.clips);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            LoadGlbModelsDoneEventDic.Remove(newglb.sign);
        }
        private void BigScreenShowImage(IMessage msg)
        {
            Texture2D texture = msg.Data as Texture2D;

            try
            {
                if (BigScreenShowImageEvent != null)
                {
                    BigScreenShowImageEvent((string)msg.Sender, texture);
                }
                if (BigScreenShowImageEventQueue.Count > 0)
                {
                    var BigScreenShowImageEventInfo = BigScreenShowImageEventQueue.Dequeue();

                    if (BigScreenShowImageEventInfo != null)
                        BigScreenShowImageEventInfo((string)msg.Sender, texture);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }          
        }

        private void BigScreenUpdateImage(IMessage msg)
        {
            Texture2D texture = msg.Data as Texture2D;

            try
            {
                if (BigScreenUpdateImageEvent != null)
                    BigScreenUpdateImageEvent((string)msg.Sender, texture);

                if (BigScreenUpdateImageEventQueue.Count > 0)
                {
                    var BigScreenUpdateImageEventInfo = BigScreenUpdateImageEventQueue.Dequeue();

                    if (BigScreenUpdateImageEventInfo != null)
                        BigScreenUpdateImageEventInfo((string)msg.Sender, texture);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void BigScreenShowVideo(IMessage msg)
        {
            Texture tex = msg.Data as Texture;

            try
            {
                if (BigScreenShowVideoEvent != null)
                    BigScreenShowVideoEvent(tex);

                if (BigScreenShowVideoEventQueue.Count > 0)
                {
                    var BigScreenShowVideoEventInfo = BigScreenShowVideoEventQueue.Dequeue();

                    if (BigScreenShowVideoEventInfo != null)
                        BigScreenShowVideoEventInfo(tex);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        private void BigScreenShowVideoFrame(IMessage msg)
        {
            Texture2D texture = msg.Data as Texture2D;

            try
            {
                if (BigScreenShowVideoFrameEvent != null)
                    BigScreenShowVideoFrameEvent(texture);

                if (BigScreenShowVideoFrameEventQueue.Count > 0)
                {
                    var BigScreenShowVideoFrameEventInfo = BigScreenShowVideoFrameEventQueue.Dequeue();

                    if (BigScreenShowVideoFrameEventInfo != null)
                        BigScreenShowVideoFrameEventInfo(texture);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        #endregion

        #region API

        #region 网络资源
        /// <summary>
        /// 下载KOD资源管理器资源到本地并取得本地URL
        /// </summary>
        /// <param name="kodLocalCache"></param>
        public static void GetKODToLocalCacheFile(LocalCacheFile kodLocalCache, LoadFileEvent loadFileEvent)
        {
            LocalCacheFile sendfile = kodLocalCache;

            if (!GetKODToLocalCacheFileEventDic.ContainsKey(kodLocalCache.sign))
            {
                GetKODToLocalCacheFileEventDic.Add(kodLocalCache.sign, loadFileEvent);
            }
            GetKODToLocalCacheFileEventDic[kodLocalCache.sign] = loadFileEvent;

            MessageDispatcher.SendMessageData(VrDispMessageType.SendCacheFile.ToString(), sendfile, 0);
        }


        /// <summary>
        /// 下载网络资源到本地并取得本地URL
        /// </summary>
        /// <param name="HttpUrl"></param>
        /// <param name="isURLSign"></param>
        /// <param name="Sign"></param>
        /// <param name="hasPrefix"></param>
        /// <param name="loadUrlFileEvent"></param>
        public static void GetUrlToLocalCacheFile(string HttpUrl, bool isURLSign, string Sign, bool hasPrefix, LoadFileEvent loadUrlFileEvent)
        {
            LocalCacheFile sendfile = new LocalCacheFile()
            {
                path = HttpUrl,
                isURLSign = isURLSign,
                sign = Sign,
                hasPrefix = hasPrefix,
                isKOD = false,
            };

            if (!LoadUrlFileEventDic.ContainsKey(Sign))
            {
                LoadUrlFileEventDic.Add(Sign, loadUrlFileEvent);
            }
            LoadUrlFileEventDic[Sign] = loadUrlFileEvent;

            MessageDispatcher.SendMessageData(VrDispMessageType.SendCacheFile.ToString(), sendfile, 0.01f);
        }

        /// <summary>
        /// 下载网络场景资源到本地并取得本地URL，并且加载场景
        /// </summary>
        /// <param name="SceneName"></param>
        /// <param name="HttpUrl"></param>
        /// <param name="isURLSign"></param>
        /// <param name="Sign"></param>
        /// <param name="hasPrefix"></param>
        /// <param name="RoomIconUrl"></param>
        /// <param name="CryptAPI"></param>
        /// <param name="CryptKind"></param>
        public static void GetUrlToLoadScene(string SceneName, string HttpUrl, bool isURLSign, string Sign, bool hasPrefix, string RoomIconUrl, string CryptAPI, int CryptKind, LoadFileEvent loadUrlSceneEvent)
        {
            LocalCacheFile sendfile = new LocalCacheFile()
            {
                path = HttpUrl,
                isURLSign = isURLSign,
                sign = Sign,
                hasPrefix = hasPrefix,
                isKOD = false,
            };

            nowScene.id = SceneName;
            nowScene.name = SceneName;
            nowScene.icon = RoomIconUrl;
            nowScene.cryptAPI = CryptAPI;
            nowScene.ckind = CryptKind;

            if (!LoadUrlSceneEventDic.ContainsKey(Sign))
            {
                LoadUrlSceneEventDic.Add(Sign, loadUrlSceneEvent);
            }
            LoadUrlSceneEventDic[Sign] = loadUrlSceneEvent;

            MessageDispatcher.SendMessageData(VrDispMessageType.SendCacheFile.ToString(), sendfile, 0.01f);
        }
        /// <summary>
        /// 生成WsMediaFile类型用时间戳
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cacheName"></param>
        /// <param name="cacheTime"></param>
        public static WsMediaFile AllocateMediaFileDataByTime(string url, string cacheName, string fileExt, string cacheTime)
        {
            WsMediaFile kodfile = new WsMediaFile
            {
                roomurl = mStaticThings.I.nowRoomServerUrl,
                preurl = mStaticThings.I.ThisKODfileUrl,
                url = url,
                name = cacheName,
                size = "11111",
                ext = fileExt,
                mtime = cacheTime,
                isupdate = false,
                fileMd5 = ""
            };

            return kodfile;
        }
        /// <summary>
        /// 生成WsMediaFile类型用MD5
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cacheName"></param>
        /// <param name="md5"></param>
        /// <returns></returns>
        public static WsMediaFile AllocateMediaFileDataByMd5(string url, string cacheName,string fileExt, string md5)
        {
            WsMediaFile kodfile = new WsMediaFile
            {
                roomurl = mStaticThings.I.nowRoomServerUrl,
                preurl = mStaticThings.I.ThisKODfileUrl,
                url = url,
                name = cacheName,
                size = "11111",
                ext = fileExt,
                mtime = "",
                isupdate = false,
                fileMd5 = md5
            };

            return kodfile;
        }
        /// <summary>
        /// 从网络文件资源管理器，加载TXT文本
        /// </summary>
        /// <param name="kodfile"></param>
        public static void KODGetOneTxtByTime(WsMediaFile kodfile, System.Action<string> KODGetTxtStringAC)
        {
            if (KODGetTxtStringAC != null)
                KODGetTxtStringEventQueue.Enqueue(KODGetTxtStringAC);

            MessageDispatcher.SendMessageData(VrDispMessageType.KODGetOneTxt.ToString(), kodfile, 0);
        }
        /// <summary>
        /// 从网络文件资源管理器，加载TXT文本
        /// </summary>
        /// <param name="kodfile"></param>
        public static void KODGetOneTxtByMd5(WsMediaFile kodfile, System.Action<string> KODGetTxtStringAC)
        {
            if (KODGetTxtStringAC != null)
                KODGetTxtStringEventQueue.Enqueue(KODGetTxtStringAC);

            MessageDispatcher.SendMessageData(VrDispMessageType.KODGetOneTxt.ToString(), kodfile, 0);
        }
        /// <summary>
        /// 从网络文件资源管理器，加载图片
        /// </summary>
        /// <param name="newtexturefile"></param>
        /// <param name="BigScreenShowImageEvent"></param>
        /// <param name="BigScreenUpdateImageEvent"></param>
        public static void KODGetOneImage(WsMediaFile newtexturefile, System.Action<string, Texture2D> BigScreenShowImageAc, System.Action<string, Texture2D> BigScreenUpdateImageAc)
        {
            if (BigScreenShowImageAc != null)
                BigScreenShowImageEventQueue.Enqueue(BigScreenShowImageAc);

            if (BigScreenUpdateImageAc != null)
                BigScreenUpdateImageEventQueue.Enqueue(BigScreenUpdateImageAc);

            MessageDispatcher.SendMessageData(VrDispMessageType.KODGetOneImage.ToString(), newtexturefile, 0);
        }
        /// <summary>
        /// 从网络文件资源管理器，加载视频
        /// </summary>
        /// <param name="newmov"></param>
        /// <param name="BigScreenShowVideoEvent"></param>
        /// <param name="BigScreenShowVideoFrameEvent"></param>
        public static void KODGetOneMov(WsMediaFile newmov, System.Action<Texture> BigScreenShowVideoAc, System.Action<Texture2D> BigScreenShowVideoFrameAc)
        {
            if (BigScreenShowVideoAc != null)
                BigScreenShowVideoEventQueue.Enqueue(BigScreenShowVideoAc);

            if (BigScreenShowVideoFrameAc != null)
                BigScreenShowVideoFrameEventQueue.Enqueue(BigScreenShowVideoFrameAc);

            MessageDispatcher.SendMessageData(VrDispMessageType.KODGetOneMov.ToString(), newmov, 0);
        }
        /// <summary>
        /// 加载glb模型
        /// </summary>
        /// <param name="Glb_url"></param>
        /// <param name="Glb_sign"></param>
        /// <param name="Glb_isScene"></param>
        /// <param name="Glb_Format"></param>
        /// <param name="Glb_LoadTransform"></param>
        /// <param name="Glb_isSyncLoad"></param>
        /// <param name="GlbAutoInit"></param>
        /// <param name="GlbAutoPlay"></param>
        /// <param name="loadGlbModelsDoneEvent"></param>
        public static void GetUrlToGlbObject(string Glb_url, string Glb_sign, bool Glb_isScene, string Glb_Format, Transform Glb_LoadTransform, bool Glb_isSyncLoad, bool GlbAutoInit, bool GlbAutoPlay, System.Action<GameObject, List<string>> loadGlbModelsDoneEvent)
        {
            WsGlbMediaFile newloadglb = new WsGlbMediaFile
            {
                url = Glb_url,
                sign = Glb_sign,
                isscene = Glb_isScene,
                format = Glb_Format == "" ? "glb" : Glb_Format,
                LoadTrasform = Glb_LoadTransform,
                isasyn = Glb_isSyncLoad,
                autoinit = GlbAutoInit,
                autoplay = GlbAutoPlay
            };

            if (!LoadGlbModelsDoneEventDic.ContainsKey(Glb_sign))
            {
                LoadGlbModelsDoneEventDic.Add(Glb_sign, loadGlbModelsDoneEvent);
            }
            LoadGlbModelsDoneEventDic[Glb_sign] = loadGlbModelsDoneEvent;

            MessageDispatcher.SendMessageData(VrDispMessageType.LoadGlbModels.ToString(), newloadglb, 0);
        }

        /// <summary>
        /// 设置视频播放信息
        /// </summary>
        /// <param name="ContorlObj"></param>
        /// <param name="RenderObj"></param>
        /// <param name="url"></param>
        /// <param name="vol">0-100</param>
        /// <param name="isloop"></param>
        /// <param name="autostart"></param>
        public static void SetVRVideoPlayer(GameObject ContorlObj, GameObject[] RenderObj, string url, float vol, bool isloop, bool autostart)
        {
            if (mStaticThings.I == null)
            {
                return;
            }

            if (vol < 0)
            {
                vol = 0;
            }
            if (vol > 100)
            {
                vol = 100;
            }

            CustomVideoPlayer cvp = new CustomVideoPlayer
            {
                ContorlObj = ContorlObj,
                RenderObj = RenderObj,
                url = url,
                vol = vol,
                isloop = isloop,
                autostart = autostart
            };
            MessageDispatcher.SendMessageData(VrDispMessageType.InitVideoPlayer.ToString(), cvp, 0);
        }
        #endregion

        #region 本地资源
        /// <summary>
        /// 加载本地路径场景
        /// </summary>
        /// <param name="SceneName"></param>
        /// <param name="Path"></param>
        /// <param name="Sign"></param>
        /// <param name="CryptAPI"></param>
        /// <param name="IconUrl"></param>
        public static void LoadLocalPathScene(string SceneName, string Path, string Sign, string CryptAPI, string IconUrl)
        {
            WsSceneInfo newscene = new WsSceneInfo
            {
                id = SceneName,
                scene = Path,
                name = SceneName,
                version = Sign,
                isremote = true,
                isupdate = false,
                icon = IconUrl,
                iskod = true,
                cryptAPI = CryptAPI
            };
            MessageDispatcher.SendMessage(false, VrDispMessageType.LoadLocalPathScene.ToString(), newscene, 0);
        }


        /// <summary>
        /// 加载本地图片
        /// </summary>
        /// <param name="LocalPath"></param>
        /// <param name="EventGetTexture"></param>
        /// <param name="EventGetTextureFail"></param>
        public static void GetPathToTexture(string LocalPath, System.Action<Texture2D> EventGetTexture, System.Action EventGetTextureFail)
        {
            SDKEngine.Instance.StartCoroutine(IELoadLocalPath(LocalPath, EventGetTexture, EventGetTextureFail));
        }
        private static IEnumerator IELoadLocalPath(string mPath, System.Action<Texture2D> EventGetTexture, System.Action EventGetTextureFail)
        {
            using (var uwr = new UnityWebRequest("File://" + mPath, UnityWebRequest.kHttpVerbGET))
            {
                uwr.downloadHandler = new DownloadHandlerTexture();
                yield return uwr.SendWebRequest();
                if (uwr.error == null)
                {
                    var GetedTexture = DownloadHandlerTexture.GetContent(uwr);

                    if (EventGetTexture != null)
                    {
                        EventGetTexture(GetedTexture);
                        EventGetTexture = null;
                    }
                }
                else
                {
                    if (EventGetTextureFail != null)
                    {
                        EventGetTextureFail();
                        EventGetTextureFail = null;
                    }
                }
                uwr.Dispose();
            }
        }

        #endregion

        #endregion
    }
}