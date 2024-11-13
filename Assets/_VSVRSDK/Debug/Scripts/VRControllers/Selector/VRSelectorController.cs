using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
namespace VSWorkSDK
{
    internal class VRSelectorController : MonoBehaviour
    {
        public bool selectorshow = false;
        public bool isTeleshow = false;

        public bool SelectorCtlEnabled = true;

        public LineRenderer localline;
        public MeshRenderer localball;

        public bool isMainScene = true;

        // Start is called before the first frame update
        private void Awake()
        {

        }
        private void Start()
        {
            MessageDispatcher.AddListener(VrDispMessageType.SetAdmin.ToString(), SetAdmin);
            if (isMainScene)
            {
                MessageDispatcher.AddListener(VrDispMessageType.TeleporterStatusChange.ToString(), TeleporterStatusChange, true);
            }
            MessageDispatcher.AddListener(VrDispMessageType.RoomDisConnected.ToString(), (msg) =>
            {
                SelectorCtlEnabled = true;
                OpenLaser(true);
            });
            if (mStaticThings.I != null && !mStaticThings.I.isVRApp)
            {
                SelectorCtlEnabled = false;
                OpenLaser(false);
            }
            else
            {
                SelectorCtlEnabled = true;
                OpenLaser(true);
            }
        }

        void SetAdmin(IMessage msg)
        {
            if ((bool)msg.Data)
            {
                SelectorCtlEnabled = true;
            }
        }

        public void DubbleClickOpenLaser(bool en)
        {
            if (SelectorCtlEnabled)
            {
                if ((en && !selectorshow) || (!en && selectorshow))
                {
                    OpenLaser(en);
                }
            }
        }


        public void OpenLaser(bool en)
        {
            if (mStaticThings.I == null)
            {
                return;
            }
            if (en)
            {
                if (!mStaticThings.I.isVRApp)
                {
                    return;
                }
                selectorshow = true;
                GetComponent<VRSelector>().ToggleDisplay(true);
                mStaticThings.I.SelectorEnabled = true;
                MessageDispatcher.SendMessage(this, VrDispMessageType.SelectorPointerStatusChange.ToString(), true, 0);
            }
            else
            {
                selectorshow = false;
                GetComponent<VRSelector>().ToggleDisplay(false);
                mStaticThings.I.SelectorEnabled = false;
                MessageDispatcher.SendMessage(this, VrDispMessageType.SelectorPointerStatusChange.ToString(), false, 0);
            }
        }

        public void EnableLaser(bool en)
        {
            if (en)
            {
                SelectorCtlEnabled = true;
            }
            else
            {
                selectorshow = false;
                GetComponent<VRSelector>().ToggleDisplay(false);
                mStaticThings.I.SelectorEnabled = false;
                SelectorCtlEnabled = false;
                MessageDispatcher.SendMessage(this, VrDispMessageType.SelectorPointerStatusChange.ToString(), false, 0);
            }
        }

        void TeleporterStatusChange(IMessage msg)
        {
            isTeleshow = (bool)msg.Data;
            if (isTeleshow)
            {
                if (selectorshow)
                {
                    GetComponent<VRSelector>().ToggleDisplay(false);
                    mStaticThings.I.SelectorEnabled = false;
                }
            }
            else
            {
                if (selectorshow)
                {
                    GetComponent<VRSelector>().ToggleDisplay(true);
                    mStaticThings.I.SelectorEnabled = true;
                }
            }
        }
    }
}