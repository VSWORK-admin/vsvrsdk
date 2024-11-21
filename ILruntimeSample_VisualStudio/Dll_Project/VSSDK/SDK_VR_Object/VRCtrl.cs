
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;

namespace Dll_Project.SDK_VR_Object
{
    class VRCtrl : DllGenerateBase
    {
        public override void  OnEnable()
        {
              VSEngine.Instance.OnEventPointEnterHandler += VRPointEnter;
        }
        public void VRPointEnter(GameObject obj)
        {
            if (obj == null)
                return;
            if (obj.name=="Cube")
            {
                obj.gameObject.SetActive(false);
            }
            //GeneralTools.Log("VRPointEnter name = " + obj.name);
            //当前没有抓取物体 射线碰到哪个就作为当前物体
        }
        public override void OnDisable()
        {
              VSEngine.Instance.OnEventPointEnterHandler += VRPointEnter;
        }
    }
}
