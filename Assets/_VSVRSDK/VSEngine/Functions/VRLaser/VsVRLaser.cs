using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSEngine
{
    public class VsVRLaser : BaseFunction
    {
        /// <summary>
        /// 激光笔开关状态
        /// </summary>
        public static bool SelectorEnabled
        {
            get
            {
                if (mStaticThings.I == null) return false;
                return mStaticThings.I.SelectorEnabled;
            }
        }
        /// <summary>
        /// 激光笔状态改变事件（参数：激光笔开关状态）
        /// </summary>
        public static event Action<bool> SelectorPointerStatusChangeEvent;
        private GameObject PointedObj;
        /// <summary>
        /// 激光笔物体指向变化事件（参数1：激光笔操作类型，参数2：被指向物体）
        /// </summary>
        public static event Action<VRPointObjEventType, GameObject> VRLaserChangeEvent;
        public VsVRLaser() : base(FunctionType.VsVRLaser)
        {

        }
        internal override void Awake()
        {
            base.Awake();

            MessageDispatcher.AddListener(VrDispMessageType.SelectorPointerStatusChange.ToString(), SelectorPointerStatusChange);
            foreach (var item in Enum.GetNames(typeof(VRPointObjEventType)))
            {
                MessageDispatcher.AddListener(item, VRLaserChange, true);
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
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

            MessageDispatcher.RemoveListener(VrDispMessageType.SelectorPointerStatusChange.ToString(), SelectorPointerStatusChange);
            foreach (var item in Enum.GetNames(typeof(VRPointObjEventType)))
            {
                MessageDispatcher.RemoveListener(item, VRLaserChange, true);
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

        void SelectorPointerStatusChange(IMessage msg)
        {
            bool ispointon = (bool)msg.Data;

            try
            {
                if (SelectorPointerStatusChangeEvent != null)
                    SelectorPointerStatusChangeEvent(ispointon);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void VRLaserChange(IMessage msg)
        {
            GameObject pobj = msg.Data as GameObject;
            PointedObj = pobj;

            VRPointObjEventType vRPointObjEventType;
            if (Enum.TryParse<VRPointObjEventType>(msg.Type, out vRPointObjEventType))
            {
                try
                {
                    if (VRLaserChangeEvent != null)
                        VRLaserChangeEvent(vRPointObjEventType, PointedObj);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        #endregion
    }
}