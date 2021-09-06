using System.Collections.Generic;
using UnityEngine;
using LitJson;

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
    public string now_groupid;
    public string now_ScenePrefix;
    public bool WsAvatarIsReady = false;
    public bool AutoLogin;
    public string nowRoomID;
    public string nowRoomServerUrl;
    public string nowRoomServerGetUrl;
    public string nowRoomChID;
    public bool nowRoomVoiceUpEnabled;
    public string nowRoomStartChID;
    public string nowRoomVoiceType;
    public string nowRoomGMEappID;
    public string nowRoomGMEroomID;
    public bool nowRoomGMETxtEnabled;
    public string nowRoomVoiceAPI;
    public string nowRoomActionAPI;
    public string nowRoomTBPAPI;
    public string nowRoomGMEroomExID;
    public string nowRoomExChID;
    public string nowRoomAdminCMD;
    public string nowRoomPass;
    public Dictionary<string,string> nowRoomSettings = new Dictionary<string, string>();
    public Dictionary<string, string> AppVersionSettings = new Dictionary<string, string>();
    public bool nowRoomMediaEnabled;
    public string nowSceneLoadIcon;
    public string nowSceneLoadName;
    public bool IsAvatarVisibleOnLoading;
    public int nowRoomEnable3dSound;
    public int nowRoomGMEroomTeamID;
    public int nowRoomVoiceRange;
    public string nowRoomExEnabled;
    public string nowRoomGMEappKey;
    public int nowRoomMaxCount;
    public int nowRoomAeMaxCount;
    public int nowMicVol;
    public bool ischconnecting;
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
    public bool PTTconected = false;
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
    public static JsonData userdata;
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
    public bool isfullweb = false;
    public bool IsHideAllAvatar = false;
    public float AvatarStaticHeight = 1.4f;
    public float MaxHideDistance = 120f;
    public float MaxAvatarMeshHideDistance = 80f;

    public bool AirPlayEnabled = false;
    public bool AirPlayIson = false;
    public bool islogedin = false;
    

    public WsAvatarFrameList nowAvatarFrameList;

    public List<string> ActionBlackList = new List<string>();
    public List<string> VoiceBlackList = new List<string>();
    public bool TeleportByDict = false;
    public float TeleportDistance = 15f;
    public Dictionary<string,string> TeleportDict = new Dictionary<string,string>();

    public Vector3 PenPosFix = Vector3.zero;

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