using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSWorkSDK
{
    public class StandaloneNetwork : MonoBehaviour
    {
        Queue<string> SocketMessages = new Queue<string>();
        //Queue<JSONObject> WsAvatarQueue = new Queue<JSONObject>();

        public static bool bConnected = false;

        bool sendedready = false;

        public float OpenCheckConnectionTime = 1f;
        public float CheckHeartTime = 6f;
        public float CheckDelayTime = 3f;
        public bool IsSendAllEnabled = true;
        bool FirstConnectChanel = true;
        string LastChanelUrl = "";

        public float outtime;

        bool isauthfaild = false;

        private void Awake()
        {

        }

        private void Start()
        {
            //MessageDispatcher.AddListener(WsMessageType.SendWsAvatar.ToString(), SendWsAvatar, true);
            MessageDispatcher.AddListener(WsMessageType.SendPlaceMark.ToString(), SendPlaceMark, true);
            MessageDispatcher.AddListener(WsMessageType.SendChangeObj.ToString(), SendChangeObj, true);
            MessageDispatcher.AddListener(WsMessageType.SendCChangeObj.ToString(), SendCChangeObj, true);
            MessageDispatcher.AddListener(WsMessageType.SendAllCChangeObj.ToString(), SendAllCChangeObj, true);
            MessageDispatcher.AddListener(WsMessageType.SendMarkAdmin.ToString(), SendMarkAdmin, true);
            MessageDispatcher.AddListener(WsMessageType.SendTeleportTo.ToString(), SendTeleportTo, true);
            MessageDispatcher.AddListener(WsMessageType.SendLoadScene.ToString(), SendLoadScene, true);
            MessageDispatcher.AddListener(WsMessageType.SendMovingObj.ToString(), SendMovingObj, true);
            MessageDispatcher.AddListener(WsMessageType.SendMedia.ToString(), SendMedia, true);
            MessageDispatcher.AddListener(WsMessageType.SendBigScreen.ToString(), SendBigScreen, true);
            MessageDispatcher.AddListener(WsMessageType.SendPCCamera.ToString(), SendPCCamera, true);
            MessageDispatcher.AddListener(WsMessageType.SendProgress.ToString(), SendProgress, true);
            MessageDispatcher.AddListener(WsMessageType.SendChangeAvatar.ToString(), SendChangeAvatar, true);
            MessageDispatcher.AddListener(WsMessageType.SendUrlScene.ToString(), SendUrlScene, true);
            MessageDispatcher.AddListener(WsMessageType.SendGetData.ToString(), SendGetData, true);
            MessageDispatcher.AddListener(WsMessageType.SendSaveData.ToString(), SendSaveData, true);
            MessageDispatcher.AddListener(WsMessageType.SendChangeRoom.ToString(), SendChangeRoom, true);
            MessageDispatcher.AddListener(WsMessageType.SendCanvasView.ToString(), SendCanvasView, true);
            MessageDispatcher.AddListener(WsMessageType.SendVRVoice.ToString(), SendVRVoice, true);
            MessageDispatcher.AddListener(WsMessageType.SendVRPic.ToString(), SendVRPic, true);

            CheckHeart();
            //InvokeRepeating("CheckHeart", 2, CheckHeartTime);
            //InvokeRepeating("CheckDelayByWs", 0, CheckDelayTime);
        }
        private void OnEnable()
        {
        }
        IEnumerator IEReconnectWsRoom()
        {
            DisConeectTemp();
            yield return new WaitForSeconds(0.1f);
            ConnectSocketIO();
        }

        bool checkheartmark = false;
        int rechecknum = 0;
        string authfaildString = "";

        bool isSocketOpenConnectVoice = false;

        public void setCheckMarkOK()
        {
            checkheartmark = true;
            rechecknum = 0;
        }

        void CheckHeart()
        {
            StartCoroutine(IEReconnectWsRoom());
        }


        void DelayExitRoom()
        {
            //GUIManager.Instance.loginUIController.ExitRoom(false);
            //GameManager.Instance.infoController.InfoLog("已经尝试重新连接 4 次,无法重新连接到房间，请检查您的网络。", InfoColor.red, 8f);
        }

        float sendtime;
        void CheckDelayByWs()
        {
            if (mStaticThings.I.WsAvatarIsReady && sendedready)
            {
                sendtime = Time.realtimeSinceStartup;

                //StandaloneSender.SendCheckDelay();
            }
            else if ((!sendedready || !mStaticThings.I.WsAvatarIsReady) && !isauthfaild)
            {
                StandaloneSender.SendConnectReady(VSWorkSDK.EngineTestManager.Instance.avatarManager.GetCurrentAvatarFrame());
                sendedready = true;
            }
        }

        public void ConnectOfflineSocketIO()
        {
            sendedready = true;
            mStaticThings.I.WsAvatarIsReady = true;
            //mStaticThings.I.isAdmin = true;
            MessageDispatcher.SendMessage(this, VrDispMessageType.RoomConnected.ToString(), mStaticThings.I.nowRoomServerUrl, 0);
            //VRMenuController.I.SystemMemuEnable(false);
            //MessageDispatcher.SendMessage(this, VrDispMessageType.ForceSystemMenuEnable.ToString(), false, 0);
        }

        public void ConnectSocketIO(bool connectVoice = false)
        {
            DisConeectTemp();
            isSocketOpenConnectVoice = connectVoice;
            DoConnectSocketIO();

        }

        public void ConnectChanelRoomByChID(string roomChID)
        {
            mStaticThings.I.ischconnecting = true;
            ConnectSocketIO();
        }

        void DoConnectSocketIO()
        {
            string serverip = mStaticThings.I.nowRoomServerUrl;
            isauthfaild = false;
            if (serverip.Contains("127.0.0.1"))
            {
                ConnectOfflineSocketIO();
            }
            else
            {
                //if (IsInvoking("checkconnecttimeout"))
                //{
                //    CancelInvoke("checkconnecttimeout");
                //}
                //open
                CancelInvoke("DelayExitRoom");

                if (!sendedready && !isauthfaild)
                {
                    StandaloneSender.SendConnectReady(VSWorkSDK.EngineTestManager.Instance.avatarManager.GetCurrentAvatarFrame());
                    sendedready = true;
                }

                //if (IsInvoking("checkconnecttimeout"))
                //{
                //    CancelInvoke("checkconnecttimeout");
                //}
                mStaticThings.I.ischconnecting = false;
                //Invoke("checksocketlink", OpenCheckConnectionTime);

                StandaloneSender.SendConnectReady(VSWorkSDK.EngineTestManager.Instance.avatarManager.GetCurrentAvatarFrame());
            }
            bConnected = true;
        }

        public void checkconnecttimeout()
        {
            MessageDispatcher.SendMessage(VrDispMessageType.RoomConnectTimeOut.ToString());
            MessageDispatcher.SendMessage(this, VrDispMessageType.RoomConnectedError.ToString(), mStaticThings.I.nowRoomServerUrl, 0);
        }

        public void DisConnectSocketIO()
        {
            MessageDispatcher.SendMessage(this, VrDispMessageType.RoomConnectedClose.ToString(), mStaticThings.I.nowRoomServerUrl, 0);
            mStaticThings.I.isAdmin = false;
            sendedready = false;
            ClearWsAvatar();
            mStaticThings.I.WsAvatarIsReady = false;
            MessageDispatcher.SendMessage(this, VrDispMessageType.RoomDisConnected.ToString(), mStaticThings.I.nowRoomServerUrl, 0);

            if (IsInvoking("checkconnecttimeout"))
            {
                CancelInvoke("checkconnecttimeout");
            }

            bConnected = false;
        }


        public void DisConeectTemp()
        {
            sendedready = false;
            mStaticThings.I.WsAvatarIsReady = false;
        }




        private float timeCount = 0.0f;
        private void Update()
        {
            timeCount += Time.deltaTime;
            if (timeCount > 0.5f)
            {
                StandaloneSendSynMsg();
                timeCount = 0.0f;
            }
        }

        private void StandaloneSendSynMsg()
        {
            sendSynChcekAvatar();
            sendAlist();
        }
        private void sendSynChcekAvatar()
        {
            //if (StandaloneSender.roomobj.avatars.Count <= 0) return;

            //for (int i = 0; i < StandaloneSender.roomobj.avatars.Count; i++)
            //{
            //    if (!StandaloneSender.socketDic.Contains(StandaloneSender.roomobj.avatars[i].Wsid))
            //    {
            //        if (StandaloneSender.avatarroomdic.ContainsKey(StandaloneSender.roomobj.avatars[i].Id))
            //        {
            //            StandaloneSender.avatarroomdic.Remove(StandaloneSender.roomobj.avatars[i].Id);
            //        }
            //        StandaloneSender.roomobj.avatars.RemoveAt(i);
            //        i--;
            //    }
            //}

            var nowroom = StandaloneSender.roomobj;

            var AvatarFrameList = new WsAvatarFrameList();
            AvatarFrameList.alist = new List<WsAvatarFrame>();
            AvatarFrameList.chdata = new Dictionary<string, string>();

            for (var i = 0; i < nowroom.avatars.Count; i++)
            {
                var avatarInfo = nowroom.avatars[i];

                AvatarFrameList.alist.Add(avatarInfo);
            }
            AvatarFrameList.nowscene = nowroom.nowscene;
            AvatarFrameList.nowmedia = nowroom.nowmedia;
            AvatarFrameList.nowbigscreen = nowroom.nowbigscreen;

            foreach (var key in nowroom.chdata.Keys)
            {
                var value = nowroom.chdata[key];
                //var keyValueData = new pbStrKeyValue();
                //keyValueData.Key = key;
                //keyValueData.Value = value;
                AvatarFrameList.chdata.Add(key,value);
            }

            StandaloneSender.SendSYN_CHECKAVATAR(AvatarFrameList);
        }
        private void sendAlist()
        {
            //if (StandaloneSender.socketDic.Count <= 0) return;

            var nowroom = StandaloneSender.roomobj;

            if (nowroom.avatars.Count > 0)
            {
                if (nowroom.alist.Count > 0)
                {
                    //var msg = new S_SYN_AVATARLIST();
                    var AvatarFrameJianList = new WsAvatarFrameJianList();

                    for (var i = 0; i < nowroom.alist.Count; i++)
                    {
                        var avatarInfo = nowroom.alist[i];

                        AvatarFrameJianList.jlist.Add(avatarInfo);
                    }

                    StandaloneSender.SendSYN_AVATARLIST(AvatarFrameJianList);

                    nowroom.alist.Clear();
                }
            }
        }

        private void LateUpdate()
        {
        }

        void checksocketlink()
        {
            //if (socketObj == null)
            //{
            //    MessageDispatcher.SendMessage(this, VrDispMessageType.RoomDisConnected.ToString(), mStaticThings.I.nowRoomServerUrl, 0);
            //}
        }

        public void SetAuthFaild(string logstring = "")
        {
            isauthfaild = true;
            //GUIManager.Instance.loginUIController.ExitRoom(true);
        }

        public void SetConnectReady()
        {
            if (!isauthfaild)
            {
                MessageDispatcher.SendMessage(this, VrDispMessageType.RoomConnected.ToString(), mStaticThings.I.nowRoomServerUrl, 0);
            }
        }

        public void SendWsAvatar(WsAvatarFrameJian nowframe)
        {
            StandaloneSender.SendWsAvatar(nowframe);
        }

        public void SendChangeAvatar(IMessage msg)
        {
            WsAvatarFrame nowframe = VSWorkSDK.EngineTestManager.Instance.avatarManager.GetCurrentAvatarFrame();

            StandaloneSender.SendChangeAvatar(nowframe);
        }

        void SendGetData(IMessage msg)
        {
            string key = "";
            if (msg.Data != null && msg.Data.GetType() == typeof(VRSaveRoomData))
            {

                VRSaveRoomData sendsave = msg.Data as VRSaveRoomData;
                key = sendsave.key;
            }
            VRSaveRoomData newsave = new VRSaveRoomData
            {
                key = key
            };
            StandaloneSender.SendGetData(newsave);
        }


        void SendSaveData(IMessage msg)
        {

            VRSaveRoomData data = msg.Data as VRSaveRoomData;

            StandaloneSender.SendSaveData(data);
        }

        void SendChangeRoom(IMessage msg)
        {
            VRChanelRoom newroom = msg.Data as VRChanelRoom;

            if (mStaticThings.I.WsAvatarIsReady && newroom.roomid == mStaticThings.I.nowRoomExChID)
            {
                return;
            }

            mStaticThings.I.nowRoomChID = mStaticThings.I.nowRoomStartChID + newroom.roomid;
            mStaticThings.I.nowRoomExChID = newroom.roomid;

            if ((bool)msg.Sender)
            {

                ConnectChanelRoomByChID(mStaticThings.I.nowRoomChID);
            }
            else
            {
                newroom.roomid = mStaticThings.I.nowRoomChID;
                StandaloneSender.SendChangeRoom(newroom);
            }
        }

        void SendPlaceMark(IMessage msg)
        {
            WsPlaceMarkList newChange = msg.Data as WsPlaceMarkList;

            if (newChange != null)
            {
                StandaloneSender.SendPlaceMarkList(newChange);
            }
        }
        void SendChangeObj(IMessage msg)
        {
            WsChangeInfo newChange = msg.Data as WsChangeInfo;

            StandaloneSender.SendChangeObj(newChange);
        }

        void SendCChangeObj(IMessage msg)
        {
            WsCChangeInfo newChange = msg.Data as WsCChangeInfo;

            StandaloneSender.SendCChangeObj(newChange, 0);
        }

        void SendAllCChangeObj(IMessage msg)
        {
            WsCChangeInfo newChange = msg.Data as WsCChangeInfo;

            StandaloneSender.SendCChangeObj(newChange, 1);
        }

        void SendMarkAdmin(IMessage msg)
        {
            var data = msg.Data as WsAdminMark;
            if (data != null)
            {
                StandaloneSender.SendMarkAdmin(/*data.name, */data.id);
            }
        }

        void SendTeleportTo(IMessage msg)
        {
            var data = msg.Data as WsMultiTeleportInfo;
            if (data != null)
            {
                StandaloneSender.SendMultiTeleportInfo(data);
            }
        }

        void SendLoadScene(IMessage msg)
        {
            var data = msg.Data as WsSceneInfo;
            if (data != null)
            {
                StandaloneSender.SendLoadScene(data);
            }
        }

        void SendMovingObj(IMessage msg)
        {
            var data = msg.Data as WsMovingObj;
            if (data != null)
            {
                StandaloneSender.SendMovingObj(data);
            }
        }

        public void SendNewAvatar(bool isfromsdk, bool hasLinkedScene = false)
        {
            StartCoroutine(IEWaitSendAvatar(isfromsdk, hasLinkedScene));
        }

        IEnumerator IEWaitSendAvatar(bool isfromsdk, bool hasLinkedScene)
        {
            yield return new WaitForSeconds(2);
            WsAvatarFrame newf = VSWorkSDK.EngineTestManager.Instance.avatarManager.GetCurrentAvatarFrame();

            if (isfromsdk)
            {
                newf.scene.scene = "";
                newf.scene.id = "";
                newf.scene.name = "";
            }
            else if (hasLinkedScene)
            {
                newf.scene = mStaticThings.I.nowRoomLinkScene;
            }

            StandaloneSender.SendNewAvatar(newf);
        }

        void SendMedia(IMessage msg)
        {
            var data = msg.Data as WsMediaFrame;
            StandaloneSender.SendMediaInfo(data);
        }

        void SendBigScreen(IMessage msg)
        {
            var data = msg.Data as WsBigScreen;
            StandaloneSender.SendBigSceneInfo(data);
        }

        void SendPCCamera(IMessage msg)
        {
            var data = msg.Data as CameraScreenInfo;
            StandaloneSender.SendCameraScreenInfo(data);
        }

        void SendProgress(IMessage msg)
        {
            var data = msg.Data as WsProgressInfo;
            StandaloneSender.SendProgressInfo(data);
        }

        void SendUrlScene(IMessage msg)
        {
            var data = msg.Data as WsMediaFile;
            StandaloneSender.SendUrlScene(data);
        }

        void SendCanvasView(IMessage msg)
        {
            var data = msg.Data as CanvasWebviewActionFrame;
            StandaloneSender.SendCanvasView(data);
        }

        void SendVRVoice(IMessage msg)
        {
            var data = msg.Data as WsVoiceFrame;
            StandaloneSender.SendVoiceFrame(data);
        }

        void SendVRPic(IMessage msg)
        {
            var data = msg.Data as WsPicFrame;
            StandaloneSender.SendPicFrame(data);
        }

        public void mRecieveCheckDelay(/*JSONObject e*/)
        {
            setCheckMarkOK();
        }

        void ClearWsAvatar()
        {
            VSWorkSDK.EngineTestManager.Instance.avatarManager.ClearAvatars();
            mStaticThings.DynClientAvatarsDic.Clear();
            mStaticThings.AllStaticAvatarsDic.Clear();
            mStaticThings.AllStaticAvatarList.Clear();
        }


    }
}