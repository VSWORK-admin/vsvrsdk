using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSEngine
{
    public class VsAuthority : BaseFunction
    {
        private bool IsAdmin;
        /// <summary>
        /// 主持人改变事件（参数：是否为Admin）
        /// </summary>
        public static event Action<bool> SetAdminEvent;
        public VsAuthority() : base(FunctionType.VsAuthority)
        {

        }
        internal override void Awake()
        {
            base.Awake();

            MessageDispatcher.AddListener(VrDispMessageType.SetAdmin.ToString(), SetAdmin, true);
        }
        // Start is called before the first frame update
        internal override void Start()
        {
            base.Awake();
        }

        // Update is called once per frame
        internal override void Update()
        {
            base.Awake();
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        internal override void OnEnable()
        {
            base.OnEnable();
        }
        internal override void OnDisable()
        {
            base.OnDisable();
        }
        internal override void OnDestroy()
        {
            base.OnDestroy();

            MessageDispatcher.RemoveListener(VrDispMessageType.SetAdmin.ToString(), SetAdmin, true);

            try
            {
                SetAdminEvent = null;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        #region Event
        private void SetAdmin(IMessage msg)
        {
            int sort;
            if (mStaticThings.I == null)
            {
                sort = 0;
            }
            else
            {
                sort = mStaticThings.I.GetSortNumber(mStaticThings.I.mAvatarID);
            }
            IsAdmin = (sort == 0);

            bool getadmin = (bool)msg.Data;

            if (getadmin != IsAdmin)
            {
                IsAdmin = getadmin;

                try
                {
                    if (SetAdminEvent != null)
                        SetAdminEvent(IsAdmin);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        #endregion

        #region API
        /// <summary>
        /// 获取当前的Admin状态
        /// </summary>
        /// <returns></returns>
        public static bool GetAdminStatus()
        {
            if (mStaticThings.I == null)
            {
                return false;
            }

            if (mStaticThings.I.isAdmin || mStaticThings.I.sadmin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}