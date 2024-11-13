using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSWorkSDK
{
    public class VRSetMenurootController : MonoBehaviour
    {
        private void Awake()
        {
            mStaticThings.I.MenuAnchor = transform;
        }
    }
}