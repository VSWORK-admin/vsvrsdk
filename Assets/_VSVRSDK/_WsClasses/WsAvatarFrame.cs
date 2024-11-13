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

public enum BigScreenModeType
{
    screen,
    web
}
/// <summary>
/// 帧同步中的人物姿势数量简版
/// </summary>
[System.Serializable]
public class PoseFrameJian
{
    /// <summary>
    /// 本地位置
    /// </summary>
    public Vector3 hp;
    /// <summary>
    /// 本地旋转
    /// </summary>
    public Quaternion hr;
    /// <summary>
    /// 左手位置
    /// </summary>
    public Vector3 hlp;
    /// <summary>
    /// 左手旋转
    /// </summary>
    public Quaternion hlr;
    /// <summary>
    /// 右手位置
    /// </summary>
    public Vector3 hrp;
    /// <summary>
    /// 右手旋转
    /// </summary>
    public Quaternion hrr;
};
/// <summary>
/// 人物帧同步数据
/// </summary>
[Serializable]
public class WsAvatarFrame
{
    /// <summary>
    /// NikeName
    /// </summary>
    public string name;
    /// <summary>
    /// 性别
    /// </summary>
    public int sex;
    /// <summary>
    /// websocket id
    /// </summary>
    public string wsid;
    //public string scene;//SceneName
    /// <summary>
    /// 场景信息
    /// </summary>
    public WsSceneInfo scene;
    /// <summary>
    /// 是否是vr
    /// </summary>
    public bool vr;
    /// <summary>
    /// 是否是远程人物
    /// </summary>
    public bool isremote;
    /// <summary>
    /// avatar Enabled
    /// </summary>
    public bool ae;
    /// <summary>
    /// Sys avatarID(人物id)
    /// </summary>
    public string id;
    /// <summary>
    /// avatar id(人物模型id)
    /// </summary>
    public string aid;
    /// <summary>
    /// SelectorEnabled (是否开启激光笔)
    /// </summary>
    public bool e;
    /// <summary>
    /// 是否带上头显（某些vr中无效）
    /// </summary>
    public int m;//Mounted
    /// <summary>
    /// 是否是admin
    /// </summary>
    public bool a; //isAdmin
    /// <summary>
    /// 当前人物的世界坐标
    /// </summary>
    public Vector3 wp;//WorldPos
    /// <summary>
    /// 当前人物的麦克风音量
    /// </summary>
    public int vol;
    /// <summary>
    /// 当前人物的世界旋转
    /// </summary>
    public Quaternion wr;//WorldRot
    /// <summary>
    /// 当前人物的缩放
    /// </summary>
    public Vector3 ws;//WorldScale
    /// <summary>
    /// 当前人物的姿势信息
    /// </summary>
    public PoseFrameJian cp;//CurPose
    /// <summary>
    /// 当前人物的激光笔颜色
    /// </summary>
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
/// <summary>
/// 人物帧同步数据简版
/// </summary>
[Serializable]
public class WsAvatarFrameJian
{
    /// <summary>
    /// Sys avatarID(人物id)
    /// </summary>
    public string id;
    /// <summary>
    /// 模型id
    /// </summary>
    public string aid;
    /// <summary>
    /// SelectorEnabled (是否开启激光笔)
    /// </summary>
    public bool e;
    /// <summary>
    /// 是否带上头显（某些vr中无效）
    /// </summary>
    public int m;
    /// <summary>
    /// 人物姿势
    /// </summary>
    public int p;//pose
    /// <summary>
    /// 人物动作
    /// </summary>
    public int lp;
    /// <summary>
    /// 设置隐身
    /// </summary>
    public int rp;
    /// <summary>
    /// 激光笔的宽度
    /// </summary>
    public int l;
    /// <summary>
    /// 麦克风音量
    /// </summary>
    public int vol;
    /// <summary>
    /// 是否为vr应用
    /// </summary>
    public bool vr;
    /// <summary>
    /// 人物世界位置（WorldPos）
    /// </summary>
    public Vector3 wp;
    /// <summary>
    /// 人物世界旋转（WorldRot）
    /// </summary>
    public Quaternion wr;
    /// <summary>
    /// 人物缩放（WorldScale）
    /// </summary>
    public Vector3 ws;
    /// <summary>
    /// 人物姿势数据
    /// </summary>
    public PoseFrameJian cp;
    /// <summary>
    /// 当前画笔颜色
    /// </summary>
    public string cl;
}

[Serializable]
public class WsAvatarFrameList
{
    /// <summary>
    /// 频道内所有人物数据缓存（大约4秒同步一次数据）
    /// </summary>
    public List<WsAvatarFrame> alist;
    /// <summary>
    /// 当前场景
    /// </summary>
    public WsSceneInfo nowscene;
    /// <summary>
    /// 状态或数据缓存
    /// </summary>
    public Dictionary<string, string> chdata;
    /// <summary>
    /// 当前媒体数据
    /// </summary>
    public WsMediaFrame nowmedia;
    /// <summary>
    /// 当前大屏幕数据
    /// </summary>
    public WsBigScreen nowbigscreen;
}
/// <summary>
/// 帧同步数据（可调节一般小于1秒一次）
/// </summary>
[Serializable]
public class WsAvatarFrameJianList
{
    public List<WsAvatarFrameJian> jlist;
}

[Serializable]
public class WsMediaFrame
{
    /// <summary>
    /// id（一般用人物id--> Sys avatarID(人物id)）
    /// </summary>
    public string wsid;
    /// <summary>
    /// 一些介绍信息
    /// </summary>
    public string info;
    /// <summary>
    /// 文件数据组
    /// </summary>
    public List<WsMediaFile> files;
    /// <summary>
    /// 是指定的人还是所有人
    /// </summary>
    public WsMediaPostKind pkind;
    /// <summary>
    /// 指定的人的目标人物id（一般用人物id--> Sys avatarID(人物id)）
    /// </summary>
    public string towsid;
    /// <summary>
    /// 是否更新
    /// </summary>
    public bool isupdate;
}
/// <summary>
/// 内置的语音功能（非声网和腾讯GME）
/// </summary>
[Serializable]
public class WsVoiceFrame
{
    /// <summary>
    /// Sys avatarID(人物id)
    /// </summary>
    public string id;
    /// <summary>
    /// 音频数据(data)
    /// </summary>
    public string v;
    /// <summary>
    /// 音频通道（channels）
    /// </summary>
    public int ch;
    /// <summary>
    /// 音频码率(frequency)
    /// </summary>
    public int fr;
    /// <summary>
    /// 主相机的世界左边位置
    /// </summary>
    public Vector3 v3;
    /// <summary>
    /// 语音范围默认20f
    /// </summary>
    public float l;
    /// <summary>
    /// 是否压缩音频数据
    /// </summary>
    public bool z;
}
/// <summary>
/// 同步图片数据（暂时无用）
/// </summary>
[Serializable]
public class WsPicFrame
{
    /// <summary>
    /// Sys avatarID(人物id)
    /// </summary>
    public string id;
    /// <summary>
    /// 图片数据
    /// </summary>
    public string p;
    /// <summary>
    /// 
    /// </summary>
    public string tp;
    /// <summary>
    /// 宽度
    /// </summary>
    public int w;
    /// <summary>
    /// 高度
    /// </summary>
    public int h;

}
/// <summary>
/// 人物模型身体部分数据
/// </summary>
[Serializable]
public class WsAvatarBody
{
    /// <summary>
    /// Sys avatarID(人物id)
    /// </summary>
    public string id;
    /// <summary>
    /// avatar id(人物模型id)
    /// </summary>
    public string aid;
    /// <summary>
    /// 部位数据
    /// </summary>
    public Dictionary<string, string> bodypart;
}

public enum WsMediaPostKind
{
    /// <summary>
    /// 单人（指定的人）
    /// </summary>
    single,
    /// <summary>
    /// 所有人
    /// </summary>
    all
}

[Serializable]
public class WsMediaFile
{
    /// <summary>
    /// 默认为：mStaticThings.I.nowRoomServerUrl
    /// </summary>
    public string roomurl;
    /// <summary>
    /// 默认为：mStaticThings.I.ThisKODfileUrl
    /// </summary>
    public string preurl;
    /// <summary>
    /// 媒体文件地址
    /// </summary>
    public string url;
    /// <summary>
    /// 媒体文件名称
    /// </summary>
    public string name;
    /// <summary>
    /// 媒体文件大小
    /// </summary>
    public string size;
    /// <summary>
    /// 用来确保文件唯一性计算md5的时间戳（如果fileMd5字段不是32位md5）
    /// </summary>
    public string mtime;
    /// <summary>
    /// 后缀名
    /// </summary>
    public string ext;
    /// <summary>
    /// 文件32位md5
    /// </summary>
    public string fileMd5;
    /// <summary>
    /// 是否是更新
    /// </summary>
    public bool isupdate;
}


[Serializable]
public class WsGlbMediaFile
{
    /// <summary>
    /// Glb_url 地址
    /// </summary>
    public string url;
    /// <summary>
    /// 唯一签名
    /// </summary>
    public string sign;
    /// <summary>
    /// 是否是场景
    /// </summary>
    public bool isscene;
    /// <summary>
    /// 是否自动播放
    /// </summary>
    public bool autoplay;
    /// <summary>
    /// 是否异步
    /// </summary>
    public bool isasyn;
    /// <summary>
    /// 格式（glb或gltf）
    /// </summary>
    public string format;
    /// <summary>
    /// 是否自动初始化
    /// </summary>
    public bool autoinit;
    /// <summary>
    /// 加载的transform
    /// </summary>
    public Transform LoadTrasform;
}

[Serializable]
public class GlbSceneObjectFile
{
    /// <summary>
    /// 加载完成后的glb物体
    /// </summary>
    public GameObject glbobj;
    /// <summary>
    /// 唯一签名
    /// </summary>
    public string sign;
    /// <summary>
    /// 是否场景
    /// </summary>
    public bool isscene;
    /// <summary>
    /// 是否自动播放
    /// </summary>
    public bool autoplay;
    /// <summary>
    /// 是否已经初始化
    /// </summary>
    public bool isinited;
    /// <summary>
    /// glb的动画播放器
    /// </summary>
    public Animation GlbAnination;
    /// <summary>
    /// glb动画片段名列表
    /// </summary>
    public List<string> clips = new List<string>();
    /// <summary>
    /// 加载的transform
    /// </summary>
    public Transform LoadTrasform;
    /// <summary>
    /// 错误码
    /// </summary>
    public string errcode;
}

[Serializable]
public class LocalCacheFile
{
    /// <summary>
    /// 网络地址
    /// </summary>
    public string path;
    /// <summary>
    /// 是否path地址只是用来取得sign的
    /// </summary>
    public bool isURLSign;
    /// <summary>
    /// 唯一签名
    /// </summary>
    public string sign;
    /// <summary>
    /// 是否有名称前程（一般是mStaticThings.I.now_ScenePrefix）
    /// </summary>
    public bool hasPrefix;
    /// <summary>
    /// 是否是文件服务器资源
    /// </summary>
    public bool isKOD;
}
/// <summary>
/// 语音初始化数据
/// </summary>
[Serializable]
public class VRVoiceInitConfig
{
    public string appid;
    public string appkey;
    public string roomid;
    public bool initPTT;
    public string userid;
}

/// <summary>
/// 频道房间连接完成后返回的数据
/// </summary>
[Serializable]
public class ConnectAvatars
{
    /// <summary>
    /// 请求频道http url 用的socketid
    /// </summary>
    public string wsid;
    /// <summary>
    /// 排序，0是管理员
    /// </summary>
    public int sort;
    /// <summary>
    /// 当前激光笔颜色
    /// </summary>
    public string cl;
    /// <summary>
    /// 当前场景中的人物数据
    /// </summary>
    public List<WsAvatarFrame> sceneavatars;
    /// <summary>
    /// 当前场景数据
    /// </summary>
    public WsSceneInfo nowscene;
    /// <summary>
    /// 缓存的状态或配置数据
    /// </summary>
    public Dictionary<string, string> chdata;
    /// <summary>
    /// 当前媒体资源数据
    /// </summary>
    public WsMediaFrame nowmedia;
    /// <summary>
    /// 当前大屏幕数据
    /// </summary>
    public WsBigScreen nowbigscreen;
}
/// <summary>
/// 服务器缓存数据
/// </summary>
[Serializable]
public class VRSendSaveData
{
    public string id;
    public bool sall;
    public string key;
    public string value;
}
/// <summary>
/// 切换管理员数据
/// </summary>
[Serializable]
public class WsAdminMark
{
    public string name;
    public string id;
}

/// <summary>
/// 位置点相关信息
/// </summary>
[Serializable]
public class WsPlaceMark
{
    /// <summary>
    ///  PlayceDot Name
    /// </summary>
    public string dname;
    /// <summary>
    /// Avatar ID
    /// </summary>
    public string id;
}

/// <summary>
/// 位置点相关信息
/// </summary>
[Serializable]
public class WsPlaceMarkList
{
    public WsPlaycePortKind kind;
    /// <summary>
    /// Avatar ID
    /// </summary>
    public string id;
    /// <summary>
    /// PlayceGroup Name
    /// </summary>
    public string gname;
    public List<WsPlaceMark> marks;
}

public class PlacePortObj
{
    public GameObject nowselectobj;
    /// <summary>
    ///  PlayceGroup Name
    /// </summary>
    public WsTeleportKind telekind;
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
/// <summary>
/// 同步网络数据用
/// </summary>
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
/// <summary>
/// 同步网络数据用
/// </summary>
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
/// <summary>
/// 场景数据
/// </summary>
[Serializable]
public class WsSceneInfo
{
    /// <summary>
    /// 场景id
    /// </summary>
    public string id;
    /// <summary>
    /// 场景路径
    /// </summary>
    public string scene;
    /// <summary>
    /// 场景名称
    /// </summary>
    public string name;
    /// <summary>
    /// 版本号
    /// </summary>
    public string version;
    /// <summary>
    /// 是否网络场景
    /// </summary>
    public bool isremote;
    /// <summary>
    /// 是否是更新
    /// </summary>
    public bool isupdate;
    /// <summary>
    /// 是否来自资源管理器
    /// </summary>
    public bool iskod;
    /// <summary>
    /// 图标地址
    /// </summary>
    public string icon;
    /// <summary>
    /// 资源管理器媒体数据
    /// </summary>
    public WsMediaFile kod;
    /// <summary>
    /// 加密信息
    /// </summary>
    public string cryptAPI;
    /// <summary>
    /// 加密类型
    /// </summary>
    public int ckind;
}

public class URLIDSceneInfo
{
    public string server;
    public string id;
    public bool isnowserver;
    public bool update;
}
/// <summary>
/// 同步移动物体数据
/// </summary>
[Serializable]
public class WsMovingObj
{
    /// <summary>
    /// 人物的角色id
    /// </summary>
    public string id;
    /// <summary>
    /// GameObject的name
    /// </summary>
    public string name;
    /// <summary>
    /// 是相对位置（localposition）还是绝对位置（position）
    /// </summary>
    public bool islocal;
    /// <summary>
    /// 移动命令（已使用：i表示移动旋转缩放，s表示关闭重力，e表示开启重力）
    /// </summary>
    public string mark;
    /// <summary>
    /// 位置
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// 旋转
    /// </summary>
    public Quaternion rotation;
    /// <summary>
    /// 缩放
    /// </summary>
    public Vector3 scale;
}
/// <summary>
/// 同步大屏幕信息
/// </summary>
[Serializable]
public class WsBigScreen
{
    /// <summary>
    /// 人物的角色id
    /// </summary>
    public string id;
    /// <summary>
    /// 大屏幕开关
    /// </summary>
    public bool enabled;
    /// <summary>
    /// 大屏幕曲率
    /// </summary>
    public int angle;
    /// <summary>
    /// 位置
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// 旋转
    /// </summary>
    public Quaternion rotation;
    /// <summary>
    /// 缩放
    /// </summary>
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
/// <summary>
/// 同步进度数据
/// </summary>
public class WsProgressInfo
{
    /// <summary>
    /// 人物的角色id
    /// </summary>
    public string wsid;
    /// <summary>
    /// 名称
    /// </summary>
    public string name;
    /// <summary>
    /// 唯一签名
    /// </summary>
    public string sign;
    /// <summary>
    /// 进度
    /// </summary>
    public float progress;
    /// <summary>
    /// 是否是场景加载
    /// </summary>
    public bool issceneload;
}

public class VRProgressInfo
{
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
/// <summary>
/// vr手柄振动相关数据
/// </summary>
[Serializable]
public class Vibrationinfo
{
    public HandModelType hand;
    public float frequency;
    public float amplitude;
    public float lasttime;
}
/// <summary>
/// ump播放器数据
/// </summary>
[Serializable]
public class CustomVideoPlayer
{
    /// <summary>
    /// ump播放控制器（上面要挂UniversalMediaPlayer脚本）
    /// </summary>
    public GameObject ContorlObj;
    /// <summary>
    /// 播放器播放类型默认为UMP插件
    /// </summary>
    public VideoPlayerKind videoPlayerKind= VideoPlayerKind.UMP;
    /// <summary>
    /// 用来渲染的物体
    /// </summary>
    public GameObject[] RenderObj;
    /// <summary>
    /// 播放器地址
    /// </summary>
    public string url;
    /// <summary>
    /// 音量
    /// </summary>
    public float vol;
    /// <summary>
    /// 是否循环播放
    /// </summary>
    public bool isloop;
    /// <summary>
    /// 是否自动开始播放
    /// </summary>
    public bool autostart;
    /// <summary>
    /// init准备期间是否播放系统默认图片
    /// </summary>
    public bool InitShowDefaultPIC;

    /// <summary>
    /// 纹理属性名 默认为"_MainTex" 根据shader设置 比如lit的纹理为"_BaseMap"
    /// </summary>
    public string texturePropertyName;

    /// <summary>
    /// 跳转到值（时长）
    /// </summary>
    public float seekTime;
    /// <summary>
    /// 是否静音
    /// </summary>
    public bool isMute;
    /// <summary>
    /// 总时长
    /// </summary>
    public double DurationTime;
    /// <summary>
    /// 当前时长
    /// </summary>
    public double CurrentTime;
    /// <summary>
    /// 是否在播放
    /// </summary>
    public bool IsPlaying;

}
public enum VideoPlayerKind
{
    Ffmpeg,
    VideoPlayer,
    Avpro,
    UMP
}

[Serializable]
public class urporstandardOBJ
{
    public GameObject mObj;
    public Material StandardMat;
    public Material UrpMat;
}
/// <summary>
/// 切换频道内房间用
/// </summary>
[Serializable]
public class VRChanelRoom
{
    /// <summary>
    /// 人物的角色id
    /// </summary>
    public string aid;
    /// <summary>
    /// 房间id
    /// </summary>
    public string roomid;
    /// <summary>
    /// 请求频道http url 用的socketid
    /// </summary>
    public string wsid;
}
/// <summary>
/// 切换频道用
/// </summary>
[Serializable]
public class VRRootChanelRoom
{
    /// <summary>
    /// 频道id
    /// </summary>
    public string roomid;
    /// <summary>
    /// 语音id
    /// </summary>
    public string voiceid;
    /// <summary>
    /// 令牌
    /// </summary>
    public string token;
    /// <summary>
    /// 
    /// </summary>
    public bool samescene = false;
    /// <summary>
    /// 
    /// </summary>
    public string loadingscene = "";
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


public class VRLoadSceneParam
{
    public VRWsRemoteScene localscene;
    public bool iskod;
    public WsMediaFile kodscene;
    public bool isupdatesyse;
    public bool ism;
}

[Serializable]
public class VRSaveRoomData
{
    public bool isclear;
    public bool sall;
    public bool forever;
    public string key;
    public string value;
}

public class VRGLBObjectData
{
    public GameObject result;
    public AnimationClip[] ani;
    public Transform loadtrans;
    public string sign;
    public bool isscene;
}

public class GifRecordData
{
    /// <summary>
    /// 宽度
    /// </summary>
    public int width;
    /// <summary>
    /// 高度
    /// </summary>
    public int height;
    /// <summary>
    /// 1-30
    /// </summary>
    public int FramePerSecond;
    /// <summary>
    /// 时长
    /// </summary>
    public int TimeSecond;
    /// <summary>
    /// 是否循环
    /// </summary>
    public bool bRepeat;
}
[Serializable]
public class LoadAvatarOBJ
{
    /// <summary>
    /// 模型ID
    /// </summary>
    public string aid;
    public GameObject avatarModel;
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
    enmount,
    serverinputon,
    serverinputoff,
    logon,
    logoff
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
        for (int i = 0; i < md5Data.Length; i++)
        {
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
        ext = ext.ToLower();
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
        if (ext.ToLower() == "scene")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsXSceneBundle(string ext)
    {
        if (ext.ToLower() == "xscene")
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
        ext = ext.ToLower();
        if (ext == "mov" || ext == "MOV" || ext == "mp4" || ext == "mkv" || ext == "m4v")
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
        if (ext.ToLower() == "bundle")
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
        ext = ext.ToLower();
        if (ext == "glb" || ext == "gltf")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsTxt(string ext)
    {
        ext = ext.ToLower();
        if (ext == "txt" || ext == "json")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsVrOrder(string ext)
    {
        if (ext.ToLower() == "order")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsPDF(string ext)
    {
        if (ext.ToLower() == "pdf")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsPPT3d(string ext)
    {
        if (ext.ToLower() == "vrppt")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsCacheOrder(string ext)
    {
        if (ext.ToLower() == "cache")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsLinkOrder(string ext)
    {
        if (ext.ToLower() == "link")
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public static bool IsKodScene(string filename)
    {
        if (filename.StartsWith("a") || filename.StartsWith("w") || filename.StartsWith("i") || filename.StartsWith("b") || filename.StartsWith("x") || filename.StartsWith("j") || filename.StartsWith("m") || filename.StartsWith("n"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static string getrealRoomGetServer(string roomstring)
    {
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

    public static String displayFileSize(long size)
    {
        if (size >= GB)
        {
            return ((float)size / GB).ToString("F1") + " GB";
        }
        else if (size >= MB)
        {
            return ((float)size / MB).ToString("F1") + " MB";
        }
        else if (size >= KB)
        {
            return ((float)size / KB).ToString("F1") + " KB";
        }
        else
        {
            return ((float)size).ToString("F1") + " B";
        }
    }

}