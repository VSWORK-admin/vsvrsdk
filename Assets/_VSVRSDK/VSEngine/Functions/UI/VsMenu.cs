using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSEngine
{
    public class VsMenu : BaseFunction
    {
        /// <summary>
        /// 系统菜单开关事件（参数：开关状态）
        /// </summary>
        public static event Action<bool> SystemMenuActionEvent;
        /// <summary>
        /// 场景菜单开关事件（参数：开关状态）
        /// </summary>
        public static event Action<bool> SceneMenuActionEvent;
        public VsMenu() : base(FunctionType.VsMenu)
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

            MessageDispatcher.AddListener(VrDispMessageType.SystemMenuEvent.ToString(), SystemMenuAction, true);
            MessageDispatcher.AddListener(VrDispMessageType.SceneMenuEnable.ToString(), SceneMenuAction, true);
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();

            MessageDispatcher.RemoveListener(VrDispMessageType.SystemMenuEvent.ToString(), SystemMenuAction, true);
            MessageDispatcher.RemoveListener(VrDispMessageType.SceneMenuEnable.ToString(), SceneMenuAction, true);
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
        private void SystemMenuAction(IMessage msg)
        {
            bool ison = (bool)msg.Data;

            try
            {
                if (SystemMenuActionEvent != null)
                    SystemMenuActionEvent(ison);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void SceneMenuAction(IMessage msg)
        {
            bool ison = (bool)msg.Data;

            try
            {
                if (SceneMenuActionEvent != null)
                    SceneMenuActionEvent(ison);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        #endregion

        #region API


        /// <summary>
        /// 设置系统菜单开关
        /// </summary>
        /// <param name="enabled"></param>
        public static void SetVRSystemMemuEnable(bool enabled)
        {
            if (mStaticThings.I == null) { return; }
            MessageDispatcher.SendMessageData(VrDispMessageType.SystemMenuEnable.ToString(), enabled, 0);
        }

        #endregion
    }
}