using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VSWorkSDK;

namespace Dll_Project
{
    public class SDKSystemExpand : DllGenerateBase
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
            VSEngine.Instance.OnEventSystemExpandEvent += OnSystemExpand;

        }
        public override void OnDisable()
        {
            base.OnDisable();
            VSEngine.Instance.OnEventSystemExpandEvent -= OnSystemExpand;

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
                VSEngine.Instance.SendSystemExpandEvent("ExpandEventKey",new List<object> {
                    "I AM OK",
                    666,
                    VSEngine.Instance.GetMyAvatarID(),
                    VSEngine.Instance.GetBigScreenViewRoot(),
                    VSEngine.Instance.GetBigScreenDefaultTexture()
                    });
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.LogError("ChangeAvatarWsSendFrame");
                VSEngine.Instance.SendSystemExpandEvent("ChangeAvatarWsSendFrame", new List<object> {
                    5
                    });
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.LogError("SetBackAvatarWsSendFrame");
                VSEngine.Instance.SendSystemExpandEvent("SetBackAvatarWsSendFrame", new List<object> {0 });
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        void OnSystemExpand(string eventname,List<object> eventparam)
        {
            Debug.Log("VSEngine OnSystemExpand eventname " + eventname + " param " + eventparam.Count);
            if (eventname == "ExpandEventKey")
            {
                if (eventparam.Count >= 5)
                {
                    string param1 = (string)eventparam[0];
                    int param2 = (int)eventparam[1];
                    string param3 = (string)eventparam[2];
                    Transform param4 = eventparam[3] as Transform;
                    Texture param5 = eventparam[4] as Texture;
                    string logparam = string.Format("VSEngine {0} {1} {2} {3} {4}x{5}", param1, param2, param3, param4.position, param5.width, param5.height);
                    Debug.Log(logparam);
                }
            }
        }
    }
}
