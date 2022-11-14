using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VSEngine
{
    public class SDKEngine : MonoBehaviour
    {
        public VsAuthority authority = new VsAuthority();
        public VsAvatar avatar = new VsAvatar();
        public VsBigScreen bigScreen = new VsBigScreen();
        public VsCache cache = new VsCache();
        public VsCamera vsCamera = new VsCamera();
        public VsChanel chanel = new VsChanel();
        public VsFile file = new VsFile();
        public VsVRInput input = new VsVRInput();
        public VsMessage message = new VsMessage();
        public VsObjectControl objectControl = new VsObjectControl();
        public VsVoiceRoom voiceRoom = new VsVoiceRoom();
        public VsScene scene = new VsScene();
        public VsVRSystem system = new VsVRSystem();
        public VsMenu menu = new VsMenu();
        public VsVRLaser laser = new VsVRLaser();
        public VsCloudRender cloudRender = new VsCloudRender();

        private static SDKEngine _Instance;
        public static SDKEngine Instance { get { return _Instance; } }

        private void Awake()
        {
            _Instance = this;
            foreach (var item in BaseFunction.BaseFunctions)
            {
                if(item.Value != null)
                {
                    item.Value.Awake();
                }
            }
        }

        private void Start()
        {
            foreach (var item in BaseFunction.BaseFunctions)
            {
                if (item.Value != null)
                {
                    item.Value.Start();
                }
            }
        }

        private void OnEnable()
        {
            foreach (var item in BaseFunction.BaseFunctions)
            {
                if (item.Value != null)
                {
                    item.Value.OnEnable();
                }
            }
        }

        private void OnDisable()
        {
            foreach (var item in BaseFunction.BaseFunctions)
            {
                if (item.Value != null)
                {
                    item.Value.OnDisable();
                }
            }
        }

        private void Update()
        {
            foreach (var item in BaseFunction.BaseFunctions)
            {
                if (item.Value != null)
                {
                    item.Value.Update();
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (var item in BaseFunction.BaseFunctions)
            {
                if (item.Value != null)
                {
                    item.Value.FixedUpdate();
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var item in BaseFunction.BaseFunctions)
            {
                if (item.Value != null)
                {
                    item.Value.OnDestroy();
                }
            }
            _Instance = null;
        }

        #region Event

        #endregion

        #region API

        /// <summary>
        /// 设置vr命令
        /// </summary>
        /// <param name="input"></param>
        public void SetVRInputField(InputField input)
        {
            MessageDispatcher.SendMessage(input, VrDispMessageType.InputFildClicked.ToString(), input.text, 0);
        }

        #endregion
    }

}
