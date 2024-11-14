using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKVrEvent : DllGenerateBase
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
            VSEngine.Instance.OnEventVrTriggerClick += OnTriggerClick;
            VSEngine.Instance.OnEventVrLeftTriggerUp += OnLeftTriggerUp;
            VSEngine.Instance.OnEventVrLeftTriggerDown += OnLeftTrggerDown;
            VSEngine.Instance.OnEventVrRightTriggerUp += OnRightTriggerUp;
            VSEngine.Instance.OnEventVrRightTriggerDown += OnRightTriggerDown;
            VSEngine.Instance.OnEventVrStartButtonClick += OnStartButtonClick;
            VSEngine.Instance.OnEventVrXButtonUp += OnXButtonUp;
            VSEngine.Instance.OnEventVrXButtonDown += OnXButtonDown;
            VSEngine.Instance.OnEventVrYButtonUp += OnYButtonUp;
            VSEngine.Instance.OnEventVrYButtonDown += OnYButtonDown;
            VSEngine.Instance.OnEventVrAButtonUp += OnAButtonUp;
            VSEngine.Instance.OnEventVrAButtonDown += OnAButtonDown;
            VSEngine.Instance.OnEventVrBButtonUp += OnBButtonUp;
            VSEngine.Instance.OnEventVrBButtonDown += OnBButtonDown;
            VSEngine.Instance.OnEventVrRightStickClick += OnRightStickClick;
            VSEngine.Instance.OnEventVrLeftStickClick += OnLeftStickClick;
            VSEngine.Instance.OnEventVrLeftGrabUp += OnLeftGrabUp;
            VSEngine.Instance.OnEventVrLeftGrabDown += OnLeftGrabDown;
            VSEngine.Instance.OnEventVrRightGrabUp += OnRightGrabUp;
            VSEngine.Instance.OnEventVrRightGrabDown += OnRightGrabDown;
            VSEngine.Instance.OnEventVrLeftStickLeft += OnLeftStickLeft;
            VSEngine.Instance.OnEventVrLeftStickRight += OnLeftStickRight;
            VSEngine.Instance.OnEventVrLeftStickUp += OnLeftStickUp;
            VSEngine.Instance.OnEventVrLeftStickDown += OnLeftStickDown;
            VSEngine.Instance.OnEventVrLeftStickRelease += OnLeftStickRelease;
            VSEngine.Instance.OnEventVrRightStickLeft += OnRightStickLeft;
            VSEngine.Instance.OnEventVrRightStickRight += OnRightStickRight;
            VSEngine.Instance.OnEventVrRightStickUp += OnRightStickUp;
            VSEngine.Instance.OnEventVrRightStickDown += OnRigtStickDown;
            VSEngine.Instance.OnEventVrRightStickRelease += OnRightStickRelease;
            VSEngine.Instance.OnEventVrLeftStickAxis += OnLeftStickAxis;
            VSEngine.Instance.OnEventVrRightStickAxis += OnRightStickAxis;
            VSEngine.Instance.OnEventVrLeftTriggerAxis += OnLeftTriggerAxis;
            VSEngine.Instance.OnEventVrRightTriggerAxis += OnRightTriggerAxis;
            VSEngine.Instance.OnEventVrLeftGrabAxis += OnLeftGrabAxis;
            VSEngine.Instance.OnEventVrRightGrabAxis += OnRightGrabAxis;
            VSEngine.Instance.OnEventVrDevicePutOn += OnDevicePuOn;
            VSEngine.Instance.OnEventVrDeviceTakeOff += OnDeviceTakeOff;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventVrTriggerClick -= OnTriggerClick;
            VSEngine.Instance.OnEventVrLeftTriggerUp -= OnLeftTriggerUp;
            VSEngine.Instance.OnEventVrLeftTriggerDown -= OnLeftTrggerDown;
            VSEngine.Instance.OnEventVrRightTriggerUp -= OnRightTriggerUp;
            VSEngine.Instance.OnEventVrRightTriggerDown -= OnRightTriggerDown;
            VSEngine.Instance.OnEventVrStartButtonClick -= OnStartButtonClick;
            VSEngine.Instance.OnEventVrXButtonUp -= OnXButtonUp;
            VSEngine.Instance.OnEventVrXButtonDown -= OnXButtonDown;
            VSEngine.Instance.OnEventVrYButtonUp -= OnYButtonUp;
            VSEngine.Instance.OnEventVrYButtonDown -= OnYButtonDown;
            VSEngine.Instance.OnEventVrAButtonUp -= OnAButtonUp;
            VSEngine.Instance.OnEventVrAButtonDown -= OnAButtonDown;
            VSEngine.Instance.OnEventVrBButtonUp -= OnBButtonUp;
            VSEngine.Instance.OnEventVrBButtonDown -= OnBButtonDown;
            VSEngine.Instance.OnEventVrRightStickClick -= OnRightStickClick;
            VSEngine.Instance.OnEventVrLeftStickClick -= OnLeftStickClick;
            VSEngine.Instance.OnEventVrLeftGrabUp -= OnLeftGrabUp;
            VSEngine.Instance.OnEventVrLeftGrabDown -= OnLeftGrabDown;
            VSEngine.Instance.OnEventVrRightGrabUp -= OnRightGrabUp;
            VSEngine.Instance.OnEventVrRightGrabDown -= OnRightGrabDown;
            VSEngine.Instance.OnEventVrLeftStickLeft -= OnLeftStickLeft;
            VSEngine.Instance.OnEventVrLeftStickRight -= OnLeftStickRight;
            VSEngine.Instance.OnEventVrLeftStickUp -= OnLeftStickUp;
            VSEngine.Instance.OnEventVrLeftStickDown -= OnLeftStickDown;
            VSEngine.Instance.OnEventVrLeftStickRelease -= OnLeftStickRelease;
            VSEngine.Instance.OnEventVrRightStickLeft -= OnRightStickLeft;
            VSEngine.Instance.OnEventVrRightStickRight -= OnRightStickRight;
            VSEngine.Instance.OnEventVrRightStickUp -= OnRightStickUp;
            VSEngine.Instance.OnEventVrRightStickDown -= OnRigtStickDown;
            VSEngine.Instance.OnEventVrRightStickRelease -= OnRightStickRelease;
            VSEngine.Instance.OnEventVrLeftStickAxis -= OnLeftStickAxis;
            VSEngine.Instance.OnEventVrRightStickAxis -= OnRightStickAxis;
            VSEngine.Instance.OnEventVrLeftTriggerAxis -= OnLeftTriggerAxis;
            VSEngine.Instance.OnEventVrRightTriggerAxis -= OnRightTriggerAxis;
            VSEngine.Instance.OnEventVrLeftGrabAxis -= OnLeftGrabAxis;
            VSEngine.Instance.OnEventVrRightGrabAxis -= OnRightGrabAxis;
            VSEngine.Instance.OnEventVrDevicePutOn -= OnDevicePuOn;
            VSEngine.Instance.OnEventVrDeviceTakeOff -= OnDeviceTakeOff;
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Update()
        {
            base.Update();

        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnTriggerClick()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnTriggerClick", InfoColor.green, 3, false);
            Vibrationinfo info = new Vibrationinfo()
            {
                hand = HandModelType.HandModelAll,
                frequency = 0.5f,
                lasttime = 3,
                amplitude = 0
            };
            VSEngine.Instance.SetVrHandVibration(info);
        }
        void OnLeftTriggerUp()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftTriggerUp", InfoColor.green, 3, false);
        }
        void OnLeftTrggerDown()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftTrggerDown", InfoColor.green, 3, false);
            VSEngine.Instance.SetVrResetSensor();
        }
        void OnRightTriggerUp()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightTriggerUp", InfoColor.green, 3, false);
        }
        void OnRightTriggerDown()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightTriggerDown", InfoColor.green, 3, false);
        }
        void OnStartButtonClick()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnStartButtonClick", InfoColor.green, 3, false);
        }
        void OnXButtonUp()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnXButtonUp", InfoColor.green, 3, false);
        }
        void OnXButtonDown()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnXButtonDown", InfoColor.green, 3, false);
        }
        void OnYButtonUp()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnYButtonUp", InfoColor.green, 3, false);
        }
        void OnYButtonDown()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnYButtonDown", InfoColor.green, 3, false);
        }
        void OnAButtonUp()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnAButtonUp", InfoColor.green, 3, false);
        }
        void OnAButtonDown()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnAButtonDown", InfoColor.green, 3, false);
        }
        void OnBButtonUp()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnBButtonUp", InfoColor.green, 3, false);
        }
        void OnBButtonDown()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnBButtonDown", InfoColor.green, 3, false);
        }
        void OnRightStickClick()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightStickClick", InfoColor.green, 3, false);
        }
        void OnLeftStickClick()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftStickClick", InfoColor.green, 3, false);
        }
        void OnLeftGrabUp()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftGrabUp", InfoColor.green, 3, false);
        }
        void OnLeftGrabDown()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftGrabDown", InfoColor.green, 3, false);
        }
        void OnRightGrabUp()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightGrabUp", InfoColor.green, 3, false);
        }
        void OnRightGrabDown()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightGrabDown", InfoColor.green, 3, false);
        }
        void OnLeftStickLeft()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftStickLeft", InfoColor.green, 3, false);
        }
        void OnLeftStickRight()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftStickRight", InfoColor.green, 3, false);
        }
        void OnLeftStickUp()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftStickUp", InfoColor.green, 3, false);
        }
        void OnLeftStickDown()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftStickDown", InfoColor.green, 3, false);
        }
        void OnLeftStickRelease()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftStickRelease", InfoColor.green, 3, false);
        }
        void OnRightStickLeft()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightStickLeft", InfoColor.green, 3, false);
        }
        void OnRightStickRight()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightStickRight", InfoColor.green, 3, false);
        }
        void OnRightStickUp()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightStickUp", InfoColor.green, 3, false);
        }
        void OnRigtStickDown()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRigtStickDown", InfoColor.green, 3, false);
        }
        void OnRightStickRelease()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightStickRelease", InfoColor.green, 3, false);
        }
        void OnLeftStickAxis(Vector2 axis)
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftStickAxis x:" + axis.x + " y:" + axis.y, InfoColor.green, 3, false);
        }
        void OnRightStickAxis(Vector2 axis)
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightStickAxis x:" + axis.x + " y:" + axis.y, InfoColor.green, 3, false);
        }
        void OnLeftTriggerAxis(Vector2 axis)
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftTriggerAxis x:" + axis.x + " y:" + axis.y, InfoColor.green, 3, false);
        }
        void OnRightTriggerAxis(Vector2 axis)
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightTriggerAxis x:" + axis.x + " y:" + axis.y, InfoColor.green, 3, false);
        }
        void OnLeftGrabAxis(Vector2 axis)
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnLeftGrabAxis x:" + axis.x + " y:" + axis.y, InfoColor.green, 3, false);
        }
        void OnRightGrabAxis(Vector2 axis)
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnRightGrabAxis x:" + axis.x + " y:" + axis.y, InfoColor.green, 3, false);
        }
        void OnDevicePuOn()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnDevicePuOn ", InfoColor.green, 10, false);
        }
        void OnDeviceTakeOff()
        {
            VSEngine.Instance.ShowRoomMarqueeLog("OnDeviceTakeOff ", InfoColor.green, 10, false);
        }
    }
}
