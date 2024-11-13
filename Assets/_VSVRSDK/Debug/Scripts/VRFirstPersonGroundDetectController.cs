using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
namespace VSWorkSDK
{
    public class VRFirstPersonGroundDetectController : MonoBehaviour
    {
        public LayerMask excludeLayers; // excluding for performance
        public GameObject LastPointObj;
        float nowscale;
        public bool SetETCSpeed = false;
        private static VRFirstPersonGroundDetectController instance;

        public static VRFirstPersonGroundDetectController I
        {
            get
            {
                return instance;
            }
        }
        private void Awake()
        {
            instance = this;

        }

        private void Start()
        {
            MessageDispatcher.AddListener(VrDispMessageType.RoomConnected.ToString(), (msg) =>
            {
                LastPointObj = null;
            });
        }

        public bool detectWhileOnline = true;
        // Update is called once per frame
        private void LateUpdate()
        {

            //if (detectWhileOnline /*&& !GameDataManager.socketIOData.isonline && !GameDataManager.socketIOData.IsOnOffline*/) {
            //return;
            //}

            if (SetETCSpeed && Mathf.Abs(nowscale - mStaticThings.I.MainVRROOT.lossyScale.x) > 0.01f)
            {
                MessageDispatcher.SendMessageData(VrDispMessageType.VRScaleChange.ToString(), nowscale);
                Debug.LogWarning("nowscale :" + nowscale);
                //SelfEyeCameraController.I.walkmanager.GetComponent<ETCJoystick>().axisX.speed = 3.5f * nowscale;
                //SelfEyeCameraController.I.walkmanager.GetComponent<ETCJoystick>().axisY.speed = 3.5f * nowscale;
                //SelfEyeCameraController.I.jumpmanager.GetComponent<ETCButton>().axis.speed = 4f * nowscale;
                //SelfEyeCameraController.I.jumpmanager.GetComponent<ETCButton>().axis.gravity = 9.8f * nowscale;
            }
            nowscale = mStaticThings.I.MainVRROOT.lossyScale.x;
            transform.position = mStaticThings.I.Maincamera.position;
            Vector3 start = transform.position; // take off position
            RaycastHit[] hits = Physics.RaycastAll(start, -transform.up, 2.6f * nowscale, ~excludeLayers);
            if (hits.Length > 0)
            {
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

                if (hits[minIndex].transform.gameObject != LastPointObj)
                {
                    LastPointObj = hits[minIndex].transform.gameObject;
                    MessageDispatcher.SendMessage(this, VrDispMessageType.TelePortToMesh.ToString(), LastPointObj.name, 0);
                }
            }
            else
            {
                if (LastPointObj != null)
                {
                    MessageDispatcher.SendMessage(this, VrDispMessageType.TelePortToMesh.ToString(), "", 0);
                    LastPointObj = null;
                }
            }
        }
    }
}