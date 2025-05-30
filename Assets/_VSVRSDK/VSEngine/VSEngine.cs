using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VSWorkSDK.Data;
using VSWorkSDK.Enume;

namespace VSWorkSDK
{
    public class VSEngine
    {
        private static VSEngine _instance;
        public static VSEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VSEngine();
                }

                return _instance;
            }
        }

        #region Event
        /// <summary>
        /// 登录成功事件
        /// </summary>
        public event Action<LoginData> OnEventLoginSuccess;
        /// <summary>
        /// 登录失败事件
        /// </summary>
        public event Action OnEventLoginFaild;
        /// <summary>
        /// 退出登录事件
        /// </summary>
        public event Action OnEventLoginOut;
        /// <summary>
        /// 扩展事件（用于热更新的接口向SDK场景传递数据）
        /// 参数 string 为事件类型标识
        /// 参数 List 为事件数据 
        /// </summary>
        public event Action<string, List<object>> OnEventSystemExpandEvent;
        /// <summary>
        /// 接收场景数据保存事件 
        /// 调用RequestRoomSavedData请求房间存储的数据后 触发事件
        /// </summary>
        public event Action<Dictionary<string, string>> OnEventReceiveRoomSavedData;
        /// <summary>
        /// 接收场景数据同步事件 
        /// 调用SendRoomSyncData 向房间内的其他用户同步数据，房间内所有人都将接收到
        /// RoomSycnData 一般 a: 存放自定义消息的key 用于区别不同的消息
        /// </summary>
        public event Action<RoomSycnData> OnEventReceiveRoomSyncData;
        /// <summary>
        /// 场景实例化avatar角色事件
        /// 开始实例化某个角色时触发  自己进入房间后房间内的人物都将依次实例化
        /// </summary>
        public event Action<WsAvatarFrame> OnEventRoomInitAvatar;
        /// <summary>
        /// 场景接收到新的人物事件
        /// 有新人进入房间触发
        /// </summary>
        public event Action<WsAvatarFrame> OnEventRoomEnterNewAvatar;
        /// <summary>
        /// avatar角色模型加载成功事件
        /// 参数 Transform 是实例化的模型驱动器节点 该节点的名称以该用户的实际avatarid命名
        /// </summary>
        public event Action<Transform> OnEventAvatarLoadDone;
        /// <summary>
        /// avatar角色数据修改事件  
        /// 名字、模型、换装修改后触发
        /// </summary>
        public event Action<string> OnEventAvatarChanged;
        /// <summary>
        /// 选择avatar角色事件
        /// 参数 被选择的角色ID
        /// </summary>
        public event Action<string> OnEventSelectAvatar;
        /// <summary>
        /// 销毁avatar角色事件
        /// 该人物离开房间时触发
        /// 参数 string 是人物avatarid
        /// </summary>
        public event Action<string> OnEventDestroyAvatar;
        /// <summary>
        /// 删除avatar模型事件
        /// 人物模型即将被销毁时触发 
        /// 参数 GameObject 是要被销毁的驱动器节点
        /// </summary>
        public event Action<GameObject> OnEventDestroyAvatarModel;
        /// <summary>
        /// 加载FBX avatar角色模型事件
        /// LoadFbxAvatarModel 请求加载fbx人物模型，加载成功后触发
        /// </summary>
        public event Action<LoadAvatarOBJ> OnEventLoadedFbxAvatarModel;
        /// <summary>
        /// 切换相机视角事件 
        /// SwitchCameraViewMode 切换第一第三人称后回调
        /// 参数 1 代表当前是第一人称 3 是当前是第三人称
        /// </summary>
        public event Action<int> OnEventSwitchCameraViewMode;
        /// <summary>
        /// 管理员改变事件
        /// 参数 ConnectAvatars sceneavatars[0].id是当前管理员
        /// </summary>
        public event Action<ConnectAvatars> OnEventAdminChanged;
        /// <summary>
        /// 自己admin状态改变事件
        /// </summary>
        public event Action<bool> OnEventSelfAdminStatusChanged;
        /// <summary>
        /// 大屏幕显示图片事件
        /// 大屏幕图片更换后触发，参数是大屏当前展示的图
        /// </summary>
        public event Action<Texture2D> OnEventBigScreenShowImage;
        /// <summary>
        /// 大屏幕加载视屏已准备事件
        /// LoadBigScreenMovieFile 大屏播放视频请求，视频先缓存到本地，缓存完成后触发
        /// 参数是视频地址
        /// </summary>
        public event Action<string> OnEventBigScreenPrepareVideo;
        /// <summary>
        /// 大屏幕接收到RTSP推流事件
        /// 大屏播放推流请求回调，推流地址一般存放在order文件，LoadBigScreenOrderFile请求order文件后触发回调
        /// 参数是推流地址
        /// </summary>
        public event Action<string> OnEventBigScreenRecieveRTSP;
        /// <summary>
        /// 大屏幕显示视频第一帧事件
        /// </summary>
        public event Action<Texture2D> OnEventBigScreenVideoFrameReady;
        /// <summary>
        /// 大屏幕更新图片事件
        /// </summary>
        public event Action<Texture2D> OnEventBigScreenUpdateImage;
        /// <summary>
        /// 接收到网页消息事件
        /// 网页与工程互通消息，网页向工程发送数据回调
        /// 参数 网页发送的数据
        /// </summary>
        public event Action<string> OnEventReceiveWebviewMessage;
        /// <summary>
        /// 加载order文件接收到文件内容事件
        /// 参数 order文件 内容
        /// </summary>
        public event Action<string> OnEventGetOrderFileString;
        /// <summary>
        /// 加载TXT文件接收到文件内容事件
        /// 参数 TXT 内容
        /// </summary>
        public event Action<string> OnEventGetTxtFileString;
        /// <summary>
        /// 文件下载进度
        /// </summary>
        public event Action<VRProgressInfo> OnEventDownLoadFileProgress;
        /// <summary>
        /// 文件缓存成功后事件  返回缓存路径数据
        /// </summary>
        public event Action<LocalCacheFile> OnEventReceiveLocalCacheFile;
        /// <summary>
        /// 接收到PDF文件页数事件
        /// RequestPdfRenderPlayerPageCount 请求PDF文件页数回调  
        /// 参数GameObject为PDF控制器
        /// </summary>
        public event Action<GameObject, int> OnEventReceivePdfPageCount;
        /// <summary>
        /// 接收到PDF的页面贴图
        /// VsEngine.Instance.ShowPdfRenderPlayerPage 显示PDF页回调
        /// 参数GameObject为PDF控制器
        /// </summary>
        public event Action<GameObject, Texture2D> OnEventReceivePdfRenderTexture;
        /// <summary>
        /// 接收到视频渲染贴图
        /// RequestVideoTexture 请求视频画面 触发回调  
        /// 参数GameObject为PDF控制器
        /// </summary>
        public event Action<GameObject, Texture> OnEventReceiveVideoRenderTexture;
        /// <summary>
        /// 接收到视频准备完成第一帧贴图  
        /// 参数GameObject为视频控制器
        /// </summary>
        public event Action<GameObject, Texture> OnEventReceiveVideoFirstFrameReady;
        /// <summary>
        /// 接收视频当前播放时长
        /// RequestVideoCurrentTime 触发回调  
        /// 参数GameObject为视频控制器
        /// </summary>
        public event Action<GameObject, double> OnEventReceiveVideoCurrentTime;
        /// <summary>
        /// 接收视频信息
        /// RequestVideoInfo 触发回调  
        /// 参数GameObject为视频控制器
        /// </summary>
        public event Action<GameObject, CustomVideoPlayer> OnEventReceiveVideoInfo;
        /// <summary>
        /// 接收视频播放完成回调  
        /// 参数GameObject为视频控制器
        /// </summary>
        public event Action<GameObject, string> OnEventReceiveVideoFinish;
        /// <summary>
        /// GIF录制结束 保存到本地事件
        /// 参数 本地文件地址
        /// </summary>
        public event Action<string> OnEventReceiveGifLocalPath;
        /// <summary>
        /// GIF录制进度事件
        /// </summary>
        public event Action<float> OnEventReceiveGifProgress;
        /// <summary>
        /// 房间连接成功回调  
        /// 参数string 服务器地址
        /// </summary>
        public event Action<string> OnEventRoomConnected;
        /// <summary>
        /// 房间连接报错回调  
        /// 参数string 服务器地址
        /// </summary>
        public event Action<string> OnEventRoomConnectError;
        /// <summary>
        /// 房间断开链接回调  
        /// 参数string 服务器地址
        /// </summary>
        public event Action<string> OnEventRoomDisConnected;
        /// <summary>
        /// 房间关闭事件回调  
        /// 参数string 服务器地址
        /// </summary>
        public event Action<string> OnEventRoomConnectClose;
        /// <summary>
        /// 自己离开房间回调
        /// 通过系统主菜单退出房间事件
        /// </summary>
        public event Action OnEventSelfLeaveRoom;
        /// <summary>
        /// 房间连接到新的频道
        /// </summary>
        public event Action<VRRootChanelRoom> OnEventRoomConnectNewChannel;
        /// <summary>
        /// 语音房间初始化回调
        /// 参数 VRVoiceInitConfig
        /// </summary>
        public event Action<VRVoiceInitConfig> OnEventVoiceRoomInit;
        /// <summary>
        /// 语音房间连接成功回调
        /// 参数 string 语音房间ID
        /// </summary>
        public event Action<string> OnEventVoiceRoomConnected;
        /// <summary>
        /// 语音房间断开连接回调
        /// 参数 string 语音房间ID
        /// </summary>
        public event Action<string> OnEventVoiceRoomDisconnected;
        /// <summary>
        /// 退出语音房间回调
        /// </summary>
        public event Action OnEventVoiceRoomExitEvent;
        /// <summary>
        /// 麦克风音量改变回调
        /// 参数 int 音量改变量
        /// </summary>
        public event Action<int> OnEventVoiceMicrophoneVolumeChange;
        /// <summary>
        /// 改变扬声器音量回调  听到场景内其他用户声音大小
        /// 参数 int 音量改变量
        /// </summary>
        public event Action<int> OnEventVoiceLoudSpeakerVolumeChange;
        /// <summary>
        /// 开关麦事件 
        /// 数据 TRUE  关麦 FALSE 开麦
        /// </summary>
        public event Action<bool> OnEventVoiceMicrophoneMult;
        /// <summary>
        /// 场景内用户开关麦事件
        /// 数据 string 用户ID  bool 开关麦
        /// </summary>
        public event Action<string, bool> OnEventMicrophoneStatusChange;
        /// <summary>
        /// 点击场景物体事件
        /// 参数 GameObject 3D空间物体
        /// </summary>
        public event Action<GameObject> OnEventPointClickHandler;
        /// <summary>
        /// 射线进入事件
        /// 参数 GameObject 3D空间物体
        /// </summary>
        public event Action<GameObject> OnEventPointEnterHandler;
        /// <summary>
        /// 射线离开事件
        /// 参数 GameObject 3D空间物体
        /// </summary>
        public event Action<GameObject> OnEventPointExitHandler;
        /// <summary>
        /// 自身位置改变事件
        /// </summary>
        public event Action<string> OnEventSelfPlaceTo;
        /// <summary>
        /// 自身脚踩到地面区域切换事件
        /// 参数 string 地面物体名称
        /// </summary>
        public event Action<string> OnEventSelfStepOnMesh;
        /// <summary>
        /// 人物集合到位置组事件
        /// 参数 string 位置组名称  bool 是否是所有人
        /// </summary>
        public event Action<string, bool> OnEventAllPlaceToGroup;
        /// <summary>
        /// 人物传送到某个位置事件
        /// </summary>
        public event Action<WsTeleportInfo> OnEventAvatarTeleportTo;
        /// <summary>
        /// 当前是否可以传送事件
        /// </summary>
        public event Action<bool> OnEventTeleportStatusChanged;
        /// <summary>
        /// VR 射线触碰放置点选择事件
        /// </summary>
        public event Action<PlayceEnterMessage> OnEventVrPlacePointEnter;
        /// <summary>
        /// VR 射线离开放置点选择事件
        /// </summary>
        public event Action<PlayceEnterMessage> OnEventVrPlacePointExit;
        /// <summary>
        /// 人物放置到某个位置事件
        /// </summary>
        public event Action<PlacePortObj> OnEventPlacePortTo;
        /// <summary>
        /// 物体移动
        /// 参数 WsMovingObj 被移动的物体参数
        /// </summary>
        public event Action<WsMovingObj> OnEventMovingObject;
        /// <summary>
        /// 加载glb模型结束事件
        /// 参数 加载的模型
        /// </summary>
        public event Action<GlbSceneObjectFile> OnEventGlbModelLoadDone;
        /// <summary>
        /// 用户共享屏幕回调
        /// 有用户分享或关闭分享 屏幕、摄像头触发回调
        /// 参数 UserScreenShareReqData  分享用户ID 开启关闭分享
        /// </summary>
        public event Action<UserScreenShareReqExData> OnEventUserShareScreen;
        /// <summary>
        /// 用户共享屏幕接收到第一帧画面事件
        /// 参数 shareuserid
        /// </summary>
        public event Action<string> OnEventShareScreenFrameReady;
        /// <summary>
        /// 共享图片尺寸改变
        /// 参数 分享面板名称、分享画面宽、高
        /// </summary>
        public event Action<string, int, int> OnEventScreenShareTextureSizeChange;
        /// <summary>
        /// 接收到MV信息
        /// RequestMusicVedioInfo 请求MV信息回调
        /// 参数 视频信息 json格式
        /// </summary>
        public event Action<string> OnEventReceiveMusicVedioInfo;
        /// <summary>
        /// 流式接口接收到单个数据事件
        /// 参数 接收到的数据
        /// </summary>
        public event Action<string> OnEventReceiveStreamWebData;
        /// <summary>
        /// 流式接口传输完成事件
        /// 参数 接收到的数据
        /// </summary>
        public event Action<string> OnEventReceiveStreamWebResult;
        /// <summary>
        /// 手柄扳机键触发
        /// </summary>
        public event Action OnEventVrTriggerClick;
        /// <summary>
        /// 左手柄的扳机键抬起
        /// </summary>
        public event Action OnEventVrLeftTriggerUp;
        /// <summary>
        /// 左手柄的扳机键按下
        /// </summary>
        public event Action OnEventVrLeftTriggerDown;
        /// <summary>
        /// 右手柄的扳机键抬起
        /// </summary>
        public event Action OnEventVrRightTriggerUp;
        /// <summary>
        /// 右手柄的扳机键按下
        /// </summary>
        public event Action OnEventVrRightTriggerDown;
        /// <summary>
        /// 手柄菜单键触发
        /// </summary>
        public event Action OnEventVrStartButtonClick;
        /// <summary>
        /// 手柄X键抬起
        /// </summary>
        public event Action OnEventVrXButtonUp;
        /// <summary>
        /// 手柄X键按下
        /// </summary>
        public event Action OnEventVrXButtonDown;
        /// <summary>
        /// 手柄Y键抬起
        /// </summary>
        public event Action OnEventVrYButtonUp;
        /// <summary>
        /// 手柄Y键抬起
        /// </summary>
        public event Action OnEventVrYButtonDown;
        /// <summary>
        /// 手柄A键抬起
        /// </summary>
        public event Action OnEventVrAButtonUp;
        /// <summary>
        /// 手柄A键按下
        /// </summary>
        public event Action OnEventVrAButtonDown;
        /// <summary>
        /// 手柄B键抬起
        /// </summary>
        public event Action OnEventVrBButtonUp;
        /// <summary>
        /// 手柄B键按下
        /// </summary>
        public event Action OnEventVrBButtonDown;
        /// <summary>
        /// 右手柄摇杆键触发
        /// </summary>
        public event Action OnEventVrRightStickClick;
        /// <summary>
        /// 左手柄摇杆键触发
        /// </summary>
        public event Action OnEventVrLeftStickClick;
        /// <summary>
        /// 左手柄抓握键抬起
        /// </summary>
        public event Action OnEventVrLeftGrabUp;
        /// <summary>
        /// 左手柄抓握键按下
        /// </summary>
        public event Action OnEventVrLeftGrabDown;
        /// <summary>
        /// 右手柄抓握键抬起
        /// </summary>
        public event Action OnEventVrRightGrabUp;
        /// <summary>
        /// 右手柄抓握键按下
        /// </summary>
        public event Action OnEventVrRightGrabDown;
        /// <summary>
        /// 左手柄摇杆方向键左
        /// </summary>
        public event Action OnEventVrLeftStickLeft;
        /// <summary>
        /// 左手柄摇杆方向键右
        /// </summary>
        public event Action OnEventVrLeftStickRight;
        /// <summary>
        /// 左手柄摇杆方向键上
        /// </summary>
        public event Action OnEventVrLeftStickUp;
        /// <summary>
        /// 左手柄摇杆方向键下
        /// </summary>
        public event Action OnEventVrLeftStickDown;
        /// <summary>
        /// 左手柄摇杆方向键还原
        /// </summary>
        public event Action OnEventVrLeftStickRelease;
        /// <summary>
        /// 左手柄摇杆坐标轴数据（Vector2）
        /// </summary>
        public event Action<Vector2> OnEventVrLeftStickAxis;
        /// <summary>
        /// 右手柄摇杆方向键左
        /// </summary>
        public event Action OnEventVrRightStickLeft;
        /// <summary>
        /// 右手柄摇杆方向键右
        /// </summary>
        public event Action OnEventVrRightStickRight;
        /// <summary>
        /// 右手柄摇杆方向键上
        /// </summary>
        public event Action OnEventVrRightStickUp;
        /// <summary>
        /// 右手柄摇杆方向键下
        /// </summary>
        public event Action OnEventVrRightStickDown;
        /// <summary>
        /// 右手柄摇杆方向键还原
        /// </summary>
        public event Action OnEventVrRightStickRelease;
        /// <summary>
        /// 右手柄摇杆坐标轴数据（Vector2）
        /// </summary>
        public event Action<Vector2> OnEventVrRightStickAxis;
        /// <summary>
        /// 左手柄扳机键坐标轴数据（float）
        /// </summary>
        public event Action<Vector2> OnEventVrLeftTriggerAxis;
        /// <summary>
        /// 右手柄扳机键坐标轴数据（float）
        /// </summary>
        public event Action<Vector2> OnEventVrRightTriggerAxis;
        /// <summary>
        /// 左手柄抓握键坐标轴数据（float）
        /// </summary>
        public event Action<Vector2> OnEventVrLeftGrabAxis;
        /// <summary>
        /// 右手柄抓握键坐标轴数据（float）
        /// </summary>
        public event Action<Vector2> OnEventVrRightGrabAxis;
        /// <summary>
        /// 带上头显（某些vr无效）
        /// </summary>
        public event Action OnEventVrDevicePutOn;
        /// <summary>
        /// 脱下头显（某些vr无效）
        /// </summary>
        public event Action OnEventVrDeviceTakeOff;
        /// <summary>
        /// 激光笔状态改变事件  打开关闭
        /// </summary>
        public event Action<bool> OnEventVrLaserStatusChanged;
        /// <summary>
        /// VR键盘输入事件
        /// 参数 键盘输入框内容
        /// </summary>
        public event Action<string> OnEventVrKeyboardChanged;
        /// <summary>
        /// 在视频录制结束时 获取录制视频的地址
        /// 参数 保存地址
        /// </summary>
        public event Action<string> OnEventGetCapturePath;
        /// <summary>
        /// 在音频录制结束时，获取录制的音频地址
        /// 参数 保存地址
        /// </summary>
        public event Action<string> OnEventGetCollectAudioPath;
        /// <summary>
        /// 结束录制推流消息
        /// </summary>
        public event Action OnEventStopCaptureRtmpSucceed;

        /// <summary>
        /// 是否显示屏幕投屏的Canvas遮罩1
        /// 参数 是否显示Canvas遮罩1
        /// </summary>
        public event Action<bool> OnEventShowCanvasMask_1OnGameScreen;
        /// <summary>
        /// 是否显示屏幕投屏的Canvas遮罩1
        /// 参数 是否显示Canvas遮罩2
        /// </summary>
        public event Action<bool> OnEventShowCanvasMask_2OnGameScreen;





        /// <summary>
        /// 系统设置面板显示隐藏事件
        /// </summary>
        public event Action<bool> OnEventSystemSettingPanelHide;
        /// <summary>
        /// 监听关闭系统投屏和摄像机或投屏失败的事件
        /// </summary>
        public event Action OnEventStopSelfShareByOutSide;
        /// <summary>
        /// 是否显示网页页面
        /// </summary>
        public event Action<bool> OnEventNoticeWebViewShow;
        /// <summary>
        /// 根据网址来关闭网页
        /// </summary>
        public event Action<bool> OnEventNoticeWebViewClosed;

        /// <summary>
        /// 关闭所有网页
        /// </summary>
        public event Action<bool> OnEventCloseWebviewByHand;


        /// <summary>
        /// 获取接收角色Root节点的事件。
        /// </summary>
        public event Action<string, GameObject> OnEventRecieveAvatarDriverOBJ;

        /// <summary>
        /// 切换摄像机导播模式事件
        /// </summary>
        public event Action<bool> OnEventCameraDirectorMode;

        /// <summary>
        /// 声网分享失败回调
        /// </summary>
        public event Action<int> OnEventAgoraShareFaild;

        /// <summary>
        /// 接收网页沟通消息
        /// </summary>
        public event Action<string> OnEventReceiveWebviewMessageBase64;

        /// <summary>
        /// 获取加载的AB包资源的OBJ
        /// </summary>
        public event Action<string, object> OnEventReceiverLoadAssetBundleObj;

        /// <summary>
        /// String为缓存的sign bool  为是否存在
        /// </summary>
        public event Action<string, bool> OnEventGetCheckFileAlreadyCached;

        /// <summary>
        /// 获取人物朝向
        /// </summary>
        public event Action<Vector3> OnEventReciveAvatarDirect;

        /// <summary>
        /// 获取加载的高斯泼溅的模型
        /// </summary>
        public event Action<GaussianModelData> OnEventReciveLoadGaussionModel;

        /// <summary>
        /// 获取云渲染信息
        /// </summary>
        public event Action<int, string> OnEventHandleCloudRenderMsgInfo;

        /// <summary>
        /// 获取点击地面的目标点信息
        /// </summary>
        public event Action<Vector3> OnEventReciveClickWalkTargetPoint;

        /// <summary>
        /// 接收切换摄像机的类型
        /// </summary>
        public event Action<int> OnEventReciveSwitchCameraType;

        /// <summary>
        /// 返回翻译资源 注意使用该事件需要再退出场景是将注册方法移除
        /// </summary>
        public event Action<string> OnEventTextTranslateResult;

        /// <summary>
        /// 返回语音转文字的实时结果
        /// </summary>
        public event Action<string, int> OnEventTencentVoiceIdentifyResult;

        /// <summary>
        /// 返回文字转语音的结果
        /// </summary>
        public event Action OnEventTencentTextToVoiceEnd;

        /// <summary>
        /// 返回一句话语音转文字的结果
        /// </summary>
        public event Action<string> OnEventLongVoiceIdentifyResult;
        /// <summary>
        /// 返回加载模型进度
        /// </summary>
        public event Action<VRProgressInfo> OnEventLoadModelProgress;
        /// <summary>
        /// 网页点击输入框
        /// </summary>
        public event Action<bool> OnEventWebClickInputField;

        /// <summary>
        /// RTC中人员加入频道
        /// </summary>
        public event Action<string> OnEventWebRtcAvatarJoinScene;
        /// <summary>
        /// 会议模式下切换完音频输入回调
        /// </summary>
        public event Action OnEventWebRtcHideAudioChangeResult;
        /// <summary>
        /// 场景加载结束回调
        /// </summary>
        public event Action OnEventLoadSceneFinish;
        /// <summary>
        /// Sdk内部消息调用
        /// </summary>
        public event Action<string, List<object>> OnEventSDKExpandEvent;
        #endregion

        #region API
#pragma warning disable CS0626
        /// <summary>
        /// 扩展事件（用于热更新的接口）
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventSystemExpandEvent">OnEventSystemExpandEvent</see>
        /// </summary>
        /// <param name="eventname">事件接口名</param>
        /// <param name="eventparam">接口参数</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SendSystemExpandEvent(string eventname, List<object> eventparam, float delay = 0);
        /// <summary>
        /// 扩展事件（用于热更新的接口）
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventSystemExpandEvent">OnEventSystemExpandEvent</see>
        /// </summary>
        /// <param name="eventname">事件接口名</param>
        /// <param name="eventparam">接口参数</param>
        /// <param name="sender">接口参数</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SendSystemExpandEvent(string eventname, List<object> eventparam, object sender, float delay = 0);
        /// <summary>
        /// 扩展事件（用于SDK内部调用消息）
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventSDKExpandEvent">OnEventSystemExpandEvent</see>
        /// </summary>
        /// <param name="eventname">事件接口名</param>
        /// <param name="eventparam">接口参数</param>
        /// <param name="sender">接口参数</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SendSDKExpandEvent(string eventname, List<object> eventparam, object sender, float delay = 0);
        /// <summary>
        /// 获取应用内热更扩展的数据
        /// </summary>
        /// <param name="key">数据请求的key
        /// 有参数的传JSON格式例{"key" : key,"userID":"3000180124"}
        /// 获取指定人物手柄位置{"key" : "GetAvatarHands","userid":"3000180124"}  返回值为 Transform[]  0为左手 1为右手
        /// 获取根据人物ID获取人物所处平台 {"key" : "GetPlatformBasedOnAvatarID","userid":"3000180124"}  返回值 string 安卓"a" 苹果"i" Windows"w" Mac"m" VR"v"云端"c"
        /// 无参数的传字符串 
        /// 获取当前声网频道用户  AgoraVoiceUser  值为list<string> or Null
        /// 获取人物当前位置  GetAvatarIpose 获取人物的当前姿态  返回值为Int    Idle = 1 << 0, Walk = 1 << 1, Run = 1 << 2,  Jump = 1 << 3, Sit = 1 << 4,  Stand = 1 << 5,         //站起  WalkLeft = 1 << 6,      //左走WalkRight = 1 << 7,WalkBack = 1 << 8,      //后退RunBack = 1 << 9,TurnLeft = 1 << 10,     //左转TurnRight = 1 << 11,RunLeft = 1 << 12,       RunRight = 1 << 13,CustomAction = 1 << 14,   //自定义动作Dancing = 1 << 15,        //跳舞FlipAni = 1 << 16,        //空间翻转TurnPause = 1 << 17       //旋转角度过小 停在旋转中
        /// 获取本机器上可用摄像头的信息  GetCameraInfo   返回值时JSON字符串
        /// 是否使用空间笔 IsUseSpacePen 返回值时Bool
        /// 获取空间笔的分享状态 getRtcSharingState  返回值为 bool 
        /// 获取主工程的Canvas SystemUICanvas  返回值时Canvas
        /// 获取当前频道的组织ID列表 GetChannelGroups  返回值为string[]
        /// 获取缓存文件的本地保存根目录 GetCacheFileLocalPathRoot  返回值为String
        /// 获取人物的音量变化列表 AvatarMicStatusDic  返回值为 Dictionary<string, int> string时人物ID，int 是音量
        /// 获取会议模式的音频输入选择页面是否输入 GetRtcSelectAudioPanel 返回值为Bool 
        /// 获取网页登录传入的信息 GetWebLoginUserData 返回值为String 
        /// </param>
        /// <returns>请求的数据</returns>
        public extern object GetSystemData(string key);
        /// <summary>
        /// 保存数据到当前场景服务端 以字典的格式存储
        /// 与RequestRoomSavedData配合使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="bclear">是否清除该条数据</param>
        /// <param name="ball">是否保存到所有场景</param>
        /// <param name="bforver">是否用永远保存，如果设置为false,当场景内用户都离开时，该数据自动清除</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SaveDataToRoom(string key, string value, bool bclear = false, bool ball = false, bool bforver = false, float delay = 0);
        /// <summary>
        /// 请求当前场景服务端保持的数据
        /// 该方法相关的回调事件为： <see cref="VSWorkSDK.VSEngine.OnEventReceiveRoomSavedData">OnEventReceiveRoomSavedData</see>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="receiveCallback">没有数据 返回的字典key为 @"key" + "_null"</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void RequestRoomSavedData(string key, Action<Dictionary<string, string>> receiveCallback, float delay = 0);
        /// <summary>
        /// 同步网络数据 用于同步自定义数据
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventReceiveRoomSyncData">OnEventReceiveRoomSyncData</see>
        /// </summary>
        /// <param name="data">RoomSycnData类型数据 一般 a 作为消息的key</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SendRoomSyncData(RoomSycnData data, float delay = 0);
        /// <summary>
        /// 切换频道
        /// </summary>
        /// <param name="channelid">频道id</param>
        /// <param name="voiceid">语音id</param>
        /// <param name="token">令牌</param>
        /// <param name="samescene"></param>
        /// <param name="loadingscene"></param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void ConnectToNewChannel(string channelid, string voiceid, string token = "", bool samescene = false, string loadingscene = "", float delay = 0);

        /// <summary>
        /// 设置WebRtc邀请按钮是否可交互
        /// </summary>
        /// <param name="isEnable"></param>
        public extern void SetWebRtcInviteEnable(bool isEnable, float delay = 0);

        /// <summary>
        /// 请求加载高斯泼溅文件
        /// </summary>
        /// <param name="url"> 文件地址</param>
        /// <param name="md5">文件的MD5码</param>
        /// <param name="blocal"> 是否是本地的</param>
        /// <param name="root"> 父节点</param>
        /// <param name="extralparam">预留额外参数</param>
        /// <param name="delay"></param>
        public extern void LoadGaussianModel(string url, string md5, bool blocal, GameObject root, string extralparam, float delay = 0);
        /// <summary>
        /// 设置底部菜单面板的显示和隐藏
        /// </summary>
        /// <param name="isEnable"></param>
        /// <param name="delay"></param>
        public extern void SetMenuPanelEnable(bool isEnable, float delay = 0);

        /// <summary>
        /// 设置声场静音
        /// </summary>
        /// <param name="bMute">开启或者关闭</param>
        /// <param name="muteRemote">静音远端</param>
        /// <param name="muteSelf">静音自己</param>
        /// <param name="delay"></param>
        public extern void SetSoundMute(bool bMute, bool muteRemote, bool muteSelf, float delay = 0);
        /// <summary>
        /// 开始进行文本翻译
        /// </summary>
        /// <param name="data"></param>
        public extern void StartTextTranslate(string mAvatarID, string words, string Stable, string timestamp, float delay = 0);
        /// <summary>
        /// 开启系统侧语音转文字
        /// </summary>
        /// <param name="str"></param>
        /// <param name="delay"></param>
        public extern void StartSystemVoiceIdentify(float delay = 0);
        /// <summary>
        /// 结束系统侧的语音转文字
        /// </summary>
        /// <param name="delay"></param>
        public extern void EndSystemVoiceIdenetify(float delay = 0);
        /// <summary>
        /// 开始系统侧的文字转语音
        /// </summary>
        /// <param name="str"></param>
        /// <param name="delay"></param>
        public extern void StartSystemTextToVoice(string words, int voiceType, int voiceSpeed, float delay = 0);
        /// <summary>
        /// 结束系统侧的文字转语音
        /// </summary>
        /// <param name="delay"></param>
        public extern void EndSystemTextToVoice(float delay = 0);

        /// <summary>
        /// 结束系统侧长语音识别
        /// </summary>
        /// <param name="delay"></param>
        public extern void EndSystemLongVoiceIdentify(float delay = 0);


        /// <summary>
        /// 开启系统侧长语音识别
        /// </summary>
        /// <param name="delay"></param>
        public extern void StartSystemLongVoiceIdentify(float delay = 0);
        /// <summary>
        /// 设置翻译的信息
        /// </summary>
        /// <param name="mAvatarID">人物ID</param>
        /// <param name="words">翻译文字</param>
        /// <param name="Stable">当前文本是否是稳定态</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="delay"></param>
        /// <summary>
        /// 屏幕顶部跑马灯提示
        /// </summary>
        /// <param name="Info">提示文字</param>
        /// <param name="infocolor">显示颜色</param>
        /// <param name="showtime">停留时长</param>
        /// <param name="sync">别人同步显示</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void ShowRoomMarqueeLog(string Info, InfoColor infocolor, float showtime, bool sync, float delay = 0);
        /// <summary>
        /// 切换视角
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventSwitchCameraViewMode">OnEventSwitchCameraViewMode</see>
        /// </summary>
        /// <param name="mode">要切换的视角模式</param>
        /// <param name="switchAnimation">是否显示过渡的动画</param>
        public extern void SwitchCameraViewMode(CameraViewMode mode, bool switchAnimation = false, float delay = 0);
        /// <summary>
        /// 限制自身avatar移动，无法通过摇杆和wasd移动人物
        /// </summary>
        /// <param name="blimit">是否开启限制移动</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LimitSelfMove(bool blimit, float delay = 0);
        /// <summary>
        /// 限制自身avatar旋转 无法通过摇杆旋转方向
        /// </summary>
        /// <param name="blimit">是否开启限制旋转</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LimitSelfRotate(bool blimit, float delay = 0);
        /// <summary>
        /// 人物脱离系统控制 系统不再控制人物走、跑、跳、转向等
        /// 一般用于SDK自己控制人物，比如瞬移
        /// </summary>
        /// <param name="bundercontrol">是否启用脱离系统控制</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void MakeSelfActionUnderControl(bool bundercontrol, float delay = 0);
        /// <summary>
        /// 改变自身avatar当前的动画状态（该方法一般与MakeSelfActionUnderControl配合使用）
        /// </summary>
        /// <param name="state">动画状态值 如1代表Idle动画 2代表走 4代表跑</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void ChangeSelfActionState(ActorActionType state, float delay = 0);
        /// <summary>
        /// 锁定自身走跑状态 
        /// 比如锁定 LOCKWALK，那人物只能走路无法切换跑
        /// 锁定 LOCKRUN，那人物只能跑
        /// </summary>
        /// <param name="lockmode">锁定走跑状态</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LockSelfWalkMode(WalkLockMode lockmode, float delay = 0);
        /// <summary>
        /// 立刻同步自身avatar数据给其他人
        /// </summary>
        public extern void SyncSelfAvatarDataImmediately();
        /// <summary>
        /// 立刻刷新本地自身avatar数据
        /// 一般提高自身刷新帧率
        /// </summary>
        public extern void RefreshSelfAvatarDataImmediately();
        /// <summary>
        /// 设置同步帧率
        /// 如果某些情况下人物抖动严重或需要高帧率可以调用此方法
        /// </summary>
        /// <param name="rate"> value range [1,50] 值越大帧率越低</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetAvatarSyncFrameRate(float rate, float delay = 0);
        /// <summary>
        /// 还原成系统的同步帧率
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void ReSetAvatarSyncFrameRate(float delay = 0);

        /// <summary>
        /// 设置自己入座
        /// </summary>
        /// <param name="chair">入座的椅子物体</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetSelfSitOnChair(GameObject chair, float delay = 0);
        /// <summary>
        /// 设置自己从椅子上起身
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetSelfStandUpFromChair(float delay = 0);
        /// <summary>
        /// 播放avatar自定义动画
        /// 自定义动作绑定在avatar模型上 DefaultAnimation节点上的动画状态机 动画片段名称以"CustomAction_"命名的为自定义动作
        /// 比如 CustomAction_0  0为自定义动画序号
        /// </summary>
        /// <param name="customaction">自定义动画序号 需要和avatar模型制作打包人员确认需要播放的自定义动作序号</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void PlayAvatarCustomAction(int customaction, float delay = 0);
        /// <summary>
        /// 播放全身的自定义动画ID
        /// 
        /// </summary>
        /// <param name="fullCustomActionID"> ID需要根据主工程的绑定顺序来定</param>
        /// <param name="delay"></param>
        public extern void PlayAvatarSystemFullActionReq(int fullCustomActionID, float delay);
        /// <summary>
        /// 播放系统自带跳舞动画
        /// </summary>
        /// <param name="dancing">跳舞动画序号[0-2]</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void PlayAvatarDancingAction(int dancing, float delay = 0);
        /// <summary>
        /// 播放系统自带说话时动作动画
        /// </summary>
        /// <param name="speak">说话动画序号[0-5]</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void PlayAvatarSpeakAction(int speak, float delay = 0);

        /// <summary>
        /// 播放系统挂载的角色的全身动作
        /// </summary>
        /// <param name="ActionID">动作在主工程中的ID</param>
        /// <param name="delay"></param>
        public extern void PlayAvatarSystemFullAction(int ActionID, float delay = 0);

        /// <summary>
        /// 播放系统挂载的通用的半身动作
        /// </summary>
        /// <param name="ActionID">动作在主工程中的ID</param>
        /// <param name="delay"></param>
        public extern void PlayAvatarSystemHalfAction(int ActionID, float delay = 0);
        /// <summary>
        /// 开启自动寻路模式
        /// 注：1.主要通过消息体的内容区分是指定跟随还是自由导航模式，指定目标跟随需要传入followtarget的值，自由导航模式不需要传入followtarget的值，但是需要传入targetpos的值
        /// 2.场景要注意设置障碍物
        /// </summary>
        /// <param name="followTarget">目标体 可使用挂载CustomAvatarDriver的物体（只有指定目标时才使用，如果使用自由寻路模式 这个参数设置为空）</param>
        /// <param name="moveType">移动类型（暂时未使用）</param>
        /// <param name="offset">相对目标点的偏移值 如(1,1,0)</param>
        /// <param name="targetpos">目标位置(指定目标寻路时 这个参数值不使用)</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void StartAutoFindPathMode(GameObject followTarget, int moveType, Vector3 offset, Vector3 targetpos, float delay = 0);
        /// <summary>
        /// 结束自动寻路模式
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void StopAutoFindPathMode(float delay = 0);
        /// <summary>
        /// 设置自身人物模型跟随相机旋转方式
        /// </summary>
        /// <param name="followtype"></param>
        /// <param name="delay"></param>
        public extern void SetSelfAvatarFollowCamera(AvatarFollowCamera followtype, float delay = 0);
        /// <summary>
        /// 设置人物飞行模式
        /// </summary>
        /// <param name="bflight"></param>
        /// <param name="minheight"></param>
        /// <param name="maxheight"></param>
        /// <param name="delay"></param>
        public extern void SetAvatarFlightMode(bool bflight, float minheight, float maxheight, float delay = 0);
        /// <summary>
        /// 锁定触摸旋转相机或左键旋转相机
        /// </summary>
        /// <param name="block"></param>
        /// <param name="delay"></param>
        public extern void SetLockTouchRotateCamera(bool block, float delay = 0);
        /// <summary>
        /// 设置隐藏场景内除了admin外的所有avatar角色
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetHideAvatarsExeptAdmin(float delay = 0);
        /// <summary>
        ///  设置隐藏场景内的所有avatar角色
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetForceHideAllAvatars(bool bforcehide, float delay = 0);
        /// <summary>
        /// 设置隐藏场景内已选择的avatar角色
        /// </summary>
        /// <param name="selectavatar">选择的avatarID列表</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetForceHideSelectAvatars(List<string> selectavatar, bool bforcehide, float delay = 0);
        /// <summary>
        /// 设置隐藏指定avatar角色
        /// </summary>
        /// <param name="avatarid">要隐藏的avatarID</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetForceHideAvatar(string avatarid, bool bforcehide, float delay = 0);
        /// <summary>
        /// 设置自身avatar角色一直可见
        /// </summary>
        /// <param name="bvisiblealltime">true:一直可见状态 false:不是一直可见状态时  计时器开始切换到声网观众角色</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetSelfVisibleAllTime(bool bvisiblealltime, float delay = 0);
        /// <summary>
        /// 强制设置自身avatar角色隐藏
        /// </summary>
        /// <param name="bhide"></param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetForceHideSelfAvatarModel(bool bhide, float delay = 0);
        /// <summary>
        /// 加载Avatar fbx模型
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventLoadedFbxAvatarModel">OnEventLoadedFbxAvatarModel</see>
        /// </summary>
        /// <param name="avataraid">要加载的avatarID</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadFbxAvatarModel(string avataraid, float delay = 0);
        /// <summary>
        /// 清除资源中的avatar角色模型
        /// </summary>
        /// <param name="avataraid">要清除的avatarID</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void ClearFbxAvatarModelResource(string avataraid, float delay = 0);

        /// <summary>
        /// 强制同步自身数据到服务器
        /// </summary>
        /// <param name="delay"></param>
        public extern void SyncSelfFrameDataToServer(float delay = 0);
        /// <summary>
        /// 切换自己的avatar模型
        /// </summary>
        /// <param name="modelid">模型ID</param>
        /// <param name="delay">延迟调用</param>
        public extern void ChangeSelfAvatarModel(string modelid, float delay = 0);
        /// <summary>
        /// 模型换装
        /// 注:该方法的使用需要avatar角色模型先绑定好多套衣服，具体可与模型开发沟通
        /// </summary>
        /// <param name="bodyPart">需要换装的数据格式为：要换装的部分字符串如：Hair等，要换装的序号 如：1></param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void ChangeAvatarBodyPart(Dictionary<string, string> bodyPart, float delay = 0);
        /// <summary>
        /// 获取avatar模型节点
        /// </summary>
        /// <param name="avatarid">avatarID</param>
        /// <returns></returns>
        public extern Transform GetAvatarModel(string avatarid);
        /// <summary>
        /// 获取avatar当前帧数据
        /// </summary>
        /// <param name="avatarid">avatarID</param>
        /// <returns></returns>
        public extern WsAvatarFrameJian GetAvatarCurrentFrameData(string avatarid);
        /// <summary>
        /// 设置user为管理员
        /// </summary>
        /// <param name="userid">avatarid</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetUserAdmin(string userid, float delay = 0);
        /// <summary>
        /// 显示大屏幕
        /// </summary>
        /// <param name="bshow">是否显示大屏幕</param>
        public extern void ShowBigScreen(bool bshow, float delay = 0);
        /// <summary>
        /// 设置大屏幕的属性
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="rot">旋转</param>
        /// <param name="scale">缩放</param>
        /// <param name="curvangle">大屏幕曲率</param>
        /// <param name="benable">大屏幕是否打开</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetBigScreenProperty(Vector3 pos, Quaternion rot, Vector3 scale, int curvangle, bool benable, float delay = 0);
        /// <summary>
        /// 设置大屏幕尺寸
        /// </summary>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public extern void SetBigScreenSize(float width, float height);
        /// <summary>
        /// 设置大屏幕显示图片
        /// WsMediaFile 可以只传 URL
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventBigScreenShowImage">OnEventBigScreenShowImage</see>
        /// </summary>
        /// <param name="filedata">图片媒体文件数据</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetBigScreenShowImage(WsMediaFile filedata, float delay = 0);
        /// <summary>
        /// 设置大屏幕显示图片
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventBigScreenShowImage">OnEventBigScreenShowImage</see>
        /// </summary>
        /// <param name="tex">图片</param>
        /// <param name="sendevent">是否发送设置成功事件</param>
        public extern void SetBigScreenShowImage(Texture2D tex, bool sendevent = false, float delay = 0);
        /// <summary>
        /// 设置大屏幕准备视频
        /// 只有管理员才能显示视频控制面板 播放、暂停、快进等
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventBigScreenPrepareVideo">OnEventBigScreenPrepareVideo</see>
        /// </summary>
        /// <param name="videopath">视频地址</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetBigScreenPrepareVideo(string videopath, float delay = 0);
        /// <summary>
        /// 设置大屏幕显示RTSP推流
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventBigScreenRecieveRTSP">OnEventBigScreenRecieveRTSP</see>
        /// </summary>
        /// <param name="url">RTSP推流地址</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetBigScreenShowRTSP(string url, float delay = 0);
        /// <summary>
        /// 打开大屏幕网页
        /// PC、VR端是在应用内打开网页，显示在大屏上，也可切换到全屏显示 手机端使用手机浏览器打开原生网页，显示在应用之上
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <param name="badminonly">只有admin可用</param>
        /// <param name="bsync">同步</param>
        /// <param name="displayUI">是否显示在屏幕UI上</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="showtoolbar">是否显示工具栏</param>
        /// <param name="offset">半屏窗口偏移中心</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void OpenBigScreenWebView(WebViewParamterDataEx webViewParamterDataEx, float delay = 0);

        /// <summary>
        /// 打开网页显示
        /// </summary>
        /// <param name="url"></param>
        /// <param name="badminonly"></param>
        /// <param name="bsync"></param>
        /// <param name="displayUI"></param>
        /// <param name="bfullscreen"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="showtoolbar"></param>
        /// <param name="offset"></param>
        /// <param name="delay"></param>
        public extern void OpenBigScreenWebInnerView(WebViewParamterDataEx webViewParamterDataEx, float delay);
        /// <summary>
        /// 关闭网页
        /// OpenBigScreenWebView打开
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void CloseBigScreenWebView(string url, Transform WebViewRoot, float delay = 0);
        public extern void CloseBigScreenWebInner(string url, Transform WebViewRoot, float delay = 0);
        /// <summary>
        /// 显示大屏幕的Web页面
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="delay"></param>
        public extern void ShowBigScreenWeb(bool bShow, float delay = 0);

        /// <summary>
        /// 发送消息给网页
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventReceiveWebviewMessage">OnEventReceiveWebviewMessage</see>
        /// </summary>
        /// <param name="action">消息标签</param>
        /// <param name="content">消息内容</param>
        public extern void SendMsgToWebview(string action, string content, float delay = 0);
        /// <summary>
        /// 修改网页屏幕尺寸
        /// </summary>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="showtoolbar">是否显示工具栏</param>
        /// <param name="offset">半屏窗口偏移中心</param>
        public extern void ChangeWebviewSize(bool displayUI, bool bfullscreen, int width, int height, bool showtoolbar, Vector2 offset, float delay = 0);

        /// <summary>
        /// 下载文件并保存到本地
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventDownLoadFileProgress">OnEventDownLoadFileProgress</see>
        /// </summary>
        /// <param name="media">媒体文件信息</param>
        /// <param name="LoadDoneCallback">下载成功回调 参数 localpath 是返回结果 本地文件缓存地址 </param>
        /// <param name="progressCallback">下载进度回调 与<see cref="VSWorkSDK.VSEngine.OnEventDownLoadFileProgress">OnEventDownLoadFileProgress</see>实现效果一样</param>
        /// <param name="cancancle">是否可以中断下载</param>
        /// <param name="showprogress">是否显示下载进度</param>
        /// <param name="syncprogressevent">是否同步下载进度</param>
        /// <param name="sendprogressevent">是否发送下载进度</param>
        /// <param name="timeout"></param>
        public extern void DownloadAndCacheFile(WsMediaFile media, SDKWebResponseCallback LoadDoneCallback, Action<float> progressCallback = null,
                             bool cancancle = true, bool showprogress = false, bool syncprogressevent = false, bool sendprogressevent = false, int timeout = 0);
        /// <summary>
        /// 加载图片文件
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventDownLoadFileProgress">OnEventDownLoadFileProgress</see>
        /// </summary>
        /// <param name="media">图片媒体文件数据</param>
        /// <param name="LoadDoneCallback">下载成功回调 参数 img是返回结果</param>
        /// <param name="progressCallback">下载进度回调 与<see cref="VSWorkSDK.VSEngine.OnEventDownLoadFileProgress">OnEventDownLoadFileProgress</see>实现效果一样</param>
        public extern void LoadImageFile(WsMediaFile media, SDKWebResponseCallback LoadDoneCallback, Action<float> progressCallback = null);
        /// <summary>
        /// get接口请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="data">url地址附加参数</param>
        /// <param name="getDoneCallBack">成功回调 结果</param>
        /// <param name="progressCallBack">进度回调</param>
        /// <param name="heads">web请求头</param>
        /// <param name="timeout">超时时间</param>
        public extern void GetWebRequest(string url, Dictionary<string, object> data, Action<string> getDoneCallBack,
                                              Action<float> progressCallBack = null, Dictionary<string, string> heads = null, int timeout = 0);
        /// <summary>
        /// post接口请求
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="data">提交数据</param>
        /// <param name="postDoneCallBack">提交成功回调</param>
        /// <param name="progressCallBack">提交进度回调</param>
        /// <param name="header">请求头部</param>
        /// <param name="timeout">超时时间</param>
        public extern void PostWebRequest(string url, WWWForm data, Action<string> postDoneCallBack, Action<float> progressCallBack, Dictionary<string, string> header = null, int timeout = 0);

        /// <summary>
        /// 提交raw数据
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="rawdata">raw数据</param>
        /// <param name="postDoneCallBack">提交成功回调</param>
        /// <param name="progressCallBack">提交进度回调</param>
        /// <param name="header">请求头部</param>
        /// <param name="timeout">超时时间</param>
        public extern void PostWebRequestRawData(string url, string rawdata, Action<string> postDoneCallBack, Action<float> progressCallBack, Dictionary<string, string> header = null, int timeout = 0);
        /// <summary>
        /// 请求流式接口
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventReceiveStreamWebData">OnEventReceiveStreamWebData</see> <see cref="VSWorkSDK.VSEngine.OnEventReceiveStreamWebResult">OnEventReceiveStreamWebResult</see>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="type">请求类型</param>
        /// <param name="body">请求内容</param>
        public extern void StartStreamWebRequest(string url, WebRequestType type, string body, float delay = 0);
        /// <summary>
        /// 请求流式对话
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventReceiveStreamWebData">OnEventReceiveStreamWebData</see> <see cref="VSWorkSDK.VSEngine.OnEventReceiveStreamWebResult">OnEventReceiveStreamWebResult</see>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="content">请求内容</param>
        public extern void StartStreamChatWebRequest(string url, string content, float delay = 0);

        /// <summary>
        /// 加载并同步大屏幕媒体文件
        /// 只对主持人、助理主持人可用，并且在房间连接成功，房间支持媒体文件加载（IsRoomEnableMediaRes）
        /// </summary>
        /// <param name="mediafile">媒体文件数据 URL 是必须 其余参数非必须</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadBigScreenMediaFileAndSync(WsMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 加载大屏幕图片文件
        /// </summary>
        /// <param name="mediafile">媒体文件数据 URL 是必须 其余参数非必须</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadBigScreenImageFile(WsMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 加载场景文件
        /// </summary>
        /// <param name="mediafile">媒体文件数据 URL 是必须 其余参数非必须</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadSceneFile(WsMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 加载大屏幕视频文件
        /// </summary>
        /// <param name="mediafile">媒体文件数据 URL 是必须 其余参数非必须</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadBigScreenMovieFile(WsMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 加载大屏幕order文件  存储直播流地址
        /// </summary>
        /// <param name="mediafile">媒体文件数据 URL 是必须 其余参数非必须</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadBigScreenOrderFile(WsMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 加载GLB模型文件
        /// </summary>
        /// <param name="mediafile">媒体文件数据 URL 是必须 其余参数非必须</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadGlbFile(WsMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 加载GLB模型文件
        /// </summary>
        /// <param name="mediafile"></param>
        /// <param name="delay"></param>
        public extern void LoadGlbFile(WsGlbMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 加载txt文本文件 
        /// </summary>
        /// <param name="mediafile">媒体文件数据 URL 是必须 其余参数非必须</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadTxtFile(WsMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 加载大屏幕PDF文件
        /// </summary>
        /// <param name="mediafile">媒体文件数据 URL 是必须 其余参数非必须</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadBigScreenPDFFile(WsMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 加载大屏幕网页链接
        /// </summary>
        /// <param name="mediafile">媒体文件数据 URL 是必须 其余参数非必须</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadBigScreenWebViewLinkFile(WsMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 通过加载资源管理器文件切换频道
        /// 比如文件内容是 linkroom,a270d4b8-025c-4666-a7af-e5e7ee194657,vbqrpayi 
        /// a270d4b8-025c-4666-a7af-e5e7ee194657 频道ID
        /// vbqrpayi 语音ID
        /// </summary>
        /// <param name="mediafile">媒体文件数据 URL 是必须 其余参数非必须</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void LoadSwitchRoomChannelFile(WsMediaFile mediafile, float delay = 0);
        /// <summary>
        /// 创建PDF播放器
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventReceivePdfRenderTexture">OnEventReceivePdfRenderTexture</see>
        /// </summary>
        /// <param name="render">pdf控制器 作为回调判断依据</param>
        /// <param name="url">pdf文件本地地址，先把文件下载缓存到本地<see cref="VSWorkSDK.VSEngine.DownloadAndCacheFile">DownloadAndCacheFile</see></param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void CreatePdfRenderPlayer(GameObject render, string url, float delay = 0);
        /// <summary>
        /// 显示PDF指定页的内容
        /// 在CreatePdfRenderPlayer方法之后调用
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventReceivePdfRenderTexture">OnEventReceivePdfRenderTexture</see>
        /// </summary>
        /// <param name="render">pdf控制器</param>
        /// <param name="page">页数</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void ShowPdfRenderPlayerPage(GameObject render, int page, float delay = 0);
        /// <summary>
        /// 请求PDF文件总页数
        /// 在CreatePdfRenderPlayer方法之后调用
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventReceivePdfPageCount">OnEventReceivePdfPageCount</see>
        /// </summary>
        /// <param name="render">pdf播放器</param>
        /// <param name="receiveCallback">接收回调 与<see cref="VSWorkSDK.VSEngine.OnEventReceivePdfPageCount">OnEventReceivePdfPageCount</see>实现效果一样</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void RequestPdfRenderPlayerPageCount(GameObject render, Action<GameObject, int> receiveCallback, float delay = 0);
        /// <summary>
        /// 初始化视频播放器
        /// </summary>
        /// <param name="controller">视频控制器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="url">视频地址</param>
        /// <param name="renderobjs">渲染视频的物体</param>
        /// <param name="volume">音量</param>
        /// <param name="bloop">是否循环</param>
        /// <param name="autostart">是否自动播放</param>
        /// <param name="showdefaultimg">是否显示默认图片</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void InitVideoPlayer(GameObject controller, VideoPlayerKind videotype, string url, GameObject[] renderobjs,
                                            float volume = 100, bool bloop = false, bool autostart = false, bool showdefaultimg = false, float delay = 0);
        /// <summary>
        /// 播放视频
        /// InitVideoPlayer之后调用
        /// </summary>
        /// <param name="controler">视频播放器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void PlayVideo(GameObject controler, VideoPlayerKind videotype, float delay = 0);
        /// <summary>
        /// 暂停视频
        /// </summary>
        /// <param name="controler">视频播放器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void PauseVideo(GameObject controler, VideoPlayerKind videotype, float delay = 0);
        /// <summary>
        /// 停止视频
        /// </summary>
        /// <param name="controler">视频播放器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void StopVideo(GameObject controler, VideoPlayerKind videotype, float delay = 0);
        /// <summary>
        /// 跳转视频到指定时间点
        /// </summary>
        /// <param name="controler">视频播放器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="seekpos"></param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SeekVideoPos(GameObject controler, VideoPlayerKind videotype, float seekpos, float delay = 0);
        /// <summary>
        /// 设置视频音量
        /// </summary>
        /// <param name="controler">视频播放器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="vol">音量值</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVideoVolume(GameObject controler, VideoPlayerKind videotype, float vol, float delay = 0);
        /// <summary>
        /// 设置视频是否循环
        /// </summary>
        /// <param name="controler">视频播放器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="loop">是否循环</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVideoLoop(GameObject controler, VideoPlayerKind videotype, bool loop, float delay = 0);
        /// <summary>
        /// 设置视频是否静音
        /// </summary>
        /// <param name="controler">视频播放器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="mute">是否静音</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVideoMuteAudio(GameObject controler, VideoPlayerKind videotype, bool mute, float delay = 0);
        /// <summary>
        /// 获取视频贴图
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventReceiveVideoRenderTexture">OnEventReceiveVideoRenderTexture</see>
        /// </summary>
        /// <param name="controller">视频播放器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="receiveCallback">接收回调 与<see cref="VSWorkSDK.VSEngine.OnEventReceiveVideoRenderTexture">OnEventReceiveVideoRenderTexture</see>实现效果一样</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void RequestVideoTexture(GameObject controller, VideoPlayerKind videotype, Action<GameObject, Texture> receiveCallback, float delay = 0);
        /// <summary>
        /// 获取视频当前播放时长
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventReceiveVideoCurrentTime">OnEventReceiveVideoCurrentTime</see>
        /// </summary>
        /// <param name="controller">视频播放器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="receiveCallback">接收回调 与<see cref="VSWorkSDK.VSEngine.OnEventReceiveVideoCurrentTime">OnEventReceiveVideoCurrentTime</see>实现效果一样</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void RequestVideoCurrentTime(GameObject controller, VideoPlayerKind videotype, Action<GameObject, double> receiveCallback, float delay = 0);
        /// <summary>
        /// 请求视频信息
        /// </summary>
        /// <param name="controller">视频播放器</param>
        /// <param name="videotype">播放器类型</param>
        /// <param name="receiveCallback">接收回调 与<see cref="VSWorkSDK.VSEngine.OnEventReceiveVideoInfo">OnEventReceiveVideoInfo</see>实现效果一样</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void RequestVideoInfo(GameObject controller, VideoPlayerKind videotype, Action<GameObject, CustomVideoPlayer> receiveCallback, float delay = 0);
        /// <summary>
        /// 开始录制当前场景画面为GIF图片
        /// <see cref="VSWorkSDK.VSEngine.OnEventReceiveGifProgress">OnEventReceiveGifProgress</see>
        /// </summary>
        /// <param name="param"></param>
        /// <param name="delay"></param>
        public extern void StartRecordGif(GifRecordData param, float delay = 0);
        /// <summary>
        /// 停止录制GIF并保存到本地
        /// <see cref="VSWorkSDK.VSEngine.OnEventReceiveGifLocalPath">OnEventReceiveGifLocalPath</see>
        /// </summary>
        /// <param name="delay"></param>
        public extern void StopAndSaveRecordGif(float delay = 0);
        /// <summary>
        /// 打开麦克风（和别人语音交流）
        /// </summary>
        /// <param name="bopen">设置false关闭麦克风（无法和别人语音交流）</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVoiceMicrophoneOn(bool bopen, float delay = 0);
        /// <summary>
        /// 设置启用麦克风 （设置后可以打开麦克风、语音交流）
        /// </summary>
        /// <param name="benable">设置false无法打开麦克风、语音交流</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVoiceMicrophoneEnable(bool benable, float delay = 0);
        /// <summary>
        /// 设置扬声器开 设置后听到其他人语音
        /// </summary>
        /// <param name="bopen">设置false后无法听到其他人语音</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVoiceLoudSpeakerOn(bool bopen, float delay = 0);
        /// <summary>
        /// 连接房间语音区
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventVoiceRoomConnected">OnEventVoiceRoomConnected</see>
        /// </summary>
        /// <param name="exroom">房间语音ID</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVoiceConnectToExRoom(string exroom, float delay = 0);
        /// <summary>
        /// 离开语音房间
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventVoiceRoomExitEvent">OnEventVoiceRoomExitEvent</see>
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVoiceExitRoom(float delay = 0);
        /// <summary>
        /// 修改声场范围 （语音可听范围）
        /// </summary>
        /// <param name="range">语音范围大小</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVoiceRange(int range, float delay = 0);
        /// <summary>
        /// 设置扬声器音量+  场景内其他用户音量大小
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventVoiceLoudSpeakerVolumeChange">OnEventVoiceLoudSpeakerVolumeChange</see>
        /// </summary>
        /// <param name="addvolume">增加量</param>
        public extern void SetAddVoiceLoudSpeakerVolume(int addvolume);
        /// <summary>
        /// 设置扬声器音量- 场景内其他用户音量大小
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventVoiceLoudSpeakerVolumeChange">OnEventVoiceLoudSpeakerVolumeChange</see>
        /// </summary>
        /// <param name="minusvolume">减少量 正数</param>
        public extern void SetMinusVoiceLoudSpeakerVolume(int minusvolume);
        /// <summary>
        /// 设置扬声器音量最大
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventVoiceLoudSpeakerVolumeChange">OnEventVoiceLoudSpeakerVolumeChange</see>
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVoiceLoudSpeakerVolumeMax(float delay = 0);
        /// <summary>
        /// 设置自己麦克风音量+
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventVoiceMicrophoneVolumeChange">OnEventVoiceMicrophoneVolumeChange</see>
        /// </summary>
        /// <param name="addvolume">增加量</param>
        public extern void SetAddMicrophoneVolume(int addvolume);
        /// <summary>
        /// 设置自己麦克风音量-
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventVoiceMicrophoneVolumeChange">OnEventVoiceMicrophoneVolumeChange</see>
        /// </summary>
        /// <param name="minusvolume">减少量 正数</param>
        public extern void SetMinusMicrophoneVolume(int minusvolume);
        /// <summary>
        /// 设置自己麦克风音量最大
        /// 该方法相关的回调事件为：<see cref="VSWorkSDK.VSEngine.OnEventVoiceMicrophoneVolumeChange">OnEventVoiceMicrophoneVolumeChange</see>
        /// </summary>
        /// <param name="minusvolume">减少量</param>
        public extern void SetMicrophoneVolumeMax(float delay = 0);
        /// <summary>
        /// 麦克风是否打开
        /// </summary>
        /// <returns></returns>
        public extern bool IsVoiceMicrophoneOn();
        /// <summary>
        /// 麦克风是否禁用
        /// </summary>
        /// <returns></returns>
        public extern bool IsVoiceMicrophoneEnable();
        /// <summary>
        /// 是否屏蔽场景内其他用户声音
        /// </summary>
        /// <returns></returns>
        public extern bool IsVoiceLoudSpeakerOn();
        /// <summary>
        /// 设置是否开启语音静默模式，如果房间内就自己一人，系统检测无语音需求，语音会自动断开
        /// </summary>
        /// <param name="bforbiden">TRUE 禁用静默模式 语音不会自动断开</param>
        /// <param name="delay"></param>
        public extern void SetForbidenVoiceSilence(bool bforbiden, float delay = 0);
        /// <summary>
        /// 设置启用系统屏幕分享按钮
        /// </summary>
        /// <param name="benable"></param>
        /// <param name="delay"></param>
        public extern void EnableScreenShareButton(bool benable, float delay = 0);
        /// <summary>
        /// 设置启用系统摄像头分享按钮
        /// </summary>
        /// <param name="benable"></param>
        /// <param name="delay"></param>
        public extern void EnableCameraShareButton(bool benable, float delay = 0);
        /// <summary>
        /// 设置语音变声
        /// </summary>
        /// <param name="type"></param>
        /// <param name="delay"></param>
        public extern void SetVoiceConversion(Voice_Conversion_Type type, float delay = 0);
        /// <summary>
        /// 设置语音美化，一般用于歌唱情景
        /// </summary>
        /// <param name="type"></param>
        /// <param name="delay"></param>
        public extern void SetVoiceBeautifier(Voice_Beautifier_Type type, float delay = 0);
        /// <summary>
        /// 根据角色ID列表禁用或启用
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="AvatarIDlist"></param>
        /// <param name="delay"></param>
        public extern void SetVoiceMicrophoneEnableFromAvatarIDlist(bool bEnable, List<string> AvatarIDlist, float delay);
        /// <summary>
        /// 设置所有用户移动到指定group位置
        /// </summary>
        /// <param name="groupname">group名</param>
        /// <param name="ball">是否全部移动</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetAllAvatarToGroupPos(string groupname, bool ball, float delay = 0);

        /// <summary>
        /// 同步移动场景内物体
        /// </summary>
        /// <param name="objname">物体名称</param>
        /// <param name="pos">移动位置</param>
        /// <param name="rot">旋转</param>
        /// <param name="scale">缩放</param>
        /// <param name="blocal">是否本地</param>
        /// <param name="mark">物体移动类型</param>
        /// <param name="delay"></param>
        public extern void SendMovingObject(string objname, Vector3 pos, Quaternion rot, Vector3 scale, bool blocal, MovingObjectMarkType mark, float delay = 0);
        /// <summary>
        /// 标记物体是否可移动
        /// </summary>
        /// <param name="obj">物体</param>
        /// <param name="bcanmove">是否可移动</param>
        public extern void MarkObjectCanMove(GameObject obj, bool bcanmove);
        /// <summary>
        /// 开始共享屏幕
        /// </summary>
        /// <param name="showpanel">分享到哪个屏幕</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void StartShareScreen(string showpanel = "", float delay = 0);
        /// <summary>
        /// 结束分享屏幕
        /// 共享
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void StopShareScreen(float delay = 0);
        /// <summary>
        /// 共享第一个桌面屏幕
        /// </summary>
        /// <param name="showpanel">分享到哪个屏幕</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void ShareFirstScreen(string showpanel = "", float delay = 0);
        /// <summary>
        /// 是否打开共享屏幕的声音
        /// </summary>
        /// <param name="benable">是否打开</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void EnableScreenShareSound(bool benable, float delay = 0);
        /// <summary>
        /// 开始共享摄像头
        /// </summary>
        /// <param name="showpanel">分享到哪个屏幕</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void StartShareCamera(string showpanel = "", float delay = 0);
        /// <summary>
        /// 结束分享摄像头
        /// 共享
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void StopShareCamera(float delay = 0);
        /// <summary>
        /// 设置是否支持多用户分享
        /// 共享
        /// </summary>
        /// <param name="bmulti">是否支持多用户</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetRoomMultiScreenShare(bool bmulti, float delay = 0);
        /// <summary>
        /// 指定屏幕显示某人分享画面
        /// 共享
        /// </summary>
        /// <param name="sharedata">分享数据</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetUpScreenShareToViewPanel(SetupScreenShareView sharedata, float delay = 0);
        /// <summary>
        /// 预览投屏  分享
        /// </summary>
        /// <param name="shareuserid">共享屏幕用户ID</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void StartPreviewScreenShareView(string shareuserid, float delay = 0);

        /// <summary>
        /// 结束预览投屏 分享
        /// </summary>
        /// <param name="shareuserid">共享屏幕用户ID</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void StopPreviewScreenShareView(string shareuserid, float delay = 0);
        /// <summary>
        /// 关闭用户共享屏幕
        /// </summary>
        /// <param name="shareuserid">分享屏幕用户ID</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void CloseUserScreenShare(string shareuserid, float delay = 0);
        /// <summary>
        /// 请求媒体播放信息
        /// </summary>
        /// <param name="receiveCallback">请求回调 与<see cref="VSWorkSDK.VSEngine.OnEventReceiveMusicVedioInfo">OnEventReceiveMusicVedioInfo</see>实现效果一样</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void RequestMusicVedioInfo(Action<string> receiveCallback, float delay = 0);

        /// <summary>
        /// 设置当前MV的声道模式
        /// </summary>
        /// <param name="mode">声道模式</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetMusicVedioAudioDualMonoMode(AudioDualMonoModeType mode, float delay = 0);
        /// <summary>
        /// 播放MV
        /// </summary>
        /// <param name="url">媒体文件的路径。本地路径和在线路径都支持。在Android平台上，如果你需要以URI格式打开文件，请使用open</param>
        /// <param name="bloop">是否循环</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void PlayMusicVedio(string url, bool bloop, float delay = 0);
        /// <summary>
        /// 暂停播放MV
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void PauseMusicVedio(float delay = 0);
        /// <summary>
        /// 重新播放MV
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void ResumeMusicVedio(float delay = 0);
        /// <summary>
        /// 停止播放MV。
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void StopMusicVedio(float delay = 0);
        /// <summary>
        /// 调整MV本地播放音量
        /// </summary>
        /// <param name="volume">本地播放音量，范围从0到100</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void AdjustMusicVedioVolume(int volume, float delay = 0);
        /// <summary>
        /// 调整MV的远端音量
        /// 可以调用此方法来调整远程用户听到的MV的音量
        /// </summary>
        /// <param name="volume">音量，范围从0到400:0:静音100：（默认值）原始音量400：原始音量的四倍（将音频信号放大四倍）</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void AdjustRemoteMusicVedioVolume(int volume, float delay = 0);
        /// <summary>
        /// 设置MV跳转到新的播放位置。
        /// </summary>
        /// <param name="pos">新播放位置（ms）</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SeekMusicVedioPosition(long pos, float delay = 0);

        /// <summary>
        /// MV的声音模式
        /// </summary>
        /// <param name="type">0立体声 1左声道 2右声道 3混合声道 </param>
        /// <param name="delay"></param>
        public extern void SetMusicVedioAudioTrackMode(AudioDualMonoModeType type, float delay = 0);
        /// <summary>
        /// 停止MV的播放
        /// </summary>
        /// <param name="delay"></param>
        public extern void SetStopSelfMediaPlayerByOhter(float delay = 0);
        /// <summary>
        /// 重置VR传感器
        /// </summary>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVrResetSensor(float delay = 0);
        /// <summary>
        /// 设置VR手柄震动
        /// </summary>
        /// <param name="data">频率、时长等参数</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void SetVrHandVibration(Vibrationinfo data, float delay = 0);
        /// <summary>
        /// unity 输入框转到VR键盘输入显示
        /// </summary>
        /// <param name="field">输入框</param>
        /// <param name="delay"></param>
        public extern void BindInputToVrKeyboard(InputField field, float delay = 0);
        /// <summary>
        /// 显示帧率数据
        /// </summary>
        /// <param name="bshowfps">是否显示</param>
        public extern void ShowFps(bool bshowfps);

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void ClearCacheDataFile(CacheDataFileType type, float delay = 0);

        /// <summary>
        /// 清除场景数据
        /// </summary>
        /// <param name="type">场景数据类型</param>
        public extern void ClearSceneDataObject(SceneDataObjectType type);

        /// <summary>
        /// 清除缓存及GC
        /// </summary>
        public extern void CleanCachesAndGC();

        /// <summary>
        /// 设置VR激光笔可用
        /// </summary>
        /// <param name="benable"></param>
        public extern void SetVrLaserEnable(bool benable);

        /// <summary>
        /// 设置VR激光笔打开
        /// </summary>
        /// <param name="benable"></param>
        public extern void SetVrLaserOpen(bool benable);
        /// <summary>
        /// 设置VR点击地面移动功能可用
        /// </summary>
        /// <param name="enable"></param>
        public extern void SetVrPlacePortEnable(bool enable);

        /// <summary>
        /// 设置场景渲染质量
        /// </summary>
        /// <param name="level">渲染器质量级别</param>
        public extern void SetRenderQuality(RenderQualityLevel level);

        /// <summary>
        /// 设置同步数据帧速率
        /// </summary>
        /// <param name="framerate">同步数据帧率类型</param>
        public extern void SetSendFrameRate(SyncDataFrameRate framerate);
        /// <summary>
        /// 设置显示系统菜单
        /// </summary>
        /// <param name="benable">是否显示</param>
        public extern void SetSystemMenuEnable(bool benable);
        /// <summary>
        /// 显示内存使用情况
        /// </summary>
        /// <param name="bshow">是否显示</param>
        public extern void ShowMemoryUsed(bool bshow);
        /// <summary>
        /// 显示Avatar同步帧率数据
        /// </summary>
        /// <param name="bshow">是否显示</param>
        public extern void ShowAvatarSyncFrameRate(bool bshow);
        /// <summary>
        /// 选择的用户踢出房间
        /// </summary>
        public extern void KickOutSelectedUser();
        /// <summary>
        /// 结束所有下载操作
        /// </summary>
        public extern void CancelAllDownloadOperate();
        /// <summary>
        /// 自定义的命令
        /// </summary>
        /// <param name="Order"></param>
        public extern void CustomCommitOrder(string Order);

        /// <summary>
        /// 根据Url取消下载
        /// </summary>
        /// <param name="url"></param>
        public extern void CancelDownloadOperate(string url);
        /// <summary>
        /// 调整人物高度
        /// </summary>
        /// <param name="type"></param>
        public extern void SetAvatarHeightFix(AvatarHeightFixType type);

        /// <summary>
        /// 显示场景内所有Avatar名牌
        /// </summary>
        /// <param name="bshow">是否显示</param>
        public extern void ShowAvatarNamePanel(bool bshow);
        /// <summary>
        /// 设置画笔是否可用
        /// </summary>
        /// <param name="bactive"></param>
        public extern void SetLaserPenEnable(bool bactive);
        /// <summary>
        /// 开启画笔功能
        /// </summary>
        public extern void OpenLaserPenDraw();
        /// <summary>
        /// 开启空间画笔功能
        /// </summary>
        public extern void OpenSpacePenDraw();
        /// <summary>
        /// 关闭画笔功能
        /// </summary>
        public extern void CloseLaserPenDraw();
        /// <summary>
        /// 开启拍照功能
        /// </summary>
        /// <param name="benable">是否开启</param>
        public extern void SetTakePhotoEnable(bool benable);
        /// <summary>
        /// 开启导播模式
        /// </summary>
        /// <param name="bdirector"></param>
        public extern void SetCameraDirectorMode(bool bdirector);
        /// <summary>
        /// 设置是否可接收到VR设备穿戴事件
        /// </summary>
        /// <param name="benable"></param>
        public extern void SetEnableVrDeviceOnOffEvent(bool benable);
        /// <summary>
        /// 打开log调试界面
        /// </summary>
        /// <param name="benable">是否打开</param>
        public extern void SetLogerEnable(bool benable);

        /// <summary>
        /// 打开大屏幕可拖拽模式
        /// </summary>
        /// <param name="benable">是否打开</param>
        public extern void SetBigscreenGrabEnabled(bool benable);
        /// <summary>
        /// 设置自己隐身,不同步自身数据给其他人，无法与别人交流
        /// </summary>
        /// <param name="bhide">是否隐藏</param>
        public extern void SetSelfAvatarHide(bool bhide);
        /// <summary>
        /// 向云渲染发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="delay"></param>
        public extern void SendMessageToCloudRender(string msg, float delay = 0);
        /// <summary>
        /// 云渲染输入框转到网页输入框输入显示
        /// </summary>
        /// <param name="field"></param>
        /// <param name="delay"></param>
        public extern void BindInputToCloudRenderKeyboard(InputField field, float delay = 0);
        /// <summary>
        /// 设置云渲染退出时发送的json数据
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="delay"></param>
        public extern void SetCloudRenderExitData(string jsonData, float delay = 0);
        /// <summary>
        /// 云渲染开启关闭应用麦克风 使用网页麦克风
        /// </summary>
        /// <param name="bopen"></param>
        /// <param name="delay"></param>
        public extern void SetCloudRenderOpenMicrophone(bool bopen, float delay = 0);
        /// <summary>
        /// 云渲染打开关闭网页摇杆
        /// </summary>
        /// <param name="bopen"></param>
        /// <param name="delay"></param>
        public extern void SetCloudRenderOpenJoystick(bool bopen, float delay = 0);
        /// <summary>
        /// 获取当前unity版本
        /// </summary>
        /// <returns>unity版本号</returns>
        public extern string GetNowUnityVersion();
        /// <summary>
        /// 获取当前应用名称
        /// </summary>
        /// <returns>当前应用名称</returns>
        public extern string GetAppName();
        /// <summary>
        /// 获取当前应用版本号
        /// </summary>
        /// <returns>应用版本号</returns>
        public extern string GetAppVersion();
        /// <summary>
        /// 获取动作服务器参数Key
        /// </summary>
        /// <returns>动作服务器参数</returns>
        public extern string GetApiKey();
        /// <summary>
        /// 获取动作服务器参数Token
        /// </summary>
        /// <returns>动作服务器参数</returns>
        public extern string GetApiToken();

        /// <summary>
        /// 获取接口版本
        /// </summary>
        /// <returns>接口版本</returns>
        public extern string GetApiVersion();
        /// <summary>
        /// 获取当前语言
        /// </summary>
        /// <returns>当前语言</returns>
        public extern LanguageType GetChoosedLanguage();
        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="type"></param>
        public extern void SetLanguage(LanguageType type);
        /// <summary>
        /// 获取当前主相机
        /// </summary>
        /// <returns>当前主相机</returns>
        public extern Camera GetMainCamera();
        /// <summary>
        /// 获取VR相机
        /// </summary>
        /// <returns>VR相机</returns>
        public extern Camera[] GetVRCameras();
        /// <summary>
        /// 获取VR左手设备
        /// </summary>
        /// <returns>VR左手设备</returns>
        public extern Transform GetVRLeftHand();
        /// <summary>
        /// 获取VR右手设备
        /// </summary>
        /// <returns>VR右手设备</returns>
        public extern Transform GetVRRightHand();
        /// <summary>
        /// 获取VR左手传送锚点
        /// </summary>
        /// <returns>VR左手传送锚点</returns>
        public extern Transform GetVRLeftTeleportAnchor();
        /// <summary>
        /// 获取VR右手传送锚点
        /// </summary>
        /// <returns>VR右手传送锚点</returns>
        public extern Transform GetVRRightTeleportAnchor();
        /// <summary>
        /// 获取VR左手手指锚点
        /// </summary>
        /// <returns>VR左手手指锚点</returns>
        public extern Transform GetVRLeftFingerAnchor();
        /// <summary>
        /// 获取VR右手手指锚点
        /// </summary>
        /// <returns>VR右手手指锚点</returns>
        public extern Transform GetVRRightFingerAnchor();
        /// <summary>
        /// 获取主角的控制器（可以获取character controller）
        /// </summary>
        /// <returns>主角的控制器</returns>
        public extern Transform GetMainCharacter();
        /// <summary>
        /// 获取自由视角相机
        /// </summary>
        /// <returns>自由视角相机</returns>
        public extern Transform GetFreeCamera();
        /// <summary>
        /// 获取主角视角拍照相机
        /// </summary>
        /// <returns>主角视角拍照相机</returns>
        public extern Camera GetPhotoCamera();
        /// <summary>
        /// 获取系统菜单锚点
        /// </summary>
        /// <returns>系统菜单锚点</returns>
        public extern Transform GetSystemMenuAnchor();
        /// <summary>
        /// 是否为导播相机模式
        /// </summary>
        /// <returns>是否为全局相机</returns>
        public extern bool IsDirectorCamera();

        /// <summary>
        /// 获取VR射线终点物体
        /// </summary>
        /// <returns>检测到的物体</returns>
        public extern Transform GetLaserEndPoint();
        /// <summary>
        /// 获取人物的相对位置（MainVRROOT的子物体）
        /// </summary>
        /// <returns>相对位置</returns>
        public extern Transform GetCharacterTrackFix();
        /// <summary>
        /// 获取当前登录服务器url
        /// </summary>
        /// <returns>服务器url</returns>
        public extern string GetNowServerUrl();
        /// <summary>
        /// 获取当前测试服务器url
        /// </summary>
        /// <returns>服务器url</returns>
        public extern string GetNowTestServerUrl();
        /// <summary>
        /// 获取中台服务器域名
        /// </summary>
        /// <returns>中台服务器域名</returns>
        public extern string GetMiddlePlatformServer();
        /// <summary>
        /// 获取中台配置地址
        /// </summary>
        /// <returns>中台配置地址</returns>
        public extern string GetMiddlePlatformConfigUrl();
        /// <summary>
        /// 获取备用服务器地址
        /// </summary>
        /// <returns>备用服务器地址</returns>
        public extern string GetBackupServerUrl();
        /// <summary>
        /// 获取用户版本
        /// </summary>
        /// <returns>用户版本</returns>
        public extern string GetUserVersion();
        /// <summary>
        /// 获取后台用户数据缓存
        /// </summary>
        /// <returns>后台用户数据缓存</returns>
        public extern JsonData GetMyUserData();
        /// <summary>
        /// 获取区分平台的请求
        /// </summary>
        /// <returns>区分平台的请求</returns>
        public extern string GetPlatformTokenFixUrl();
        /// <summary>
        /// 获取当前组织id
        /// </summary>
        /// <returns>组织id</returns>
        public extern string GetNowGroupId();
        /// <summary>
        /// 获取当前场景的前缀（用来区分平台的）
        /// </summary>
        /// <returns>当前场景的前缀</returns>
        public extern string GetNowScenePrefix();
        /// <summary>
        /// 判断是否是离线频道
        /// </summary>
        /// <returns>是否是离线频道</returns>
        public extern bool IsRoomLocalMode();
        /// <summary>
        /// 判断人物连接频道完成
        /// </summary>
        /// <returns>人物连接频道完成</returns>
        public extern bool IsAvatarReady();
        /// <summary>
        /// 获取当前频道id
        /// </summary>
        /// <returns>当前频道id</returns>
        public extern string GetNowRoomID();
        /// <summary>
        /// 获取当前动作服务器websocket url
        /// </summary>
        /// <returns>当前动作服务器websocket url</returns>
        public extern string GetNowRoomServerUrl();
        /// <summary>
        /// 获取当前动作频道id
        /// </summary>
        /// <returns>当前动作频道id</returns>
        public extern string GetNowActionRoomID();
        /// <summary>
        /// 获取当前语音的appid
        /// </summary>
        /// <returns>当前语音的appid</returns>
        public extern string GetNowRoomVoiceAppID();
        /// <summary>
        /// 获取当前语音的房间ID
        /// </summary>
        /// <returns>当前语音的房间ID</returns>
        public extern string GetNowRoomVoiceRoomID();
        /// <summary>
        /// 获取当前语音子房间ID
        /// </summary>
        /// <returns>当前语音子房间ID</returns>
        public extern string GetNowRoomVoiceExRoomID();
        /// <summary>
        /// 获取当前频道的管理员命令
        /// </summary>
        /// <returns>当前频道的管理员命令</returns>
        public extern string GetRoomAdminCommand();
        /// <summary>
        /// 设置当前频道的管理员命令
        /// 用于系统控制面板输入口令开启管理
        /// </summary>
        /// <param name="command">管理员命令</param>
        public extern void SetRoomAdminCommand(string command);
        /// <summary>
        /// 获取房间后台配置数据
        /// </summary>
        /// <returns>屏幕分享相关配置</returns>
        public extern Dictionary<string, string> GetNowRoomSettings();
        /// <summary>
        /// 获取软件热更新版本相关配置
        /// </summary>
        /// <returns>软件热更新版本相关配置</returns>
        public extern Dictionary<string, string> GetAppVersionSettings();
        /// <summary>
        /// 判断当前频道是否可以下载资源
        /// </summary>
        /// <returns>当前频道是否可以下载资源</returns>
        public extern bool IsRoomEnableMediaRes();
        /// <summary>
        /// 获取当前频道加载图标url
        /// </summary>
        /// <returns>当前频道加载图标url</returns>
        public extern string GetNowSceneIconUrl();
        /// <summary>
        /// 获取当前频道名称
        /// </summary>
        /// <returns>当前频道名称</returns>
        public extern string GetNowSceneName();
        /// <summary>
        /// 判断加载场景时是否看见人（主要用于vr）
        /// </summary>
        /// <returns>是否看见人</returns>
        public extern bool IsAvatarVisibleOnLoading();
        /// <summary>
        /// 获取当前房间声场范围
        /// </summary>
        /// <returns>当前音量范围</returns>
        public extern int GetNowRoomVoiceRange();
        /// <summary>
        /// 获取语音房间的appkey（时效）
        /// </summary>
        /// <returns>语音房间的appkey（时效）</returns>
        public extern string GetNowRoomVoiceAppKey();
        /// <summary>
        /// 获取频道最大人数
        /// </summary>
        /// <returns>频道最大人数</returns>
        public extern int GetNowRoomMaxUserCount();
        /// <summary>
        /// 获取最大avatar可见数量（超过这人数的人能进入但其他就看不见了）
        /// </summary>
        /// <returns>最大可见数量</returns>
        public extern int GetNowRoomMaxVisibleUserCount();
        /// <summary>
        /// 判断麦克风开关状态
        /// </summary>
        /// <returns>开关状态</returns>
        public extern bool IsMicrophoneEnable();
        /// <summary>
        /// 获取当前麦克风音量
        /// </summary>
        /// <returns>当前麦克风音量</returns>
        public extern int GetNowMicrophoneVolume();
        /// <summary>
        /// 获取当前频道绑定的初始场景信息
        /// </summary>
        /// <returns>场景信息</returns>
        public extern WsSceneInfo GetNowRoomLinkSceneInfo();
        /// <summary>
        /// 获取当前场景信息
        /// </summary>
        /// <returns>场景信息</returns>
        public extern WsSceneInfo GetNowSceneInfo();
        /// <summary>
        /// 获取自己的人物的avatar角色id
        /// </summary>
        /// <returns>角色id</returns>
        public extern string GetMyAvatarID();
        /// <summary>
        /// 获取自己的人物的avatar角色模型id
        /// </summary>
        /// <returns>角色模型id</returns>
        public extern string GetMyAvatarModelID();
        /// <summary>
        /// 设置自己的人物的avatar角色模型id
        /// </summary>
        /// <param name="aid">角色模型id</param>
        public extern void SetMyAvatarModelID(string aid);
        /// <summary>
        /// 获取自己的人物的avatar角色初始模型id
        /// </summary>
        /// <returns>模型id</returns>
        public extern string GetMyAvatarDefaultModelID();
        /// <summary>
        /// 设置自己的人物的avatar角色初始模型id
        /// </summary>
        /// <param name="aid">模型id</param>
        public extern void SetMyAvatarDefaultModelID(string aid);
        /// <summary>
        /// 获取自己avatar角色的性别
        /// </summary>
        /// <returns>性别</returns>
        public extern int GetMyAvatarSex();
        /// <summary>
        /// 获取自己的昵称
        /// </summary>
        /// <returns>昵称</returns>
        public extern string GetMyNickName();
        /// <summary>
        /// 设置自己的昵称
        /// </summary>
        /// <param name="name">昵称</param>
        public extern void SetMyNickName(string name);
        /// <summary>
        /// 判断vr激光笔是否打开
        /// </summary>
        /// <returns>是否打开</returns>
        public extern bool IsLaserPenEnable();
        /// <summary>
        /// 判断语音连接是否成功
        /// </summary>
        /// <returns>是否成功</returns>
        public extern bool IsRoomVoiceConnected();
        /// <summary>
        /// 获取当前选择avatar角色id
        /// </summary>
        /// <returns>角色id</returns>
        public extern string GetNowSelectAvatarID();
        /// <summary>
        /// 设置当前选择avatar角色id
        /// </summary>
        /// <param name="avatarid">角色id</param>
        public extern void SetNowSelectAvatarID(string avatarid);
        /// <summary>
        /// 判断是否为vr应用
        /// </summary>
        /// <returns>是否为vr应用</returns>
        public extern bool IsPlatformVrApp();
        /// <summary>
        /// 判断是否为手机端
        /// </summary>
        /// <returns>是否为手机端</returns>
        public extern bool IsPlatformPhone();
        /// <summary>
        /// 判断是否为云渲染端
        /// </summary>
        /// <returns>是否为云渲染端</returns>
        public extern bool IsPlatformCloudRender();
        /// <summary>
        /// 判断是否为云渲染手机端
        /// </summary>
        /// <returns>是否为云渲染手机端</returns>
        public extern bool IsPlatformCloudRenderPhone();
        /// <summary>
        /// 判断是否为云渲染端小程序
        /// </summary>
        /// <returns>是否为云渲染端小程序</returns>
        public extern bool IsPlatformMiniProgram();
        /// <summary>
        /// 获取当前位置组名称
        /// </summary>
        /// <returns>当前位置组名称</returns>
        public extern string GetNowPlaceGroupName();
        /// <summary>
        /// 获取资源服务器地址(用于拼接)
        /// </summary>
        /// <returns>资源服务器地址</returns>
        public extern string GetMediaResServerUrl();
        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <returns>是否为管理员</returns>
        public extern bool IsAdmin();
        /// <summary>
        /// 判断是否为助理管理员
        /// </summary>
        /// <returns>判断是否为助理管理员</returns>
        public extern bool IsSAmin();
        /// <summary>
        /// 判断自己的avatar角色是否隐藏
        /// </summary>
        /// <returns>是否隐藏</returns>
        public extern bool IsMyAvatarHide();
        /// <summary>
        /// 设置加载glb场景的初始位置、旋转、缩放
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="scale"></param>
        public extern void SetGlbSceneLoadTransform(Vector3 pos, Vector3 rot, float scale);
        /// <summary>
        /// 设置加载glb物体的初始位置、旋转、缩放
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="rot">旋转</param>
        /// <param name="scale">缩放</param>
        public extern void SetGlbObjectLoadTransform(Vector3 pos, Vector3 rot, float scale);
        /// <summary>
        /// 获取glb模型的根路径
        /// </summary>
        /// <returns>模型的根路径</returns>
        public extern Transform GetGlbRoot();
        /// <summary>
        /// 获取glbScene模型的根路径
        /// </summary>
        /// <returns>模型的根路径</returns>
        public extern Transform GetGlbSceneRoot();
        /// <summary>
        /// 获取glbObj模型的根路径
        /// </summary>
        /// <returns>模型的根路径</returns>
        public extern Transform GetGlbObjectRoot();
        /// <summary>
        /// 获取大屏幕的根路径（主要用来控制位置）
        /// </summary>
        /// <returns>根路径</returns>
        public extern Transform GetBigScreenRoot();

        /// <summary>
        /// 获取大屏幕显示根节点（主要用来控制显示隐藏）
        /// </summary>
        /// <returns>大屏幕</returns>
        public extern Transform GetBigScreenViewRoot();
        /// <summary>
        /// 获取大屏幕显示面板（图片RawImage显示）
        /// </summary>
        /// <returns></returns>
        public extern RawImage GetBigScreenView();
        /// <summary>
        /// 获取大屏幕系统默认图
        /// </summary>
        /// <returns></returns>
        public extern Texture2D GetBigScreenDefaultTexture();
        /// <summary>
        /// 获取屏幕分享大屏（whiteBoard）
        /// </summary>
        /// <returns>分享大屏</returns>
        public extern Transform GetScreenSharePanel();
        /// <summary>
        /// 判断是否显示avatar角色扬声器头标
        /// </summary>
        /// <returns>是否显示</returns>
        public extern bool IsShowAvatarSpeakIcon();
        /// <summary>
        /// 设置显示avatar角色扬声器头标
        /// </summary>
        /// <param name="bshow">是否显示</param>
        public extern void ShowAvatarSpeakIcon(bool bshow, float delay = 0);

        /// <summary>
        /// VR端遥感转向
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="delay"></param>
        public extern void EnableVRStickControlRotate(bool bEnable, float delay);
        /// <summary>
        /// VR端遥感行走
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="delay"></param>
        public extern void EnableVRStickControlWalk(bool bEnable, float delay);
        /// <summary>
        /// 判断是否显示avatar角色名牌
        /// </summary>
        /// <returns>是否显示</returns>
        public extern bool IsShowAvatarNamePanel();
        /// <summary>
        /// 判断是否为全景模式
        /// </summary>
        /// <returns>是否为全景模式</returns>
        public extern bool IsPanoramaMode();
        /// <summary>
        /// 获取当前设备名称(ios,mac,pc,phone等)
        /// </summary>
        /// <returns>设备名称</returns>
        public extern string GetNowDeviceName();
        /// <summary>
        /// 获取当前设备编号
        /// </summary>
        /// <returns>设备编号</returns>
        public extern string GetNowDeviceSNNumber();
        /// <summary>
        /// 判断是否开启画笔
        /// </summary>
        /// <returns>是否开启画笔</returns>
        public extern bool IsOpenDrawing();
        /// <summary>
        /// 获取最新链接频道房间数据
        /// </summary>
        /// <returns>房间数据</returns>
        public extern List<VRRootChanelRoom> GetLastLinkChannelRoomData();
        /// <summary>
        /// 获取当前频道房间内的所有人物数据字典（key 为avatarID）
        /// </summary>
        /// <returns>人物数据字典</returns>
        public extern Dictionary<string, WsAvatarFrame> GetAllAvatarData();
        /// <summary>
        /// 获取所有AvatarID列表
        /// </summary>
        /// <returns>AvatarID列表</returns>
        public extern List<string> GetAllAvatarID();
        /// <summary>
        /// 获取当前显示的AvatarID列表
        /// </summary>
        /// <returns>AvatarID列表</returns>
        public extern List<string> GetAllActiveAvatarID();
        /// <summary>
        /// 获取所有显示的Avatar的帧同步数据
        /// </summary>
        /// <returns>帧同步数据</returns>
        public extern Dictionary<string, WsAvatarFrameJian> GetAllActiveAvatarData();
        /// <summary>
        /// 获取当前Avatar的排序（第一个为Admin）
        /// </summary>
        /// <param name="avatarid">排序</param>
        /// <returns></returns>
        public extern int GetAvatarInQueueIndex(string avatarid);
        /// <summary>
        /// 获取所有Avatar的昵称列表
        /// </summary>
        /// <returns>昵称列表</returns>
        public extern List<string> GetAllAvatarName();
        /// <summary>
        /// 获取当前显示的Avatar的昵称列表
        /// </summary>
        /// <returns>昵称列表</returns>
        public extern List<string> GetAllActiveAvatarName();
        /// <summary>
        /// 获取激光笔颜色
        /// </summary>
        /// <returns>颜色 如041A5ACC  RGBA</returns>
        public extern string GetLaserPenColor();
        /// <summary>
        /// 设置激光笔颜色
        /// </summary>
        /// <param name="color">颜色 如041A5ACC  RGBA</param>
        public extern void SetLaserPenColor(string color);
        /// <summary>
        /// 判断场景是否更新完成
        /// </summary>
        /// <returns>是否更新完成</returns>
        public extern bool IsSceneUpdateComplete();
        /// <summary>
        /// 判断是否隐藏场景中所有Avatar
        /// </summary>
        /// <returns>是否隐藏</returns>
        public extern bool IsHideAllAvatar();
        /// <summary>
        /// 获取人物的默认高度
        /// </summary>
        /// <returns>默认高度</returns>
        public extern float GetAvatarDefaultHeight();
        /// <summary>
        /// 设置人物的默认高度
        /// </summary>
        /// <param name="height">默认高度</param>
        public extern void SetAvatarDefaultHeight(float height);
        /// <summary>
        /// 获取avatar角色最大可见范围（相对自己，超过这个范围的角色删除）
        /// </summary>
        /// <returns>范围半径</returns>
        public extern float GetMaxAvatarVisibleRange();
        /// <summary>
        /// 设置avatar角色最大可见范围（相对自己，超过这个范围的角色删除）
        /// </summary>
        /// <param name="maxrange">范围半径</param>
        public extern void SetMaxAvatarVisibleRange(float maxrange);
        /// <summary>
        /// 获取avatar角色最大可见范围（相对自己，超过这个范围的角色隐藏）
        /// </summary>
        /// <returns>范围半径</returns>
        public extern float GetMaxAvatarModelVisibleRange();
        /// <summary>
        /// 设置avatar角色最大可见范围（相对自己，超过这个范围的角色隐藏）
        /// </summary>
        /// <param name="maxrange">范围半径</param>
        public extern void SetMaxAvatarModelVisibleRange(float maxrange);
        /// <summary>
        /// 获取avatar角色最小可见范围（相对自己，avatar小于这个范围隐藏）
        /// </summary>
        /// <returns>范围半径</returns>
        public extern float GetMinAvatarModelVisibleRange();
        /// <summary>
        /// 设置avatar角色最小可见范围（相对自己，avatar小于这个范围隐藏）
        /// </summary>
        /// <returns>范围半径</returns>
        public extern void SetMinAvatarModelVisibleRange(float minrange);
        /// <summary>
        /// 获取最大显示avatar角色模型数量
        /// </summary>
        /// <returns>模型数量</returns>
        public extern int GetMaxShowAvatarModelCount();
        /// <summary>
        /// 设置最大显示avatar角色模型数量
        /// </summary>
        /// <param name="maxcount">模型数量</param>
        public extern void SetMaxShowAvatarModelCount(int maxcount);
        /// <summary>
        /// 获取当前频道所有人物数据缓存（大约4秒同步一次数据）
        /// </summary>
        /// <returns>数据缓存</returns>
        public extern WsAvatarFrameList GetNowRoomAvatarFrameData();
        /// <summary>
        /// 获取动作房间黑名单AvatarID  
        /// 在此名单的avatar将不会在场景中显示 房间内也不会有黑名单用户的数据
        /// </summary>
        /// <returns>avatarID列表</returns>
        public extern List<string> GetActionRoomBlackAvatarID();
        /// <summary>
        /// 设置动作黑名单AvatarID
        /// 在此名单的avatar将不会在场景中显示 房间内也不会有黑名单用户的数据
        /// </summary>
        /// <param name="blacklist">AvatarID列表</param>
        public extern void SetActionRoomBlackAvatarID(List<string> blacklist);
        /// <summary>
        /// 获取语音房间黑名单AvatarID
        /// 在此名单中的无法与此用户语音交流
        /// </summary>
        /// <returns>AvatarID列表</returns>
        public extern List<string> GetVoiceRoomBlackAvatarID();
        /// <summary>
        /// 设置语音黑名单AvatarID
        /// 在此名单中的无法与此用户语音交流
        /// </summary>
        /// <param name="blacklist">AvatarID列表</param>
        public extern void SetVoiceRoomBlackAvatarID(List<string> blacklist);
        /// <summary>
        /// 设置移动位置组延迟时间
        /// </summary>
        /// <param name="delaytime">延迟时间</param>
        public extern void SetStartPlaceGroupDelayTime(float delaytime);
        /// <summary>
        /// 判断是否仅管理员打开激光笔
        /// </summary>
        /// <returns>是否仅管理员打开激光笔</returns>
        public extern bool IsAdminUserLaserPenOnly();
        /// <summary>
        /// 设置是否仅管理员打开激光笔
        /// </summary>
        /// <param name="badmin">是否仅管理员打开激光笔</param>
        public extern void SetAdminUserLaserPenOnly(bool badmin);
        /// <summary>
        /// 判断是否屏蔽激光笔
        /// </summary>
        /// <returns>是否屏蔽激光笔</returns>
        public extern bool IsBlockLaserPen();
        /// <summary>
        /// 设置是否屏蔽激光笔
        /// </summary>
        /// <param name="block">是否屏蔽激光笔</param>
        public extern void SetBlockLaserPen(bool block);
        /// <summary>
        /// 判断是否为第三人物视角模式
        /// </summary>
        /// <returns>是否为第三人物视角模式</returns>
        public extern bool IsThirdPersonMode();
        /// <summary>
        /// 获取相机近截面
        /// </summary>
        /// <returns>相机近截面</returns>
        public extern float GetVrCameraNearClipPlane();
        /// <summary>
        /// 设置相机近截面
        /// </summary>
        /// <param name="value">相机近截面</param>
        public extern void SetVrCameraNearClipPlane(float value);
        /// <summary>
        /// 获取相机远截面
        /// </summary>
        /// <returns>相机远截面</returns>
        public extern float GetVrCameraFarClipPlane();
        /// <summary>
        /// 设置相机远截面
        /// </summary>
        /// <param name="value">相机远截面</param>
        public extern void SetVrCameraFarClipPlane(float value);
        /// <summary>
        /// 判断自己是否隐身（相对自己）
        /// </summary>
        /// <returns>是否隐身</returns>
        public extern bool IsSelfHideVisibleModel();
        /// <summary>
        /// 设置自己是否隐身（相对自己）
        /// </summary>
        /// <param name="bvisible">是否隐身</param>
        public extern void SetSelfHideVisibleModel(bool bvisible);
        /// <summary>
        /// 获取屏幕分享用户数据
        /// </summary>
        /// <returns>用户数据</returns>
        public extern List<ScreenShareData> GetScreenShareUserData();
        /// <summary>
        /// 获取名牌显示的最大距离 （超过这个值名牌将隐藏）
        /// </summary>
        /// <returns>最大距离</returns>
        public extern float GetNamePanelVisibleMaxRange();
        /// <summary>
        /// 设置名牌显示的最大距离 （超过这个值名牌将隐藏
        /// </summary>
        /// <param name="maxrange">最大距离</param>
        public extern void SetNamePanelVisibleMaxRange(float maxrange);
        /// <summary>
        /// 获取名牌最大缩放值（系统限制限定名牌缩放的最大值为3）
        /// </summary>
        /// <returns>缩放值</returns>
        public extern float GetNamePanelMaxScale();
        /// <summary>
        /// 设置名牌最大缩放值（系统限制限定名牌缩放的最大值为3）
        /// </summary>
        /// <param name="maxscale">缩放值</param>
        public extern void SetNamePanelMaxScale(float maxscale);
        /// <summary>
        /// 判断当前是否正在翻转空间
        /// </summary>
        /// <returns>是否正在翻转</returns>
        public extern bool IsSpaceFliping();
        /// <summary>
        /// 开启录制视频
        /// </summary>
        /// <param name="SaveName">  保存名称</param>
        /// <param name="videoWidth"> 视频宽度</param>
        /// <param name="videoHight"> 视频高度</param>
        /// <param name="frameRate">帧率</param>
        /// <param name="renderTexture">录制的Texture 为Null 时录制屏幕，不为空时录制该Texture画面</param>
        /// <param name="delay"></param>
        public extern void VideoCaptureStart(string SaveName, int videoWidth, int videoHight, int frameRate = 30, RenderTexture renderTexture = null, float delay = 0);
        /// <summary>
        /// 关闭录制视频
        /// </summary>
        /// <param name="delay"></param>
        public extern void VideoCaptureStop(float delay = 0);
        /// <summary>
        /// 开启音频录制功能
        /// </summary>
        /// <param name="SaveName">保存</param>
        /// <param name="delay"></param>
        public extern void CollectAudioStart(string SaveName, float delay = 0);
        /// <summary>
        /// 关闭音频录制
        /// </summary>
        /// <param name="delay"></param>
        public extern void CollectAudioStop(float delay = 0);
        /// <summary>
        /// 推流画面
        /// </summary>
        /// <param name="rtmpPath">推流地址</param>
        /// <param name="videoWidth">录制宽度</param>
        /// <param name="videoHight">录制高度</param>
        /// <param name="frameRate">录制帧率</param>
        /// <param name="renderTexture">录制的Texture 为Null 时录制屏幕，不为空时录制该Texture画面</param>
        /// <param name="delay"></param>
        public extern void VideoCaptureRtmpStart(string rtmpPath, int videoWidth, int videoHight, int frameRate, RenderTexture renderTexture = null, float delay = 0);
        /// <summary>
        /// 关闭画面推流
        /// </summary>
        /// <param name="delay"></param>
        public extern void VideoCaptureRtmpStop(float delay = 0);
        /// <summary>
        /// 分享应用窗口的画面时是否显示Canvas元素
        /// </summary>
        /// <param name="showpanel"></param>
        /// <param name="delay"></param>
        public extern void SetCaanvasOnGameScreenShare(Canvas showpanel, bool bShow, float delay);
        /// <summary>
        /// SDK资源释放
        /// </summary>
        public extern void Dispose();

        public extern void RequestCameraFaceType(float delay = 0);
        /// <summary>
        /// 设置翻译的语言
        /// </summary>
        /// <param name="Language"></param>
        /// <param name="delay"></param>
        public extern void SetupTranslateLanguage(string Language, float delay = 0);

        /// <summary>
        /// 第三视角切换是否锁定
        /// </summary>
        /// <param name="ModeID"></param>
        /// <param name="delay"></param>
        public extern void LockThirdPersonMode(int ModeID, float delay = 0);
        /// <summary>
        /// 设置网页是否置顶
        /// </summary>
        /// <param name="bTop"></param>
        /// <param name="delay"></param>
        public extern void SetWebviewOnTop(bool bTop, float delay = 0);
        /// <summary>
        /// 物体进行位置控制偏移旋转
        /// </summary>
        /// <param name="gameObject">物体</param>
        /// <param name="ControlType">控制类型 </param>
        /// <param name="delay"></param>
        public extern void StartGameObjectTransformController(GameObject gameObject, TransformControlType ControlType, float delay = 0);

        /// <summary>
        /// 停止物体位移控制
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="delay"></param>
        public extern void StopGameObjectTransformController(GameObject gameObject, float delay = 0);
        /// <summary>
        /// 设置物体控制相机
        /// </summary>
        /// <param name="bControl">是否控制 bool</param>
        /// <param name="camera">摄像机实例</param>
        /// <param name="delay"></param>
        public extern void SetTransformControlCamera(bool bControl, Camera camera, float delay = 0);
        /// <summary>
        /// 是否显示MIC图标
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="delay"></param>
        public extern void ShowMicRootEnable(bool bShow, float delay = 0);
        /// <summary>
        /// 是否显示跳跃图标
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="delay"></param>
        public extern void ShowJumpRootEnable(bool bShow, float delay = 0);
        /// <summary>
        /// 是否第三人称切换图标
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="delay"></param>
        public extern void ShowThirdPersionRootEnable(bool bShow, float delay = 0);
        /// <summary>
        /// 是否显示跑步图标
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="delay"></param>
        public extern void ShowBtnRunRootEnable(bool bShow, float delay = 0);
        /// <summary>
        /// 设置按钮是否打开
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="delay"></param>
        public extern void SettingButtonClick(bool bShow, float delay = 0);

        /// <summary>
        /// 显示或者关闭应用的遥感控制
        /// </summary>
        /// <param name="bShow"></param>
        /// <param name="delay"></param>
        public extern void ShowOrHideAppStickControl(bool bShow, float delay = 0);

    
        /// <summary>
        /// 保存Log信息到服务器
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="delay"></param>
        public extern void LogSaveToServer(string Data, float delay = 0);
        /// <summary>
        /// 校验文件是否缓存
        /// </summary>
        /// <param name="Sign"></param>
        /// <param name="delay"></param>
        public extern void CheckFileAlreadyCached(string Sign, float delay = 0);
        /// <summary>
        /// 设置语音重置
        /// </summary>
        /// <param name="delay"></param>
        public extern static void SetVRResetVoice(float delay = 0);
        /// <summary>
        /// 开启广播语音
        /// </summary>
        /// <param name="ExroomID"></param>
        /// <param name="Delay"></param>
        public extern void StartBroadcastVoice(string ExroomID, float Delay = 0);
     


        /// <summary>
        /// 设置默认大屏的图
        /// </summary>
        /// <param name="DefaultImage">默认图片</param>
        /// <param name="delay"></param>
        public extern void SetDefaultBigScreenImage(Texture DefaultImage, float delay = 0);

        /// <summary>
        /// 松开VR手柄握持的物体
        /// </summary>
        /// <param name="Gameobject"></param>
        /// <param name="delay"></param>
        public extern void StopGrabObject(GameObject Gameobject, float delay = 0);

        /// <summary>
        /// 加载AB包中的物体
        /// </summary>
        /// <param name="Url"> AB包文件地址</param>
        /// <param name="AssetName">资源名 为空时会加载AB包的默认首位资源</param>
        /// <param name="delay"></param>
        public extern void LoadAssetBundleObj(string Url, string AssetName = "", float delay = 0);

        /// <summary>
        /// 改变文件下载的超市时长
        /// </summary>
        /// <param name="outTime">时长 单位为毫秒</param>
        /// <param name="delay"></param>
        public extern void ChangeFileDownloadTimeOut(int outTime, float delay = 0);

        /// <summary>
        /// 取消下载文件资源
        /// </summary>
        /// <param name="fileSign"></param>
        /// <param name="delay"></param>
        public extern void CancelDownLoadAsset(string fileSign, float delay = 0);

        /// <summary>
        /// 设置导播模式是否启用
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="delay"></param>
        public extern void SetDirectorModelEnable(bool bEnable, float delay = 0);

        /// <summary>
        /// 退出空间
        /// </summary>
        /// <param name="delay"></param>
        public extern void VRExitRoom(float delay = 0);

        /// <summary>
        /// 设置UI控制退出
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="delay"></param>
        public extern void SetUIControllerEnable(bool bEnable, float delay = 0);

        /// <summary>
        /// 切换分享摄像头
        /// </summary>
        /// <param name="delay"></param>
        public extern void SwitchShareCamera(float delay = 0);

        /// <summary>
        /// 开始分享场景内的画面
        /// </summary>
        /// <param name="showpanel"> 分画面的名字</param>
        /// <param name="delay"></param>
        public extern void StartShareGameScreen(string showpanel, float delay = 0);

        /// <summary>
        /// 停止分享场景内画面
        /// </summary>
        /// <param name="delay"></param>
        public extern void StopShareGameScreen(float delay = 0);

        /// <summary>
        /// 停止流式请求
        /// </summary>
        /// <param name="RequestID"></param>
        /// <param name="delay"></param>
        public extern void StopChatStreamRequest(string RequestID, float delay = 0);

        /// <summary>
        /// 点击地面行走功能是否启用
        /// </summary>
        /// <param name="bEnable">开启或者关闭</param>
        /// <param name="delay"></param>
        public extern void ClickGroundWalkEnable(bool bEnable, float delay = 0);
       
        /// <summary>
        /// 开启自动寻路模式
        /// 注：1.主要通过消息体的内容区分是指定跟随还是自由导航模式，指定目标跟随需要传入followtarget的值，自由导航模式不需要传入followtarget的值，但是需要传入targetpos的值
        /// 2.场景要注意设置障碍物
        /// </summary>
        /// <param name="FollowTaret">跟随的角色 不为空时会一直跟随</param>
        /// <param name="targetpos">目标位置(指定目标寻路时 这个参数值不使用)</param>
        /// <param name="offset"> 自动寻路距离目标点的偏移值</param>
        /// <param name="movetype">暂时没有使用预留值</param>
        /// <param name="delay">延迟调用 秒</param>
        public extern void OnStartAutoFindPath(GameObject FollowTaret, Vector3 targetpos, Vector3 offset, int movetype, float delay = 0);
        /// <summary>
        /// 停止自动寻路
        /// </summary>
        public extern void StopAutoFindPath();

        /// <summary>
        /// 更新地图烘培数据
        /// </summary>
        public extern void OnUpdateNavmeshData();

        /// <summary>
        /// 指定摄像机的设备信息
        /// </summary>
        /// <param name="PanelName">摄像头投屏的页面</param>
        /// <param name="DeviceInfo">摄像头驱动信息</param>
        /// <param name="delay"></param>
        public extern void AppointCameraDeviceInfo(string PanelName, string DeviceInfo, float delay = 0);
        /// <summary>
        /// 请求所有的摄像头信息。用于SDK端指定摄像头分享
        /// </summary>
        /// <param name="delay"></param>
        public extern void SendCameraDeviceInfo(float delay = 0);
        /// <summary>
        /// 关闭下排菜单页面
        /// </summary>
        /// <param name="delay"></param>
        public extern void CloseUnderMenuPanel(float delay = 0);
        /// <summary>
        /// 是否开启RTC推流
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="delay"></param>
        public extern void RTCPushEnable(bool bEnable, float delay = 0);

        /// <summary>
        /// 隐藏或显示Rtc的UI
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="delay"></param>
        public extern void ShowOrHideRTCUI(bool bEnable, float delay = 0);
        /// <summary>
        /// 设置第三人称切换摄像机的偏移值
        /// </summary>
        /// <param name="offset">偏移值</param>
        /// <param name="delay"></param>
        public extern void ThirdPersionChangeCameraOffset(Vector3 offset, float delay = 0);
        /// <summary>
        /// 恢复摄像机的偏移值
        /// </summary>
        /// <param name="delay"></param>
        public extern void RevertThirdPersionCameraOffset(float delay = 0);


        /// <summary>
        /// 获取摄像头的设备信息
        /// </summary>
        /// <returns></returns>
        public extern string GetCameraDeviceInfo();
        /// <summary>
        /// 关闭场景加载结束的黑色渐变遮罩
        /// </summary>
        public extern void CloseSceneLoadBlackGradientMask(float delay = 0);
       /// <summary>
       /// 设置第一人称的摄像机位置
       /// </summary>
       /// <param name="x"></param>
       /// <param name="y"></param>
       /// <param name="z"></param>
       /// <param name="animation">是直接切换还是有摄像机切换的过程动画</param>
       /// <param name="duration">切换动画的播放时长</param>
       /// <param name="delay"></param>
        public extern void SetFirstPersonCameraPosition(float x, float y, float z, bool animation, float duration, float delay = 0);
        /// <summary>
        /// 设置第一人称的摄像机旋转
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="animation">是直接切换还是有摄像机切换的过程动画</param>
        /// <param name="duration">切换动画的播放时长</param>
        /// <param name="delay"></param>
        public extern void SetFirstPersonCameraRotate(float x, float y, float z, bool animation, float duration, float delay = 0);
        /// <summary>
        /// 删除主工程的物体
        /// </summary>
        /// <param name="targetObject">目标物体</param>
        /// <param name="delay"></param>
        public extern void SDKDestroyMainObject(GameObject targetObject, float delay);
        /// <summary>
        /// 切换Rtc分享摄像机
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="Delay"></param>
        public extern void ChangeRtcShareCamera(Camera camera, float Delay = 0);
        /// <summary>
        /// 是否禁用VR按钮的键值
        /// </summary>
        /// <param name="setButtonEnable">键值字典，例禁用手柄Y键：{VRRaw_Y_ButtonDown，flase}</param>
        /// <param name="delay"></param>
        public extern void DisableVRButtonKeyCode(Dictionary<string, bool> setButtonEnable, float delay = 0);
        /// <summary>
        /// 设置VR端遥感行走时的黑色遮罩显示或隐藏
        /// </summary>
        /// <param name="isShow"></param>
        /// <param name="delay"></param>
        public extern void SetVrStickMoveMaskShowORHide(bool isShow, float delay = 0);
        /// <summary>
        /// 设置VR端遥感移动的速度
        /// </summary>
        /// <param name="speed">默认值参考值是0.04f</param>
        /// <param name="delay"></param>
        public extern void SetVrStickMoveSpeed(float speed = 0.04f, float delay = 0);
        /// <summary>
        /// 用系统APP打开文件
        /// </summary>
        /// <param name="filePath">文件地址</param>
        /// <param name="delay"></param>
        public extern void OpenFileBySystemApp(string filePath, float delay = 0);
        /// <summary>
        /// 是否开启跳跃功能
        /// </summary>
        /// <param name="bJump"></param>
        /// <param name="delay"></param>
        public extern void JumpControlEnabled(bool bJump, float delay = 0);
        /// <summary>
        /// 是否打开主工程的菜单页面
        /// </summary>
        /// <param name="isopen"></param>
        /// <param name="delay"></param>
        public extern void OpenMainMenuPanel(bool isopen, float delay = 0);
        /// <summary>
        /// 空间回退时是否要记录人物的位置
        /// </summary>
        /// <param name="Postion"></param>
        /// <param name="quaternion"></param>
        /// <param name="Scale"></param>
        /// <param name="delay"></param>
        public extern void RecodeCallBackSceneAvatarPOS(Vector3 Postion, Quaternion quaternion, Vector3 Scale, float delay = 0);
        /// <summary>
        /// 会议模式中打开音频切换的页面
        /// </summary>
        /// <param name="isOpen"></param>
        /// <param name="Delay"></param>
        public extern void OpenAudioChangePanel(bool isOpen, float Delay = 0);
        /// <summary>
        /// 会议模式中显示或隐藏音频切换的Toggle
        /// </summary>
        /// <param name="isOpen"></param>
        /// <param name="Delay"></param>
        public extern void WebRtcShowOrHideAudioChangeToggle(bool isOpen, float Delay = 0);
        /// <summary>
        /// 开启绿幕直播
        /// </summary>
        /// <param name="Type"> webCamera 0  ,CameraShare 1   ,Video 2</param>
        /// <param name="UserID"></param>
        /// <param name="delay"></param>
        public extern void StartLiveBroadcast(int Type, string UserID, float delay = 0);
        /// <summary>
        /// 关闭绿幕直播
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="delay"></param>
        public extern void StopLiveBroadcast(string UserID, float delay = 0);
        /// <summary>
        /// 设置直播位置
        /// </summary>
        /// <param name="fixedTransform">把绿幕放到指定位置</param>
        /// <param name="offset">偏移</param>
        /// <param name="lookForward"></param>
        /// <param name="expandjson">额外的数据</param>
        /// <param name="delay"></param>
        public extern void SetLiveBroadcastPos(Transform fixedTransform, Vector3 offset, bool lookForward, string expandjson, float delay = 0);
        /// <summary>
        /// 开启调节绿幕位置旋转缩放的三维轴
        /// </summary>
        /// <param name="bSET"></param>
        /// <param name="delay"></param>
        public extern void EnableSetDeckardChromaGizmo(bool bSET, float delay = 0);
        /// <summary>
        /// 开启会议模式大屏分享页面
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="delay"></param>
        public extern void OpenWebrtcBigScreenSharePanel(string UserID, float delay = 0);
        /// <summary>
        /// 关闭会议模式大屏分享页面
        /// </summary>
        public extern void CloseWebrtcBigScreenSharePanel();
        /// <summary>
        /// 设置会议模式的人员分享页面的开启和关闭
        /// </summary>
        /// <param name="IsOpen"></param>
        /// <param name="delay"></param>
        public extern void SetPersonnelControlPanel(bool IsOpen, float delay = 0);

        /// <summary>
        /// MAC和Windowss模拟按键消息
        /// </summary>
        /// <param name="keycode"></param>
        public extern void VirtualKeyCodeDown(int keycode);
        /// <summary>
        /// MAC和Windowss模拟按键消息
        /// </summary>
        /// <param name="keycode"></param>
        public extern void VirtualKeyCodeUp(int keycode);

#pragma warning restore CS0626
        #endregion
    }

}
