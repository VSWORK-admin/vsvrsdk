using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSEngine
{
    public class VsMessage : BaseFunction
    {
        public static event System.Action<WsChangeInfo> RecieveChangeObjEvent;
        private static Queue<System.Action<WsChangeInfo>> RecieveChangeObjEventQueue = new Queue<Action<WsChangeInfo>>();

        public static event System.Action<WsCChangeInfo> RecieveCChangeObjEvent;
        private static Queue<System.Action<WsCChangeInfo>> RecieveCChangeObjEventQueue = new Queue<Action<WsCChangeInfo>>();
        public VsMessage() : base(FunctionType.VsMessage)
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

            MessageDispatcher.AddListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj, true);
            MessageDispatcher.AddListener(WsMessageType.RecieveCChangeObj.ToString(), RecieveCChangeObj, true);
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();

            MessageDispatcher.RemoveListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj, true);
            MessageDispatcher.RemoveListener(WsMessageType.RecieveCChangeObj.ToString(), RecieveCChangeObj, true);

            try
            {
                RecieveChangeObjEvent = null;
                RecieveCChangeObjEvent = null;
                RecieveChangeObjEventQueue.Clear();
                RecieveCChangeObjEventQueue.Clear();
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

        void RecieveChangeObj(IMessage msg)
        {
            WsChangeInfo rinfo = msg.Data as WsChangeInfo;

            try
            {
                if (RecieveChangeObjEvent != null)
                    RecieveChangeObjEvent(rinfo);

                if (RecieveChangeObjEventQueue.Count > 0)
                {
                    var RecieveChangeObjEventInfo = RecieveChangeObjEventQueue.Dequeue();

                    if (RecieveChangeObjEventInfo != null)
                        RecieveChangeObjEventInfo(rinfo);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        void RecieveCChangeObj(IMessage msg)
        {
            WsCChangeInfo rinfo = msg.Data as WsCChangeInfo;

            try
            {
                if (RecieveCChangeObjEvent != null)
                    RecieveCChangeObjEvent(rinfo);

                if (RecieveCChangeObjEventQueue.Count > 0)
                {
                    var RecieveCChangeObjEventInfo = RecieveCChangeObjEventQueue.Dequeue();

                    if (RecieveCChangeObjEventInfo != null)
                        RecieveCChangeObjEventInfo(rinfo);
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
        /// 发送数据
        /// </summary>
        /// <param name="wsID"></param>
        /// <param name="wsName"></param>
        /// <param name="wsKind"></param>
        /// <param name="wsChangenum"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="RecieveChangeObjEvent"></param>
        public static void SendChangeObj(System.Action<WsChangeInfo> RecieveChangeObjAc, string wsID, string wsName, string wsKind, string wsChangenum, string a, string b = "", string c = "", string d = "", string e = "")
        {
            WsChangeInfo wsinfo = new WsChangeInfo()
            {
                id = wsID,
                name = wsName,
                kind = wsKind,
                changenum = wsChangenum,
                a = a,
                b = b,
                c = c,
                d = d,
                e = e,
            };

            if(RecieveChangeObjAc != null)
                RecieveChangeObjEventQueue.Enqueue(RecieveChangeObjAc);

            MessageDispatcher.SendMessageData(WsMessageType.SendChangeObj.ToString(), wsinfo, 0);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bAll">是否当前频道所有人</param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <param name="RecieveCChangeObjAc"></param>
        public static void SendCChangeObj(System.Action<WsCChangeInfo> RecieveCChangeObjAc,bool bAll,string a, string b = "", string c = "", string d = "", string e = "", string f = "", string g = "")
        {
            WsCChangeInfo wsinfo = new WsCChangeInfo()
            {
                a = a,
                b = b,
                c = c,
                d = d,
                e = e,
                f = f,
                g = g,
            };
            if (RecieveCChangeObjAc != null)
                RecieveCChangeObjEventQueue.Enqueue(RecieveCChangeObjAc);

            if(!bAll)
            {
                MessageDispatcher.SendMessageData(WsMessageType.SendCChangeObj.ToString(), wsinfo, 0);
            }
            else
            {
                MessageDispatcher.SendMessageData(WsMessageType.SendAllCChangeObj.ToString(), wsinfo, 0);
            }
        }
        #endregion

    }
}
