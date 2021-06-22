using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace ILRuntimeAdapter
{   
    public class DllGenerateBaseAdapter : CrossBindingAdaptor
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
        CrossBindingMethodInfo<System.Boolean> mOnApplicationPause_12 = new CrossBindingMethodInfo<System.Boolean>("OnApplicationPause");
        CrossBindingMethodInfo mOnApplicationQuit_13 = new CrossBindingMethodInfo("OnApplicationQuit");
        CrossBindingMethodInfo<System.Single[], System.Int32> mOnAudioFilterRead_14 = new CrossBindingMethodInfo<System.Single[], System.Int32>("OnAudioFilterRead");
        CrossBindingMethodInfo mOnBecameInvisible_15 = new CrossBindingMethodInfo("OnBecameInvisible");
        CrossBindingMethodInfo mOnBecameVisible_16 = new CrossBindingMethodInfo("OnBecameVisible");
        CrossBindingMethodInfo mOnBeforeTransformParentChanged_17 = new CrossBindingMethodInfo("OnBeforeTransformParentChanged");
        CrossBindingMethodInfo mOnCanvasGroupChanged_18 = new CrossBindingMethodInfo("OnCanvasGroupChanged");
        CrossBindingMethodInfo<UnityEngine.Collision> mOnCollisionEnter_19 = new CrossBindingMethodInfo<UnityEngine.Collision>("OnCollisionEnter");
        CrossBindingMethodInfo<UnityEngine.Collision2D> mOnCollisionEnter2D_20 = new CrossBindingMethodInfo<UnityEngine.Collision2D>("OnCollisionEnter2D");
        CrossBindingMethodInfo<UnityEngine.Collision> mOnCollisionExit_21 = new CrossBindingMethodInfo<UnityEngine.Collision>("OnCollisionExit");
        CrossBindingMethodInfo<UnityEngine.Collision2D> mOnCollisionExit2D_22 = new CrossBindingMethodInfo<UnityEngine.Collision2D>("OnCollisionExit2D");
        CrossBindingMethodInfo<UnityEngine.Collision> mOnCollisionStay_23 = new CrossBindingMethodInfo<UnityEngine.Collision>("OnCollisionStay");
        CrossBindingMethodInfo<UnityEngine.Collision2D> mOnCollisionStay2D_24 = new CrossBindingMethodInfo<UnityEngine.Collision2D>("OnCollisionStay2D");
        CrossBindingMethodInfo mOnConnectedToServer_25 = new CrossBindingMethodInfo("OnConnectedToServer");
        CrossBindingMethodInfo<UnityEngine.ControllerColliderHit> mOnControllerColliderHit_26 = new CrossBindingMethodInfo<UnityEngine.ControllerColliderHit>("OnControllerColliderHit");
        CrossBindingMethodInfo mOnDrawGizmos_27 = new CrossBindingMethodInfo("OnDrawGizmos");
        CrossBindingMethodInfo mOnGUI_28 = new CrossBindingMethodInfo("OnGUI");
        CrossBindingMethodInfo<System.Single> mOnJointBreak_29 = new CrossBindingMethodInfo<System.Single>("OnJointBreak");
        CrossBindingMethodInfo<UnityEngine.Joint2D> mOnJointBreak2D_30 = new CrossBindingMethodInfo<UnityEngine.Joint2D>("OnJointBreak2D");
        CrossBindingMethodInfo<System.Int32> mOnLevelWasLoaded_31 = new CrossBindingMethodInfo<System.Int32>("OnLevelWasLoaded");
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
        CrossBindingMethodInfo<System.Boolean> mOnApplicationFocus_61 = new CrossBindingMethodInfo<System.Boolean>("OnApplicationFocus");

        static DllGenerateBaseAdapter Instance = null;
        public DllGenerateBaseAdapter()
        {
            Instance = this;
        }
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
                if (Instance.mInit_0.CheckShouldInvokeBase(this.instance))
                    base.Init();
                else
                    Instance.mInit_0.Invoke(this.instance);
            }

            public override void Awake()
            {
                if (Instance.mAwake_1.CheckShouldInvokeBase(this.instance))
                    base.Awake();
                else
                    Instance.mAwake_1.Invoke(this.instance);
            }

            public override void Start()
            {
                if (Instance.mStart_2.CheckShouldInvokeBase(this.instance))
                    base.Start();
                else
                    Instance.mStart_2.Invoke(this.instance);
            }

            public override void OnEnable()
            {
                if (Instance.mOnEnable_3.CheckShouldInvokeBase(this.instance))
                    base.OnEnable();
                else
                    Instance.mOnEnable_3.Invoke(this.instance);
            }

            public override void OnDisable()
            {
                if (Instance.mOnDisable_4.CheckShouldInvokeBase(this.instance))
                    base.OnDisable();
                else
                    Instance.mOnDisable_4.Invoke(this.instance);
            }

            public override void Update()
            {
                if (Instance.mUpdate_5.CheckShouldInvokeBase(this.instance))
                    base.Update();
                else
                    Instance.mUpdate_5.Invoke(this.instance);
            }

            public override void FixedUpdate()
            {
                if (Instance.mFixedUpdate_6.CheckShouldInvokeBase(this.instance))
                    base.FixedUpdate();
                else
                    Instance.mFixedUpdate_6.Invoke(this.instance);
            }

            public override void LateUpdate()
            {
                if (Instance.mLateUpdate_7.CheckShouldInvokeBase(this.instance))
                    base.LateUpdate();
                else
                    Instance.mLateUpdate_7.Invoke(this.instance);
            }

            public override void OnDestroy()
            {
                if (Instance.mOnDestroy_8.CheckShouldInvokeBase(this.instance))
                    base.OnDestroy();
                else
                    Instance.mOnDestroy_8.Invoke(this.instance);
            }

            public override void OnAnimatorMove()
            {
                if (Instance.mOnAnimatorMove_9.CheckShouldInvokeBase(this.instance))
                    base.OnAnimatorMove();
                else
                    Instance.mOnAnimatorMove_9.Invoke(this.instance);
            }

            public override void Reset()
            {
                if (Instance.mReset_10.CheckShouldInvokeBase(this.instance))
                    base.Reset();
                else
                    Instance.mReset_10.Invoke(this.instance);
            }

            public override void OnAnimatorIK(System.Int32 layerIndex)
            {
                if (Instance.mOnAnimatorIK_11.CheckShouldInvokeBase(this.instance))
                    base.OnAnimatorIK(layerIndex);
                else
                    Instance.mOnAnimatorIK_11.Invoke(this.instance, layerIndex);
            }
            public override void OnApplicationFocus(System.Boolean focus)
            {
                if (Instance.mOnApplicationFocus_61.CheckShouldInvokeBase(this.instance))
                    base.OnApplicationFocus(focus);
                else
                    Instance.mOnApplicationFocus_61.Invoke(this.instance, focus);
            }

            public override void OnApplicationPause(System.Boolean pause)
            {
                if (Instance.mOnApplicationPause_12.CheckShouldInvokeBase(this.instance))
                    base.OnApplicationPause(pause);
                else
                    Instance.mOnApplicationPause_12.Invoke(this.instance, pause);
            }

            public override void OnApplicationQuit()
            {
                if (Instance.mOnApplicationQuit_13.CheckShouldInvokeBase(this.instance))
                    base.OnApplicationQuit();
                else
                    Instance.mOnApplicationQuit_13.Invoke(this.instance);
            }

            public override void OnAudioFilterRead(System.Single[] data, System.Int32 channels)
            {
                if (Instance.mOnAudioFilterRead_14.CheckShouldInvokeBase(this.instance))
                    base.OnAudioFilterRead(data, channels);
                else
                    Instance.mOnAudioFilterRead_14.Invoke(this.instance, data, channels);
            }

            public override void OnBecameInvisible()
            {
                if (Instance.mOnBecameInvisible_15.CheckShouldInvokeBase(this.instance))
                    base.OnBecameInvisible();
                else
                    Instance.mOnBecameInvisible_15.Invoke(this.instance);
            }

            public override void OnBecameVisible()
            {
                if (Instance.mOnBecameVisible_16.CheckShouldInvokeBase(this.instance))
                    base.OnBecameVisible();
                else
                    Instance.mOnBecameVisible_16.Invoke(this.instance);
            }

            public override void OnBeforeTransformParentChanged()
            {
                if (Instance.mOnBeforeTransformParentChanged_17.CheckShouldInvokeBase(this.instance))
                    base.OnBeforeTransformParentChanged();
                else
                    Instance.mOnBeforeTransformParentChanged_17.Invoke(this.instance);
            }

            public override void OnCanvasGroupChanged()
            {
                if (Instance.mOnCanvasGroupChanged_18.CheckShouldInvokeBase(this.instance))
                    base.OnCanvasGroupChanged();
                else
                    Instance.mOnCanvasGroupChanged_18.Invoke(this.instance);
            }

            public override void OnCollisionEnter(UnityEngine.Collision collision)
            {
                if (Instance.mOnCollisionEnter_19.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionEnter(collision);
                else
                    Instance.mOnCollisionEnter_19.Invoke(this.instance, collision);
            }

            public override void OnCollisionEnter2D(UnityEngine.Collision2D collision)
            {
                if (Instance.mOnCollisionEnter2D_20.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionEnter2D(collision);
                else
                    Instance.mOnCollisionEnter2D_20.Invoke(this.instance, collision);
            }

            public override void OnCollisionExit(UnityEngine.Collision collision)
            {
                if (Instance.mOnCollisionExit_21.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionExit(collision);
                else
                    Instance.mOnCollisionExit_21.Invoke(this.instance, collision);
            }

            public override void OnCollisionExit2D(UnityEngine.Collision2D collision)
            {
                if (Instance.mOnCollisionExit2D_22.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionExit2D(collision);
                else
                    Instance.mOnCollisionExit2D_22.Invoke(this.instance, collision);
            }

            public override void OnCollisionStay(UnityEngine.Collision collision)
            {
                if (Instance.mOnCollisionStay_23.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionStay(collision);
                else
                    Instance.mOnCollisionStay_23.Invoke(this.instance, collision);
            }

            public override void OnCollisionStay2D(UnityEngine.Collision2D collision)
            {
                if (Instance.mOnCollisionStay2D_24.CheckShouldInvokeBase(this.instance))
                    base.OnCollisionStay2D(collision);
                else
                    Instance.mOnCollisionStay2D_24.Invoke(this.instance, collision);
            }

            public override void OnConnectedToServer()
            {
                if (Instance.mOnConnectedToServer_25.CheckShouldInvokeBase(this.instance))
                    base.OnConnectedToServer();
                else
                    Instance.mOnConnectedToServer_25.Invoke(this.instance);
            }

            public override void OnControllerColliderHit(UnityEngine.ControllerColliderHit hit)
            {
                if (Instance.mOnControllerColliderHit_26.CheckShouldInvokeBase(this.instance))
                    base.OnControllerColliderHit(hit);
                else
                    Instance.mOnControllerColliderHit_26.Invoke(this.instance, hit);
            }

            public override void OnDrawGizmos()
            {
                if (Instance.mOnDrawGizmos_27.CheckShouldInvokeBase(this.instance))
                    base.OnDrawGizmos();
                else
                    Instance.mOnDrawGizmos_27.Invoke(this.instance);
            }

            public override void OnGUI()
            {
                if (Instance.mOnGUI_28.CheckShouldInvokeBase(this.instance))
                    base.OnGUI();
                else
                    Instance.mOnGUI_28.Invoke(this.instance);
            }

            public override void OnJointBreak(System.Single breakForce)
            {
                if (Instance.mOnJointBreak_29.CheckShouldInvokeBase(this.instance))
                    base.OnJointBreak(breakForce);
                else
                    Instance.mOnJointBreak_29.Invoke(this.instance, breakForce);
            }

            public override void OnJointBreak2D(UnityEngine.Joint2D joint)
            {
                if (Instance.mOnJointBreak2D_30.CheckShouldInvokeBase(this.instance))
                    base.OnJointBreak2D(joint);
                else
                    Instance.mOnJointBreak2D_30.Invoke(this.instance, joint);
            }

            public override void OnMouseDown()
            {
                if (Instance.mOnMouseDown_32.CheckShouldInvokeBase(this.instance))
                    base.OnMouseDown();
                else
                    Instance.mOnMouseDown_32.Invoke(this.instance);
            }

            public override void OnMouseDrag()
            {
                if (Instance.mOnMouseDrag_33.CheckShouldInvokeBase(this.instance))
                    base.OnMouseDrag();
                else
                    Instance.mOnMouseDrag_33.Invoke(this.instance);
            }

            public override void OnMouseEnter()
            {
                if (Instance.mOnMouseEnter_34.CheckShouldInvokeBase(this.instance))
                    base.OnMouseEnter();
                else
                    Instance.mOnMouseEnter_34.Invoke(this.instance);
            }

            public override void OnMouseExit()
            {
                if (Instance.mOnMouseExit_35.CheckShouldInvokeBase(this.instance))
                    base.OnMouseExit();
                else
                    Instance.mOnMouseExit_35.Invoke(this.instance);
            }

            public override void OnMouseOver()
            {
                if (Instance.mOnMouseOver_36.CheckShouldInvokeBase(this.instance))
                    base.OnMouseOver();
                else
                    Instance.mOnMouseOver_36.Invoke(this.instance);
            }

            public override void OnMouseUp()
            {
                if (Instance.mOnMouseUp_37.CheckShouldInvokeBase(this.instance))
                    base.OnMouseUp();
                else
                    Instance.mOnMouseUp_37.Invoke(this.instance);
            }

            public override void OnMouseUpAsButton()
            {
                if (Instance.mOnMouseUpAsButton_38.CheckShouldInvokeBase(this.instance))
                    base.OnMouseUpAsButton();
                else
                    Instance.mOnMouseUpAsButton_38.Invoke(this.instance);
            }

            public override void OnParticleCollision(UnityEngine.GameObject other)
            {
                if (Instance.mOnParticleCollision_39.CheckShouldInvokeBase(this.instance))
                    base.OnParticleCollision(other);
                else
                    Instance.mOnParticleCollision_39.Invoke(this.instance, other);
            }

            public override void OnParticleSystemStopped()
            {
                if (Instance.mOnParticleSystemStopped_40.CheckShouldInvokeBase(this.instance))
                    base.OnParticleSystemStopped();
                else
                    Instance.mOnParticleSystemStopped_40.Invoke(this.instance);
            }

            public override void OnParticleTrigger()
            {
                if (Instance.mOnParticleTrigger_41.CheckShouldInvokeBase(this.instance))
                    base.OnParticleTrigger();
                else
                    Instance.mOnParticleTrigger_41.Invoke(this.instance);
            }

            public override void OnPostRender()
            {
                if (Instance.mOnPostRender_42.CheckShouldInvokeBase(this.instance))
                    base.OnPostRender();
                else
                    Instance.mOnPostRender_42.Invoke(this.instance);
            }

            public override void OnPreCull()
            {
                if (Instance.mOnPreCull_43.CheckShouldInvokeBase(this.instance))
                    base.OnPreCull();
                else
                    Instance.mOnPreCull_43.Invoke(this.instance);
            }

            public override void OnPreRender()
            {
                if (Instance.mOnPreRender_44.CheckShouldInvokeBase(this.instance))
                    base.OnPreRender();
                else
                    Instance.mOnPreRender_44.Invoke(this.instance);
            }

            public override void OnRectTransformDimensionsChange()
            {
                if (Instance.mOnRectTransformDimensionsChange_45.CheckShouldInvokeBase(this.instance))
                    base.OnRectTransformDimensionsChange();
                else
                    Instance.mOnRectTransformDimensionsChange_45.Invoke(this.instance);
            }

            public override void OnRectTransformRemoved()
            {
                if (Instance.mOnRectTransformRemoved_46.CheckShouldInvokeBase(this.instance))
                    base.OnRectTransformRemoved();
                else
                    Instance.mOnRectTransformRemoved_46.Invoke(this.instance);
            }

            public override void OnRenderImage(UnityEngine.RenderTexture source, UnityEngine.RenderTexture destination)
            {
                if (Instance.mOnRenderImage_47.CheckShouldInvokeBase(this.instance))
                    base.OnRenderImage(source, destination);
                else
                    Instance.mOnRenderImage_47.Invoke(this.instance, source, destination);
            }

            public override void OnRenderObject()
            {
                if (Instance.mOnRenderObject_48.CheckShouldInvokeBase(this.instance))
                    base.OnRenderObject();
                else
                    Instance.mOnRenderObject_48.Invoke(this.instance);
            }

            public override void OnTransformChildrenChanged()
            {
                if (Instance.mOnTransformChildrenChanged_49.CheckShouldInvokeBase(this.instance))
                    base.OnTransformChildrenChanged();
                else
                    Instance.mOnTransformChildrenChanged_49.Invoke(this.instance);
            }

            public override void OnTransformParentChanged()
            {
                if (Instance.mOnTransformParentChanged_50.CheckShouldInvokeBase(this.instance))
                    base.OnTransformParentChanged();
                else
                    Instance.mOnTransformParentChanged_50.Invoke(this.instance);
            }

            public override void OnTriggerEnter(UnityEngine.Collider other)
            {
                if (Instance.mOnTriggerEnter_51.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerEnter(other);
                else
                    Instance.mOnTriggerEnter_51.Invoke(this.instance, other);
            }

            public override void OnTriggerEnter2D(UnityEngine.Collider2D collision)
            {
                if (Instance.mOnTriggerEnter2D_52.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerEnter2D(collision);
                else
                    Instance.mOnTriggerEnter2D_52.Invoke(this.instance, collision);
            }

            public override void OnTriggerExit(UnityEngine.Collider other)
            {
                if (Instance.mOnTriggerExit_53.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerExit(other);
                else
                    Instance.mOnTriggerExit_53.Invoke(this.instance, other);
            }

            public override void OnTriggerExit2D(UnityEngine.Collider2D collision)
            {
                if (Instance.mOnTriggerExit2D_54.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerExit2D(collision);
                else
                    Instance.mOnTriggerExit2D_54.Invoke(this.instance, collision);
            }

            public override void OnTriggerStay(UnityEngine.Collider other)
            {
                if (Instance.mOnTriggerStay_55.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerStay(other);
                else
                    Instance.mOnTriggerStay_55.Invoke(this.instance, other);
            }

            public override void OnTriggerStay2D(UnityEngine.Collider2D collision)
            {
                if (Instance.mOnTriggerStay2D_56.CheckShouldInvokeBase(this.instance))
                    base.OnTriggerStay2D(collision);
                else
                    Instance.mOnTriggerStay2D_56.Invoke(this.instance, collision);
            }

            public override void OnValidate()
            {
                if (Instance.mOnValidate_57.CheckShouldInvokeBase(this.instance))
                    base.OnValidate();
                else
                    Instance.mOnValidate_57.Invoke(this.instance);
            }

            public override void OnWillRenderObject()
            {
                if (Instance.mOnWillRenderObject_58.CheckShouldInvokeBase(this.instance))
                    base.OnWillRenderObject();
                else
                    Instance.mOnWillRenderObject_58.Invoke(this.instance);
            }

            public override void OnServerInitialized()
            {
                if (Instance.mOnServerInitialized_59.CheckShouldInvokeBase(this.instance))
                    base.OnServerInitialized();
                else
                    Instance.mOnServerInitialized_59.Invoke(this.instance);
            }

            public override void OnDrawGizmosSelected()
            {
                if (Instance.mOnDrawGizmosSelected_60.CheckShouldInvokeBase(this.instance))
                    base.OnDrawGizmosSelected();
                else
                    Instance.mOnDrawGizmosSelected_60.Invoke(this.instance);
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return instance.ToString();
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}

