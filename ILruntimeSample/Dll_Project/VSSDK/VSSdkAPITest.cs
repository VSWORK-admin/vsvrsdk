using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VSWorkSDK;
using VSWorkSDK.Data;
using LitJson;

namespace Dll_Project
{
    public class VSSdkAPITest : DllGenerateBase
    {

        public override void Init()
        {
            base.Init();
        }
        public override void Awake()
        {
            base.Awake();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            VSEngine.Instance.OnEventTextTranslateResult += OnEventTextTranslateResult;

            VSEngine.Instance.OnEventTencentVoiceIdentifyResult += OnEventTencentVoiceIdentifyResult;

            VSEngine.Instance.OnEventTencentTextToVoiceEnd += OnEventTencentTextToVoiceResult;

            VSEngine.Instance.OnEventLongVoiceIdentifyResult += OnEventLongVoiceIdentifyResult;

            VSEngine.Instance.OnEventSystemSettingPanelHide += OnEventSystemSettingPanelHide;

            VSEngine.Instance.OnEventStopSelfShareByOutSide += OnEventStopSelfShareByOutSide;

            VSEngine.Instance.OnEventNoticeWebViewShow += OnEventNoticeWebViewShow;

            VSEngine.Instance.OnEventNoticeWebViewClosed += OnEventNoticeWebViewClosed;

            VSEngine.Instance.OnEventCloseWebviewByHand += OnEventCloseWebviewByHand;

            VSEngine.Instance.OnEventRecieveAvatarDriverOBJ += OnEventRecieveAvatarDriverOBJ;

            VSEngine.Instance.OnEventCameraDirectorMode += OnEventCameraDirectorMode;

            VSEngine.Instance.OnEventAgoraShareFaild += OnEventAgoraShareFaild;

            VSEngine.Instance.OnEventReceiveWebviewMessageBase64 += OnEventReceiveWebviewMessageBase64;

            VSEngine.Instance.OnEventReceiverLoadAssetBundleObj += OnEventReceiverLoadAssetBundleObj;

            VSEngine.Instance.OnEventGetCheckFileAlreadyCached += OnEventGetCheckFileAlreadyCached;

            VSEngine.Instance.OnEventReciveAvatarDirect += OnEventReciveAvatarDirect;

            VSEngine.Instance.OnEventReciveLoadGaussionModel += OnEventReciveLoadGaussionModel;

            VSEngine.Instance.OnEventHandleCloudRenderMsgInfo += OnEventHandleCloudRenderMsgInfo;

            VSEngine.Instance.OnEventReciveClickWalkTargetPoint += OnEventReciveClickWalkTargetPoint;

            VSEngine.Instance.OnEventReciveSwitchCameraType += OnEventReciveSwitchCameraTpye;
        }

        private void OnEventTextTranslateResult(string obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventTencentVoiceIdentifyResult(string arg1, int arg2)
        {
            Debug.LogError("OnEventTextTranslateResult" + arg1 + arg2);
        }

        private void OnEventTencentTextToVoiceResult()
        {
            Debug.LogError("OnEventTextTranslateResult");
        }

        private void OnEventLongVoiceIdentifyResult(string obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventSystemSettingPanelHide(bool obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventStopSelfShareByOutSide()
        {
            Debug.LogError("OnEventTextTranslateResult");
        }

        private void OnEventNoticeWebViewShow(bool obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventNoticeWebViewClosed(bool obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventCloseWebviewByHand(bool obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventRecieveAvatarDriverOBJ(string arg1, GameObject arg2)
        {
            Debug.LogError("OnEventTextTranslateResult" + arg1 + arg2);
        }

        private void OnEventCameraDirectorMode(bool obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventAgoraShareFaild(int obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventReceiveWebviewMessageBase64(string obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventReceiverLoadAssetBundleObj(string arg1, object arg2)
        {
            Debug.LogError("OnEventTextTranslateResult" + arg1 + arg2);
        }

        private void OnEventGetCheckFileAlreadyCached(string arg1, bool arg2)
        {
            Debug.LogError("OnEventTextTranslateResult" + arg1 + arg2);
        }

        private void OnEventReciveAvatarDirect(Vector3 obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventReciveLoadGaussionModel(GaussianModelData obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + JsonMapper.ToJson(obj));
        }

        private void OnEventHandleCloudRenderMsgInfo(int arg1, string arg2)
        {
            Debug.LogError("OnEventTextTranslateResult" + arg1 + arg2);
        }

        private void OnEventReciveClickWalkTargetPoint(Vector3 obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        private void OnEventReciveSwitchCameraTpye(int obj)
        {
            Debug.LogError("OnEventTextTranslateResult" + obj);
        }

        public override void OnDisable()
        {
            base.OnDisable();

        }
        public override void Start()
        {
            base.Start();
        }
        public override void Update()
        {
            base.Update();
            if (Input.GetKey(KeyCode.LeftControl))
            {
                CtrlClickDown();
            }
            if (Input.GetKey(KeyCode.RightAlt))
            {
                AltClickDown();
            }
        }
        public void CtrlClickDown()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                VSEngine.Instance.SetDirectorModelEnable(true);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                VSEngine.Instance.SetDirectorModelEnable(false);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                string url = "https://vs.vscloud.vip/vs/index.php?user/publicLink&fid=875b0l4hIrkjEMMRk9rIH9tQangpeZcOLMyDjlubnw5foa1mS2EK5YOoJcnCgN35DbmKOnb6AL1ldLlNfbx-y1UBNZ0eNcCZFTf7g7bqGJo8zhXV560HOYCWzA&file_name=/test666.bin";
                bool blocal = false;
                GameObject root = GameObject.Find("GaussianRoot");
                VSEngine.Instance.LoadGaussianModel(url, "", blocal, root, "");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                VSEngine.Instance.SetMenuPanelEnable(false);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                VSEngine.Instance.SetMenuPanelEnable(true);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                VSEngine.Instance.StartTextTranslate(mStaticThings.I.mAvatarID, "你是谁啊", "2", ((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString());
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                VSEngine.Instance.SetupTranslateLanguage("English");
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                VSEngine.Instance.SetupTranslateLanguage("Japanese");
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                VSEngine.Instance.StartSystemVoiceIdentify();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                VSEngine.Instance.EndSystemVoiceIdenetify();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                VSEngine.Instance.StartSystemTextToVoice("雨，自古以来就是诗人和作家笔下永恒的主题，它既能带来生机，也能带来毁灭。下面是一篇形容雨大的文章：暴雨的狂想乌云压顶，天空如同被泼洒了浓墨，沉重得仿佛随时都会坠落。空气中弥漫着潮湿的气息，预示着即将到来的风暴。突然，一阵狂风呼啸而过，卷起地上的尘土，将它们抛向空中，形成一片混沌。接着，雨来了。它不是轻描淡写的细雨，也不是温柔的春雨，而是一场肆无忌惮的暴雨。雨点如同千军万马，从天而降，撞击着大地，发出震耳欲聋的响声。它们无情地敲打着窗户，似乎想要穿透玻璃，进入屋内。屋檐下，雨水汇聚成一条条小溪，迅速地流淌着。街道上的积水迅速上升，车辆驶过时溅起的水花，像是海浪拍打着岸边。行人匆匆，伞成了他们唯一的庇护，但即便是如此，也难以抵挡这狂暴的雨势。树木在雨中摇曳，它们的枝叶被雨水打得沙沙作响，仿佛在诉说着自然的愤怒。花儿在雨中低垂，花瓣被无情地打落，铺就了一条条花径。整个城市，在这暴雨的洗礼下，显得格外脆弱。然而，暴雨也有它独特的美。雨后的空气清新，带着泥土的芬芳，让人精神一振。雨滴在水面上激起的涟漪，一圈圈扩散开去，像是大自然的画笔，绘制出一幅幅动人的画面。暴雨，是自然力量的展现，是对人类的一种提醒：在大自然的面前，我们是如此的渺小。它让我们学会敬畏，学会珍惜，学会在风雨中寻找生存的智慧。这篇文章试图捕捉暴雨的狂野与美丽，以及它给人们带来的感受和启示。希望你喜欢这篇描述雨大的文章。", 1003, 0);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                VSEngine.Instance.EndSystemTextToVoice();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                VSEngine.Instance.StartSystemLongVoiceIdentify();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                VSEngine.Instance.EndSystemLongVoiceIdentify();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                VSEngine.Instance.ThirdPersionChangeCameraOffset(new Vector3(10, 100, 100));
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                VSEngine.Instance.RevertThirdPersionCameraOffset();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                VSEngine.Instance.LockThirdPersonMode(0);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                VSEngine.Instance.LockThirdPersonMode(1);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                VSEngine.Instance.SetSoundMute(true, true, false);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                VSEngine.Instance.SetSoundMute(true, false, true);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                VSEngine.Instance.SetSoundMute(true, true, true);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                VSEngine.Instance.SetSoundMute(false, false, false);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                VSEngine.Instance.ShowOrHideAppStickControl(false);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                VSEngine.Instance.ShowOrHideAppStickControl(true);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //   VSEngine.Instance.CheckFileAlreadyCached(true);
            }
        }
        public void AltClickDown()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                VSEngine.Instance.CheckFileAlreadyCached("dsfasd");
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                Texture texture = BaseMono.ExtralDataObjs[0].Target as Texture;
                VSEngine.Instance.SetDefaultBigScreenImage(texture);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                VSEngine.Instance.StopGrabObject(BaseMono.gameObject);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
              //  VSEngine.Instance.LoadAssetBundleObj(false);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                VSEngine.Instance.SetMenuPanelEnable(true);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                VSEngine.Instance.StartTextTranslate(mStaticThings.I.mAvatarID, "你是谁啊", "2", ((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString());
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                VSEngine.Instance.SetupTranslateLanguage("English");
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                VSEngine.Instance.SetupTranslateLanguage("Japanese");
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                VSEngine.Instance.StartSystemVoiceIdentify();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                VSEngine.Instance.EndSystemVoiceIdenetify();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                VSEngine.Instance.StartSystemTextToVoice("雨，自古以来就是诗人和作家笔下永恒的主题，它既能带来生机，也能带来毁灭。下面是一篇形容雨大的文章：暴雨的狂想乌云压顶，天空如同被泼洒了浓墨，沉重得仿佛随时都会坠落。空气中弥漫着潮湿的气息，预示着即将到来的风暴。突然，一阵狂风呼啸而过，卷起地上的尘土，将它们抛向空中，形成一片混沌。接着，雨来了。它不是轻描淡写的细雨，也不是温柔的春雨，而是一场肆无忌惮的暴雨。雨点如同千军万马，从天而降，撞击着大地，发出震耳欲聋的响声。它们无情地敲打着窗户，似乎想要穿透玻璃，进入屋内。屋檐下，雨水汇聚成一条条小溪，迅速地流淌着。街道上的积水迅速上升，车辆驶过时溅起的水花，像是海浪拍打着岸边。行人匆匆，伞成了他们唯一的庇护，但即便是如此，也难以抵挡这狂暴的雨势。树木在雨中摇曳，它们的枝叶被雨水打得沙沙作响，仿佛在诉说着自然的愤怒。花儿在雨中低垂，花瓣被无情地打落，铺就了一条条花径。整个城市，在这暴雨的洗礼下，显得格外脆弱。然而，暴雨也有它独特的美。雨后的空气清新，带着泥土的芬芳，让人精神一振。雨滴在水面上激起的涟漪，一圈圈扩散开去，像是大自然的画笔，绘制出一幅幅动人的画面。暴雨，是自然力量的展现，是对人类的一种提醒：在大自然的面前，我们是如此的渺小。它让我们学会敬畏，学会珍惜，学会在风雨中寻找生存的智慧。这篇文章试图捕捉暴雨的狂野与美丽，以及它给人们带来的感受和启示。希望你喜欢这篇描述雨大的文章。", 1003, 0);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                VSEngine.Instance.EndSystemTextToVoice();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                VSEngine.Instance.StartSystemLongVoiceIdentify();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                VSEngine.Instance.EndSystemLongVoiceIdentify();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                VSEngine.Instance.ThirdPersionChangeCameraOffset(new Vector3(10, 100, 100));
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                VSEngine.Instance.RevertThirdPersionCameraOffset();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                VSEngine.Instance.LockThirdPersonMode(0);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                VSEngine.Instance.LockThirdPersonMode(1);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                VSEngine.Instance.SetSoundMute(true, true, false);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                VSEngine.Instance.SetSoundMute(true, false, true);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                VSEngine.Instance.SetSoundMute(true, true, true);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                VSEngine.Instance.SetSoundMute(false, false, false);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                VSEngine.Instance.ShowOrHideAppStickControl(false);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                VSEngine.Instance.ShowOrHideAppStickControl(true);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //   VSEngine.Instance.CheckFileAlreadyCached(true);
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}
