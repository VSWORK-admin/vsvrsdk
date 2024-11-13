using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using com.ootii.Messages;

namespace VSWorkSDK
{

    internal class VRScreenTouchSelectorController : MonoBehaviour
    {
        Camera FPSCamera;
        GameObject LastObj;
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        public LayerMask excludeLayers; // excluding for performance
        private void Start()
        {
            if (!mStaticThings.I.isVRApp || (mStaticThings.I.isVRApp && !mStaticThings.I.ismobile))
            {
                FPSCamera = mStaticThings.I.Maincamera.GetComponent<Camera>();
            }
        }


#if UNITY_ANDROID || UNITY_IPHONE
    void OnEnable()
    {
        if(!mStaticThings.I.isVRApp){
            IT_Gesture.onMultiTapE += OnMultiTap;
        }
        
        //IT_Gesture.onLongTapE += OnMultiTap;
    }

    void OnDisable()
    {
        if(!mStaticThings.I.isVRApp){
            IT_Gesture.onMultiTapE -= OnMultiTap;
        }
        //IT_Gesture.onLongTapE -= OnMultiTap;
    }
    void OnMultiTap(Tap tap)
    {
#if UNITY_STANDALONE_WIN
        //do a raycast base on the position of the tap
        Ray ray = FPSCamera.ScreenPointToRay(tap.pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~excludeLayers))
        {
            LastObj = hit.collider.gameObject;
            

            if (LastObj != null && !LastObj.GetComponent<NamePanelButtonMarker>() && !IsPointerOverGameObject(tap.pos))
            {
                if (LastObj.GetComponent<VRUISelectorMark>())
                {
                    ExecuteEvents.Execute(LastObj.gameObject, pointer, ExecuteEvents.pointerClickHandler);
                }
                MessageDispatcher.SendMessage(tap.pos, VRPointObjEventType.VRPointClick.ToString(), LastObj, 0);
            }
            if(LastObj.GetComponent<CurvedUI.CurvedUISettings>()){
                MessageDispatcher.SendMessage(tap.pos, VRPointObjEventType.VRPointClick.ToString(), LastObj, 0);
            }
        }
#endif
    }

    private bool IsPointerOverGameObject(Vector2 mousePosition)
    {       
        //创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击位置发射一条射线，检测是否点击UI
        EventSystem.current.RaycastAll(eventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

#else

            void Update()
        {
            if (mStaticThings.I.isVRApp || mStaticThings.I.IsThirdCamera)
            {
                return;
            }

            Ray ray;
            RaycastHit hit;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                pointer = new PointerEventData(EventSystem.current);
                ray = FPSCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~excludeLayers))
                {

                    if (hit.collider.gameObject != LastObj)
                    {
                        if (LastObj != null)
                        {
                            // Debug.LogWarning("Exit" + LastObj.gameObject);
                            MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointExit.ToString(), LastObj, 0);

                            if (LastObj.GetComponent<VRUISelectorMark>())
                            {
                                ExecuteEvents.Execute(LastObj.gameObject, pointer, ExecuteEvents.pointerExitHandler);
                            }
                        }
                        LastObj = hit.collider.gameObject;
                        //Debug.LogWarning("Enter" + LastObj.gameObject);
                        MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointEnter.ToString(), LastObj, 0);

                        if (LastObj.GetComponent<VRUISelectorMark>())
                        {
                            ExecuteEvents.Execute(LastObj.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
                        }
                    }
                }
                else
                {
                    if (LastObj != null)
                    {
                        //Debug.LogWarning("Exit" + LastObj.gameObject);
                        MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointExit.ToString(), LastObj, 0);

                        if (LastObj.GetComponent<VRUISelectorMark>())
                        {
                            ExecuteEvents.Execute(LastObj.gameObject, pointer, ExecuteEvents.pointerExitHandler);

                        }
                        LastObj = null;
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    CommonVREventController.I.mouseTrigger = 1f;
                    MessageDispatcher.SendMessage(this, CommonVREventType.VR_RightTriggerDown.ToString(), true, 0);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    CommonVREventController.I.mouseTrigger = 0f;
                    MessageDispatcher.SendMessage(this, CommonVREventType.VRCommitButtonClick.ToString(), true, 0);
                    MessageDispatcher.SendMessage(this, CommonVREventType.VRRaw_RightTrigger.ToString(), true, 0);

                    if (LastObj != null)
                    {
                        //Debug.LogWarning("CLICKED" + LastObj.gameObject);
                        if (LastObj.GetComponent<VRUISelectorMark>())
                        {
                            ExecuteEvents.Execute(LastObj.gameObject, pointer, ExecuteEvents.pointerClickHandler);
                        }
                        MessageDispatcher.SendMessage(Input.mousePosition, VRPointObjEventType.VRPointClick.ToString(), LastObj, 0);
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    ray = FPSCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~excludeLayers))
                    {
                        //if (hit.collider.gameObject.GetComponent<CurvedUI.CurvedUISettings>())
                        {
                            MessageDispatcher.SendMessage(Input.mousePosition, VRPointObjEventType.VRPointClick.ToString(), hit.collider.gameObject, 0);
                        }
                    }
                }
            }
        }
#endif
    }

}
