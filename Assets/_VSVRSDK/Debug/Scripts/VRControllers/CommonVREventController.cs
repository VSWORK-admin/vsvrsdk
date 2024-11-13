using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

using UnityEngine.XR;
namespace VSWorkSDK
{
    internal enum VrDispSocketioEvent
    {
        GetDelayTime,
        GetFpsNumber,
        GetMemNumber
    }

    internal class CommonVREventController : MonoBehaviour
    {
        private static CommonVREventController instance;

        public int rightgrabstatus = 0;
        public int leftgrabstatus = 0;

        public int ismounted = 2;

        public float mouseTrigger = 0f;
        public bool IsmountEnabled = true;

        public int nowpose;
        public int lpose;
        public int rpose;

        public int nowneo2state = 0;
        public static CommonVREventController I
        {
            get
            {
                return instance;
            }
        }

        SteamVREventController steamvr = new SteamVREventController();

        private void Awake()
        {
            instance = this;

            SetOritPortoff();
        }

        public TouchScreenKeyboard m_keyboard;

        private void Start()
        {
            if (CommonVRActorController.I.CurrentDevice == CustomVRDevice.steamvr)
            {
                steamvr.Start();
            }
        }

        public void SetOritPorton()
        {
            return;
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {

                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = false;
            }
        }

        public void SetOritPortoff()
        {
            return;
            if (!mStaticThings.I.isVRApp && mStaticThings.I.ismobile && Application.platform != RuntimePlatform.IPhonePlayer)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
            }

        }

        public void HMDMounted()
        {
            if (IsmountEnabled)
            {
                ismounted = 2;
                MessageDispatcher.SendMessage(this, CommonVREventType.HMDMounted.ToString(), true, 0);
            }

        }

        public void HMDUnmounted()
        {
            if (IsmountEnabled)
            {
                ismounted = 1;
                MessageDispatcher.SendMessage(this, CommonVREventType.HMDUnmounted.ToString(), true, 0);
            }
        }

        void Update()
        {
            if (CommonVRActorController.I.CurrentDevice == CustomVRDevice.steamvr)
                steamvr.Update();
        }

        public Vector2 Get_VR_LeftStickAxis()
        {
            if (CommonVRActorController.I.CurrentDevice == CustomVRDevice.steamvr)
                return steamvr.Get_VR_LeftStickAxis();
            else return Vector2.zero;
        }

        public Vector2 Get_VR_RightStickAxis()
        {
            if (CommonVRActorController.I.CurrentDevice == CustomVRDevice.steamvr)
                return steamvr.Get_VR_RightStickAxis();
            else return Vector2.zero;
        }

        public float GetVR_LeftTriggerAxis()
        {
            if (CommonVRActorController.I.CurrentDevice == CustomVRDevice.steamvr)
                return steamvr.GetVR_LeftTriggerAxis();
            else return 0.0f;
        }

        public float GetVR_RightTriggerAxis()
        {
            if (CommonVRActorController.I.CurrentDevice == CustomVRDevice.steamvr)
                mouseTrigger = steamvr.GetVR_RightTriggerAxis();
            else mouseTrigger = 0.0f;
            return mouseTrigger;
        }

        public float GetVR_LeftGrabAxis()
        {
            if (CommonVRActorController.I.CurrentDevice == CustomVRDevice.steamvr)
                return steamvr.GetVR_LeftGrabAxis();
            else return 0.0f;
        }

        public float GetVR_RightGrabAxis()
        {
            if (CommonVRActorController.I.CurrentDevice == CustomVRDevice.steamvr)
                return steamvr.GetVR_RightGrabAxis();
            else return 0.0f;
        }

        public int GetleftGrabStatus()
        {
            return leftgrabstatus;
        }

        public int GetrightGranStatus()
        {
            return rightgrabstatus;
        }
    }
}