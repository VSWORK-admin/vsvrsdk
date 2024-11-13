using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WebViewParamterData
{
    public bool sync;
    public bool adminonly;
    public string url;
    public bool displayUI;
    public bool bfullscreen;
    public bool showtoolbar;
    public int width;
    public int height;
    public Vector2 offset;
}
[Serializable]
public class WebViewParamterDataEx : WebViewParamterData
{
    public WebviewLoadType loadtype;
    //网页放置点
    public Transform webviewroot;
    //单个网页放置的容器
    public Transform pagecontainer;
    
    public string ex_paramter;
}

public enum WebviewLoadType
{
    EmbeddedWeb,  //内嵌网页浏览器 支持在3d空间展示
    NativeWeb,      //原生网页浏览器 2d
}
