## 一、环境搭建
### 1. 开发环境
unity版本： 使用 **unity2021.3.23**  版本
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
AutoGraphics API : true
GraphicsAPIs :  OpenGLES3 + Vulkan
LightmapEncoding : High Quality 
LightmapStreaming Enabled : True 
Allow 'unsafe' Code : True

## PlayerSetting-> XR Settings:
Virtual Reality Supported : True
VirtualRealitySDKs : none
Stereo Rendering Mode : SinglePass
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
###### 3.1. 场景内使用URP渲染管线
###### 3.2. 场景内的材质使用  URP 中的Shader，不要使用其他Shader
###### 3.3. 场景内尽量不使用实时灯光，场景中需要添加 Light Probe Group （光照探针）用来给动态物体提供光环境
###### 3.4. 场景内尽量少使用法线贴图，使用的图片大小尺寸避免过大造成渲染资源浪费
###### 3.5. 场景内的图片和光照贴图尽可能的在打包之前设置到最小分辨率，Android 和 iOS 设置为 ASTC 6x6 压缩格式

## 四、VR设置
###### 4.1.位置点 、位置组 、位置面
```
（1）_VSVRSDK -》 _Bundle_Prefab -》 _StartPoint	拖入到场景中设置角色出生点
（2）在允许自由走动的平面上 添加 meshcollider 或者 boxcollider 然后添加 VRPlayceMeshMark 脚本到该物体上。
```

###### 4.2.Unity UGUI 按钮与交互
```
在 UGUI 的canvas 上添加 VRUISelectorProxy 脚本， 场景运行时 脚本会自动为子物体添加射线交互事件，若按钮的宽高是由其父物体设置的，则需要手动为按钮添加合适的boxcollider
```

## 五、交互编程
#### 5.1 可视化编程 ： 使用```Playmaker``` + ```VSVR Plamaker VRActions```
#### 5.2 代码编程 ： 使用ILruntime C# 脚本，使用说明详见 vsvrdll 示例工程  DllProject/Click_show_hideDemo:
> https://gitee.com/vswork_admin/vsvrsdk/tree/master/DllProject/Click_show_hideDemo

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
# URP 渲染管线 :  
    安卓 : a_  (覆盖设备包括 ：Oculus quest， pico neo2， pico G2 ，baidu VR ，IQUT VR, HTC Vive focus ， 安卓手机端)
    iOS : i_  （覆盖设备包括 ：iphone ，ipad） 
    Windows : w_  （覆盖设备包括 ：Oculus Rift S , HTC VIVE， WMR ，Window 桌面端） 
    MAC OS : m_  （覆盖设备包括 ：Mac OS 桌面端）
# Unity HDRP 渲染管线 ：
    安卓 ： b_ (覆盖设备包括 ： WindowsHDRP)

例如 ： 如果需要安卓打包，则需要在Standard 渲染管线设置场景，打包资源包名称 为  ceshi.scene, 则需要上传至资源管理器后台的资源包名称需加上前缀为 ： a_ceshi.scene
如多种设备需要同时加载，则需要各自打包命名成相同文件名称，并加入对应前缀，放置到同一文件目录下即可。 如 ： a_ceshi.scene   i_ceshi.scene   w_ceshi.scene   m_ceshi.scene
```