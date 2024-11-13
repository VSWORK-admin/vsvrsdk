using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace VSWorkSDK
{
    public class EngineTestManager : MonoBehaviour
    {
        private static EngineTestManager _Instance;
        public static EngineTestManager Instance { get { return _Instance; } }

        public StandaloneNetwork standaloneNetwork;
        public LoadSceneManager sceneManager;
        public TestAvatarManager avatarManager;
        //数据
        public static Msg_RoomData roomData = new Msg_RoomData();
        public List<string> CheckAvatarList = new List<string>();
        public TextAsset myDataInfo;
        private static long GUID = 0;
        private void Awake()
        {
            _Instance = this;

            MessageDispatcher.AddListener(VrDispMessageType.SendCacheFile.ToString(), SendCacheFile);

            roomData.InitMyData(myDataInfo.text);
        }
        private void Start()
        {
            mStaticThings.I.GlbRoot = new GameObject().transform;
            mStaticThings.I.GlbRoot.name = "_GlbRoot";
            mStaticThings.I.GlbRoot.gameObject.AddComponent<DontDestroyOnLoad>();

            mStaticThings.I.GlbSceneRoot = new GameObject().transform;
            mStaticThings.I.GlbSceneRoot.name = "_GlbSceneRoot";
            mStaticThings.I.GlbSceneRoot.parent = mStaticThings.I.GlbRoot;

            mStaticThings.I.GlbOjbRoot = new GameObject().transform;
            mStaticThings.I.GlbOjbRoot.name = "_GlbObjRoot";
            mStaticThings.I.GlbOjbRoot.parent = mStaticThings.I.GlbRoot;
        }

        public static long GetGuid()
        {
            long guid = System.Threading.Interlocked.Increment(ref GUID);
            return guid;
        }

        private void OnDestroy()
        {

        }
        private void Update()
        {
            
        }

        #region TestMsgHandler

        void SendCacheFile(IMessage msg)
        {
            LocalCacheFile sendfile = msg.Data as LocalCacheFile;
            //Debug.LogWarning(sendfile.path);

            string oriurl;
            if (sendfile.isKOD)
            {
                oriurl = mStaticThings.I.ThisKODfileUrl + sendfile.path.Substring(0, sendfile.path.LastIndexOf('/') + 1);
            }
            else
            {
                oriurl = sendfile.path.Substring(0, sendfile.path.LastIndexOf('/') + 1);
            }

            string oriname = sendfile.path.Substring(sendfile.path.LastIndexOf('/') + 1);
            string downloadurl;
            string downloadname;
            if (sendfile.hasPrefix)
            {
                downloadurl = oriurl + mStaticThings.I.now_ScenePrefix + oriname.Substring(1, oriname.Length - 1);
                downloadname = mStaticThings.I.now_ScenePrefix + oriname.Substring(1, oriname.Length - 1);
            }
            else
            {
                downloadurl = oriurl + oriname;
                downloadname = oriname;
            }

            if (sendfile.isURLSign)
            {
                StartCoroutine(geturlsign(downloadurl, downloadname, sendfile.sign));
            }
            else
            {
                GetURLToLocalCache(downloadurl, downloadname, sendfile.sign);
            }
        }

        IEnumerator geturlsign(string downloadurl, string downloadname, string urlsign)
        {

            UnityWebRequest request = UnityWebRequest.Get(@urlsign);
            yield return request.SendWebRequest();
            if (request.error != null)
            {
                Debug.LogWarning("Room Server get sign error");
                GetURLToLocalCache(downloadurl, downloadname, downloadname);
                yield break;
            }
            string str = request.downloadHandler.text;
            GetURLToLocalCache(downloadurl, downloadname, str, urlsign);
            request.Dispose();
        }


        void GetURLToLocalCache(string url, string filename, string sign, string urlsign = "")
        {
            //Debug.LogWarning(url);
            UnityWebRequest request = UnityWebRequest.Get(url);
            var response = new WebResponse();
            string vpath = Application.streamingAssetsPath + "/" + filename;
            EngineTestManager.Instance.DoGetFile(request, response, vpath, (res) =>
            {
                if (res.Error == null)
                {
                    LocalCacheFile rcfile = new LocalCacheFile()
                    {
                        path = vpath,
                        sign = urlsign == "" ? sign : urlsign,
                        isURLSign = false,
                        hasPrefix = false
                    };
                    MessageDispatcher.SendMessage(this, VrDispMessageType.GetLocalCacheFile.ToString(), rcfile, 0);
                }
            });
        }
        #endregion

        #region TestMsgSender

        #endregion

        #region TestFunction
        public void DoGetFile(UnityWebRequest webRequest, WebResponse response, string path, WebResponseCallback doneCallback, WebResponseCallback progressCallback = null)
        {
            StartCoroutine(IE_DoRequestFile(webRequest, response, path, doneCallback, progressCallback));
        }
        public void DoGetImg(UnityWebRequest webRequest, WebResponse response, WebResponseCallback doneCallback, WebResponseCallback progressCallback = null)
        {
            StartCoroutine(IE_DoRequestImg(webRequest, response, doneCallback, progressCallback));
        }
        private static IEnumerator IE_DoRequestFile(UnityWebRequest webRequest, WebResponse response, string path, WebResponseCallback doneCallback, WebResponseCallback progressCallback = null)
        {
            webRequest.downloadHandler = new DownloadHandlerFile(path);

            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            {
                response.DownloadProgress = webRequest.downloadProgress;
                if (progressCallback != null) progressCallback(response);
                yield return new WaitForSeconds(0.3f);
            }
            if (!webRequest.isNetworkError)
            {
                response.IsDone = true;
                response.localpath = path;
                response.islocalfile = false;
                response.Error = null;
                //response.memoryStream = new MemoryStream(webRequest.downloadHandler.data);
            }
            else
            {
                response.IsDone = true;
                response.Error = webRequest.error;
                response.localpath = "";
            }
            doneCallback(response);
        }
        private static IEnumerator IE_DoRequestImg(UnityWebRequest webRequest, WebResponse response, WebResponseCallback doneCallback, WebResponseCallback progressCallback = null)
        {
            webRequest.downloadHandler = new DownloadHandlerTexture();

            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            {
                response.DownloadProgress = webRequest.downloadProgress;
                if (progressCallback != null) progressCallback(response);
                yield return new WaitForSeconds(0.2f);
            }
            if (!webRequest.isNetworkError)
            {
                response.IsDone = true;
                response.Error = null;
                response.img = DownloadHandlerTexture.GetContent(webRequest);
                response.memoryStream = new MemoryStream(webRequest.downloadHandler.data);
                response.islocalfile = false;
            }
            else
            {
                response.IsDone = true;
                response.Error = webRequest.error;
            }
            doneCallback(response);
        }
        #endregion
    }
}