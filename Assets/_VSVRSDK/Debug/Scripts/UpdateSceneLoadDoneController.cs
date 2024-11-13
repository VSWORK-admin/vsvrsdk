using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VSWorkSDK
{
    internal class UpdateSceneLoadDoneController : MonoBehaviour
    {

        public ETCJoystick movejoystick;
        public ETCJoystick rotatejoystick;
        public ETCButton jumpbutton;

        // Start is called before the first frame update
        void Start()
        {
            if (!mStaticThings.I.isVRApp || (mStaticThings.I.isVRApp && !mStaticThings.I.ismobile))
            {
                movejoystick.axisX.directTransform = mStaticThings.I.MainVRROOT;
                movejoystick.axisY.directTransform = mStaticThings.I.MainVRROOT;
                //movejoystick.onMoveEnd.AddListener(AvatarActionController.Instance.OnMoveEnd);

                rotatejoystick.axisX.directTransform = mStaticThings.I.MainVRROOT;
                rotatejoystick.axisY.directTransform = mStaticThings.I.Maincamera;

                jumpbutton.axis.directTransform = mStaticThings.I.MainVRROOT;
            }


        }

    }
}
