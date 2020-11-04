using ILRuntime.Runtime.Enviorment;
using ILRuntimeAdapter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DllManager : MonoBehaviour
{
    public static ILRuntime.Runtime.Enviorment.AppDomain appdomain;

    public TextAsset DllAsset;
    public TextAsset PdbAsset;
    public static DllManager Instance
    {
        get { return _Instance; }
    }
    private static DllManager _Instance = null;

    System.IO.MemoryStream fs;
    System.IO.MemoryStream p;

    public event System.Action OnAssemblyLoadOver = null;

    public ExtralData[] ExtralDatas;

    private void Awake()
    {
        _Instance = this;
    }

    void Start()
    {
        LoadILAssembly();
    }

    public void LoadILAssembly()
    {
        LoadHotFixAssembly2(DllAsset.bytes, PdbAsset.bytes);
    }

    void LoadHotFixAssembly2(byte[] dll, byte[] pdb)
    {
        appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();

        fs = new MemoryStream(dll);
        p = new MemoryStream(pdb);
        try
        {
            appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        }
        catch
        {
            Debug.LogError("加载热更DLL失败，请确保已经通过VS打开Assets/Samples/ILRuntime/1.6/Demo/HotFix_Project/HotFix_Project.sln编译过热更DLL");
        }

        InitializeILRuntime();

        OnDllLoaded();

        if (OnAssemblyLoadOver != null)
        {
            OnAssemblyLoadOver();
        }
    }

    void InitializeILRuntime()
    {
#if DEBUG && UNITY_EDITOR
        appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
        //这里做一些ILRuntime的注册，如委托适配器，值类型绑定等等

        appdomain.AllowUnboundCLRMethod = true;

        ILRuntime.Runtime.Generated.DllCLRBindings.Initialize(appdomain);

        RegisterAdapter();
		
		RegisterDelegate();
    }

    unsafe void OnDllLoaded()
    {
        appdomain.Invoke("Dll_Project.DllMain", "Main", null, null);
    }

    private void RegisterAdapter()
    {
        appdomain.RegisterCrossBindingAdaptor(new DllGenerateBaseAdapter());
        appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
        appdomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
    }
	
	private void RegisterDelegate()
    {
        appdomain.DelegateManager.RegisterMethodDelegate<com.ootii.Messages.IMessage>();

        appdomain.DelegateManager.RegisterDelegateConvertor<com.ootii.Messages.MessageHandler>((act) =>
        {
            return new com.ootii.Messages.MessageHandler((rMessage) =>
            {
                ((Action<com.ootii.Messages.IMessage>)act)(rMessage);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
        {
            return new DG.Tweening.TweenCallback(() =>
            {
                ((Action)act)();
            });
        });
    }

    private void OnDestroy()
    {
        if (fs != null)
            fs.Close();
        if (p != null)
            p.Close();
        fs = null;
        p = null;

        appdomain = null;
        _Instance = null;
    }
}
