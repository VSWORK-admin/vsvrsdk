using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
#if ILHotFix
[ILInject.ILInjectorAttribute(ILInject.InjectFlag.NoInject)]
#endif
public class GeneralDllBehavior : MonoBehaviour
{
    [HideInInspector]
    public string DllName = string.Empty;
    public string ScriptClassName = string.Empty;

    public string OtherData;
    public ExtralData[] ExtralDatas;
    public ExtralDataObj[] ExtralDataObjs;
    public ExtralDataInfo[] ExtralDataInfos;

    private bool bInit = false;
    private bool executeEnableFunFail = false;
    private bool executeStartFunFail = false;
    public DllGenerateBase DllClass
    {
        get
        {
            //兼容老版本写法
            if (string.IsNullOrEmpty(ScriptClassName))
            {
                GeneralDllBehaviorAdapter adpter = GetComponent<GeneralDllBehaviorAdapter>();
                if (adpter != null)
                {
                    DllName = adpter.DllName;
                    ScriptClassName = adpter.ScriptClassName;
                }
            }
            Instantiate();
            return GenClass;
        }
    }

    private DllGenerateBase GenClass = null;
    private void Awake()
    {
        Instantiate();
    }
    public void Instantiate()
    {
        if (GenClass != null) return;
        if (string.IsNullOrEmpty(ScriptClassName)) return;
        //Debug.Log($"GeneralDllBehavior Instantiate dllname:{DllName} scriptname:{ScriptClassName}");
        try
        {
            if (!string.IsNullOrEmpty(DllName))
            {
                if (MultiDllManager.Instance != null && MultiDllManager.Instance.AllDllInstance.ContainsKey(DllName))
                {
                    GenClass = MultiDllManager.Instance.AllDllInstance[DllName].appdomain.Instantiate<DllGenerateBase>(ScriptClassName);
                }

                OnScriptInited();
            }
            else
            {
                if (DllManager.appdomain != null)
                    GenClass = DllManager.appdomain.Instantiate<DllGenerateBase>(ScriptClassName);

                OnScriptInited();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("DebugLog.Exception Awake " + "DllName : " + DllName + "ScriptClassName : " + ScriptClassName + " gameObject.name : " + gameObject.name + "\r\n" + e.ToString());
        }
    }
    private void OnScriptInited()
    {
        if (GenClass != null)
        {
            GenClass.BaseMono = this;

            //if (!bInit)
            {
                GenClass.Init();
            }

            GenClass.Awake();
            if (executeEnableFunFail)
            {
                executeEnableFunFail = false;
                GenClass.OnEnable();
            }
            if (executeStartFunFail)
            {
                executeStartFunFail = false;
                GenClass.Start();
            }
        }
    }
    private void Start()
    {
        if (GenClass != null)
        {
            GenClass.Start();
        }
        else
        {
            executeStartFunFail = true;
        }
    }
    private void OnEnable()
    {
        if (GenClass != null)
        {
            GenClass.OnEnable();
        }
        else
        {
            executeEnableFunFail = true;
        }
    }
    private void OnDisable()
    {
        if (GenClass != null)
        {
            GenClass.OnDisable();
        }
        executeEnableFunFail = false;
    }

    private void Update()
    {
        if (GenClass != null)
        {
            GenClass.Update();
        }
    }
    private void FixedUpdate()
    {
        if (GenClass != null)
        {
            GenClass.FixedUpdate();
        }
    }
    private void LateUpdate()
    {
        if (GenClass != null)
        {
            GenClass.LateUpdate();
        }
    }
    private void OnDestroy()
    {
        if (GenClass != null)
        {
            if (DllName.IsNullOrEmpty())
                MessageDispatcher.SendMessage(GenClass, VrDispMessageType.SDKScriptDestroyed.ToString(), ScriptClassName, 0);
            GenClass.OnDestroy();
        }
    }
    private void OnAnimatorMove()
    {
        if (GenClass != null)
        {
            GenClass.OnAnimatorMove();
        }
    }
    private void Reset()
    {
        if (GenClass != null)
        {
            GenClass.Reset();
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (GenClass != null)
        {
            GenClass.OnAnimatorIK(layerIndex);
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (GenClass != null)
        {
            GenClass.OnApplicationFocus(focus);
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (GenClass != null)
        {
            GenClass.OnApplicationPause(pause);
        }
    }
    private void OnApplicationQuit()
    {
        if (GenClass != null)
        {
            GenClass.OnApplicationQuit();
        }
    }
    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (GenClass != null)
        {
            GenClass.OnAudioFilterRead(data, channels);
        }
    }
    private void OnBecameInvisible()
    {
        if (GenClass != null)
        {
            GenClass.OnBecameInvisible();
        }
    }
    private void OnBecameVisible()
    {
        if (GenClass != null)
        {
            GenClass.OnBecameVisible();
        }
    }
    private void OnBeforeTransformParentChanged()
    {
        if (GenClass != null)
        {
            GenClass.OnBeforeTransformParentChanged();
        }
    }
    private void OnCanvasGroupChanged()
    {
        if (GenClass != null)
        {
            GenClass.OnCanvasGroupChanged();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (GenClass != null)
        {
            GenClass.OnCollisionEnter(collision);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GenClass != null)
        {
            GenClass.OnCollisionEnter2D(collision);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (GenClass != null)
        {
            GenClass.OnCollisionExit(collision);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (GenClass != null)
        {
            GenClass.OnCollisionExit2D(collision);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (GenClass != null)
        {
            GenClass.OnCollisionStay(collision);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GenClass != null)
        {
            GenClass.OnCollisionStay2D(collision);
        }
    }
    private void OnConnectedToServer()
    {
        if (GenClass != null)
        {
            GenClass.OnConnectedToServer();
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (GenClass != null)
        {
            GenClass.OnControllerColliderHit(hit);
        }
    }
    private void OnDrawGizmos()
    {
        if (GenClass != null)
        {
            GenClass.OnDrawGizmos();
        }
    }
    private void OnGUI()
    {
        if (GenClass != null)
        {
            GenClass.OnGUI();
        }
    }
    private void OnJointBreak(float breakForce)
    {
        if (GenClass != null)
        {
            GenClass.OnJointBreak(breakForce);
        }
    }
    private void OnJointBreak2D(Joint2D joint)
    {
        if (GenClass != null)
        {
            GenClass.OnJointBreak2D(joint);
        }
    }

#if UNITY_5 || UNITY_2017 || UNITY_2018 || UNITY_2019_1 || UNITY_2019_2 || UNITY_2019_3
    private void OnLevelWasLoaded(int level)
    {
        if (GenClass != null)
        {
            GenClass.OnLevelWasLoaded(level);
        }
    }
#endif
    private void OnMouseDown()
    {
        if (GenClass != null)
        {
            GenClass.OnMouseDown();
        }
    }
    private void OnMouseDrag()
    {
        if (GenClass != null)
        {
            GenClass.OnMouseDrag();
        }
    }
    private void OnMouseEnter()
    {
        if (GenClass != null)
        {
            GenClass.OnMouseEnter();
        }
    }
    private void OnMouseExit()
    {
        if (GenClass != null)
        {
            GenClass.OnMouseExit();
        }
    }
    private void OnMouseOver()
    {
        if (GenClass != null)
        {
            GenClass.OnMouseOver();
        }
    }
    private void OnMouseUp()
    {
        if (GenClass != null)
        {
            GenClass.OnMouseUp();
        }
    }
    private void OnMouseUpAsButton()
    {
        if (GenClass != null)
        {
            GenClass.OnMouseUpAsButton();
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (GenClass != null)
        {
            GenClass.OnParticleCollision(other);
        }
    }
    private void OnParticleSystemStopped()
    {
        if (GenClass != null)
        {
            GenClass.OnParticleSystemStopped();
        }
    }

    private void OnParticleTrigger()
    {
        if (GenClass != null)
        {
            GenClass.OnParticleTrigger();
        }
    }

    private void OnPostRender()
    {
        if (GenClass != null)
        {
            GenClass.OnPostRender();
        }
    }
    private void OnPreCull()
    {
        if (GenClass != null)
        {
            GenClass.OnPreCull();
        }
    }
    private void OnPreRender()
    {
        if (GenClass != null)
        {
            GenClass.OnPreRender();
        }
    }
    private void OnRectTransformDimensionsChange()
    {
        if (GenClass != null)
        {
            GenClass.OnRectTransformDimensionsChange();
        }
    }
    private void OnRectTransformRemoved()
    {
        if (GenClass != null)
        {
            GenClass.OnRectTransformRemoved();
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (GenClass != null)
        {
            GenClass.OnRenderImage(source, destination);
        }
    }
    private void OnRenderObject()
    {
        if (GenClass != null)
        {
            GenClass.OnRenderObject();
        }
    }
    private void OnTransformChildrenChanged()
    {
        if (GenClass != null)
        {
            GenClass.OnTransformChildrenChanged();
        }
    }
    private void OnTransformParentChanged()
    {
        if (GenClass != null)
        {
            GenClass.OnTransformParentChanged();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GenClass != null)
        {
            GenClass.OnTriggerEnter(other);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GenClass != null)
        {
            GenClass.OnTriggerEnter2D(collision);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (GenClass != null)
        {
            GenClass.OnTriggerExit(other);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GenClass != null)
        {
            GenClass.OnTriggerExit2D(collision);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (GenClass != null)
        {
            GenClass.OnTriggerStay(other);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GenClass != null)
        {
            GenClass.OnTriggerStay2D(collision);
        }
    }
    private void OnValidate()
    {
        if (GenClass != null)
        {
            GenClass.OnValidate();
        }
    }
    private void OnWillRenderObject()
    {
        if (GenClass != null)
        {
            GenClass.OnWillRenderObject();
        }
    }
    private void OnServerInitialized()
    {
        if (GenClass != null)
        {
            GenClass.OnServerInitialized();
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (GenClass != null)
        {
            GenClass.OnDrawGizmosSelected();
        }
    }
#if UNITY_5 || UNITY_2017
    private void OnDisconnectedFromMasterServer(NetworkDisconnection info)
    {
        if (GenClass != null)
        {
            GenClass.OnDisconnectedFromMasterServer(info);
        }
    }
    private void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        if (GenClass != null)
        {
            GenClass.OnDisconnectedFromServer(info);
        }        
    }

    private void OnFailedToConnect(NetworkConnectionError error)
    {
        if (GenClass != null)
        {
            GenClass.OnFailedToConnect(error);
        }           
    }
    private void OnFailedToConnectToMasterServer(NetworkConnectionError error)
    {
        if (GenClass != null)
        {
            GenClass.OnFailedToConnectToMasterServer(error);
        }         
    }
    private void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (GenClass != null)
        {
            GenClass.OnMasterServerEvent(msEvent);
        }        
    }
    private void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        if (GenClass != null)
        {
            GenClass.OnNetworkInstantiate(info);
        }           
    }
    private void OnPlayerConnected(NetworkPlayer player)
    {
        if (GenClass != null)
        {
            GenClass.OnPlayerConnected(player);
        }           
    }
    private void OnPlayerDisconnected(NetworkPlayer player)
    {
        if (GenClass != null)
        {
            GenClass.OnPlayerDisconnected(player);
        }         
    }
    private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (GenClass != null)
        {
            GenClass.OnSerializeNetworkView(stream,info);
        }           
    }
#endif
}
