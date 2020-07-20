using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtNearController : MonoBehaviour
{
    public GameObject enobj;
    public float distance;
    bool isenabled = true;
    // Update is called once per frame
    void Update()
    {
        if (mStaticThings.I == null) { return; }
        if (mStaticThings.I.IsThirdCamera)
        {

            transform.LookAt(mStaticThings.I.PCCamra);
        }
        else
        {
            transform.LookAt(mStaticThings.I.Maincamera);
        }

        if (mStaticThings.I.isVRApp)
        {
            return;
        }

        if (mStaticThings.I.IsThirdCamera)
        {
            if (Vector3.Distance(transform.position, mStaticThings.I.PCCamra.position) < distance*transform.lossyScale.x)
            {
                enobj.SetActive(false);
                isenabled = false;
            }
            else
            {
                enobj.SetActive(true);
                isenabled = true;
            }
        }
        else
        {
            if (!isenabled)
            {
                enobj.SetActive(true);
                isenabled = true;
            }
        }



    }
}
