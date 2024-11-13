## I. Environment Setup
### 1. Development Environment
Unity Version: Use **Unity 2021.3.23** version

### 2. VSVRSDK Environment Setup
###### 2.1.1 Register, log in, check to local or directly download the zip project package:
> https://gitee.com/vswork_admin/vsvrsdk

or

> https://github.com/VSWORK-admin/vsvrsdk

## II. Settings for Different Unity3D Platforms:

Android:
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

## III. Scene Settings
###### 3.1. Use URP rendering pipeline in the scene
###### 3.2. Use URP shaders for materials in the scene, do not use other shaders
###### 3.3. Minimize the use of real-time lights, add Light Probe Group in the scene to provide lighting environment for dynamic objects
###### 3.4. Minimize the use of normal maps, avoid large image sizes that waste rendering resources
###### 3.5. Set images and lightmaps to the smallest resolution before packaging, set Android and iOS to ASTC 6x6 compression format

## IV. VR Settings
###### 4.1. Position points, position groups, position surfaces
```
(1) Drag _VSVRSDK -> _Bundle_Prefab -> _StartPoint into the scene to set the spawn point
(2) Add a mesh collider or box collider to the surface where free movement is allowed, then add the VRPlayceMeshMark script to the object.
```

###### 4.2. Unity UGUI buttons and interactions
```
Add the VRUISelectorProxy script to the canvas of the UGUI. During scene runtime, the script will automatically add ray interaction events to child objects. If the button's width and height are set by its parent, manually add an appropriate box collider to the button.
```

## V. Scene Packaging
###### 6.1. Packaging name settings
> Set the scenes to be packaged as AssetBundles: set bundle names and suffixes (scene), or change the file suffix to .scene after packaging
###### 6.2. Resource check
> Open Window -> AssetBundle Browser, sort by Size, check for redundant or oversized files (mainly oversized images, use external image editing software to change the format; if there is an alpha channel, change to png, otherwise jpg)
###### 6.3. Choose packaging platform

- For Android platform scenes, set BuildTarget to **Android** in AssetBundle Browser

- For iOS platform, set to **iOS**

- For Windows platform, set to **Standalone Windows**

- For macOS platform, set to **Standalone OSX Universal**

**It is recommended to open Unity's CacheServer: Edit -> Preferences -> CacheServer -> Cache Server Mode, select Local to avoid resource reloading due to platform switching, saving time for packaging asset bundles in the same project for different platforms.**
###### 6.4. Collaborative debugging and usage
> Before uploading to the resource management backend, set the filename prefixes for bundle files required by different platforms
```
# URP rendering pipeline:
    Android: a_ (covers devices such as Oculus Quest, Pico Neo2, Pico G2, Baidu VR, IQUT VR, HTC Vive Focus, Android phones)
    iOS: i_ (covers devices like iPhone, iPad)
    Windows: w_ (covers devices like Oculus Rift S, HTC VIVE, WMR, Windows desktop)
    macOS: m_ (covers devices like macOS desktop)
# Unity HDRP rendering pipeline:
    Android: b_ (covers devices like WindowsHDRP)

For example: For Android packaging, set the scene in the Standard rendering pipeline, package the resource bundle named ceshi.scene, then upload the resource bundle with the prefix: a_ceshi.scene
If multiple devices need to load simultaneously, package them with the same filename but different prefixes, place them in the same directory. For instance: a_ceshi.scene, i_ceshi.scene, w_ceshi.scene, m_ceshi.scene
```