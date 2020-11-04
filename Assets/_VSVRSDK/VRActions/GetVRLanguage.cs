using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class GetVRLanguage : FsmStateAction
    {
        public FsmInt LanguageSort;
        public FsmString LanguageString;
        public override void OnEnter()
        {
            int sort;
            string lstring;
            if(mStaticThings.I == null){
                sort = 0;
                lstring = LanguageType.English.ToString();
                return;
            }else{
                sort = (int)mStaticThings.I.NowLanguageType;
                lstring = mStaticThings.I.NowLanguageType.ToString();
            }
            LanguageSort.Value = sort;
            LanguageString.Value = lstring;
            Finish();
        }
    }
}
