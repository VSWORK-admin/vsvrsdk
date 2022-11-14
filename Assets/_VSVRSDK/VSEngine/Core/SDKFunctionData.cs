using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSEngine
{
    /// <summary>
    /// Avatar数据
    /// </summary>
    public class AvatarData
    {
        public int sort;
        public List<string> AllAvatars;
        public List<string> AllAvatarNames;
        public List<string> ActiveAvatars;
        public List<string> ActiveNickNames;
    }
    /// <summary>
    /// 最新链接频道房间数据
    /// </summary>
    public class LastIDLinkChanelRoomData
    {
        public List<string> RoomIds;
        public List<string> VoiceIds;
    }
    /// <summary>
    /// 文件请求回调
    /// </summary>
    public class LoadFileEvent
    {
        public System.Action<string,string,string> GetPathSucc;
        public System.Action GetPathFail;
    }
    /// <summary>
    /// 场景加载回调
    /// </summary>
    public class LoadSceneEventType
    {
        /// <summary>
        /// bool 为false表示开始下载场景，true为开始加载场景
        /// </summary>
        public Action<bool, VRWsRemoteScene> LoadingSceneStartEvent;
        /// <summary>
        /// 加载场景结束
        /// </summary>
        public Action<VRWsRemoteScene> LoadingSceneEndEvent;
        /// <summary>
        /// 加载场景失败
        /// </summary>
        public Action<VRWsRemoteScene> LoadingSceneFaildEvent;
        /// <summary>
        /// 加载场景成功
        /// </summary>
        public Action<VRWsRemoteScene> LoadingSceneSuccEvent;
        /// <summary>
        /// 加载进度
        /// </summary>
        public Action<WsProgressInfo> RecieveProgressEvent;
    }
}