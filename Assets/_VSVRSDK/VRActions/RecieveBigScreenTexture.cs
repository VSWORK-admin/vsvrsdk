using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class RecieveBigScreenTexture : FsmStateAction
    {

        // Code that runs on entering the state.


        public FsmTexture wsTexture;
        public FsmEvent RecieveTextureEvent;
        public FsmString wsTexturePath;
        public FsmEvent UpdateTextureEvent;
        public override void OnEnter()
        {
            MessageDispatcher.AddListener(VrDispMessageType.BigScreenShowImage.ToString(), BigScreenShowImage);
            MessageDispatcher.AddListener(VrDispMessageType.BigScreenUpdateImage.ToString(), BigScreenUpdateImage);
            MessageDispatcher.AddListener(VrDispMessageType.BigScreenShowVideoFrame.ToString(), BigScreenShowVideoFrame);
        }

        // Code that runs when exiting the state.
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenShowImage.ToString(), BigScreenShowImage);
            MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenUpdateImage.ToString(), BigScreenUpdateImage);
            MessageDispatcher.RemoveListener(VrDispMessageType.BigScreenShowVideoFrame.ToString(), BigScreenShowVideoFrame);
        }

        void BigScreenShowImage(IMessage msg)
        {
            Texture2D texture = msg.Data as Texture2D;
            wsTexturePath.Value = (string)msg.Sender;
            wsTexture.Value = texture;
            Fsm.Event(RecieveTextureEvent);
        }

        void BigScreenShowVideoFrame(IMessage msg)
        {
            Texture2D texture = msg.Data as Texture2D;
            wsTexture.Value = texture;
            Fsm.Event(RecieveTextureEvent);
        }

        void BigScreenUpdateImage(IMessage msg)
        {
            Texture2D texture = msg.Data as Texture2D;
            wsTexturePath.Value = (string)msg.Sender;
            wsTexture.Value = texture;
            Fsm.Event(UpdateTextureEvent);
        }

    }

}
