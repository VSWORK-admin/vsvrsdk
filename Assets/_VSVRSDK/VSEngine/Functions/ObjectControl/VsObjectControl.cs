using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VSEngine
{
    public class VsObjectControl : BaseFunction
    {
        /// <summary>
        /// 接收物体控制事件包括移动旋转和缩放（其他人的控制物体也在）
        /// </summary>
        public static event Action<WsMovingObj> RecieveMovingObjEvent;
        public VsObjectControl() : base(FunctionType.VsObjectControl)
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
            MessageDispatcher.AddListener(WsMessageType.RecieveMovingObj.ToString(), RecieveMovingObj,true);
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
            MessageDispatcher.RemoveListener(WsMessageType.RecieveMovingObj.ToString(), RecieveMovingObj, true);

            try
            {
                RecieveMovingObjEvent = null;
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
        private void RecieveMovingObj(IMessage msg)//位置同步
        {
            WsMovingObj newMovingObj = msg.Data as WsMovingObj;

            try
            {
                if (RecieveMovingObjEvent != null)
                    RecieveMovingObjEvent(newMovingObj);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        #endregion

        #region API
        /// <summary>
        /// 设置vr物体加载位置
        /// </summary>
        /// <param name="SceneLoadMark"></param>
        /// <param name="SceneLoadVector3"></param>
        /// <param name="SceneLoadScale"></param>
        /// <param name="ObjLoadMark"></param>
        /// <param name="ObjLoadVector3"></param>
        /// <param name="ObjLoadScale"></param>
        public static void SetVRObjLoadPosition(GameObject SceneLoadMark, Vector3 SceneLoadVector3, float SceneLoadScale, GameObject ObjLoadMark, Vector3 ObjLoadVector3, float ObjLoadScale)
        {
            if (mStaticThings.I == null) return;

            if (SceneLoadMark != null)
            {
                mStaticThings.I.GlbSceneLoadPosition = SceneLoadMark.transform.position;
                mStaticThings.I.GlbSceneLoadRotation = SceneLoadMark.transform.eulerAngles;
                mStaticThings.I.GlbSceneLoadScale = SceneLoadMark.transform.localScale.x;
            }
            else
            {
                mStaticThings.I.GlbSceneLoadPosition = SceneLoadVector3;
                mStaticThings.I.GlbSceneLoadScale = SceneLoadScale;
            }

            if (ObjLoadMark != null)
            {
                mStaticThings.I.GlbObjLoadPosition = ObjLoadMark.transform.position;
                mStaticThings.I.GlbObjLoadRotation = ObjLoadMark.transform.eulerAngles;
                mStaticThings.I.GlbObjLoadScale = ObjLoadMark.transform.localScale.x;
            }
            else
            {
                mStaticThings.I.GlbObjLoadPosition = ObjLoadVector3;
                mStaticThings.I.GlbObjLoadScale = ObjLoadScale;
            }
        }
        /// <summary>
        /// 设置物体控制包括移动旋转和缩放（广播给当前房间的人）
        /// </summary>
        /// <param name="movingObj">物体相关数据</param>
        public static void SetVRObjectMove(WsMovingObj movingObj)
        {
            MessageDispatcher.SendMessageData(WsMessageType.SendMovingObj.ToString(), movingObj, 0);
        }


        #endregion
    }
}