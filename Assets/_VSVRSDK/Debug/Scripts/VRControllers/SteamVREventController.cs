using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR) && WITH_STEAMVR
using Valve.VR;
#endif

namespace VSWorkSDK
{
    internal class SteamVREventController : VREventControllerInterface
    {
        bool lstickmark = false;
        bool rstickmark = false;
        bool sendrstic = false;
        bool sendlstic = false;
        private float rtrigger = 0.0f;
        private float ltrigger = 0.0f;

        public int rightgrabstatus
        {
            get { return CommonVREventController.I.rightgrabstatus; }
            set { CommonVREventController.I.rightgrabstatus = value; }
        }
        public int leftgrabstatus
        {
            get { return CommonVREventController.I.leftgrabstatus; }
            set { CommonVREventController.I.leftgrabstatus = value; }
        }


#if (UNITY_STANDALONE_WIN || UNITY_EDITOR) && WITH_STEAMVR

        public SteamVR_Behaviour_Pose poseRight;
        public SteamVR_Behaviour_Pose poseLeft;
        public SteamVR_Action_Boolean commit_BT = SteamVR_Input.GetBooleanAction("commit");
        public SteamVR_Action_Single commitvector = SteamVR_Input.GetSingleAction("commitvector");
        public SteamVR_Action_Boolean B_BT = SteamVR_Input.GetBooleanAction("B");
        public SteamVR_Action_Boolean A_BT = SteamVR_Input.GetBooleanAction("A");
        public SteamVR_Action_Boolean X_BT = SteamVR_Input.GetBooleanAction("X");
        public SteamVR_Action_Boolean Y_BT = SteamVR_Input.GetBooleanAction("Y");
        public SteamVR_Action_Boolean menu_BT = SteamVR_Input.GetBooleanAction("menu");
        public SteamVR_Action_Boolean grab_BT = SteamVR_Input.GetBooleanAction("grab");
        public SteamVR_Action_Vector2 vector2_BT = SteamVR_Input.GetVector2Action("vector2");
        public SteamVR_Action_Boolean Teleport_BT = SteamVR_Input.GetBooleanAction("Teleport");


        public SteamVR_Action_Boolean Up_BT = SteamVR_Input.GetBooleanAction("Up");
        public SteamVR_Action_Boolean Down_BT = SteamVR_Input.GetBooleanAction("Down");
        public SteamVR_Action_Boolean Left_BT = SteamVR_Input.GetBooleanAction("Left");
        public SteamVR_Action_Boolean Right_BT = SteamVR_Input.GetBooleanAction("Right");
        public SteamVR_Action_Boolean TouchpadClick = SteamVR_Input.GetBooleanAction("touchpadclick");

        public override void Start()
        {
            poseRight = mStaticThings.I.RightHand.GetComponent<SteamVR_Behaviour_Pose>();
            poseLeft = mStaticThings.I.LeftHand.GetComponent<SteamVR_Behaviour_Pose>();
            MessageDispatcher.AddListener(CommonVREventType.VRrecenter.ToString(), (msg) =>
            {
            //Valve.VR.OpenVR.System.ResetSeatedZeroPose();
            //Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
            //SteamVR.instance.hmd.ResetSeatedZeroPose();

            XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);

                InputTracking.Recenter();
            });
        }

        public override void Update()
        {
            SteamVRInputDetect();
        }

        private void SteamVRInputDetect()
        {
            if (commit_BT.GetStateUp(poseRight.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VRCommitButtonClick.ToString(), true, 0);
                MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_RightTrigger.ToString(), true, 0);
            }

            if (commit_BT.GetStateDown(poseRight.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_RightTriggerDown.ToString(), true, 0);
            }

            if (menu_BT.GetStateUp(poseRight.inputSource) || menu_BT.GetStateUp(poseLeft.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_Start_ButtonClick.ToString(), true, 0);
            }

            if (B_BT.GetStateUp(poseRight.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_B_ButtonClick.ToString(), true, 0);
            }

            if (B_BT.GetStateDown(poseRight.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_B_ButtonDown.ToString(), true, 0);
            }

            if (X_BT.GetStateUp(poseLeft.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_X_ButtonClick.ToString(), true, 0);
            }

            if (X_BT.GetStateDown(poseLeft.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_X_ButtonDown.ToString(), true, 0);
            }

            if (Y_BT.GetStateUp(poseLeft.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_Y_ButtonClick.ToString(), true, 0);
            }

            if (Y_BT.GetStateDown(poseLeft.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_Y_ButtonDown.ToString(), true, 0);
            }

            if (A_BT.GetStateUp(poseRight.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_A_ButtonUp.ToString(), true, 0);
            }

            if (A_BT.GetStateDown(poseRight.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_A_ButtonDown.ToString(), true, 0);
            }

            if (Teleport_BT.GetStateUp(poseRight.inputSource))
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_Right_StickClick.ToString(), true, 0);
            }

            if (TouchpadClick.GetStateUp(poseRight.inputSource))
            {
                float newx = Get_VR_RightStickAxis().x;
                float newy = Get_VR_RightStickAxis().y;
                if (newx >= 0)
                {
                    if (Mathf.Abs(newy) <= newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VR_Right_StickClick.ToString(), true, 0);
                    }
                    else if (newy > newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_B_ButtonClick.ToString(), true, 0);
                    }
                    else
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_A_ButtonUp.ToString(), true, 0);
                    }
                }
                else
                {
                    if (Mathf.Abs(newy) <= -newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_X_ButtonClick.ToString(), true, 0);
                    }
                    else if (newy > newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_B_ButtonClick.ToString(), true, 0);
                    }
                    else
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_A_ButtonUp.ToString(), true, 0);
                    }
                }
            }

            if (TouchpadClick.GetStateDown(poseRight.inputSource))
            {
                float newx = Get_VR_RightStickAxis().x;
                float newy = Get_VR_RightStickAxis().y;
                if (newx >= 0)
                {
                    if (Mathf.Abs(newy) <= newx)
                    {
                        //MessageDispatcher.SendMessage(this, CommonVREventType.VR_RRelease.ToString(), true, 0);
                    }
                    else if (newy > newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_B_ButtonDown.ToString(), true, 0);
                    }
                    else
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_A_ButtonDown.ToString(), true, 0);
                    }
                }
                else
                {
                    if (Mathf.Abs(newy) <= -newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_X_ButtonDown.ToString(), true, 0);
                    }
                    else if (newy > newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_B_ButtonDown.ToString(), true, 0);
                    }
                    else
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_A_ButtonDown.ToString(), true, 0);
                    }
                }
            }

            if (TouchpadClick.GetStateUp(poseLeft.inputSource))
            {
                float newx = Get_VR_LeftStickAxis().x;
                float newy = Get_VR_LeftStickAxis().y;
                if (newx >= 0)
                {
                    if (Mathf.Abs(newy) <= newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VR_Left_StickClick.ToString(), true, 0);
                    }
                    else if (newy > newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_Y_ButtonClick.ToString(), true, 0);
                    }
                    else
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_X_ButtonClick.ToString(), true, 0);
                    }
                }
                else
                {
                    if (Mathf.Abs(newy) <= -newx)
                    {
                        //MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_X_ButtonClick.ToString(), true, 0);
                    }
                    else if (newy > newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_Y_ButtonClick.ToString(), true, 0);
                    }
                    else
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_X_ButtonClick.ToString(), true, 0);
                    }
                }
            }

            if (TouchpadClick.GetStateDown(poseLeft.inputSource))
            {
                float newx = Get_VR_LeftStickAxis().x;
                float newy = Get_VR_LeftStickAxis().y;
                if (newx >= 0)
                {
                    if (Mathf.Abs(newy) <= newx)
                    {
                        //MessageDispatcher.SendMessage(this, CommonVREventType.VR_Left_StickClick.ToString(), true, 0);
                    }
                    else if (newy > newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_Y_ButtonDown.ToString(), true, 0);
                    }
                    else
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_X_ButtonDown.ToString(), true, 0);
                    }
                }
                else
                {
                    if (Mathf.Abs(newy) <= -newx)
                    {
                        //MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_X_ButtonClick.ToString(), true, 0);
                    }
                    else if (newy > newx)
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_Y_ButtonDown.ToString(), true, 0);
                    }
                    else
                    {
                        MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_X_ButtonDown.ToString(), true, 0);
                    }
                }
            }




            if (grab_BT.GetStateUp(poseRight.inputSource))
            {
                rightgrabstatus = 0;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_Right_GrabUp.ToString(), true, 0);
            }

            if (grab_BT.GetStateDown(poseRight.inputSource))
            {
                rightgrabstatus = 1;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_Right_GrabDown.ToString(), true, 0);
            }



            if (grab_BT.GetStateUp(poseLeft.inputSource))
            {
                leftgrabstatus = 0;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_Left_GrabUp.ToString(), true, 0);
            }

            if (grab_BT.GetStateDown(poseLeft.inputSource))
            {
                leftgrabstatus = 1;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_Left_GrabDown.ToString(), true, 0);
            }

            GetSteamAxis();

            if (Up_BT.GetStateDown(poseRight.inputSource))
            {
                rstickmark = true;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_RUp.ToString(), true, 0);
            }
            if (Down_BT.GetStateDown(poseRight.inputSource))
            {
                rstickmark = true;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_RDown.ToString(), true, 0);
            }
            if (Left_BT.GetStateDown(poseRight.inputSource))
            {
                rstickmark = true;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_RLeft.ToString(), true, 0);
            }
            if (Right_BT.GetStateDown(poseRight.inputSource))
            {
                rstickmark = true;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_RRight.ToString(), true, 0);
            }

            if (rstickmark)
            {
                if (Get_VR_RightStickAxis().x == 0 && Get_VR_RightStickAxis().y == 0)
                {
                    rstickmark = false;
                    MessageDispatcher.SendMessage(this, CommonVREventType.VR_RRelease.ToString(), true, 0);
                }
            }


            if (Up_BT.GetStateDown(poseLeft.inputSource))
            {
                lstickmark = true;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_LUp.ToString(), true, 0);
            }
            if (Down_BT.GetStateDown(poseLeft.inputSource))
            {
                lstickmark = true;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_LDown.ToString(), true, 0);
            }
            if (Left_BT.GetStateDown(poseLeft.inputSource))
            {
                lstickmark = true;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_LLeft.ToString(), true, 0);
            }
            if (Right_BT.GetStateDown(poseLeft.inputSource))
            {
                lstickmark = true;
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_LRight.ToString(), true, 0);
            }

            if (lstickmark)
            {
                if (Get_VR_LeftStickAxis().x == 0 && Get_VR_LeftStickAxis().y == 0)
                {
                    lstickmark = false;
                    MessageDispatcher.SendMessage(this, CommonVREventType.VR_LRelease.ToString(), true, 0);
                }
            }
        }


        private void GetSteamAxis()
        {

            if (Mathf.Abs(Get_VR_LeftStickAxis().x) > 0.1 || Mathf.Abs(Get_VR_LeftStickAxis().y) > 0.1)
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_LefStickAxis.ToString(), Get_VR_LeftStickAxis(), 0);
            }


            if (Mathf.Abs(Get_VR_RightStickAxis().x) > 0.1 || Mathf.Abs(Get_VR_RightStickAxis().y) > 0.1)
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_RightStickAxis.ToString(), Get_VR_RightStickAxis(), 0);
            }

            if (GetVR_LeftTriggerAxis() > 0)
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_LeftTriggerAxis.ToString(), GetVR_LeftTriggerAxis(), 0);
            }

            if (GetVR_RightTriggerAxis() > 0)
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_RightTriggerAxis.ToString(), GetVR_RightTriggerAxis(), 0);
            }

            if (GetVR_LeftGrabAxis() > 0)
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_LeftGrabAxis.ToString(), GetVR_LeftGrabAxis(), 0);
            }

            if (GetVR_RightGrabAxis() > 0)
            {
                MessageDispatcher.SendMessage(this, CommonVREventType.VR_RightGrabAxis.ToString(), GetVR_RightGrabAxis(), 0);
            }
        }

        public override Vector2 Get_VR_LeftStickAxis()
        {
            return vector2_BT.GetAxis(poseLeft.inputSource);
        }

        public override Vector2 Get_VR_RightStickAxis()
        {
            return vector2_BT.GetAxis(poseRight.inputSource);
        }

        public override float GetVR_LeftTriggerAxis()
        {
            return commitvector.GetAxis(poseLeft.inputSource);
        }

        public override float GetVR_RightTriggerAxis()
        {
            return commitvector.GetAxis(poseRight.inputSource);
        }
#endif
    }

}