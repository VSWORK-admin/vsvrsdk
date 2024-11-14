## 说明文档 ##

### 一. 用途
>工程为VSVR SDK拓展工程，用于生成VSVR交互SDK所需的bytes字节码。

>编写VSVR交互逻辑时将此工程check到本地，并在此工程编写VSVR C# 交互代码，建议使用VisualStudio编写。

### 二. 使用步骤
#### 2.1 入口函数
代码 从```Dll_Project.DllMain``` 类的 ```Main()``` 函数开始执行, 对应的c#文件为 ```/Dll_Project/DllMain.cs```，入口函数中将2.4.2中配置的脚本进行初始化操作（将脚本所在gameobject启用），如示例中操作：
```
public static void Main()
{
    UnityEngine.Debug.Log("Dll Run Main !");

    foreach (var obj in DllManager.Instance.ExtralDatas)
    {
        if (obj.Target != null)
        {
            obj.Target.gameObject.SetActive(true);
        }
    }
}
```
#### 2.2 交互逻辑代码编写
###### 2.2.1 创建脚本文件
在```/Dll_Project/DllMain.cs```文件同目录下创建```*.cs```文件，文件命名于class命名相同，如工程示例中的```Click_show_hideDemo``` (**位置在 vsvrsdk 工程/Assets/Scenes/ILruntime_Example/Click_show_hide_Objs_Dll**)

###### 2.2.2 继承类
代码中不能直接继承MonoBehaviour类必须继承DllGenerateBase包装类作为替代

#### 2.3 生成byte字节码
###### 2.3.1 设置生成事件
在VisualStudio  项目->'Dll_Project'属性 -> 生成事件-> 生成后事件命令行 填写 工程的绝对路径：
```
copy "$(TargetDir)$(ProjectName).dll" "D:\vsvrsdk\Assets\Scenes\ILruntimeSample\$(ProjectName)dll.bytes"

copy "$(TargetDir)$(ProjectName).pdb" "D:\vsvrsdk\Assets\Scenes\ILruntimeSample\$(ProjectName)pdb.bytes"
```

或在Unity工程根目录 创建 ```DllProject```  文件夹,将工程放置在此文件夹中 并将 ```.bytes``` 文件指定到 相对目录  如：
```
copy "$(TargetDir)$(ProjectName).dll" "$(SolutionDir)..\Assets\Scenes\ILruntimeSample\$(ProjectName).dll.bytes"

copy "$(TargetDir)$(ProjectName).pdb" "$(SolutionDir)..\Assets\Scenes\ILruntimeSample\$(ProjectName).pdb.bytes"
```

###### 2.3.1 生成字节码
解决方案配置 设置为 ```Release``` 

点击 生成->生成解决方案后 会自动创建 两个 ```.bytes``` 文件 分别为：
```Dll_Projectdll.bytes```  和 ```Dll_Projectpdb.bytes``` ,两个文件为unity场景所需要的交互逻辑字节码文件。

生成后事件命令行配置为Unity工程目录中的场景所在路径 后生成的 字节码文件则自动会出现在 Unity场景所在路径内,若保持默认的  ```$(ProjectDir)unitybytes``` 则需要在生成后手动将 ```unitybytes``` 中的两个字节码文件拷贝到Unity工程内的场景所在目录。


#### 2.4 Unity工程配置
###### 2.4.1 配置加载字节码
将```DllManager.cs```组件拖入SDK场景任意物体中(**一个场景中只能存在一个```DllManager.cs```组件**),然后将```Dll_Projectdll.bytes```,```Dll_Projectpdb.bytes```分别拖入```DllAsset```,```PdbAsset```中
###### 2.4.2 配置交互脚本
将```GeneralDllBehavior```组件托入SDK场景任意物体中,```ScriptClassName```中填写包装类名(如果有命名空间,填写时也必须包含命名空间名称如```Dll_Project.ClickDemo```,如果不包含命名空间则直接填写类名，如```ClickDemo```),这个类就可以像平时写unity的MonoBehaviour类一样了。

将```GeneralDllBehavior```组件所在物体拖入2.4.1 中创建的 ```DllManager``` 所在 Inspector 中的 ExtralDatas 中，如果有多个```GeneralDllBehavior```，则在DllManager中设置多个 ExtralDatas，并依次拖入。
###### 2.4.3 配置交互脚本初始化
**手动将```GeneralDllBehavior```组件所在物体在Inspector中设置为不活动**，在入口函数中，在初始化时会将组件自动启用（```SetActive(true)```）即完成初始化。
