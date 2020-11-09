using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRORNOVRMarker : MonoBehaviour
{
    public GameObject[] HideInNoVR;
    public GameObject[] HideInVR;
    public GameObject[] ShowInNoVR;
    public GameObject[] ShowInVR;
    
    void Start()
    {
        if(mStaticThings.I != null){
            if(mStaticThings.I.isVRApp){
                foreach (var item in HideInVR)
                {
                    item.SetActive(false);
                }
                foreach (var item in ShowInVR)
                {
                     item.SetActive(true);
                }
            }else{
                foreach (var item in HideInNoVR)
                {
                    item.SetActive(false);
                }
                foreach (var item in ShowInNoVR)
                {
                     item.SetActive(true);
                }
            }
        }
        
    }
}
