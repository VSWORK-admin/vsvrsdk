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
    //��ҳ���õ�
    public Transform webviewroot;
    //������ҳ���õ�����
    public Transform pagecontainer;
    
    public string ex_paramter;
}

public enum WebviewLoadType
{
    EmbeddedWeb,  //��Ƕ��ҳ����� ֧����3d�ռ�չʾ
    NativeWeb,      //ԭ����ҳ����� 2d
}
