using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;
using VSWorkSDK.Enume;

namespace Dll_Project
{
    public class SDKMusicVedio : DllGenerateBase
    {
        string musicvediourl = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=8b05778U8migefd_gZ-8PY8DjE9g3EaBWG0vzYRMcRPXR5yvzu7vp5sEwiI7ukxGClrlywvUh1xLF3NgTIKBnNtul7dK2mCj8CqPZkuztY1Uc21G29pXQ7pWRw_2c6R9Dw&file_name=/%E6%B5%B7%E6%B4%8B.mp4";
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
            VSEngine.Instance.OnEventReceiveMusicVedioInfo += OnMusicVedioInfo;

        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventReceiveMusicVedioInfo -= OnMusicVedioInfo;

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
                VSEngine.Instance.RequestMusicVedioInfo((info)=> {
                    Debug.Log("SDKEngine RequestMusicVedioInfo " + info);
                });
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.SetMusicVedioAudioDualMonoMode(AudioDualMonoModeType.AUDIO_DUAL_MONO_L);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                VSEngine.Instance.PlayMusicVedio(musicvediourl, true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                VSEngine.Instance.PauseMusicVedio();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                VSEngine.Instance.ResumeMusicVedio();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                VSEngine.Instance.StopMusicVedio();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                VSEngine.Instance.AdjustMusicVedioVolume(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                VSEngine.Instance.AdjustRemoteMusicVedioVolume(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                VSEngine.Instance.SeekMusicVedioPosition(0);
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnMusicVedioInfo(string info)
        {
            Debug.Log("VSEngine OnMusicVedioInfo info " + info);
        }
    }
}
