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
copy "$(TargetDir)$(ProjectName).dll" "D:\vsvrsdk\Assets\Scenes\ILruntime_Example\$(ProjectName)dll.bytes"
copy "$(TargetDir)$(ProjectName).pdb" "D:\vsvrsdk\Assets\Scenes\ILruntime_Example\$(ProjectName)pdb.bytes"
```

或在Unity工程根目录 创建 ```DllProject```  文件夹,将工程放置在此文件夹中 并将 ```.bytes``` 文件指定到 相对目录  如：
```
copy "$(TargetDir)$(ProjectName).dll" "$(SolutionDir)..\..\Assets\Scenes\ILruntime_Example\$(ProjectName)dll.bytes"
copy "$(TargetDir)$(ProjectName).pdb"  "$(SolutionDir)..\..\Assets\Scenes\ILruntime_Example\$(ProjectName)pdb.bytes"
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
将```GeneralDllBehavior```组件托入SDK场景任意物体中,```ScriptClassName```中填写包装类名(如果有命名空间,填写时也必须包含命名空间名称如```Dll_Project.TestSetLeftHand```,如果不包含命名空间则直接填写类名，如```Click_show_hideDemo```),这个类就可以像平时写unity的MonoBehaviour类一样了。

将```GeneralDllBehavior```组件所在物体拖入2.4.1 中创建的 ```DllManager``` 所在 Inspector 中的 ExtralDatas 中，如果有多个```GeneralDllBehavior```，则在DllManager中设置多个 ExtralDatas，并依次拖入。
###### 2.4.3 配置交互脚本初始化
**手动将```GeneralDllBehavior```组件所在物体在Inspector中设置为不活动**，在入口函数中，在初始化时会将组件自动启用（```SetActive(true)```）即完成初始化。

#### 2.5 基本使用
###### 2.5.1 脚本内获取场景物体
使用 ExtralData 类,它在其他脚本中也存在,主要是用来方便获取一些场景中物体的引用而不需要写一些Find代码
如在```Click_show_hideDemo```中需要操作设置某个物体可见则 可以将物体拖入```Click_show_hideDemo```所在的```GeneralDllBehavior```组件中的 ExtralDatas 中 并使用```BaseMono.ExtralDatas``` 来获取到 ```ExtralData[]``` 如代码中所示：
```
public List<GameObject> ClickObjs = new List<GameObject>();
public List<GameObject> ShowObjs = new List<GameObject>();

public override void Awake()
{
    ClickObjs.Clear();
    ShowObjs.Clear();

    foreach (var obj in BaseMono.ExtralDatas)
    {
        if (obj.Target != null)
        {
            if (obj.OtherData.Equals("0"))
            {
                ClickObjs.Add(obj.Target.gameObject);
            }
            else if (obj.OtherData.Equals("1"))
            {
                ShowObjs.Add(obj.Target.gameObject);
            }
        }
    }
}
```
###### 2.5.2 发送和接收VR指令消息
加入引用 ```using com.ootii.Messages;```


发送VR指令信息：使用 ```MessageDispatcher.SendMessage``` 发送 类型为 ```WsMessageType.SendCChangeObj``` 字符串的信息
消息体为 ```WsCChangeInfo```类

```
WsCChangeInfo wsinfo1 = new WsCChangeInfo()
{
    a = "showitem",
    b = showItemName,
    c = string.Empty,
    d = string.Empty,
    e = string.Empty,
    f = string.Empty,
    g = string.Empty,
};

MessageDispatcher.SendMessage(this, WsMessageType.SendCChangeObj.ToString(), wsinfo1, 0);
```



创建接收信息方法
```
void RecieveCChangeObj(IMessage msg)
{
    WsCChangeInfo rinfo = msg.Data as WsCChangeInfo;
    ShowbStr = rinfo.b;
    Debug.LogWarning(ShowbStr);
}
```


绑定和解绑接收信息事件：
```
public override void OnEnable()
{
    MessageDispatcher.AddListener(WsMessageType.RecieveCChangeObj.ToString(), RecieveCChangeObj);
}

public override void OnDisable()
{
    MessageDispatcher.RemoveListener(WsMessageType.RecieveCChangeObj.ToString(), RecieveCChangeObj);
}
```
###### 2.5.3 获取常用的物体
```mStaticThings```类中有VR中常用的物体可以直接用```mStaticThins.I.xxx```单例获取，在使用之前先判断 ```mStaticThins.I```是否不为 ```null```

比如在 ```Dll_Project.TestSetLeftHand```范例中，将一个球在Update中设置到左手手柄位置一直跟随：
```
public override void Update()
{
    if (mStaticThings.I != null) {
        BaseMono.ExtralDatas[0].Target.gameObject.transform.position = mStaticThings.I.LeftHand.position;
    }
}
```

###### 2.5.4 获取手柄点击的物体
监听 string 为 “VRPointClick” 的事件，事件返回的是 GameObject

```
public override void OnEnable()
{
    MessageDispatcher.AddListener(VRPointObjEventType.VRPointClick.ToString(), GetPointEventType);
}

public override void OnDisable()
{
    MessageDispatcher.RemoveListener(VRPointObjEventType.VRPointClick.ToString(), GetPointEventType);
}

void GetPointEventType(IMessage msg)
{
    GameObject pobj = msg.Data as GameObject;
    ClickedObj = pobj;

    HandleGetPointedObj();
}
```

###### 2.5.5 获取手柄按键事件
监听 ```enum CommonVREventType.xxxx.ToString()```  的事件

```
public override void OnEnable()
{
    MessageDispatcher.AddListener(CommonVREventType.VRRaw_RightTrigger.ToString(), GetVRInput);
}

public override void OnDisable()
{
    MessageDispatcher.RemoveListener(CommonVREventType.VRRaw_RightTrigger.ToString(), GetVRInput);
}

Vector2 Rcieved2DAxis;
Float Rcieved1DAxis;

void GetVRInput(IMessage msg)
{
    if(msg.Type == CommonVREventType.VR_LefStickAxis.ToString() || msg.Type == CommonVREventType.VR_RightStickAxis.ToString()){
        Rcieved2DAxis.Value = (Vector2)msg.Data;
    }else if(msg.Type == CommonVREventType.VR_LeftTriggerAxis.ToString() || msg.Type == CommonVREventType.VR_RightTriggerAxis.ToString() || msg.Type == CommonVREventType.VR_LeftGrabAxis.ToString() || msg.Type == CommonVREventType.VR_RightGrabAxis.ToString()){
        Rcieved1DAxis.Value = (float)msg.Data;
    }
}
```
###### 2.5.6 其他事件和设置可以参考  vsvrsdk 工程内的  ```Assets/_VSVRSDK/VRActions``` 中的 playmaker 实现的案例模仿书写C#交互，VRActions功能在 vsvrsdk 工程的 README中有详细介绍