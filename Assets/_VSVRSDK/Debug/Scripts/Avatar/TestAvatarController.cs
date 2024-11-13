using com.ootii.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VSWorkSDK
{
    public class TestAvatarController : MonoBehaviour
    {
        public Text nametxt;
        public Text tipnametxt;
        internal string AvatarID;
        internal string aid;
        internal int sex;
        internal bool isvr;

        bool delnow = false;
        bool ignorelog = false;
        bool avatarloaded = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        internal void ResetAssets()
        {
            if (IsInvoking("AddQueueToLoadAvatar"))
            {
                CancelInvoke("AddQueueToLoadAvatar");
            }
        }
        internal IEnumerator IEDestroyGameObject()
        {
            ResetAssets();

            yield return new WaitForEndOfFrame();

            if (EngineTestManager.Instance.avatarManager.AllTestAvatar.ContainsKey(AvatarID))
            {
                EngineTestManager.Instance.avatarManager.AllTestAvatar.Remove(AvatarID);
            }

            MessageDispatcher.SendMessage(this, "DeleteCustomAvatarWSDriver", AvatarID, 0);
            if (mStaticThings.I.TeleportDict.ContainsKey(AvatarID))
            {
                mStaticThings.I.TeleportDict.Remove(AvatarID);
            }

            Destroy(gameObject);
        }
        internal void DoDestroy(bool v)
        {
            StartCoroutine(IEDestroyGameObject());
        }

        internal void DestroyAvatarMesh()
        {
            MessageDispatcher.SendMessage(this, "DestroyAvatarMesh", gameObject, 0);
            ResetAssets();
        }

        internal void StopDestroy()
        {
            if (IsInvoking("DoDelayDestroy"))
            {
                CancelInvoke("DoDelayDestroy");
            }
        }

        internal void RecieveWsAvatar(WsAvatarFrameJian curWsAvatarFrame)
        {
            //throw new NotImplementedException();
        }

        internal void DelayDestroy(bool m_delnow, bool _ignorelog = false)
        {
            delnow = m_delnow;
            ignorelog = _ignorelog;
            avatarloaded = false;
            if (delnow)
            {
                DoDestroy();
            }
            else
            {
                if (!IsInvoking("DoDelayDestroy"))
                {
                    Invoke("DoDelayDestroy", 5);
                }
            }
        }

        private void DoDestroy()
        {
            StartCoroutine(IEDestroyGameObject());
        }
    }
}