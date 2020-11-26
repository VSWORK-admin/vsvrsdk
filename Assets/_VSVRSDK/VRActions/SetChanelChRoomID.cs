using UnityEngine;
using com.ootii.Messages;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetChanelChRoomID : FsmStateAction
    {
        public FsmString ChRoomID;
        public FsmBool ForceSetRoom;
        public override void OnEnter()
        {
            if(mStaticThings.I != null){
                VRChanelRoom ch = new VRChanelRoom{
                    aid= mStaticThings.I.mAvatarID,
                    roomid = ChRoomID.Value,
                    wsid = mStaticThings.I.mWsID
                };
                MessageDispatcher.SendMessage(ForceSetRoom.Value, WsMessageType.SendChangeRoom.ToString(), ch, 0);
            }

            Finish();
        }
    }

}
