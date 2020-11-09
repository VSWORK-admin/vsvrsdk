using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("VRActions")]
    public class GetUrlToLocalCacheFile : FsmStateAction
    {
        public FsmString HttpUrl;
        public FsmBool isURLSign;
        public FsmString Sign;
        public FsmBool hasPrefix;
        public FsmEvent GetLocalPath;
        public FsmEvent GetLocalPathFaild;
        public FsmString GetedSign;
        public FsmString LocalPath;
        public FsmString LocalUrl;

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
                }
                else
                {
                    Fsm.Event(GetLocalPathFaild);
                }
            }

        }

    }

}
