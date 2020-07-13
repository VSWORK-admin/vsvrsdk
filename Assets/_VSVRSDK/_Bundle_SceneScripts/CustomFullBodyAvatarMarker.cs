using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFullBodyAvatarMarker : MonoBehaviour
{
    public Transform headroot;
    public Transform footRoot;
    public Transform leftAnchor;
    public Transform rightAnchor;

    public Transform DefaltHead;
    public Transform Defaltleft;
    public Transform Defaltright;

    bool disabletrigger = false;
    bool enabledtrigger = false;
    bool isenabled = true;
    public Transform LaserStart;
    public Transform namepanel;

    private void Update()
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

            if (Vector3.Distance(headroot.position, mStaticThings.I.PCCamra.position) < 0.1 * transform.lossyScale.x)
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
