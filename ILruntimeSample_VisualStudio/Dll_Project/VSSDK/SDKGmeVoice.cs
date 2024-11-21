using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKGmeVoice : DllGenerateBase
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
            VSEngine.Instance.OnEventVoiceRoomInit += OnVoiceRoomInit;
            VSEngine.Instance.OnEventVoiceRoomConnected += OnVoiceConnected;
            VSEngine.Instance.OnEventVoiceRoomDisconnected += OnVoiceDisconnected;
            VSEngine.Instance.OnEventVoiceRoomExitEvent += OnVoiceRoomExit;
            VSEngine.Instance.OnEventVoiceMicrophoneVolumeChange += OnVoiceRecordingVolumeChange;
            VSEngine.Instance.OnEventVoiceLoudSpeakerVolumeChange += OnVoicePlaybackVolumeChange;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventVoiceRoomInit -= OnVoiceRoomInit;
            VSEngine.Instance.OnEventVoiceRoomConnected -= OnVoiceConnected;
            VSEngine.Instance.OnEventVoiceRoomDisconnected -= OnVoiceDisconnected;
            VSEngine.Instance.OnEventVoiceRoomExitEvent -= OnVoiceRoomExit;
            VSEngine.Instance.OnEventVoiceMicrophoneVolumeChange -= OnVoiceRecordingVolumeChange;
            VSEngine.Instance.OnEventVoiceLoudSpeakerVolumeChange -= OnVoicePlaybackVolumeChange;
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
                bool brecordingon = VSEngine.Instance.IsVoiceMicrophoneOn();
                bool brecordingenable = VSEngine.Instance.IsVoiceMicrophoneEnable();
                bool bplaybackon = VSEngine.Instance.IsVoiceLoudSpeakerOn();
                Debug.Log("VSEngine recording on " + brecordingon.ToString() + " recording enable " + 
                    brecordingenable.ToString() + " playback on " + bplaybackon.ToString());

                VSEngine.Instance.SetVoiceMicrophoneOn(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.SetVoiceMicrophoneOn(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                VSEngine.Instance.SetVoiceMicrophoneEnable(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                VSEngine.Instance.SetVoiceMicrophoneEnable(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                VSEngine.Instance.SetVoiceLoudSpeakerOn(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                VSEngine.Instance.SetVoiceLoudSpeakerOn(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                VSEngine.Instance.SetVoiceConnectToExRoom("test");
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                VSEngine.Instance.SetVoiceExitRoom();
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                VSEngine.Instance.SetVoiceRange(10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                VSEngine.Instance.SetAddVoiceLoudSpeakerVolume(10);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                VSEngine.Instance.SetMinusVoiceLoudSpeakerVolume(10);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                VSEngine.Instance.SetVoiceLoudSpeakerVolumeMax();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                VSEngine.Instance.SetAddMicrophoneVolume(10);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                VSEngine.Instance.SetMinusMicrophoneVolume(10);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                VSEngine.Instance.SetMicrophoneVolumeMax();
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnVoiceRoomInit(VRVoiceInitConfig config)
        {
            Debug.Log("VSEngine OnVoiceRoomInit config " + config.roomid);
        }
        void OnVoiceDisconnected(string roomid)
        {
            Debug.Log("VSEngine OnVoiceDisconnected roomid " + roomid);
        }
        void OnVoiceConnected(string roomid)
        {
            Debug.Log("VSEngine OnVoiceConnected roomid " + roomid);
        }
        void OnVoiceRoomExit()
        {
            Debug.Log("VSEngine OnVoiceRoomExit ");
        }
        void OnVoiceRecordingVolumeChange(int change)
        {
            Debug.Log("VSEngine OnVoiceRecordingVolumeChange change " + change);
        }
        void OnVoicePlaybackVolumeChange(int change)
        {
            Debug.Log("VSEngine OnVoicePlaybackVolumeChange change " + change);
        }
    }
}
