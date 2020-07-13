using UnityEngine;
using com.ootii.Messages;
using UnityEngine.Video;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class RecieveBigScreenVideoPlayer : FsmStateAction
    {

        // Code that runs on entering the state.
        public FsmTexture wsTexture;
        public FsmEvent RecieveTextureEvent;
        public override void OnEnter()
        {
            MessageDispatcher.AddListener(VrDispMessageType.BigScreenShowVideo.ToString(), BigScreenShowVideo);
        }
        // Code that runs when exiting the state.
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenShowVideo.ToString(), BigScreenShowVideo);
        }
        void BigScreenShowVideo(IMessage msg)
        {
            Texture tex = msg.Data as Texture;
            wsTexture.Value = tex;
            Fsm.Event(RecieveTextureEvent);
        }
    }

}
