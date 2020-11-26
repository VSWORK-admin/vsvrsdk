using UnityEngine;
using com.ootii.Messages;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetRootChanelChRoomID : FsmStateAction
    {
        public FsmString RootRoomID;
        public FsmString RootVoiceID;
        public override void OnEnter()
        {
            VRRootChanelRoom ch = new VRRootChanelRoom
            {
                roomid = RootRoomID.Value,
                voiceid = RootVoiceID.Value,
            };
            MessageDispatcher.SendMessage(this, VrDispMessageType.ConnectToNewChanel.ToString(), ch, 0);
            Finish();
        }
    }

}
