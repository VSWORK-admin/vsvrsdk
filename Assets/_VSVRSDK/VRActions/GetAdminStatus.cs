using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class GetAdminStatus : FsmStateAction
    {

        // Code that runs on entering the state.
        public FsmBool ByPass;
        public FsmEvent AdminTrue;
        public FsmEvent AdminFalse;


        public override void Reset()
        {
            AdminTrue = null;
            AdminFalse = null;
        }

        public override void OnEnter()
        {
            if (mStaticThings.I == null)
            {
                return;
            }
            if (ByPass.Value)
            {
                Fsm.Event(AdminTrue);
            }
            else
            {
                if(mStaticThings.I.isAdmin || mStaticThings.I.sadmin){
                    Fsm.Event(AdminTrue);
                }else{
                    Fsm.Event(AdminFalse);
                }
                
            }
            Finish();
        }

    }

}
