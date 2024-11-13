using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSWorkSDK
{
    public class RoomInfo
    {
        public Dictionary<string, WsAvatarFrame> linkdic = new Dictionary<string, WsAvatarFrame>();
        public List<WsAvatarFrame> avatars = new List<WsAvatarFrame>();
        public List<WsPlaceMark> playcelist = new List<WsPlaceMark>();
        public string gname = string.Empty;
        public WsMediaFrame nowmedia = new WsMediaFrame();
        public WsSceneInfo nowscene = new WsSceneInfo();
        public WsBigScreen nowbigscreen = new WsBigScreen();
        public List<WsAvatarFrameJian> alist = new List<WsAvatarFrameJian>();
        public Dictionary<string, string> chdata = new Dictionary<string, string>();
    };
    public class StandaloneSender
    {
        public static string socket_client_id = "MY_Client";
        public static string nowRoomID = "MY_Room";
        public static RoomInfo roomobj = new RoomInfo();//模拟房间信息

        public static Dictionary<string, string> avatarroomdic = new Dictionary<string, string>();//模拟人物信息

        public static List<string> socketDic = new List<string>();//模拟连接数
        public static void SendMarkAdmin(string SysAvatarID)
        {
            var nowroom = roomobj;

            WsAvatarFrame tempavatar = null;
            for (var i = 0; i < nowroom.avatars.Count; i++)
            {
                if (nowroom.avatars[i].id == SysAvatarID)
                {
                    tempavatar = nowroom.avatars[i];
                    nowroom.avatars.RemoveAt(i);
                    break;
                }
            }
            if (tempavatar != null)
            {
                nowroom.avatars.Insert(0,tempavatar);
            }

            var ConnectAvatars = new ConnectAvatars();

            ConnectAvatars.wsid = "";
            ConnectAvatars.sort = 0;
            ConnectAvatars.sceneavatars = new List<WsAvatarFrame>();
            ConnectAvatars.nowscene = new WsSceneInfo();
            ConnectAvatars.chdata = new Dictionary<string, string>();
            ConnectAvatars.nowmedia = new WsMediaFrame();
            ConnectAvatars.nowbigscreen = new WsBigScreen();

            for (var i = 0; i < nowroom.avatars.Count; i++)
            {
                var avatarInfo = nowroom.avatars[i];


                ConnectAvatars.sceneavatars.Add(avatarInfo);
            }

            EngineTestManager.roomData.OnMarkAdminMsgReturn(ConnectAvatars);
        }

        public static void SendWsAvatar(WsAvatarFrameJian data)
        {
            var nowroom = roomobj;
            if (nowroom != null)
            {
                nowroom.alist.Add(data);
            }
        }

        public static void SendChangeObj(WsChangeInfo data)
        {
            EngineTestManager.roomData.OnChangeObjMsgReturn(data);
        }

        public static void SendCChangeObj(WsCChangeInfo data, int Type)
        {
            //type 暂时无用，因为没模拟频道
            EngineTestManager.roomData.OnCChangeObjMsgReturn(data);
        }

        public static void SendPlaceMarkList(WsPlaceMarkList data)
        {
            var nowroom = roomobj;
            if (nowroom != null)
            {
                if (data.kind == WsPlaycePortKind.all)
                {
                    nowroom.playcelist.Clear();
                    nowroom.playcelist.AddRange(data.marks);
                    nowroom.gname = data.gname;
                }
                else
                {
                    if (nowroom.avatars[0] != null && data.id == nowroom.avatars[0].id)
                    {
                        nowroom.gname = data.gname;
                    }
                    changePlaycelist(data.marks);
                }

                EngineTestManager.roomData.OnPlaceMarkListMsgReturn(data);
            }
        }
        private static void changePlaycelist(List<WsPlaceMark> WsPlaceMarks)
        {
            var nowroom = roomobj;
            var marks = WsPlaceMarks;
            if (marks.Count > 0)
            {
                //deletePlayce(WsPlaceMark.marks[0].id);
                var delid = marks[0].id;
                for (var i = 0; i < nowroom.playcelist.Count; i++)
                {
                    if (nowroom.playcelist[i].id == delid)
                    {
                        nowroom.playcelist.RemoveAt(i);
                        nowroom.playcelist.Add(marks[0]);
                        break;
                    }
                }
            }
        }

        public static void SendMultiTeleportInfo(WsMultiTeleportInfo data)
        {
            EngineTestManager.roomData.OnMultiTeleportInfoMsgReturn(data);
        }

        public static void SendLoadScene(WsSceneInfo data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            var sceneInfo = data;

            var nowroom = roomobj;
            if (nowroom.avatars.Count > 0)
            {
                if (!sceneInfo.isupdate)
                {
                    if (!sceneInfo.iskod)
                    {
                        nowroom.nowscene = sceneInfo;
                    }
                }
            }
            //cleannullavatar(nowRoomID);
            if (!string.IsNullOrEmpty(sceneInfo.id))
            {
                EngineTestManager.roomData.OnLoadSceneMsgReturn(data);
            }
        }

        public static void SendMovingObj(WsMovingObj data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            EngineTestManager.roomData.OnMovingObjMsgReturn(data);
        }

        public static void SendNewAvatar(WsAvatarFrame data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            var newavatar = data;

            var nowroom = roomobj;
            if (nowroom != null && nowroom.linkdic.ContainsKey(socket_client_id) && nowroom.linkdic[socket_client_id] == null)
            {
                nowroom.linkdic[socket_client_id] = newavatar;
            }
            else
            {
                //initroomobj(nowroom);
                nowroom.linkdic[socket_client_id] = newavatar;
            }
            var index = -1;
            for (var i = 0; i < nowroom.avatars.Count; i++)
            {
                if (nowroom.avatars[i].id == newavatar.id)
                {
                    index = i;
                }
            }
            if (index == -1 && newavatar.wsid != "_temp")
            {
                if (nowroom.avatars.Count == 0)
                {
                    nowroom.nowscene = newavatar.scene;
                }

                if (newavatar.ae != false)
                {
                    if (string.IsNullOrEmpty(newavatar.cl))
                    {
                        newavatar.cl = "#FFFFFF";
                    }

                    nowroom.avatars.Add(newavatar);
                }
                else
                {
                    if (string.IsNullOrEmpty(newavatar.cl))
                    {
                        newavatar.cl = "#FFFFFF";
                    }

                    newavatar.ae = true;

                    nowroom.avatars.Add(newavatar);
                }

#if UNITY_EDITOR
                Debug.Log("--------------------");
                Debug.Log("room: " + nowRoomID + " : 角色: " + newavatar.name + "   ID: " + newavatar.id + "  已创建 ,  当前角色数: " + nowroom.avatars.Count);
                Debug.Log("Admin Is: " + nowroom.avatars[0].name + "   ID: " + nowroom.avatars[0].id);
                Debug.Log("--------------------");
#endif
                avatarroomdic[newavatar.id] = nowRoomID;

                EngineTestManager.roomData.OnNewAvatarMsgReturn(newavatar);
            }
            //cleannullavatar(nowRoomID);

        }

        //private static void cleannullavatar(string nowroomID)
        //{
        //    var nowroom = roomobj;
        //    if (nowroom != null)
        //    {
        //        for (var i = 0; i < nowroom.avatars.Count; i++)
        //        {
        //            if (nowroom.avatars[i] == null || nowroom.avatars[i] == null || string.IsNullOrEmpty(nowroom.avatars[i].Id) || nowroom.avatars[i].Wsid == "_temp")
        //            {
        //                nowroom.avatars.RemoveAt(i);
        //                i--;
        //            }
        //        }
        //    }
        //}

        public static void SendConnectReady(WsAvatarFrame data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);
//#if 模拟发送信息
            SendCONNECT_READY();
//#endif
        }

        public static void SendMediaInfo(WsMediaFrame data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            var mediaFrame = data;
            if (mediaFrame != null)
            {
                var nowroom = roomobj;
                if (nowroom != null)
                {
                    if (mediaFrame.files[0].ext == "scene" || mediaFrame.files[0].ext == "xscene")
                    {
                        if (!mediaFrame.files[0].isupdate)
                        {
                            nowroom.nowscene.scene = (mediaFrame.files[0].name);
                            nowroom.nowscene.name = (mediaFrame.files[0].name);
                            nowroom.nowscene.isremote = (true);
                            nowroom.nowscene.isupdate = (false);
                            nowroom.nowscene.iskod = (true);
                            nowroom.nowscene.kod = (mediaFrame.files[0]);
                        }
                    }
                    else
                    {
                        if (mediaFrame.files[0].ext == "jpg" || mediaFrame.files[0].ext == "png")
                        {
                            nowroom.nowmedia = mediaFrame;
                        }
                    }

                    EngineTestManager.roomData.OnMediaInfoMsgReturn(mediaFrame);
                }
            }
        }

        public static void SendBigSceneInfo(WsBigScreen data)
        {

            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);
            var bigScreen = data;

            var nowroom = roomobj;
            if (nowroom != null)
            {
                nowroom.nowbigscreen = bigScreen;

                EngineTestManager.roomData.OnBigSceneInfoMsgReturn(bigScreen);
            }
        }

        public static void SendCameraScreenInfo(CameraScreenInfo data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            EngineTestManager.roomData.OnCameraScreenInfoMsgReturn(data);
        }

        public static void SendProgressInfo(WsProgressInfo data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            EngineTestManager.roomData.OnProgressInfoMsgReturn(data);
        }

        //public static void SendCheckDelay()
        //{
        //    C_CHECK_DELAY msg = new C_CHECK_DELAY();

        //    //GameManager.Instance.socketIO.SendMsg(msg);
        //    //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

        //    var msg2 = new S_CHECK_DELAY();
        //    msg2.Time = 123;
        //    EngineTestManager.roomData.OnCheckDelayMsgReturn(msg2);
        //}

        public static void SendChangeAvatar(WsAvatarFrame data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            var avatarFrame = data;

            var nowroom = roomobj;
            if (nowroom != null)
            {
                if (string.IsNullOrEmpty(avatarFrame.cl))
                {
                    avatarFrame.cl = "#C0C0C0";
                }
                for (var i = 0; i < nowroom.avatars.Count; i++)
                {
                    if (nowroom.avatars[i].id.Equals(avatarFrame.id))
                    {
                        nowroom.avatars[i].name = (avatarFrame.name);
                        nowroom.avatars[i].sex = (avatarFrame.sex);
                        nowroom.avatars[i].aid = (avatarFrame.aid);
                        nowroom.avatars[i].wsid = (avatarFrame.wsid);
                        nowroom.avatars[i].ae = (avatarFrame.ae);
                        nowroom.avatars[i].cl = (avatarFrame.cl);
                        break;
                    }
                }
            }
        }

        public static void SendUrlScene(WsMediaFile data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            var mediaFile = data;
            var nowroom = roomobj;
            if (nowroom != null)
            {
                nowroom.nowscene.scene = (mediaFile.name);
                nowroom.nowscene.name = (mediaFile.name);
                nowroom.nowscene.isremote = (true);
                nowroom.nowscene.isupdate = (false);
                nowroom.nowscene.iskod = (true);
                nowroom.nowscene.kod = (mediaFile);
            }
        }

        public static void SendSaveData(VRSaveRoomData data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);
            var saveData = data;
            if (saveData.sall)
            {
                roomobj.chdata[saveData.key] = saveData.value;
            }
            else
            {
                var nowroom = roomobj;
                if (nowroom != null)
                {
                    nowroom.chdata[saveData.key] = saveData.value;
                }
            }
        }

        public static void SendGetData(VRSaveRoomData data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            var saveData = data;
            Dictionary<string, string> DataDic = new Dictionary<string, string>();
            DataDic.Clear();
            var nowroom = roomobj;
            if (nowroom != null)
            {
                if (saveData != null && !string.IsNullOrEmpty(saveData.key))
                {
                    if (nowroom.chdata.ContainsKey(saveData.key))
                    {
                        DataDic.Add(saveData.key, saveData.value);
                    }
                }
                else
                {
                    foreach (var key in nowroom.chdata.Keys)
                    {
                        var value = nowroom.chdata[key];
                        DataDic.Add(key,value);
                    }
                }

                EngineTestManager.roomData.OnGetDataMsgReturn(DataDic);
            }
        }

        public static void SendChangeRoom(VRChanelRoom data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            EngineTestManager.roomData.OnChangeRoomMsgReturn(data);
        }

        public static void SendCanvasView(CanvasWebviewActionFrame data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);

            EngineTestManager.roomData.OnCanvasViewMsgReturn(data);
        }

        public static void SendVoiceFrame(WsVoiceFrame data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);
            EngineTestManager.roomData.OnVoiceFrameMsgReturn(data);
        }

        public static void SendPicFrame(WsPicFrame data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, msg);
            EngineTestManager.roomData.OnPicFrameMsgReturn(data);
        }

//#if 模拟发送信息

        public static void SendSYN_CHECKAVATAR(WsAvatarFrameList data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, data);

            EngineTestManager.roomData.OnCheckAvatarMsgReturn(data);
        }

        public static void SendSYN_AVATARLIST(WsAvatarFrameJianList data)
        {
            //GameManager.Instance.socketIO.SendMsg(msg);
            //OneParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendMsg, data);
            EngineTestManager.roomData.OnSynAvatarListMsgReturn(data);
        }

        //public static void SendAuthFaild()
        //{
        //    var msg = new S_AUTH_FAILD();

        //    MessageDataManager.roomData.OnAuthFaildMsgReturn(msg);
        //}

        private static void SendCONNECT_READY()
        {
            var nowroom = roomobj;

            //var msg = new S_CONNECT_READY();
            var ConnectAvatars = new ConnectAvatars();

            ConnectAvatars.wsid = socket_client_id;
            ConnectAvatars.sort = nowroom.avatars.Count;
            ConnectAvatars.sceneavatars = new List<WsAvatarFrame>();
            ConnectAvatars.nowscene = new WsSceneInfo();
            ConnectAvatars.chdata = new Dictionary<string, string>();
            ConnectAvatars.nowmedia = new WsMediaFrame();
            ConnectAvatars.nowbigscreen = new WsBigScreen();

            for (var i = 0; i < nowroom.avatars.Count; i++)
            {
                var avatarInfo = nowroom.avatars[i];
                ConnectAvatars.sceneavatars.Add(avatarInfo);
            }
            ConnectAvatars.nowscene = nowroom.nowscene;
            ConnectAvatars.nowmedia = nowroom.nowmedia;
            ConnectAvatars.nowbigscreen = nowroom.nowbigscreen;

            foreach (var key in nowroom.chdata.Keys)
            {
                var value = nowroom.chdata[key];
                //var keyValueData = new pbStrKeyValue();
                //keyValueData.Key = key;
                //keyValueData.Value = value;
                ConnectAvatars.chdata.Add(key,value);
            }
            //submsg.setChdataList(utils.utils.CheckNull(roomobj[nowroom].chdata));
            if (nowroom.avatars.Count == 0)
            {
                ConnectAvatars.cl = "#FF0000";
            }
            else
            {
                ConnectAvatars.cl = "#FFFFFF";
            }

            EngineTestManager.roomData.OnConnectReadyMsgReturn(ConnectAvatars);
        }
        private static void deletePlayce(string id)
        {
            for (var i = 0; i < roomobj.playcelist.Count; i++)
            {
                if (roomobj.playcelist[i].id == id)
                {
                    roomobj.playcelist.RemoveAt(i);
                    break;
                }
            }
        }
        public static void AvatarDisconnect(string id)
        {
            deletePlayce(id);
            ///////////////////////////////////////////////////////////avatar 优化
            for (var i = 0; i < roomobj.avatars.Count; i++)
            {
                if (roomobj.avatars[i].id == id || roomobj.avatars[i] == null)
                {
                    roomobj.avatars.RemoveAt(i);
                    break;
                }
            }

            EngineTestManager.roomData.OnAvatarDisconnectMsgReturn(id);

            MessageDispatcher.SendMessage(true, VrDispMessageType.DestroyWsAvatar.ToString(), id, 0);

            Debug.Log("**********");
            Debug.Log("AvatarID: " + id + "断开链接 , 当前所有连接数 : __" + roomobj.avatars.Count);
            Debug.Log("**********");
        }

        //#endif
    }
}