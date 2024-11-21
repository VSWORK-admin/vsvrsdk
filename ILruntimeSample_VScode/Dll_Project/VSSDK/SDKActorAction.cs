using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKActorAction : DllGenerateBase
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
            VSEngine.Instance.OnEventSwitchCameraViewMode += OnSwitchViewMode;
            VSEngine.Instance.OnEventPointClickHandler +=OnPointClick;
           // VSEngine.Instance.OnEventSettingButtonClick += OnEventSettingButtonClick;
        }

        private void OnEventSettingButtonClick(bool obj)
        {
            Debug.LogError("OnEventSettingButtonClick" + obj+BaseMono.name);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventSwitchCameraViewMode -= OnSwitchViewMode;
          

        }
        public override void Start()
        {
            base.Start();
        }
        public override void Update()
        {
            base.Update();
            //if (Input.GetKeyDown(KeyCode.Alpha0))
            //{
            //    VSEngine.Instance.SwitchCameraViewMode(CameraViewMode.FirstPerson);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    VSEngine.Instance.LimitSelfMove(true);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    VSEngine.Instance.LimitSelfRotate(true);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    VSEngine.Instance.LimitSelfMove(false);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    VSEngine.Instance.LimitSelfRotate(false);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha5))
            //{
            //    VSEngine.Instance.MakeSelfActionUnderControl(true);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha6))
            //{
            //    VSEngine.Instance.ChangeSelfActionState(ActorActionType.Jump);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha7))
            //{
            //    VSEngine.Instance.LockSelfWalkMode(WalkLockMode.LOCKWALK);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha8))
            //{
            //    VSEngine.Instance.SyncSelfAvatarDataImmediately();
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha9))
            //{
            //    VSEngine.Instance.SetAvatarSyncFrameRate(1);
            //}
            //if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
            //{
            //    VSEngine.Instance.ReSetAvatarSyncFrameRate();
            //}
            //if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B))
            //{
            //    VSEngine.Instance.PlayAvatarCustomAction(25);
            //}
            //if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            //{
            //    VSEngine.Instance.PlayAvatarDancingAction(1);
            //}
            //if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
            //{
            //    VSEngine.Instance.PlayAvatarSpeakAction(1);
            //}
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnSwitchViewMode(int viewmode)
        {
            Debug.Log("VSEngine OnSwitchViewMode viewmode " + viewmode);
        }
        void OnPointClick(GameObject obj)
        {
            Debug.Log("VSEngine ActorAction obj " + obj.name);
        }
    }
}
