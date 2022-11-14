using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSEngine
{
    public class VsCloudRender : BaseFunction
    {        
        /// <summary>
        /// 接收gif保存的本地路径
        /// </summary>
        public static event System.Action<string> RecieveSetGifLocalPathEvent;
        public VsCloudRender() : base(FunctionType.VsCloudRender)
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
            MessageDispatcher.AddListener(VrDispMessageType.SetGifLocalPath.ToString(), SetGifLocalPath, true);
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
            MessageDispatcher.RemoveListener(VrDispMessageType.SetGifLocalPath.ToString(), SetGifLocalPath, true);

            try
            {
                RecieveSetGifLocalPathEvent = null;
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
        private void SetGifLocalPath(IMessage rMessage)
        {
            string data = rMessage.Data as string;

            try
            {
                if (RecieveSetGifLocalPathEvent != null)
                    RecieveSetGifLocalPathEvent(data);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        #endregion

        #region API
        /// <summary>
        /// 设置云渲染退出信息
        /// </summary>
        /// <param name="jsonData"></param>
        public static void SetCloudRenderExitData(string jsonData)
        {
            MessageDispatcher.SendMessageData(CloudRenderMessageType.CloudRender_SetExitData.ToString(), jsonData, 0);
        }
        /// <summary>
        /// 开始录制gif（参数类型：GifRecordData）
        /// </summary>
        public static void StartRecordGif(GifRecordData recordData)
        {
            //GifRecordData recordData = new GifRecordData();
            //recordData.width = 400;
            //recordData.height = 300;
            //recordData.bRepeat = true;
            //recordData.FramePerSecond = 15;
            //recordData.TimeSecond = 4;
            MessageDispatcher.SendMessageData(VrDispMessageType.StartRecordGif.ToString(), recordData, 0);
        }
        /// <summary>
        /// 停止并保存录制的gif（无参数）
        /// </summary>
        public static void StopAndSaveGif()
        {
            MessageDispatcher.SendMessageData(VrDispMessageType.StopAndSaveGif.ToString(), "", 0);
        }
        /// <summary>
        /// 人物跳舞
        /// </summary>
        public static void AvatarDancing(bool bDance)
        {
            MessageDispatcher.SendMessageData("AvatarDancingReq", bDance ? 1:0);
        }
        #endregion
    }
}