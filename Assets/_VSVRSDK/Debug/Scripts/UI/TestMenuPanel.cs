using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSWorkSDK
{
    public class TestMenuPanel : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickChangeAdminBtn()
        {
            WsAdminMark newAdminMark = new WsAdminMark
            {
                name = "",
                id = mStaticThings.I.mAvatarID
            };
            if (mStaticThings.I.SendAvatar)
            {
                MessageDispatcher.SendMessage(this, WsMessageType.SendMarkAdmin.ToString(), newAdminMark, 0);
                MessageDispatcher.SendMessage(this, VrDispMessageType.SetAdmin.ToString(), true, 0);
            }
        }

        public void OnClickChangeSAdminBtn()
        {
            mStaticThings.I.sadmin = true;
            MessageDispatcher.SendMessage(this, VrDispMessageType.SetAdmin.ToString(), true, 0);
        }

        public void OnCancelAdmin()
        {
            mStaticThings.I.sadmin = false;
            mStaticThings.I.isAdmin = false;

            MessageDispatcher.SendMessage(this, VrDispMessageType.SetAdmin.ToString(), false, 0);
        }

        public void AddOneAvatar()
        {
            WsAvatarFrame newf = VSWorkSDK.EngineTestManager.Instance.avatarManager.GetCurrentAvatarFrame();

            newf.id = "Rot_" + EngineTestManager.GetGuid();
            newf.aid = "RotAvatar_" + newf.id;
            newf.name = newf.id;
            newf.scene = mStaticThings.I.nowRoomLinkScene;

            StandaloneSender.SendNewAvatar(newf);
        }

        public void MinusOneAvatar()
        {
            foreach(var avatar in VSWorkSDK.EngineTestManager.Instance.avatarManager.AllTestAvatar)
            {
                if(avatar.Key.Contains("Rot"))
                {
                    StandaloneSender.AvatarDisconnect(avatar.Key);
                    break;
                }
            }
        }
    }
}