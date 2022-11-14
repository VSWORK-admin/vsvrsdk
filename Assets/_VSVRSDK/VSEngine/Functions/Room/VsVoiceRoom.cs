using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSEngine
{
    public class VsVoiceRoom : BaseFunction
    {
        public VsVoiceRoom() : base(FunctionType.VsVoiceRoom)
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
        /// 连接拓展语音房间
        /// </summary>
        /// <param name="ExRoomID"></param>
        public static void SetVoiceExRoomID(string ExRoomID)
        {
            MessageDispatcher.SendMessageData(VoiceDispMessageType.ConnectVoiceExRoom.ToString(), ExRoomID, 0);
        }
        /// <summary>
        /// 退出语音房间
        /// </summary>
        public static void SetVoiceRoomExit()
        {
            MessageDispatcher.SendMessage(VoiceDispMessageType.ExitVoiceRoom.ToString());
        }
        #endregion
    }
}