using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DllGenerateBase
{
    public GeneralDllBehavior BaseMono = null;

    public virtual void Init()
    {

    }

    public virtual void Awake()
    {

    }

    public virtual void Start()
    {

    }
    public virtual void OnEnable()
    {

    }
    public virtual void OnDisable()
    {

    }

    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {

    }
    public virtual void LateUpdate()
    {

    }
    public virtual void OnDestroy()
    {

    }
    public virtual void OnAnimatorMove()
    {

    }
    public virtual void Reset()
    {

    }
    public virtual void OnAnimatorIK(int layerIndex)
    {

    }
    public virtual void OnApplicationFocus(bool focus)
    {

    }
    public virtual void OnApplicationPause(bool pause)
    {

    }
    public virtual void OnApplicationQuit()
    {

    }
    public virtual void OnAudioFilterRead(float[] data, int channels)
    {

    }
    public virtual void OnBecameInvisible()
    {

    }
    public virtual void OnBecameVisible()
    {

    }
    public virtual void OnBeforeTransformParentChanged()
    {

    }
    public virtual void OnCanvasGroupChanged()
    {

    }
    public virtual void OnCollisionEnter(Collision collision)
    {

    }
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }
    public virtual void OnCollisionExit(Collision collision)
    {

    }
    public virtual void OnCollisionExit2D(Collision2D collision)
    {

    }
    public virtual void OnCollisionStay(Collision collision)
    {

    }
    public virtual void OnCollisionStay2D(Collision2D collision)
    {

    }
    public virtual void OnConnectedToServer()
    {

    }
    public virtual void OnControllerColliderHit(ControllerColliderHit hit)
    {

    }
    public virtual void OnDrawGizmos()
    {

    }
    public virtual void OnGUI()
    {

    }
    public virtual void OnJointBreak(float breakForce)
    {

    }
    public virtual void OnJointBreak2D(Joint2D joint)
    {

    }

#if UNITY_5 || UNITY_2017 || UNITY_2018 || UNITY_2019_1 || UNITY_2019_2 || UNITY_2019_3
    public virtual void OnLevelWasLoaded(int level)
    {

    }
#endif
    public virtual void OnMouseDown()
    {

    }
    public virtual void OnMouseDrag()
    {

    }
    public virtual void OnMouseEnter()
    {

    }
    public virtual void OnMouseExit()
    {

    }
    public virtual void OnMouseOver()
    {

    }
    public virtual void OnMouseUp()
    {

    }
    public virtual void OnMouseUpAsButton()
    {

    }
    public virtual void OnParticleCollision(GameObject other)
    {

    }
    public virtual void OnParticleSystemStopped()
    {

    }

    public virtual void OnParticleTrigger()
    {

    }

    public virtual void OnPostRender()
    {

    }
    public virtual void OnPreCull()
    {

    }
    public virtual void OnPreRender()
    {

    }
    public virtual void OnRectTransformDimensionsChange()
    {

    }
    public virtual void OnRectTransformRemoved()
    {

    }
    public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

    }
    public virtual void OnRenderObject()
    {

    }
    public virtual void OnTransformChildrenChanged()
    {

    }
    public virtual void OnTransformParentChanged()
    {

    }
    public virtual void OnTriggerEnter(Collider other)
    {

    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
    public virtual void OnTriggerExit(Collider other)
    {

    }
    public virtual void OnTriggerExit2D(Collider2D collision)
    {

    }

    public virtual void OnTriggerStay(Collider other)
    {

    }
    public virtual void OnTriggerStay2D(Collider2D collision)
    {

    }
    public virtual void OnValidate()
    {

    }
    public virtual void OnWillRenderObject()
    {

    }
    public virtual void OnServerInitialized()
    {

    }
    public virtual void OnDrawGizmosSelected()
    {

    }
#if UNITY_5 || UNITY_2017
    public virtual void OnDisconnectedFromMasterServer(NetworkDisconnection info)
    {
        
    }
    public virtual void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        
    }

    public virtual void OnFailedToConnect(NetworkConnectionError error)
    {
        
    }
    public virtual void OnFailedToConnectToMasterServer(NetworkConnectionError error)
    {
        
    }
    public virtual void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        
    }
    public virtual void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        
    }
    public virtual void OnPlayerConnected(NetworkPlayer player)
    {
        
    }
    public virtual void OnPlayerDisconnected(NetworkPlayer player)
    {
        
    }
    public virtual void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        
    }
#endif
}
