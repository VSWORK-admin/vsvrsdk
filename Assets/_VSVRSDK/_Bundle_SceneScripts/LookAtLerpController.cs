using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtLerpController : MonoBehaviour
{
    
    void Start()
    {
        transform.rotation = mStaticThings.I.Maincamera.rotation;
    }

    private void Update() {
        transform.position = mStaticThings.I.Maincamera.position;
        transform.rotation = Quaternion.Lerp(transform.rotation,mStaticThings.I.Maincamera.rotation,Time.deltaTime);
    }
}
