using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetLoadUrlIdScene : FsmStateAction
    {
        public FsmString server;
        public FsmString id;
        public FsmBool isnowserver;
        public FsmBool update;
        // Code that runs on entering the state.
        public override void OnEnter()
        {
            URLIDSceneInfo urlinfo = new URLIDSceneInfo()
            {
                server = server.Value,
                id = id.Value,
                isnowserver = isnowserver.Value,
                update = update.Value
            };
            MessageDispatcher.SendMessage(this, VrDispMessageType.DownloadURLIDScene.ToString(), urlinfo, 0);
            Finish();
        }
    }

}
