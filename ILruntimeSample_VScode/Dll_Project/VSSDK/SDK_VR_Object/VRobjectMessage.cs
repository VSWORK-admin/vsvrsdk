using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;
using UnityEngine.UI;
namespace Dll_Project.SDK_VR_Object
{
    class VRobjectMessage : DllGenerateBase
    {
        public GameObject All_Scene;
        public float Rotation;
        public Transform Rotate;
        public GameObject SkySphere;
        public GameObject _NetRoot;
        // Start is called before the first frame update
        public override void Start()
        {
      

               All_Scene = BaseMono.ExtralDatas[0].Target.gameObject;
            Rotate = BaseMono.ExtralDatas[1].Target;
            SkySphere = BaseMono.ExtralDatas[2].Target.gameObject;
            _NetRoot = BaseMono.ExtralDatas[3].Target.gameObject;
        }
        public override void OnEnable()
        {
            BaseMono.StartCoroutine(delay(2));

        
            VSEngine.Instance.OnEventReceiveRoomSyncData += Instance_OnEventReceiveRoomSyncData;
        }
     public  IEnumerator delay(float time)
        {
            yield return new WaitForSeconds(time);
            VRdebug.instance.Log("VRobjectMessage");
        }
        private void VRRawRightTriggerUp()
        {
            VRdebug.instance.Log("VRRawRightTriggerUp");
        }

        private void VRRawRightTriggerDown()
        {
            VRdebug.instance.Log("VRRawRightTriggerDown");
        }

        public override void OnDisable()
        {
          
            VSEngine.Instance.OnEventReceiveRoomSyncData -= Instance_OnEventReceiveRoomSyncData;
        }
        private void Instance_OnEventReceiveRoomSyncData(VSWorkSDK.Data.RoomSycnData obj)
        {
            switch (obj.a)
            {
                case "showstage":
                    Show();
                    break;
                case "hidestage":
                    Hide();
                    break;
                case "showpano":
                    ShowPano();
                    break;
                case "hidepano":
                    HidePano();
                    break;
                case "rrpano":
                    RotationRightPano();
                    break;
                case "rlpano":
                    RotationleftPano();
                    break;
                case "resetpano":
                    ResetPano();
                    break;
                case "showglb":
                    ShowGlb();
                    break;
                case "resetglb":
                    ResetGlb();
                    break;
                case "hideglb":
                    HideGlb();
                    break;
                case "shownet":
                    ShowNet();
                    break;
                case "hidenet":
                    HideNet();
                    break;
                default:
                    break;
            }
        }
        void Show()
        {
            All_Scene.SetActive(true);

        }
        void Hide()
        {
            All_Scene.SetActive(false);
        }
        void ShowPano()
        {
            SkySphere.gameObject.SetActive(true);
        }
        void HidePano()
        {
            SkySphere.gameObject.SetActive(false);
        }
        void RotationRightPano()
        {
            Rotate.DORotate(new Vector3(Rotate.transform.rotation.x, Rotate.transform.rotation.y + 30, Rotate.transform.rotation.z), 0.1f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear);

        }
        void RotationleftPano()
        {
            Rotate.DORotate(new Vector3(Rotate.transform.rotation.x, Rotate.transform.rotation.y - 30, Rotate.transform.rotation.z), 0.1f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear);
        }
        void ResetPano()
        {
            Rotate.rotation = new Quaternion(0, 0, 0, 0);
        }
        void ShowGlb()
        {
            if (mStaticThings.I != null)
            {
                GameObject obj = mStaticThings.I.GlbRoot.gameObject;
                obj.SetActive(true);
            }
        }
        void ResetGlb()
        {
            mStaticThings.I.GlbRoot.gameObject.BroadcastMessage("resetall", SendMessageOptions.DontRequireReceiver);
        }
        void HideGlb()
        {
            if (mStaticThings.I != null)
            {
                GameObject obj = mStaticThings.I.GlbRoot.gameObject;
                obj.SetActive(false);
            }
        }
        void ShowNet()
        {
            _NetRoot.SetActive(true);
        }
        void HideNet()
        {
            _NetRoot.SetActive(false);
        }


        // Update is called once per frame
        public override void Update()
        {

        }

    }
}
