using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVRObjLoadPosition : FsmStateAction
    {
        public FsmGameObject SceneLoadMark;
        public FsmVector3 SceneLoadVector3;
        public FsmFloat SceneLoadScale;
        public FsmGameObject ObjLoadMark;
        public FsmVector3 ObjLoadVector3;
        public FsmFloat ObjLoadScale;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if (mStaticThings.I != null)
            {
                if(SceneLoadMark.Value != null){
                    mStaticThings.I.GlbSceneLoadPosition = SceneLoadMark.Value.transform.position;
                    mStaticThings.I.GlbSceneLoadRotation = SceneLoadMark.Value.transform.eulerAngles;
                    mStaticThings.I.GlbSceneLoadScale = SceneLoadMark.Value.transform.localScale.x;
                }else{
                    mStaticThings.I.GlbSceneLoadPosition = SceneLoadVector3.Value;
                    mStaticThings.I.GlbSceneLoadScale = SceneLoadScale.Value;
                }

                if(ObjLoadMark.Value != null){
                    mStaticThings.I.GlbObjLoadPosition = ObjLoadMark.Value.transform.position;
                    mStaticThings.I.GlbObjLoadRotation = ObjLoadMark.Value.transform.eulerAngles;
                    mStaticThings.I.GlbObjLoadScale = ObjLoadMark.Value.transform.localScale.x;
                }else{
                    mStaticThings.I.GlbObjLoadPosition = ObjLoadVector3.Value;
                    mStaticThings.I.GlbObjLoadScale = ObjLoadScale.Value;
                }
            }
            Finish();
        }
    }
}
