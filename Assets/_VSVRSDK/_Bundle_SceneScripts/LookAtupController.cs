using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtupController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (mStaticThings.I == null) { return; }
        Vector3 newpos;
        if(mStaticThings.I.IsThirdCamera){
            newpos= new Vector3(mStaticThings.I.PCCamra.position.x,transform.position.y,mStaticThings.I.PCCamra.position.z);
        }else{
            newpos= new Vector3(mStaticThings.I.Maincamera.position.x,transform.position.y,mStaticThings.I.Maincamera.position.z);
        }
         
        transform.LookAt(newpos);
    }
}
