## 1. Environment Setup
### 1. Development Environment
Unity Version: Use **Unity2022.3.54** version

### 2. VSVRSDK Environment Setup
###### 2.1.1 Register and log in, check out to local or directly download the zip project package
> https://gitee.com/vswork_admin/vsvrsdk

or

> https://github.com/VSWORK-admin/vsvrsdk

## 2. Unity3D Settings for Different Publishing Platforms:

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

## 3. Scene Settings
###### 3.1. Use URP rendering pipeline in the scene
###### 3.2. No other third-party plugins can be introduced within the scene, except for VRSDK. Other plugins need to be removed during packaging
###### 3.3. Use shaders from URP for materials in the scene; avoid using other shaders
###### 3.4. Avoid using real-time lighting in the scene; add Light Probe Group to provide a light environment for dynamic objects
###### 3.5. Minimize the use of normal maps in the scene; avoid oversized images which can waste rendering resources
###### 3.6. Set images and lightmaps to the smallest resolution before packaging; for Android and iOS, use ASTC 6x6 compression format

## 4. Start Point Settings
###### 4.1. Position Points, Position Groups, Position Planes
```
(1) _VSVRSDK -》 _Bundle_Prefab -》 _StartPoint Drag into the scene to set the character spawn point
(2) Add meshcollider or boxcollider to the surface where free movement is allowed, then add the VRPlayceMeshMark script to that object.
```

## 5. Scene Packaging
###### 5.1. Set Package Name
> Set the AssetBundle for the scene to be packaged: set the bundle name and suffix (scene), or change the file suffix to .scene after packaging
###### 5.2. Resource Check
> Open Window -》 AssetBundle Browser, sort by Size, and check to avoid unnecessary files and overly large files (especially large images; use external image editing software to change the image format, convert to png if there's an Alpha channel, otherwise to jpg)
###### 5.3. Select Packaging Platform

- For Android platform scene files, set BuildTarget in AssetBundle Browser to **Android**

- For iOS platform, set to **iOS**

- For Windows platform scenes, set to **Standalone Windows**

- For macOS platform, set to **Standalone OSX Universal**

**It is recommended to open Unity's CacheServer: Edit -> Preferences -> CacheServer -> Cache Server Mode, select Local to avoid resource duplication due to platform switching, saving time when packaging asset bundles for different platforms within the same project.**

## 6. Interactive Programming and Code Debugging
> Support development using VSCode or Visual Studio, development language: C#.

Sample projects and documentation (Gitee): 
- VSCode: https://gitee.com/vswork_admin/vsvrsdk/tree/master/ILruntimeSample_VScode
- Visual Studio: https://gitee.com/vswork_admin/vsvrsdk/tree/master/ILruntimeSample_VisualStudio

Sample projects and documentation (GitHub): 
- VSCode: https://github.com/VSWORK-admin/vsvrsdk/tree/master/ILruntimeSample_VScode
- Visual Studio: https://github.com/VSWORK-admin/vsvrsdk/tree/master/ILruntimeSample_VisualStudio