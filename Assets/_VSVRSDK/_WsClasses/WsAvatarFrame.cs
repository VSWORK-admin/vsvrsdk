using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
public enum InfoColor
{
    black,
    green,
    red,
    yellow
}

public enum LanguageType : byte
{
    English = 0,
    Chinese,
    Korean,
    Japanese,
    LangA,
    LangB,
    LangC,
    LangD,
    LangE,
    LangF,
    langG,
    langH,
    langI,
    langJ,
    langK,
    langL,
    langM,

    langN
    
}

public enum CanvasWebviewActionType
{
    click,
    scroll,
    loadurl,
    refresh,
    back,
    forward,
    setres,
    resize,
    zoomin,
    zoomout,
    input
}

[Serializable]
public class CanvasWebviewActionFrame
{
    public string id;
    public string cname;
    public CanvasWebviewActionType type;
    public Vector2 v2;
    public string str;
    public float fl;

}

public enum BigScreenModeType{
    screen,
    web
}



[Serializable]
public class WsAvatarFrame
{
    public string name;//NikeName
    public int sex;
    public string wsid;//WsID
    //public string scene;//SceneName
    public WsSceneInfo scene;
    public bool vr;
    public bool isremote;
    public bool ae;//avatar Enabled
    public string id;//Sys avatarID
    public string aid;//avatar id
    public bool e;//SelEnable
    public int m;//Mounted
    public bool a; //isAdmin
    public Vector3 wp;//WorldPos
    public int vol;
    public Quaternion wr;//WorldRot
    public Vector3 ws;//WorldScale
    public PoseFrameJian cp;//CurPose
    public string cl;

    public WsAvatarFrameJian ToJian()
    {
        WsAvatarFrameJian newj = new WsAvatarFrameJian
        {
            e = e,
            id = id,
            vol = vol,
            wp = wp,
            wr = wr,
            ws = ws,
            cp = cp,
            cl = cl
        };
        return newj;
    }
}

[Serializable]
public class WsAvatarFrameJian
{
    public string id;//AvatarID
    public string aid; 
    public bool e;//SelEnable
    public int m;//Mounted
    public int p;//pose
    public int lp;//left handpos
    public int rp;//right handpos
    public int l;//laser width
    public int vol;
    public bool vr;//Is VRapp
    public Vector3 wp;//WorldPos
    public Quaternion wr;//WorldRot
    public Vector3 ws;//WorldScale
    public PoseFrameJian cp;//CurPose
    public string cl;
}

public class PoseFrameJian
{
    public Vector3 hp;
    public Quaternion hr;
    public Vector3 hlp;
    public Quaternion hlr;
    public Vector3 hrp;
    public Quaternion hrr;
};

[Serializable]
public class WsAvatarFrameList
{
    public List<WsAvatarFrame> alist;
    public WsSceneInfo nowscene;
    public Dictionary<string,string> chdata;
    public WsMediaFrame nowmedia;
    public WsBigScreen nowbigscreen;
}

[Serializable]
public class WsAvatarFrameJianList
{
    public List<WsAvatarFrameJian> jlist;
}

[Serializable]
public class WsMediaFrame
{
    public string wsid;
    public string info;
    public List<WsMediaFile> files;
    public WsMediaPostKind pkind;
    public string towsid;
    public bool isupdate;
}

[Serializable]
public class WsVoiceFrame
{
    public string id;
    public string v;
    public int ch;
    public int fr;
    public Vector3 v3;
    public float l;
    public bool z;
}

[Serializable]
public class WsPicFrame
{
    public string id;
    public string p;
    public string tp;
    public int w;
    public int h;

}

public enum WsMediaPostKind
{
    single,
    all
}

[Serializable]
public class WsMediaFile
{
    public string roomurl;
    public string preurl;
    public string url;
    public string name;
    public string size;
    public string mtime;
    public string ext;
    public string fileMd5;
    public bool isupdate;
}


[Serializable]
public class WsGlbMediaFile
{
    public string url;
    public string sign;
    public bool isscene;
    public bool autoplay;
    public bool isasyn;
    public string format;
    public bool autoinit;
    public Transform LoadTrasform;
}

[Serializable]
public class GlbSceneObjectFile
{
    public GameObject glbobj;
    public string sign;
    public bool isscene;
    public bool autoplay;
    public bool isinited;
    public Animation GlbAnination;
    public List<string> clips = new List<string>();
    public Transform LoadTrasform;
    public string errcode;
}

[Serializable]
public class LocalCacheFile
{
    public string path;
    public bool isURLSign;
    public string sign;
    public bool hasPrefix;
    public bool isKOD;
}

[Serializable]
public class VRVoiceInitConfig
{
    public string appid;
    public string appkey;
    public string roomid;
    public bool initPTT;
    public string userid;
}


[Serializable]
public class ConnectAvatars
{
    public string wsid;
    public int sort;
    public string cl;
    public List<WsAvatarFrame> sceneavatars;
    public WsSceneInfo nowscene;
    public Dictionary<string,string> chdata;
    public WsMediaFrame nowmedia;
    public WsBigScreen nowbigscreen;
}

[Serializable]
public class VRSendSaveData
{
    public string id;
    public bool sall;
    public string key;
    public string value;
}

[Serializable]
public class WsAdminMark
{
    public string name;
    public string id;
}


[Serializable]
public class WsPlaceMark
{
    public string dname; //PlayceDot Name
    public string id;  //Avatar ID
}


[Serializable]
public class WsPlaceMarkList
{
    public WsPlaycePortKind kind;
    public string id;  //Avatar ID
    public string gname; //PlayceGroup Name
    public List<WsPlaceMark> marks;
}

public class PlacePortObj
{
    public GameObject nowselectobj;
    public WsTeleportKind telekind; //PlayceGroup Name
    public bool isGroup;
    public bool isArrow;
}


[Serializable]
public class WsTeleportInfo
{
    public WsTeleportKind TeleKind;
    public WsTeleTransform TeleTarget;
    public bool isArrow;
}


[Serializable]
public class WsMultiTeleportInfo
{
    public string id;
    public WsTeleportKind TeleKind;
    public bool isArrow;
    public List<WsTeleTransform> AllTeleTransforms = new List<WsTeleTransform>();
}

[Serializable]
public class WsTeleTransform
{
    public string id;
    public string objname;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Quaternion originalrotation;
}

[Serializable]
public enum WsTeleportKind
{
    myself,
    single,
    all
}

[Serializable]
public enum WsPlaycePortKind
{
    single,
    all
}

public enum PlayceDotKind
{
    normal,
    direction,
    recenter
}

[Serializable]
public class WsChangeInfo
{
    public string id;
    public string name;
    public string kind;
    public string changenum;
    public string a;
    public string b;
    public string c;
    public string d;
    public string e;
}

[Serializable]
public class WsCChangeInfo
{
    public string a;
    public string b;
    public string c;
    public string d;
    public string e;
    public string f;
    public string g;
}

[Serializable]
public class WsSceneInfo
{
    public string id;
    public string scene;
    public string name;
    public string version;
    public bool isremote;
    public bool isupdate;
    public bool iskod;
    public string icon;
    public WsMediaFile kod;
    public string cryptAPI;
    public int ckind;
}

public class URLIDSceneInfo
{
    public string server; 
    public string id; 
    public bool isnowserver; 
    public bool update;
}

[Serializable]
public class WsMovingObj
{
    public string id;
    public string name;
    public bool islocal;
    public string mark;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

[Serializable]
public class WsBigScreen
{
    public string id;
    public bool enabled;
    public int angle;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

[Serializable]
public class WsGMEInfo
{
    public string appID;
    public string roomID;
    public string appKey;
}


[Serializable]
public class CameraScreenInfo
{    
    public string sendid;
    public bool isfree;
    public bool ismyself;
    public string lockwsid;
    public float view;
    public float near;
    public float far;
    public Vector3 position;
    public Quaternion rotation;
}

public class WsProgressInfo{
    public string wsid;
    public string name;
    public string sign;
    public float progress;
    public bool issceneload;
}

public class VRProgressInfo{
    public string name;
    public string sign;
    public float progress;
    public bool isdone;
}

public class UserSpeakInfo
{
    public string id;
    public int status;
}

public class AudioRecodingInfo
{
    public int maxlenth;
    public string speachlang;
    public string translang;
}

public class AudioRecodingResult
{
    public int code;
    public string fileid;
    public string filePath;
    public string result;
}


public class PlayceEnterMessage
{
    public WsTeleportKind teleportKind;
    public GameObject coll;
    public string wsid;
}


[Serializable]
public class TempAvatar
{
    public string avatarid;
    public GameObject avatarprefab;
}

[Serializable]
public class Vibrationinfo{
    public HandModelType hand;
    public float frequency;
    public float amplitude;
    public float lasttime;
}

[Serializable]
public class CustomVideoPlayer{
    public GameObject ContorlObj;
    public GameObject[] RenderObj;
    public string url;
    public float vol;
    public bool isloop;
    public bool autostart;
}

[Serializable]
public class urporstandardOBJ{
    public GameObject mObj;
    public Material StandardMat;
    public Material UrpMat;
}

[Serializable]
public class VRChanelRoom{
    public string aid;
    public string roomid;
    public string wsid;
}

[Serializable]
public class VRRootChanelRoom{
    public string roomid;
    public string voiceid;
}

public class VRWsRemoteScene
{
    public string id { get; set; }
    public string name { get; set; }
    public string path { get; set; }
    public string version { get; set; }
    public string intro { get; set; }
    public string icon { get; set; }
    public string bundle { get; set; }
    public string localpath { get; set; }
}


public class VRLoadSceneParam{
    public VRWsRemoteScene localscene;
    public bool iskod;
    public WsMediaFile kodscene;
    public bool isupdatesyse;
    public bool ism ;
}

[Serializable]
public class VRSaveRoomData{
    public bool isclear;
    public bool sall;
    public bool forever;
    public string key;
    public string value;
}

public class VRGLBObjectData{
    public GameObject result;
    public AnimationClip[] ani;
    public Transform loadtrans;
    public string sign;
    public bool isscene;
}

public class VRAvatarPoseData {
    public int pose;
    public int lpose;
    public int rpose;
}


public enum VROrderName
{
    admin1,
    fpson,
    fpsoff,
    cscene,
    cfile,
    cavatar,
    csystem,
    clogin,
    selon,
    seloff,
    selopen,
    selclose,
    placeon,
    placeoff,
    logout,
    selout,
    login,
    q3,
    q2,
    q1,
    q0,
    micbig,
    micsmall,
    micon,
    micoff,
    micmax,
    spemax,
    speakbig,
    speaksmall,
    speakon,
    speakoff,
    fps0,
    fps1,
    fps2,
    fps3,
    fps4,
    fps5,
    fps6,
    fps7,
    fps8,
    fps9,
    fps10,
    tipon,
    tipoff,
    startscene,
    panelon,
    paneloff,
    autologin,
    manlogin,
    memon,
    memoff,
    gc,
    autoon,
    autooff,
    level1,
    level2,
    level3,
    afpson,
    afpsoff,
    cancel,
    cglbs,
    cglbo,
    glbson,
    sadmin,
    cadmin,
    add1,
    min1,
    add5,
    min5,
    hreset,
    shown,
    hiden,
    showv,
    hidev,
    openp,
    closep,
    activep,
    deactivep,
    clearp,
    pon,
    poff,
    hideavatar,
    unhideavatar,
    urpon,
    urpoff,
    scon,
    scoff,
    avon,
    avoff,
    avaon,
    avaoff,
    micenable,
    micdisable,
    camt,
    camf,
    dismount,
    enmount
}


public class VRUtils
{
    private static char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    /// <summary>
    /// 生成0-z的随机字符串
    /// </summary>
    /// <param name="length">字符串长度</param>
    /// <returns>随机字符串</returns>
    public static string GenerateRandomString(int length)
    {
        string checkCode = String.Empty;
        System.Random rd = new System.Random();
        for (int i = 0; i < length; i++)
        {
            checkCode += constant[rd.Next(36)].ToString();
        }
        return checkCode;
    }


    public static string GetRandomUserID()
    {
        string id = "";
        while (id.Length != 8)
        {
            id += UnityEngine.Random.Range(1, 9) + "";
        }
        return id;
    }


    public static string GetMD5(string msg)
    {  
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();  
        byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);  
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);  
        md5.Clear();  

        string destString = "";  
        for (int i = 0; i < md5Data.Length; i++) {  
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');  
        }  
        destString = destString.PadLeft(32, '0');  
        return destString;
    }


    public static bool ValidateIPAddress(string ipAddress)
    {
        //Regex validipregex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
        //bool a = (ipAddress != "" && validipregex.IsMatch(ipAddress.Trim())) ? true : false;

        bool b = Regex.IsMatch(ipAddress, "[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+.?");

        return b;

    }

    public static bool IsPicture(string ext)
    {
        if (ext == "jpg" || ext == "png" || ext == "jpeg" || ext == "bmp" || ext == "tga")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsSceneBundle(string ext)
    {
        if (ext == "scene" )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsXSceneBundle(string ext){
        if (ext == "xscene")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsMOV(string ext)
    {
        if (ext == "mov" || ext == "MOV" || ext == "mp4" || ext == "mkv"|| ext == "m4v")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsObjBundle(string ext)
    {
        if (ext == "bundle")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsGlb(string ext)
    {
        if (ext == "glb" || ext == "gltf")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsTxt(string ext){
        if(ext == "txt" || ext == "json" ){
            return true;
        }else{
            return false;
        }
    }

    public static bool IsVrOrder(string ext){
        if(ext == "order"){
            return true;
        }else{
            return false;
        }
    }

     public static bool IsPDF(string ext){
        if(ext == "pdf"){
            return true;
        }else{
            return false;
        }
    }

    public static bool IsPPT3d(string ext){
        if(ext == "vrppt"){
            return true;
        }else{
            return false;
        }
    }

    public static bool IsCacheOrder(string ext){
        if(ext == "cache"){
            return true;
        }else{
            return false;
        }
    }

    public static bool IsLinkOrder(string ext){
        if(ext == "link"){
            return true;
        }else{
            return false;
        }
    }



    public static bool IsKodScene(string filename)
    {
        if (filename.StartsWith("a") || filename.StartsWith("w") || filename.StartsWith("i") || filename.StartsWith("b") || filename.StartsWith("x")|| filename.StartsWith("j") ||filename.StartsWith("m") ||filename.StartsWith("n"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static string getrealRoomGetServer(string roomstring){
        string thisroomstring;

        if (roomstring.Contains("@"))
        {
            string[] roomarr = roomstring.Split('@');

            if (roomstring.StartsWith("ws://"))
            {
                thisroomstring = "http://" + mStaticThings.I.now_ServerURL + roomarr[1];
            }
            else if (roomstring.StartsWith("wss://"))
            {
                thisroomstring = "https://" + mStaticThings.I.now_ServerURL + roomarr[1];
            }
            else
            {
                thisroomstring = "http://" + mStaticThings.I.now_ServerURL + roomarr[1];
            }
        }
        else
        {
            if (roomstring.StartsWith("ws://"))
            {
                thisroomstring = "http://" + roomstring.Substring(5, roomstring.Length - 5);
            }
            else if (roomstring.StartsWith("wss://"))
            {
                thisroomstring = "https://" + roomstring.Substring(6, roomstring.Length - 6);
            }
            else
            {
                thisroomstring = "http://" + roomstring;
            }
        }
        return thisroomstring;
    }

    public static long KB = 1024;  
    public static long MB = KB * 1024;  
    public static long GB = MB * 1024;  
    
    public static String displayFileSize(long size) {  
        if (size >= GB) {  
            return ((float)size / GB).ToString("F1") + " GB";  
        } else if (size >= MB) {  
            return ((float)size / MB).ToString("F1") + " MB";   
        } else if (size >= KB) {  
            return ((float)size / KB).ToString("F1")+ " KB";    
        } else {  
            return ((float)size).ToString("F1")+ " B";     
        }  
    } 
    
}