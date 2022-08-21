using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class AvatarChairMark : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public string ChairID = "";
    [HideInInspector]
    public string AvatarID = "";
    [SerializeField]
    public float SitEnableDistance = 2;
    [SerializeField]
    public GameObject ForwardTransform;

    private string SyncAvatarSitChairMsg = "SyncAvatarSitChairMsg";
    private void Awake()
    {
        if (string.IsNullOrEmpty(ChairID))
        {
            ChairID = transform.name;
        }
    }

    private void OnEnable()
    {
        MessageDispatcher.AddListener(WsMessageType.RecieveCChangeObj.ToString(), SyncAvatarSit);
    }

    private void OnDisable()
    {
        MessageDispatcher.RemoveListener(WsMessageType.RecieveCChangeObj.ToString(), SyncAvatarSit);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAvatarSitOnChair(string avatarid)
    {
        WsCChangeInfo wsinfo = new WsCChangeInfo()
        {
            a = SyncAvatarSitChairMsg,
            b = ChairID,
            c = avatarid,
        };
        MessageDispatcher.SendMessage("", WsMessageType.SendCChangeObj.ToString(), wsinfo, 0);
    }

    void SyncAvatarSit(IMessage msg)
    {
        WsCChangeInfo info = msg.Data as WsCChangeInfo;
        
        if (info.a == SyncAvatarSitChairMsg && info.b == ChairID)
        {
            if (AvatarID == "" && info.c != "")
            {
                Debug.Log("Chair SitOn Avatar = " + info.c + " chairid " + info.b);
            }
            else if (AvatarID != "" && info.c == "")
            {
                Debug.Log("Chair StandUp Avatar = " + AvatarID + " chairid " + info.b);
            }
            AvatarID = info.c;
        }
    }

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(AvatarID);
    }

}
