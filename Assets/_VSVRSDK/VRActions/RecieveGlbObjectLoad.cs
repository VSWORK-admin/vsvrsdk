using UnityEngine;
using com.ootii.Messages;
using UnityEngine.Video;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class RecieveGlbObjectLoad : FsmStateAction
    {
        // Code that runs on entering the state.
        public FsmString GlbSign;
        public FsmGameObject GlbGameObject;
        public FsmBool IsScene;
        [ArrayEditor(VariableType.String)]
        public FsmArray GlbAnimationClips;
        public FsmGameObject LoadTransformObject;

        public FsmBool OnlyGetKodGlb;
        public FsmEvent LoadGlbObjectDone;
        public override void OnEnter()
        {
            MessageDispatcher.AddListener(VrDispMessageType.LoadGlbModelsDone.ToString(), LoadGlbModelsDone);
        }
        // Code that runs when exiting the state.
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.LoadGlbModelsDone.ToString(), LoadGlbModelsDone);
        }
        void LoadGlbModelsDone(IMessage msg)
        {
            GlbSceneObjectFile newglb = msg.Data as GlbSceneObjectFile;
            if (!OnlyGetKodGlb.Value || (OnlyGetKodGlb.Value && newglb.LoadTrasform != null))
            {
                GlbSign.Value = newglb.sign;
                GlbGameObject.Value = newglb.glbobj;
                IsScene.Value = newglb.isscene;
                GlbAnimationClips.stringValues = newglb.clips.ToArray();
                LoadTransformObject.Value = newglb.LoadTrasform.gameObject;

                Fsm.Event(LoadGlbObjectDone);
            }

        }
    }

}
