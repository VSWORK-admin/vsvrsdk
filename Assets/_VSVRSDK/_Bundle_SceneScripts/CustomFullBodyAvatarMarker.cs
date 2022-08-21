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

    public Transform LaserStart;
    public Transform namepanel;
    Vector3 lastpos;
    public GameObject root;
    public float checkdistance = 30f;
    public float checktime = 0.2f;
    public float forcesettime = 0.5f;

    public bool forceset = false;


    private Transform animationAvatarNameRoot;
    private void OnEnable()
    {
        InvokeRepeating("DetectDistance", 0, checktime);
    }

    private void OnDisable()
    {
        CancelInvoke("DetectDistance");
    }

    void DetectDistance()
    {
        float dist = Vector3.Distance(lastpos, transform.position);
        lastpos = transform.position;
        if (dist > checkdistance)
        {
            forceset = true;
            if(IsInvoking("SetForcesetFalse")){
                CancelInvoke("SetForcesetFalse");
            }
            Invoke("SetForcesetFalse",forcesettime);
        }
    }

    void SetForcesetFalse(){
        forceset = false;
    }

    public Transform GetNamePanelRoot()
    {
        Transform trnameheadroot = DefaltHead;
        
        if (animationAvatarNameRoot == null)
        {
            Transform trhead = null;
            foreach(var v in transform.GetComponentsInChildren<Transform>())
            {
                if (v.name == "Head")
                {
                    trhead = v;
                    break;
                }
            }
            if (trhead != null)
            {
                GameObject root = new GameObject("NamePanel");
                root.transform.SetParent(trhead);
                root.transform.localPosition = Vector3.zero;
                root.transform.localRotation = Quaternion.Euler(Vector3.zero);
                animationAvatarNameRoot = root.transform;
            }
        }
        trnameheadroot = animationAvatarNameRoot;
        
        return trnameheadroot;
    }
}
