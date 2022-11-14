using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSEngine
{
    public class VsChanel : BaseFunction
    {        
        /// <summary>
        /// 房间是否成功连接
        /// </summary>
        public static bool IsRoomConnectReady
        {
            get
            {
                if (mStaticThings.I == null) return false;
                return mStaticThings.I.WsAvatarIsReady;
            }
        }

        /// <summary>
        /// 房间连接事件
        /// </summary>
        public static event Action SetRoomConnectedEvent;
        /// <summary>
        /// 房间断开连接事件
        /// </summary>
        public static event Action SetRoomDisConnectedEvent;
        public VsChanel() : base(FunctionType.VsChanel)
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

            MessageDispatcher.AddListener(VrDispMessageType.RoomConnected.ToString(), RoomConnected);
            MessageDispatcher.AddListener(VrDispMessageType.RoomDisConnected.ToString(), RoomDisConnected);
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();

            MessageDispatcher.RemoveListener(VrDispMessageType.RoomConnected.ToString(), RoomConnected);
            MessageDispatcher.RemoveListener(VrDispMessageType.RoomDisConnected.ToString(), RoomDisConnected);

            try
            {
                SetRoomConnectedEvent = null;
                SetRoomDisConnectedEvent = null;
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
        void RoomConnected(IMessage msg)
        {
            try
            {
                if (SetRoomConnectedEvent != null)
                    SetRoomConnectedEvent();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        void RoomDisConnected(IMessage msg)
        {
            try
            {
                if (SetRoomDisConnectedEvent != null)
                    SetRoomDisConnectedEvent();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        #endregion

        #region API
        /// <summary>
        /// 连接新频道
        /// </summary>
        /// <param name="RootRoomID"></param>
        /// <param name="RootVoiceID"></param>
        public static void SetRootChanelChRoomID(string RootRoomID, string RootVoiceID)
        {
            VRRootChanelRoom ch = new VRRootChanelRoom
            {
                roomid = RootRoomID,
                voiceid = RootVoiceID,
            };
            MessageDispatcher.SendMessageData(VrDispMessageType.ConnectToNewChanel.ToString(), ch, 0);
        }
        /// <summary>
        /// 获取最新链接频道房间数据
        /// </summary>
        /// <returns></returns>
        public static LastIDLinkChanelRoomData GetLastIDLinkChanelRoomList()
        {
            LastIDLinkChanelRoomData linkChanelRoomData = new LastIDLinkChanelRoomData();

            if (mStaticThings.I == null)
            {
                return null;
            }
            else
            {
                linkChanelRoomData.RoomIds = new List<string>();
                linkChanelRoomData.VoiceIds = new List<string>();
                for (int i = 0; i < mStaticThings.I.LastIDLinkChanelRoomList.Count; i++)
                {
                    linkChanelRoomData.RoomIds.Add(mStaticThings.I.LastIDLinkChanelRoomList[i].roomid);
                }
                for (int i = 0; i < mStaticThings.I.LastIDLinkChanelRoomList.Count; i++)
                {
                    linkChanelRoomData.VoiceIds.Add(mStaticThings.I.LastIDLinkChanelRoomList[i].voiceid);
                }
            }
            return linkChanelRoomData;
        }

        #endregion

    }
}