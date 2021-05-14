## 1、Environment setup
### 1. Development Environment
#### 1.1 unity version: use **unity2019.2.16** version

### 2. VSVRSDK Environment setup

#### 2.1 Register and log in gitee.com ,  use Git or svn checkout the sdk project
> https://gitee.com/vswork_admin/vsvrsdk

or

> https://github.com/VSWORK-admin/vsvrsdk

## 2、Settings for Different Platform in Unity3d：

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

## 3、Scene setting
###### 3.1. Use the LWRP rendering pipeline in the scene
###### 3.2. The materials Shader in the scene use Lightweight Render Pipeline Shader 
###### 3.3. Try not to use real-time lights in the scene. The Light Probe Group needs to be added to the scene to provide light environment for dynamic objects.
###### 3.4. Use normal maps as little as possible in the scene, and use image sizes to avoid excessively large rendering resources

## 4、VR Setting
###### 4.1.Location point, location group, location surface
```
（1）Drag _VSVRSDK -》 _Bundle_Prefab -》 PlayceMarks -》_Playces	 into the scene and right click to select unpack prefab completely
（2）Set the initial position group on the WSPlayceController of the _Playces object，
（3）Set the position points contained in the position group on the VRPlayceGroup of the position group object
（4）Add meshcollider or boxcollider on a plane that allows free walking, and then add VRPlayceMeshMark script to the object.
If the object is added to the child object of the _Playces object, the object will be hidden by default and will only appear when the position moves.
```
###### 4.2.Third-person camera
```
(1) Drag _VSVRSDK-》_Bundle_Prefab-》CameraMark-》 _CameraScreens  into the scene and right-click to select unpack prefab completely
(2) Set the initial third-person view camera and other cameras on the PCScreenCameraSceneController of _CameraScreens,
The camera will appear when the host clicks "Switch Camera", and you can use the laser pointer to select the camera. You can also use SetVRScreenCamera in VRAction to switch cameras.
```
###### 4.3.Bigscreen
```
(1) Drag  _VSVRSDK-》 _Bundle_Prefab-》Bigscreen -》 _BigScreens into the scene and right-click to select unpack prefab completely
(2) Set the initial position of the large screen and the initial state of the large screen on the WsBigScreenController of _BigScreens
```
###### 4.4.Custom Scene Menu
```
(1)  Drag _VSVRSDK-》_Bundle_Prefab-》_CustomSceneMenu into the scene and right-click to select unpack prefab completely
(2) Set the way the buttons appear and hide on the HomeVRMenuController of _CustomSceneMenu
(3) Button name: change_A_B_C_D means: broadcast A B C D message to other users when the button is clicked
(4) Button name: placeto_G1 means: when the button is clicked, all users move to the G1 location group
```
###### 4.5.Unity UGUI button and action
```
Add the VRUISelectorProxy script to the canvas of UGUI. When the scene is running, the script will automatically add ray interaction events for the child objects. If the width and height of the button are set by its parent, you need to manually add a suitable boxcollider to the button
```

## 5、Interactive programming
#### 5.1 Visual programming: Use ```Playmaker``` + ```VSVR Plamaker VRActions```
#### 5.2 Code programming: Use ILruntime C# Script. For detailed instructions, please refer to the vsvrdll example project or **DllProject/Click_show_hideDemo** in this project:
> https://gitee.com/vswork_admin/vsvrdll

or

> https://github.com/VSWORK-admin/vsvrdll


## 6、Scene packaging
###### 6.1. Package name setting
> Set AssetBundle for the scene that needs to be packaged ：et the name and suffix of the bundle (scene), or change the file suffix to .scene after packaging
###### 6.2. Resource check
> Open Window -> AssetBundle Browser to sort by Size, check to avoid redundant files and too large files (mainly too large images, you need to use external image editing software to change the image format, if there is an Alpha channel, it is recommended to change to png format, If there is no Alpha channel, it is recommended to change to jpg format)
###### 6.3. Packaging platform selection
- The scene file required by the Android platform needs to be set to **Android** on the Build Target of AssetBundle Browser.

- The running scene of the iOS platform needs to be set to **iOS**.

- The running scene of the Window platform needs to be set to **Standalone Windows**.

- The running scene of the MacOS platform needs to be set to **Standalone OSX Universal**.

**It is recommended to open Unity's CacheServer: Edit -> Preferences -> CacheServer -> Cache Server Mode and select Local to avoid repeated loading of resources caused by platform switching, and save the time for assetbundles of different platforms to be packaged in the same project.**
###### 6.4. Multi-person debugging and use
> Before uploading to the resource management background, you need to set the file name prefix for the bundle files required by different platforms
```
# LWRP rendering pipeline:
     Android: a_ (covered devices include: Oculus quest, pico neo2, pico G2 ，baidu VR ，IQUT VR, HTC Vive focus, Android mobile terminal)
     iOS: i_ (covered devices include: iPhone ， iPAD)
     Windows: w_ (covered devices include: Oculus Rift S, HTC VIVE, WMR, Window desktop)
     MAC OS: m_ (covered devices include: Mac OS desktop)
# Unity Standard rendering pipeline:
     Android: b_ (covered devices include: Huawei VR glasses)

For example: If you need to package bundle for Huawei VR glasses, you need to set the scene in the Standard rendering pipeline. The packaged resource package name is ceshi.scene, and the resource package name that needs to be uploaded to the resource manager backend must be prefixed with: b_ceshi.scene
If multiple devices need to be loaded at the same time, they need to be packaged and named with the same file name, added with the corresponding prefix, and placed in the same file directory. Such as: a_ceshi.scene b_ceshi.scene w_ceshi.scene
```


## 7、VSVR supports 3D GLB format Rule description

> VSVR supports direct loading of GLB models, which can be uploaded to the VSVR background VR resource manager, and can be watched by multiple people in VR in real time. And rely on the VSVR SDK to perform personalized control operations on the GLB model. In the Glb_LoadAndControl_Example example, the manual dismantling of the GLB model by multiple people is completed, the laser pointer is picked up, the object is rotated, the distance of the object, the dynamic marking and other operations are completed.

#### 7.1 glb format export:
##### 7.1.1 Support 3DMAX, MAYA, C4D, Blender, Sketchup and other software, you need to install the glb format export plug-in, it is recommended to export the glb model through blender.
##### 7.1.2 Support models within ```500,000``` faces (triangular faces), the size of the texture does not exceed ```2048*2048```, and the number of textures does not exceed ```20```
#### 7.2. File and model name rules
##### 7.2.1 File Naming

* Allow repeated loading:
     >Repeated loading is allowed by default (objects that allow repeated loading are allowed to be controlled by the handle by default
  , If the movement of the object is blocked, add ``` _mc_ ``` mark to the object name)
* Duplicate loading is not allowed:
     >Add ```_s``` in the name (after adding ```_s```, the objects in the scene cannot be controlled by default.
     If you need to control some of the objects, you need to add ``` _c_``` mark to the objects), such as: ```test_s.glb```

##### 7.2.2 Object Naming

* Meshcollier and placemark are added to objects by default to support moving characters to objects
* Do not add meshcollider mark (laser pointer can pass through): ```_mm_```
* Do not add placemark mark (the character cannot move to the object): ```_mt_```
* Add controllable objects (allowing automatic loading of objects will automatically add controllable objects, objects in glb files with the suffix ```_s``` are not controllable by default): ```_c_```
* Object can not be grasped control mark: ```_mc_```
* Add dynamic rigid body (attribute: allow dynamic rotation ```_r_```): ```_p_```
* Hide the object messrender to make the object invisible: ```_mr_```
* Load 0 points (spawn point): ```_zero_```
* Scale mark: ```_scale_```

## 8. VSVR supports flat format. Rule description
#### 8.1 Picture format
##### 8.1.1 Flat picture
The recommended format for large-screen demo images is ```jpg```, and the image size is recommended to be in the range of ```1280*720 - 1600*900```
##### 8.1.2 Panorama picture
The recommended format for panoramic pictures is ```jpg```, and the picture size is recommended to be in the range of ```4096*2048 - 6000*3000```
#### 8.2 Video format
Recommended format for flat video : ```mp4``` Encoding: ```H.264``` The bit rate is recommended to be controlled in the range of ```4Mbps-6Mbps```
Recommended format for panoramic video: ```mp4``` Encoding: ```H.264``` The bit rate is recommended to be controlled in the range of ```10Mbps-20Mbps```
#### 8.3 Video stream format
Video stream format supports ```rtmp``` ```rtsp``` ```hls``` protocol format video stream
Write the video streaming address into a txt file, change the file suffix of ```.txt``` to ```.order``` and upload to the explorer, click to play
#### 8.4 Video on-demand format
Video on demand url support encoding ```h.264``` ```h.265``` format url on demand
Write the video-on-demand url address into a txt file, change the suffix of ```.txt``` to ```.order``` and upload to the resource manager, click to play


## 9、 VSVRSDK VRAction description
#### EventAdminChange ```Event```: detect the host change event
>Usage example: Used to cooperate with switching some objects that only the host can see or operate, the operation panel automatically disappears after the host switches, etc.
```
M Enabled: Whether the script is enabled
AdminTrue: Switch yourself to the moderator
adminFalse: Support switching to non-host
```
#### EventCustomVRInput ```Event```: Detect VR Input event
>Usage example: handle button, helmet removal and other events, such as using the joystick to control a certain UI to switch to the next page, the background music sound becomes smaller after the helmet is removed, and the handle grips moving objects, etc.
```
InputType: VR Input event to be detected
Input Event: The detected VR Input event is triggered and executed
Rcieved 2D Axis: Return to the 2D position of the joystick when the joystick is changed
Rcieved 1D Axis: Hold key and trigger key to return to 1D position when pressed
```
#### EventCustomVRInputRouter ```Event```: VR Input event routing
>Usage example: handle button, helmet on and off events, such as using the joystick to control a certain UI to switch to the next page, the background music sound becomes smaller after the helmet is removed, and the handle grips moving objects, etc.
```
InputType: VR Input event to be detected

Message Object: The object used to receive information
Fsm 2D Axis Name: The variable name that returns to the 2D position of the joystick when the joystick is changed
Fsm 1D Axis Name: The name of the variable that returns to the 1D position when the hold key and trigger key are pressed
Fsm Event Name: The name of the trigger event
```

#### EventPlayerPlaceChange ```Event```: own character moves to a location point or location surface
>Use case: the door opens automatically when the character moves to a certain position


#### EventRoomConnectionChange ```Event```: Detect room login status event
> Example of use: UI prompts when the room is disconnected or reconnected
#### EventSelectAvatarWsid ```Event```: select a character
>Usage example: When the laser pointer selects a role, it triggers an event and returns the role ID. The laser pointer can click on a role name tag to move the role to the front. You need to send information to other roles, and execute by their respective IDs. event.
```
Select ID: selected role ID
Event SelectID: Event is triggered when a role is selected
```
#### EventSelectorPointerStatusChange ```Event```: Laser pointer status change
> Example of use: when the laser pointer is displayed, some clickable objects are displayed, and when the laser pointer is turned off, these clickable objects are hidden


#### EventTeleportStatusChange ```Event```: mobile status switch
> Example of use: When each character starts to move with position (press the joystick), some objects hide or appear


#### EventVRLaserChange ```Event```: When the laser pointer moves to an object or leaves an object or clicks the confirmation button on an object to obtain the changed object
```
Point Event Type: Choose to monitor Enter, Exit or click events
PointEvent: Event triggered
Pointed Obj: Returns the object of the event currently monitored by the laser pointer
```
#### EventVRSystemMenuChange ```Events```: system menu and scene menu call out and close events
>Usage example: When the system menu is called out, an object is automatically hidden, and when the scene menu is called out, the controllable object mark appears automatically


#### GetAdminStatus ```Get```: Determine whether you are currently the moderator.
#### GetAvatarSort ```Get```: Get the sort of all characters in the current room.
#### GetKODToLocalCacheFile ```Get```: Cache remote files of KOD service to cache file
>Usage example: cache a file in the KOD server to the local, and return the local path for other modules to use
```
KOD Url: Relative path on the KOD server IsURLSign: Whether to control the version through the network url file
HasPrefix: Whether to add the platform prefix according to its own platform, Android add "a" windows add "w"
GetLocalPath: Cached file success event
GetLocalPathFaild: Cache file failure event
Geted Sign: Get the signature
LocalPath: local file path after successful caching
LocalUrl: URL path after successful caching
```
#### GetPathToScene ```Get```: Cache the local file path to the scene
>Use example: load scene with GetKODToLocalCacheFile
```
Path: The local path of the scene file
Sign: Sign the scene file
SceneName: the name of the scene
```
#### GetPathToTexture ```Get``` ：Cache local file path to Texture
>Usage example: Cooperate with 8 GetKODToLocalCacheFile to load remote picture materials
```
Path: The local path of the image file
EventGetTexture: Get the picture success event
EventGetTextureFail: the event of failure to get the picture
Geted Texture: Texture obtained
```
#### GetUrlToLocalCacheFile ```Get```: Serving remote files from remote servers to cache files via URL cache
> Example of use: Cache an Internet file
```
HttpUrl: The path address where the file is located
IsURLSign: Whether to use the content of the http txt file as the version mark
Sign: Signature (version) or http signature file path
HasPrefix: Whether to automatically obtain the corresponding scene file according to the device, such as "a_xx.xx" "w_xx.xx"
GetLocalPath: Get the file successfully
GetLocalPathFaild: Failed to get the file
GetedSign: Get the signature
LocalPath: The local path of the cached file
LocalUrl: The local url of the cached file (`File: //`+ local path)
```

#### GetUrlToLoadScene ```Get```: Serving scenes in remote servers through URL cache
>Use example: Load another url link scene in the scene
```
HttpUrl: The path address where the scene file is located
IsURLSign: Whether to use the content of the http txt file as the version mark
Sign: Signature (version) or http signature file path
SceneName: scene name
HasPrefix: Whether to automatically obtain the corresponding scene file according to the device, such as "a_xx.scene" "w_xx.scene"
GetLocalPath: Get the scene successfully
GetLocalPathFaild: Failed to get the scene
```

#### GetVRGameObjects ```Get```: Get the relevant parameters in the VR scene
>Usage example: Get the position of the left and right hand handles, get your own nickname, etc. to complete functions with other modules
````
Everyframe: Update the acquired information every frame
Maincamera: Returns the gameobject of the current main camera
LeftHand: Return the left hand gameobject
RightHand: Return the right hand gameobjet
LeftTeleportAnchor: Returns the initial position object of the Teleport ray of the left hand handle
RightTeleportAnchor: Returns the initial position object of the Teleport ray of the right hand handle
leftFingerPointerAnchor: return to the initial position of the left-hand handle laser pointer
RightFingrerPointerAnchor: Return to the initial position of the right-hand handle laser pointer
MainVRROOT: Return the Root object of the character
LaserPoint: return the object gameobject of the laser pointer point
GlbRoot: Returns the total parent object of the Glb scene and the Glb object
GlbSceneRoot: Returns the parent object of the Glb scene (the object with the suffix _s added to the loaded glb file)
GlbOjbRoot: Returns the parent object of the Glb object (the object whose suffix _s is not added to the loaded glb file)
BigscreenRoot: return to the big screen gamobject
Msex: Go back to the current character sex 0 1
mWsID: return the current role network connection ID
mAvatarID: return to the current character ID
MnickName: Return the nickname of the current role
GMEconnected: Return to voice connection status
NowMicVol: Return the volume of the current voice microphone
MicEnabled: Returns whether the current Mic is disabled
NowSelectedAvatarWsid: Returns the network connection ID of the currently selected character
nowUnityVersion: returns the current Unity version
isVRApp: Returns whether the VR is connected through the helmet
ismobile: Returns whether it is a mobile device access
AutoPlayScene: Returns whether the scene is in auto play mode
Startaid: Get your own default role id
MenuAnchor: Get the system menu anchor object
isadmin sadmin: Get whether the user is the moderator
isDrawingenabled: Get whether the own brush is enabled
````
#### GetVRLanguage ```Get``` ： Get Current Language
>Usage example: Detect language and set scene panel for that language
```
LanguageSort ： Get the current language sort   0： English  1：Chinese 3:Korean 4:Japanese
LanguageString ： Get the current language string   English  Chinese Korean Japanese 

```

#### RecieveBigScreenTexture ```Get on the Internet```: get the picture on the big screen
>Usage example: After the resource library obtains the panoramic picture, the 360 ​​picture changes
```
WsTexture: image resource obtained or cached
RecieveTextureEvent: Play new image events on the big screen
WsTexturePaht: the path of the obtained big screen picture
UpdateTextureEvent: Resource manager cached image success event
```
#### RecieveBigScreenVideoPlayer ```Get from the network```: The big screen gets the video frame. Example: When the big screen is playing the video, it will update the video frame texture to the three-dimensional object.

#### RecieveCustomWsMessages ```Get on the network```: Get the network synchronization information
>Usage example: All role synchronization events are obtained through this module
```
Bypass: Ignore network acquisition and trigger Recieve event
Recieve: Get any network information trigger event
AbcdefgFlow: The received ABCDEFG information is processed in order
Ws_a-Ws_g: A information to be compared
RecieveSame_a-RecieveSame_g receives the same information as Ws_a-Ws_g to trigger an event
A-G: Store the received A-G message text
```
#### RecieveWsPlaceMark ```Network acquisition``` ：Get location change information
>Usage example: When moving to a certain position or group of positions, some objects are displayed, when moving to a certain group of positions, the third angle camera switches to the corresponding angle of view
````
WsGroupName: The name of the location group to be compared
WsMyPlaceName: The name of the location to be compared
RecieveSameGroupName: Received the same location group name trigger event as WsGroupName
RecieveMysamePlaceaName: Receiving the name of the same location as WsMyPlaceName triggers the event
RecievedGroupName: Store the name of the received location group
RecievedPlaceName: Store the name of the received place
````
#### SendCommitOrder ```Network Send``` ：Send debugging password
#### SendCustomWsMessages ```Network Sending```: Sending network synchronization information Example of use: Sending synchronization information to all other roles (including oneself) ABCDEFG
#### SendWsAllInfoLog ```Network Send```: Send Log information to other roles
#### SendWsAllPlayceTo ```Send to the network```: Send the move position group command to other characters (including itself) Usage example: All characters move to a certain position group
```
GroupName: Location group name
ToALl: whether to send to everyone
```
#### SetCustomVRInput ```Settings```: Set VRInput command
>Usage example: Handle vibration reset position (valid on Windows, invalid on Quest)
```
Input Type: instruction type
Vibinfo Hand: Choose handle vibration
Frequency: Vibration frequency 0-1
Amplitude: Vibration intensity 0-1
Lasttime: duration 0-4
```
#### SetHandModelEnabled ```Settings```: Set whether to hide the handle model. Example: Hide the handle model when picking up the tool, and display the handle model when putting down the tool
#### SetLoadUrlIdScene ```Settings```: Load the scene on the remote server
```
Server: remote server address ID: scene ID
IsNowServer: Whether to use the currently logged in remote server
Update: Whether to only update the scene without loading
```
#### SetMaterialTextrueEveryFrame ```Settings```: Change the texture for the object every frame, and cooperate with RecieveBigScreenVideoPlayer to update the texture of the video playback to the object
#### SetVRBigScreen ```Set```: Replace the big screen to a certain position
#### SetVRInputField ```Settings```: Set the InputField of a UI as input state
#### SetVRControllingObj ```Settings```: Set an object to control and frame synchronization state
```
Controlling Obj: Objects that need to be set
Islocal: whether it is a world location or its own location
Is Sending: Whether to send the location information of this object
TriiggerStart: start sending position information
TriggerEnd: End sending location information
TriggerEndEvent: End sending location information trigger event
FrameSend: send frame rate 1: send at the highest frame rate 2: send at half the frame rate 3: send at one-third frame rate 4: send at one-quarter frame rate
ControllStart: Start position information received
Controlling: position information received
ControllEnd: End position information received
```
#### SetVRMovingObj ```Settings```: Set an object to frame synchronization state
>Usage example: After the handle picks up an object, it needs to be set as MovingObj to synchronize to other characters
```
MovingObj: Objects that need to be set
Islocal: Whether it is the world position or its own position
IsSending: Whether to send the location information of this object
FrameSend: send frame rate 1: send at the highest frame rate 2: send at half the frame rate 3: send at one-third frame rate 4: send at one-quarter frame rate
```

#### SetVRScreenCamera ```Settings```: Set the position of the third-view camera
>Use example: When moving to a certain position group, switch the corresponding third-view camera, (drag the Prefab of the third-view camera to CameraMark)


#### SetVRGameObject ```Settings```: Set your own VR information
```
Aid: Set the role ID 1. The field can be set to the string id provided by the backend 2. The field can be set to the full path of the glb role starting with http and add configuration parameters after glb
     The parameter format is: ?info="glb role type"_"version information"_"head type"
     Glb role type: 1: half-length role 2: full-body role t: any glb model
     Version information: Fill in a different version string and the role will be cached again
     Head type: 0: glb zero point is on the ground 1: glb zero point is on the head, but only rotates on the y axis 2: glb zero point is on the head, and the rotation completely follows the head
     Leave blank if not changed
Name: Change the name of the head brand name, if you don’t change it, leave it blank
Hideavatar: Whether to hide the character
SetColor: Whether to change the color of the pen and laser pointer
Pencolor: the color of the pen
```

#### SendChangeAvatar ```Send on the Internet```: Send your own VR information to the server, used in conjunction with SetVRGameObject
#### SetVRObjLoadPosition ```Settings```: Set the position of glb scene and glb object loading
#### SetVRSystemMenuEnable ```Settings```: Set the display and hiding of the system menu
#### SetVRVideoPlayer ```Settings```: Set a video player
```
ContorlObj: Used to set the control object of the player. After the setting is completed, you can control the playback of the player through SendMessage:
Play: "Play", stop: "Stop", pause: "Pause", prepare: "Prepare"
RenderObj: Array fill in the number of Array first, and then drag in the objects that need to be rendered. The objects can be UI RawTextrue or 3D objects made of Unlit-》Texture. Multiple objects can be set at once
Url: string video stream address, or remote video address or local path address of the cached video (non-url address)
Vol: Float sound size range 0-100
Isloop: bool whether to play in a loop (if it is a video stream, uncheck this item)
Autostart: bool whether to play automatically (if you don’t check auto play, the video will only be prepared by Prepare, you can use SendMessage to ContorlObj to control playback)
```
#### SetChanelChRoomID ```Settings```: Set an action channel
```
ChRoomID: Set a roomid with 2-4 lowercase letters, such as: "aa", "abcd", if this field is empty, it will enter the initial roomid
```
#### SetVoiceExRoomID ```Settings```: Set a voice channel
```
ExRoomID: Set an ExRoomID with 2-4 lowercase letters, such as: "aa", "abcd", if this field is empty, it will enter the initial voice roomid
```
#### SetVoiceRoomExit ```Settings```: Exit the voice channel
```
ExRoomID: Set an ExRoomID with 2-4 lowercase letters, such as: "aa", "abcd", if this field is empty, it will enter the initial voice roomid
```

#### SetRootChanelChRoomID ```Settings```: Set to enter a new Root channel
```
RootRoomID: Set the RootRoomID provided by the background
RootVoiceID: Set the RootVoiceID provided by the background
```