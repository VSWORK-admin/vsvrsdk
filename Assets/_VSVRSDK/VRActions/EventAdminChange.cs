using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class EventAdminChange : FsmStateAction
    {
        // Code that runs on entering the state.
        public FsmEvent AdminTrue;
        public FsmEvent AdminFalse;
        public FsmBool IsAdmin;
        public override void OnEnter()
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
            IsAdmin.Value = (sort == 0);
            MessageDispatcher.AddListener(VrDispMessageType.SetAdmin.ToString(), SetAdmin);
        }
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.SetAdmin.ToString(), SetAdmin);
        }
        void SetAdmin(IMessage msg)
        {
            bool getadmin = (bool)msg.Data;

            if (getadmin != IsAdmin.Value)
            {
                IsAdmin.Value = getadmin;
                Fsm.Event(getadmin ? AdminTrue : AdminFalse);
            }
        }

    }

}
