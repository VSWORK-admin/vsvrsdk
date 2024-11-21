## 说明文档 ##

### 一. 用途
>工程为VSVR SDK拓展工程，用于生成VSVR交互SDK所需的bytes字节码。

>编写VSVR交互逻辑时将此工程check到本地，并在此工程编写VSVR C# 交互代码，建议使用VisualStudio编写。

>VSVRDevTool 下载：https://oss.vswork.vip/Files/vsvr/2.7.3/VSVR-DevTools_v2.7.3.zip
>场景包加密工具 下载 ：https://oss.vswork.vip/Files/vsvr/VSVRCryptTool1.0.zip
### 二. 使用步骤
#### 2.1 交互逻辑代码编写
###### 2.1.1 创建脚本文件
在```/Dll_Project/```文件目录下创建```*.cs```文件，文件命名于class命名相同，如工程示例中的```ClickDemo.cs``` (**位置在 vsvrsdk 工程/Assets/Scenes/ILruntimeSample/**)

###### 2.1.2 继承类
代码中不能直接继承MonoBehaviour类必须继承DllGenerateBase包装类作为替代

#### 2.2 生成byte字节码
###### 2.2.1 设置生成事件
在VScode Dll_Project.csproj  在生成以后 执行的命令行配置：
``` 
<ItemGroup>
    <!-- 定义需要复制的文件 -->
    <None Update="$(OutputPath)$(AssemblyName).dll">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="$(OutputPath)$(AssemblyName).pdb">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>

  <Target Name="PostBuild" AfterTargets="Build">
    <!-- 将 .dll 文件复制到输出目录，并更改文件扩展名为 .dll.bytes -->
    <Copy SourceFiles="$(OutputPath)$(AssemblyName).dll"
          DestinationFiles="../../Assets/Scenes/ILruntimeSample/$(AssemblyName).dll.bytes" />
          
    <!-- 将 .pdb 文件复制到输出目录，并更改文件扩展名为 .pdb.bytes -->
    <Copy SourceFiles="$(OutputPath)$(AssemblyName).pdb"
          DestinationFiles="../../Assets/Scenes/ILruntimeSample/$(AssemblyName).pdb.bytes" />
  </Target>

```

###### 2.2.2 生成字节码
在Vscode 中 的 Terminal 中执行 ```dotnet build``` 命令，生成字节码。生成后会自动将dll 和pdb 文件拷贝到Unity工程中。可以自行在Dll_Project.csproj中配置Unity功能内的路径。


#### 2.3 Unity工程配置
###### 2.3.1 配置加载字节码
将```DllManager.cs```组件拖入SDK场景任意物体中(**一个场景中只能存在一个```DllManager.cs```组件**),然后将```Dll_Project.dll.bytes```,```Dll_Project.pdb.bytes```分别拖入```DllAsset```,```PdbAsset```中

###### 2.3.2 配置交互脚本
将```GeneralDllBehaviorAdapter```组件托入SDK场景任意物体中,```ScriptClassName```中填写包装类名(如果有命名空间,填写时也必须包含命名空间名称如```Dll_Project.ClickDemo```,如果不包含命名空间则直接填写类名，如```ClickDemo```),这个类就可以像平时写unity的MonoBehaviour类一样了。



#### 2.4  代码编写

获取Unity中的Extral Datas、 Extral Datas  Objs 等在Unity Inspector 中配置的数据：

在Init方法中获取：通过BaseMono 获取到的ExtralDatas 数组中的第一个元素的Target属性，即为Extral Datas 中配置的对象。
```

        public Transform targetTransform; //自定义的目标Transform对象

        // 重写Init方法，进行初始化操作
        public override void Init()
        {
            targetTransform = BaseMono.ExtralDatas[0].Target;//获取unity中Inspector 窗口中拖入的物体对象
            Debug.Log("Click_Demo Init !");
        }
```


#### 2.5  Debug 断点调试

在VScode 中 安装  ILRuntime Debug 插件，插件地址：https://marketplace.visualstudio.com/items?itemName=liiir.ilruntime-debug

创建launch.json 文件，文件内容如下：
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
在VSVRDevTool进入场景后 开启调试模式，可以打断点进行调试。