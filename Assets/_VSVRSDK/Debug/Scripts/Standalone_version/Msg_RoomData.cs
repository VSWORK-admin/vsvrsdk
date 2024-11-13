using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using com.ootii.Messages;
using UnityEngine.SceneManagement;
using LitJson;

namespace VSWorkSDK
{
    public class CloudRenderRoomData
    {
        public string vr_rooms_id;
        public string vr_rooms_name;
        public string vr_rooms_socketioserver;
        public string vr_rooms_firstroom;
        public string vr_rooms_pass;
        public string vr_rooms_title;
        public string vr_rooms_settings;
        public int vr_rooms_mediaen;
        public int vr_rooms_vtype;
        public int vr_rooms_ven;
        public int vr_rooms_firstteam;
        public int vr_rooms_en3d;
        public int vr_rooms_vrange;
        public float vr_rooms_maxhdis;
        public int vr_rooms_adis;
        public string vr_rooms_gmeappid;
        public string vr_rooms_gmeappkey;
        public string vr_rooms_gmeroomid;
        public int vr_rooms_gmetxtenabled;
        public string vr_rooms_voiceapi;
        public string vr_rooms_actionapi;
        public string vr_rooms_tbpapi;
        public int vr_rooms_exenable;
        public int vr_rooms_chenable;
        public string vr_rooms_icon;
        public int vr_rooms_max;
        public int vr_rooms_aemax;
        public string vr_rooms_admincmd;
        public string vr_rooms_token;
        public string vr_scenes_id;
        public string create_time;
        public string update_time;
        public string vr_rooms_config;
        public int vr_rooms_share;
        public int vr_group_max;
        public int vr_rooms_vip;
        public int vr_rooms_bind;
    }

    public class CloudRenderAvatarData
    {
        public int public_avatars_id;
        public string public_avatars_mark;
        public string vr_avatars_id;
        public int public_avatars_sort;
        public int public_avatars_enable;
        public string app_id;
        public string vr_avatars_path;
        public string vr_avatars_bundle;
        public string vr_avatars_glb;
        public string vr_avatars_glbkind;
        public string vr_avatars_version;
        public string vr_avatars_name;
        public int vr_avatars_gender;
        public int vr_avatars_glbv;
        public string vr_avatars_intro;
        public string vr_avatars_icon;
        public int vr_avatars_sort;
        public int vr_avatars_enabled;
        public string create_time;
        public string update_time;
        public string group_id;
        public string vr_avatars_show;
        public int is_top;
        public int is_public_recommend;
    }

    public class CloudRenderUserData
    {
        public string vr_user_nickname;
        public string vr_user_id;
        public int vr_user_sex;
    }

    public class CloudRenderGroup
    {
        public string vr_user_id;
        public string vr_user_name;
        public string vr_user_country;
        public string vr_user_phone;
        public string vr_user_userpass;
        public string vr_user_intro;
        public string vr_user_nickname;
        public string vr_user_icon;
        public string vr_user_ustream;
        public string vr_user_stream;
        public int vr_user_sex;
        public string vr_user_regapp;
        public string vr_user_token;
        public string vr_user_logintime;
        public int user_group_id;
        public string vr_group_id;
        public string user_group_pass;
        public string user_group_name;
        public int user_group_single;
        public string name;
        public int user_group_enabled;
        public int user_group_enhide;
        public string vr_group_name;
        public string vr_group_intro;
        public string vr_group_icon;
        public int vr_group_filekind;
        public string vr_group_fileserver;
        public string vr_group_fileurl;
        public string vr_group_fileweb;
        public string vr_group_fileadmin;
        public string vr_group_filepass;
        public string vr_group_linkroomid;
        public int user_max_num;
        public string initial_admin;
        public string initial_password;
        public string create_time;
        public string update_time;
        public string app_id;
        public string vr_group_alias;

    }

    public class CloudRenderLoginData
    {
        public int messageId;
        public CloudRenderRoomData roomInfo;
        public CloudRenderAvatarData avatar;
        public CloudRenderUserData userInfo;
        public CloudRenderGroup group;
        public string apitoken;
        public string username;
        public bool isPC;
        public bool debug;
        public bool openAgora;
        public bool hideLeftStick;
    }
    public class Msg_RoomData
    {
        //新连接进服务器接收自身 和 已有房间内的角色信息
        public void OnMarkAdminMsgReturn(ConnectAvatars cavatars)
        {
            if (cavatars.sceneavatars.Count <= 0)
            {
                //GameManager.Instance.infoController.InfoLog(LanguageDataBase.GetLanguageString("VRController", "AvatarNotReady"), InfoColor.yellow);
                return;
            }

            if (cavatars.sceneavatars[0].id == mStaticThings.I.mAvatarID)
            {
                mStaticThings.I.isAdmin = true;
            }
            else
            {
                mStaticThings.I.isAdmin = false;
            }
            mStaticThings.AllStaticAvatarsDic.Clear();
            mStaticThings.AllStaticAvatarList.Clear();
            foreach (var item in cavatars.sceneavatars)
            {
                if (!mStaticThings.I.ActionBlackList.Contains(item.id))
                {
                    mStaticThings.AllStaticAvatarsDic.Add(item.id, item);
                    mStaticThings.AllStaticAvatarList.Add(item.id);
                }
            }
            MessageDispatcher.SendMessage(this, VrDispMessageType.Adminchanged.ToString(), cavatars, 0);
            //GameManager.Instance.infoController.InfoLog(LanguageDataBase.GetLanguageString("WsSocketIOController", "SocketIOConnect_2") + cavatars.sceneavatars[0].name);
            //GameManager.Instance.infoController.InfoLog("主持人切换为 :  " + cavatars.sceneavatars[0].name);
        }

        public void OnChangeObjMsgReturn(WsChangeInfo newChange)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveChangeObj.ToString(), newChange, 0);
        }

        public void OnCChangeObjMsgReturn(WsCChangeInfo newChange)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveCChangeObj.ToString(), newChange, 0);
        }

        public void OnPlaceMarkListMsgReturn(WsPlaceMarkList newPlayceMark)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecievePlaceMark.ToString(), newPlayceMark, 0);
        }

        public void OnMultiTeleportInfoMsgReturn(WsMultiTeleportInfo newTeleinfo)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveTeleportTo.ToString(), newTeleinfo, 0);
        }

        public void OnLoadSceneMsgReturn(WsSceneInfo newSceneInfo)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveLoadScene.ToString(), newSceneInfo, 0);
        }
        public void OnMovingObjMsgReturn(WsMovingObj newSceneInfo)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveMovingObj.ToString(), newSceneInfo, 0);
        }
        //新加入角色
        public void OnNewAvatarMsgReturn(WsAvatarFrame NewAvatarFrame)
        {
            if (mStaticThings.I.ActionBlackList.Contains(NewAvatarFrame.id))
            {
                return;
            }


            if (mStaticThings.I.mAvatarID != NewAvatarFrame.id)
            {
                if (!VSWorkSDK.EngineTestManager.Instance.avatarManager.AllTestAvatar.ContainsKey(NewAvatarFrame.id))
                {
                    //Debug.LogWarning("Creat WS Avatar: " + NewAvatarFrame.id);
                    MessageDispatcher.SendMessage(true, VrDispMessageType.InitWsAvatar.ToString(), NewAvatarFrame, 0);
                }
                else
                {
                    VSWorkSDK.EngineTestManager.Instance.avatarManager.SetAvatarStopDestroy(NewAvatarFrame);
                }


            }

            if (!mStaticThings.DynClientAvatarsDic.ContainsKey(NewAvatarFrame.id))
            {
                mStaticThings.DynClientAvatarsDic.Add(NewAvatarFrame.id, NewAvatarFrame.ToJian());
            }

            if (!mStaticThings.AllStaticAvatarsDic.ContainsKey(NewAvatarFrame.id))
            {
                if (NewAvatarFrame.a && NewAvatarFrame.ae)
                {
                    mStaticThings.AllStaticAvatarList.Insert(0, NewAvatarFrame.id);
                    mStaticThings.AllActiveAvatarList.Insert(0, NewAvatarFrame.id);
                    if (NewAvatarFrame.id != mStaticThings.I.mAvatarID)
                    {
                        mStaticThings.I.isAdmin = false;
                    }
                    else
                    {
                        //mStaticThings.I.isAdmin = true;
                    }
                }
                else
                {
                    mStaticThings.AllStaticAvatarList.Add(NewAvatarFrame.id);
                    if (NewAvatarFrame.ae)
                    {
                        mStaticThings.AllActiveAvatarList.Add(NewAvatarFrame.id);
                    }

                }
                mStaticThings.AllStaticAvatarsDic.Add(NewAvatarFrame.id, NewAvatarFrame);

                //刷新人员列表
                //SingleMsg.AutoAllocate((int)MsgEnum_UnderMenu.RefuseUserList);
                //接收到新的人物
                MessageDispatcher.SendMessage(this, WsMessageType.RecieveNewAvartar.ToString(), NewAvatarFrame, 0);
            }
        }

        //新连接进服务器接收自身和 已有房间内的角色信息
        public bool isfirstopen = true;
        //首次连接频道 收到频道返回的信息
        public void OnConnectReadyMsgReturn(ConnectAvatars cavatars)
        {
            //ConnectAvatars cavatars = JsonUtility.FromJson<ConnectAvatars>(e.ToString());
            Debug.Log("Recieve CheckReady");


            //自身新增角色
            mStaticThings.I.mWsID = cavatars.wsid;
            if (mStaticThings.I.nowpencolor == "" && cavatars.cl != null && cavatars.cl != "")
            {
                mStaticThings.I.nowpencolor = cavatars.cl;
            }

            mStaticThings.AllStaticAvatarsDic.Clear();
            mStaticThings.DynClientAvatarsDic.Clear();
            mStaticThings.AllStaticAvatarList.Clear();
            mStaticThings.AllActiveAvatarList.Clear();
            if (mStaticThings.I.isAdmin && mStaticThings.I.SendAvatar)
            {
                mStaticThings.AllStaticAvatarsDic.Add(mStaticThings.I.mAvatarID, VSWorkSDK.EngineTestManager.Instance.avatarManager.GetCurrentAvatarFrame());
                mStaticThings.AllStaticAvatarList.Add(mStaticThings.I.mAvatarID);
                if (mStaticThings.I.SendAvatar)
                {
                    mStaticThings.AllActiveAvatarList.Add(mStaticThings.I.mAvatarID);
                }

            }
            //初始化已有角色
            if (cavatars.sceneavatars.Count > 0)
            {
                foreach (var item in cavatars.sceneavatars)
                {
                    if (!mStaticThings.AllStaticAvatarsDic.ContainsKey(item.id) && !mStaticThings.I.ActionBlackList.Contains(item.id))
                    {
                        mStaticThings.AllStaticAvatarsDic.Add(item.id, item);
                        mStaticThings.AllStaticAvatarList.Add(item.id);
                        if (item.ae)
                        {
                            mStaticThings.AllActiveAvatarList.Add(item.id);
                        }
                    }

                    if (!mStaticThings.DynClientAvatarsDic.ContainsKey(item.id) && item.id != mStaticThings.I.mAvatarID && !mStaticThings.I.ActionBlackList.Contains(item.id))
                    {

                        mStaticThings.DynClientAvatarsDic.Add(item.id, item.ToJian());
                        if (!VSWorkSDK.EngineTestManager.Instance.avatarManager.AllTestAvatar.ContainsKey(item.id))
                        {
                            MessageDispatcher.SendMessage(true, VrDispMessageType.InitWsAvatar.ToString(), item, 0);
                        }
                        else
                        {
                            VSWorkSDK.EngineTestManager.Instance.avatarManager.SetAvatarStopDestroy(item);
                        }
                    }
                }
            }


            if (!mStaticThings.AllStaticAvatarsDic.ContainsKey(mStaticThings.I.mAvatarID))
            {
                mStaticThings.AllStaticAvatarsDic.Add(mStaticThings.I.mAvatarID, VSWorkSDK.EngineTestManager.Instance.avatarManager.GetCurrentAvatarFrame());
                mStaticThings.AllStaticAvatarList.Add(mStaticThings.I.mAvatarID);
                if (mStaticThings.I.SendAvatar)
                {
                    mStaticThings.AllActiveAvatarList.Add(mStaticThings.I.mAvatarID);
                }
            }

            //Debug.LogWarning("Now Scene :" + cavatars.nowscene.scene);
            // Debug.LogWarning(JsonUtility.ToJson(cavatars));

            bool hasLinkedScene = false;
            // bool isnewscene = false;

            if (mStaticThings.I.nowRoomLinkScene != null && !string.IsNullOrEmpty(mStaticThings.I.nowRoomLinkScene.scene))
            {
                if (mStaticThings.I.nowRoomLinkScene.scene != mStaticThings.I.mScene.scene)
                {
                    isfirstopen = false;
                    VSWorkSDK.EngineTestManager.Instance.sceneManager.LoadNextScene(mStaticThings.I.nowRoomLinkScene, true);
                    hasLinkedScene = true;
                    if (cavatars.sceneavatars != null && cavatars.sceneavatars.Count > 0)
                    {
                        MessageDispatcher.SendMessage(this, WsMessageType.SendLoadScene.ToString(), mStaticThings.I.nowRoomLinkScene, 0);
                    }
                }
            }
            else if (cavatars.sceneavatars != null && cavatars.sceneavatars.Count > 0)
            {
                if (cavatars.nowscene != null && cavatars.nowscene.scene != null && cavatars.nowscene.scene != "")
                {
                    if (cavatars.nowscene.scene != mStaticThings.I.mScene.scene || (cavatars.nowscene.scene == mStaticThings.I.mScene.scene && cavatars.nowscene.version != mStaticThings.I.mScene.version))
                    {
                        if (!cavatars.nowscene.iskod)
                        {
                            isfirstopen = false;
                            VSWorkSDK.EngineTestManager.Instance.sceneManager.LoadNextScene(cavatars.nowscene, true);
                            //Debug.LogWarning(JsonUtility.ToJson(cavatars.nowscene));
                        }
                        else if (cavatars.nowscene.scene.Substring(1) != mStaticThings.I.mScene.scene.Substring(1) || (cavatars.nowscene.scene.Substring(1) == mStaticThings.I.mScene.scene.Substring(1) && (cavatars.nowscene.kod.mtime + "-" + cavatars.nowscene.kod.size) != (mStaticThings.I.mScene.kod.mtime + "-" + mStaticThings.I.mScene.kod.size)))
                        {
                            string preurl;
                            if (cavatars.nowscene.kod.roomurl == mStaticThings.I.nowRoomServerUrl || !mStaticThings.I.localroomserver)
                            {
                                preurl = cavatars.nowscene.kod.preurl;
                            }
                            else
                            {
                                preurl = mStaticThings.I.ThisKODfileUrl;
                            }
                            cavatars.nowscene.kod.url = preurl + cavatars.nowscene.kod.url;
                            isfirstopen = false;
                            mStaticThings.I.IsSelfJoinScene = true;
                            MessageDispatcher.SendMessage(this, VrDispMessageType.KODGetOneScene.ToString(), cavatars.nowscene.kod, 0);
                        }
                    }
                    else if (mStaticThings.I.nowRoomLinkScene != null && mStaticThings.I.nowRoomLinkScene.id != "" && mStaticThings.I.nowRoomLinkScene.id != null && (cavatars.nowscene.scene == "" || cavatars.nowscene.scene == null) && mStaticThings.I.nowRoomChID == mStaticThings.I.nowRoomStartChID)
                    {
                        if (mStaticThings.I.nowRoomLinkScene.scene != mStaticThings.I.mScene.scene || (mStaticThings.I.nowRoomLinkScene.scene == mStaticThings.I.mScene.scene && mStaticThings.I.nowRoomLinkScene.version != mStaticThings.I.mScene.version))
                        {
                            isfirstopen = false;
                            VSWorkSDK.EngineTestManager.Instance.sceneManager.LoadNextScene(mStaticThings.I.nowRoomLinkScene, true);
                            hasLinkedScene = true;
                        }
                    }
                }
                else if (cavatars.sceneavatars[0].scene != null && cavatars.sceneavatars[0].scene.scene != null && cavatars.sceneavatars[0].scene.scene != "")
                {
                    if (cavatars.sceneavatars[0].scene.scene != mStaticThings.I.mScene.scene || (cavatars.sceneavatars[0].scene.scene == mStaticThings.I.mScene.scene && cavatars.sceneavatars[0].scene.version != mStaticThings.I.mScene.version))
                    {
                        // Debug.LogWarning(cavatars.sceneavatars[0].scene.kod);

                        // Debug.LogWarning(cavatars.sceneavatars[0].scene.kod.name);
                        if (!cavatars.sceneavatars[0].scene.iskod)
                        {
                            isfirstopen = false;
                            VSWorkSDK.EngineTestManager.Instance.sceneManager.LoadNextScene(cavatars.sceneavatars[0].scene, true);
                            //isnewscene = true;
                        }
                        else if (cavatars.sceneavatars[0].scene.kod.name != "" && cavatars.sceneavatars[0].scene.kod.name != null)
                        {
                            string preurl;
                            if (cavatars.sceneavatars[0].scene.kod.roomurl == mStaticThings.I.nowRoomServerUrl || !mStaticThings.I.localroomserver)
                            {
                                preurl = cavatars.sceneavatars[0].scene.kod.preurl;
                            }
                            else
                            {
                                preurl = mStaticThings.I.ThisKODfileUrl;
                            }
                            isfirstopen = false;
                            cavatars.sceneavatars[0].scene.kod.url = preurl + cavatars.sceneavatars[0].scene.kod.url;
                            mStaticThings.I.IsSelfJoinScene = true;
                            MessageDispatcher.SendMessage(this, VrDispMessageType.KODGetOneScene.ToString(), cavatars.sceneavatars[0].scene.kod, 0);
                        }
                    }
                    else if (mStaticThings.I.nowRoomLinkScene != null && (cavatars.sceneavatars[0].scene.scene == "" || cavatars.sceneavatars[0].scene.scene == null) && mStaticThings.I.nowRoomChID == mStaticThings.I.nowRoomStartChID)
                    {
                        if (mStaticThings.I.nowRoomLinkScene.scene != mStaticThings.I.mScene.scene)
                        {
                            isfirstopen = false;
                            VSWorkSDK.EngineTestManager.Instance.sceneManager.LoadNextScene(mStaticThings.I.nowRoomLinkScene, true);
                            hasLinkedScene = true;
                        }
                    }
                }
            }
            else
            {
                //Debug.LogWarning(JsonUtility.ToJson(cavatars));
                //Debug.LogWarning(JsonUtility.ToJson(mStaticThings.I.nowRoomLinkScene));
                if (mStaticThings.I.nowRoomLinkScene != null && mStaticThings.I.nowRoomLinkScene.id != "" && mStaticThings.I.nowRoomLinkScene.id != null)
                {
                    if (mStaticThings.I.nowRoomLinkScene.scene != mStaticThings.I.mScene.scene)
                    {
                        isfirstopen = false;
                        mStaticThings.I.IsSelfJoinScene = false;

                        //GameManager.Instance.loadAssetsController.DownloadFromUrlIDScene("", mStaticThings.I.nowRoomLinkScene.id, true, false);
                        URLIDSceneInfo uRLIDScene = new URLIDSceneInfo
                        {
                            server = "",
                            id = mStaticThings.I.nowRoomLinkScene.id,
                            isnowserver = true,
                            update = false
                        };

                        MessageDispatcher.SendMessage(this, WsMessageType.SendLoadScene.ToString(), mStaticThings.I.nowRoomLinkScene, 0);
                        hasLinkedScene = true;
                    }
                }
                else if (mStaticThings.I.nowRoomLinkScene == null && mStaticThings.I.nowRoomLinkScene.id == "")
                {
                    URLIDSceneInfo uRLIDScene = new URLIDSceneInfo
                    {
                        server = "",
                        id = mStaticThings.I.mScene.scene,
                        isnowserver = true,
                        update = false
                    };
                    MessageDispatcher.SendMessage(this, WsMessageType.SendLoadScene.ToString(), mStaticThings.I.nowRoomLinkScene, 0);
                    hasLinkedScene = true;
                }
                else
                {
                    if (SceneManager.GetActiveScene().name == "Loading")
                    {
                        VSWorkSDK.EngineTestManager.Instance.sceneManager.LoadDefaultStartScene();
                        WsSceneInfo newscene = new WsSceneInfo
                        {
                            id = mStaticThings.I.mAvatarID,
                            scene = "Start",
                            name = "Start",
                            version = "1",
                            isremote = false,
                            isupdate = false,
                            iskod = false
                        };
                        MessageDispatcher.SendMessage(this, WsMessageType.SendLoadScene.ToString(), newscene, 0);
                    }
                    else
                    {

                    }

                }
            }

            MessageDispatcher.SendMessage(this, VrDispMessageType.VRDoRecieveConnection.ToString(), cavatars, 0);
            //GameManager.Instance.socketIO.SetConnectReady();
            //GameManager.Instance.socketIO.SendNewAvatar(GameDataManager.LoadAssetsData.LoadFromeSDK, hasLinkedScene);
            //TwoParamMsg.AutoAllocate((int)MsgEnum_SocketIO.SendNewAvatar, GameDataManager.LoadAssetsData.LoadFromeSDK, hasLinkedScene);
        }

        public void OnMediaInfoMsgReturn(WsMediaFrame newMediaFrame)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveMedia.ToString(), newMediaFrame, 0);
        }

        public void OnBigSceneInfoMsgReturn(WsBigScreen newBigScreen)
        {
            if (newBigScreen.id.Length > 1)
            {
                MessageDispatcher.SendMessage(this, WsMessageType.RecieveBigScreen.ToString(), newBigScreen, 0);
            }
        }

        public void OnCameraScreenInfoMsgReturn(CameraScreenInfo newcam)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecievePCCamera.ToString(), newcam, 0);
        }

        public void OnProgressInfoMsgReturn(WsProgressInfo newprog)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveProgress.ToString(), newprog, 0);
        }

        //public void OnCheckDelayMsgReturn(S_CHECK_DELAY msg)
        //{
        //    //GameManager.Instance.socketIO.mRecieveCheckDelay();
        //    SingleMsg.AutoAllocate((int)MsgEnum_SocketIO.mRecieveCheckDelay);
        //}

        public void OnGetDataMsgReturn(Dictionary<string, string> roomdatadic)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveGetData.ToString(), roomdatadic, 0);
        }

        public void OnChangeRoomMsgReturn(VRChanelRoom chinfo)
        {
            mStaticThings.I.nowRoomChID = chinfo.roomid;
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveChangeRoom.ToString(), chinfo, 0);
        }

        public void OnCanvasViewMsgReturn(CanvasWebviewActionFrame cwaf)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveCanvasView.ToString(), cwaf, 0);
        }

        public void OnVoiceFrameMsgReturn(WsVoiceFrame wf)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveVRVoice.ToString(), wf, 0);
        }

        public void OnPicFrameMsgReturn(WsPicFrame wf)
        {
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveVRPic.ToString(), wf, 0);
        }

        public void OnCheckAvatarMsgReturn(WsAvatarFrameList avatarframes)
        {
            mStaticThings.I.nowAvatarFrameList = avatarframes;

            List<string> DeletetempDynList = new List<string>();
            List<string> nowwslist = new List<string>();
            foreach (var item in avatarframes.alist)
            {
                nowwslist.Add(item.id);
            }
            foreach (var item in mStaticThings.DynClientAvatarsDic)
            {
                if (!nowwslist.Contains(item.Value.id))
                {
                    DeletetempDynList.Add(item.Value.id);
                }
            }

            mStaticThings.AllStaticAvatarsDic.Clear();
            mStaticThings.AllStaticAvatarList.Clear();
            mStaticThings.AllActiveAvatarList.Clear();
            EngineTestManager.Instance.CheckAvatarList.Clear();
            // Debug.LogWarning(avatarframes.alist.Count);
            //Debug.LogWarning(JsonUtility.ToJson(avatarframes));

            foreach (var item in avatarframes.alist)
            {
                EngineTestManager.Instance.CheckAvatarList.Add(item.id);
                if (!mStaticThings.I.ActionBlackList.Contains(item.id))
                {
                    if (!mStaticThings.DynClientAvatarsDic.ContainsKey(item.id))
                    {
                        mStaticThings.DynClientAvatarsDic.Add(item.id, item.ToJian());
                    }

                    if (!mStaticThings.AllStaticAvatarsDic.ContainsKey(item.id))
                    {
                        mStaticThings.AllStaticAvatarsDic.Add(item.id, item);
                    }
                    if (!mStaticThings.AllStaticAvatarList.Contains(item.id))
                    {
                        mStaticThings.AllStaticAvatarList.Add(item.id);
                    }


                    if (item.ae)
                    {
                        mStaticThings.AllActiveAvatarList.Add(item.id);
                        if (item.id != mStaticThings.I.mAvatarID)
                        {
                            if (!VSWorkSDK.EngineTestManager.Instance.avatarManager.AllTestAvatar.ContainsKey(item.id))
                            {
                                //Debug.LogWarning("Creat WS Avatar: " + item.id);
                                MessageDispatcher.SendMessage(true, VrDispMessageType.InitWsAvatar.ToString(), item, 0);
                            }
                            else
                            {
                                VSWorkSDK.EngineTestManager.Instance.avatarManager.SetAvatarStopDestroy(item);
                            }
                        }
                        else
                        {
                            mStaticThings.I.SendAvatar = true;
                        }
                    }
                    else
                    {
                        if (VSWorkSDK.EngineTestManager.Instance.avatarManager.AllTestAvatar.ContainsKey(item.id))
                        {
                            MessageDispatcher.SendMessage(true, VrDispMessageType.DestroyWsAvatar.ToString(), item.id, 0);
                        }

                        if (item.id != mStaticThings.I.mAvatarID)
                        {
                        }
                        else
                        {
                            mStaticThings.I.SendAvatar = false;
                        }
                    }
                }
                else
                {
                    if (VSWorkSDK.EngineTestManager.Instance.avatarManager.AllTestAvatar.ContainsKey(item.id))
                    {
                        MessageDispatcher.SendMessage(false, VrDispMessageType.DestroyWsAvatar.ToString(), item.id, 0);
                    }
                    if (mStaticThings.DynClientAvatarsDic.ContainsKey(item.id))
                    {
                        mStaticThings.DynClientAvatarsDic.Remove(item.id);
                    }
                }
            }

            if (!EngineTestManager.Instance.CheckAvatarList.Contains(mStaticThings.I.mAvatarID))
            {
                mStaticThings.I.WsAvatarIsReady = false;
                EngineTestManager.Instance.standaloneNetwork.SendNewAvatar(false);
                Debug.Log("AllStaticAvatarList no mAvatarID ,ReSend");
            }
            else
            {
                mStaticThings.I.WsAvatarIsReady = true;
            }

            foreach (var item in DeletetempDynList)
            {
                MessageDispatcher.SendMessage(false, VrDispMessageType.DestroyWsAvatar.ToString(), item, 0);
                if (mStaticThings.DynClientAvatarsDic.ContainsKey(item))
                {
                    mStaticThings.DynClientAvatarsDic.Remove(item);
                }
            }

            MessageDispatcher.SendMessage(this, VrDispMessageType.VRDoRecieveCheckAvatarlist.ToString(), avatarframes, 0);

            //刷新人员列表
            //SingleMsg.AutoAllocate((int)MsgEnum_UnderMenu.RefuseUserList);


            //Debug.LogWarning("NowServer Scene：" + avatarframes.nowscene.scene);

            if ((mStaticThings.I.nowRoomLinkScene != null && mStaticThings.I.nowRoomLinkScene.scene != "") || mStaticThings.I.nowRoomLinkScene.scene != "" || avatarframes.nowscene == null || avatarframes.nowscene.scene == null || avatarframes.nowscene.scene == "")
            {
                return;
            }
            //Debug.LogWarning("RRRRRRRRRRR  " + JsonUtility.ToJson(avatarframes.nowscene));

            if (!avatarframes.nowscene.iskod)
            {
                if (avatarframes.nowscene.scene != mStaticThings.I.mScene.scene)
                {
                    VSWorkSDK.EngineTestManager.Instance.sceneManager.LoadNextScene(avatarframes.nowscene, true);
                }
            }
            else
            {
                if (avatarframes.nowscene.scene.Substring(1) != mStaticThings.I.mScene.scene.Substring(1) || (avatarframes.nowscene.scene.Substring(1) == mStaticThings.I.mScene.scene.Substring(1) && (avatarframes.nowscene.kod.mtime + "-" + avatarframes.nowscene.kod.size) != (mStaticThings.I.mScene.kod.mtime + "-" + mStaticThings.I.mScene.kod.size)))
                {
                    //Debug.LogWarning(JsonUtility.ToJson(avatarframes.nowscene));

                    string preurl;
                    if (avatarframes.nowscene.kod.roomurl == mStaticThings.I.nowRoomServerUrl || !mStaticThings.I.localroomserver)
                    {
                        preurl = avatarframes.nowscene.kod.preurl;
                    }
                    else
                    {
                        preurl = mStaticThings.I.ThisKODfileUrl;
                    }
                    avatarframes.nowscene.kod.url = preurl + avatarframes.nowscene.kod.url;

                    mStaticThings.I.IsSelfJoinScene = true;
                    MessageDispatcher.SendMessage(this, VrDispMessageType.KODGetOneScene.ToString(), avatarframes.nowscene.kod, 0);
                }

            }
        }

        public void OnSynAvatarListMsgReturn(WsAvatarFrameJianList CurList)
        {
            if (CurList.jlist.Count > 0)
            {
                foreach (var CurWsAvatarFrame in CurList.jlist)
                {
                    VSWorkSDK.EngineTestManager.Instance.avatarManager.RecieveWsAvatar(CurWsAvatarFrame);

                    if (!mStaticThings.DynClientAvatarsDic.ContainsKey(CurWsAvatarFrame.id))
                    {
                        mStaticThings.DynClientAvatarsDic.Add(CurWsAvatarFrame.id, CurWsAvatarFrame);
                    }
                    mStaticThings.DynClientAvatarsDic[CurWsAvatarFrame.id] = CurWsAvatarFrame;

                    MessageDispatcher.SendMessage(this, "RecieveVRAvatarFrame", CurWsAvatarFrame, 0);
                }
            }
        }

        public void OnAvatarDisconnectMsgReturn(string Id)
        {
            //一个人离线了
            MessageDispatcher.SendMessage(this, WsMessageType.RecieveDelAvatar.ToString(), Id, 0);

            if (Id == mStaticThings.I.mAvatarID) return;
            //刷新人员列表
            //SingleMsg.AutoAllocate((int)MsgEnum_UnderMenu.RefuseUserList);
        }

        public void InitMyData(string jsonInfo)
        {
            CloudRenderLoginData cloudRenderLoginData = Newtonsoft.Json.JsonConvert.DeserializeObject<CloudRenderLoginData>(jsonInfo);
            SetGroup(cloudRenderLoginData.group);
            //user
            string roomstring = cloudRenderLoginData.roomInfo.vr_rooms_socketioserver;

            if (roomstring.Contains("@"))
            {
                mStaticThings.I.localroomserver = true;
                string[] roomarr = roomstring.Split('@');

                if (roomstring.StartsWith("ws://"))
                {
                    mStaticThings.I.nowRoomServerUrl = roomarr[0] + mStaticThings.I.now_ServerURL + roomarr[1];
                    mStaticThings.I.nowRoomServerGetUrl = "http://" + mStaticThings.I.now_ServerURL + roomarr[1];
                }
                else if (roomstring.StartsWith("wss://"))
                {
                    mStaticThings.I.nowRoomServerUrl = roomarr[0] + mStaticThings.I.now_ServerURL + roomarr[1];
                    mStaticThings.I.nowRoomServerGetUrl = "https://" + mStaticThings.I.now_ServerURL + roomarr[1];
                }
                else
                {
                    mStaticThings.I.nowRoomServerUrl = "http://" + roomarr[0] + mStaticThings.I.now_ServerURL + roomarr[1];
                    mStaticThings.I.nowRoomServerGetUrl = "http://" + mStaticThings.I.now_ServerURL + roomarr[1];
                }
            }
            else if (roomstring.Contains("127.0.0.1"))
            {
                mStaticThings.I.localroomserver = true;
                mStaticThings.I.nowRoomServerGetUrl = roomstring;
                mStaticThings.I.nowRoomServerUrl = roomstring;
            }
            else
            {
                mStaticThings.I.localroomserver = false;
                if (roomstring.StartsWith("ws://"))
                {
                    mStaticThings.I.nowRoomServerGetUrl = "http://" + roomstring.Substring(5, roomstring.Length - 5);
                    mStaticThings.I.nowRoomServerUrl = roomstring;
                }
                else if (roomstring.StartsWith("wss://"))
                {
                    mStaticThings.I.nowRoomServerGetUrl = "https://" + roomstring.Substring(6, roomstring.Length - 6);
                    mStaticThings.I.nowRoomServerUrl = roomstring;
                }
                else
                {
                    mStaticThings.I.nowRoomServerGetUrl = "http://" + roomstring;
                    mStaticThings.I.nowRoomServerUrl = "ws://" + roomstring;
                }
            }

            //GameManager.Instance.userLoginController.now_phone = cloudRenderLoginData.username;
            mStaticThings.apikey = VRUtils.GetMD5(cloudRenderLoginData.username);

            mStaticThings.I.mNickName = cloudRenderLoginData.userInfo.vr_user_nickname;
            mStaticThings.I.mAvatarID = cloudRenderLoginData.userInfo.vr_user_id;
            mStaticThings.I.msex = cloudRenderLoginData.userInfo.vr_user_sex;
            //mStaticThings.userdata = cloudRenderLoginData.userInfo;
            mStaticThings.apitoken = cloudRenderLoginData.apitoken;
            mStaticThings.urltokenfix = "&apikey=" + mStaticThings.apikey + "&apitoken=" + mStaticThings.apitoken + "&version=" + "1.9.8" + "&appname=" + "vsvr";

            //room
            mStaticThings.I.nowRoomVoiceType = cloudRenderLoginData.roomInfo.vr_rooms_vtype.ToString();
            mStaticThings.I.nowRoomGMEappID = cloudRenderLoginData.roomInfo.vr_rooms_gmeappid;
            mStaticThings.I.nowRoomGMEappKey = cloudRenderLoginData.roomInfo.vr_rooms_gmeappkey;
            mStaticThings.I.nowRoomGMEroomID = cloudRenderLoginData.roomInfo.vr_rooms_gmeroomid;
            mStaticThings.I.nowRoomGMETxtEnabled = cloudRenderLoginData.roomInfo.vr_rooms_gmetxtenabled == 1;
            mStaticThings.I.nowRoomVoiceAPI = cloudRenderLoginData.roomInfo.vr_rooms_voiceapi;
            mStaticThings.I.nowRoomEnable3dSound = cloudRenderLoginData.roomInfo.vr_rooms_en3d;
            mStaticThings.I.nowRoomGMEroomTeamID = cloudRenderLoginData.roomInfo.vr_rooms_firstteam;

            mStaticThings.I.nowRoomActionAPI = cloudRenderLoginData.roomInfo.vr_rooms_actionapi;
            mStaticThings.I.nowRoomTBPAPI = cloudRenderLoginData.roomInfo.vr_rooms_tbpapi;
            mStaticThings.I.nowRoomAdminCMD = cloudRenderLoginData.roomInfo.vr_rooms_admincmd;
            mStaticThings.I.nowRoomExEnabled = cloudRenderLoginData.roomInfo.vr_rooms_exenable.ToString();
            mStaticThings.I.nowRoomPass = cloudRenderLoginData.roomInfo.vr_rooms_pass;
            mStaticThings.I.nowRoomID = cloudRenderLoginData.roomInfo.vr_rooms_id;
            mStaticThings.I.nowRoomMaxCount = cloudRenderLoginData.roomInfo.vr_rooms_max;
            mStaticThings.I.nowRoomAeMaxCount = cloudRenderLoginData.roomInfo.vr_rooms_aemax;
            mStaticThings.I.nowRoomChID = cloudRenderLoginData.roomInfo.vr_rooms_firstroom;
            mStaticThings.I.nowRoomStartChID = cloudRenderLoginData.roomInfo.vr_rooms_firstroom;
            mStaticThings.I.nowRoomVoiceRange = cloudRenderLoginData.roomInfo.vr_rooms_vrange;
            mStaticThings.I.nowRoomMediaEnabled = cloudRenderLoginData.roomInfo.vr_rooms_mediaen == 1;
            mStaticThings.I.nowRoomSettings = JsonMapper.ToObject<Dictionary<string, string>>(cloudRenderLoginData.roomInfo.vr_rooms_settings);//JsonConvert.DeserializeObject<Dictionary<string,string>>(jsd["vr_rooms_settings"].ToString());

            mStaticThings.I.nowRoomVoiceUpEnabled = cloudRenderLoginData.roomInfo.vr_rooms_ven == 1;
            mStaticThings.I.MaxHideDistance = cloudRenderLoginData.roomInfo.vr_rooms_maxhdis;
            mStaticThings.I.MaxAvatarMeshHideDistance = cloudRenderLoginData.roomInfo.vr_rooms_adis;
            mStaticThings.I.now_groupid = cloudRenderLoginData.avatar.group_id;

            //avatar
            mStaticThings.I.aid = cloudRenderLoginData.avatar.vr_avatars_id;
            mStaticThings.I.startaid = cloudRenderLoginData.avatar.vr_avatars_id;
            PlayerPrefs.SetString("UserStartAid" + mStaticThings.I.mAvatarID, cloudRenderLoginData.avatar.vr_avatars_id);

            if (cloudRenderLoginData.roomInfo.vr_scenes_id != "0" && cloudRenderLoginData.roomInfo.vr_scenes_id != "")
            {

                WsSceneInfo nowroomlinkscene = new WsSceneInfo()
                {
                    iskod = false,
                    isremote = true,
                    isupdate = false,
                    scene = cloudRenderLoginData.roomInfo.vr_scenes_id,
                    id = cloudRenderLoginData.roomInfo.vr_scenes_id
                };
                mStaticThings.I.nowRoomLinkScene = nowroomlinkscene;

                VRRootChanelRoom newroom = new VRRootChanelRoom
                {
                    roomid = mStaticThings.I.nowRoomID,
                    voiceid = mStaticThings.I.nowRoomGMEroomID
                };
                mStaticThings.I.LastIDLinkChanelRoomList.Add(newroom);

                if (roomstring.Contains("127.0.0.1"))
                {
                    if (mStaticThings.I.nowRoomLinkScene.scene != mStaticThings.I.mScene.scene || (mStaticThings.I.nowRoomLinkScene.scene == mStaticThings.I.mScene.scene && mStaticThings.I.nowRoomLinkScene.version != mStaticThings.I.mScene.version))
                    {
                        EngineTestManager.Instance.sceneManager.LoadNextScene(mStaticThings.I.nowRoomLinkScene, true);
                    }
                }
            }
            else
            {
                mStaticThings.I.nowRoomLinkScene.id = "";
                mStaticThings.I.nowRoomLinkScene.scene = "";
                mStaticThings.I.nowRoomLinkScene.name = "";
            }

            mStaticThings.I.SendAvatar = true;
        }

        private void SetGroup(CloudRenderGroup jsd)
        {
            if (jsd == null) return;
            string fileserver = jsd.vr_group_fileserver;
            string fileurl = jsd.vr_group_fileurl;
            string filekind = jsd.vr_group_filekind.ToString();
            string koden = jsd.user_group_enabled.ToString();
            mStaticThings.I.GrouplinkedroomID = jsd.vr_group_linkroomid;

            if (fileserver.Contains("@"))
            {
                string[] fileserverarr = fileserver.Split('@');
                fileserver = fileserverarr[0] + mStaticThings.I.now_ServerURL + fileserverarr[1];
            }
            if (fileurl.Contains("@"))
            {
                string[] fileurlarr = fileurl.Split('@');
                fileurl = fileurlarr[0] + mStaticThings.I.now_ServerURL + fileurlarr[1];
            }

            //nowfileserver = fileserver;
            //nowfielurl = fileurl;
            //nowfilekind = filekind;
            ////nowkodname = jsd["user_group_name"].ToString();
            //nowkodname = jsd.user_group_name;
            ////nowkodpass = jsd["user_group_pass"].ToString();
            //nowkodpass = jsd.user_group_pass;

            mStaticThings.I.enhide = false;

            //VRSelectAvatarController.I.SetEnhideEnable(mStaticThings.I.enhide);
            //getRoomSelectController.selectAvatarController.SetEnhideEnable(mStaticThings.I.enhide);

            //获取组织的场景
            //mStaticThings.I.now_groupid = jsd["vr_group_id"].ToString();
            mStaticThings.I.now_groupid = jsd.vr_group_id;
        }
    }
}