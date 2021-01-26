using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{


    [ActionCategory("VRActions")]
    public class SendWsCameraView : FsmStateAction
    {
        
        public FsmBool SendMyCameraToAll;
		public FsmBool SendSelectCameraToAll;
		public FsmBool SendAllSelfCamera;
		public FsmBool SendThirdCamera;
        // Code that runs on entering the state.
        public override void OnEnter()
        {
			if(SendMyCameraToAll.Value){
				SetThisFollowCamera();
			}else if(SendSelectCameraToAll.Value){
				SetSelectFollowCamera();
			}else if(SendAllSelfCamera.Value){
				SetAllMyselfFollowCamera();
			}else if(SendThirdCamera.Value){
				 MessageDispatcher.SendMessage(this, VrDispMessageType.CameraScreenSetFree.ToString(), true, 0);
			}
            
            Finish();
        }

        public void SetThisFollowCamera()
        {
            CameraScreenInfo sendinfo = new CameraScreenInfo()
            {
                sendid = mStaticThings.I.mAvatarID,
                isfree = false,
                ismyself = false,
                lockwsid = mStaticThings.I.mAvatarID
            };
            MessageDispatcher.SendMessage(this, WsMessageType.SendPCCamera.ToString(), sendinfo, 0);
        }

        public void SetSelectFollowCamera()
        {
            CameraScreenInfo sendinfo = new CameraScreenInfo()
            {
                sendid = mStaticThings.I.mAvatarID,
                isfree = false,
                ismyself = false,
                lockwsid = mStaticThings.I.NowSelectedAvararid
            };
            MessageDispatcher.SendMessage(this, WsMessageType.SendPCCamera.ToString(), sendinfo, 0);
        }

        public void SetAllMyselfFollowCamera()
        {
            CameraScreenInfo sendinfo = new CameraScreenInfo()
            {
                sendid = mStaticThings.I.mAvatarID,
                isfree = false,
                ismyself = true,
                lockwsid = "none"
            };
            MessageDispatcher.SendMessage(this, WsMessageType.SendPCCamera.ToString(), sendinfo, 0);
        }


    }

}
