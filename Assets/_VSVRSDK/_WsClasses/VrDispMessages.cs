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
    //移动到Mesh物体
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
    VRLoadAvatarDone
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
    Any,
    VRrecenter,
    ControllerVibration,
    VRCommitButtonClick,
    VRRaw_RightTrigger,
    VRRaw_LeftTrigger,
    VRRaw_Start_ButtonClick,
    VRRaw_X_ButtonClick,
    VRRaw_Y_ButtonClick,
    VRRaw_A_ButtonUp,
    VRRaw_B_ButtonClick,
    VR_Right_StickClick,
    VR_Left_StickClick,
    VR_Left_GrabUp,
    VR_Left_GrabDown,
    VR_Right_GrabUp,
    VR_Right_GrabDown,
    VR_LLeft,
    VR_LRight,
    VR_LUp,
    VR_LDown,
    VR_LRelease,
    VR_RLeft,
    VR_RRight,
    VR_RUp,
    VR_RDown,
    VR_RRelease,
    VR_LefStickAxis,
    VR_RightStickAxis,
    VR_LeftTriggerAxis,
    VR_RightTriggerAxis,
    VR_LeftGrabAxis,
    VR_RightGrabAxis,
    HMDMounted,
    HMDUnmounted,
    VRRaw_A_ButtonDown,
    VR_LeftTriggerDown,
    VR_RightTriggerDown,
    VRRaw_B_ButtonDown,
    VRRaw_X_ButtonDown,
    VRRaw_Y_ButtonDown,
    
}
