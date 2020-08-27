## 一、环境搭建
### 1. unity版本： 使用 unity2019.2.16 版本
### 2. VSVRSDK 环境搭建
#### 2.1 方式 A   
注册并登录 gitee.com ,   Git 或 svn 获取 或直接下载 zip工程包
> Git ：https://gitee.com/vswork_admin/vsvrsdk.git

> Svn： svn://gitee.com/vswork_admin/vsvrsdk

> zip： https://gitee.com/vswork_admin/vsvrsdk

#### 2.2 方式 B    
手动部署
###### 2.2.1. 新建空工程
###### 2.2.2. 插件 package manager 中 添加插件 ：
```
LightweightRP
Asset Bundle Browser
```
###### 2.2.3. 设置渲染管线：
Project Settings ： Scriptable Render Pipeline Settings 中 创建 LWRP 设置
###### 2.2.4. 先导入第三方插件 然后导入   VSVR_SDK：

 >  必须导入的第三方插件 ： DOTween Pro、Event System - Dispatcher、 Playmaker  、Playmaker_CustomActions （PlayMaker第三方接口）

 >  可按需导入的第三方插件 ： BlendShape 、FinalIK、MagicaCloth、PathFinding，Unity Timeline ShaderGraph 

## 二、 unity3d 的 安卓 和 Windows设置：

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

## PlayerSetting-> XR Settings:
Virtual Reality Supported : True
VirtualRealitySDKs : none
Stereo Rendering Mode : Multiview
```


Windows:
```
## PlayerSetting -> OtherSettings: 
ColorSpace : Linear
AutoGraphics API for Windows : True 
LightmapEncoding : High Quality 
LightmapStreaming Enabled : True

## PlayerSetting -> XR Settings: 
Virtual Reality Supported : True
VirtualRealitySDKs : none
Stereo Rendering Mode : SinglePass
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
## 五、场景打包
###### 5.1. 打包名称设置
> 将需要打包的场景设置AssetBundle ：设置bundle的名称和后缀（scene），也可以打包后将文件后缀名改为 .scene 
###### 5.2. 资源检查
> 打开 Window -》 AssetBundle Browser	以Size排序 ，检查避免出现多余的文件和过大的文件（主要是过大的图片，需要使用外部图片编辑软件改变图片格式，如果有Alpha通道则建议改为png格式，如果没有Alpha通道则建议改为jpg格式）
###### 5.3. 打包平台选择
>安卓平台所需场景文件 需要再AssetBundle Browser中设置为Android , Window平台运行场景需要设置为Standalone Windows
###### 5.4. 多人调试和使用
> 在上传至资源管理后台之前 需要为不同平台所需bundle文件设置文件名前缀 
```
# LWRP 渲染管线 :  
    安卓 : a_  (覆盖设备包括 ：Oculus quest， pico neo2， pico G2 ， 安卓手机端)
    Windows : w_  （覆盖设备包括 ：Oculus Rift S , HTC VIVE， WMR ，Window 桌面端） 
# Unity Standard 渲染管线 ：
    安卓 ： b_ (覆盖设备包括 ： 华为VR眼镜)

例如 ： 如果需要为华为VR眼镜打包，则需要在Standard 渲染管线设置场景，打包资源包名称 为  ceshi.scene, 则需要上传至资源管理器后台的资源包名称需加上前缀为 ： b_ceshi.scene
如多种设备需要同时加载，则需要各自打包命名成相同文件名称，并加入对应前缀，放置到同一文件目录下即可。 如 ： a_ceshi.scene   b_ceshi.scene   w_ceshi.scene
```