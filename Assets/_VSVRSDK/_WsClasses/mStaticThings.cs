using System.Collections.Generic;
using UnityEngine;

public class mStaticThings : MonoBehaviour
{
    public string now_UnityVersion;
    public Transform Maincamera;
    public Camera[] VRCameras;
    public Transform LeftHand;
    public Transform RightHand;
    public Transform LeftTeleportAnchor;
    public Transform RightTeleportAnchor;
    public Transform LeftFingerPointerAnchor;
    public Transform RightFingerPointerAnchor;
    public Transform MainVRROOT;
    public Transform PCCamra;
    public Camera Photocamera;
    public Transform LaserPoint;
    public Transform trackfix;
    public string now_ServerURL = "vr.vswork.vip";
    public string sub_ServerURL = "eyouar.com";
    public static string apiversion = "api2";
    public static string userversion = "user2";
    public static string serverhttp = "https://";
    public static string urltokenfix = "";
    public string now_phone;
    public string now_pass;
    public string now_groupid;
    public string now_ScenePrefix;
    public bool WsAvatarIsReady = false;
    public bool AutoLogin;
    public string nowRoomID;
    public string nowRoomServerUrl;
    public string nowRoomServerGetUrl;
    public string nowRoomChID;
    public string nowRoomGMEappID;
    public string nowRoomGMEroomID;
    public string nowRoomGMEroomExID;
    public string nowRoomAdminCMD;
    public string nowRoomPass;
    public string nowRoomExEnabled;
    public string nowRoomGMEappKey;
    public int nowRoomMaxCount;
    public int nowRoomAeMaxCount;
    public int nowMicVol;
    public WsSceneInfo nowRoomLinkScene;
    public bool MicEnabled;
    public string mAvatarID;
    public string aid;
    public string startaid;
    public string mWsID = "";
    public int msex;
    public string mNickName;
    public WsSceneInfo mScene;
    public bool SelectorEnabled = false;
    public bool GMEconected = false;
    public string NowSelectedAvararid = "";
    public bool IsSelfJoinScene = false;
    public bool isVRApp = true;
    public bool ismobile;
    public string nowGroupName;
    public string ThisKODfileUrl;
    public string ThisKODfileServer;
    public int AutoPlayScene;
    public bool isAdmin;
    public bool SendAvatar;
    public Vector3 GlbSceneLoadPosition;
    public Vector3 GlbSceneLoadRotation;
    public float GlbSceneLoadScale = 1f;
    public Vector3 GlbObjLoadPosition;
    public Vector3 GlbObjLoadRotation;
    public float GlbObjLoadScale = 1f;
    public Transform GlbRoot;
    public Transform GlbSceneRoot;
    public Transform GlbOjbRoot;
    public Transform BigscreenRoot;
    public bool IsThirdCamera = false;
    public bool sadmin = false;
    public bool showvol = true;
    public bool shownamepanel = true;
    public bool ispanorama = false;
    public string nowdevicename;
    public Transform MenuAnchor;
    public bool isurp;
    public bool isdrawingon;
    public bool enhide;
    public static string apikey = "";
    public static string apitoken = "";
    public List<VRRootChanelRoom> LastIDLinkChanelRoomList = new List<VRRootChanelRoom>();
    public LanguageType NowLanguageType = LanguageType.English;

    public static Dictionary<string, WsAvatarFrame> AllStaticAvatarsDic = new Dictionary<string, WsAvatarFrame>();
    public static List<string> AllStaticAvatarList = new List<string>();
    public static List<string> AllActiveAvatarList = new List<string>();
    public static Dictionary<string, WsAvatarFrameJian> DynClientAvatarsDic = new Dictionary<string, WsAvatarFrameJian>();
    private static mStaticThings instance;
    public bool localroomserver = false;
    public bool isfpsmoving = false;
    public List<bool> movingmarklist = new List<bool>();
    public string DeviceSNnumber;
    public string nowpencolor = "";
    public bool IsUpdateDone = false;
    public string GrouplinkedroomID ="";
    public bool FirstLoadGroupRoom = true;

    public static mStaticThings I
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        isAdmin = false;
    }

    public int GetSortNumber(string wsid)
    {

        int i = AllActiveAvatarList.IndexOf(wsid);
        int cnt = AllActiveAvatarList.Count;
        return i < 0 ? cnt : i;
    }

    public List<string> GetAllStaticAvatarList()
    {
        return AllStaticAvatarList;
    }

    public List<string> GetAllActiveAvatarList()
    {
        return AllActiveAvatarList;
    }

    public List<string> GetAllStaticAvatarsDicNames()
    {
        List<string> nicknames = new List<string>();
        for(int i = 0;i<AllStaticAvatarList.Count;i++){
            if(AllStaticAvatarsDic.ContainsKey(AllStaticAvatarList[i])){
                nicknames.Add(AllStaticAvatarsDic[AllStaticAvatarList[i]].name);
            }  
        }

        return nicknames;
    }
    public List<string> GetAllActiveAvatarsDicNames()
    {
        List<string> nicknames = new List<string>();
        for(int i = 0;i<AllActiveAvatarList.Count;i++){
            if(AllStaticAvatarsDic.ContainsKey(AllActiveAvatarList[i])){
                nicknames.Add(AllStaticAvatarsDic[AllActiveAvatarList[i]].name);
            }  
        }

        return nicknames;
    }

}