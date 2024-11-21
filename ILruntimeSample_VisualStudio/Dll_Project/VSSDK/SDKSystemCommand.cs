using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;
using VSWorkSDK.Enume;

namespace Dll_Project
{
    public class SDKSystemCommand : DllGenerateBase
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

        }
        public override void OnDisable()
        {
            base.OnDisable();

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
                VSEngine.Instance.ShowFps(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.ClearCacheDataFile( CacheDataFileType.AvatarFile);
                VSEngine.Instance.ClearCacheDataFile(CacheDataFileType.MediaFile);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                VSEngine.Instance.ClearSceneDataObject( SceneDataObjectType.GlbObject);
                VSEngine.Instance.ClearSceneDataObject(SceneDataObjectType.LaserDrawing);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                VSEngine.Instance.SetVrLaserEnable(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                VSEngine.Instance.SetVrLaserOpen(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                VSEngine.Instance.SetVrLaserOpen(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                VSEngine.Instance.SetRenderQuality( RenderQualityLevel.Middle);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                VSEngine.Instance.SetSendFrameRate( SyncDataFrameRate.Rate_10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                VSEngine.Instance.SetSystemMenuEnable(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                VSEngine.Instance.ShowMemoryUsed(true);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha0))
            {
                VSEngine.Instance.CleanCachesAndGC();
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.ShowAvatarSyncFrameRate(true);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha2))
            {
                VSEngine.Instance.KickOutSelectedUser();
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha3))
            {
                VSEngine.Instance.CancelAllDownloadOperate();
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha4))
            {
                VSEngine.Instance.CancelAllDownloadOperate();
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha5))
            {
                VSEngine.Instance.SetAvatarHeightFix( AvatarHeightFixType.AddFiveCM);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha6))
            {
                VSEngine.Instance.ShowAvatarNamePanel(false);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha7))
            {
                VSEngine.Instance.SetLaserPenEnable(false);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha8))
            {
                VSEngine.Instance.SetCameraDirectorMode(true);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha9))
            {
                VSEngine.Instance.SetLogerEnable(true);
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha0))
            {
                VSEngine.Instance.SetBigscreenGrabEnabled(true);
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.SetSelfAvatarHide(true);
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha2))
            {
                VSEngine.Instance.OpenLaserPenDraw();
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha3))
            {
                VSEngine.Instance.OpenSpacePenDraw();
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha4))
            {
                VSEngine.Instance.CloseLaserPenDraw();
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
