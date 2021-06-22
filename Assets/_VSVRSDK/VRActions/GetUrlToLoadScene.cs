using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("VRActions")]
    public class GetUrlToLoadScene : FsmStateAction
    {
        public FsmString HttpUrl;
        public FsmBool isURLSign;
        public FsmString Sign;
        public FsmString SceneName;
        public FsmBool hasPrefix;
        public FsmString RoomIconUrl;
        public FsmEvent GetLocalPath;
        public FsmEvent GetLocalPathFaild;
        public FsmString GetedSign;
        public FsmString LocalPath;
        public FsmString LocalUrl;
        public FsmString CryptAPI;
        public FsmInt CryptKind;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            LocalCacheFile sendfile = new LocalCacheFile(){
                path = HttpUrl.Value,
                isURLSign = isURLSign.Value,
                sign = Sign.Value,
                hasPrefix = hasPrefix.Value,
                isKOD = false,
            };
            MessageDispatcher.AddListener(VrDispMessageType.GetLocalCacheFile.ToString(), GetCacheFile);
            MessageDispatcher.SendMessage(this,VrDispMessageType.SendCacheFile.ToString(), sendfile, 0.01f);
        }
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(VrDispMessageType.GetLocalCacheFile.ToString(), GetCacheFile);
        }
        void GetCacheFile(IMessage msg)
        {
            LocalCacheFile sendfile = msg.Data as LocalCacheFile;
            LocalPath.Value = sendfile.path;
            LocalUrl.Value = "File://" + LocalPath;
            GetedSign.Value = sendfile.sign;
            if(sendfile.sign == Sign.Value){
                if (sendfile.path != "")
                {
                    Fsm.Event(GetLocalPath);
                    WsMediaFile kodfile = new WsMediaFile{
                        roomurl = mStaticThings.I.nowRoomServerUrl,
                        preurl = mStaticThings.I.ThisKODfileUrl,
                        url = HttpUrl.Value,
                        name = SceneName.Value,
                        size = "11111",
                        ext = "scene",
                        mtime = sendfile.sign,
                        isupdate = false,
                        fileMd5 = ""
                    };

                    WsSceneInfo newscene = new WsSceneInfo
                    {
                        id = SceneName.Value,
                        scene = sendfile.path,
                        name = SceneName.Value,
                        version = Sign.Value,
                        isremote = true,
                        isupdate = false,
                        iskod = true,
                        icon = RoomIconUrl.Value,
                        kod=kodfile,
                        cryptAPI =CryptAPI.Value,
                        ckind = CryptKind.Value
                    };
                    MessageDispatcher.SendMessage(true, VrDispMessageType.LoadLocalPathScene.ToString(), newscene, 0);
                }
                else
                {
                    Fsm.Event(GetLocalPathFaild);
                }
            }

        }

    }

}
