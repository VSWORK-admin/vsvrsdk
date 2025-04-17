using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
using AutoList = System.Collections.Generic.List<object>;
#else
using AutoList = System.Collections.Generic.List<object>;
#endif

namespace ILRuntimeAdapter
{   
    public class DllConditionalBaseAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(global::DllConditionalBase);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }

        public class Adapter : global::DllConditionalBase, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo mAbort_0 = new CrossBindingMethodInfo("Abort");
            CrossBindingMethodInfo mInit_1 = new CrossBindingMethodInfo("Init");
            CrossBindingMethodInfo mAwake_2 = new CrossBindingMethodInfo("Awake");
            CrossBindingFunctionInfo<System.Boolean> mCanUpdate_3 = new CrossBindingFunctionInfo<System.Boolean>("CanUpdate");
            CrossBindingMethodInfo mStart_4 = new CrossBindingMethodInfo("Start");
            CrossBindingFunctionInfo<Kurisu.AkiBT.Status> mOnUpdate_5 = new CrossBindingFunctionInfo<Kurisu.AkiBT.Status>("OnUpdate");
            CrossBindingFunctionInfo<System.Boolean> mIsUpdatable_6 = new CrossBindingFunctionInfo<System.Boolean>("IsUpdatable");
            CrossBindingMethodInfo mOnStart_7 = new CrossBindingMethodInfo("OnStart");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            public override void Abort()
            {
                if (mAbort_0.CheckShouldInvokeBase(this.instance))
                    base.Abort();
                else
                    mAbort_0.Invoke(this.instance);
            }

            public override void Init()
            {
                if (mInit_1.CheckShouldInvokeBase(this.instance))
                    base.Init();
                else
                    mInit_1.Invoke(this.instance);
            }

            public override void Awake()
            {
                if (mAwake_2.CheckShouldInvokeBase(this.instance))
                    base.Awake();
                else
                    mAwake_2.Invoke(this.instance);
            }

            public override System.Boolean CanUpdate()
            {
                if (mCanUpdate_3.CheckShouldInvokeBase(this.instance))
                    return base.CanUpdate();
                else
                    return mCanUpdate_3.Invoke(this.instance);
            }

            public override void Start()
            {
                if (mStart_4.CheckShouldInvokeBase(this.instance))
                    base.Start();
                else
                    mStart_4.Invoke(this.instance);
            }

            public override Kurisu.AkiBT.Status OnUpdate()
            {
                if (mOnUpdate_5.CheckShouldInvokeBase(this.instance))
                    return base.OnUpdate();
                else
                    return mOnUpdate_5.Invoke(this.instance);
            }

            public override System.Boolean IsUpdatable()
            {
                if (mIsUpdatable_6.CheckShouldInvokeBase(this.instance))
                    return base.IsUpdatable();
                else
                    return mIsUpdatable_6.Invoke(this.instance);
            }

            public override void OnStart()
            {
                if (mOnStart_7.CheckShouldInvokeBase(this.instance))
                    base.OnStart();
                else
                    mOnStart_7.Invoke(this.instance);
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    if (!isInvokingToString)
                    {
                        isInvokingToString = true;
                        string res = instance.ToString();
                        isInvokingToString = false;
                        return res;
                    }
                    else
                        return instance.Type.FullName;
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}

