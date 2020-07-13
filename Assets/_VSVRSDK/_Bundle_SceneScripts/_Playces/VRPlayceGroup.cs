using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using DG.Tweening;
using System;
public class VRPlayceGroup : MonoBehaviour
{
    public GameObject _coll;
    public List<VRPlayceDot> _VRPlayceDots;

    public GameObject _sphere;
    public GameObject _tip;

    public Material playcetip_normal;
    public Material playcetip_sel;


    void Start()
    {
        foreach (VRPlayceDot item in _VRPlayceDots.ToArray())
        {
            if(item == null){
                _VRPlayceDots.Remove(item);
            }
        }
        _coll.AddComponent<VRPlayceGroupMark>().PlayceGroup = gameObject;
        MessageDispatcher.AddListener(VrDispMessageType.VRPlaycePointEnter.ToString(), VRTelePointEnter);
        MessageDispatcher.AddListener(VrDispMessageType.VRPlaycePointExit.ToString(), VRTelePointExit);
 
    }

    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener(VrDispMessageType.VRPlaycePointEnter.ToString(), VRTelePointEnter);
        MessageDispatcher.RemoveListener(VrDispMessageType.VRPlaycePointExit.ToString(), VRTelePointExit);
    }

    void VRTelePointEnter(IMessage msg)
    {
        PlayceEnterMessage pem = (PlayceEnterMessage)msg.Data;
        if (pem.coll == _coll)
        {
            ChangeMat(playcetip_sel);
            DOTween.Kill(_coll);
            _tip.transform.DOScale(new Vector3(1.6f, 1.6f, 1.6f), 0.6f).SetEase(Ease.OutBack).SetId(_coll);
            _sphere.transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.4f).SetEase(Ease.Linear).SetId(_coll);
            int i = 0;
            List<string> wslist = getlist();
            foreach (var item in _VRPlayceDots)
            {
                if (i < wslist.Count)
                {
                    PlayceEnterMessage enterobj = new PlayceEnterMessage
                    {
                        coll = item._coll,
                        teleportKind = WsTeleportKind.all,
                        wsid = wslist[i]
                    };
                    MessageDispatcher.SendMessage(this, VrDispMessageType.VRPlaycePointEnter.ToString(), enterobj, 0);
                    i++;
                }else{
                    return;
                }

            }
        }
    }

    List<string> getlist()
    {
        List<string> newdic = new List<string>();
        foreach (var item in mStaticThings.AllStaticAvatarsDic)
        {
            newdic.Add(item.Key);
        }
        return newdic;
    }

    void VRTelePointExit(IMessage msg)
    {
        PlayceEnterMessage pem = (PlayceEnterMessage)msg.Data;
        if (pem.coll == _coll)
        {
            ChangeMat(playcetip_normal);
            DOTween.Kill(_coll);
            _tip.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetEase(Ease.Linear).SetId(_coll);
            _sphere.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetEase(Ease.Linear).SetId(_coll);
            foreach (var item in _VRPlayceDots)
            {
                PlayceEnterMessage exitobj = new PlayceEnterMessage
                {
                    coll = item._coll,
                    teleportKind = WsTeleportKind.all,
                    wsid = ""
                };
                MessageDispatcher.SendMessage(this, VrDispMessageType.VRPlaycePointExit.ToString(), exitobj, 0);
            }
        }
    }

    void ChangeMat(Material mt)
    {
        _tip.GetComponent<MeshRenderer>().material = mt;
        _sphere.GetComponent<MeshRenderer>().material = mt;
    }
}
