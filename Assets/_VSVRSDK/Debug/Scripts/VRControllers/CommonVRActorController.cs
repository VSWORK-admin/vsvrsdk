using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.PostProcessing;
using System;
#if WITH_OCULUSVR
using Oculus;
#endif
namespace VSWorkSDK
{
    [Serializable]
    internal class VRTransformActor
    {
        public Transform Maincamera;
        public Camera[] VRCameras;
        public Transform LeftHand;
        public Transform RightHand;
        public Transform LeftTeleportAnchor;
        public Transform RightTeleportAnchor;
        public Transform LeftFingerPointerAnchor;
        public Transform RightFingerPointerAnchor;
        public Transform MainVRROOT;
        public Transform Trackfix;
        public Camera photocamera;
    }
    [Serializable]
    internal class VRtransformActorName
    {
        public CustomVRDevice name;
        public VRTransformActor actor;
    }

    internal enum CustomVRDevice
    {
        pc,
        steamvr,
    }

    internal class CommonVRActorController : MonoBehaviour
    {
        public CustomVRDevice CurrentDevice = CustomVRDevice.pc;
        public List<VRtransformActorName> actors = new List<VRtransformActorName>();
        public VRtransformActorName nowDevice;
        private static CommonVRActorController instance;

        public Vector3 StartTrackfixvector = Vector3.zero;
        public static CommonVRActorController I
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
#if UNITY_2019
            string modelname = XRDevice.model == null ? "" : XRDevice.model;
#elif UNITY_2021
            string modelname = XRSettings.loadedDeviceName == null ? "" : XRSettings.loadedDeviceName;
#endif

            if (CurrentDevice == CustomVRDevice.pc)
            {
                mStaticThings.I.isVRApp = false;
                nowDevice = actors[1];

                mStaticThings.I.nowdevicename = CustomVRDevice.pc.ToString();
            }
            else if (CurrentDevice == CustomVRDevice.steamvr)
            {
                mStaticThings.I.isVRApp = true;
                nowDevice = actors[0];
                mStaticThings.I.nowdevicename = CustomVRDevice.steamvr.ToString();
            }

            //mStaticThings.I.DeviceSNnumber  = SystemInfo.deviceUniqueIdentifier;
#if UNITY_ANDROID && !UNITY_EDITOR
        try{
            AndroidJavaObject jo = new AndroidJavaObject("android.os.Build");
            string srnum = jo.GetStatic<string>("SERIAL");
            if(srnum != null && srnum != ""){
                mStaticThings.I.DeviceSNnumber = srnum;
            }
        }catch{

        }
#endif


            mStaticThings.I.movingmarklist.Add(false);
            mStaticThings.I.movingmarklist.Add(false);


            foreach (var item in actors)
            {
                if (item.name != nowDevice.name)
                {
                    if (item.actor != null && item.actor.MainVRROOT != null)
                    {
                        item.actor.MainVRROOT.gameObject.SetActive(false);
                    }
                }
            }

            if (CurrentDevice == CustomVRDevice.pc)
            {
                actors[1].actor.MainVRROOT.gameObject.SetActive(true);
                SetCaneraRoot(actors[1].actor);
            }
            else if (CurrentDevice == CustomVRDevice.steamvr)
            {
                actors[0].actor.MainVRROOT.gameObject.SetActive(true);
                SetCaneraRoot(actors[0].actor);
            }

            foreach (var item in mStaticThings.I.VRCameras)
            {
                item.enabled = true;
            }
        }


        void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

        }


        void SetCaneraRoot(VRTransformActor act)
        {
            mStaticThings.I.Maincamera = act.Maincamera;
            mStaticThings.I.VRCameras = act.VRCameras;

            //if (mStaticThings.I.ismobile)
            //{
            //    if (mStaticThings.I.Maincamera.GetComponent<PostProcessLayer>())
            //    {
            //        mStaticThings.I.Maincamera.GetComponent<PostProcessLayer>().stopNaNPropagation = false;
            //        mStaticThings.I.Maincamera.GetComponent<PostProcessLayer>().enabled = false;
            //    }

            //    foreach (Camera item in mStaticThings.I.VRCameras)
            //    {
            //        if (item.GetComponent<PostProcessLayer>())
            //        {
            //            item.GetComponent<PostProcessLayer>().stopNaNPropagation = false;
            //            item.GetComponent<PostProcessLayer>().enabled = false;
            //        }
            //    }
            //}

            mStaticThings.I.LeftHand = act.LeftHand;
            mStaticThings.I.RightHand = act.RightHand;
            mStaticThings.I.LeftTeleportAnchor = act.LeftTeleportAnchor;
            mStaticThings.I.RightTeleportAnchor = act.RightTeleportAnchor;
            mStaticThings.I.LeftFingerPointerAnchor = act.LeftFingerPointerAnchor;
            mStaticThings.I.RightFingerPointerAnchor = act.RightFingerPointerAnchor;
            mStaticThings.I.MainVRROOT = act.MainVRROOT;
            mStaticThings.I.trackfix = act.Trackfix;
            mStaticThings.I.Photocamera = act.photocamera;
            StartTrackfixvector = mStaticThings.I.trackfix.localPosition;
        }
    }
}