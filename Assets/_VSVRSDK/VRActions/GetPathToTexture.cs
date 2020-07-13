using UnityEngine;
using com.ootii.Messages;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("VRActions")]
    public class GetPathToTexture : FsmStateAction
    {
        public FsmString Path;
        public FsmEvent EventGetTexture;
        public FsmEvent EventGetTextureFail;
        public FsmTexture GetedTexture;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            StartCoroutine(IELoadLocalPath(Path.Value));
        }
        IEnumerator IELoadLocalPath(string mPath)
        {
            using (var uwr = new UnityWebRequest("File://" + mPath, UnityWebRequest.kHttpVerbGET))
            {
                uwr.downloadHandler = new DownloadHandlerTexture();
                yield return uwr.SendWebRequest();
                if (uwr.error == null)
                {
                    GetedTexture.Value = DownloadHandlerTexture.GetContent(uwr);
                    Fsm.Event(EventGetTexture);
                }
                else
                {
                    GetedTexture.Value = null;
                    Fsm.Event(EventGetTextureFail);
                }
                uwr.Dispose();
            }
        }

        private void OnDestroy()
        {
           GetedTexture.Value = null;
           Resources.UnloadUnusedAssets(); 
        }
    }

}
