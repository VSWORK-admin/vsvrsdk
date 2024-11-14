using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKAdmin : DllGenerateBase
    {
        public override void Init()
        {
            base.Init();
        }
        public override void Awake()
        {
            base.Awake();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            VSEngine.Instance.OnEventAdminChanged += OnAdminChanged;
            VSEngine.Instance.OnEventPointClickHandler += OnPointClick;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventAdminChanged -= OnAdminChanged;

        }
        public override void Start()
        {
            base.Start();
        }
        public override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                VSEngine.Instance.SetUserAdmin(VSEngine.Instance.GetMyAvatarID());
            }
            
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnAdminChanged(ConnectAvatars admindata)
        {
            Debug.Log("VSEngine OnAdminChanged admin " + admindata.sort);
        }
        void OnPointClick(GameObject obj)
        {
            Debug.Log("VSEngine SDKAdmin obj " + obj.name);
        }
    }
}
