using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class VSModelController : DllGenerateBase
    {
        private GameObject pointClickObj;
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
            VSEngine.Instance.OnEventMovingObject += OnMovingObject;
            VSEngine.Instance.OnEventPointClickHandler += OnPointClick;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventMovingObject -= OnMovingObject;
            VSEngine.Instance.OnEventPointClickHandler -= OnPointClick;
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
                VSEngine.Instance.MarkObjectCanMove(pointClickObj, true);
            }

        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnPointClick(GameObject obj)
        {
            pointClickObj = obj;
        }
        void OnMovingObject(WsMovingObj objdata)
        {
            Debug.Log("VSEngine OnMovingObject objdata " + objdata.name);
        }
    }
}
