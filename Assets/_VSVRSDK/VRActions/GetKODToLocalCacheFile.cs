using UnityEngine;
using com.ootii.Messages;
namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("VRActions")]
    public class GetKODToLocalCacheFile : FsmStateAction
    {
        public FsmString KODUrl;
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
                path = KODUrl.Value,
                isURLSign = isURLSign.Value,
                sign = Sign.Value,
                hasPrefix = hasPrefix.Value,
                isKOD = true
            };
            MessageDispatcher.AddListener(VrDispMessageType.GetLocalCacheFile.ToString(), GetCacheFile);
            MessageDispatcher.SendMessage(this,VrDispMessageType.SendCacheFile.ToString(), sendfile, 0);
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
