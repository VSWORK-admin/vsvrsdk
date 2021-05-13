using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dll_Project { 
    public class TestSetLeftHand : DllGenerateBase
    {
        public override void Init()
        {
            Debug.Log("TestMessageDispatcher Init !");
        }
        public override void Awake()
        {
            Debug.Log("TestMessageDispatcher Awake !");
        }

        public override void Start()
        {
            Debug.Log("TestMessageDispatcher Start !");
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
            if (mStaticThings.I != null) {
                BaseMono.ExtralDatas[0].Target.gameObject.transform.position = mStaticThings.I.LeftHand.position;
            }
        }
        public override void OnTriggerEnter(Collider other)
        {
            Debug.LogWarning(other);
        }
    }
}