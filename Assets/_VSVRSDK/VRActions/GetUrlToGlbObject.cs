using UnityEngine;
using com.ootii.Messages;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("VRActions")]
    public class GetUrlToGlbObject : FsmStateAction
    {
        public FsmString Glb_url;
        public FsmString Glb_sign;
        public FsmGameObject Glb_LoadTransform;
        public FsmBool Glb_isScene;


        public FsmGameObject Load_GlbGameObject;
        [ArrayEditor(VariableType.String)]
        public FsmArray Load_GlbAnimationClips;
        public FsmEvent LoadGlbObjectDone;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            WsGlbMediaFile newloadglb = new WsGlbMediaFile{
                url = Glb_url.Value,
                sign = Glb_sign.Value,
                isscene = Glb_isScene.Value,
                LoadTrasform = Glb_LoadTransform.Value.transform
            };
            MessageDispatcher.SendMessage(this,VrDispMessageType.LoadGlbModels.ToString(),newloadglb,0);
            MessageDispatcher.AddListener(VrDispMessageType.LoadGlbModelsDone.ToString(), LoadGlbModelsDone);
        }
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.LoadGlbModelsDone.ToString(),LoadGlbModelsDone);
        }
        void LoadGlbModelsDone(IMessage msg)
        {
            GlbSceneObjectFile newglb = msg.Data as GlbSceneObjectFile;
            if(newglb.sign == Glb_sign.Value){
                Load_GlbGameObject.Value = newglb.glbobj;
                Load_GlbAnimationClips.stringValues = newglb.clips.ToArray();
                Fsm.Event(LoadGlbObjectDone);
            }
        }
    }

}
