using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dll_Project
{
    public static class DllMain
    {
        public static void Main()
        {
            UnityEngine.Debug.Log("Dll Run Main !");
            GeneralDllBehaviorAdapter[] generalDllBehaviorAdapters = DllManager.Instance.transform.GetComponentsInChildren<GeneralDllBehaviorAdapter>(true);
            if (generalDllBehaviorAdapters != null)
            {
                for (int i = 0; i < generalDllBehaviorAdapters.Length; i++)
                {
                    generalDllBehaviorAdapters[i].DllName = "";
                }
            }
            foreach (var obj in DllManager.Instance.ExtralDatas)
            {
                if (obj.Target != null)
                {
                    obj.Target.gameObject.SetActive(true);
                }
            }
        }
    }
}
