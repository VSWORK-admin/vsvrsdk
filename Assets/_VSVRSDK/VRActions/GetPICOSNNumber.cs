using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class GetPICOSNNumber : FsmStateAction
    {
        public FsmString PicoSN;
        public override void OnEnter()
        {

            if(mStaticThings.I == null){
                return;
            }else{
                PicoSN = mStaticThings.I.picoSNnumber;
            }
            Finish();
        }
    }
}
