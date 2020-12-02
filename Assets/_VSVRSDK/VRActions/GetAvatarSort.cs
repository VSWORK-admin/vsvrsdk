using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class GetAvatarSort : FsmStateAction
    {
        public FsmInt Sort;

        [ArrayEditor(VariableType.String)]
        public FsmArray AllAvatars;
        [ArrayEditor(VariableType.String)]
        public FsmArray AllAvatarNames;
        [ArrayEditor(VariableType.String)]
        public FsmArray ActiveAvatars;
        [ArrayEditor(VariableType.String)]
        public FsmArray ActiveNickNames;

        public override void OnEnter()
        {
            int sort;

            if(mStaticThings.I == null){
                sort = 0;
                return;
            }else{
                sort = mStaticThings.I.GetSortNumber(mStaticThings.I.mAvatarID);
                AllAvatars.Resize(0);
                AllAvatarNames.Resize(0);
                ActiveAvatars.Resize(0);
                ActiveNickNames.Resize(0);
                AllAvatars.Values = mStaticThings.I.GetAllStaticAvatarList().ToArray().Clone() as object[];
                AllAvatarNames.Values = mStaticThings.I.GetAllStaticAvatarsDicNames().ToArray().Clone() as object[];
                ActiveAvatars.Values = mStaticThings.I.GetAllActiveAvatarList().ToArray().Clone() as object[];
                ActiveNickNames.Values = mStaticThings.I.GetAllActiveAvatarsDicNames().ToArray().Clone() as object[];
                AllAvatars.SaveChanges();
                AllAvatarNames.SaveChanges();
                ActiveAvatars.SaveChanges();
                ActiveNickNames.SaveChanges();
            }
            Sort.Value = sort;
            Finish();
        }
    }
}
