using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VSWorkSDK;

namespace Dll_Project.SDK_VR_Object
{
    public class SetVideoOrImage : DllGenerateBase
    {
        public Texture BigTexture;
        public GameObject Sphere;
        public Material Video;

        public override void Awake()
        {

            base.Awake();
        }
        public override void Init()
        {
            Sphere = BaseMono.ExtralDatas[0].Target.gameObject;
            Video= BaseMono.ExtralDataObjs[0].Target as Material;
            base.Init();
        }
        public override void Start()
        {
            VSEngine.Instance.OnEventBigScreenVideoFrameReady += OnEventBigScreenVideoFrameReady;
            VSEngine.Instance.OnEventBigScreenUpdateImage += OnEventBigScreenVideoFrameReady;
            VSEngine.Instance.OnEventBigScreenShowImage += OnEventBigScreenVideoFrameReady;
            VSEngine.Instance.OnEventSystemExpandEvent += Instance_OnEventSystemExpandEvent;
            base.Start();
        }

        private void Instance_OnEventSystemExpandEvent(string arg1, List<object> arg2)
        {

            switch (arg1)
            {
                case "BigScreenShowVideo":
                    Debug.Log("BigScreenShowVideo_Play" + BigTexture.width);
                    Texture temptex = arg2[0] as Texture;
                    if (temptex.width > 2160)
                    {
                        VRdebug.instance.LogCtrl("_MainTex" + BigTexture.width.ToString());
                        Sphere.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", temptex);
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnEventBigScreenVideoFrameReady(Texture2D tex)
        {
            if (tex!=null)
            {
                BigTexture = tex;
            
                if (BigTexture == null)
                {
                    return;
                }
                //  VRdebug.instance.LogCtrl(BigTexture.width.ToString());
                if (BigTexture.width > 2160)
                {
                    VRdebug.instance.LogCtrl("_MainTex" + BigTexture.width.ToString());
                    Video.SetTexture("_MainTex", BigTexture);
                    Sphere.GetComponent<MeshRenderer>().material = Video;
                }
            }
           
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void Update()
        {
            if (Input .GetKey(KeyCode.K))
            {
                Video.SetTexture("_MainTex", BigTexture);
                Sphere.GetComponent<MeshRenderer>().material = Video;
            }

            base.Update();
        }
        public override void OnDisable()
        {
            VSEngine.Instance.OnEventBigScreenVideoFrameReady -= OnEventBigScreenVideoFrameReady;
            VSEngine.Instance.OnEventBigScreenUpdateImage -= OnEventBigScreenVideoFrameReady;
            VSEngine.Instance.OnEventBigScreenShowImage -= OnEventBigScreenVideoFrameReady;
            base.OnDisable();
        }
        public void Log(string log)
        {
            //  BaseMono.StartCoroutine(Logdelay(log));
        }
        public void LogCtrl(string log)
        {
        }


    }
}
