using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class GetAvatarSort : FsmStateAction
    {

        // Code that runs on entering the state.

        public FsmInt Sort;

        public override void OnEnter()
        {
            int sort;

            if(mStaticThings.I == null){
                sort = 0;
            }else{
                sort = mStaticThings.I.GetSortNumber(mStaticThings.I.mAvatarID);
            }
            Sort.Value = sort;
            Finish();
        }
    }
}
