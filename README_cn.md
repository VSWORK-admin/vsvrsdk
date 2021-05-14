## 一、环境搭建
### 1. 开发环境
unity版本： 使用 **unity2019.2.16** 版本
### 2. VSVRSDK 环境搭建
###### 2.1.1  注册并登录 check到本地 或直接下载 zip工程包
> https://gitee.com/vswork_admin/vsvrsdk

或

> https://github.com/VSWORK-admin/vsvrsdk

## 二、 unity3d 的 不同发布平台的设置需求：

Android :
```
## Build Setting -> 
TextureCompression : ASTC 

## PlayerSetting -> OtherSettings:
ColorSpace : Linear
AutoGraphics API : false 
GraphicsAPIs : OpenGLES3 
LightmapEncoding : High Quality 
LightmapStreaming Enabled : True 
Minimum API Level : 24
Allow 'unsafe' Code : True

## PlayerSetting-> XR Settings:
Virtual Reality Supported : True
VirtualRealitySDKs : none
Stereo Rendering Mode : Multiview
```

iOS:
```
## PlayerSetting -> OtherSettings: 
ColorSpace : Linear
Auto Graphics API  : False 
GraphicsAPIs : Metal
LightmapEncoding : High Quality 
LightmapStreaming Enabled : True
Allow 'unsafe' Code : True

## PlayerSetting -> XR Settings: 
Virtual Reality Supported : False
```

Windows:
```
## Build Setting -> 
TargetPlatform : Windows
Architecture : x86_64 

## PlayerSetting -> OtherSettings: 
ColorSpace : Linear
AutoGraphics API for Windows : True 
LightmapEncoding : High Quality 
LightmapStreaming Enabled : True
Allow 'unsafe' Code : True

## PlayerSetting -> XR Settings: 
Virtual Reality Supported : True
VirtualRealitySDKs : none
Stereo Rendering Mode : SinglePass
```

macOS:
```
## Build Setting -> 
TargetPlatform : Mac OS X

## PlayerSetting -> OtherSettings: 
ColorSpace : Linear
AutoGraphics API for Mac : True
LightmapEncoding : High Quality 
LightmapStreaming Enabled : True
Allow 'unsafe' Code : True

## PlayerSetting -> XR Settings: 
Virtual Reality Supported : False
```



## 三、场景设置
###### 3.1. 场景内使用LWRP渲染管线
###### 3.2. 场景内的材质使用 Lightweight Render Pipline 中的Shader，不要使用Autodesk Interactive Shader
###### 3.3. 场景内尽量不使用实时灯光，场景中需要添加 Light Probe Group （光照探针）用来给动态物体提供光环境
###### 3.4. 场景内尽量少使用法线贴图，使用的图片大小尺寸避免过大造成渲染资源浪费

## 四、VR设置
###### 4.1.位置点 、位置组 、位置面
```
（1）_VSVRSDK -》 _Bundle_Prefab -》 PlayceMarks -》_Playces	拖入到场景中后鼠标右击 选择 unpack prefab completely
（2）在 _Playces 物体的 WSPlayceController 上 设置 初始位置组，
（3）在位置组物体的 VRPlayceGroup 上 设置位置组包含的位置点
（4）在允许自由走动的平面上 添加 meshcollider 或者 boxcollider 然后添加 VRPlayceMeshMark 脚本到该物体上。
如果该物体添加到 _Playces 物体的子物体，则该物体会默认隐藏，位置移动时才会出现。
```
###### 4.2.第三人称视角摄像机
```
（1）_VSVRSDK -》 _Bundle_Prefab -》CameraMark -》 _CameraScreens	拖入到场景中后鼠标右击 选择 unpack prefab completely
（2）在_CameraScreens的 PCScreenCameraSceneController 上 设置初始 第三人称视角摄像机 和其他摄像机，
摄像机会在主持人点击 “切换相机” 时出现，并可以用激光笔选择相机。也可以在 VRAction中使用SetVRScreenCamera 切换相机。
```
###### 4.3.大屏幕
```
（1）_VSVRSDK-》 _Bundle_Prefab -》Bigscreen -》 _BigScreens 拖入到场景中后鼠标右击 选择 unpack prefab completely
（2）在 _BigScreens 的 WsBigScreenController 上设置 初始大屏位置 和 大屏初始状态
```
###### 4.4.手柄菜单
```
（1）_VSVRSDK -》 _Bundle_Prefab -》_CustomSceneMenu 拖入到场景中后鼠标右击选择 unpack prefab completely
（2）在_CustomSceneMenu 的 HomeVRMenuController 上设置按钮出现和隐藏的方式
（3）按钮 name ： change_A_B_C_D	含义为 ：点击按钮时向其他用户广播 A B C D 消息
（4）按钮 name： placeto_G1 含义为 ： 点击按钮时所有用户移动到 G1位置组上
```
###### 4.5.Unity UGUI 按钮与交互
```
在 UGUI 的canvas 上添加 VRUISelectorProxy 脚本， 场景运行时 脚本会自动为子物体添加射线交互事件，若按钮的宽高是由其父物体设置的，则需要手动为按钮添加合适的boxcollider
```

## 五、交互编程
#### 5.1 可视化编程 ： 使用```Playmaker``` + ```VSVR Plamaker VRActions```
#### 5.2 代码编程 ： 使用ILruntime C# 脚本，使用说明详见 vsvrdll 示例工程地址，或打开 DllProject/Click_show_hideDemo:
> https://gitee.com/vswork_admin/vsvrdll

或

> https://github.com/VSWORK-admin/vsvrdll


## 六、场景打包
###### 6.1. 打包名称设置
> 将需要打包的场景设置AssetBundle ：设置bundle的名称和后缀（scene），也可以打包后将文件后缀名改为 .scene 
###### 6.2. 资源检查
> 打开 Window -》 AssetBundle Browser	以Size排序 ，检查避免出现多余的文件和过大的文件（主要是过大的图片，需要使用外部图片编辑软件改变图片格式，如果有Alpha通道则建议改为png格式，如果没有Alpha通道则建议改为jpg格式）
###### 6.3. 打包平台选择

- Android平台所需场景文件 需要在AssetBundle Browser 中 BuildTartget 设置为 **Android**

- iOS 平台需要设置为 **iOS**

- Window平台运行场景需要设置为 **Standalone Windows**

- MacOS 平台运行需要设置为  **Standalone OSX Universal**

**建议打开Unity的CacheServer ： Edit -> Preferences -> CacheServer -> Cache Server Mode  选择 Local， 以避免平台的切换导致的资源重复加载，节省不同平台assetbundle在同一个工程打包的时间。**
###### 6.4. 多人调试和使用
> 在上传至资源管理后台之前 需要为不同平台所需bundle文件设置文件名前缀 
```
# LWRP 渲染管线 :  
    安卓 : a_  (覆盖设备包括 ：Oculus quest， pico neo2， pico G2 ，baidu VR ，IQUT VR, HTC Vive focus ， 安卓手机端)
    iOS : i_  （覆盖设备包括 ：iphone ，ipad） 
    Windows : w_  （覆盖设备包括 ：Oculus Rift S , HTC VIVE， WMR ，Window 桌面端） 
    MAC OS : m_  （覆盖设备包括 ：Mac OS 桌面端）
# Unity Standard 渲染管线 ：
    安卓 ： b_ (覆盖设备包括 ： 华为VR眼镜)

例如 ： 如果需要为华为VR眼镜打包，则需要在Standard 渲染管线设置场景，打包资源包名称 为  ceshi.scene, 则需要上传至资源管理器后台的资源包名称需加上前缀为 ： b_ceshi.scene
如多种设备需要同时加载，则需要各自打包命名成相同文件名称，并加入对应前缀，放置到同一文件目录下即可。 如 ： a_ceshi.scene   b_ceshi.scene   w_ceshi.scene   m_ceshi.scene
```


## 七、VSVR 支持 三维GLB格式 规则说明

> VSVR 支持GLB模型直接加载，可将GLB模型上传至VSVR后台VR资源管理器中，并实时在VR中多人同步观看。并可依靠VSVR SDK对GLB模型进行个性化控制操作。在Glb_LoadAndControl_Example示例中完成了GLB模型的多人手动拆解，激光笔拿起，物体旋转，物体远近，动力学标记等操作。

#### 7.1 glb格式导出：
##### 7.1.1 支持 3DMAX 、MAYA、C4D、Blender、Sketchup 等软件，需要安装glb格式导出插件，建议通过blender导出glb模型。
##### 7.1.2  支持模型50万面以内（三角面），贴图大小不超过 2048*2048，贴图数量不超过20张
#### 7.2、文件和模型名称规则
##### 7.2.1文件命名

* 允许重复加载：
    >默认允许重复加载（允许重复加载的物体默认允许手柄抓握控制
 ，如果屏蔽物体的移动，则给物体名称中添加 ``` _mc_ ```标记）
* 不允许重复加载 ：
    >名称中加入 ```_s```  （添加 ```_s``` 后 场景内的物体默认不可以控制，
    如果需要控制其中部分物体 需要为物体添加``` _c_``` 标记 ）,如 ：```test_s.glb```

##### 7.2.2 物体命名

* 物体默认添加 meshcollier 和 placemark 支持人物移动到物体上去
* 不添加 meshcollider 标记（激光笔可以穿过）：```_mm_```
* 不添加 placemark 标记（角色无法移动到物体上）：```_mt_```
* 添加可控制物体(允许自动加载物体会自动添加可控物体，带```_s``` 后缀glb文件中物体默认不可控制)：```_c_```
* 物体不可抓握控制标记：```_mc_```
* 添加动力学刚体（属性：允许动力学旋转```_r_```）：```_p_```
* 隐藏物体meshrender 使物体不可见： ```_mr_```
* 加载0点（出生点） ：```_zero_```
* 缩放标记：```_scale_```

## 八、 VSVR 支持 平面类格式 规则说明
#### 8.1 图片格式
##### 8.1.1平面图片
大屏演示图片建议格式 ```jpg``` ，图片大小建议控制在 ```1280*720 -- 1600*900```范围
##### 8.1.2全景图片
全景图片建议格式 ```jpg``` ，图片大小建议控制在```4096*2048 -- 6000*3000```范围
#### 8.2 视频格式
平面视频建议格式: ```mp4```  编码: ```H.264``` 码率建议控制在```4Mbps - 6Mbps```范围
全景视频建议格式: ```mp4```  编码: ```H.264``` 码率建议控制在```10Mbps - 20Mbps```范围
#### 8.3 视频流格式
视频流格式支持 ```rtmp``` ```rtsp``` ```hls``` 协议格式视频流
将视频流拉流地址写入到 txt 文件中，将```.txt```文件后缀改为 ```.order``` 上传至资源管理器，点击即可播放
#### 8.4 视频在线点播格式
视频点播url支持 编码 ```h.264``` ```h.265``` 格式url点播 
将视视频点播url地址写入到 txt 文件中，将```.txt```文件后缀改为 ```.order``` 上传至资源管理器，点击即可播放

## 九、 VSVRSDK  VRAction 说明 

#### EventAdminChange  ```事件```：探测主持人改变事件
>使用示例：用来配合切换只有主持人才能看到或操作的某些物体，主持人切换后操作面板自动消失等操作
```
M Enabled： 脚本是否启用
AdminTrue： 自身切换为主持人
adminFalse： 支持切换为非主持人
```
#### EventCustomVRInput  ```事件```：探测VR Input 事件
>使用示例：手柄按钮    头盔戴上取下等事件，比如使用摇杆左右控制某个UI切换下一页，头盔摘下后背景音乐声音变小 ，手柄抓握移动物体等
```
InputType： 需要探测的VR Input事件
Input Event ： 探测的VR Input 事件 触发后执行的 Event 
Rcieved 2D Axis ：摇杆改变时返回 摇杆的2D位置
Rcieved 1D Axis ： 握持键 和 确认键 按动时返回 1D位置
```
#### EventCustomVRInputRouter  ```事件```：VR Input 事件路由
>使用示例：手柄按钮    头盔戴上取下等事件，比如使用摇杆左右控制某个UI切换下一页，头盔摘下后背景音乐声音变小 ，手柄抓握移动物体等
```
InputType： 需要探测的VR Input事件

Message Object ： 用来接收信息的物体
Fsm 2D Axis Name：摇杆改变时返回 摇杆的2D位置 的变量名称
Fsm 1D Axis Name： 握持键 和 确认键 按动时返回 1D位置 的变量名称
Fsm Event Name： 触发事件的名称
```

#### EventPlayerPlaceChange  ```事件```：自身角色移动到位置点或位置面
>使用实例 ： 人物移动到某个位置点时 门自动打开


#### EventRoomConnectionChange  ```事件```：探测房间登录状态事件
>使用示例：房间断开连接或重新连接时 做出UI提示

#### EventSelectAvatarWsid  ```事件```：选择某个角色
>使用示例：激光笔选择某个角色时触发某个事件，并返回该角色ID，可以实现激光笔点击某角色名牌移动    该角色到身前，需要发送信息给其他角色，并由各自判断ID执行事件。
```
Select ID： 选择到的角色ID
Event SelectID ：选择到某角色时触发事件
```
#### EventSelectorPointerStatusChange  ```事件```：激光笔状态改变
>使用示例：当激光笔显示时，显示某些可以点击的物体，激光笔关闭时，将这些可点击的物体隐藏


#### EventTeleportStatusChange  ```事件```：移动状态切换
>使用示例：当每个角色开始用位置移动时（按下摇杆），某些物体隐藏或出现


#### EventVRLaserChange	```事件``` ： 激光笔移动到某个物体上或离开某个物体或在某个物体上点击确认按键时获取改变的物体
```
Point Event Type：选择监听 Enter Exit 或者 click事件
PointEvent ：事件触发
Pointed Obj ： 返回当前激光笔监听的事件的物体
```
#### EventVRSystemMenuChange ```事件``` ：系统菜单和场景菜单呼出和关闭事件
>使用示例：当系统菜单呼出时自动隐藏某个物体，当场景菜单呼出时自动出现可控制物体标记


#### GetAdminStatus ```获取``` ：判断当前自身是否为主持人身份。
#### GetAvatarSort ```获取```： 获取自身在当前房间内所有角色的排序。
#### GetKODToLocalCacheFile ```获取``` ： 缓存KOD服务的远程文件到缓存文件
>使用示例：将KOD服务器中的某个文件缓存到本地，并返回本地路径给其他模块使用
```
KOD Url ：KOD服务器上的相对路径IsURLSign ： 是否通过网络url文件来控制版本
HasPrefix ： 是否根据自身平台来添加平台前缀， 安卓添加”a“ windows 添加 ”w“ 
GetLocalPath ： 缓存文件成功 事件
GetLocalPathFaild ： 缓存文件失败事件
Geted Sign ： 获取到的签名
LocalPath ： 缓存成功后的本地文件路径
LocalUrl ： 缓存成功后的URL路径
```
#### GetPathToScene ```获取```： 缓存本地文件路径到场景
>使用示例：配合 GetKODToLocalCacheFile 加载场景
```
Path ：场景文件的本地路径
Sign ： 场景文件签名
SceneName ： 场景的名称
```
#### GetPathToTexture ```获取``` ： 缓存本地文件路径到Texture
>使用示例： 配合 8 GetKODToLocalCacheFile 加载远程图片素材
```
Path ： 图片文件的本地路径
EventGetTexture ： 获取图片成功事件
EventGetTextureFail ： 获取图片失败事件
Geted Texture ： 获取到的Texture
```
#### GetUrlToLocalCacheFile ```获取```： 通过URL缓存服务远程服务器中的远程文件到缓存文件
>使用示例：缓存一个互联网文件
```
HttpUrl : 文件所在路径地址
IsURLSign : 是否使用http的txt文件内容作为版本标记
Sign ： 签名（版本） 或 http 签名文件路径
HasPrefix ：是否根据设备自动获取所对应scene文件，比如 "a_xx.xx" "w_xx.xx"
GetLocalPath : 获取到文件成功
GetLocalPathFaild : 获取到文件失败
GetedSign : 获取到的签名
LocalPath ：已缓存文件的的本地路径
LocalUrl ： 已缓存文件的本地url（`File：//`+ 本地路径）
```

#### GetUrlToLoadScene ```获取```： 通过URL缓存服务远程服务器中的场景
>使用示例：在场景中加载另外的url链接场景
```
HttpUrl : scene文件所在路径地址
IsURLSign : 是否使用http的txt文件内容作为版本标记
Sign ： 签名（版本） 或 http 签名文件路径
SceneName ：场景名称
HasPrefix ：是否根据设备自动获取所对应scene文件，比如 "a_xx.scene" "w_xx.scene"
GetLocalPath : 获取到场景成功
GetLocalPathFaild : 获取到场景失败
```

#### GetVRGameObjects ```获取``` ： 获取VR场景内相关参数
>使用示例：获取左右手手柄位置，获取自己的昵称等配合其他模块完成功能
````
Everyframe ： 每一帧更新获取到的信息
Maincamera ： 返回当前主视角相机的gameobject 
LeftHand ： 返回左手gameobject
RightHand ： 返回右手gameobjet
LeftTeleportAnchor ： 返回左手手柄Teleport射线的初始位置物体
RightTeleportAnchor： 返回右手手柄Teleport射线的初始位置物体
leftFingerPointerAnchor ： 返回左手手柄激光笔的初始位置物体
RightFingrerPointerAnchor ：返回右手手柄激光笔的初始位置物体
MainVRROOT ： 返回角色的Root物体
LaserPoint ：返回激光笔点的物体gameobject
GlbRoot ： 返回Glb 场景和 Glb物体的总父物体
GlbSceneRoot ：返回Glb场景父物体（加载的glb文件后缀添加了_s 的物体）
GlbOjbRoot ： 返回Glb物体父物体（加载的glb文件后缀未添加_s 的物体）
BigscreenRoot ： 返回大屏gamobject
Msex ： 返 回 当 前 角 色 性 别 0 1 
mWsID ： 返回当前角色 网络连接ID 
mAvatarID ： 返 回 当 前 角 色 ID 
MnickName ：返回当前角色昵称
GMEconnected ： 返回语音连接状态
NowMicVol ： 返回当前语音麦克的声音大小
MicEnabled ： 返回当前Mic是否被禁用
NowSelectedAvatarWsid ： 返回当前选择的角色的网络连接ID 
nowUnityVersion ：返回当前Unity版本
isVRApp ： 返回是否时通过头盔连接的VR 
ismobile ： 返回是否是移动设备接入
AutoPlayScene ： 返回是否是场景自动播放模式状态
Startaid : 获取自身默认角色id
MenuAnchor ：获取系统菜单锚点物体
isadmin sadmin：获取用户是否为主持人
isDrawingenabled : 获取自身画笔是否启用
````

#### GetVRLanguage ```获取``` ： 获取目前用户的语言
>使用示例 ： 检测语言并设置该语言的场景面板
```
LanguageSort ： 获取当前语言的排序   0： English  1：Chinese 3:Korean 4:Japanese
LanguageString ： 获取当前语言的字符串   English  Chinese Korean Japanese 

```

#### RecieveBigScreenTexture ```网络获取``` ： 大屏幕获取到图片
>使用示例：资源库获取到全景图片后，360图改变
```
WsTexture ： 获取或缓存到的图片资源
RecieveTextureEvent ： 大屏播放新的的图片事件
WsTexturePaht ： 获取到的大屏播放图片的路径
UpdateTextureEvent ： 资源管理器缓存图片成功事件
```
#### RecieveBigScreenVideoPlayer ```网络获取``` ： 大屏幕获取到视频帧使用示例：大屏播放视频时 同步更新视频的帧贴图到三维物体。

#### RecieveCustomWsMessages ```网络获取``` ： 获取到网络同步信息
>使用示例： 所有角色同步事件通过此模块来获取
```
Bypass：忽略网络获取，触发Recieve事件
Recieve ： 获取到任意的网络信息触发事件
AbcdefgFlow ：收到的ABCDEFG信息按顺序判断处理
Ws_a - Ws_g : 需要对比的 A 信息
RecieveSame_a  - RecieveSame_g收到与Ws_a - Ws_g相同的信息触发事件
A-G ： 存储收到的A-G信息文本
```
#### RecieveWsPlaceMark ```网络获取``` ： 获取到位置改变信息
>使用示例： 移动到某个位置或位置组时 某些物体显示，移动到某个位置组时  第三视角相机切换到相应的视角
````
WsGroupName ： 需要对比的位置组的名称
WsMyPlaceName ： 需要对比的位置的名称
RecieveSameGroupName ： 收到与WsGroupName相同的位置组名称触发事件
RecieveMysamePlaceaName ： 收到与WsMyPlaceName 相同的位置的名称触发事件
RecievedGroupName ： 存储收到的位置组的名称
RecievedPlaceName ： 存储收到的位置的名称
````
#### SendCommitOrder ```网络发送``` ： 发送调试口令
#### SendCustomWsMessages ```网络发送``` ： 发送网络同步信息使用示例：发送同步信息给其他所有角色（包括自身） ABCDEFG
#### SendWsAllInfoLog ```网络发送``` ： 发送Log信息给其他角色
#### SendWsAllPlayceTo ```网络发送``` ： 发送移动位置组指令到其他角色（包括自身） 使用示例： 所有角色移动到某个位置组
```
GroupName ： 位置组名称
ToALl ： 是否发送给所有人
```
#### SetCustomVRInput ```设置``` ： 设置VRInput指令
>使用示例：  手柄震动  重置位置（Windows端有效，Quest端无效）
```
Input Type： 指令类型
Vibinfo Hand ： 选择手柄震动
Frequency ： 震动频率 0-1 
Amplitude ： 震动强度 0-1 
Lasttime ： 持续时间 0-4
```
#### SetHandModelEnabled ```设置``` ： 设置手柄模型是否隐藏使用示例： 拿起工具时隐藏手柄模型 ，放下工具时手柄模型显示
#### SetLoadUrlIdScene ```设置```： 加载远程服务器上的场景
```
Server ： 远程服务器地址ID ： 场景ID
IsNowServer ： 是否使用当前登录的远程服务器
Update ： 是否仅更新场景而不加载
```
#### GetUrlToLoadScene ```获取```： 加载自定义路径场景
```
HttpUrl：scene 文件的url地址，填写带有“a”前缀的scene完整路径
IsURLSign：设置是否通过urltxt定义版本签名文件
Sign ：版本签名，如果IsURLSign 勾选 则填写 txt文件的url完整路径
SceneName ： 场景名称
HasPrefix ：是否按不同平台加载不同的scene ，默认需要勾选
```

#### SetMaterialTextrueEveryFrame ```设置``` ： 每一帧为物体改变贴图，配合 RecieveBigScreenVideoPlayer 将视频播放的Texture更新到物体上
#### SetVRBigScreen ```设置``` ： 将大屏幕其替换到某个位置
#### SetVRInputField ```设置``` ： 将某个UI的InputField 设置为输入状态
#### SetVRControllingObj ```设置``` ： 将某个物体设置为控制和帧同步状态
```
Controlling Obj ：需要设置的物体
Islocal ： 是否是世界位置或自身位置
Is Sending ：是否发送此物体的位置信息
TriiggerStart ：开始发送位置信息
TriggerEnd	： 结束发送位置信息
TriggerEndEvent ： 结束发送位置信息触发事件
FrameSend ：发送帧速率 1 ： 以最高帧率发送 2： 以一半的帧率发送 3：以三分之一帧率发送 4： 以四分之一的帧速率发送
ControllStart ：接收到开始位置信息
Controlling ： 接收到位置信息
ControllEnd ： 接收到结束位置信息
```
#### SetVRMovingObj ```设置``` ： 将某个物体设置为帧同步状态
>使用示例 ： 手柄拿起某物体后，需要将此物体设置为 MovingObj 同步给其他角色
```
MovingObj ： 需要设置的物体
Islocal ： 是否时世界位置或是自身位置
IsSending ： 是否发送此物体的位置信息
FrameSend ： 发送帧速率	1 ： 以最高帧率发送 2： 以一半的帧率发送 3：以三分之一帧率发送 4： 以四分之一的帧速率发送
```

#### SetVRScreenCamera ```设置``` ：设置第三视角相机的位置
>使用示例 ： 移动到某个位置组的时候 切换相应的第三视角相机，（拖动 第三视角相机的Prefab到CameraMark上）


#### SetVRGameObject ```设置``` ：设置自身VR信息
```
Aid  ： 设置角色ID  1. 字段可以设置为后台提供的字符串id 2. 字段可设置为 http 开头的 glb角色的完整路径并在glb后面添加配置参数
    参数格式为：  ?info="glb角色类型"_"版本信息"_"头部的类型"  
    glb角色类型 ： 1：半身角色  2:全身角色  t:任意glb模型
    版本信息 : 填写不同的版本字符串 会重新缓存角色
    头部类型 ：0：glb零点在地面  1：glb零点在头部，但只在y轴旋转 2：glb零点在头部，且旋转完全跟随头部
    如果不更改则置空
Name ：改变头部名牌名称，如果不更改则置空
Hideavatar : 是否隐藏角色
SetColor ： 是否改变画笔和激光笔颜色
Pencolor ： 画笔的颜色
```
#### SendChangeAvatar ```网络发送```  ： 向服务器发送 自身VR信息，与SetVRGameObject结合使用
#### SetVRObjLoadPosition ```设置``` ：设置glb 场景和glb物体加载的位置
#### SetVRSystemMenuEnable ```设置``` ：设置系统菜单的显示和隐藏
#### SetVRVideoPlayer ```设置``` ：设置一个视频播放器
```
ContorlObj ： 用来设置播放器的控制物体，设置完成后可通过 SendMessage 来控制播放器的播放：
	播放：“Play”，停止：“Stop”，暂停：“Pause”，准备：“Prepare”
RenderObj ： Array  先填写Array数量，再将需要渲染视频的物体拖入，物体可以为 UI 的 RawTextrue 或者材质为Unlit-》Texture材质的三维物体，可一次设置多个物体
Url : string  视频流地址，或 远程视频地址 或缓存完成的视频的本地 path地址（非url地址）
Vol ：float   声音大小 范围 0 - 100
Isloop ： bool  是否循环播放（若是视频流则不勾选此项）
Autostart : bool  是否自动播放 （如果不勾选自动播放，则视频只会Prepare准备，可以使用向ContorlObj发送SendMessage 消息来控制播放）
```

#### SetChanelChRoomID ```设置``` ：设置一个动作频道
```
ChRoomID ： 设置一个2-4位小写字母的一个roomid 如：“aa”，“abcd”， 如果此字段为空 则会进入 初始roomid
```
#### SetVoiceExRoomID ```设置``` ：设置一个语音频道
```
ExRoomID ： 设置一个2-4位小写字母的一个ExRoomID 如：“aa”，“abcd”， 如果此字段为空 则会进入 初始语音roomid
```
#### SetVoiceRoomExit ```设置``` ：退出语音频道
```
ExRoomID ： 设置一个2-4位小写字母的一个ExRoomID 如：“aa”，“abcd”， 如果此字段为空 则会进入 初始语音roomid
```

#### SetRootChanelChRoomID ```设置``` ：设置进入新的Root频道
```
RootRoomID ： 设置后台提供的RootRoomID
RootVoiceID： 设置后台提供的RootVoiceID
```