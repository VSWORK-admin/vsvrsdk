using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDllBehavior : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public string ScriptClassName = string.Empty;

    public string OtherData;
    public ExtralData[] ExtralDatas;
    public ExtralDataObj[] ExtralDataObjs;

    private bool bInit = false;
    public DllDragBase DllClass
    {
        get
        {
            if (DllManager.appdomain == null)
            {
                Debug.LogError("Please init appdomain first !");
                return null;
            }

            if (GenClass == null)
            {
                GenClass = DllManager.appdomain.Instantiate<DllDragBase>(ScriptClassName);
            }

            if (GenClass != null)
            {
                GenClass.BaseMono = this;

                if (!bInit)
                {
                    GenClass.Init();

                    bInit = true;
                }
            }

            return GenClass;
        }
    }

    private DllDragBase GenClass = null;
    private void Awake()
    {
        try
        {
            if (DllManager.appdomain == null)
            {
                Debug.LogError("Please init appdomain first !");
                return;
            }

            if (GenClass == null && !string.IsNullOrEmpty(ScriptClassName))
            {
                GenClass = DllManager.appdomain.Instantiate<DllDragBase>(ScriptClassName);
            }

            if (GenClass != null)
            {
                GenClass.BaseMono = this;

                if (!bInit)
                {
                    GenClass.Init();

                    bInit = true;
                }

                GenClass.Awake();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    private void Start()
    {
        if (GenClass != null)
        {
            GenClass.Start();
        }
    }
    private void OnEnable()
    {
        if (GenClass != null)
        {
            GenClass.OnEnable();
        }
    }
    private void OnDisable()
    {
        if (GenClass != null)
        {
            GenClass.OnDisable();
        }
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

    public void OnDrag(PointerEventData eventData)
    {
        if (GenClass != null)
        {
            GenClass.OnDrag(eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GenClass != null)
        {
            GenClass.OnBeginDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GenClass != null)
        {
            GenClass.OnEndDrag(eventData);
        }
    }
}
