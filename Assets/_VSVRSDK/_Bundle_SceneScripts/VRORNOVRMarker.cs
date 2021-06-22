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
                    if(item != null){
                        item.SetActive(false);
                    }
                    
                }
                foreach (var item in ShowInVR)
                {
                    if(item != null){
                     item.SetActive(true);
                    }
                }
            }else{
                foreach (var item in HideInNoVR)
                {
                    if(item != null){
                    item.SetActive(false);
                    }
                }
                foreach (var item in ShowInNoVR)
                {
                    if(item != null){
                     item.SetActive(true);
                    }
                }
            }
        }
        
    }
}
