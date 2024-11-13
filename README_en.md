## I. Environment Setup
### 1. Development Environment
Unity Version: Use **Unity2021.3.23**

### 2. VSVRSDK Environment Setup
###### 2.1.1 Register, log in, and check the project locally or download the zip package directly:
> https://gitee.com/vswork_admin/vsvrsdk

or

> https://github.com/VSWORK-admin/vsvrsdk

## II. Unity3D Settings for Different Platforms:

Android:
```
## Build Setting ->
Texture Compression: ASTC

## PlayerSetting -> OtherSettings:
Color Space: Linear
AutoGraphics API: true
Graphics APIs: OpenGLES3 + Vulkan
Lightmap Encoding: High Quality
Lightmap Streaming Enabled: True
Allow 'unsafe' Code: True

## PlayerSetting -> XR Settings:
Virtual Reality Supported: True
VirtualReality SDKs: None
Stereo Rendering Mode: SinglePass
```

iOS:
```
## PlayerSetting -> OtherSettings:
Color Space: Linear
Auto Graphics API: False
Graphics APIs: Metal
Lightmap Encoding: High Quality
Lightmap Streaming Enabled: True
Allow 'unsafe' Code: True

## PlayerSetting -> XR Settings:
Virtual Reality Supported: False
```

Windows:
```
## Build Setting ->
Target Platform: Windows
Architecture: x86_64

## PlayerSetting -> OtherSettings:
Color Space: Linear
AutoGraphics API for Windows: True
Lightmap Encoding: High Quality
Lightmap Streaming Enabled: True
Allow 'unsafe' Code: True

## PlayerSetting -> XR Settings:
Virtual Reality Supported: True
VirtualReality SDKs: None
Stereo Rendering Mode: SinglePass
```

macOS:
```
## Build Setting ->
Target Platform: Mac OS X

## PlayerSetting -> OtherSettings:
Color Space: Linear
AutoGraphics API for Mac: True
Lightmap Encoding: High Quality
Lightmap Streaming Enabled: True
Allow 'unsafe' Code: True

## PlayerSetting -> XR Settings:
Virtual Reality Supported: False
```

## III. Scene Settings
###### 3.1 Use URP rendering pipeline in the scene.
###### 3.2 Use URP shaders for materials in the scene; avoid using other shaders.
###### 3.3 Minimize the use of real-time lighting; add Light Probe Groups to provide lighting for dynamic objects.
###### 3.4 Minimize the use of normal maps; avoid oversized textures to prevent wasting rendering resources.
###### 3.5 Set images and lightmaps to the lowest resolution before packaging; use ASTC 6x6 compression format for Android and iOS.

## IV. VR Settings
###### 4.1 Position Points, Position Groups, Position Meshes
```
(1) _VSVRSDK -> _Bundle_Prefab -> StartPointMarks -> _StartPoint Drag into the scene, then right-click and select unpack prefab completely.
(2) Add a mesh collider or box collider to the plane where free movement is allowed, then add the VRPlayceMeshMark script to the object.
```

###### 4.2 Unity UGUI Buttons and Interaction
```
Add the VRUISelectorProxy script to the canvas in UGUI. The script will automatically add ray interaction events to child objects when the scene runs. If the button's width and height are set by its parent object, manually add an appropriate box collider to the button.
```

## V. Interactive Programming
#### 5.1 Visual Programming: Use ```Playmaker``` + ```VSVR Playmaker VRActions```
#### 5.2 Code Programming: Use ILRuntime C# scripts. Refer to the vsvrdll example project DllProject/Click_show_hideDemo for instructions:
> https://gitee.com/vswork_admin/vsvrsdk/tree/master/DllProject/Click_show_hideDemo

## VI. Scene Packaging
###### 6.1 Package Naming
> Set the desired scenes as AssetBundles: assign a bundle name and suffix (scene), or rename the file suffix to .scene after packaging.

###### 6.2 Resource Check
> Open Window -> AssetBundle Browser, sort by Size, and check to avoid redundant or oversized files (especially large images). Use external image editing software to change image formats as needed: PNG for images with alpha channels, JPG for those without.

###### 6.3 Platform Selection for Packaging

- For Android platform scenes, set BuildTarget in AssetBundle Browser to **Android**
- For iOS platform scenes, set BuildTarget to **iOS**
- For Windows platform scenes, set BuildTarget to **Standalone Windows**
- For macOS platform scenes, set BuildTarget to **Standalone OSX Universal**

**Enable Unity's CacheServer: Edit -> Preferences -> Cache Server -> Cache Server Mode to Local to avoid resource duplication during platform switching and save packaging time for different platform asset bundles in the same project.**

###### 6.4 Multi-user Testing and Usage
> Before uploading to the resource management backend, set filename prefixes for bundle files required by different platforms:
```
# URP Rendering Pipeline:
    Android: a_  (Covers devices like: Oculus quest, pico neo2, pico G2, baidu VR, IQUT VR, HTC Vive focus, Android phones)
    iOS: i_  (Covers devices like: iPhone, iPad)
    Windows: w_  (Covers devices like: Oculus Rift S, HTC VIVE, WMR, Windows desktops)
    macOS: m_  (Covers devices like: macOS desktops)

# Unity HDRP Rendering Pipeline:
    Android: b_ (Covers devices like: WindowsHDRP)

For example, if you need to package for Android and the standard rendering pipeline, the resource package name would be ceshi.scene. Then upload the resource package to the resource manager backend with the prefix as: a_ceshi.scene.

If multiple devices need to load simultaneously, package each with the same file name but add the corresponding prefix, then place them in the same directory, e.g., a_ceshi.scene, i_ceshi.scene, w_ceshi.scene, m_ceshi.scene.
```