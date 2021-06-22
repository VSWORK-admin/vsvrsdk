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
        public FsmString IconUrl;
        public FsmString CryptAPI;

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
                        icon = IconUrl.Value,
                        iskod = true,
                        cryptAPI = CryptAPI.Value
                    };
                    MessageDispatcher.SendMessage(false, VrDispMessageType.LoadLocalPathScene.ToString(), newscene, 0);
        }

    }

}
