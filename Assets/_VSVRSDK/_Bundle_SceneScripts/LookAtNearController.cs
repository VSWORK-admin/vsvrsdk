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
            //更正名牌的方向为正对着相机，未加mStaticThings.I.GetCurrentMainCamera().up之前，名牌的旋转是不对的
            if (mStaticThings.I.IsFliping)
            {
                transform.LookAt(mStaticThings.I.GetCurrentMainCamera(), mStaticThings.I.GetCurrentMainCamera().up);//自身的旋转角度 lcoal
            }
            else
            {
                transform.LookAt(mStaticThings.I.GetCurrentMainCamera(), -Physics.gravity);//自身的旋转角度 lcoal
            }
            
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
