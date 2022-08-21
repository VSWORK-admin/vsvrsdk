using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slate;
using WebSocketSharp;
using System.Net;
using System.Threading;
using UnityEditor.PackageManager;

namespace Dll_Project { 
    public class TestSetLeftHand : DllGenerateBase
    {
        WebSocket WSClient;


        //Start a WebSocket Client
        public void StartWSClient()
        {
            WSClient = new WebSocket("ws://127.0.0.1:4060");

            WSClient.Connect();

            WSClient.OnMessage += (sender, e) =>
            {
                Debug.Log("WSclient RecieveMessage : " + e.Data.ToString());
            };

            WSClient.OnClose += (sender, e) => {
                //metricsws.Connect();
            };
            
            WSClient.OnError += (sender, e) => {
                //metricsws.Connect();
            };
        }


        public override void Init()
        {
            //StartWSClient();
            Debug.Log("TestMessageDispatcher Init !");  
        }
        public override void Awake()
        {
            Debug.Log("TestMessageDispatcher Awake !");
        }

        public override void Start()
        {
            BaseMono.ExtralDatas[1].Target.GetComponent<Cutscene>().Play();
            Cutscene.OnCutsceneStopped += TestSetLeftHand_OnStop;
            BaseMono.ExtralDatas[1].Target.GetComponent<Cutscene>().OnSectionReached += sectionreached;
            BaseMono.ExtralDatas[1].Target.GetComponent<Cutscene>().OnGlobalMessageSend += GlobalMessageSend;
            Debug.Log("TestMessageDispatcher Start !");
        }

        void GlobalMessageSend(string str, object obj) {
            Debug.LogWarning(str  + " " + obj.GetType());
        }

        void sectionreached(Section sc) {
            Debug.LogWarning(sc.time + "  " + sc.name);
        }

        private void TestSetLeftHand_OnStop(Cutscene scene)
        {
            scene.Play();
            Debug.LogWarning(scene.name + "Restart");
        }

        public override void OnEnable()
        {
            Debug.Log("TestMessageDispatcher OnEnable !");
        }

        public override void OnDisable()
        {
            Debug.Log("HoffixTestMono OnDisable !");
        }

        public override void Update()
        {
            if (mStaticThings.I != null && mStaticThings.I.isVRApp) {
                BaseMono.ExtralDatas[0].Target.gameObject.transform.position = mStaticThings.I.LeftHand.position;
            }
        }
        public override void OnTriggerEnter(Collider other)
        {
            Debug.LogWarning(other);
        }
    }
}