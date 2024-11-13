using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRSetVersionController : MonoBehaviour
{
    public string versionset;
    public string appnameset;
    public static string nowAppname;
    public static string nowVersion;

    public Texture2D splashrawimage;
    public Sprite splashimage;

    public Texture2D vricon;
    public bool ServerInputEnabled;
    public bool RegAndFgEnabled;
    public TextAsset gmemodle;

    private static VRSetVersionController instance;
    public static VRSetVersionController I
    {
        get
        {
            return instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        string version = versionset;
        nowAppname = appnameset;
        nowVersion = version;
    }

}
