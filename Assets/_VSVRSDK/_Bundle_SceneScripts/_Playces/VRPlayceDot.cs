using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using com.ootii.Messages;
public class VRPlayceDot : MonoBehaviour
{
    public PlayceDotKind dotkind;
    public GameObject _coll;
    public bool isPlaced;
    public string PlacedWSID;
    public GameObject _kuang;
    public GameObject _tip;
    public GameObject _tipkuang_normal;
    public GameObject _tipkuang_direction;
    public GameObject _tipkuang_recenter;
    GameObject tipKuang;
    public Material playcetip_normal;
    public Material playcetip_sel;
    public GameObject human;
    public bool isEnter = false;

    public GameObject humanprefab;

    public bool shadowEnabled = false;
    GameObject humanshadow;
    string nowhuamwsid = "";
    void Start()
    {
        _coll.AddComponent<VRPlayceDotMark>().PlayceDot = gameObject;
        if (human != null)
        {
            Destroy(human);
            human = null;
        }

        if (dotkind == PlayceDotKind.normal)
        {
            tipKuang = _tipkuang_normal;
            _tipkuang_direction.SetActive(false);
            _tipkuang_recenter.SetActive(false);
        }
        if (dotkind == PlayceDotKind.direction)
        {
            tipKuang = _tipkuang_direction;
            _tipkuang_normal.SetActive(false);
            _tipkuang_recenter.SetActive(false);
        }
        else if (dotkind == PlayceDotKind.recenter)
        {
            tipKuang = _tipkuang_recenter;
            _tipkuang_normal.SetActive(false);
            _tipkuang_direction.SetActive(false);
        }
        MessageDispatcher.AddListener(VrDispMessageType.VRPlaycePointEnter.ToString(), VRTelePointEnter);
        MessageDispatcher.AddListener(VrDispMessageType.VRPlaycePointExit.ToString(), VRTelePointExit);
    }


    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener(VrDispMessageType.VRPlaycePointEnter.ToString(), VRTelePointEnter);
        MessageDispatcher.RemoveListener(VrDispMessageType.VRPlaycePointExit.ToString(), VRTelePointExit);
    }


    void CreateHumanShadow(string id)
    {
        if (isPlaced && id == PlacedWSID)
        {
            return;
        }
        shadowEnabled = true;
        nowhuamwsid = id;
        if (humanshadow == null)
        {
            humanshadow = Instantiate(humanprefab, transform.position, transform.rotation) as GameObject;
            humanshadow.name = "_Playce_temp_" + id;


            humanshadow.transform.parent = transform;
            humanshadow.transform.localScale = new Vector3(1, 1, 1);
            humanshadow.transform.localPosition = new Vector3(0, 0, 0);
        }


    }


    public void UpdateHumanShadow()
    {
        if (humanshadow != null)
        {
            if (dotkind == PlayceDotKind.direction)
            {
                if (nowhuamwsid == mStaticThings.I.mAvatarID)
                {
                    humanshadow.transform.localEulerAngles = new Vector3(0, mStaticThings.I.Maincamera.localEulerAngles.y, 0);
                    humanshadow.transform.localPosition = new Vector3(mStaticThings.I.Maincamera.localPosition.x, 0, mStaticThings.I.Maincamera.localPosition.z);
                }
                else if (mStaticThings.DynClientAvatarsDic.ContainsKey(nowhuamwsid) && mStaticThings.DynClientAvatarsDic[nowhuamwsid].cp != null)
                {
                    Vector3 rot = mStaticThings.DynClientAvatarsDic[nowhuamwsid].cp.hr.eulerAngles;
                    Vector3 pos = mStaticThings.DynClientAvatarsDic[nowhuamwsid].cp.hp;

                    humanshadow.transform.localEulerAngles = new Vector3(0, rot.y, 0);
                    humanshadow.transform.localPosition = new Vector3(pos.x, 0, pos.z);
                }
            }
            else if (dotkind == PlayceDotKind.normal)
            {
                if (nowhuamwsid == mStaticThings.I.mAvatarID)
                {
                    Vector3 pos = mStaticThings.I.Maincamera.localPosition;
                    Vector3 wrot = mStaticThings.I.MainVRROOT.rotation.eulerAngles;

                    Vector3 localVec = new Vector3(pos.x, 0, pos.z);
                    Vector3 localfix = Quaternion.AngleAxis(wrot.y, Vector3.up) * localVec;
                    Vector3 lpos = Quaternion.AngleAxis(-transform.localEulerAngles.y, Vector3.up) * localfix;

                    humanshadow.transform.localRotation = Quaternion.Euler(0, mStaticThings.I.Maincamera.localEulerAngles.y, 0) * Quaternion.AngleAxis(mStaticThings.I.MainVRROOT.eulerAngles.y, Vector3.up) * Quaternion.AngleAxis(-transform.localEulerAngles.y, Vector3.up);

                    humanshadow.transform.localPosition = new Vector3(lpos.x, 0, lpos.z);
                }
                else if (mStaticThings.DynClientAvatarsDic.ContainsKey(nowhuamwsid)  && mStaticThings.DynClientAvatarsDic[nowhuamwsid].cp != null)
                {

                    Vector3 hr = mStaticThings.DynClientAvatarsDic[nowhuamwsid].cp.hr.eulerAngles;
                    Vector3 pos = mStaticThings.DynClientAvatarsDic[nowhuamwsid].cp.hp;
                    Vector3 wrot = mStaticThings.DynClientAvatarsDic[nowhuamwsid].wr.eulerAngles;

                    Vector3 localVec = new Vector3(pos.x, 0, pos.z);
                    Vector3 localfix = Quaternion.AngleAxis(wrot.y, Vector3.up) * localVec;
                    Vector3 lpos = Quaternion.AngleAxis(-transform.localEulerAngles.y, Vector3.up) * localfix;

                    humanshadow.transform.localRotation = Quaternion.Euler(0, hr.y, 0) * Quaternion.AngleAxis(wrot.y, Vector3.up) * Quaternion.AngleAxis(-transform.localEulerAngles.y, Vector3.up); ;
                    humanshadow.transform.localPosition = new Vector3(lpos.x, 0, lpos.z);
                }

            }

        }
    }

    void VRTelePointEnter(IMessage msg)
    {
        PlayceEnterMessage pem = (PlayceEnterMessage)msg.Data;
        if (pem.coll == _coll)
        {
            if (pem.wsid != null)
            {
                CreateHumanShadow(pem.wsid);
            }
            else
            {
                if (pem.teleportKind == WsTeleportKind.myself || pem.teleportKind == WsTeleportKind.all)
                {
                    CreateHumanShadow(mStaticThings.I.mAvatarID);
                }
                else if (pem.teleportKind == WsTeleportKind.single)
                {
                    CreateHumanShadow(mStaticThings.I.NowSelectedAvararid);
                }
            }

            isEnter = true;
            ChangeMat(playcetip_sel);
            DOTween.Kill(_coll);
            _tip.transform.DOLocalMoveZ(0.00178f, 0.8f).SetEase(Ease.OutBack).SetId(_coll);
            tipKuang.transform.DOLocalMoveZ(0.00108f, 0.8f).SetEase(Ease.OutBack).SetId(_coll);
        }

    }

    void VRTelePointExit(IMessage msg)
    {
        PlayceEnterMessage pem = (PlayceEnterMessage)msg.Data;
        if (pem.coll == _coll)
        {
            isEnter = false;
            ChangeMat(playcetip_normal);
            DOTween.Kill(_coll);
            _tip.transform.DOLocalMoveZ(0, 0.3f).SetId(_coll);
            tipKuang.transform.DOLocalMoveZ(0, 0.3f).SetId(_coll);
            DeleteHumanShadow();
        }

    }

    public void DeleteHumanShadow()
    {
        if (humanshadow != null)
        {
            Destroy(humanshadow);
            humanshadow = null;
            nowhuamwsid = "";
            shadowEnabled = false;
        }
    }

    void ChangeMat(Material mt)
    {
        _kuang.GetComponent<MeshRenderer>().material = mt;
        _tip.GetComponent<MeshRenderer>().material = mt;
        tipKuang.GetComponent<MeshRenderer>().material = mt;
    }
}
