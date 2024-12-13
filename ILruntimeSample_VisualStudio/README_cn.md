## 说明文档 ##

### 一. 用途
>工程为VSVR SDK拓展工程，用于生成VSVR交互SDK所需的bytes字节码。

>编写VSVR交互逻辑时将此工程check到本地，并在此工程编写VSVR C# 交互代码，使用VisualStudio编写。

>VSVRDevTool 下载：https://oss.vswork.vip/Files/vsvr/2.7.3/VSVRDevTools_Release_2.7.3.6.zip

>场景包加密工具 下载 ：https://oss.vswork.vip/Files/vsvr/VSVRCryptTool1.0.zip
### 二. 使用步骤
#### 2.1 交互逻辑代码编写
###### 2.1.1 创建脚本文件
在```/Dll_Project/```文件目录下创建```*.cs```文件，文件命名于class命名相同，如工程示例中的```ClickDemo.cs``` (**位置在 vsvrsdk 工程/Assets/Scenes/ILruntimeSample/**)

###### 2.1.2 继承类
代码中不能直接继承MonoBehaviour类必须继承DllGenerateBase包装类作为替代

#### 2.2 生成byte字节码
###### 2.2.1 设置生成事件

在Unity工程根目录 创建 ```DllProject```  文件夹,将工程放置在此文件夹中 并将 ```.bytes``` 文件指定到 相对目录  如：
```
copy "$(TargetDir)$(ProjectName).dll" "$(SolutionDir)..\..\Assets\Scenes\ILruntimeSample\$(ProjectName).dll.bytes"

copy "$(TargetDir)$(ProjectName).pdb" "$(SolutionDir)..\..\Assets\Scenes\ILruntimeSample\$(ProjectName).pdb.bytes"
```

###### 2.2.2 生成字节码
解决方案配置 设置为 ```Release``` 

点击 生成->生成解决方案后 会自动创建 两个 ```.bytes``` 文件 分别为：
```Dll_Project.dll.bytes```  和 ```Dll_Project.pdb.bytes``` ,两个文件为unity场景所需要的交互逻辑字节码文件。

生成后事件命令行配置为Unity工程目录中的场景所在路径 后生成的 字节码文件则自动会出现在 Unity场景所在路径内,若保持默认的  ```$(ProjectDir)unitybytes``` 则需要在生成后手动将 ```unitybytes``` 中的两个字节码文件拷贝到Unity工程内的场景所在目录。



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

###### 2.5.1 安装插件
在 Visual Studio 中，进入 扩展 -> 管理扩展，搜索 "ILRuntime" 并安装 ILRuntime 调试器插件。

###### 2.5.2 开始调试

点击 调试 -> 附加到 ILRuntime，然后通过设置 RemoteHost: 127.0.0.1:5600x 来指定地址和端口，并点击附加。每次重新运行场景时，调试端口将从 56001-56100 之间变化。在运行场景后，端口号将在 VSVRDevTool 调试窗口中显示。
