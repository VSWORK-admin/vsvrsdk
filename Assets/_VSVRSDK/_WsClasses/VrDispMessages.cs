using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WsMessageType
{
    // 角色
    RecieveWsAvatar, // WsAvatarFrame
    SendWsAvatar, // WsAvatarFrame
    // 切换位置
    SendPlaceMark, // WsPlaceMark
    RecievePlaceMark, // WsPlaceMark
    // 更改物体状态
    SendChangeObj, // WsChangeInfo
    RecieveChangeObj, // WsChangeInfo
    SendCChangeObj, // WsChangeInfo
    SendAllCChangeObj,
    RecieveCChangeObj, // WsChangeInfo
    // admin
    SendMarkAdmin, // WsAdminMark
    // teleport
    SendTeleportTo, // WsMultiTeleportInfo
    RecieveTeleportTo, // WsMultiTeleportInfo
    //loadScene
    SendLoadScene, // WsSceneInfo
    RecieveLoadScene, // WsSceneInfo
    // Moving Obj
    SendMovingObj,  // WsMovingObj
    RecieveMovingObj,// WsMovingObj
    RecieveConnected,
    SendnewAvatar,
    RecieveDelAvatar,
    RecieveNewAvartar,
    RecieveAdminChange,
    SendConnectReady,
    SendMedia,
    RecieveMedia,
    SendBigScreen,
    RecieveBigScreen,
    SendPCCamera,
    RecievePCCamera,
    SendProgress,
    RecieveProgress,
    SendCheckDelay,
    RecieveCheckDelay,
    SendChangeAvatar,
    SendUrlScene,
    RecieveAlist,
    RecieveCheckAvatar,
    RecieveAuthFaild,
    SendSaveData,
    SendGetData,
    RecieveGetData,
    SendChangeRoom,
    RecieveChangeRoom,
    SendCanvasView,
    RecieveCanvasView,
    SendVRVoice,
    RecieveVRVoice,
    SendVRPic,
    RecieveVRPic
}


public enum VrDispMessageType
{
    // 加载和卸载角色
    InitWsAvatar,
    DestroyWsAvatar,
    // 切换位置
    AllPlaceTo,
    SelfPlaceTo,
    // 更改物体状态
    ChangeObj,
    // teleport
    TeleportTo,
    TelePortToMesh,
    VRTelePortToMesh,
    //UIPointer & Teleporter status
    SelectorPointerStatusChange,
    TeleporterStatusChange,
    VRPlaycePointEnter,
    VRPlaycePointExit,
    VRPlaycePort,
    AllLoadScene, 
    LoadLocalPathScene,
    SceneChanged,
    InitSelfAvatar,
    DestroySelfAvatar,
    Adminchanged,
    SetAdmin,
    VoiceEvent,
    SceneFirstConnectWS,
    KODSelectedOneFile,
    KODGetOneImage,
    KODGetOneScene,
    KODGetOneMov,
    KODGetOneVROrder,
    KODGetOneVROrderString,
    KODGetOneGlb,
    KODGetOneTxt,
    KODGetOnePDF,
    KODGetOneLinkOrder,
    KODGetOneCacheOrder,
    KODGetTxtString,
    KODGetPDFPath,
    BigScreenSetPos,
    BigScreenShowImage,
    BigScreenPrepareVideo,
    BigScreenRecieveRTSP,
    BigScreenShowVideo,
    BigScreenShowVideoFrame,
    BigScreenUpdateImage,
    BigScreenStartAnchor,
    BigScreenEndAnchor,
    KeyboardChange,
    InputFildClicked,
    Logedin,
    LoginFalse,
    GetGroups,
    GetGroupRooms,
    GetSelfRooms,
    GetPublicRooms,
    Logedout,
    RoomConnected,
    RoomDisConnected,
    RoomConnectedError,
    RoomConnectedClose,
    GMEconnected,
    GMEdisconnected,
    GMEExitRoomComplete,
    CameraScreenSetPos,
    CameraScreenSetFree,
    CameraScreenSettingBegin,
    CameraScreenSettingEnd,
    CameraScreenSceneStart,
    CameraScreenCameraSet,
    GetRemoteScenes,
    DownloadURLIDScene,
    DownloadingRemoteScene,
    DownloadDoneRemoteScene,
    DownloadErrorRemoteScene,
    CommitOrder,
    SendAllInfolog,
    SendCacheFile,
    GetLocalCacheFile,
    SelectAvatarWsid,
    LoadingSceneProgress,
    CancelAllDownload,
    GetRemoteAvatars,
    SendInfolog,
    ShowNamePanel,
    ShowVolPanel,
    SystemMenuEnable,
    SceneMenuEnable,
    SystemMenuEvent,
    InitVideoPlayer,
    ChangePipeLine,
    SetMenuRootPos,
    CharactorEditing,
    ConnectToNewChanel,
    LoadGlbModels,
    LoadGlbModelsDone,
    AvatarSpeakStatusChange,
    OpenAPKByPackagename,
    CustomLocalMessage,
    SetDeviceAutoport,
    KODGetOnePPT3D,
    KODGetOneCommonFile,
    KeyboardInput,
    RoomConnectTimeOut,
    SendCheckFile,
    GetCheckFile,
    SendCheckScene,
    GetCheckScene,
    ForceSystemMenuEnable,
    WebCanvasClick,
    AvatarInited,
    HandTipsControl,
    VRLoadSceneStart,
    VRLoadSceneEnd,
    VRGetCacheProgress,
    VRDoRecieveCheckAvatarlist,
    VRDoLoadOneMSceneDirect,
    VRUserLeaveChanel,
    VRTryToReLinkChanel,
    LoadDefaultStartScene,
    VRDoRecieveConnection,
    LoadVRBundleScenes,
    VRSetScreenControlEnabled,
    VRSetScreenPadEnabled,
    VRRoomDisConnect,
    HideAvatarsExeptAdmin,
    HideAllAvatars,
    ShowAllAvatars,
    ShowSelectAvatar,
    VRAvatarPoseChange,
    VRAvatarLposeChange,
    VRAvatarRposeChange,
    VRLoadAvatarDone,
    VRScaleChange,
    VRGetOneOrder,
    /// <summary>
    /// 开始录制gif（参数类型：GifRecordData）
    /// </summary>
    StartRecordGif,
    /// <summary>
    /// 停止并保存录制的gif（无参数）
    /// </summary>
    StopAndSaveGif,
    /// <summary>
    /// 获取gif本地地址(参数:string 地址)
    /// </summary>
    SetGifLocalPath,
    /// <summary>
    /// 获取gif保存进度(参数:float 进度0-100)
    /// </summary>
    SetGifSaveProgress,
    /// <summary>
    /// 设置渲染质量，0，低；1，中；2，高；3，最高
    /// </summary>
    SetRenderQuality,
     
    /// <summary>
    /// 发送获取加载Fbx模型（参数：string AID）
    /// </summary>
    SendLoadFbxAvatar,
    /// <summary>
    /// 获取Fbx模型（参数：Gameobject model）
    /// </summary>
    GetLoadFbxAvatar,
    AvatarChanged,
    /// <summary>
    /// 修改名牌颜色
    /// </summary>
    ChangeNamePanelColor,
    /// <summary>
    SDKScriptDestroyed,
}

public enum PDFPlugingOrder
{
    InitPDFRendererPlayer,
    GetPDFTexture,
    SendPDFTexture,
    GetPDFTotalPageCount,
    SendPDFTotalPageCount,
}

public enum VideoPlugingOrder
{
    OpenVideoFromURL,
    OpenVideoFromURLDone,
    PauseVideo,
    PlayVideo,
    StopVideo,
    IsMuteVideo,
    
    SeekVideo,
    SetVolumeVideo,
    SendCurrentTimeVideo,
    GetCurrentTimeVideo,
    SendVideoPlayerInfo,
    GetVideoPlayerInfo,
    SendVideoTexture,
    GetVideoTexture,
    IsLoopVideo,
    /// <summary>
    /// 当视频准备完毕时触发
    /// </summary>
    FirstFrameReady,
    /// <summary>
    /// 当准备去播放时触发
    /// </summary>
    ReadyToPlay,
    /// <summary>
    /// 当开始播放时触发
    /// </summary>
    Started,
    /// <summary>
    /// 当视频播放完毕时触发
    /// </summary>
    FinishedPlaying,

    //AvPro_OpenVideoFromURL,
    //AvPro_OpenVideoFromURLDone,
    //AvPro_PauseVideo,
    //AvPro_PlayVideo,
    //AvPro_StopVideo,
    //AvPro_IsMuteVideo,
    //AvPro_SeekVideo,
    //AvPro_SetVolumeVideo,
    //AvPro_SendDurationVideo,
    //AvPro_GetDurationVideo,
    //AvPro_SendCurrentTimeVideo,
    //AvPro_GetCurrentTimeVideo,
    //AvPro_SendVideoIsPlay,
    //AvPro_GetVideoIsPlay,

}

public enum VoiceDispMessageType
{
    GmemMicOn,
    GmeMicOff,
    GmeMicEnalbe,
    GmeMicDisable,
    ConnectVoiceExRoom,
    ExitVoiceRoom,
    OnStreamingSpeechComplete,
    OnStreamingRecisRunning,
    VoiceStartRecoding,
    VoiceStopRecoding,
    SetVRVoiceRange

}

public enum VRPointObjEventType
{
    VRPointEnter,
    VRPointExit,
    VRPointClick,
}

public enum HandModelType
{
    HandModelAll,
    HandModelLeft,
    HandModelRight
}


public enum VRVoiceModelEventType
{
    VRVoiceReEnterRoomEvent,
    VRVoiceExitEvent,
    VRVoiceInitEvent,

    VRVoiceSetMicVolEvent,
    VRVoiceSetMicMultEvent,
    VRVoiceSetMicMulttempEvent,
    
    VRVoiceSetSpeakMultEvent,
    VRVoiceSetSpeakMulttempEvent,
    VRVoiceSetSpeakVolEvent,


}

public enum CommonVREventType
{
    None,
    /// <summary>
    /// Oculus 专用
    /// </summary>
    Any,
    /// <summary>
    /// ResetAll
    /// </summary>
    VRrecenter,
    /// <summary>
    /// 手柄振动
    /// </summary>
    ControllerVibration,
    /// <summary>
    /// 点击（左手柄或右手柄的扳机键触发）
    /// </summary>
    VRCommitButtonClick,
    /// <summary>
    /// 右手柄的扳机键触发
    /// </summary>
    VRRaw_RightTrigger,
    /// <summary>
    /// 左手柄的扳机键触发
    /// </summary>
    VRRaw_LeftTrigger,
    /// <summary>
    /// 菜单键（左手柄或右手柄的扳机键触发）
    /// </summary>
    VRRaw_Start_ButtonClick,
    /// <summary>
    /// 左手柄X键抬起
    /// </summary>
    VRRaw_X_ButtonClick,
    /// <summary>
    /// 左手柄Y键抬起
    /// </summary>
    VRRaw_Y_ButtonClick,
    /// <summary>
    /// 右手柄A键抬起
    /// </summary>
    VRRaw_A_ButtonUp,
    /// <summary>
    /// 右手柄B键抬起
    /// </summary>
    VRRaw_B_ButtonClick,
    /// <summary>
    /// 右手柄摇杆键抬起
    /// </summary>
    VR_Right_StickClick,
    /// <summary>
    /// 左手柄摇杆键抬起
    /// </summary>
    VR_Left_StickClick,
    /// <summary>
    /// 左手柄抓握键抬起
    /// </summary>
    VR_Left_GrabUp,
    /// <summary>
    /// 左手柄抓握键按下
    /// </summary>
    VR_Left_GrabDown,
    /// <summary>
    /// 右手柄抓握键抬起
    /// </summary>
    VR_Right_GrabUp,
    /// <summary>
    /// 右手柄抓握键按下
    /// </summary>
    VR_Right_GrabDown,
    /// <summary>
    /// 左摇杆方向键左
    /// </summary>
    VR_LLeft,
    /// <summary>
    /// 左摇杆方向键右
    /// </summary>
    VR_LRight,
    /// <summary>
    /// 左摇杆方向键上
    /// </summary>
    VR_LUp,
    /// <summary>
    /// 左摇杆方向键下
    /// </summary>
    VR_LDown,
    /// <summary>
    /// 左摇杆方向键还原
    /// </summary>
    VR_LRelease,
    /// <summary>
    /// 左摇杆方向键左
    /// </summary>
    VR_RLeft,
    /// <summary>
    /// 右摇杆方向键右
    /// </summary>
    VR_RRight,
    /// <summary>
    /// 右摇杆方向键上
    /// </summary>
    VR_RUp,
    /// <summary>
    /// 右摇杆方向键下
    /// </summary>
    VR_RDown,
    /// <summary>
    /// 右摇杆方向键还原
    /// </summary>
    VR_RRelease,
    /// <summary>
    /// 左摇杆坐标轴数据（Vector2）
    /// </summary>
    VR_LefStickAxis,
    /// <summary>
    /// 右摇杆坐标轴数据（Vector2）
    /// </summary>
    VR_RightStickAxis,
    /// <summary>
    /// 左扳机键坐标轴数据（float）
    /// </summary>
    VR_LeftTriggerAxis,
    /// <summary>
    /// 右扳机键坐标轴数据（float）
    /// </summary>
    VR_RightTriggerAxis,
    /// <summary>
    /// 左抓握键坐标轴数据（float）
    /// </summary>
    VR_LeftGrabAxis,
    /// <summary>
    /// 右抓握键坐标轴数据（float）
    /// </summary>
    VR_RightGrabAxis,
    /// <summary>
    /// 带上头显（某些vr无效）
    /// </summary>
    HMDMounted,
    /// <summary>
    /// 摘下头显（某些vr无效）
    /// </summary>
    HMDUnmounted,
    /// <summary>
    /// 右手柄A键按下
    /// </summary>
    VRRaw_A_ButtonDown,
    /// <summary>
    /// 左手柄扳机键按下
    /// </summary>
    VR_LeftTriggerDown,
    /// <summary>
    /// 右手柄扳机键按下
    /// </summary>
    VR_RightTriggerDown,
    /// <summary>
    /// 右手柄B键按下
    /// </summary>
    VRRaw_B_ButtonDown,
    /// <summary>
    /// 左手柄X键按下
    /// </summary>
    VRRaw_X_ButtonDown,
    /// <summary>
    /// 左手柄Y键按下
    /// </summary>
    VRRaw_Y_ButtonDown,
    
}

public enum CloudRenderMessageType
{
    /// <summary>
    /// 设置云渲染退出时发送的json数据（发送数据类型：string）
    /// </summary>
    CloudRender_SetExitData,
    /// <summary>
    /// 设置云渲染发送开关声网功能（发送数据类型：bool）
    /// </summary>
    CloudRender_SetOpenAgora,
    /// <summary>
    /// 设置云渲染发送开关摇杆功能（发送数据类型：bool）
    /// </summary>
    CloudRender_SetOpenOrCloseJoystick,
}