using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using System;
using UnityEngine.UI;

public class ScreenSharePanel : MonoBehaviour
{
    public ScreenSharePanelData configdata = new ScreenSharePanelData();

    //分享画面展示
    public RawImage rawImage = null;

    public static Action<ScreenSharePanel, int> RegisterScreenSharePanel;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (string.IsNullOrEmpty(configdata.screenname))
            configdata.screenname = gameObject.name;

        if (RegisterScreenSharePanel != null)
            RegisterScreenSharePanel(this, 1);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool CheckScreenShareOn()
    {
        return !string.IsNullOrEmpty(configdata.shareuserid);
    }

    private void OnDestroy()
    {
        if (RegisterScreenSharePanel != null)
            RegisterScreenSharePanel(this, 0);
    }
}
