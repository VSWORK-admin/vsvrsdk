using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKAvatar : DllGenerateBase
    {
        bool bforcehideselfmodel = false;
        bool bvisiblealltime = false;
        string chooseavatar = "";
        private SDKAvatar instance = null;
        private int index = 0;
        public override void Init()
        {
            base.Init();
            instance = this;
        }
        public override void Awake()
        {
            base.Awake();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            VSEngine.Instance.OnEventRoomInitAvatar += OnRoomInitAvatar;
            VSEngine.Instance.OnEventRoomEnterNewAvatar += OnRoomEnterNewAvatar;
            VSEngine.Instance.OnEventAvatarLoadDone += OnAvatarLoadDone;
            VSEngine.Instance.OnEventDestroyAvatar += OnDestroyAvatar;
            VSEngine.Instance.OnEventDestroyAvatarModel += OnDestroyAvatarModel;
            VSEngine.Instance.OnEventLoadedFbxAvatarModel += OnLoadFfbAvatarModel;
            VSEngine.Instance.OnEventPointClickHandler +=OnPointClick;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventRoomInitAvatar -= OnRoomInitAvatar;
            VSEngine.Instance.OnEventRoomEnterNewAvatar -= OnRoomEnterNewAvatar;
            VSEngine.Instance.OnEventAvatarLoadDone -= OnAvatarLoadDone;
            VSEngine.Instance.OnEventDestroyAvatar -= OnDestroyAvatar;
            VSEngine.Instance.OnEventDestroyAvatarModel -= OnDestroyAvatarModel;
            VSEngine.Instance.OnEventLoadedFbxAvatarModel -= OnLoadFfbAvatarModel;
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
                VSEngine.Instance.SetHideAvatarsExeptAdmin();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                VSEngine.Instance.SetForceHideAllAvatars(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                VSEngine.Instance.SetForceHideAllAvatars(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                List<string> selected = new List<string>();
                selected.Add(chooseavatar);
                VSEngine.Instance.SetForceHideSelectAvatars(selected,true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                List<string> selected = new List<string>();
                selected.Add(chooseavatar);
                VSEngine.Instance.SetForceHideSelectAvatars(selected, false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                VSEngine.Instance.SetForceHideAvatar(chooseavatar,true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                VSEngine.Instance.SetForceHideAvatar(chooseavatar, false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                bvisiblealltime = !bvisiblealltime;
                VSEngine.Instance.SetSelfVisibleAllTime(bvisiblealltime);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                bforcehideselfmodel = !bforcehideselfmodel;
                VSEngine.Instance.SetForceHideSelfAvatarModel(bforcehideselfmodel);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                string aid = VSEngine.Instance.GetMyAvatarModelID();
                Debug.Log("SDKEngine LoadFbxAvatarModel aid = " + aid);
                VSEngine.Instance.LoadFbxAvatarModel(aid);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
            {
                VSEngine.Instance.ClearFbxAvatarModelResource(VSEngine.Instance.GetMyAvatarModelID());
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnRoomInitAvatar(WsAvatarFrame avatardata)
        {
            Debug.Log("VSEngine OnRoomInitAvatar avatar " + avatardata.id);
        }
        void OnAvatarLoadDone(Transform avatar)
        {
            Debug.Log("VSEngine OnAvatarLoadDone name " + avatar.name);
        }
        void OnRoomEnterNewAvatar(WsAvatarFrame avatardata)
        {
            Debug.Log("VSEngine OnRoomEnterNewAvatar avatar " + avatardata.id);
        }
        void OnDestroyAvatar(string id)
        {
            Debug.Log("VSEngine OnDestroyAvatar id " + id);
        }
        void OnDestroyAvatarModel(GameObject model)
        {
            Debug.Log("VSEngine OnDestroyAvatarModel model " + model.name);
        }
        void OnLoadFfbAvatarModel(LoadAvatarOBJ data)
        {
            Debug.Log("VSEngine OnLoadFfbAvatarModel data " + data.avatarModel.name);
        }
        void OnPointClick(GameObject obj)
        {
            Text tx = obj.GetComponentInChildren<Text>();
            List<string> allname = VSEngine.Instance.GetAllActiveAvatarName();
            
            if (tx != null && allname.Contains(tx.text))
            {
                Dictionary<string,WsAvatarFrame> alldata = VSEngine.Instance.GetAllAvatarData();
                foreach(var v in alldata)
                {
                    if (v.Value.name == tx.text)
                    {
                        chooseavatar = v.Value.id;
                        Debug.Log("SDKEngine Choose Avatar id = " + chooseavatar);
                    }
                }
            }
            instance.index++;
            Debug.Log("SDKEngine SDKAvatar index = " + index);
            OnDestroyAvatar(VSEngine.Instance.GetMyAvatarID());
        }
    }
}
