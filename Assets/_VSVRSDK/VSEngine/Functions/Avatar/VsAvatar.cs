using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSEngine
{
    public class VsAvatar : BaseFunction
    {
        private string SelectId;
        /// <summary>
        /// 选择某个角色（点击角色铭牌）事件
        /// </summary>
        public static event Action<string> SelectAvatarWsidEvent;
        /// <summary>
        /// 接收位置变化消息事件
        /// </summary>
        public static event System.Action<WsPlaceMarkList> RecievePlaceMarkEvent;
        /// <summary>
        /// 自己位置变化事件（参数：位置点名称）
        /// </summary>
        public static event System.Action<string> SelfPlaceToEvent;
        /// <summary>
        /// 所有人位置变化事件（参数1：位置组名称，参数2：是否所有人）
        /// </summary>
        public static event System.Action<string,bool> AllPlaceToEvent;
        private static Queue<System.Action<string, bool>> AllPlaceToEventQueue = new Queue<Action<string, bool>>();
        /// <summary>
        /// 摇杆移动事件（参数：点击摇杆移动或取消）
        /// </summary>
        public static event Action<bool> TeleporterStatusChangeEvent;
        /// <summary>
        /// 新人进入频道
        /// </summary>
        public static event System.Action<WsAvatarFrame> RecieveNewAvatarEvent;
        /// <summary>
        /// 有人退出频道(参数：Sys avatarID)
        /// </summary>
        public static event System.Action<string> RecieveDelAvatarEvent;
        public VsAvatar() : base(FunctionType.VsAvatar)
        {

        }
        internal override void Awake()
        {
            base.Awake();

            MessageDispatcher.AddListener(WsMessageType.RecievePlaceMark.ToString(), RecievePlaceMark, true);
            MessageDispatcher.AddListener(WsMessageType.RecieveNewAvartar.ToString(), RecieveNewAvatar, true);
            MessageDispatcher.AddListener(WsMessageType.RecieveDelAvatar.ToString(), RecieveDelAvatar, true);

            MessageDispatcher.AddListener(VrDispMessageType.SelectAvatarWsid.ToString(), SelectAvatarWsid, true);
            MessageDispatcher.AddListener(VrDispMessageType.SelfPlaceTo.ToString(), SelfPlaceTo, true);
            MessageDispatcher.AddListener(VrDispMessageType.AllPlaceTo.ToString(), AllPlaceTo, true);
            MessageDispatcher.AddListener(VrDispMessageType.TeleporterStatusChange.ToString(), TeleporterStatusChange, true);
        }
        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();

            MessageDispatcher.RemoveListener(WsMessageType.RecievePlaceMark.ToString(), RecievePlaceMark, true);
            MessageDispatcher.RemoveListener(WsMessageType.RecieveNewAvartar.ToString(), RecieveNewAvatar, true);
            MessageDispatcher.RemoveListener(WsMessageType.RecieveDelAvatar.ToString(), RecieveDelAvatar, true);

            MessageDispatcher.RemoveListener(VrDispMessageType.SelectAvatarWsid.ToString(), SelectAvatarWsid, true);
            MessageDispatcher.RemoveListener(VrDispMessageType.SelfPlaceTo.ToString(), SelfPlaceTo, true);
            MessageDispatcher.RemoveListener(VrDispMessageType.AllPlaceTo.ToString(), AllPlaceTo, true);
            MessageDispatcher.RemoveListener(VrDispMessageType.TeleporterStatusChange.ToString(), TeleporterStatusChange, true);

            try
            {
                SelectAvatarWsidEvent = null;
                RecievePlaceMarkEvent = null;
                SelfPlaceToEvent = null;
                AllPlaceToEvent = null;
                AllPlaceToEventQueue.Clear();
                TeleporterStatusChangeEvent = null;
                RecieveNewAvatarEvent = null;
                RecieveDelAvatarEvent = null;
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

        public override string ToString()
        {
            return base.ToString();
        }

        internal override void Update()
        {
            base.Update();
        }

        #region Event

        private void SelectAvatarWsid(IMessage msg)
        {
            string selectid = (string)msg.Data;
            SelectId = selectid;

            try
            {
                if (SelectAvatarWsidEvent != null)
                    SelectAvatarWsidEvent(SelectId);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        private void RecievePlaceMark(IMessage msg)
        {
            WsPlaceMarkList newwpmlist = (WsPlaceMarkList)msg.Data;

            try
            {
                if (RecievePlaceMarkEvent != null)
                    RecievePlaceMarkEvent(newwpmlist);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        private void RecieveNewAvatar(IMessage msg)
        {
            WsAvatarFrame NewAvatarFrame = (WsAvatarFrame)msg.Data;

            try
            {
                if (RecieveNewAvatarEvent != null)
                    RecieveNewAvatarEvent(NewAvatarFrame);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        private void RecieveDelAvatar(IMessage msg)
        {
            string sysAvatarId = msg.Data as string;

            try
            {
                if (RecieveDelAvatarEvent != null)
                    RecieveDelAvatarEvent(sysAvatarId);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void SelfPlaceTo(IMessage msg)
        {
            string dname = (string)msg.Data;

            try
            {
                if (SelfPlaceToEvent != null)
                    SelfPlaceToEvent(dname);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void AllPlaceTo(IMessage msg)
        {
            string gname = (string)msg.Data;
            bool controlall = (bool)msg.Sender;

            try
            {
                if (AllPlaceToEvent != null)
                    AllPlaceToEvent(gname, controlall);

                var AllPlaceToEventInfo = AllPlaceToEventQueue.Dequeue();

                if (AllPlaceToEventInfo != null)
                    AllPlaceToEventInfo(gname, controlall);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        void TeleporterStatusChange(IMessage msg)
        {
            bool ispointon = (bool)msg.Data;

            try
            {
                if (TeleporterStatusChangeEvent != null)
                    TeleporterStatusChangeEvent(ispointon);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        #endregion

        #region API
        /// <summary>
        /// 切换位置组
        /// </summary>
        /// <param name="ToAll">是否发送给所有人</param>
        /// <param name="GroupName">位置组</param>
        public static void SendWsAllPlayceTo(bool ToAll, string GroupName)
        {
            MessageDispatcher.SendMessage(ToAll, VrDispMessageType.AllPlaceTo.ToString(), GroupName, 0);
        }
        /// <summary>
        /// 获取当前Avatar的排序（第一个为Admin）
        /// </summary>
        /// <returns></returns>
        public static int GetAvatarSort()
        {
            int sort;

            if (mStaticThings.I == null)
            {
                sort = 0;
            }
            else
            {
                sort = mStaticThings.I.GetSortNumber(mStaticThings.I.mAvatarID);
            }

            return sort;
        }
        /// <summary>
        /// 获取所有Avatar ID 列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllAvatars()
        {
            return mStaticThings.I.GetAllStaticAvatarList();
        }
        /// <summary>
        /// 获取所有Avatar的昵称列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllAvatarNames()
        {
            return mStaticThings.I.GetAllStaticAvatarsDicNames();
        }
        /// <summary>
        /// 获取当前显示的Avatar ID 列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetActiveAvatars()
        {
            return mStaticThings.I.GetAllActiveAvatarList();
        }
        /// <summary>
        /// 获取当前显示的Avatar的昵称列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetActiveNickNames()
        {
            return mStaticThings.I.GetAllActiveAvatarsDicNames();
        }
        /// <summary>
        /// 设置vr人物显示信息
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="name"></param>
        /// <param name="hideavatar"></param>
        /// <param name="SetColor"></param>
        /// <param name="pencolor"></param>
        public static void SetVRGameObject(string aid, string name, bool hideavatar, bool SetColor, Color pencolor)
        {
            if (mStaticThings.I == null)
            {
                return;
            }
            if (aid != "" && aid != null)
            {
                mStaticThings.I.aid = aid;
            }
            if (name != "" && name != null)
            {
                mStaticThings.I.mNickName = name;
            }
            if (SetColor)
            {
                mStaticThings.I.nowpencolor = "#" + ColorUtility.ToHtmlStringRGB(pencolor);
            }
            mStaticThings.I.SendAvatar = !hideavatar;
        }
        /// <summary>
        /// 切换Avatar
        /// </summary>
        public static void SendChangeAvatar()
        {
            MessageDispatcher.SendMessage(WsMessageType.SendChangeAvatar.ToString());
        }
        /// <summary>
        /// 切换位置组
        /// </summary>
        /// <param name="ToAll"></param>
        /// <param name="GroupName"></param>
        /// <param name="AllPlaceToAc"></param>
        public static void SendWsAllPlayceTo(bool ToAll, string GroupName, System.Action<string,bool> AllPlaceToAc)
        {
            if(AllPlaceToAc != null)
                AllPlaceToEventQueue.Enqueue(AllPlaceToAc);

            MessageDispatcher.SendMessage(ToAll, VrDispMessageType.AllPlaceTo.ToString(), GroupName, 0);
        }
        /// <summary>
        /// 切换角色头顶铭牌的显隐
        /// </summary>
        /// <param name="state">mStaticThings.I.shownamepanel为当前状态</param>
        public static void ShowNamePanel(bool state)
        {
            MessageDispatcher.SendMessageData(VrDispMessageType.ShowNamePanel.ToString(), state, 0);
        }
        /// <summary>
        /// 切换角色头顶喇叭的显隐
        /// </summary>
        /// <param name="state">mStaticThings.I.showvol为当前状态</param>
        public static void ShowVolPanel(bool state)
        {
            MessageDispatcher.SendMessageData(VrDispMessageType.ShowVolPanel.ToString(), state, 0);
        }
        #endregion
    }
}