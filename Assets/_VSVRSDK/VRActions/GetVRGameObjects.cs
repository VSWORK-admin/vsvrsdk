using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class GetVRGameObjects : FsmStateAction
    {

        // Code that runs on entering the state.
        public bool everyframe;
        public FsmGameObject Maincamera;
        public FsmGameObject LeftHand;
        public FsmGameObject RightHand;
        public FsmGameObject LeftTeleportAnchor;
        public FsmGameObject RightTeleportAnchor;
        public FsmGameObject LeftFingerPointerAnchor;
        public FsmGameObject RightFingerPointerAnchor;
        public FsmGameObject MainVRROOT;
        public FsmGameObject LaserPoint;
        public FsmGameObject GlbRoot;
        public FsmGameObject GlbSceneRoot;
        public FsmGameObject GlbOjbRoot;
        public FsmGameObject BigscreenRoot;
        public FsmInt msex;
        public FsmString mWsID;
        public FsmString mAvatarID;
        public FsmString mNickName;
        public FsmBool GMEconnected;
        public FsmInt nowMicVol;
        public FsmBool MicEnabled;
        public FsmString NowSelectedAvatarWsid;
        public FsmString NowUityVersion;

        public FsmBool isVRApp;
        public FsmBool ismobile;
        public FsmInt AutoPlayScene;
        public FsmString startaid;
        public FsmGameObject MenuAnchor;
        public FsmBool Isadmin;
        public FsmBool Sadmin;
        public FsmBool IsDrawingenabled;
        public FsmString nowRoomExID;
        public FsmString nowRoomExEnabled;
        public FsmString nowRoomChID;
        
        public override void OnEnter()
        {
            GetGameObjects();
            if (!everyframe)
            {
                Finish();
            }
        }
        public override void OnUpdate()
        {
            if (everyframe)
            {
                GetGameObjects();
            }

        }
        void GetGameObjects()
        {
            if (mStaticThings.I != null)
            {
                Maincamera.Value = mStaticThings.I.Maincamera.gameObject;
                LeftHand.Value = mStaticThings.I.LeftHand.gameObject;
                RightHand.Value = mStaticThings.I.RightHand.gameObject;
                LeftTeleportAnchor.Value = mStaticThings.I.LeftTeleportAnchor.gameObject;
                RightTeleportAnchor.Value = mStaticThings.I.RightTeleportAnchor.gameObject;
                LeftFingerPointerAnchor.Value = mStaticThings.I.LeftFingerPointerAnchor.gameObject;
                RightFingerPointerAnchor.Value = mStaticThings.I.RightFingerPointerAnchor.gameObject;
                MainVRROOT.Value = mStaticThings.I.MainVRROOT.gameObject;
                LaserPoint.Value = mStaticThings.I.LaserPoint.gameObject;
                GlbRoot.Value = mStaticThings.I.GlbRoot.gameObject;
                GlbSceneRoot.Value = mStaticThings.I.GlbSceneRoot.gameObject;
                GlbOjbRoot.Value = mStaticThings.I.GlbOjbRoot.gameObject;
                BigscreenRoot.Value = mStaticThings.I.BigscreenRoot.gameObject;

                msex.Value = mStaticThings.I.msex;
                mWsID.Value = mStaticThings.I.mWsID;
                mAvatarID.Value = mStaticThings.I.mAvatarID;
                mNickName.Value = mStaticThings.I.mNickName;
                GMEconnected.Value = mStaticThings.I.GMEconected;
                NowSelectedAvatarWsid.Value = mStaticThings.I.NowSelectedAvararid;
                NowUityVersion.Value = mStaticThings.I.now_UnityVersion;
                nowMicVol.Value = mStaticThings.I.nowMicVol;
                MicEnabled.Value = mStaticThings.I.MicEnabled;
                isVRApp.Value = mStaticThings.I.isVRApp;
                ismobile.Value = mStaticThings.I.ismobile;
                AutoPlayScene.Value = mStaticThings.I.AutoPlayScene;
                startaid.Value = mStaticThings.I.startaid;
                MenuAnchor.Value = mStaticThings.I.MenuAnchor.gameObject;
                Isadmin.Value = mStaticThings.I.isAdmin;
                Sadmin.Value = mStaticThings.I.sadmin;
                IsDrawingenabled.Value = mStaticThings.I.isdrawingon;
                nowRoomExID.Value = mStaticThings.I.nowRoomGMEroomExID;
                nowRoomExEnabled.Value =  mStaticThings.I.nowRoomExEnabled;
                nowRoomChID.Value = mStaticThings.I.nowRoomChID;
            }
        }
    }
}
