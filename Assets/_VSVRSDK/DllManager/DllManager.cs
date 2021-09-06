using ILRuntime.Runtime.Enviorment;
using ILRuntimeAdapter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Video;
using com.ootii.Messages;
using UnityEngine.Timeline;
using LitJson;
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
        MessageDispatcher.SendMessage("GeneralDllBehaviorAwake");
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

        RegisterVRFunction(appdomain.DelegateManager);

        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();

        appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
        {
            return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
            {
                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
            });
        });
        ///litjson
        JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(obj.ToString()));
        JsonMapper.RegisterImporter<string, float>(input => float.Parse(input));

        JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);

        //messagedispach
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
       
       
        appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback<float>>((act) =>
        {
            return new DG.Tweening.TweenCallback<float>((a) =>
            {
                ((Action<float>)act)(a);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback<int>>((act) =>
        {
            return new DG.Tweening.TweenCallback<int>((a) =>
            {
                ((Action<int>)act)(a);
            });
        });

        //Dotween Setter
        appdomain.DelegateManager.RegisterFunctionDelegate<Color>();
        appdomain.DelegateManager.RegisterMethodDelegate<Color>();

        appdomain.DelegateManager.RegisterFunctionDelegate<Vector3>();
        appdomain.DelegateManager.RegisterMethodDelegate<Vector3>();
        appdomain.DelegateManager.RegisterFunctionDelegate<float>();

        appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOGetter<Color>>((act) =>
        {
            return new DG.Tweening.Core.DOGetter<Color>(() =>
            {
                return ((Func<Color>)act)();
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOGetter<float>>((act) =>
        {
            return new DG.Tweening.Core.DOGetter<float>(() =>
            {
                return ((Func<float>)act)();
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOGetter<Vector3>>((act) =>
        {
            return new DG.Tweening.Core.DOGetter<Vector3>(() =>
            {
                return ((Func<Vector3>)act)();
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<Color>>((act) =>
        {
            return new DG.Tweening.Core.DOSetter<Color>((pNewValue) =>
            {
                ((Action<Color>)act)(pNewValue);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<float>>((act) =>
        {
            return new DG.Tweening.Core.DOSetter<float>((pNewValue) =>
            {
                ((Action<float>)act)(pNewValue);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<Vector3>>((act) =>
        {
            return new DG.Tweening.Core.DOSetter<UnityEngine.Vector3>((pNewValue) =>
            {
                ((Action<UnityEngine.Vector3>)act)(pNewValue);
            });
        });





        appdomain.DelegateManager.RegisterMethodDelegate<Vector2>();
        appdomain.DelegateManager.RegisterMethodDelegate<Tap>();
        appdomain.DelegateManager.RegisterMethodDelegate<ChargedInfo>();
        appdomain.DelegateManager.RegisterMethodDelegate<DragInfo>();
        appdomain.DelegateManager.RegisterMethodDelegate<SwipeInfo>();
        appdomain.DelegateManager.RegisterMethodDelegate<PinchInfo>();
        appdomain.DelegateManager.RegisterMethodDelegate<RotateInfo>();

        appdomain.DelegateManager.RegisterMethodDelegate<int>();
        appdomain.DelegateManager.RegisterMethodDelegate<int, string>();
        appdomain.DelegateManager.RegisterMethodDelegate<bool, int, string>();
        appdomain.DelegateManager.RegisterMethodDelegate<int, float, int>();
        appdomain.DelegateManager.RegisterMethodDelegate<int, string, bool>();
        appdomain.DelegateManager.RegisterMethodDelegate<int, bool, string>();
        appdomain.DelegateManager.RegisterMethodDelegate<int, string, string>();
        appdomain.DelegateManager.RegisterMethodDelegate<int, string, int>();
        appdomain.DelegateManager.RegisterMethodDelegate<int, string, string, int>();
        appdomain.DelegateManager.RegisterMethodDelegate<int, string, string, string>();
        appdomain.DelegateManager.RegisterMethodDelegate<int, string, string, string, bool>();
        appdomain.DelegateManager.RegisterMethodDelegate<BaseEventData>();
        appdomain.DelegateManager.RegisterMethodDelegate<VideoPlayer>();
        appdomain.DelegateManager.RegisterMethodDelegate<VideoPlayer,long>();
        appdomain.DelegateManager.RegisterMethodDelegate<VideoPlayer,string>();
        appdomain.DelegateManager.RegisterMethodDelegate<VideoPlayer,double>();
        appdomain.DelegateManager.RegisterMethodDelegate<Texture2D>();
        appdomain.DelegateManager.RegisterMethodDelegate<int,int>();
        appdomain.DelegateManager.RegisterMethodDelegate<float>();
        appdomain.DelegateManager.RegisterMethodDelegate<object,EventArgs>();
        appdomain.DelegateManager.RegisterMethodDelegate<GameObject, AnimationClip[],Transform,string>();

        appdomain.DelegateManager.RegisterMethodDelegate<Slate.Cutscene>();
        appdomain.DelegateManager.RegisterMethodDelegate<Slate.Section>();
        appdomain.DelegateManager.RegisterMethodDelegate<string, object>();
        appdomain.DelegateManager.RegisterMethodDelegate<string>();
        appdomain.DelegateManager.RegisterMethodDelegate<object>();
        



        appdomain.DelegateManager.RegisterDelegateConvertor<VideoPlayer.EventHandler>((action) =>
        {
            return new VideoPlayer.EventHandler((a) =>
            {
                ((Action<VideoPlayer>)action)(a);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<VideoPlayer.FrameReadyEventHandler>((action) =>
        {
            return new VideoPlayer.FrameReadyEventHandler((a,b) =>
            {
                ((Action<VideoPlayer,long>)action)(a,b);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<VideoPlayer.ErrorEventHandler>((action) =>
        {
            return new VideoPlayer.ErrorEventHandler((a,b) =>
            {
                ((Action<VideoPlayer,string>)action)(a,b);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<VideoPlayer.TimeEventHandler>((action) =>
        {
            return new VideoPlayer.TimeEventHandler((a,b) =>
            {
                ((Action<VideoPlayer,double>)action)(a,b);
            });
        });

        appdomain.DelegateManager.RegisterMethodDelegate<System.Object, ILRuntime.Runtime.Intepreter.ILTypeInstance>();

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<BaseEventData>>((action) =>
        {
            return new UnityAction<BaseEventData>((a) =>
            {
                ((System.Action<BaseEventData>)action)(a);
            });
        });


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


        appdomain.DelegateManager.RegisterMethodDelegate<bool>();
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<bool>>((act) =>
        {
            return new UnityAction<bool>((arg0) =>
            {
                ((Action<bool>)act)(arg0);
            });
        });

        appdomain.DelegateManager.RegisterMethodDelegate<Single>();

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<Single>>((act) =>
        {
            return new UnityAction<Single>((arg0) =>
            {
                ((Action<Single>)act)(arg0);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<string>>((act) =>
        {
            return new UnityAction<string>((arg0) =>
            {
                ((Action<string>)act)(arg0);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<float>>((act) =>
        {
            return new UnityAction<float>((arg0) =>
            {
                ((Action<float>)act)(arg0);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<int>>((act) =>
        {
            return new UnityAction<int>((arg0) =>
            {
                ((Action<int>)act)(arg0);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<Vector2>>((act) =>
        {
            return new UnityAction<Vector2>((arg0) =>
            {
                ((Action<Vector2>)act)(arg0);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<Vector3>>((act) =>
        {
            return new UnityAction<Vector3>((arg0) =>
            {
                ((Action<Vector3>)act)(arg0);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<Color>>((act) =>
        {
            return new UnityAction<Color>((arg0) =>
            {
                ((Action<Color>)act)(arg0);
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

        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, int>();

        appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
        {
            return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
            {
                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, int>)act)(x, y);
            });
        });


        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, String>();
        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, float>();
        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, int>();
        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, Vector3>();
        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, Vector2>();
        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, Color>();
        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, bool>();

        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>();
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>((arg0, arg1) =>
            {
                ((Action<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>)act)(arg0, arg1);
            });
        });

        appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.String, UnityEngine.LogType>();
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Application.LogCallback>((act) =>
        {
            return new UnityEngine.Application.LogCallback((condition, stackTrace, type) =>
            {
                ((Action<System.String, System.String, UnityEngine.LogType>)act)(condition, stackTrace, type);
            });
        });
    }

    private static void RegisterVRFunction(ILRuntime.Runtime.Enviorment.DelegateManager delegateManager)
    {
        delegateManager.RegisterMethodDelegate<string, float, Vector2>();
        delegateManager.RegisterMethodDelegate<string, GameObject>();
        delegateManager.RegisterMethodDelegate<string, string, string>();
        //delegateManager.RegisterMethodDelegate<string>();
        delegateManager.RegisterMethodDelegate<WsPlaceMarkList>();
        //delegateManager.RegisterMethodDelegate<bool>();
        delegateManager.RegisterMethodDelegate<GameObject>();
        delegateManager.RegisterMethodDelegate<Texture2D>();
        delegateManager.RegisterMethodDelegate<GameObject, List<string>>();
        //delegateManager.RegisterMethodDelegate<KodFileResult>();
        delegateManager.RegisterMethodDelegate<bool, VRWsRemoteScene>();
        delegateManager.RegisterMethodDelegate<VRWsRemoteScene>();
        delegateManager.RegisterMethodDelegate<WsProgressInfo>();
        delegateManager.RegisterMethodDelegate<string, Texture2D>();
        delegateManager.RegisterMethodDelegate<Texture>();
        delegateManager.RegisterMethodDelegate<WsChangeInfo>();
        delegateManager.RegisterMethodDelegate<WsCChangeInfo>();
        delegateManager.RegisterMethodDelegate<VRChanelRoom>();
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
        StopAllCoroutines();
    }
}
