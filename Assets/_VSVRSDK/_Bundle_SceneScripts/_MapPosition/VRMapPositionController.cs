using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMapPositionController : MonoBehaviour
{
    public Transform small1;
    public Transform small2;
    public Transform big1;
    public Transform big2;

    float fixscal;
    // Start is called before the first frame update
    void Start()
    {

        fixscal = (big1.localPosition.x - big2.localPosition.x) / (small1.localPosition.x - small2.localPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        float selfsaclfix = mStaticThings.I.MainVRROOT.localScale.x;
        Vector3 localVec = new Vector3(mStaticThings.I.Maincamera.localPosition.x * selfsaclfix, 0, mStaticThings.I.Maincamera.localPosition.z * selfsaclfix);
        Vector3 localfix = Quaternion.AngleAxis(mStaticThings.I.MainVRROOT.eulerAngles.y, Vector3.up) * localVec;
        Vector3 bdv = mStaticThings.I.MainVRROOT.position + localfix;
        transform.localPosition = new Vector3(small1.localPosition.x + (bdv.x - big1.localPosition.x) / fixscal, small1.localPosition.y + (bdv.z - big1.localPosition.z) / fixscal, 0);
        
        
        Quaternion rot = Quaternion.Euler(0, mStaticThings.I.Maincamera.localRotation.eulerAngles.y, 0) * Quaternion.AngleAxis(mStaticThings.I.MainVRROOT.rotation.eulerAngles.y, Vector3.up);
        transform.localEulerAngles = new Vector3(0, 0, -rot.eulerAngles.y - 90);

    }
}
