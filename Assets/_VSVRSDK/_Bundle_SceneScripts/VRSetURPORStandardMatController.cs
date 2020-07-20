using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using com.ootii.Messages;
public class VRSetURPORStandardMatController : MonoBehaviour
{
    public List<urporstandardOBJ> changeList = new List<urporstandardOBJ>();
    // Start is called before the first frame update
    private void Start()
    {
        MessageDispatcher.AddListener(VrDispMessageType.ChangePipeLine.ToString(), ChangePipeLine);
        DochangePipeLine();
    }

    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener(VrDispMessageType.ChangePipeLine.ToString(), ChangePipeLine);
    }

    void ChangePipeLine(IMessage msg)
    {
        DochangePipeLine();
    }
    void DochangePipeLine()
    {
        foreach (var item in changeList)
        {
            if (mStaticThings.I.isurp)
            {
                if (item.mObj.GetComponent<Renderer>())
                {
                    item.mObj.GetComponent<Renderer>().material = item.UrpMat;
                }
                else
                {
                    item.mObj.GetComponent<SkinnedMeshRenderer>().material = item.UrpMat;
                }

            }
            else
            {
                if (item.mObj.GetComponent<Renderer>())
                {
                    item.mObj.GetComponent<Renderer>().material = item.StandardMat;
                }
                else
                {
                    item.mObj.GetComponent<SkinnedMeshRenderer>().material = item.StandardMat;
                }
            }
        }
    }





}
