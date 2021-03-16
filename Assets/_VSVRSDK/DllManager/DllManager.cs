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

        appdomain.DelegateManager.RegisterMethodDelegate<Vector2>();
        appdomain.DelegateManager.RegisterMethodDelegate<Tap>();
        appdomain.DelegateManager.RegisterMethodDelegate<ChargedInfo>();
        appdomain.DelegateManager.RegisterMethodDelegate<DragInfo>();
        appdomain.DelegateManager.RegisterMethodDelegate<SwipeInfo>();
        appdomain.DelegateManager.RegisterMethodDelegate<PinchInfo>();
        appdomain.DelegateManager.RegisterMethodDelegate<RotateInfo>();


        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.ShortTapHandler>((act) =>
        {
            return new IT_Gesture.ShortTapHandler((tap) =>
            {
                ((Action<Vector2>)act)(tap);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
        {
            return new UnityEngine.Events.UnityAction(() =>
            {
                ((Action)act)();
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.DoubleTapHandler>((act) =>
        {
            return new IT_Gesture.DoubleTapHandler((tap) =>
            {
                ((Action<Vector2>)act)(tap);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.MultiTapHandler>((act) =>
        {
            return new IT_Gesture.MultiTapHandler((tap) =>
            {
                ((Action<Tap>)act)(tap);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.LongTapHandler>((act) =>
        {
            return new IT_Gesture.LongTapHandler((tap) =>
            {
                ((Action<Tap>)act)(tap);
            });
        });



        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.ChargeStartHandler>((act) =>
        {
            return new IT_Gesture.ChargeStartHandler((cInfo) =>
            {
                ((Action<ChargedInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.ChargingHandler>((act) =>
        {
            return new IT_Gesture.ChargingHandler((cInfo) =>
            {
                ((Action<ChargedInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.ChargeEndHandler>((act) =>
        {
            return new IT_Gesture.ChargeEndHandler((cInfo) =>
            {
                ((Action<ChargedInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.DraggingStartHandler>((act) =>
        {
            return new IT_Gesture.DraggingStartHandler((cInfo) =>
            {
                ((Action<DragInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.DraggingHandler>((act) =>
        {
            return new IT_Gesture.DraggingHandler((cInfo) =>
            {
                ((Action<DragInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.DraggingEndHandler>((act) =>
        {
            return new IT_Gesture.DraggingEndHandler((cInfo) =>
            {
                ((Action<DragInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.MFMultiTapHandler>((act) =>
        {
            return new IT_Gesture.MFMultiTapHandler((cInfo) =>
            {
                ((Action<Tap>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.MFLongTapHandler>((act) =>
        {
            return new IT_Gesture.MFLongTapHandler((cInfo) =>
            {
                ((Action<Tap>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.MFChargeStartHandler>((act) =>
        {
            return new IT_Gesture.MFChargeStartHandler((cInfo) =>
            {
                ((Action<ChargedInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.MFChargingHandler>((act) =>
        {
            return new IT_Gesture.MFChargingHandler((cInfo) =>
            {
                ((Action<ChargedInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.MFChargeEndHandler>((act) =>
        {
            return new IT_Gesture.MFChargeEndHandler((cInfo) =>
            {
                ((Action<ChargedInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.MFDraggingStartHandler>((act) =>
        {
            return new IT_Gesture.MFDraggingStartHandler((cInfo) =>
            {
                ((Action<DragInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.MFDraggingHandler>((act) =>
        {
            return new IT_Gesture.MFDraggingHandler((cInfo) =>
            {
                ((Action<DragInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.MFDraggingEndHandler>((act) =>
        {
            return new IT_Gesture.MFDraggingEndHandler((cInfo) =>
            {
                ((Action<DragInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.SwipeStartHandler>((act) =>
        {
            return new IT_Gesture.SwipeStartHandler((cInfo) =>
            {
                ((Action<SwipeInfo>)act)(cInfo);
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.SwipingHandler>((act) =>
        {
            return new IT_Gesture.SwipingHandler((cInfo) =>
            {
                ((Action<SwipeInfo>)act)(cInfo);
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.SwipeEndHandler>((act) =>
        {
            return new IT_Gesture.SwipeEndHandler((cInfo) =>
            {
                ((Action<SwipeInfo>)act)(cInfo);
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.SwipeHandler>((act) =>
        {
            return new IT_Gesture.SwipeHandler((cInfo) =>
            {
                ((Action<SwipeInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.PinchHandler>((act) =>
        {
            return new IT_Gesture.PinchHandler((cInfo) =>
            {
                ((Action<PinchInfo>)act)(cInfo);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<IT_Gesture.RotateHandler>((act) =>
        {
            return new IT_Gesture.RotateHandler((cInfo) =>
            {
                ((Action<RotateInfo>)act)(cInfo);
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
