using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dll_Project { 
    public class TestSetLeftHand : DllGenerateBase
    {
        public override void Init()
        {
            base.Init();

            Debug.Log("TestMessageDispatcher Init !");
        }
        public override void Awake()
        {
            base.Awake();

            Debug.Log("TestMessageDispatcher Awake !");
        }

        public override void Start()
        {
            base.Start();
            Debug.Log("TestMessageDispatcher Start !");
            Debug.LogWarning(mStaticThings.I.LeftHand);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            Debug.Log("TestMessageDispatcher OnEnable !");
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Debug.Log("HoffixTestMono OnDisable !");
        }

        public override void Update()
        {
            base.Update();
            if (mStaticThings.I != null) {
                BaseMono.ExtralDatas[0].Target.gameObject.transform.position = mStaticThings.I.LeftHand.position;
            }
        }
        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            Debug.LogWarning(other);
        }

    }
}