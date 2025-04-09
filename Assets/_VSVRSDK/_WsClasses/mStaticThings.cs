using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class mStaticThings : MonoBehaviour
{
    /// <summary>
    /// 当前sdk可以用来打包场景的最低unity版本
    /// </summary>
    public string now_UnityVersion;
    /// <summary>
    /// 主摄像机
    /// </summary>
    public Transform Maincamera;
    /// <summary>
    /// vr相机
    /// </summary>
    public Camera[] VRCameras;
    /// <summary>
    /// vr左手
    /// </summary>
    public Transform LeftHand;
    /// <summary>
    /// vr右手
    /// </summary>
    public Transform RightHand;
    /// <summary>
    /// vr左手传送锚点
    /// </summary>
    public Transform LeftTeleportAnchor;
    /// <summary>
    /// vr右手传送锚点
    /// </summary>
    public Transform RightTeleportAnchor;
    /// <summary>
    /// vr左手手指锚点
    /// </summary>
    public Transform LeftFingerPointerAnchor;
    /// <summary>
    /// vr右手手指锚点
    /// </summary>
    public Transform RightFingerPointerAnchor;
    /// <summary>
    /// 主角的控制器（可以获取character controller）
    /// </summary>
    public Transform MainVRROOT;
    /// <summary>
    /// 自由视角相机
    /// </summary>
    public Transform PCCamra;
    /// <summary>
    /// 主角视角拍照相机
    /// </summary>
    public Camera Photocamera;
    /// <summary>
    /// vr射线检测到的物体
    /// </summary>
    public Transform LaserPoint;
    /// <summary>
    /// 人物的相对位置（MainVRROOT的子物体）
    /// </summary>
    public Transform trackfix;
    /// <summary>
    /// 登录服务器
    /// </summary>
    public string now_ServerURL = "vr.vswork.vip";
    /// <summary>
    /// 测试服务器
    /// </summary>
    public string now_ServerTestURL = "vr.vswork.vip";
    /// <summary>
    /// 中台服务器域名
    /// </summary>
    public string spaceMiddlePlatform = "s.vswork.space";
    /// <summary>
    /// 中台配置地址
    /// </summary>
    public string spaceMiddlePlatformConfigInfo = "https://s.vswork.space/space/396b798b-93f3-4604-9606-bdc576a9fe9f";
    /// <summary>
    /// 备用服务器
    /// </summary>
    public string sub_ServerURL = "eyouar.com";
    /// <summary>
    /// 接口版本
    /// </summary>
    public static string apiversion = "api3";
    /// <summary>
    /// 用户版本
    /// </summary>
    public static string userversion = "user3";
    /// <summary>
    /// http协议类型
    /// </summary>
    public static string serverhttp = "https://";
    /// <summary>
    /// 区分平台的请求
    /// </summary>
    public static string urltokenfix = "";
    /// <summary>
    /// 组织id
    /// </summary>
    public string now_groupid;
    /// <summary>
    /// 当前场景的前缀（用来区分平台的）
    /// </summary>
    public string now_ScenePrefix;
    /// <summary>
    /// 人物连接频道完成
    /// </summary>
    public bool WsAvatarIsReady = false;
    /// <summary>
    /// 自动登录
    /// </summary>
    public bool AutoLogin;
    /// <summary>
    /// 当前频道id
    /// </summary>
    public string nowRoomID;
    /// <summary>
    /// 当前动作服务器websocket url
    /// </summary>
    public string nowRoomServerUrl;
    /// <summary>
    /// 旧的动作服务器websocket url
    /// </summary>
    public string oldRoomServerUrl;
    /// <summary>
    /// 当前动作服务器的http url
    /// </summary>
    public string nowRoomServerGetUrl;
    /// <summary>
    /// 当前动作频道id
    /// </summary>
    public string nowRoomChID;
    /// <summary>
    /// 当前动作频道内子房间id(弃用)
    /// </summary>
    //[Obsolete("当前动作频道内子房间id(弃用)")]
    public string nowRoomStartChID;
    /// <summary>
    /// 当前频道是否支持语音
    /// </summary>
    public bool nowRoomVoiceUpEnabled;
    /// <summary>
    /// 当前频道语音类型（1：腾讯Gme,2：声网）
    /// </summary>
    public string nowRoomVoiceType;
    /// <summary>
    /// 当前语音的appid
    /// </summary>
    public string nowRoomGMEappID;
    /// <summary>
    /// 当前语音的房间ID
    /// </summary>
    public string nowRoomGMEroomID;
    /// <summary>
    /// 腾讯gme的ppt鉴权相关
    /// </summary>
    public bool nowRoomGMETxtEnabled;
    /// <summary>
    /// 暂时无用
    /// </summary>
    public string nowRoomVoiceAPI;
    /// <summary>
    /// 暂时无用
    /// </summary>
    public string nowRoomActionAPI;
    /// <summary>
    /// 暂时无用
    /// </summary>
    public string nowRoomTBPAPI;
    /// <summary>
    /// 当前语音子房间ID
    /// </summary>
    public string nowRoomGMEroomExID;
    /// <summary>
    /// 当前动作频道内子房间id(弃用)
    /// </summary>
    [Obsolete("当前动作频道内子房间id(弃用)")]
    public string nowRoomExChID;
    /// <summary>
    /// 当前频道的管理员命令
    /// </summary>
    public string nowRoomAdminCMD;
    /// <summary>
    /// 当前频道密码（主工程无用）
    /// </summary>
    public string nowRoomPass;
    /// <summary>
    /// 屏幕分享相关配置
    /// </summary>
    public Dictionary<string, string> nowRoomSettings = new Dictionary<string, string>();
    /// <summary>
    /// 软件热更新版本相关配置
    /// </summary>
    public Dictionary<string, string> AppVersionSettings = new Dictionary<string, string>();
    /// <summary>
    /// 当前频道是否可以下载资源
    /// </summary>
    public bool nowRoomMediaEnabled;
    /// <summary>
    /// 当前频道加载图标
    /// </summary>
    public string nowSceneLoadIcon;
    /// <summary>
    /// 当前频道加载图标展示图大图
    /// </summary>
    public string vrRoomsIconMax;
    /// <summary>
    /// 当前频道加载名称
    /// </summary>
    public string nowSceneLoadName;
    /// <summary>
    /// 加载场景时是否看见人（主要用于vr）
    /// </summary>
    public bool IsAvatarVisibleOnLoading;
    /// <summary>
    /// 腾讯gme的3d音效
    /// </summary>
    public int nowRoomEnable3dSound;
    /// <summary>
    /// 腾讯gme房间团队id
    /// </summary>
    public int nowRoomGMEroomTeamID;
    /// <summary>
    /// 当前音量范围
    /// </summary>
    public int nowRoomVoiceRange;
    /// <summary>
    /// 腾讯gme的额外功能
    /// </summary>
    public string nowRoomExEnabled;
    /// <summary>
    /// 语音房间的appkey（时效）
    /// </summary>
    public string nowRoomGMEappKey;
    /// <summary>
    /// 频道最大人数
    /// </summary>
    public int nowRoomMaxCount;
    /// <summary>
    /// 最大在线人数（超过这人数的人能进入但其他就看不见了）
    /// </summary>
    public int nowRoomAeMaxCount;
    /// <summary>
    /// 当前麦克风音量
    /// </summary>
    public int nowMicVol;
    /// <summary>
    /// 是否正在连接动作频道，连接完成为false,正在连接为true
    /// </summary>
    public bool ischconnecting;
    /// <summary>
    /// 当前频道绑定的初始场景
    /// </summary>
    public WsSceneInfo nowRoomLinkScene;
    /// <summary>
    /// 麦克风开关状态
    /// </summary>
    public bool MicEnabled;
    /// <summary>
    /// 人物的角色id
    /// </summary>
    public string mAvatarID;
    /// <summary>
    /// 人物的角色模型id
    /// </summary>
    public string aid;
    /// <summary>
    /// 人物的角色初始模型id
    /// </summary>
    public string startaid;
    /// <summary>
    /// 请求频道http url 用的socketid
    /// </summary>
    public string mWsID = "";
    /// <summary>
    /// 人物性别
    /// </summary>
    public int msex;
    /// <summary>
    /// 人物昵称
    /// </summary>
    public string mNickName;
    /// <summary>
    /// 用户所在国家
    /// </summary>
    public string userCountry;
    /// <summary>
    /// 用户手机号
    /// </summary>
    public string userPhone;
    /// <summary>
    /// 频道当前场景
    /// </summary>
    public WsSceneInfo mScene;
    /// <summary>
    /// vr激光笔是否打开
    /// </summary>
    public bool SelectorEnabled = false;
    /// <summary>
    /// 语音连接是否成功
    /// </summary>
    public bool GMEconected = false;
    /// <summary>
    /// 腾讯ptt连接状态
    /// </summary>
    public bool PTTconected = false;
    /// <summary>
    /// 当前选择人物id
    /// </summary>
    public string NowSelectedAvararid = "";
    /// <summary>
    /// 我自身是否进入场景
    /// </summary>
    public bool IsSelfJoinScene = false;
    /// <summary>
    /// 是否为vr应用
    /// </summary>
    public bool isVRApp = true;
    /// <summary>
    /// 是否是手机端
    /// </summary>
    public bool ismobile;
    /// <summary>
    /// 是否是云渲染端
    /// </summary>
    public bool isCloudRender = false;
    /// <summary>
    /// 是否是云渲染手机端
    /// </summary>
    public bool isCloudRenderMobile = false;
    /// <summary>
    /// 是否是云渲染端小程序
    /// </summary>
    public bool isCloudRenderMiniProgram = false;
    /// <summary>
    /// 当前位置组名称
    /// </summary>
    public string nowGroupName;
    /// <summary>
    /// 资源服务器地址(用于拼接)
    /// </summary>
    public string ThisKODfileUrl;
    /// <summary>
    /// 资源服务器地址（用于登录比ThisKODfileUrl多一个/）
    /// </summary>
    public string ThisKODfileServer;
    /// <summary>
    /// 设置场景自动播放
    /// </summary>
    public int AutoPlayScene;
    /// <summary>
    /// 是否为管理员
    /// </summary>
    public bool isAdmin;
    /// <summary>
    /// 是否发送角色信息（不发送其他人就看不见这个人）
    /// </summary>
    public bool SendAvatar;
    /// <summary>
    /// 设置vr物体加载位置
    /// </summary>
    public Vector3 GlbSceneLoadPosition;
    /// <summary>
    /// 设置vr物体加载旋转
    /// </summary>
    public Vector3 GlbSceneLoadRotation;
    /// <summary>
    /// 设置vr物体加载缩放
    /// </summary>
    public float GlbSceneLoadScale = 1f;
    /// <summary>
    /// 设置vr物体加载位置
    /// </summary>
    public Vector3 GlbObjLoadPosition;
    /// <summary>
    /// 设置vr物体加载旋转
    /// </summary>
    public Vector3 GlbObjLoadRotation;
    /// <summary>
    /// 设置vr物体加载缩放
    /// </summary>
    public float GlbObjLoadScale = 1f;
    /// <summary>
    /// glb模型的根
    /// </summary>
    public Transform GlbRoot;
    /// <summary>
    /// glbScene模型的根
    /// </summary>
    public Transform GlbSceneRoot;
    /// <summary>
    /// glbObj模型的根
    /// </summary>
    public Transform GlbOjbRoot;
    /// <summary>
    /// 大屏幕的根（主要用来控制位置）
    /// </summary>
    public Transform BigscreenRoot;
    /// <summary>
    /// 大屏幕本身（主要用来控制显示）
    /// </summary>
    public Transform BigscreenObj;
    /// <summary>
    /// 屏幕分享大屏（whiteBoard）
    /// </summary>
    public Transform ScreenShareObj;
    /// <summary>
    /// 全局相机
    /// </summary>
    public bool IsThirdCamera = false;
    /// <summary>
    /// 是否是助理管理员
    /// </summary>
    public bool sadmin = false;
    /// <summary>
    /// 是否显示扬声器头标
    /// </summary>
    public bool showvol = true;
    /// <summary>
    /// 是否显示头标
    /// </summary>
    public bool shownamepanel = true;
    /// <summary>
    /// 是否为全景模式
    /// </summary>
    public bool ispanorama = false;
    /// <summary>
    /// 当前设备名称(ios,mac,pc,phone,)
    /// </summary>
    public string nowdevicename;
    /// <summary>
    /// 主界面锚点
    /// </summary>
    public Transform MenuAnchor;
    /// <summary>
    /// 是否是urp渲染管线
    /// </summary>
    public bool isurp;
    /// <summary>
    /// 是否开启画笔
    /// </summary>
    public bool isdrawingon;
    /// <summary>
    /// 是否隐身
    /// </summary>
    public bool enhide;
    /// <summary>
    /// 动作服务器参数
    /// </summary>
    public static string apikey = "";
    /// <summary>
    /// 动作服务器参数
    /// </summary>
    public static string apitoken = "";
    /// <summary>
    /// 后台用户数据缓存
    /// </summary>
    public static JsonData userdata;
    /// <summary>
    /// 最新链接频道房间数据
    /// </summary>
    public List<VRRootChanelRoom> LastIDLinkChanelRoomList = new List<VRRootChanelRoom>();
    /// <summary>
    /// 当前语言
    /// </summary>
    public LanguageType NowLanguageType = LanguageType.English;
    /// <summary>
    /// 当前频道房间内的所有人物数据字典（key 为 Sys avatarID）
    /// </summary>
    public static Dictionary<string, WsAvatarFrame> AllStaticAvatarsDic = new Dictionary<string, WsAvatarFrame>();
    /// <summary>
    /// 所有Avatar ID 列表
    /// </summary>
    public static List<string> AllStaticAvatarList = new List<string>();
    /// <summary>
    /// 当前显示的Avatar ID 列表
    /// </summary>
    public static List<string> AllActiveAvatarList = new List<string>();
    /// <summary>
    /// 人物的帧同步数据
    /// </summary>
    public static Dictionary<string, WsAvatarFrameJian> DynClientAvatarsDic = new Dictionary<string, WsAvatarFrameJian>();
    private static mStaticThings instance;
    /// <summary>
    /// 是否是离线频道
    /// </summary>
    public bool localroomserver = false;
    /// <summary>
    /// Oculus VR 控制器相关
    /// </summary>
    public bool isfpsmoving = false;
    /// <summary>
    /// 全景相机相关
    /// </summary>
    public List<bool> movingmarklist = new List<bool>();
    /// <summary>
    /// 设备编号
    /// </summary>
    public string DeviceSNnumber;
    /// <summary>
    /// 当前画笔颜色
    /// </summary>
    public string nowpencolor = "041A5ACC";
    /// <summary>
    /// 场景是否下载完成
    /// </summary>
    public bool IsUpdateDone = false;
    /// <summary>
    /// 当前组织默认绑定房间id（暂时无用）
    /// </summary>
    public string GrouplinkedroomID = "";
    /// <summary>
    /// 第一次加载组织房间（暂时无用）
    /// </summary>
    public bool FirstLoadGroupRoom = true;
    /// <summary>
    /// 暂时无用
    /// </summary>
    public bool isfullweb = false;
    /// <summary>
    /// 是否隐藏场景中是所有人
    /// </summary>
    public bool IsHideAllAvatar = false;
    /// <summary>
    /// 人物的默认高度
    /// </summary>
    public float AvatarStaticHeight = 1.4f;
    /// <summary>
    /// 超过这个距离的人物删除（相对于自己）
    /// </summary>
    public float MaxHideDistance = 120f;
    /// <summary>
    /// 超过这个距离的人物隐藏（相对于自己）
    /// </summary>
    public float MaxAvatarMeshHideDistance = 80f;
    /// <summary>
    /// 距离自己的距离小于这个值隐藏（相对于自己）
    /// </summary>
    public float MinAvatarDistance = 0.3f;
    /// <summary>
    /// 如果设置了场景最多显示人数，则在可见人范围内，离我最远的距离
    /// </summary>
    public float NowFixMaxAvatarMeshHideDistance = 30f;
    /// <summary>
    /// 最大显示模型数量
    /// </summary>
    public int MaxAvatarMeshNumber = 20;
    /// <summary>
    /// 暂时无用
    /// </summary>
    public bool AirPlayEnabled = false;
    /// <summary>
    /// 暂时无用
    /// </summary>
    public bool AirPlayIson = false;
    /// <summary>
    /// 登录成功
    /// </summary>
    public bool islogedin = false;
    /// <summary>
    /// 当前频道所有人物数据缓存（大约4秒同步一次数据）
    /// </summary>
    public WsAvatarFrameList nowAvatarFrameList;
    /// <summary>
    /// 动作黑名单(Sys avatarID)
    /// </summary>
    public List<string> ActionBlackList = new List<string>();
    /// <summary>
    /// 语音黑名单(Sys avatarID)
    /// </summary>
    public List<string> VoiceBlackList = new List<string>();
    /// <summary>
    /// vr人物跳点方向
    /// </summary>
    public bool TeleportByDict = false;
    /// <summary>
    /// vr人物跳点距离
    /// </summary>
    public float TeleportDistance = 15f;
    /// <summary>
    /// vr人物跳点移动限制（禁足）
    /// </summary>
    public Dictionary<string, string> TeleportDict = new Dictionary<string, string>();
    /// <summary>
    /// 暂时无用
    /// </summary>
    public Vector3 PenPosFix = Vector3.zero;
    /// <summary>
    /// 暂时无用
    /// </summary>
    public string nowroomtoken = "";
    /// <summary>
    /// 暂时无用
    /// </summary>
    public string mBodytype = "";
    /// <summary>
    /// 播放延迟时间
    /// </summary>
    public float Playwaittime = 0.01f;
    /// <summary>
    /// 是否使用默认加载场景
    /// </summary>
    public bool UseDefaultLoadingScene = true;
    /// <summary>
    /// 是否仅管理员打开激光笔
    /// </summary>
    public bool adminlaseronly = false;
    /// <summary>
    /// 是否屏蔽激光笔
    /// </summary>
    public bool blocklaser = false;
    /// <summary>
    /// 是否自动切换第三人称视角
    /// </summary>
    public bool AutoSwitchtoThirdPerson = false;
    /// <summary>
    /// 是否第三人物视角模式
    /// </summary>
    public bool bThirdPersonMode = false;
    /// <summary>
    /// 相机近截面
    /// </summary>
    public float nearClipPlane = 0.1f;
    /// <summary>
    /// 相机远截面
    /// </summary>
    public float farClipPlane = 1000;
    /// <summary>
    /// 第三人称摄像机
    /// </summary>
    public Transform thirdPersonCamera;
    /// <summary>
    /// 自己隐身，是否对自己可见
    /// </summary>
    public bool bSelfHideVisible = true;
    /// <summary>
    /// 分享玩家数据
    /// </summary>
    public List<ScreenShareData> screenShareList = new List<ScreenShareData>();
    /// <summary>
    /// 当前应用名称
    /// </summary>
    public static string nowAppname;
    /// <summary>
    /// 当前版本号
    /// </summary>
    public static string nowVersion;

    /// <summary>
    /// 名牌显示的最大距离 超过这个值将隐藏
    /// </summary>
    public float MaxDisOfHideNamePanel = 7.0f;
    /// <summary>
    /// 当前显示的版本名
    /// </summary>
    public string vr_scenes_item_id;
    /// <summary>
    /// 当前显示的版本名
    /// </summary>
    public SceneVersionData _SceneVersionData;
    /// <summary>
    /// 名牌最大缩放值
    /// </summary>
    public float MaxNamePanelScale = 2.8f;
    /// <summary>
    /// 正在翻转
    /// </summary>
    public bool IsFliping = false;
    /// <summary>
    /// 自动寻路
    /// </summary>
    public bool IsAutoFindPath = false;
    /// <summary>
    /// 是否是竖屏
    /// </summary>
    public bool IsPortrait = false;
    /// <summary>
    /// 是否自动GC (20Mb)
    /// </summary>
    public static bool bOpenAutoGC = true;
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

 
    /// <summary>
    /// 获取当前Avatar的排序（第一个为Admin）
    /// </summary>
    /// <param name="wsid"></param>
    /// <returns></returns>
    public int GetSortNumber(string wsid)
    {

        int i = AllActiveAvatarList.IndexOf(wsid);
        int cnt = AllActiveAvatarList.Count;
        return i < 0 ? cnt : i;
    }
    /// <summary>
    /// 获取所有Avatar ID 列表
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllStaticAvatarList()
    {
        return AllStaticAvatarList;
    }
    /// <summary>
    /// 获取当前显示的Avatar ID 列表
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllActiveAvatarList()
    {
        return AllActiveAvatarList;
    }
    /// <summary>
    /// 获取所有Avatar的昵称列表
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllStaticAvatarsDicNames()
    {
        List<string> nicknames = new List<string>();
        for (int i = 0; i < AllStaticAvatarList.Count; i++)
        {
            if (AllStaticAvatarsDic.ContainsKey(AllStaticAvatarList[i]))
            {
                nicknames.Add(AllStaticAvatarsDic[AllStaticAvatarList[i]].name);
            }
        }

        return nicknames;
    }
    /// <summary>
    /// 获取当前显示的Avatar的昵称列表
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllActiveAvatarsDicNames()
    {
        List<string> nicknames = new List<string>();
        for (int i = 0; i < AllActiveAvatarList.Count; i++)
        {
            if (AllStaticAvatarsDic.ContainsKey(AllActiveAvatarList[i]))
            {
                nicknames.Add(AllStaticAvatarsDic[AllActiveAvatarList[i]].name);
            }
        }

        return nicknames;
    }
    /// <summary>
    /// 获取当前主相机
    /// </summary>
    /// <returns></returns>
    public Transform GetCurrentMainCamera()
    {
        if (bThirdPersonMode)
        {
            return thirdPersonCamera;
        }
        return Maincamera;
    }
    /// <summary>
    /// 当前房间是否需要加载场景
    /// 加载完成返回fasle
    /// </summary>
    /// <returns></returns>
    public bool CheckRoomNeedLoadScene()
    {
        if (mStaticThings.I.nowRoomLinkScene != null &&
            !string.IsNullOrEmpty(mStaticThings.I.nowRoomLinkScene.scene) &&
            mStaticThings.I.nowRoomLinkScene.scene != mStaticThings.I.mScene.scene)
            return true;
        return false;
    }
    public static Color HSVToRGB(Vector4 hsv)
    {
        Color color = Color.HSVToRGB(hsv.x, hsv.y, hsv.z);
        color.a = hsv.w;
        return color;
    }

    public static void RGBToHSV(Color color, ref Vector4 colorhsv)
    {
        Color.RGBToHSV(color, out colorhsv.x, out colorhsv.y, out colorhsv.z);
    }
    public static Color GetSaturation(Vector4 hsv, float x, float y)
    {
        Vector4 saturationHSV = hsv;
        saturationHSV.y = x;
        saturationHSV.z = y;
        saturationHSV.w = 1;
        return HSVToRGB(saturationHSV);
    }
    public static void HSVToRGBTexture(Vector4 hsv, ref Texture2D tex)
    {
        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                Color pixColor = GetSaturation(hsv, x * 1.0f / tex.width, y * 1.0f / tex.height);
                tex.SetPixel(x, y, pixColor);
            }
        }

        tex.Apply();
    }
}
