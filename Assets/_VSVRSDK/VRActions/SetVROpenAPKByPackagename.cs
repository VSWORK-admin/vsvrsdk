using UnityEngine;
using com.ootii.Messages;
using UnityEngine.UI;
using System.Collections.Generic;
namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class SetVROpenAPKByPackagename : FsmStateAction
    {
 
        [ArrayEditor(VariableType.String)]
        public FsmArray datakey;
        [ArrayEditor(VariableType.String)]
        public FsmArray datavalue;

        public FsmString packagename;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            
            Dictionary<string,string> senddata = new Dictionary<string,string>();
            for(int i =0;i<datakey.Values.Length;i++){
                senddata.Add(datakey.Values[i].ToString(),datavalue.Values[i].ToString());
            }
            MessageDispatcher.SendMessage(senddata, VrDispMessageType.OpenAPKByPackagename.ToString(), packagename.Value, 0);
            Finish();
        }
    }

}
