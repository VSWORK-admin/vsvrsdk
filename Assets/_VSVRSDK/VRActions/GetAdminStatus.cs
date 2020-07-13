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
            if (ByPass.Value)
            {
                Fsm.Event(AdminTrue);
            }
            else
            {
                int sort;
                if (mStaticThings.I == null)
                {
                    sort = 0;
                }
                else
                {
                    sort = mStaticThings.I.GetSortNumber(mStaticThings.I.mAvatarID);
                }
                Fsm.Event(sort == 0 ? AdminTrue : AdminFalse);
            }
            Finish();
        }

    }

}
