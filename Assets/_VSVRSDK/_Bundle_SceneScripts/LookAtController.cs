using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (mStaticThings.I == null) { return; }
        if(!mStaticThings.I.IsThirdCamera){
            transform.LookAt(mStaticThings.I.Maincamera);
        }else{
            transform.LookAt(mStaticThings.I.PCCamra);
        }
    }
}
