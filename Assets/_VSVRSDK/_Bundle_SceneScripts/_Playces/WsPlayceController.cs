using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using UnityEngine.Networking;
public class WsPlayceController : MonoBehaviour
{
    public Transform Playces;
    public Transform StartGroup;
    public Dictionary<string, string> WsPlaceMarkDic = new Dictionary<string, string>();

    bool PlayceEnabled = false;
    VRPlayceDot[] vpdots;
    bool isenabled = true;
    // Start is called before the first frame update
    void Start()
    {
        WsPlaceMarkDic.Clear();
        MessageDispatcher.AddListener(VrDispMessageType.VRPlaycePort.ToString(), VRPlaycePort);
        MessageDispatcher.AddListener(WsMessageType.RecievePlaceMark.ToString(), RecievePlaceMark);
        MessageDispatcher.AddListener(VrDispMessageType.AllPlaceTo.ToString(), AllPlaceTo);
        MessageDispatcher.AddListener(VrDispMessageType.TeleporterStatusChange.ToString(), TeleporterStatusChange);

        MessageDispatcher.AddListener(VrDispMessageType.DestroyWsAvatar.ToString(), DestroyWsAvatar);
        MessageDispatcher.AddListener(VrDispMessageType.SceneFirstConnectWS.ToString(), SceneFirstConnectWS);
        vpdots = GetComponentsInChildren<VRPlayceDot>();


        EnablePlaycesRender(false);

        if (mStaticThings.I == null) { return; }
        if (mStaticThings.I.IsSelfJoinScene)
        {
            mStaticThings.I.IsSelfJoinScene = false;
            StartCoroutine(SelfLoadScene());
        }
        else
        {
            GoStartGroup();
        }
    }


    void GoStartGroup(){
            int sort = 0;
            int max = StartGroup.GetComponent<VRPlayceGroup>()._VRPlayceDots.Count - 1;
            if (mStaticThings.AllStaticAvatarsDic.Count > 1)
            {
                sort = mStaticThings.I.GetSortNumber(mStaticThings.I.mAvatarID);
            }
            int playcenum = Mathf.Clamp(sort, 0, max);

            WsPlaceMark newteleinfo = new WsPlaceMark
            {
                id = mStaticThings.I.mAvatarID,
                dname = StartGroup.GetComponent<VRPlayceGroup>()._VRPlayceDots[playcenum].name
            };

            WsPlaceMarkList wmlist = new WsPlaceMarkList
            {
                kind = WsPlaycePortKind.single,
                gname = StartGroup.name,
                id = mStaticThings.I.mAvatarID,
                marks = new List<WsPlaceMark>()
            };
            mStaticThings.I.nowGroupName = StartGroup.name;
            wmlist.marks.Add(newteleinfo);
            PlayceToNew(newteleinfo);
            MessageDispatcher.SendMessage(this, WsMessageType.SendPlaceMark.ToString(), wmlist, 0);
    }




    void SceneFirstConnectWS(IMessage msg)
    {
        ConnectAvatars cav = (ConnectAvatars)msg.Data;
        WsPlaceMarkDic.Clear();
        if (cav.sceneavatars.Count > 0)
        {
            StartCoroutine(SelfLoadScene());
        }
        else
        {
            PlayceToGroup(StartGroup.name, true);
        }
    }


    IEnumerator SelfLoadScene()
    {
        if(mStaticThings.I.nowRoomServerGetUrl.Contains("127.0.0.1")){
            PlayceToGroup(StartGroup.name, true);
        }else{
            string url = mStaticThings.I.nowRoomServerGetUrl + "/placemark?apitoken="+mStaticThings.apitoken+"&socketid="+mStaticThings.I.mWsID;
            UnityWebRequest request = UnityWebRequest.Get(@url);
            yield return request.SendWebRequest();
            if (request.error != null)
            {
                Debug.LogWarning(request.error);
                yield break;
            }
            string str = request.downloadHandler.text;
            //Debug.LogWarning("************************************" + str);
            WsPlaceMarkList nowwpm = JsonUtility.FromJson<WsPlaceMarkList>(str);
            InitConnectGroup(nowwpm);
        }

    }


    void InitConnectGroup(WsPlaceMarkList nowwpm)
    {
        if(nowwpm.gname == null || nowwpm.gname == ""){
            GoStartGroup();
            return;
        }
        MarkAllPlayce(nowwpm, false);
        //Debug.LogWarning(JsonUtility.ToJson(nowwpm));
        Dictionary<string, string> temdic = new Dictionary<string, string>();
        foreach (var itemc in nowwpm.marks)
        {
            if (itemc.id != mStaticThings.I.mAvatarID)
            {
                temdic.Add(itemc.id, itemc.dname);
            }
        }
        Transform targetgroup = Playces.Find(nowwpm.gname);

       
        if (targetgroup == null)
        {
            targetgroup = Playces.Find(StartGroup.name);
        }
        if (targetgroup == null)
        {
            return;
        }
        mStaticThings.I.nowGroupName = targetgroup.name;
        VRPlayceGroup vrp = targetgroup.GetComponent<VRPlayceGroup>();
        if (vrp != null)
        {
            if (mStaticThings.I.SendAvatar)
            {
                foreach (var item in vrp._VRPlayceDots)
                {
                    if (item != null)
                    {
                        if (!temdic.ContainsValue(item.name))
                        {
                            PlacePortObj ppo = new PlacePortObj
                            {
                                nowselectobj = item.gameObject,
                                telekind = WsTeleportKind.myself,
                                isGroup = false
                            };
                            MessageDispatcher.SendMessage(this, VrDispMessageType.VRPlaycePort.ToString(), ppo, 0);
                            return;
                        }
                    }
                }
            }
            else
            {
                if (vrp._VRPlayceDots[vrp._VRPlayceDots.Count - 1] != null)
                {
                    PlacePortObj ppo = new PlacePortObj
                    {
                        nowselectobj = vrp._VRPlayceDots[vrp._VRPlayceDots.Count - 1].gameObject,
                        telekind = WsTeleportKind.myself,
                        isGroup = false
                    };
                    MessageDispatcher.SendMessage(this, VrDispMessageType.VRPlaycePort.ToString(), ppo, 0);
                }
                return;
            }
        }

    }

    private void OnDestroy()
    {
        isenabled = false;
        MessageDispatcher.RemoveListener(VrDispMessageType.VRPlaycePort.ToString(), VRPlaycePort);
        MessageDispatcher.RemoveListener(WsMessageType.RecievePlaceMark.ToString(), RecievePlaceMark);
        MessageDispatcher.RemoveListener(VrDispMessageType.AllPlaceTo.ToString(), AllPlaceTo);
        MessageDispatcher.RemoveListener(VrDispMessageType.TeleporterStatusChange.ToString(), TeleporterStatusChange);

        MessageDispatcher.RemoveListener(VrDispMessageType.DestroyWsAvatar.ToString(), DestroyWsAvatar);
        MessageDispatcher.RemoveListener(VrDispMessageType.SceneFirstConnectWS.ToString(), SceneFirstConnectWS);
    }


    void DestroyWsAvatar(IMessage msg)
    {
        string delid = (string)msg.Data;
        if (WsPlaceMarkDic.ContainsKey(delid))
        {
            RemoveSingleDotPlayced(WsPlaceMarkDic[delid]);
            WsPlaceMarkDic.Remove(delid);
        }
    }


    void VRPlaycePort(IMessage msg)
    {
        PlacePortObj ppo = (PlacePortObj)msg.Data;
        //Debug.LogWarning(JsonUtility.ToJson(ppo));
        if (ppo.isGroup)
        {
            MessageDispatcher.SendMessage(true, VrDispMessageType.AllPlaceTo.ToString(), ppo.nowselectobj == null ? "" : ppo.nowselectobj.name, 0);
        }
        else
        {
            if (ppo.telekind == WsTeleportKind.myself || ppo.telekind == WsTeleportKind.all)
            {
                WsPlaceMark newteleinfo = new WsPlaceMark
                {
                    id = mStaticThings.I.mAvatarID,
                    dname = ppo.nowselectobj == null ? "" : ppo.nowselectobj.name
                };

                WsPlaceMarkList wmlist = new WsPlaceMarkList
                {
                    kind = WsPlaycePortKind.single,
                    gname = mStaticThings.I.nowGroupName,
                    id = mStaticThings.I.mAvatarID,
                    marks = new List<WsPlaceMark>()
                };
                wmlist.marks.Add(newteleinfo);

                PlayceToNew(newteleinfo);
                if (mStaticThings.I.SendAvatar)
                {
                    MessageDispatcher.SendMessage(this, WsMessageType.SendPlaceMark.ToString(), wmlist, 0);
                }

            }
            else if (ppo.telekind == WsTeleportKind.single)
            {
                WsPlaceMark newsingleinfo = new WsPlaceMark
                {
                    id = mStaticThings.I.NowSelectedAvararid,
                    dname = ppo.nowselectobj == null ? "" : ppo.nowselectobj.name
                };
                WsPlaceMarkList wmlist = new WsPlaceMarkList
                {
                    kind = WsPlaycePortKind.single,
                    gname = mStaticThings.I.nowGroupName,
                    id = mStaticThings.I.mAvatarID,
                    marks = new List<WsPlaceMark>()
                };
                wmlist.marks.Add(newsingleinfo);
                PlayceToNew(newsingleinfo);
                MessageDispatcher.SendMessage(this, WsMessageType.SendPlaceMark.ToString(), wmlist, 0);
            }
        }
    }


    void TeleporterStatusChange(IMessage msg)
    {
        bool TeleEnabled = (bool)msg.Data;
        PlayceEnabled = TeleEnabled;
        EnablePlaycesRender(TeleEnabled);
        if (!PlayceEnabled)
        {
            foreach (var item in vpdots)
            {
                if (item.shadowEnabled)
                {
                    item.DeleteHumanShadow();
                }
            }
        }
    }

    private void Update()
    {
        if (PlayceEnabled)
        {
            VRPlayceDot[] vp = GetComponentsInChildren<VRPlayceDot>();
            foreach (var item in vp)
            {
                if (item.shadowEnabled)
                {
                    item.UpdateHumanShadow();
                }
            }
        }
    }


    void EnablePlaycesRender(bool enabled)
    {
        if(!isenabled){
            return;
        }
        MeshRenderer[] ms = GetComponentsInChildren<MeshRenderer>();
        foreach (var item in ms)
        {
            item.enabled = enabled;
        }
        Collider[] cs = GetComponentsInChildren<Collider>();

        foreach (var item in cs)
        {
            item.enabled = enabled;
        }

        if (!enabled) { return; }

        VRPlayceDot[] vp = GetComponentsInChildren<VRPlayceDot>();
        foreach (var item in vp)
        {
            if (item.isPlaced)
            {
                MeshRenderer[] msc = item.gameObject.GetComponentsInChildren<MeshRenderer>();
                foreach (var itemc in msc)
                {
                    itemc.enabled = false;
                }

                Collider[] csc = item.gameObject.GetComponentsInChildren<Collider>();

                foreach (var itemc in csc)
                {
                    itemc.enabled = false;
                }
            }
        }

        //禁用非主持人移动Group
        VRPlayceGroup[] vpg = GetComponentsInChildren<VRPlayceGroup>();
        foreach (var item in vpg)
        {
            if (!mStaticThings.I.isAdmin && !mStaticThings.I.sadmin && enabled)
            {
                MeshRenderer[] msc = item.gameObject.GetComponentsInChildren<MeshRenderer>();
                foreach (var itemc in msc)
                {
                    itemc.enabled = false;
                }

                Collider[] csc = item.gameObject.GetComponentsInChildren<Collider>();

                foreach (var itemc in csc)
                {
                    itemc.enabled = false;
                }
            }
        }
    }
    void AllPlaceTo(IMessage msg)
    {
        string groupname = msg.Data.ToString();
        bool controlall = (bool)msg.Sender;
        PlayceToGroup(groupname, controlall);
    }


    void PlayceToGroup(string groupname, bool controlall)
    {
        Transform Playcegroup = Playces.Find(groupname);
        VRPlayceGroup vrp;
        if (Playcegroup == null)
        {
            Debug.LogWarning("Can't Find a VRPlayceGroup : " + groupname);
            vrp = null;
        }
        else
        {
            vrp = Playcegroup.GetComponent<VRPlayceGroup>();
        }

        if (vrp != null && vrp._VRPlayceDots.Count < 1)
        {
            Debug.LogWarning("GroupDot : " + groupname + "Don't have any VRPlayceDot");
        }


        WsPlaceMarkList wmlist = new WsPlaceMarkList
        {
            kind = WsPlaycePortKind.all,
            gname = groupname,
            id = mStaticThings.I.mAvatarID,
            marks = new List<WsPlaceMark>()
        };

        int cnt = 0;

        List<string> nonactivelist = new List<string>();

        foreach (var item in mStaticThings.AllActiveAvatarList)
        {
            string temname;
            if (vrp != null)
            {
                if (vrp._VRPlayceDots.Count > cnt)
                {
                    temname = vrp._VRPlayceDots[cnt] != null?vrp._VRPlayceDots[cnt].gameObject.name : "";
                }
                else
                {
                    temname = vrp._VRPlayceDots[vrp._VRPlayceDots.Count - 1] != null ? vrp._VRPlayceDots[vrp._VRPlayceDots.Count - 1].gameObject.name:"";
                }
            }
            else
            {
                temname = "";
            }

            cnt++;

            WsPlaceMark newteleinfo = new WsPlaceMark
            {
                id = item,
                dname = temname
            };
            if (item == mStaticThings.I.mAvatarID)
            {
                PlayceToNew(newteleinfo);
            }
            if (!controlall)
            {
                PlayceToNew(newteleinfo);
            }

            wmlist.marks.Add(newteleinfo);
        }

        foreach (var item in mStaticThings.AllStaticAvatarList)
        {
            if (!mStaticThings.AllActiveAvatarList.Contains(item))
            {
                string temname;
                if (vrp != null)
                {
                    if (vrp._VRPlayceDots.Count > cnt)
                    {
                        temname = vrp._VRPlayceDots[cnt]!= null?vrp._VRPlayceDots[cnt].gameObject.name:"";
                    }
                    else
                    {
                        temname = vrp._VRPlayceDots[vrp._VRPlayceDots.Count - 1] != null ? vrp._VRPlayceDots[vrp._VRPlayceDots.Count - 1].gameObject.name :"";
                    }
                }
                else
                {
                    temname = "";
                }

                WsPlaceMark newteleinfo = new WsPlaceMark
                {
                    id = item,
                    dname = temname
                };
                if (item == mStaticThings.I.mAvatarID)
                {
                    PlayceToNew(newteleinfo);
                }
                if (!controlall)
                {
                    PlayceToNew(newteleinfo);
                }
                wmlist.marks.Add(newteleinfo);
            }
        }

        // /Debug.LogWarning(JsonUtility.ToJson(wmlist));
        if (controlall)
        {
            MarkAllPlayce(wmlist);
            MessageDispatcher.SendMessage(this, WsMessageType.SendPlaceMark.ToString(), wmlist, 0);
        }
    }


    void RecievePlaceMark(IMessage msg)
    {
        WsPlaceMarkList newwpmlist = (WsPlaceMarkList)msg.Data;
        //Debug.LogWarning("TELEPORT Sender:  " + JsonUtility.ToJson(newwpmlist));

        foreach (var item in newwpmlist.marks)
        {
            PlayceToNew(item);
            if (mStaticThings.AllActiveAvatarList.Contains(item.id))
            {
                if (WsPlaceMarkDic.ContainsKey(item.id))
                {
                    WsPlaceMarkDic[item.id] = item.dname;
                }
                else
                {
                    WsPlaceMarkDic.Add(item.id, item.dname);
                }
            }
        }

        if (newwpmlist.kind == WsPlaycePortKind.all)
        {
            MarkAllPlayce(newwpmlist);
            mStaticThings.I.nowGroupName = newwpmlist.gname;
        }

    }


    void MarkAllPlayce(WsPlaceMarkList wpmlist, bool ContainMyself = true)
    {
        VRPlayceDot[] vp = GetComponentsInChildren<VRPlayceDot>();
        foreach (var item in vp)
        {
            item.isPlaced = false;
            item.PlacedWSID = "";
        }

        foreach (var item in wpmlist.marks)
        {
            if (item.id != mStaticThings.I.mAvatarID || ContainMyself)
            {
                if (mStaticThings.AllActiveAvatarList.Contains(item.id))
                {
                    Transform target = Playces.Find(item.dname);
                    if (target != null)
                    {
                        VRPlayceDot tvp = target.GetComponent<VRPlayceDot>();
                        if (tvp != null)
                        {
                            tvp.isPlaced = true;
                            tvp.PlacedWSID = item.id;
                        }
                    }
                }
            }
        }
    }

    void RemoveSingleDotPlayced(string dotname)
    {
        Transform target = Playces.Find(dotname);
        if (target != null)
        {
            if (target.GetComponent<VRPlayceDot>())
            {
                target.GetComponent<VRPlayceDot>().isPlaced = false;
                target.GetComponent<VRPlayceDot>().PlacedWSID = "";
            }
        }
    }


    void PlayceToNew(WsPlaceMark wpm)
    {
        if (mStaticThings.I == null) { return; }
        if (wpm.dname == "")
        {
            if (WsPlaceMarkDic.ContainsKey(wpm.id))
            {
                if (WsPlaceMarkDic[wpm.id] != wpm.dname)
                {
                    RemoveSingleDotPlayced(WsPlaceMarkDic[wpm.id]);
                    WsPlaceMarkDic[wpm.id] = wpm.dname;
                }
            }
            else
            {
                WsPlaceMarkDic.Add(wpm.id, wpm.dname);
            }
            return;
        }

        Transform target = Playces.Find(wpm.dname);
        if (target == null)
        {
            int sort = mStaticThings.I.GetSortNumber(mStaticThings.I.mAvatarID);
            int maxnum = StartGroup.GetComponent<VRPlayceGroup>()._VRPlayceDots.Count - 1;
            int findnum = Mathf.Clamp(sort, 0, maxnum);
            target = StartGroup.GetComponent<VRPlayceGroup>()._VRPlayceDots[findnum].transform;
        }


        if (WsPlaceMarkDic.ContainsKey(wpm.id))
        {
            if (WsPlaceMarkDic[wpm.id] != target.name)
            {
                RemoveSingleDotPlayced(WsPlaceMarkDic[wpm.id]);
                WsPlaceMarkDic[wpm.id] = target.name;
            }
        }
        else
        {
            WsPlaceMarkDic.Add(wpm.id, target.name);
        }


        VRPlayceDot vp = target.GetComponent<VRPlayceDot>();
        if (vp == null) { return; }

        vp.isPlaced = true;
        vp.PlacedWSID = wpm.id;

        //Debug.LogWarning(wpm.id + "      " + mStaticThings.I.mWsID);
        if (wpm.id == mStaticThings.I.mAvatarID)
        {
            if(mStaticThings.I.MainVRROOT == null){
                return;
            }
            bool ccenabled = false;
            if( mStaticThings.I.MainVRROOT.GetComponent<CharacterController>()){
                if(mStaticThings.I.MainVRROOT.GetComponent<CharacterController>().enabled){
                    ccenabled  =true;
                    mStaticThings.I.MainVRROOT.GetComponent<CharacterController>().enabled = false;
                }
            }
            PlayceDotKind dotkind = vp.dotkind;
            mStaticThings.I.MainVRROOT.position = target.position;
            if (dotkind == PlayceDotKind.direction)
            {
                mStaticThings.I.MainVRROOT.rotation = target.rotation;
            }
            else if (dotkind == PlayceDotKind.recenter)
            {
                mStaticThings.I.MainVRROOT.rotation = target.rotation;
                MessageDispatcher.SendMessage(CommonVREventType.VRrecenter.ToString());
            }
            mStaticThings.I.MainVRROOT.localScale = target.lossyScale;

            foreach (var item in mStaticThings.I.VRCameras)
            {
                item.nearClipPlane = target.lossyScale.x * 0.1f;
                item.farClipPlane = target.lossyScale.x * 1000f;
            }

            if(ccenabled){
                mStaticThings.I.MainVRROOT.GetComponent<CharacterController>().enabled = true;
            }
            MessageDispatcher.SendMessage(this, VrDispMessageType.SelfPlaceTo.ToString(), wpm.dname, 0);
            
        }
    }
}
