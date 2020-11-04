using UnityEngine;
using com.ootii.Messages;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("VRActions")]
    public class GetPathToScene : FsmStateAction
    {
        public FsmString Path;
        public FsmString Sign;
        public FsmString SceneName;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
                    WsSceneInfo newscene = new WsSceneInfo
                    {
                        id = SceneName.Value,
                        scene = Path.Value,
                        name = SceneName.Value,
                        version = Sign.Value,
                        isremote = true,
                        isupdate = false,
                        iskod = true
                    };
                    MessageDispatcher.SendMessage(false, VrDispMessageType.LoadLocalPathScene.ToString(), newscene, 0);
        }

    }

}
