using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ETCAxis;
using System;
//using RootMotion.FinalIK;
namespace VSWorkSDK
{
    internal class AvatarActionController : MonoBehaviour
    {
        public static AvatarActionController Instance = null;
        public enum AvatarActionState
        {
            Idle = 1 << 0,
            Walk = 1 << 1,
            Run = 1 << 2,
            Jump = 1 << 3,
            Sit = 1 << 4,
            Stand = 1 << 5,         //站起
            WalkLeft = 1 << 6,      //左走
            WalkRight = 1 << 7,
            WalkBack = 1 << 8,      //后退
            RunBack = 1 << 9,
            TurnLeft = 1 << 10,     //左转
            TurnRight = 1 << 11,
            HeadUp = 1 << 12,       //抬头
            HeadDown = 1 << 13,
            WaveHand = 1 << 14,     //招手
        }

        private Action<int, Vector3> doDirectAction;
        private Func<int, int> preCheckAction;
        //private Action<CustomAvatarWSDriver, WsAvatarFrameJian> doSyncAvatarTransform;
        private Animator avatarAnimator;

        private DirectAction curAction;

        private void Awake()
        {
            if (Instance != null)
                Debug.LogError("AvatarActionController Instance already");

            Instance = this;
            avatarAnimator = GetComponent<Animator>();
        }

        public void SetPreCheckFun(Func<int, int> fun)
        {
            preCheckAction = fun;
        }
        public void SetDoDirectAction(Action<int, Vector3> fun)
        {
            doDirectAction = fun;
        }
        public int PreCheckAction(DirectAction directaction)
        {
            int code = preCheckAction != null ? preCheckAction((int)directaction) : 0;

            return code;
        }

        //public void SetDoSyncAvatarTransform(Action<CustomAvatarWSDriver,WsAvatarFrameJian> action)
        //{
        //    doSyncAvatarTransform = action;
        //}

        public void DoDirectAction(DirectAction directaction, Vector3 localAxis)
        {
            curAction = directaction;
            if (doDirectAction != null)
                doDirectAction((int)directaction, localAxis);
        }

        public void OnMoveEnd()
        {
            if (doDirectAction != null)
                doDirectAction(-1, Vector3.zero);
        }

        //public void SetAvatarAnimatorEnable(string id,bool benable)
        //{
        //    CustomAvatarWSDriver avatardriver = null;
        //    if (GameManager.Instance.avatarManager.AvatarGameObjectDic.TryGetValue(id,out avatardriver))
        //    {
        //        Animator ani = avatardriver.gameObject.GetComponentInChildren<Animator>(true);
        //        if (ani != null)
        //        {
        //            ani.enabled = benable;
        //        }

        //        VRIK avatarvrik = avatardriver.GetComponentInChildren<VRIK>(true);
        //        if (avatarvrik != null)
        //        {
        //            avatarvrik.enabled = !benable;
        //        }
        //        else
        //        {
        //            Debug.LogError("SetAvatarAnimatorEnable Error 查找VRIK");
        //        }
        //    }
        //}

        //public void SyncAvatarTransform(CustomAvatarWSDriver avatar,WsAvatarFrameJian CurWsAvatarFrame)
        //{
        //    if (doSyncAvatarTransform != null && avatar != null && avatar.ison)
        //    {
        //        doSyncAvatarTransform(avatar,CurWsAvatarFrame);
        //    }
        //}

        public Animator GetDefaultAnimator()
        {
            return avatarAnimator;
        }
    }
}