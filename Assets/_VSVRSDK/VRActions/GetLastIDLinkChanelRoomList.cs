using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class GetLastIDLinkChanelRoomList : FsmStateAction
    {
        [ArrayEditor(VariableType.String)]
        public FsmArray roomids;
        [ArrayEditor(VariableType.String)]
        public FsmArray voiceids;
        public override void OnEnter()
        {
            if(mStaticThings.I == null){
                return;
            }else{
 
                roomids.Resize(0);
                voiceids.Resize(0);
                        List<string> temproomids = new List<string>();
                        List<string> tempvoiceids = new List<string>();
                for(int i = 0;i<mStaticThings.I.LastIDLinkChanelRoomList.Count;i++){
                        temproomids.Add(mStaticThings.I.LastIDLinkChanelRoomList[i].roomid);
                    
                }
                for(int i = 0;i<mStaticThings.I.LastIDLinkChanelRoomList.Count;i++){
                        tempvoiceids.Add(mStaticThings.I.LastIDLinkChanelRoomList[i].voiceid);
                    
                }
                roomids.Values = temproomids.ToArray().Clone() as object[];
                voiceids.Values = tempvoiceids.ToArray().Clone() as object[];

                roomids.SaveChanges();
                voiceids.SaveChanges();
            }
            Finish();
        }
    }
}
