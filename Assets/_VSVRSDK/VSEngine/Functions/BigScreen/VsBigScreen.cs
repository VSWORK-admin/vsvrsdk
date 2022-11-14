using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSEngine
{
    public class VsBigScreen : BaseFunction
    {
        public VsBigScreen() : base(FunctionType.VsBigScreen)
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

        #region Event

        #endregion

        #region API
        /// <summary>
        /// 设置VR大屏坐标信息
        /// </summary>
        /// <param name="ScreenMark"></param>
        /// <param name="enabled"></param>
        public static void SetVRBigScreen(BigScreenSelectController ScreenMark, bool enabled)
        {
            if (mStaticThings.I == null) { return; }
            WsBigScreen wbs = new WsBigScreen()
            {
                id = mStaticThings.I.mAvatarID,
                enabled = enabled,
                angle = ScreenMark.ScreenAngle,
                position = ScreenMark.transform.position,
                rotation = ScreenMark.transform.rotation,
                scale = ScreenMark.startscale
            };
            MessageDispatcher.SendMessageData(VrDispMessageType.BigScreenEndAnchor.ToString(), wbs, 0);
        }
        #endregion
    }
}