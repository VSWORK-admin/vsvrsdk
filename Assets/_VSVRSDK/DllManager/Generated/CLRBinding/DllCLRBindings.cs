using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class DllCLRBindings
    {
        public static readonly List<string> blackTransformList = new List<string>
        {
            "_WsAvatarsRoot","_GlbRoot","_GlbSceneRoot","_GlbObjRoot","[DOTween]",
            "MessageDispatcherStub","Dispatcher"
        };
        public static readonly List<string> whiteTransformRootList = new List<string>
        {
            "_WsAvatarsRoot","_GlbRoot"
        };
        public enum ObjectOpration
        {
            None = 0,
            Delete,
            Modified
        }
        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            DLL_UnityEngine_Object_Binding.Register(app);
            DLL_UnityEngine_GameObject_Binding.Register(app);
            DLL_UnityEngine_Transform_Binding.Register(app);
			DLL_UnityEngine_Application_Binding.Register(app);

        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }

        public static bool CheckHasDontDestroyComponentInParent(UnityEngine.Object obj, ObjectOpration opration,string funcName)
        {
            if (obj as UnityEngine.GameObject != null)
            {
                UnityEngine.GameObject gameObject = obj as UnityEngine.GameObject;

                if (DllCLRBindings.blackTransformList.Contains(gameObject.name))
                {
                    DllCLRBindings.ShowLogByType(DllCLRBindings.ObjectOpration.Modified, funcName);
                    return true;
                }
                else if (DllCLRBindings.CheckHasDontDestroyComponent(gameObject, opration, funcName))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool CheckHasDontDestroyComponent(UnityEngine.GameObject gameObject, ObjectOpration opration,string funcName)
        {
            UnityEngine.Transform RootTrans = null;

            for (UnityEngine.Transform Trans = gameObject.transform; ;)
            {
                if (Trans.parent == null)
                {
                    RootTrans = Trans;
                    break;
                }
                Trans = Trans.parent;
            }

            if(DllCLRBindings.whiteTransformRootList.Contains(RootTrans.gameObject.name))
            {
                return false;
            }
            if (RootTrans.GetComponent("DonotDesroyController") != null)
            {
                ShowLogByType(opration, funcName);

                return true;
            }
            return false;
        }

        public static void ShowLogByType(ObjectOpration opration, string funcName)
        {
            switch (opration)
            {
                case ObjectOpration.Delete:
                    UnityEngine.Debug.LogError("The fixed object of the main project cannot be deleted ! function : " + funcName);
                    break;
                case ObjectOpration.Modified:
                    UnityEngine.Debug.LogError("The fixed object of the main project cannot be modified ! function : " + funcName);
                    break;
            }
        }
    }
}
