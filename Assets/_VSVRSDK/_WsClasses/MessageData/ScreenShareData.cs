using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//当前单个分享数据
[Serializable]
public class ScreenShareData
{
    public Transform panelroot;
    public Transform screenpanel;  //分享画面面板
    public string shareuserid;
    public string shareroomid;
    public int sharetype;
    public string defaultpanel;  //默认显示在哪个面板
    public float sharestarttime;    //分享时间点
    public bool bsharing;  //正在分享
    public bool bwebshare;
}

//分享展示面板数据
[Serializable]
public class ScreenSharePanelData
{
    public string screenname;
    public int screenwidth;
    public int screenheight;
    public string shareuserid;
    public int sharetype;
    public bool btaken;  //被占用
    public bool bbigscreen;  //场景大屏
}

//用户分享通知
[Serializable]
public class UserScreenShareReqData
{
    public string shareuserid;
    public bool bshare;
}
[Serializable]
public class UserScreenShareReqExData
{
    public string shareuserid;
    public bool bshare;
    public int sharetype;
}

//指定屏幕显示某人分享画面
[Serializable]
public class SetupScreenShareView
{
    public string shareuserid;
    public string panelname;
    public bool bforcechange;
    public bool bshow;
}
//当前单个分享数据
[Serializable]
public class SceneVersionData
{ /// <summary>
  /// 
  /// </summary>
    public string id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string vr_scenes_version { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int type_num { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int prog_type { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int pdb_version { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string pdb_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string pdb_time { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int dll_version { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string dll_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string dll_time { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int scenes_version { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string vr_scenes_id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string create_at { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string update_at { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int _version { get; set; }
    /// <summary>
    /// 东莞CIO1.3.0
    /// </summary>
    public string notes { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string changed_by { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string json_url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string is_del { get; set; }
    public int vr_scenes_type { get; set; }
    public string vr_scenes_typestr { get; set; }
}

