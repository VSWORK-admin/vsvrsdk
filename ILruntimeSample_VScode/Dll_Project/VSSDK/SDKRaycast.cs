using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKRaycast : DllGenerateBase
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
            VSEngine.Instance.OnEventPointClickHandler += OnPointClick;
            VSEngine.Instance.OnEventPointEnterHandler += OnPointEnter;
            VSEngine.Instance.OnEventPointExitHandler += OnPointExit;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventPointClickHandler -= OnPointClick;
            VSEngine.Instance.OnEventPointEnterHandler -= OnPointEnter;
            VSEngine.Instance.OnEventPointExitHandler -= OnPointExit;
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
        void OnPointClick(GameObject clicked)
        {
            Debug.Log("VSEngine OnPointClick click " + clicked.name);
        }
        void OnPointEnter(GameObject pointenter)
        {
            Debug.Log("VSEngine OnPointEnter pointenter " + pointenter.name);
        }
        void OnPointExit(GameObject pointexit)
        {
            Debug.Log("VSEngine OnPointExit pointenter " + pointexit.name);
        }
    }
}
