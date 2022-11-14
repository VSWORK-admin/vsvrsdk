using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSEngine
{
    public class VsVRInput : BaseFunction
    {
        /// <summary>
        /// VR输入事件（参数1：输入类型，参数2：一维轴数据，参数3：2维轴数据）
        /// </summary>
        public static event System.Action<CommonVREventType, float, Vector2> CommonVREvent;
        public VsVRInput() : base(FunctionType.VsVRInput)
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
            foreach (var item in Enum.GetNames(typeof(CommonVREventType)))
            {
                MessageDispatcher.RemoveListener(item, GetInput, true);
            }
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var item in Enum.GetNames(typeof(CommonVREventType)))
            {
                MessageDispatcher.RemoveListener(item, GetInput, true);
            }

            try
            {
                CommonVREvent = null;
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

        private void GetInput(IMessage msg)
        {
            Vector2 Rcieved2DAxis = Vector2.zero;
            float Rcieved1DAxis = 0.0f;

            if (msg.Type == CommonVREventType.VR_LefStickAxis.ToString() || msg.Type == CommonVREventType.VR_RightStickAxis.ToString())
            {
                Rcieved2DAxis = (Vector2)msg.Data;
            }
            else if (msg.Type == CommonVREventType.VR_LeftTriggerAxis.ToString() || msg.Type == CommonVREventType.VR_RightTriggerAxis.ToString() || msg.Type == CommonVREventType.VR_LeftGrabAxis.ToString() || msg.Type == CommonVREventType.VR_RightGrabAxis.ToString())
            {
                Rcieved1DAxis = (float)msg.Data;
            }

            if (CommonVREvent != null)
            {
                CommonVREventType commonVREventType;
                if (Enum.TryParse<CommonVREventType>(msg.Type, out commonVREventType))
                {
                    try
                    {
                        CommonVREvent(commonVREventType, Rcieved1DAxis, Rcieved2DAxis);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        CommonVREvent = null;
                    }
                }
            }
        }

        #endregion

        #region API

        /// <summary>
        /// 设置话筒状态
        /// </summary>
        /// <param name="SendMicEnabled"></param>
        /// <param name="SendMicDisabled"></param>
        /// <param name="SendMicOn"></param>
        /// <param name="SendMicOff"></param>
        public static void SetVRMicEnabled(bool SendMicEnabled, bool SendMicDisabled, bool SendMicOn, bool SendMicOff)
        {
            if (SendMicEnabled)
            {
                MessageDispatcher.SendMessage(VoiceDispMessageType.GmeMicEnalbe.ToString());
            }
            else if (SendMicDisabled)
            {
                MessageDispatcher.SendMessage(VoiceDispMessageType.GmeMicDisable.ToString());
            }

            if (SendMicOn)
            {
                MessageDispatcher.SendMessage(VoiceDispMessageType.GmemMicOn.ToString());
            }
            else if (SendMicOff)
            {
                MessageDispatcher.SendMessage(VoiceDispMessageType.GmeMicOff.ToString());
            }
        }
        /// <summary>
        /// 设置角色手模型显示或隐藏
        /// </summary>
        /// <param name="handModelType"></param>
        /// <param name="enabled"></param>
        public static void SetHandModelEnabled(HandModelType handModelType, bool enabled)
        {
            MessageDispatcher.SendMessageData(handModelType.ToString(), enabled, 0);
        }
        #endregion
    }
}
