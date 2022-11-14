using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSEngine
{
    public enum FunctionType
    {
        None,
        VsAuthority,
        VsAvatar,
        VsVoiceRoom,
        VsVRLaser,
        VsVRInput,
        VsMenu,
        VsVRSystem,
        VsFile,
        VsObjectControl,
        VsBigScreen,
        VsCamera,
        VsScene,
        VsMessage,
        VsChanel,
        VsCache,
        VsCloudRender,

    }

    public class BaseFunction
    {
        public FunctionType NowFunctionType = FunctionType.None;

        private static Dictionary<FunctionType,BaseFunction> baseFunctions = new Dictionary<FunctionType, BaseFunction>();
        internal static Dictionary<FunctionType, BaseFunction> BaseFunctions { get { return baseFunctions; } }

        public BaseFunction(FunctionType fcType)
        {
            NowFunctionType = fcType;
            if (!baseFunctions.ContainsKey(NowFunctionType))
                baseFunctions.Add(NowFunctionType, this);
        }
        ~BaseFunction()
        {
            if (baseFunctions.ContainsKey(NowFunctionType))
                baseFunctions.Remove(NowFunctionType);
        }

        internal virtual void Awake()
        {

        }
        internal virtual void Start()
        {

        }
        internal virtual void Update()
        {

        }
        internal virtual void FixedUpdate()
        {

        }

        internal virtual void OnEnable()
        {

        }
        internal virtual void OnDisable()
        {

        }
        internal virtual void OnDestroy()
        {

        }
    }

}
