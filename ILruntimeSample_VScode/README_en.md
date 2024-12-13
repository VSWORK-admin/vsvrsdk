## Documentation ##

### 1. Purpose
>The project is an extension project for the VSVR SDK, used to generate the bytecode required by the VSVR interaction SDK.

>When writing VSVR interaction logic, check this project locally and write the VSVR C# interaction code in this project. 

>Download VSVRDevTool: https://oss.vswork.vip/Files/vsvr/2.7.3/VSVRDevTools_Release_2.7.3.6.zip

>Download Scene Package Encryption Tool: https://oss.vswork.vip/Files/vsvr/VSVRCryptTool1.0.zip

### 2. Usage Steps
#### 2.1 Writing Interaction Logic Code
###### 2.1.1 Creating Script Files
Create a `*.cs` file in the `/Dll_Project/` directory, and name the file the same as the class name, such as `ClickDemo.cs` in the project example (**located in vsvrsdk Project/Assets/Scenes/ILruntimeSample/**).

###### 2.1.2 Inheriting Classes
The code cannot directly inherit from the MonoBehaviour class; it must inherit from the DllGenerateBase wrapper class instead.

#### 2.2 Generating Bytecode
###### 2.2.1 Setting Up Post-Build Event
In `VScode Dll_Project.csproj`, set up post-build commands:
``` 
<ItemGroup>
    <!-- Define the files to be copied -->
    <None Update="$(OutputPath)$(AssemblyName).dll">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="$(OutputPath)$(AssemblyName).pdb">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>

  <Target Name="PostBuild" AfterTargets="Build">
    <!-- Copy the .dll file to the output directory and change its extension to .dll.bytes -->
    <Copy SourceFiles="$(OutputPath)$(AssemblyName).dll"
          DestinationFiles="../../Assets/Scenes/ILruntimeSample/$(AssemblyName).dll.bytes" />
          
    <!-- Copy the .pdb file to the output directory and change its extension to .pdb.bytes -->
    <Copy SourceFiles="$(OutputPath)$(AssemblyName).pdb"
          DestinationFiles="../../Assets/Scenes/ILruntimeSample/$(AssemblyName).pdb.bytes" />
  </Target>

```

###### 2.2.2 Generating Bytecode
Run the `dotnet build` command in the Terminal of VSCode to generate the bytecode. After generation, the dll and pdb files will be automatically copied to the Unity project. You can configure the path within Unity in `Dll_Project.csproj` as needed.

#### 2.3 Unity Project Configuration
###### 2.3.1 Configuring Bytecode Loading
Drag the `DllManager.cs` component into any object in the SDK scene (**only one `DllManager.cs` component can exist in a scene**), then drag `Dll_Project.dll.bytes` and `Dll_Project.pdb.bytes` into `DllAsset` and `PdbAsset`, respectively.

###### 2.3.2 Configuring Interaction Scripts
Drag the `GeneralDllBehaviorAdapter` component into any object in the SDK scene. Fill in the `ScriptClassName` with the name of the wrapper class (if there is a namespace, it must be included, e.g., `Dll_Project.ClickDemo`; if there is no namespace, just write the class name, e.g., `ClickDemo`). This class can then be written similarly to a usual Unity MonoBehaviour class.

#### 2.4 Coding

Retrieve the Extra Datas, Extra Datas Objs configured in Unity Inspector:

In the Init method, access the objects configured in ExtraDatas through the BaseMono to get the first element's Target property in the ExtraDatas array, which corresponds to the object configured in Extra Datas.

```csharp
public Transform targetTransform; // Custom target Transform object

// Override the Init method for initialization operations
public override void Init()
{
    targetTransform = BaseMono.ExtralDatas[0].Target;// Get the object dragged in the Unity Inspector window
    Debug.Log("Click_Demo Init !");
}
```

#### 2.5 Debugging with Breakpoints

Install the ILRuntime Debug plugin in VScode. Plugin URL: https://marketplace.visualstudio.com/items?itemName=liiir.ilruntime-debug

Create a `launch.json` file with the following content:
```
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "ilruntime",
            "request": "launch",
            "name": "Attach to ILRuntime",
            "address": "127.0.0.1:56001",
            "debug": true,
            "stopOnEntry": true
        }
    ]
}
```
After entering the scene in VSVRDevTool, you can enable debug mode to set breakpoints for debugging. The port used for debugging will incrementally change from 56001 to 56100 with each run. If you need to set breakpoints, you'll need to change the corresponding port in launch.json. You can view the port number in VSVRDevTool.

#### 2.6 AI-Assisted Development

Install the Marscode plugin in VScode to assist with AI development.