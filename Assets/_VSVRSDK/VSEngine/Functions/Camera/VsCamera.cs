using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSEngine
{
    public enum CameraViewType
    {
        ThisFollowCamera,
        SelectFollowCamera,
        AllMyselfFollowCamera,
        CameraScreenSetFree
    }
    public class VsCamera : BaseFunction
    {
        public VsCamera() : base(FunctionType.VsCamera)
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

            if (bEveryframeSetNormalScreenCamera && CameraMark != null)
            {
                MessageDispatcher.SendMessage(this, VrDispMessageType.CameraScreenCameraSet.ToString(), CameraMark, 0);
            }
        }

        #region Event

        #endregion

        #region API
        private static PCScreenCameraMark CameraMark;
        private static bool bEveryframeSetNormalScreenCamera = false;
        /// <summary>
        /// 设置普通相机
        /// </summary>
        /// <param name="CameraMark"></param>
        /// <param name="everyframe"></param>
        public static void SetNormalScreenCamera(PCScreenCameraMark cameraMark, bool everyframe)
        {
            CameraMark = cameraMark;
            bEveryframeSetNormalScreenCamera = everyframe;

            if (CameraMark != null)
            {
                MessageDispatcher.SendMessageData(VrDispMessageType.CameraScreenCameraSet.ToString(), CameraMark, 0);
            }
        }
        /// <summary>
        /// 设置VR相机
        /// </summary>
        /// <param name="CameraMark"></param>
        public static void SetVRScreenCamera(PCScreenCameraMark cameraMark)
        {
            CameraMark = cameraMark;

            if (CameraMark != null)
            {
                MessageDispatcher.SendMessageData(VrDispMessageType.CameraScreenSettingEnd.ToString(), CameraMark, 0);
            }
        }

        /// <summary>
        /// 切换相机模式
        /// </summary>
        /// <param name="cameraViewType"></param>
        public static void SendWsCameraView(CameraViewType cameraViewType)
        {
            if (cameraViewType == CameraViewType.ThisFollowCamera)
            {
                SetThisFollowCamera();
            }
            else if (cameraViewType == CameraViewType.SelectFollowCamera)
            {
                SetSelectFollowCamera();
            }
            else if (cameraViewType == CameraViewType.AllMyselfFollowCamera)
            {
                SetAllMyselfFollowCamera();
            }
            else if (cameraViewType == CameraViewType.CameraScreenSetFree)
            {
                MessageDispatcher.SendMessageData(VrDispMessageType.CameraScreenSetFree.ToString(), true, 0);
            }
        }

        private static void SetThisFollowCamera()
        {
            CameraScreenInfo sendinfo = new CameraScreenInfo()
            {
                sendid = mStaticThings.I.mAvatarID,
                isfree = false,
                ismyself = false,
                lockwsid = mStaticThings.I.mAvatarID
            };
            MessageDispatcher.SendMessageData(WsMessageType.SendPCCamera.ToString(), sendinfo, 0);
        }

        private static void SetSelectFollowCamera()
        {
            CameraScreenInfo sendinfo = new CameraScreenInfo()
            {
                sendid = mStaticThings.I.mAvatarID,
                isfree = false,
                ismyself = false,
                lockwsid = mStaticThings.I.NowSelectedAvararid
            };
            MessageDispatcher.SendMessageData(WsMessageType.SendPCCamera.ToString(), sendinfo, 0);
        }

        private static void SetAllMyselfFollowCamera()
        {
            CameraScreenInfo sendinfo = new CameraScreenInfo()
            {
                sendid = mStaticThings.I.mAvatarID,
                isfree = false,
                ismyself = true,
                lockwsid = "none"
            };
            MessageDispatcher.SendMessageData(WsMessageType.SendPCCamera.ToString(), sendinfo, 0);
        }
        /// <summary>
        /// 切换第一或第三人称视角
        /// </summary>
        /// <param name="bThirdPersionView"></param>
        /// <param name="bCameraAnim"></param>
        public static void SwitcPersionViewMode(bool bThirdPersionView,bool bCameraAnim)
        {
            int result = 0;
            if(bThirdPersionView)
            {
                if (!bCameraAnim)
                    result = 10;
                else
                    result = 11;
            }
            else
            {
                if (!bCameraAnim)
                    result = 0;
                else
                    result = 1;
            }
            MessageDispatcher.SendMessageData("SwitcPersionViewModeReq", result);
        }

        #endregion
    }
}