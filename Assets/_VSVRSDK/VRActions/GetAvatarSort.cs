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
                AllAvatars.stringValues = mStaticThings.AllStaticAvatarList.ToArray();
                ActiveAvatars.stringValues = mStaticThings.AllActiveAvatarList.ToArray();
                ActiveNickNames.Clear();
                List<string> nicknames = new List<string>();
                foreach (var item in mStaticThings.AllStaticAvatarsDic)
                {
                    nicknames.Add(item.Value.name);
                }
                ActiveNickNames.stringValues = nicknames.ToArray();
            }
            Sort.Value = sort;
            Finish();
        }
    }
}
