﻿using System.Collections;
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
    RecieveAlist,
    RecieveCheckAvatar
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
    KODGetOneGlb,
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
    ChangePipeLine
}


public enum GMEDispMessageType
{

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