using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSEngine
{
    public class VsVRSystem : BaseFunction
    {
        public VsVRSystem() : base(FunctionType.VsVRSystem)
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
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
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

        #region API
        /// <summary>
        /// 获取设备SN编号（暂时无用）
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceSNNumber()
        {
            if (mStaticThings.I == null)
            {
                return string.Empty;
            }
            else
            {
                return mStaticThings.I.DeviceSNnumber;
            }
        }
        /// <summary>
        /// 打开其他安卓程序并发送消息
        /// </summary>
        /// <param name="senddata"></param>
        /// <param name="packagename"></param>
        public static void SetVROpenAPKByPackagename(Dictionary<string, string> senddata, string packagename)
        {
            MessageDispatcher.SendMessage(senddata, VrDispMessageType.OpenAPKByPackagename.ToString(), packagename, 0);
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        public static void SendCommitOrder(string OrderString)
        {
            MessageDispatcher.SendMessageData(VrDispMessageType.CommitOrder.ToString(), OrderString, 0);
        }
        /// <summary>
        /// 发送本地消息
        /// </summary>
        public static void SendCustomLocalMessage(string CustomMessage)
        {
            MessageDispatcher.SendMessage(VrDispMessageType.CustomLocalMessage.ToString(), CustomMessage, 0);
        }
        /// <summary>
        /// 发送log
        /// </summary>
        /// <param name="Info"></param>
        /// <param name="infocolor"></param>
        /// <param name="lasttime"></param>
        /// <param name="SendToOther"></param>
        public static void SendWsAllInfoLog(string Info, InfoColor infocolor, float lasttime, bool SendToOther)
        {
            if (mStaticThings.I == null) { return; }
            WsChangeInfo wsinfo = new WsChangeInfo()
            {
                id = mStaticThings.I.mAvatarID,
                name = "InfoLog",
                a = Info,
                b = infocolor.ToString(),
                c = lasttime.ToString(),
            };
            if (SendToOther)
            {
                MessageDispatcher.SendMessageData(VrDispMessageType.SendAllInfolog.ToString(), wsinfo, 0);
            }
            else
            {
                MessageDispatcher.SendMessageData(VrDispMessageType.SendInfolog.ToString(), wsinfo, 0);
            }
        }
        #endregion
    }
}