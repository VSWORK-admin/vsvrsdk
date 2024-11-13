using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSWorkSDK
{
    internal class VREventControllerInterface
    {
        public virtual void Awake() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        // Start is called before the first frame update
        public virtual void Start() { }

        // Update is called once per frame
        public virtual void Update() { }

        public virtual float GetVR_RightGrabAxis()
        {
            return 0.0f;
        }
        public virtual float GetVR_LeftGrabAxis()
        {
            return 0.0f;
        }
        public virtual float GetVR_RightTriggerAxis()
        {
            return 0.0f;
        }
        public virtual float GetVR_LeftTriggerAxis()
        {
            return 0.0f;
        }
        public virtual Vector2 Get_VR_RightStickAxis()
        {
            return Vector2.zero;
        }
        public virtual Vector2 Get_VR_LeftStickAxis()
        {
            return Vector2.zero;
        }
    }
}