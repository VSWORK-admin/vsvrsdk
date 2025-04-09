using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSWorkSDK.Enume
{
    public enum CameraViewMode
    {
        FirstPerson = 1,
        ThirdPerson = 3,
    }
    public enum WalkLockMode
    {
        UNLOCK,
        LOCKWALK,  //只能走
        LOCKRUN,   //只能跑
    }
    public enum MovingObjectMarkType
    {
        MovingObject,
        UserGravity,
        UnUseGravity
    }
    public enum CacheDataFileType
    {
        SceneFile,
        MediaFile,
        AvatarFile,
        SystemData,
        LoginData,
    }
    public enum SceneDataObjectType
    {
        GlbScene,
        GlbObject,
        AdminData,
        LaserDrawing
    }
    public enum RenderQualityLevel
    {
        Low,
        Middle,
        High,
        Highest
    }
    public enum SyncDataFrameRate
    {
        Rate_100,
        Rate_50,
        Rate_30,
        Rate_25,
        Rate_20,
        Rate_16,
        Rate_10,
        Rate_7,
        Rate_5,
        Rate_2,
        Rate_1
    }
    public enum AvatarHeightFixType
    {
        AddOneCM,
        MinOneCM,
        AddFiveCM,
        MinFiveCM,
        ResetHeight
    }
    public enum ActorActionType
    {
        Idle = 1,
        Walk = 2,
        Run = 4,
        Jump = 8,
        Sit = 16,
        Stand = 32,
        WalkLeft = 64,
        WalkRight = 128,
        WalkBack = 256,
        RunBack = 512,
        TurnLeft = 1024,
        TurnRight = 2048,
        RunLeft = 4096,
        RunRight = 8192,
        CustomAction = 16384,
        Dancing = 32768,
        FlipAni = 65536
    }
    public enum AudioDualMonoModeType
    {
        AUDIO_DUAL_MONO_STEREO = 0,
        AUDIO_DUAL_MONO_L = 1,
        AUDIO_DUAL_MONO_R = 2,
        AUDIO_DUAL_MONO_MIX = 3
    }
    public enum WebRequestType
    {
        POST,
        GET,
    }
    public enum ShareFaildType
    {
        NoDevice,       //没有设备
        NoneCaptchPic,  //没有采集到画面  设备被占用或设备损坏
        Quit,           //分享用户不在分享中
    }
    public enum TransformControlType
    {
        Move,
        Rotate,
        Scale,
        Universal
    }
    public enum AvatarFollowCamera
    {
        Never,  //永不跟随
        Alaways,    //一直跟随
        SpecifyTime //指定时间相机不动，再跟随
    }
    public enum ScreenShareType
    {
        PCWindow,
        Camera,
        GameScreen,
        PhoneWindow,
        MediaPlayer,
        MicAudio,
    }
    public enum Voice_Conversion_Type
    {
        //原声，即关闭变声效果
        VOICE_CONVERSION_OFF = 0,
        //中性。为避免音频失真，请确保仅对女声设置该效果
        VOICE_CHANGER_NEUTRAL = 50397440,
        //甜美。为避免音频失真，请确保仅对女声设置该效果
        VOICE_CHANGER_SWEET = 50397696,
        //稳重。为避免音频失真，请确保仅对男声设置该效果
        VOICE_CHANGER_SOLID = 50397952,
        //低沉。为避免音频失真，请确保仅对男声设置该效果
        VOICE_CHANGER_BASS = 50398208
    }
    public enum Voice_Beautifier_Type
    {
        //原声，即关闭美声效果
        VOICE_BEAUTIFIER_OFF = 0,
        //磁性（男）
        CHAT_BEAUTIFIER_MAGNETIC = 16843008,
        //清新（女）
        CHAT_BEAUTIFIER_FRESH = 16843264,
        //活力（女）
        CHAT_BEAUTIFIER_VITALITY = 16843520,
        //歌唱美声
        SINGING_BEAUTIFIER = 16908544,
        //浑厚
        TIMBRE_TRANSFORMATION_VIGOROUS = 16974080,
        //低沉
        TIMBRE_TRANSFORMATION_DEEP = 16974336,
        //圆润
        TIMBRE_TRANSFORMATION_MELLOW = 16974592,
        //假音
        TIMBRE_TRANSFORMATION_FALSETTO = 16974848,
        //饱满
        TIMBRE_TRANSFORMATION_FULL = 16975104,
        //清澈
        TIMBRE_TRANSFORMATION_CLEAR = 16975360,
        //高亢
        TIMBRE_TRANSFORMATION_RESOUNDING = 16975616,
        //嘹亮
        TIMBRE_TRANSFORMATION_RINGING = 16975872,
        //超高音质，即让音频更清晰、细节更丰富
        ULTRA_HIGH_QUALITY_VOICE = 17039616
    }
}
