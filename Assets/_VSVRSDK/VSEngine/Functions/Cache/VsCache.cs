using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSEngine
{
    public class VsCache : BaseFunction
    {
        /// <summary>
        /// 接收缓存数据（参数:数据字典）
        /// </summary>
        public static event Action<Dictionary<string, string>> RecieveGetDataEvent; 
        private static Queue<Action<Dictionary<string, string>>> RecieveGetDataEventQueue = new Queue<Action<Dictionary<string, string>>>(); 
        public VsCache() : base(FunctionType.VsCache)
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
            MessageDispatcher.AddListener(WsMessageType.RecieveGetData.ToString(), RecieveGetData);
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
            MessageDispatcher.AddListener(WsMessageType.RecieveGetData.ToString(), RecieveGetData);

            try
            {
                RecieveGetDataEvent = null;
                RecieveGetDataEventQueue.Clear();
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
        private void RecieveGetData(IMessage rMessage)
        {
            Dictionary<string, string> dic = rMessage.Data as Dictionary<string, string>;

            try
            {
                if (RecieveGetDataEvent != null)
                    RecieveGetDataEvent(dic);

                var RecieveGetDataEventInfo = RecieveGetDataEventQueue.Dequeue();

                if (RecieveGetDataEventInfo != null)
                {
                    RecieveGetDataEventInfo(dic);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        #endregion

        #region API
        /// <summary>
        /// 发送 请求频道服务端缓存数据
        /// </summary>
        /// <param name="RecieveGetDataAc">数据回调</param>
        public static void SendGetData(Action<Dictionary<string, string>> RecieveGetDataAc)
        {
            if (RecieveGetDataAc != null)
                RecieveGetDataEventQueue.Enqueue(RecieveGetDataAc);
            MessageDispatcher.SendMessage(WsMessageType.SendGetData.ToString(), 0f);
        }
        /// <summary>
        /// 发送 保存需要缓存的频道服务端数据
        /// </summary>
        /// <param name="bAll">是否保存在当前频道中的所有动作房间中</param>
        /// <param name="key">需要保存的数据的唯一值</param>
        /// <param name="value">需要保存的数据</param>
        public static void SendSaveData(bool bAll,string key,string value)
        {
            VRSaveRoomData changeInfo = new VRSaveRoomData
            {
                sall = bAll,
                key = key,
                value = value,
            };
            MessageDispatcher.SendMessageData(WsMessageType.SendSaveData.ToString(), changeInfo, 0);
        }
        #endregion
    }
}