
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;
using VSWorkSDK.Data;
using UnityEngine.UI;

namespace Dll_Project.SDK_VR_Object
{
    enum SetPosType
    {
        None,
        Glb,
        Screen,
        Net
    }
    class SetPosition : DllGenerateBase
    {
        public Transform Mask;
        public Transform GlbLoadPos;
        public Transform ScreenLoadPos;
        public Transform NetLoadPos;


        bool synPos;
        public Transform MoveGameObjectPos;
        public SetPosType posType = SetPosType.None;
        private bool islocal;
        private bool isSending;
        Vector3 nowpos;
        Quaternion nowrot;
        Vector3 nowscal;
        public override void Init()
        {
            base.Init();
        }
        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {

            Mask = BaseMono.ExtralDatas[0].Target;
            GlbLoadPos = BaseMono.ExtralDatas[1].Target;
            ScreenLoadPos = BaseMono.ExtralDatas[2].Target;
            NetLoadPos = BaseMono.ExtralDatas[3].Target;
            for (int i = 0; i < BaseMono.ExtralDatas[4].Info.Length; i++)
            {
                BaseMono.ExtralDatas[4].Info[i].Target.GetComponent<Button>().onClick.AddListener(SetGlbEvent);
            }
            for (int i = 0; i < BaseMono.ExtralDatas[5].Info.Length; i++)
            {
                BaseMono.ExtralDatas[5].Info[i].Target.GetComponent<Button>().onClick.AddListener(SetScreenEvent);
            }
            for (int i = 0; i < BaseMono.ExtralDatas[6].Info.Length; i++)
            {
                BaseMono.ExtralDatas[6].Info[i].Target.GetComponent<Button>().onClick.AddListener(SetNetEvent);
            }
            ///设置Glb场景加载位置
            if (BaseMono.ExtralDatas[7].Target!=null)
            {
                mStaticThings.I.GlbSceneLoadPosition = BaseMono.ExtralDatas[7].Target.position;
                mStaticThings.I.GlbSceneLoadRotation = BaseMono.ExtralDatas[7].Target.eulerAngles;
                mStaticThings.I.GlbSceneLoadScale = BaseMono.ExtralDatas[7].Target.localScale.x;
            }
            else
            {
                mStaticThings.I.GlbSceneLoadPosition = BaseMono.ExtralDataInfos[0].vector3Data[0];
                mStaticThings.I.GlbSceneLoadScale = BaseMono.ExtralDataInfos[0].floatData;
            }
            ///设置Glb加载位置
            if (BaseMono.ExtralDatas[8].Target != null)
            {
                mStaticThings.I.GlbObjLoadPosition = BaseMono.ExtralDatas[8].Target.position;
                mStaticThings.I.GlbObjLoadRotation = BaseMono.ExtralDatas[8].Target.eulerAngles;
                mStaticThings.I.GlbObjLoadScale = BaseMono.ExtralDatas[8].Target.localScale.x;
            }
            else
            {
                mStaticThings.I.GlbObjLoadPosition = BaseMono.ExtralDataInfos[1].vector3Data[0];
                mStaticThings.I.GlbObjLoadScale = BaseMono.ExtralDataInfos[1].floatData;
            }
            base.Start();
        }
        public override void OnEnable()
        {
            VSEngine.Instance.OnEventPointClickHandler += VRPointClick;
            VSEngine.Instance.OnEventMovingObject += RecieveMovingObj;

        }
        float MaskClickDelayTime=0.6f;
        public override void Update()
        {
            if (Mask.gameObject.activeInHierarchy)
            {
                MaskClickDelayTime -= Time.deltaTime;
                Mask.position = mStaticThings.I.LaserPoint.position;
            }
            else
            {
                MaskClickDelayTime = 0.6f;
            }
            base.Update();
        }

        public override void OnDisable()
        {
            VSEngine.Instance.OnEventPointClickHandler -= VRPointClick;
            VSEngine.Instance.OnEventMovingObject -= RecieveMovingObj;

        }
        public void VRPointClick(GameObject gameObject)
        {
            if (gameObject==null)
            {
                return;
            }
            if (!Mask.gameObject.activeInHierarchy)
            {
                return;
            }
            if (MaskClickDelayTime>0)
            {
                return;
            }
            Mask.gameObject.SetActive(false);
            if (mStaticThings.I.isAdmin || mStaticThings.I.sadmin)
            {
                switch (posType)
                {
                    case SetPosType.None:
                        break;
                    case SetPosType.Glb:
                        GlbLoadPos.localPosition = mStaticThings.I.LaserPoint.position;
                        MoveGameObjectPos = GlbLoadPos;
                        mStaticThings.I.GlbObjLoadPosition = GlbLoadPos.transform.position;
                        mStaticThings.I.GlbObjLoadRotation = GlbLoadPos.transform.eulerAngles;
                        mStaticThings.I.GlbObjLoadScale = GlbLoadPos.transform.localScale.x;
                        synPos = true;
                        isSending = true;
                        SyncMoveObj();
                        break;
                    case SetPosType.Screen:
                        ScreenLoadPos.localPosition = mStaticThings.I.LaserPoint.position;
                        MoveGameObjectPos = ScreenLoadPos;
                        synPos = true;
                        isSending = true;
                        BigScreenSelectController bigScreenMark = ScreenLoadPos.GetComponent<BigScreenSelectController>();
                        WsBigScreen wbs = new WsBigScreen()
                        {
                            id = mStaticThings.I.mAvatarID,
                            enabled = true,
                            angle = bigScreenMark.ScreenAngle,
                            position = ScreenLoadPos.transform.position,
                            rotation = ScreenLoadPos.transform.rotation,
                            scale = bigScreenMark.startscale
                        };
                        VSEngine.Instance.SendSystemExpandEvent("BigScreenEndAnchor", new List<object> { wbs });
                      //  VSEngine.Instance.SetBigScreenProperty(wbs.position, wbs.rotation, wbs.scale, wbs.angle, wbs.enabled, 0);
                        break;
                    case SetPosType.Net:
                        NetLoadPos.parent.gameObject.SetActive(false);
                        NetLoadPos.localPosition = mStaticThings.I.LaserPoint.position;
                        MoveGameObjectPos = NetLoadPos;
                        synPos = true;
                        isSending = true;
                        break;
                    default:
                        break;
                }
            }

        }
        public void SetGlbEvent()
        {
            posType = SetPosType.Glb;
            SetMarkPos(posType);
        }
        public void SetScreenEvent()
        {
            posType = SetPosType.Screen;
            SetMarkPos(posType);
        }
        public void SetNetEvent()
        {
            posType = SetPosType.Net;
            SetMarkPos(posType);
        }
        public void SetMarkPos(SetPosType MarkType)
        {
            Mask.gameObject.SetActive(true);
        }
        void RecieveMovingObj(WsMovingObj newMovingObj)
        {
            //  VRdebug.instance.LogCtrl("RECIEVE+SyncMoveObj");
            if (mStaticThings.I == null) { return; }
            if (newMovingObj.id != mStaticThings.I.mAvatarID)
            {
                if (newMovingObj.name == GlbLoadPos.name)
                {
                    VRdebug.instance.LogCtrl("RECIEVE+SyncMoveObj+" + newMovingObj.name + newMovingObj.position.x + " " + newMovingObj.position.y + " " + newMovingObj.position.z);
                    RecieveMove(newMovingObj, GlbLoadPos);
                    mStaticThings.I.GlbSceneLoadPosition = newMovingObj.position;
                    mStaticThings.I.GlbSceneLoadScale = newMovingObj.scale.x;
                    mStaticThings.I.GlbObjLoadPosition = newMovingObj.position;
                    // mStaticThings.I.GlbObjLoadRotation = newMovingObj..eulerAngles;
                    mStaticThings.I.GlbObjLoadScale = newMovingObj.scale.x;
                }
                else if (newMovingObj.name == ScreenLoadPos.name)
                {
                    RecieveMove(newMovingObj, ScreenLoadPos);

                }
                else if (newMovingObj.name == NetLoadPos.name)
                {
                    RecieveMove(newMovingObj, NetLoadPos);

                }
            }
        }
        public void RecieveMove(WsMovingObj newMovingObj, Transform MovingObj)
        {
            if (newMovingObj.islocal)
            {
                MovingObj.transform.localPosition = newMovingObj.position;
                MovingObj.transform.localRotation = newMovingObj.rotation;
            }
            else
            {
                MovingObj.transform.position = newMovingObj.position;
                MovingObj.transform.rotation = newMovingObj.rotation;
            }
            VRdebug.instance.LogCtrl("RECIEVE+MovingObj+" + MovingObj.name + MovingObj.position.x + " " + MovingObj.position.y + " " + MovingObj.position.z);
            MovingObj.transform.localScale = newMovingObj.scale;
        }
        void MoveEnd()
        {
            posType = SetPosType.None;
            //  isSending = false;
            MoveGameObjectPos = null;
        }
        void SyncMoveObj()
        {
            Vector3 sendpos;
            Quaternion sendnowrot;
            Vector3 sendnowscal;
            if (islocal)
            {
                sendpos = MoveGameObjectPos.transform.localPosition;
                sendnowrot = MoveGameObjectPos.transform.localRotation;
            }
            else
            {
                sendpos = MoveGameObjectPos.transform.position;
                sendnowrot = MoveGameObjectPos.transform.rotation;
            }
            sendnowscal = MoveGameObjectPos.transform.localScale;

            if (sendpos != nowpos || sendnowrot != nowrot || sendnowscal != nowscal)
            {
                nowpos = sendpos;
                nowrot = sendnowrot;
                nowscal = sendnowscal;
                WsMovingObj sendMovingObj = new WsMovingObj()
                {
                    id = mStaticThings.I.mAvatarID,
                    name = MoveGameObjectPos.name,
                    islocal = islocal,
                    position = sendpos,
                    rotation = sendnowrot,
                    scale = sendnowscal
                };
                VSEngine.Instance.SendMovingObject(sendMovingObj.name, sendMovingObj.position, sendMovingObj.rotation, sendMovingObj.scale, sendMovingObj.islocal,  VSWorkSDK.Enume.MovingObjectMarkType.MovingObject, 0);
                VRdebug.instance.LogCtrl("SyncMoveObj");
                //MessageDispatcher.SendMessage(this, WsMessageType.SendMovingObj.ToString(), sendMovingObj, 0);
            }
        }
    }
}
