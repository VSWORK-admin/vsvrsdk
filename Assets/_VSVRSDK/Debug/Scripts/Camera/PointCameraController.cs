using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using com.ootii.Messages;
namespace VSWorkSDK
{
    internal class PointCameraController : MonoBehaviour
    {
        private Vector3 oldMousePos;
        private Vector3 newMosuePos;
        private float rotSpeed = 0.09f;
        private Vector3 initPos = Vector3.zero;
        private Vector3 initRot = Vector3.zero;
        public Transform EyeCamera;
        public Transform RootObj;

        public ETCJoystick ej;

        bool pointcamenable = true;

        private void Start()
        {
            if (!mStaticThings.I.isVRApp || (mStaticThings.I.isVRApp && !mStaticThings.I.ismobile))
            {
                EyeCamera = mStaticThings.I.Maincamera;
                RootObj = mStaticThings.I.MainVRROOT;
                initPos = EyeCamera.position;
                initRot = EyeCamera.eulerAngles;
            }

        }

        void Update()
        {
            if (pointcamenable)
            {
                SuperViewMouse();
            }
            //Debug.LogWarning(Input.GetAxis("Horizontal1"));
        }



        public void SetPointCamEnable(bool en)
        {
            pointcamenable = en;


            if (en)
            {
                //EyeCamera.localEulerAngles = new Vector3(EyeCamera.localEulerAngles.x, EyeCamera.localEulerAngles.y, 0);
                RootObj.localEulerAngles = new Vector3(0, EyeCamera.localEulerAngles.y, 0);
                EyeCamera.localEulerAngles = new Vector3(EyeCamera.localEulerAngles.x, 0, 0);
                //SelfEyeCameraController.I.SetGamePadEnable(true);
            }
            else
            {
                mStaticThings.I.movingmarklist[0] = false;
            }
        }


        private void SuperViewMouse()
        {
            // if (Input.touchCount == 1)
            // {
            //     int fingerId = Input.GetTouch(0).fingerId;
            //     if (!EventSystem.current.IsPointerOverGameObject(fingerId) && !ej.isOnDrag)
            //     {
            //         newMosuePos = Input.GetTouch(0).position;
            //         Vector3 dis = newMosuePos - oldMousePos;
            //         float angleX = dis.x * 0.008f;//* Time.deltaTime
            //         float angleY = dis.y * 0.008f;//* Time.deltaTime
            //         EyeCamera.Rotate(new Vector3(-angleY, 0, 0), Space.Self);
            //         RootObj.Rotate(new Vector3(0, angleX, 0), Space.World);
            //         if (Input.GetTouch(0).phase == TouchPhase.Began)
            //         {
            //             oldMousePos = Input.GetTouch(0).position;
            //         }
            //     }
            // }
            // else if (Input.touchCount == 2)
            // {
            //     int findex = 1;
            //     if (Input.GetTouch(1).position.x > Input.GetTouch(0).position.x)
            //     {
            //         findex = 1;
            //     }
            //     else
            //     {
            //         findex = 0;
            //     }
            //     int fingerId = Input.GetTouch(findex).fingerId;
            //     if (!EventSystem.current.IsPointerOverGameObject(fingerId))
            //     {
            //         newMosuePos = Input.GetTouch(findex).position;
            //         Vector3 dis = newMosuePos - oldMousePos;
            //         float angleX = dis.x * 0.008f;//* Time.deltaTime
            //         float angleY = dis.y * 0.008f;//* Time.deltaTime
            //         EyeCamera.Rotate(new Vector3(-angleY, 0, 0), Space.Self);
            //         RootObj.Rotate(new Vector3(0, angleX, 0), Space.World);
            //         if (Input.GetTouch(findex).phase == TouchPhase.Began)
            //         {
            //             oldMousePos = Input.GetTouch(findex).position;
            //         }
            //     }
            // }
            // else 

            if (Input.touchCount == 0)
            {
                if (Input.GetMouseButton(1))
                {
                    mStaticThings.I.movingmarklist[0] = true;
                    newMosuePos = Input.mousePosition;
                    Vector3 dis = newMosuePos - oldMousePos;
                    float angleX = dis.x * rotSpeed;//* Time.deltaTime
                    float angleY = dis.y * rotSpeed;//* Time.deltaTime
                    Vector3 direct = new Vector3(-angleY, 0, 0);
                    EyeCamera.Rotate(direct, Space.Self);
                    RootObj.Rotate(new Vector3(0, angleX, 0), Space.World);
                    AvatarActionController.Instance.DoDirectAction(ETCAxis.DirectAction.Rotate, direct);
                }
                else
                {
                    mStaticThings.I.movingmarklist[0] = false;
                }
                oldMousePos = Input.mousePosition;

                if (Input.GetMouseButtonUp(1))
                {
                    AvatarActionController.Instance.DoDirectAction(ETCAxis.DirectAction.Rotate, Vector3.zero);
                }
            }
            else
            {
                mStaticThings.I.movingmarklist[0] = false;
            }
        }
    }
}