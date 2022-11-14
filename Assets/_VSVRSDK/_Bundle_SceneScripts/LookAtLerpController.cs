using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtLerpController : MonoBehaviour
{
    
    void Start()
    {
        transform.rotation = mStaticThings.I.GetCurrentMainCamera().rotation;
    }

    private void Update() {
        Transform maincam = mStaticThings.I.GetCurrentMainCamera();
        transform.position = maincam.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, maincam.rotation,Time.deltaTime);
    }
}
