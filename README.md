## 一、环境搭建
#### 方式A GIT or SVN
 Git 或 svn 获取，使用 unity2019.2.16 版本打开
> Git ：https://gitee.com/vswork_admin/vsvrsdk.git

> Svn： svn://gitee.com/vswork_admin/vsvrsdk

#### 方式B  手动部署
###### 1. unity版本：1.4.x 及之前版本 使用 unity2019.2.16 版本
###### 2. 插件 package manager 中 添加插件 ：
```
LightweightRP
Asset Bundle Browser
```
###### 3. 设置渲染管线：
Project Settings ： Scriptable Render Pipeline Settings 中 创建 LWRP 设置
###### 4. 先导入第三方插件 然后导入   VSVR_SDK：

 > 4.1 必须导入的第三方插件 ： DOTween Pro、Event System - Dispatcher、 Playmaker  、Playmaker_CustomActions （PlayMaker第三方接口）

 > 4.2 可按需导入的第三方插件 ： BlendShape 、FinalIK、MagicaCloth、PathFinding，Unity Timeline ShaderGraph 

## 二、 安卓和Windows设置：

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
VirtualRealitySDKs : Oculus False False False 
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
VirtualRealitySDKs : Oculus True True 
Stereo Rendering Mode : SinglePass
```


### 三、场景设置
###### 1. 场景内使用LWRP渲染管线
###### 2. 场景内的材质使用 Lightweight Render Pipline 中的Shader，不要使用Autodesk Interactive Shader
###### 3. 场景内尽量不使用实时灯光，场景中需要添加 Light Probe Group （光照探针）用来给动态物体提供光环境
###### 4. 场景内尽量少使用法线贴图，使用的图片大小尺寸避免过大造成渲染资源浪费

### 四、VR设置
###### 1.位置点 、位置组 、位置面
```
（1）_VSVRSDK -》 _Bundle_Prefab -》 PlayceMarks -》_Playces	拖入到场景中后鼠标右击 选择 unpack prefab completely
（2）在 _Playces 物体的 WSPlayceController 上 设置 初始位置组，
（3）在位置组物体的 VRPlayceGroup 上 设置位置组包含的位置点
（4）在允许自由走动的平面上 添加 meshcollider 或者 boxcollider 然后添加 VRPlayceMeshMark 脚本到该物体上。
如果该物体添加到 _Playces 物体的子物体，则该物体会默认隐藏，位置移动时才会出现。
```
###### 2.第三人称视角摄像机
```
（1）_VSVRSDK -》 _Bundle_Prefab -》CameraMark -》 _CameraScreens	拖入到场景中后鼠标右击 选择 unpack prefab completely
（2）在_CameraScreens的 PCScreenCameraSceneController 上 设置初始 第三人称视角摄像机 和其他摄像机，
摄像机会在主持人点击 “切换相机” 时出现，并可以用激光笔选择相机。也可以在 VRAction中使用SetVRScreenCamera 切换相机。
```
###### 3.大屏幕
```
（1）_VSVRSDK-》 _Bundle_Prefab -》Bigscreen -》 _BigScreens 拖入到场景中后鼠标右击 选择 unpack prefab completely
（2）在 _BigScreens 的 WsBigScreenController 上设置 初始大屏位置 和 大屏初始状态
```
###### 4.手柄菜单
```
（1）_VSVRSDK -》 _Bundle_Prefab -》_CustomSceneMenu 拖入到场景中后鼠标右击选择 unpack prefab completely
（2）在_CustomSceneMenu 的 HomeVRMenuController 上设置按钮出现和隐藏的方式
（3）按钮 name ： change_A_B_C_D	含义为 ：点击按钮时向其他用户广播 A B C D 消息
（4）按钮 name： placeto_G1 含义为 ： 点击按钮时所有用户移动到 G1位置组上
```
###### 5.Unity UGUI 按钮与交互
```
在 UGUI 的canvas 上添加 VRUISelectorProxy 脚本， 场景运行时 脚本会自动为子物体添加射线交互事件，若按钮的宽高是由其父物体设置的，则需要手动为按钮添加合适的boxcollider
```
