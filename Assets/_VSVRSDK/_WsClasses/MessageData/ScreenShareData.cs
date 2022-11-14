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

//指定屏幕显示某人分享画面
[Serializable]
public class SetupScreenShareView
{
    public string shareuserid;
    public string panelname;
    public bool bforcechange;
    public bool bshow;
}


