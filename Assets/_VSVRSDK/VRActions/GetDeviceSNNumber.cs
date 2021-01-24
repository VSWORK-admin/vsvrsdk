using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class GetDeviceSNNumber : FsmStateAction
    {
        public FsmString DeviceSNnumber;
        public override void OnEnter()
        {

            if(mStaticThings.I == null){
                return;
            }else{
                DeviceSNnumber = mStaticThings.I.DeviceSNnumber;
            }
            Finish();
        }
    }
}
