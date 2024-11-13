using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
using AutoList = System.Collections.Generic.List<object>;
#else
using AutoList = ILRuntime.Other.UncheckedList<object>;
#endif

namespace ILRuntimeAdapter
{   
    public class DllGenerateBaseAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(global::DllGenerateBase);
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

        public class Adapter : global::DllGenerateBase, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo mInit_0 = new CrossBindingMethodInfo("Init");
            CrossBindingMethodInfo mAwake_1 = new CrossBindingMethodInfo("Awake");
            CrossBindingMethodInfo mStart_2 = new CrossBindingMethodInfo("Start");
            CrossBindingMethodInfo mOnEnable_3 = new CrossBindingMethodInfo("OnEnable");
            CrossBindingMethodInfo mOnDisable_4 = new CrossBindingMethodInfo("OnDisable");
            CrossBindingMethodInfo mUpdate_5 = new CrossBindingMethodInfo("Update");
            CrossBindingMethodInfo mFixedUpdate_6 = new CrossBindingMethodInfo("FixedUpdate");
            CrossBindingMethodInfo mLateUpdate_7 = new CrossBindingMethodInfo("LateUpdate");
            CrossBindingMethodInfo mOnDestroy_8 = new CrossBindingMethodInfo("OnDestroy");
            CrossBindingMethodInfo mOnAnimatorMove_9 = new CrossBindingMethodInfo("OnAnimatorMove");
            CrossBindingMethodInfo mReset_10 = new CrossBindingMethodInfo("Reset");
            CrossBindingMethodInfo<System.Int32> mOnAnimatorIK_11 = new CrossBindingMethodInfo<System.Int32>("OnAnimatorIK");
            CrossBindingMethodInfo<System.Boolean> mOnApplicationFocus_12 = new CrossBindingMethodInfo<System.Boolean>("OnApplicationFocus");
            CrossBindingMethodInfo<System.Boolean> mOnApplicationPause_13 = new CrossBindingMethodInfo<System.Boolean>("OnApplicationPause");
            CrossBindingMethodInfo mOnApplicationQuit_14 = new CrossBindingMethodInfo("OnApplicationQuit");
            CrossBindingMethodInfo<System.Single[], System.Int32> mOnAudioFilterRead_15 = new CrossBindingMethodInfo<System.Single[], System.Int32>("OnAudioFilterRead");
            CrossBindingMethodInfo mOnBecameInvisible_16 = new CrossBindingMethodInfo("OnBecameInvisible");
            CrossBindingMethodInfo mOnBecameVisible_17 = new CrossBindingMethodInfo("OnBecameVisible");
            CrossBindingMethodInfo mOnBeforeTransformParentChanged_18 = new CrossBindingMethodInfo("OnBeforeTransformParentChanged");
            CrossBindingMethodInfo mOnCanvasGroupChanged_19 = new CrossBindingMethodInfo("OnCanvasGroupChanged");
            CrossBindingMethodInfo<UnityEngine.Collision> mOnCollisionEnter_20 = new CrossBindingMethodInfo<UnityEngine.Collision>("OnCollisionEnter");
            CrossBindingMethodInfo<UnityEngine.Collision2D> mOnCollisionEnter2D_21 = new CrossBindingMethodInfo<UnityEngine.Collision2D>("OnCollisionEnter2D");
            CrossBindingMethodInfo<UnityEngine.Collision> mOnCollisionExit_22 = new CrossBindingMethodInfo<UnityEngine.Collision>("OnCollisionExit");
            CrossBindingMethodInfo<UnityEngine.Collision2D> mOnCollisionExit2D_23 = new CrossBindingMethodInfo<UnityEngine.Collision2D>("OnCollisionExit2D");
            CrossBindingMethodInfo<UnityEngine.Collision> mOnCollisionStay_24 = new CrossBindingMethodInfo<UnityEngine.Collision>("OnCollisionStay");
            CrossBindingMethodInfo<UnityEngine.Collision2D> mOnCollisionStay2D_25 = new CrossBindingMethodInfo<UnityEngine.Collision2D>("OnCollisionStay2D");
            CrossBindingMethodInfo mOnConnectedToServer_26 = new CrossBindingMethodInfo("OnConnectedToServer");
            CrossBindingMethodInfo<UnityEngine.ControllerColliderHit> mOnControllerColliderHit_27 = new CrossBindingMethodInfo<UnityEngine.ControllerColliderHit>("OnControllerColliderHit");
            CrossBindingMethodInfo mOnDrawGizmos_28 = new CrossBindingMethodInfo("OnDrawGizmos");
            CrossBindingMethodInfo mOnGUI_29 = new CrossBindingMethodInfo("OnGUI");
            CrossBindingMethodInfo<System.Single> mOnJointBreak_30 = new CrossBindingMethodInfo<System.Single>("OnJointBreak");
            CrossBindingMethodInfo<UnityEngine.Joint2D> mOnJointBreak2D_31 = new CrossBindingMethodInfo<UnityEngine.Joint2D>("OnJointBreak2D");
            CrossBindingMethodInfo mOnMouseDown_32 = new CrossBindingMethodInfo("OnMouseDown");
            CrossBindingMethodInfo mOnMouseDrag_33 = new CrossBindingMethodInfo("OnMouseDrag");
            CrossBindingMethodInfo mOnMouseEnter_34 = new CrossBindingMethodInfo("OnMouseEnter");
            CrossBindingMethodInfo mOnMouseExit_35 = new CrossBindingMethodInfo("OnMouseExit");
            CrossBindingMethodInfo mOnMouseOver_36 = new CrossBindingMethodInfo("OnMouseOver");
            CrossBindingMethodInfo mOnMouseUp_37 = new CrossBindingMethodInfo("OnMouseUp");
            CrossBindingMethodInfo mOnMouseUpAsButton_38 = new CrossBindingMethodInfo("OnMouseUpAsButton");
            CrossBindingMethodInfo<UnityEngine.GameObject> mOnParticleCollision_39 = new CrossBindingMethodInfo<UnityEngine.GameObject>("OnParticleCollision");
            CrossBindingMethodInfo mOnParticleSystemStopped_40 = new CrossBindingMethodInfo("OnParticleSystemStopped");
            CrossBindingMethodInfo mOnParticleTrigger_41 = new CrossBindingMethodInfo("OnParticleTrigger");
            CrossBindingMethodInfo mOnPostRender_42 = new CrossBindingMethodInfo("OnPostRender");
            CrossBindingMethodInfo mOnPreCull_43 = new CrossBindingMethodInfo("OnPreCull");
            CrossBindingMethodInfo mOnPreRender_44 = new CrossBindingMethodInfo("OnPreRender");
            CrossBindingMethodInfo mOnRectTransformDimensionsChange_45 = new CrossBindingMethodInfo("OnRectTransformDimensionsChange");
            CrossBindingMethodInfo mOnRectTransformRemoved_46 = new CrossBindingMethodInfo("OnRectTransformRemoved");
            CrossBindingMethodInfo<UnityEngine.RenderTexture, UnityEngine.RenderTexture> mOnRenderImage_47 = new CrossBindingMethodInfo<UnityEngine.RenderTexture, UnityEngine.RenderTexture>("OnRenderImage");
            CrossBindingMethodInfo mOnRenderObject_48 = new CrossBindingMethodInfo("OnRenderObject");
            CrossBindingMethodInfo mOnTransformChildrenChanged_49 = new CrossBindingMethodInfo("OnTransformChildrenChanged");
            CrossBindingMethodInfo mOnTransformParentChanged_50 = new CrossBindingMethodInfo("OnTransformParentChanged");
            CrossBindingMethodInfo<UnityEngine.Collider> mOnTriggerEnter_51 = new CrossBindingMethodInfo<UnityEngine.Collider>("OnTriggerEnter");
            CrossBindingMethodInfo<UnityEngine.Collider2D> mOnTriggerEnter2D_52 = new CrossBindingMethodInfo<UnityEngine.Collider2D>("OnTriggerEnter2D");
            CrossBindingMethodInfo<UnityEngine.Collider> mOnTriggerExit_53 = new CrossBindingMethodInfo<UnityEngine.Collider>("OnTriggerExit");
            CrossBindingMethodInfo<UnityEngine.Collider2D> mOnTriggerExit2D_54 = new CrossBindingMethodInfo<UnityEngine.Collider2D>("OnTriggerExit2D");
            CrossBindingMethodInfo<UnityEngine.Collider> mOnTriggerStay_55 = new CrossBindingMethodInfo<UnityEngine.Collider>("OnTriggerStay");
            CrossBindingMethodInfo<UnityEngine.Collider2D> mOnTriggerStay2D_56 = new CrossBindingMethodInfo<UnityEngine.Collider2D>("OnTriggerStay2D");
            CrossBindingMethodInfo mOnValidate_57 = new CrossBindingMethodInfo("OnValidate");
            CrossBindingMethodInfo mOnWillRenderObject_58 = new CrossBindingMethodInfo("OnWillRenderObject");
            CrossBindingMethodInfo mOnServerInitialized_59 = new CrossBindingMethodInfo("OnServerInitialized");
            CrossBindingMethodInfo mOnDrawGizmosSelected_60 = new CrossBindingMethodInfo("OnDrawGizmosSelected");

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

            public override void Init()
            {
                if (mInit_0.CheckShouldInvokeBase(this.instance))
                    base.Init();
                else
                    mInit_0.Invoke(this.instance);
            }

            public override void Awake()
            {
                if (mAwake_1.CheckShouldInvokeBase(this.instance))
                    base.Awake();
                else
                    mAwake_1.Invoke(this.instance);
            }

            public override void Start()
            {
                if (mStart_2.CheckShouldInvokeBase(this.instance))
                    base.Start();
                else
                    mStart_2.Invoke(this.instance);
            }

            public override void OnEnable()
            {
                if (mOnEnable_3.CheckShouldInvokeBase(this.instance))
                    base.OnEnable();
                else
                    mOnEnable_3.Invoke(this.instance);
            }

            public override void OnDisable()
            {
                if (mOnDisable_4.CheckShouldInvokeBase(this.instance))
                    base.OnDisable();
                else
                    mOnDisable_4.Invoke(this.instance);
            }

            public override void Update()
            {
                if (mUpdate_5.CheckShouldInvokeBase(this.instance))
                    base.Update();
                else
                    mUpdate_5.Invoke(this.instance);
            }

            public override void FixedUpdate()
            {
                if (mFixedUpdate_6.CheckShouldInvokeBase(this.instance))
                    base.FixedUpdate();
                else
                    mFixedUpdate_6.Invoke(this.instance);
            }

            public override void LateUpdate()
            {
                if (mLateUpdate_7.CheckShouldInvokeBase(this.instance))
                    base.LateUpdate();
                else
                    mLateUpdate_7.Invoke(this.instance);
            }

            public override void OnDestroy()
            {
                if (mOnDestroy_8.CheckShouldInvokeBase(this.instance))
                    base.OnDestroy();
                else
                    mOnDestroy_8.Invoke(this.instance);
            }

            public override void OnAnimatorMove()
            {
                if (mOnAnimatorMove_9.CheckShouldInvokeBase(this.instance))
                    base.OnAnimatorMove();
                else
                    mOnAnimatorMove_9.Invoke(this.instance);
            }

            public override void Reset()
            {
                if (mReset_10.CheckShouldInvokeBase(this.instance))
                    base.Reset();
                else
                    mReset_10.Invoke(this.instance);
            }

            public override void OnAnimatorIK(System.Int32 layerIndex)
            {
                if (mOnAnimatorIK_11.CheckShouldInvokeBase(this.instance))
                    base.OnAnimatorIK(layerIndex);
                else
                    mOnAnimatorIK_11.Invoke(this.instance, layerIndex);
            }

            public override void OnApplicationFocus(System.Boolean focus)
            {
                if (mOnApplicationFocus_12.CheckShouldInvokeBase(this.instance))
                    base.OnApplicationFocus(focus);
                else
                    mOnApplicationFocus_12.Invoke(this.instance, focus);
            }

            public override void OnApplicationPause(System.Boolean pause)
            {
                if (mOnApplicationPause_13.CheckShouldInvokeBase(this.instance))
                    base.OnApplicationPause(pause);
                else
                    mOnApplicationPause_13.Invoke(this.instance, pause);
            }

            public override void OnApplicationQuit()
            {
                if (mOnApplicationQuit_14.CheckShouldInvokeBase(this.instance))
                    base.OnApplicationQuit();
                else
                    mOnApplicationQuit_14.Invoke(this.instance);
            }

            public override void OnAudioFilterRead(System.Single[] data, System.Int32 channels)
            {
                if (mOnAudioFilterRead_15.CheckShouldInvokeBase(this.instance))
                    base.OnAudioFilterRead(data, channels);
                else
                    mOnAudioFilterRead_15.Invoke(this.instance, data, channels);
            }

            public override void OnBecameInvisible()
            {
                if (mOnBecameInvisible_16.CheckShouldInvokeBase(this.instance))
                    base.OnBecameInvisible();
                else
                    mOnBecameInvisible_16.Invoke(this.instance);
            }

            public override void OnBecameVisible()
            {
                if (mOnBecameVisible_17.CheckShouldInvokeBase(this.instance))
                    base.OnBecameVisible();
                else
                    mOnBecameVisible_17.Invoke(this.instance);
            }

            public override void OnBeforeTransformParentChanged()
            {
                if (mOnBeforeTransformParentChanged_18.CheckShouldInvokeBase(this.instance))
                    base.OnBeforeTransformParentChanged();
                else
                    mOnBeforeTransformParentChanged_18.Invoke(this.instance);
            }

            public override void OnCanvasGroupChanged()
            {
                if (mOnCanvasGroupChanged_19.CheckShouldInvokeBase(this.instance))
                    base.OnCanvasGroupChanged();
                else
                    mOnCanvasGroupChanged_19.Invoke(this.instance);
            }

            public override void OnCollisionEnter(UnityEngine.Collision collision)
            {
                if (mOnCollisionEnter_20.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionEnter(collision);
                else
                    mOnCollisionEnter_20.Invoke(this.instance, collision);
            }

            public override void OnCollisionEnter2D(UnityEngine.Collision2D collision)
            {
                if (mOnCollisionEnter2D_21.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionEnter2D(collision);
                else
                    mOnCollisionEnter2D_21.Invoke(this.instance, collision);
            }

            public override void OnCollisionExit(UnityEngine.Collision collision)
            {
                if (mOnCollisionExit_22.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionExit(collision);
                else
                    mOnCollisionExit_22.Invoke(this.instance, collision);
            }

            public override void OnCollisionExit2D(UnityEngine.Collision2D collision)
            {
                if (mOnCollisionExit2D_23.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionExit2D(collision);
                else
                    mOnCollisionExit2D_23.Invoke(this.instance, collision);
            }

            public override void OnCollisionStay(UnityEngine.Collision collision)
            {
                if (mOnCollisionStay_24.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionStay(collision);
                else
                    mOnCollisionStay_24.Invoke(this.instance, collision);
            }

            public override void OnCollisionStay2D(UnityEngine.Collision2D collision)
            {
                if (mOnCollisionStay2D_25.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionStay2D(collision);
                else
                    mOnCollisionStay2D_25.Invoke(this.instance, collision);
            }

            public override void OnConnectedToServer()
            {
                if (mOnConnectedToServer_26.CheckShouldInvokeBase(this.instance))
                    base.OnConnectedToServer();
                else
                    mOnConnectedToServer_26.Invoke(this.instance);
            }

            public override void OnControllerColliderHit(UnityEngine.ControllerColliderHit hit)
            {
                if (mOnControllerColliderHit_27.CheckShouldInvokeBase(this.instance))
                    base.OnControllerColliderHit(hit);
                else
                    mOnControllerColliderHit_27.Invoke(this.instance, hit);
            }

            public override void OnDrawGizmos()
            {
                if (mOnDrawGizmos_28.CheckShouldInvokeBase(this.instance))
                    base.OnDrawGizmos();
                else
                    mOnDrawGizmos_28.Invoke(this.instance);
            }

            public override void OnGUI()
            {
                if (mOnGUI_29.CheckShouldInvokeBase(this.instance))
                    base.OnGUI();
                else
                    mOnGUI_29.Invoke(this.instance);
            }

            public override void OnJointBreak(System.Single breakForce)
            {
                if (mOnJointBreak_30.CheckShouldInvokeBase(this.instance))
                    base.OnJointBreak(breakForce);
                else
                    mOnJointBreak_30.Invoke(this.instance, breakForce);
            }

            public override void OnJointBreak2D(UnityEngine.Joint2D joint)
            {
                if (mOnJointBreak2D_31.CheckShouldInvokeBase(this.instance))
                    base.OnJointBreak2D(joint);
                else
                    mOnJointBreak2D_31.Invoke(this.instance, joint);
            }

            public override void OnMouseDown()
            {
                if (mOnMouseDown_32.CheckShouldInvokeBase(this.instance))
                    base.OnMouseDown();
                else
                    mOnMouseDown_32.Invoke(this.instance);
            }

            public override void OnMouseDrag()
            {
                if (mOnMouseDrag_33.CheckShouldInvokeBase(this.instance))
                    base.OnMouseDrag();
                else
                    mOnMouseDrag_33.Invoke(this.instance);
            }

            public override void OnMouseEnter()
            {
                if (mOnMouseEnter_34.CheckShouldInvokeBase(this.instance))
                    base.OnMouseEnter();
                else
                    mOnMouseEnter_34.Invoke(this.instance);
            }

            public override void OnMouseExit()
            {
                if (mOnMouseExit_35.CheckShouldInvokeBase(this.instance))
                    base.OnMouseExit();
                else
                    mOnMouseExit_35.Invoke(this.instance);
            }

            public override void OnMouseOver()
            {
                if (mOnMouseOver_36.CheckShouldInvokeBase(this.instance))
                    base.OnMouseOver();
                else
                    mOnMouseOver_36.Invoke(this.instance);
            }

            public override void OnMouseUp()
            {
                if (mOnMouseUp_37.CheckShouldInvokeBase(this.instance))
                    base.OnMouseUp();
                else
                    mOnMouseUp_37.Invoke(this.instance);
            }

            public override void OnMouseUpAsButton()
            {
                if (mOnMouseUpAsButton_38.CheckShouldInvokeBase(this.instance))
                    base.OnMouseUpAsButton();
                else
                    mOnMouseUpAsButton_38.Invoke(this.instance);
            }

            public override void OnParticleCollision(UnityEngine.GameObject other)
            {
                if (mOnParticleCollision_39.CheckShouldInvokeBase(this.instance))
                    base.OnParticleCollision(other);
                else
                    mOnParticleCollision_39.Invoke(this.instance, other);
            }

            public override void OnParticleSystemStopped()
            {
                if (mOnParticleSystemStopped_40.CheckShouldInvokeBase(this.instance))
                    base.OnParticleSystemStopped();
                else
                    mOnParticleSystemStopped_40.Invoke(this.instance);
            }

            public override void OnParticleTrigger()
            {
                if (mOnParticleTrigger_41.CheckShouldInvokeBase(this.instance))
                    base.OnParticleTrigger();
                else
                    mOnParticleTrigger_41.Invoke(this.instance);
            }

            public override void OnPostRender()
            {
                if (mOnPostRender_42.CheckShouldInvokeBase(this.instance))
                    base.OnPostRender();
                else
                    mOnPostRender_42.Invoke(this.instance);
            }

            public override void OnPreCull()
            {
                if (mOnPreCull_43.CheckShouldInvokeBase(this.instance))
                    base.OnPreCull();
                else
                    mOnPreCull_43.Invoke(this.instance);
            }

            public override void OnPreRender()
            {
                if (mOnPreRender_44.CheckShouldInvokeBase(this.instance))
                    base.OnPreRender();
                else
                    mOnPreRender_44.Invoke(this.instance);
            }

            public override void OnRectTransformDimensionsChange()
            {
                if (mOnRectTransformDimensionsChange_45.CheckShouldInvokeBase(this.instance))
                    base.OnRectTransformDimensionsChange();
                else
                    mOnRectTransformDimensionsChange_45.Invoke(this.instance);
            }

            public override void OnRectTransformRemoved()
            {
                if (mOnRectTransformRemoved_46.CheckShouldInvokeBase(this.instance))
                    base.OnRectTransformRemoved();
                else
                    mOnRectTransformRemoved_46.Invoke(this.instance);
            }

            public override void OnRenderImage(UnityEngine.RenderTexture source, UnityEngine.RenderTexture destination)
            {
                if (mOnRenderImage_47.CheckShouldInvokeBase(this.instance))
                    base.OnRenderImage(source, destination);
                else
                    mOnRenderImage_47.Invoke(this.instance, source, destination);
            }

            public override void OnRenderObject()
            {
                if (mOnRenderObject_48.CheckShouldInvokeBase(this.instance))
                    base.OnRenderObject();
                else
                    mOnRenderObject_48.Invoke(this.instance);
            }

            public override void OnTransformChildrenChanged()
            {
                if (mOnTransformChildrenChanged_49.CheckShouldInvokeBase(this.instance))
                    base.OnTransformChildrenChanged();
                else
                    mOnTransformChildrenChanged_49.Invoke(this.instance);
            }

            public override void OnTransformParentChanged()
            {
                if (mOnTransformParentChanged_50.CheckShouldInvokeBase(this.instance))
                    base.OnTransformParentChanged();
                else
                    mOnTransformParentChanged_50.Invoke(this.instance);
            }

            public override void OnTriggerEnter(UnityEngine.Collider other)
            {
                if (mOnTriggerEnter_51.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerEnter(other);
                else
                    mOnTriggerEnter_51.Invoke(this.instance, other);
            }

            public override void OnTriggerEnter2D(UnityEngine.Collider2D collision)
            {
                if (mOnTriggerEnter2D_52.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerEnter2D(collision);
                else
                    mOnTriggerEnter2D_52.Invoke(this.instance, collision);
            }

            public override void OnTriggerExit(UnityEngine.Collider other)
            {
                if (mOnTriggerExit_53.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerExit(other);
                else
                    mOnTriggerExit_53.Invoke(this.instance, other);
            }

            public override void OnTriggerExit2D(UnityEngine.Collider2D collision)
            {
                if (mOnTriggerExit2D_54.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerExit2D(collision);
                else
                    mOnTriggerExit2D_54.Invoke(this.instance, collision);
            }

            public override void OnTriggerStay(UnityEngine.Collider other)
            {
                if (mOnTriggerStay_55.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerStay(other);
                else
                    mOnTriggerStay_55.Invoke(this.instance, other);
            }

            public override void OnTriggerStay2D(UnityEngine.Collider2D collision)
            {
                if (mOnTriggerStay2D_56.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerStay2D(collision);
                else
                    mOnTriggerStay2D_56.Invoke(this.instance, collision);
            }

            public override void OnValidate()
            {
                if (mOnValidate_57.CheckShouldInvokeBase(this.instance))
                    base.OnValidate();
                else
                    mOnValidate_57.Invoke(this.instance);
            }

            public override void OnWillRenderObject()
            {
                if (mOnWillRenderObject_58.CheckShouldInvokeBase(this.instance))
                    base.OnWillRenderObject();
                else
                    mOnWillRenderObject_58.Invoke(this.instance);
            }

            public override void OnServerInitialized()
            {
                if (mOnServerInitialized_59.CheckShouldInvokeBase(this.instance))
                    base.OnServerInitialized();
                else
                    mOnServerInitialized_59.Invoke(this.instance);
            }

            public override void OnDrawGizmosSelected()
            {
                if (mOnDrawGizmosSelected_60.CheckShouldInvokeBase(this.instance))
                    base.OnDrawGizmosSelected();
                else
                    mOnDrawGizmosSelected_60.Invoke(this.instance);
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

