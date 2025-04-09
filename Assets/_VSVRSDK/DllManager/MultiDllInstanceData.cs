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
using System.Linq;
using VSWorkSDK;
using VSWorkSDK.Data;

public class MultiDllInstanceData
{
    public string DllName = string.Empty;
    /// <summary>
    /// 调试端口号
    /// </summary>
    public int DebugPort = 56002;

    internal ILRuntime.Runtime.Enviorment.AppDomain appdomain;

    public TextAsset DllAsset;
    public TextAsset PdbAsset;
    public bool bBindScene { get { return DllAsset != null && PdbAsset != null; } }

    internal System.IO.MemoryStream fs;
    internal System.IO.MemoryStream p;

    public event System.Action OnAssemblyLoadOver = null;

    public ExtralData[] ExtralDatas;

    public byte[] OnlineDll = null;
    public byte[] OnlinePdb = null;
    public bool bOnlineMode { get { return OnlineDll != null && OnlinePdb != null; } }

    public byte[] TestDll = null;
    public byte[] TestPdb = null;

    public bool bTestMode { get { return TestDll != null && TestPdb != null; } }

    public bool bDefaultCallMain = true;

    private int _runType = 0;
    public int RunType { get { return _runType; } }

    internal int LoadedMode = 1;
    public MultiDllInstanceData()
    {
    }
    public MultiDllInstanceData(string dllName)
    {
        DllName = dllName;
    }
    public MultiDllInstanceData(string dllName, byte[] OnlineDll, byte[] OnlinePdb)
    {
        DllName = dllName;
        this.OnlineDll = OnlineDll;
        this.OnlinePdb = OnlinePdb;
    }
    internal void Awake()
    {

    }

    internal void Start()
    {
#if ILHotFix
        if (bTestMode)
        {
            OnTestModeLoaded();
            return;
        }
        if (bOnlineMode)
        {
            OnDllLoaded();
            if (OnAssemblyLoadOver != null)
            {
                OnAssemblyLoadOver();
            }

            MessageDispatcher.SendMessageData("GeneralDllBehaviorAwake",DllName);
            return;
        }
#endif
        LoadILAssembly();
    }
    internal void OnTestModeLoaded()
    {
        OnDllLoaded();

        if (OnAssemblyLoadOver != null)
        {
            OnAssemblyLoadOver();
        }

        MessageDispatcher.SendMessageData("GeneralDllBehaviorAwake", DllName);
    }

    unsafe void OnDllLoaded()
    {
        appdomain.Invoke("Dll_Project.DllMain", "Main", null, DllName);

        //#if ILHotFix && DEBUG

        //        _runType = (int)VSVR_Debug.RtcMsgDllRunType.Running;
        //        GameManager.Instance.timerManager.doOnce(100, () => {

        //            if (VSVR_Debug.DebugManager.Instance != null)
        //            {
        //                VSVR_Debug.DebugManager.Instance.SendDllInfoMsg(DllName, DebugPort, LoadedMode, _runType);
        //            }
        //        });
        //#endif
    }

    private void LoadILAssembly()
    {
        if (!bBindScene) return;

        UnLoadAssembly();

        LoadHotFixAssembly(DllAsset.bytes, PdbAsset.bytes);
    }
    internal void LoadOnlineAssembly()
    {
        if (bOnlineMode)
        {
            LoadHotFixAssembly(OnlineDll, OnlinePdb);
        }
        else
        {
            Debug.LogError(string.Format("Can't get online dll file data or online pdb file data !"));
        }
    }
    internal void LoadTestAssembly()
    {
        if (bTestMode)
        {
            LoadHotFixAssembly(TestDll, TestPdb);
        }
        else
        {
            Debug.LogError(string.Format("Can't get test dll file data or test pdb file data !"));
        }
    }
    internal void LoadOnlineAssemblyRaw(byte[] dll, byte[] pdb)
    {
        UnLoadAssembly();

        OnlineDll = dll;
        OnlinePdb = pdb;

        LoadHotFixAssembly(dll, pdb);
    }
    internal void LoadTestAssemblyRaw(byte[] dll, byte[] pdb)
    {
        UnLoadAssembly();

        TestDll = dll;
        TestPdb = pdb;

        LoadHotFixAssembly(dll, pdb);
    }

    private void LoadHotFixAssembly(byte[] dll, byte[] pdb)
    {
        appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();

        if (bTestMode)
        {
            dll = TestDll;
            pdb = TestPdb;

#if ILHotFix
            LoadedMode = (int)VSVR_Debug.RtcMsgDllLoadType.TestMode;
#endif
        }
        else if (bOnlineMode)
        {
            dll = OnlineDll;
            pdb = OnlinePdb;

#if ILHotFix
            LoadedMode = (int)VSVR_Debug.RtcMsgDllLoadType.OnlineMode;
#endif
        }
        else
        {
#if ILHotFix
            LoadedMode = (int)VSVR_Debug.RtcMsgDllLoadType.InSceneMode;
#endif
        }

        fs = new MemoryStream(dll);
        p = new MemoryStream(pdb);
        try
        {
            appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        }
        catch
        {
            Debug.LogError("DebugLog.Exception 加载热更DLL失败，请确保已经通过VS打开Assets/Samples/ILRuntime/1.6/Demo/HotFix_Project/HotFix_Project.sln编译过热更DLL");
        }

        InitializeILRuntime();

        if (bDefaultCallMain)
        //if (!bTestMode || !bOnlineMode)
        {
            OnDllLoaded();

            if (OnAssemblyLoadOver != null)
            {
                OnAssemblyLoadOver();
            }

            MessageDispatcher.SendMessageData("GeneralDllBehaviorAwake", DllName);
        }

        //#if ILHotFix
        //        if (!VRPublishSettingController.I.bMultiInstance)
        //        {
        //            appdomain.DebugService.StartDebugService(DebugPort);
        //        }
        //#else
        //        appdomain.DebugService.StartDebugService(DebugPort);
        //#endif
    }
    internal void UnLoadAssembly()
    {
        if (bTestMode)
        {
            TestDll = null;
            TestPdb = null;
        }
        if (bOnlineMode)
        {
            OnlineDll = null;
            OnlinePdb = null;
        }
#if ILHotFix
        if (!VRPublishSettingController.I.bMultiInstance)
        {
            if (appdomain != null)
            {
                appdomain.DebugService.StopDebugService();
            }
        }
#else
        if (appdomain != null)
        {
            appdomain.DebugService.StopDebugService();
        }
#endif
        if (fs != null)
        {
            fs.Close();
            fs.Dispose();
            fs = null;
        }

        if (p != null)
        {
            p.Close();
            p.Dispose();
            p = null;
        }
        if (appdomain != null)
        {
            appdomain.Dispose();
            appdomain = null;
        }
        //#if ILHotFix && DEBUG
        //        _runType = (int)VSVR_Debug.RtcMsgDllRunType.Destory;
        //        if (VSVR_Debug.DebugManager.Instance != null)
        //        {
        //            VSVR_Debug.DebugManager.Instance.SendDllInfoMsg(DllName, DebugPort, LoadedMode, _runType);
        //        }
        //#endif
    }
    void InitializeILRuntime()
    {
#if DEBUG && UNITY_EDITOR
        appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
        //这里做一些ILRuntime的注册，如委托适配器，值类型绑定等等

        appdomain.AllowUnboundCLRMethod = true;

#if ILHotFix

#if UNITY_STANDALONE_WIN

        if(DllManager.Instance != null && DllManager.appdomain != null)
        {
            appdomain.UseShareCLR(DllManager.appdomain);
        }
        else if (MultiDllManager.Instance.AllDllInstance.Count > 0)
        {
            var item = MultiDllManager.Instance.AllDllInstance.First();

            if (item.Value != null)
            {
                appdomain.UseShareCLR(ILManager.appdomain);
            }
        }
        else
        {
            if (ILManager.Instance.bWindowsJit)
            {
                ILRuntime.Runtime.Generated.DllCLRBindings.Initialize(appdomain);
                appdomain.UseMainCLR(ILManager.appdomain);
            }
            else
            {
                ILRuntime.Runtime.Generated.DllCLRBindings.Initialize(appdomain);
                ILRuntime.Runtime.Generated.Normal_CLRBindings.Initialize(appdomain);
                ILRuntime.Runtime.Generated.System_CLRBindings.Initialize(appdomain);
                ILRuntime.Runtime.Generated.NoNamespace_GenCLRBindings.Initialize(appdomain);
                ILRuntime.Runtime.Generated.WithNamespace_CLRBindings.Initialize(appdomain);
                ILRuntime.Runtime.Generated.Modules_CLRBindings.Initialize(appdomain);
            }
        }
#else
        if(DllManager.Instance != null && DllManager.appdomain != null)
        {
            appdomain.UseShareCLR(DllManager.appdomain);
        }
        else if (MultiDllManager.Instance.AllDllInstance.Count > 0)
        {
            var item = MultiDllManager.Instance.AllDllInstance.First();

            if(item.Value != null)
            {
                appdomain.UseShareCLR(ILManager.appdomain);
            }
        }
        else
        {
            ILRuntime.Runtime.Generated.DllCLRBindings.Initialize(appdomain);
            appdomain.UseMainCLR(ILManager.appdomain);
        }
#endif

#else
        ILRuntime.Runtime.Generated.DllCLRBindings.Initialize(appdomain);
#endif
        RegisterAdapter();

        RegisterDelegate();

        //#if ILHotFix && DEBUG
        //        _runType = (int)VSVR_Debug.RtcMsgDllRunType.Loaded;
        //        if (VSVR_Debug.DebugManager.Instance != null)
        //        {
        //            VSVR_Debug.DebugManager.Instance.SendDllInfoMsg(DllName, DebugPort, LoadedMode, _runType);
        //        }
        //#endif
    }
    private void RegisterAdapter()
    {
        appdomain.RegisterCrossBindingAdaptor(new DllGenerateBaseAdapter());
        appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
        appdomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
        appdomain.RegisterCrossBindingAdaptor(new DllDragBaseAdapter());
        appdomain.RegisterCrossBindingAdaptor(new DllActionBaseAdapter());
        appdomain.RegisterCrossBindingAdaptor(new DllCompositeBaseAdapter());
        appdomain.RegisterCrossBindingAdaptor(new DllConditionalBaseAdapter());
        appdomain.RegisterCrossBindingAdaptor(new DllDecoratorBaseAdapter());
    }

    private void RegisterDelegate()
    {
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
                try
                {
                    ((Action<com.ootii.Messages.IMessage>)act)(rMessage);
                }
                catch (Exception e)
                {
                    string methodname = act != null && act.Target != null ? act.Target.ToString() : "";
                    if (rMessage != null && rMessage.Type != null)
                    {
                        Debug.LogError("DebugLog.Exception Exception : " + "rMessage.Type : " + rMessage.Type + " funtion : " + methodname + "\r\n" + e);
                    }
                    else
                    {
                        Debug.LogError("DebugLog.Exception Exception : " + " funtion : " + methodname + "\r\n" + e);
                    }
                }
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
        appdomain.DelegateManager.RegisterMethodDelegate<WebResponseData>();
        appdomain.DelegateManager.RegisterDelegateConvertor<SDKWebResponseCallback>((act) =>
        {
            return new SDKWebResponseCallback((a) =>
            {
                ((Action<WebResponseData>)act)(a);
            });
        });

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
        appdomain.DelegateManager.RegisterMethodDelegate<VideoPlayer, long>();
        appdomain.DelegateManager.RegisterMethodDelegate<VideoPlayer, string>();
        appdomain.DelegateManager.RegisterMethodDelegate<VideoPlayer, double>();
        appdomain.DelegateManager.RegisterMethodDelegate<Texture2D>();
        appdomain.DelegateManager.RegisterMethodDelegate<int, int>();
        appdomain.DelegateManager.RegisterMethodDelegate<float>();
        appdomain.DelegateManager.RegisterMethodDelegate<object, EventArgs>();
        appdomain.DelegateManager.RegisterMethodDelegate<GameObject, AnimationClip[], Transform, string>();

        appdomain.DelegateManager.RegisterMethodDelegate<Slate.Cutscene>();
        appdomain.DelegateManager.RegisterMethodDelegate<Slate.Section>();
        appdomain.DelegateManager.RegisterMethodDelegate<string, object>();
        appdomain.DelegateManager.RegisterMethodDelegate<string>();
        appdomain.DelegateManager.RegisterMethodDelegate<object>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();




        appdomain.DelegateManager.RegisterDelegateConvertor<VideoPlayer.EventHandler>((action) =>
        {
            return new VideoPlayer.EventHandler((a) =>
            {
                ((Action<VideoPlayer>)action)(a);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<VideoPlayer.FrameReadyEventHandler>((action) =>
        {
            return new VideoPlayer.FrameReadyEventHandler((a, b) =>
            {
                ((Action<VideoPlayer, long>)action)(a, b);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<VideoPlayer.ErrorEventHandler>((action) =>
        {
            return new VideoPlayer.ErrorEventHandler((a, b) =>
            {
                ((Action<VideoPlayer, string>)action)(a, b);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<VideoPlayer.TimeEventHandler>((action) =>
        {
            return new VideoPlayer.TimeEventHandler((a, b) =>
            {
                ((Action<VideoPlayer, double>)action)(a, b);
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

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction>((act) =>
        {
            return new UnityAction(() =>
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
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Texture2D>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<UnityEngine.Texture2D>((data) =>
            {
                ((Action<UnityEngine.Texture2D>)act)(data);
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

        appdomain.DelegateManager.RegisterFunctionDelegate<System.String, System.Int32, System.Char, System.Char>();
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.UI.InputField.OnValidateInput>((act) =>
        {
            return new UnityEngine.UI.InputField.OnValidateInput((text, charIndex, addedChar) =>
            {
                return ((Func<System.String, System.Int32, System.Char, System.Char>)act)(text, charIndex, addedChar);
            });
        });
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, UnityEngine.Texture2D>();
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Threading.ParameterizedThreadStart>((act) =>
        {
            return new System.Threading.ParameterizedThreadStart((obj) =>
            {
                ((Action<System.Object>)act)(obj);
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Threading.ThreadStart>((act) =>
        {
            return new System.Threading.ThreadStart(() =>
            {
                ((Action)act)();
            });
        });

        RegisterVRFunction(appdomain.DelegateManager);

        RegisterMessageFunction(appdomain.DelegateManager);

    }



    private void RegisterMessageFunction(ILRuntime.Runtime.Enviorment.DelegateManager delegateManager)
    {
        //mqtt
        delegateManager.RegisterMethodDelegate<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs>();
        delegateManager.RegisterMethodDelegate<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishedEventArgs>();
        delegateManager.RegisterMethodDelegate<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribedEventArgs>();
        delegateManager.RegisterMethodDelegate<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgUnsubscribedEventArgs>();
        delegateManager.RegisterMethodDelegate<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribeEventArgs>();
        delegateManager.RegisterMethodDelegate<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgUnsubscribeEventArgs>();
        delegateManager.RegisterMethodDelegate<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgConnectEventArgs>();
        delegateManager.RegisterMethodDelegate<object, EventArgs>();
        delegateManager.RegisterMethodDelegate<GameObject, AnimationClip[], Transform, string>();

        delegateManager.RegisterDelegateConvertor<uPLibrary.Networking.M2Mqtt.MqttClient.MqttMsgPublishEventHandler>((action) =>
        {
            return new uPLibrary.Networking.M2Mqtt.MqttClient.MqttMsgPublishEventHandler((a, b) =>
            {
                ((Action<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs>)action)(a, b);
            });
        });

        delegateManager.RegisterDelegateConvertor<uPLibrary.Networking.M2Mqtt.MqttClient.MqttMsgPublishedEventHandler>((action) =>
        {
            return new uPLibrary.Networking.M2Mqtt.MqttClient.MqttMsgPublishedEventHandler((a, b) =>
            {
                ((Action<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishedEventArgs>)action)(a, b);
            });
        });

        delegateManager.RegisterDelegateConvertor<uPLibrary.Networking.M2Mqtt.MqttClient.MqttMsgSubscribedEventHandler>((action) =>
        {
            return new uPLibrary.Networking.M2Mqtt.MqttClient.MqttMsgSubscribedEventHandler((a, b) =>
            {
                ((Action<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribedEventArgs>)action)(a, b);
            });
        });
        delegateManager.RegisterDelegateConvertor<uPLibrary.Networking.M2Mqtt.MqttClient.MqttMsgUnsubscribedEventHandler>((action) =>
        {
            return new uPLibrary.Networking.M2Mqtt.MqttClient.MqttMsgUnsubscribedEventHandler((a, b) =>
            {
                ((Action<object, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgUnsubscribedEventArgs>)action)(a, b);
            });
        });

        delegateManager.RegisterDelegateConvertor<uPLibrary.Networking.M2Mqtt.MqttClient.MqttMsgDisconnectEventHandler>((action) =>
        {
            return new uPLibrary.Networking.M2Mqtt.MqttClient.MqttMsgDisconnectEventHandler((a, b) =>
            {
                ((Action<object, EventArgs>)action)(a, b);
            });
        });


        //websocket
        delegateManager.RegisterMethodDelegate<System.Object, WebSocketSharp.MessageEventArgs>();
        delegateManager.RegisterMethodDelegate<System.Object, WebSocketSharp.ErrorEventArgs>();
        delegateManager.RegisterMethodDelegate<System.Object, WebSocketSharp.CloseEventArgs>();

        appdomain.DelegateManager.RegisterDelegateConvertor<System.EventHandler>((act) =>
        {
            return new System.EventHandler((sender, e) =>
            {
                ((Action<System.Object, System.EventArgs>)act)(sender, e);
            });
        });

        delegateManager.RegisterDelegateConvertor<System.EventHandler<WebSocketSharp.MessageEventArgs>>((act) =>
        {
            return new System.EventHandler<WebSocketSharp.MessageEventArgs>((sender, e) =>
            {
                ((Action<System.Object, WebSocketSharp.MessageEventArgs>)act)(sender, e);
            });
        });

        delegateManager.RegisterDelegateConvertor<System.EventHandler<WebSocketSharp.ErrorEventArgs>>((act) =>
        {
            return new System.EventHandler<WebSocketSharp.ErrorEventArgs>((sender, e) =>
            {
                ((Action<System.Object, WebSocketSharp.ErrorEventArgs>)act)(sender, e);
            });
        });

        delegateManager.RegisterDelegateConvertor<System.EventHandler<WebSocketSharp.CloseEventArgs>>((act) =>
        {
            return new System.EventHandler<WebSocketSharp.CloseEventArgs>((sender, e) =>
            {
                ((Action<System.Object, WebSocketSharp.CloseEventArgs>)act)(sender, e);
            });
        });
        appdomain.DelegateManager.RegisterMethodDelegate<VSWorkSDK.Data.WebResponseData>();
        appdomain.DelegateManager.RegisterDelegateConvertor<VSWorkSDK.Data.SDKWebResponseCallback>((act) =>
        {
            return new VSWorkSDK.Data.SDKWebResponseCallback((response) =>
            {
                ((Action<VSWorkSDK.Data.WebResponseData>)act)(response);
            });
        });
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Transform>();
        appdomain.DelegateManager.RegisterMethodDelegate<VRVoiceInitConfig>();
        appdomain.DelegateManager.RegisterMethodDelegate<VRProgressInfo>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, System.Int32>();
        appdomain.DelegateManager.RegisterMethodDelegate<VRRootChanelRoom>();
        appdomain.DelegateManager.RegisterMethodDelegate<UserScreenShareReqData>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.Int32, System.Int32>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, UnityEngine.Texture>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, System.Double>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, global::CustomVideoPlayer>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, System.String>();
        appdomain.DelegateManager.RegisterMethodDelegate<global::ConnectAvatars>();
        appdomain.DelegateManager.RegisterMethodDelegate<VSWorkSDK.Data.RoomSycnData>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.Collections.Generic.List<System.Object>>();
        appdomain.DelegateManager.RegisterMethodDelegate<global::LocalCacheFile>();
        appdomain.DelegateManager.RegisterMethodDelegate<global::GlbSceneObjectFile>();
        appdomain.DelegateManager.RegisterMethodDelegate<global::UserScreenShareReqExData>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.Int32>();
        appdomain.DelegateManager.RegisterMethodDelegate<GaussianModelData>();

    }

    private void RegisterVRFunction(ILRuntime.Runtime.Enviorment.DelegateManager delegateManager)
    {
        delegateManager.RegisterMethodDelegate<string, float, Vector2>();
        delegateManager.RegisterMethodDelegate<string, GameObject>();
        delegateManager.RegisterMethodDelegate<string, string, string>();
        //delegateManager.RegisterMethodDelegate<string>();
        delegateManager.RegisterMethodDelegate<WsPlaceMarkList>();
        //delegateManager.RegisterMethodDelegate<bool>();
        delegateManager.RegisterMethodDelegate<GameObject>();
        delegateManager.RegisterMethodDelegate<Texture2D>();
        delegateManager.RegisterMethodDelegate<Cubemap>();
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
        delegateManager.RegisterMethodDelegate<WsBigScreen>();
        delegateManager.RegisterMethodDelegate<LoadAvatarOBJ>();

        delegateManager.RegisterMethodDelegate<WsAvatarFrame>();
        delegateManager.RegisterMethodDelegate<WsMovingObj>();
        delegateManager.RegisterMethodDelegate<string, bool>();
        delegateManager.RegisterMethodDelegate<Dictionary<string, string>>();
        delegateManager.RegisterMethodDelegate<CommonVREventType, float, Vector2>();
        delegateManager.RegisterMethodDelegate<VRPointObjEventType, GameObject>();

        delegateManager.RegisterFunctionDelegate<System.Boolean>();
    }
}
