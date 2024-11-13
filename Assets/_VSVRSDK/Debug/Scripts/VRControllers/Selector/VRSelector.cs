using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace VSWorkSDK
{
    internal class VRSelector : MonoBehaviour
    {
        public GameObject positionMarker; // marker for display ground position
        private Transform bodyTransforn; // target transferred by teleport
        public LayerMask excludeLayers; // excluding for performance
        private LineRenderer arcRenderer;
        private bool displayActive = false; // don't update path when it's false.
        public RaycastHit[] hits;
        public GameObject LastPointObj;
        public PointerEventData pointer = new PointerEventData(EventSystem.current);
        public bool PointedUI = false;
        public bool IsMainScene = true;



        // Teleport target transform to ground positio

        // Active Teleporter Arc Path

        bool trigger = false;

        public void ToggleDisplay(bool active)
        {
            if (!active)
            {
                positionMarker.SetActive(false);
                displayActive = false;
                if (LastPointObj != null)
                {
                    MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointExit.ToString(), LastPointObj, 0);
                    //Debug.LogWarning("PointerExit : " + LastPointObj);
                    LastPointObj = null;
                }

                arcRenderer.enabled = false;
                hits = new RaycastHit[0];
            }
            else
            {
                displayActive = true;
            }
        }

        private void Awake()
        {
            arcRenderer = GetComponent<LineRenderer>();
            arcRenderer.enabled = false;
            positionMarker.SetActive(false);

        }
        private void Start()
        {
            if (mStaticThings.I != null)
            {
                mStaticThings.I.LaserPoint = positionMarker.transform;
            }

            MessageDispatcher.AddListener(CommonVREventType.VRRaw_RightTrigger.ToString(), (msg) =>
            {
                trigger = false;
            });

            MessageDispatcher.AddListener(CommonVREventType.VR_RightTriggerDown.ToString(), (msg) =>
            {
                trigger = true;
            });

        }

        private void Update()
        {
            if (displayActive)
            {
                UpdatePath();
            }
            else
            {
                arcRenderer.enabled = false;
                positionMarker.SetActive(false);

                //CurvedUIInputModule.CustomControllerRay = new Ray();
            }
        }



        private void UpdatePath()
        {
            if (mStaticThings.I == null)
            {
                return;
            }
            transform.position = mStaticThings.I.RightFingerPointerAnchor.position;
            transform.rotation = mStaticThings.I.RightFingerPointerAnchor.rotation;

            Ray myRay = new Ray(this.transform.position, this.transform.forward);

            //CurvedUIInputModule.CustomControllerRay = myRay;
            //CurvedUIInputModule.CustomControllerButtonState = trigger;

            Vector3 start = transform.position; // take off position
            Vector3 end;// = transform.position + transform.forward*100;
            Ray nr = new Ray(transform.position, transform.forward);
            //Debug.DrawRay(nr.origin, nr.direction, Color.blue);

            RaycastHit[] hits = Physics.RaycastAll(start, transform.forward, 300, ~excludeLayers);

            pointer = new PointerEventData(EventSystem.current);
            //Debug.LogWarning(hits.Length);
            if (hits.Length > 0)
            {
                //if(IsMainScene){
                //    GameManager.Instance.SelfDrawLineController.islaserOnPanorama = false;
                //}
                float min = hits[0].distance; //lets assume that the minimum is at the 0th place
                int minIndex = 0; //store the index of the minimum because thats hoow we can find our object

                for (int i = 1; i < hits.Length; ++i)// iterate from the 1st element to the last.(Note that we ignore the 0th element)
                {
                    if (hits[i].transform != gameObject.transform && hits[i].distance < min) //if we found smaller distance and its not the player we got a new minimum
                    {
                        min = hits[i].distance; //refresh the minimum distance value
                        minIndex = i; //refresh the distance
                    }
                }

                if (hits[minIndex].transform.gameObject.GetComponent<VRUIsystemMenuMark>())
                {
                    //if(IsMainScene){
                    //    GameManager.Instance.SelfDrawLineController.isLaserOnSystemMenu = true;
                    //}
                    end = hits[minIndex].point;
                }
                else
                {
                    end = hits[minIndex].point + hits[minIndex].normal * 0.01f;
                    //if(IsMainScene){
                    //    GameManager.Instance.SelfDrawLineController.isLaserOnSystemMenu = false;
                    //}
                }
                if (hits[minIndex].transform.gameObject != LastPointObj)
                {
                    if (LastPointObj != null)
                    {
                        //Debug.LogWarning("PointerExit : " + LastPointObj);
                        MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointExit.ToString(), LastPointObj, 0);

                        if (LastPointObj.GetComponent<VRUISelectorMark>())
                        {
                            ExecuteEvents.Execute(LastPointObj.gameObject, pointer, ExecuteEvents.pointerExitHandler);
                        }
                    }
                    LastPointObj = hits[minIndex].transform.gameObject;
                    MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointEnter.ToString(), LastPointObj, 0);

                    if (LastPointObj.GetComponent<VRUISelectorMark>())
                    {
                        ExecuteEvents.Execute(LastPointObj.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
                    }



                    //Debug.LogWarning("PointerEnter : " + LastPointObj);
                }
            }
            else
            {
                end = transform.position + transform.forward * 300;
                if (LastPointObj != null)
                {
                    MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointExit.ToString(), LastPointObj, 0);

                    if (LastPointObj.GetComponent<VRUISelectorMark>())
                    {
                        ExecuteEvents.Execute(LastPointObj.gameObject, pointer, ExecuteEvents.pointerExitHandler);
                    }
                    //Debug.LogWarning("PointerExit: " + LastPointObj);
                    LastPointObj = null;
                    MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointEnter.ToString(), LastPointObj, 0);
                }
                //if(IsMainScene){
                //    GameManager.Instance.SelfDrawLineController.isLaserOnSystemMenu = false;
                //    GameManager.Instance.SelfDrawLineController.islaserOnPanorama = true;
                //}

            }

            /*if (LastPointObj != null && LastPointObj.GetComponent<VRBigscreenMark>())
            {
                PointedUI = false;
                // if (LastPointObj.GetComponent<VRUISelectorMark>() && OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
                // {
                //     ExecuteEvents.Execute(LastPointObj.gameObject, pointer, ExecuteEvents.pointerClickHandler);
                // }
            }
            else */
            if (LastPointObj != null && LastPointObj.GetComponent<RectTransform>())
            {
                PointedUI = true;
                // if (LastPointObj.GetComponent<VRUISelectorMark>() && OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
                // {
                //     ExecuteEvents.Execute(LastPointObj.gameObject, pointer, ExecuteEvents.pointerClickHandler);
                // }
            }
            else
            {
                PointedUI = false;
            }




            positionMarker.transform.position = end;





            positionMarker.SetActive(true);

            arcRenderer.SetPosition(0, start);
            arcRenderer.SetPosition(1, end);

            float fix = mStaticThings.I.MainVRROOT.localScale.x;

            if (PointedUI)
            {
                arcRenderer.startWidth = 0.004f * fix;
                arcRenderer.endWidth = 0.002f * fix;
                positionMarker.transform.localScale = mStaticThings.I.MainVRROOT.lossyScale * 0.1f;
            }
            else
            {
                arcRenderer.startWidth = 0.004f * fix;
                arcRenderer.endWidth = 0.01f * fix;
                positionMarker.transform.localScale = mStaticThings.I.MainVRROOT.lossyScale;
            }

            arcRenderer.enabled = true;
        }




    }

}