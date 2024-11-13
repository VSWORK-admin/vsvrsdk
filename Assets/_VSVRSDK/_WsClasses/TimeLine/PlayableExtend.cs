using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;


public class PlayableExtend
{
    /// <summary>
    /// 获取绑定的轨道
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="playableDirector"></param>
    /// <param name="trackname"></param>
    /// <returns></returns>
    public static PlayableBinding GetPlayableBinding(PlayableDirector playableDirector, string trackname)
    {
        foreach (PlayableBinding pb in playableDirector.playableAsset.outputs)//每次都用循环获取 这里性能可能会有影响
        {
            if (pb.streamName.Equals(trackname))
            {
                return pb;
            }
        }
        throw new NullReferenceException($"未找到对应的[{trackname}]名称!");
    }

    /// <summary>
    /// 一次性获取所有的轨道信息
    ///  
    /// </summary>
    /// <param name="playableDirector"></param>
    /// <returns></returns>
    public static Dictionary<string, PlayableBinding> GetPlayableBindings(PlayableDirector playableDirector)
    {
        Dictionary<string, PlayableBinding> bindingDict = new Dictionary<string, PlayableBinding>();   //获取TimeLine的轨道 
        foreach (PlayableBinding pb in playableDirector.playableAsset.outputs)
        {
            if (!bindingDict.ContainsKey(pb.streamName))
            {
                bindingDict.Add(pb.streamName, pb);
            }
        }
        return bindingDict;
    }

    /// <summary>
    /// 设置对应的轨道mute
    /// </summary>
    /// <param name="playableDirector"></param>
    /// <param name="trackname"></param>
    /// <param name="value"></param>
    public static void SetTackMute(PlayableDirector playableDirector, string trackname, bool value)
    {
        var ass = GetPlayableBinding(playableDirector, trackname).sourceObject as TrackAsset;
        ass.muted = value;
    }

    /// <summary>
    /// 设置对应的轨道mute，并重新绘制
    /// 在PlayableBehaviour下 会没有效果并且报错
    /// </summary>
    /// <param name="playableDirector"></param>
    /// <param name="trackname"></param>
    /// <param name="value"></param>
    public static void SetTackMuteRebuildGraph(PlayableDirector playableDirector, string trackname, bool value)
    {
        var ass = GetPlayableBinding(playableDirector, trackname).sourceObject as TrackAsset;
        ass.muted = value;

        double t0 = playableDirector.time;
        playableDirector.RebuildGraph();
        playableDirector.time = t0;
        playableDirector.Play();
    }

    /// <summary>
    /// 获取轨道对应轨道类型
    /// 比如 获取轨道中 AnimationTrackAsset 对其操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="playableDirector"></param>
    /// <param name="trackName"></param>
    /// <returns></returns>
    public static TrackAsset GetTrackAsset(PlayableDirector playableDirector, string trackName)
    {
        return GetPlayableBinding(playableDirector, trackName).sourceObject as TrackAsset;
    }

    /// <summary>
    /// 绑定对应的物体
    /// 比如说重新绑定 animatortrack 中的对象
    /// 传入 轨道名称 和对象对应的动画器(Animator)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="playableDirector"></param>
    /// <param name="trackName"></param>
    /// <param name="_object"></param>
    public static void SetBind(PlayableDirector playableDirector, string trackName, Animator _object)
    {
        playableDirector.SetGenericBinding(GetPlayableBinding(playableDirector, trackName).sourceObject, _object);
    }
}
