using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class GetAvatarSort : FsmStateAction
    {
        public FsmInt Sort;

        [ArrayEditor(VariableType.String)]
        public FsmArray AllAvatars;
        [ArrayEditor(VariableType.String)]
        public FsmArray ActiveAvatars;
        public override void OnEnter()
        {
            int sort;

            if(mStaticThings.I == null){
                sort = 0;
                return;
            }else{
                sort = mStaticThings.I.GetSortNumber(mStaticThings.I.mAvatarID);
                AllAvatars.stringValues = mStaticThings.AllStaticAvatarList.ToArray();
                ActiveAvatars.stringValues = mStaticThings.AllActiveAvatarList.ToArray();
            }
            Sort.Value = sort;
            Finish();
        }
    }
}
