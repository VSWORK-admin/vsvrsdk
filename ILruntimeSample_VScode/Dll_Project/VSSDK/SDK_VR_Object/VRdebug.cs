using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Dll_Project.SDK_VR_Object
{
   public  class VRdebug:DllGenerateBase
    {
        public Transform Content;
        public Text Text;
        public Scrollbar Vertical;
        public static VRdebug instance;
        public override void Awake()
        {
            instance = this;
            base.Awake();
        }
        public override void Init()
        {
            Content = BaseMono.ExtralDatas[0].Target;
            Text = Content.GetChild(0).gameObject.GetComponent<Text>();
            Vertical = BaseMono.ExtralDatas[1].Target.GetComponent<Scrollbar>();
            base.Init();
        }
        public override void Start()
        {
            base.Start();
        }
        public override void OnEnable()
        {
            base.OnEnable();
        }
        int index = 0;
        public override void Update()
        {
          
            base.Update();
        }
        public override void OnDisable()
        {
            base.OnDisable();
        }public void Log(string log)
        {
          //  BaseMono.StartCoroutine(Logdelay(log));
        }
        public void LogCtrl(string log)
        {
            BaseMono.StartCoroutine(Logdelay(log));
        }
        IEnumerator  Logdelay(string log)
        {
         
            yield return new WaitForSeconds(0.3f);
            Text.text = log;
            GameObject.Instantiate<GameObject>(Text.gameObject, Content);
            Vertical.value = 0;
        }

    }
}
