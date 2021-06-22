using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAvatarMarker : MonoBehaviour
{
    public Transform headroot;
    public float headrotefix;
    public bool isrotx;

    public Transform leftAnchor;
    public Transform rightAnchor;

    bool disabletrigger = false;
    bool enabledtrigger = false;
    bool isenabled = true;
    public Transform LaserStart;

    public Transform namepanel;
    private void LateUpdate()
    {

        if (mStaticThings.I == null || mStaticThings.I.isVRApp)
        {
            return;
        }

        if (mStaticThings.I.IsThirdCamera)
        {
            if (disabletrigger)
            {
                setenable(false);
                isenabled = false;
                disabletrigger = false;
            }

            if (enabledtrigger)
            {
                setenable(true);
                isenabled = true;
                enabledtrigger = false;
            }

            if (Vector3.Distance(headroot.position, mStaticThings.I.PCCamra.position) < 0.2 * transform.lossyScale.x)
            {
                if (isenabled)
                {
                    disabletrigger = true;
                }
            }
            else
            {
                if (!isenabled)
                {
                    enabledtrigger = true;
                }
            }
        }else{
            if(!isenabled){
                setenable(true);
                isenabled = true;
            }
        }


    }

    void setenable(bool en)
    {
        MeshRenderer[] ms = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer item in ms)
        {
            item.enabled = en;
        }

        SkinnedMeshRenderer[] sms = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer item in sms)
        {
            item.enabled = en;
        }
    }

}
