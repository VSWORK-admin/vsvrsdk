using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKTeleport : DllGenerateBase
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
            VSEngine.Instance.OnEventSelfPlaceTo += OnSelfPlaceTo;
            VSEngine.Instance.OnEventSelfStepOnMesh += OnSelfStepOnMesh;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventSelfPlaceTo -= OnSelfPlaceTo;
            VSEngine.Instance.OnEventSelfStepOnMesh -= OnSelfStepOnMesh;
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
                VRPlayceGroup group = GameObject.FindObjectOfType<VRPlayceGroup>();
                VSEngine.Instance.SetAllAvatarToGroupPos(group.name,true);
            }

        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnSelfPlaceTo(string placename)
        {
            Debug.Log("VSEngine OnSelfPlaceTo placename " + placename);
        }
        void OnSelfStepOnMesh(string meshname)
        {
            Debug.Log("VSEngine OnSelfStepOnMesh meshname " + meshname);
        }
    }
}
