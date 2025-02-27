﻿using DG.Tweening; // 引入DG.Tweening库，用于动画处理
using System.Collections.Generic; // 引入System.Collections.Generic命名空间用于字典
using UnityEngine; // 引入Unity引擎相关命名空间
using VSWorkSDK; // 引入VSWorkSDK库
using VSWorkSDK.Data; // 引入VSWorkSDK数据命名空间


namespace Dll_Project
{
    // ClickDemo类继承自DllGenerateBase
    public class ClickDemo : DllGenerateBase
    {
        // 定义一个VR点对象事件类型，默认为VRPointClick
        public VRPointObjEventType PointEventType = VRPointObjEventType.VRPointClick;


        // 存储每个物体的初始材质的字典
        private Dictionary<string, Material> originalMaterials = new Dictionary<string, Material>();

        
        public Transform targetTransform; //自定义的目标Transform对象

        // 重写Init方法，进行初始化操作
        public override void Init()
        {
            if(BaseMono.ExtralDatas.Length > 0){
                targetTransform = BaseMono.ExtralDatas[0].Target;//获取unity中Inspector 窗口中拖入的物体对象
                Debug.Log("ExtraDataTargetName： " + targetTransform.name);
            }
            
            Debug.Log("Click_Demo Init !");
        }

        // 重写Awake方法，在对象激活时调用
        public override void Awake()
        {
            Debug.Log("Click_Demo Awake !");
            // 订阅点击事件和房间同步数据接收事件
            VSEngine.Instance.OnEventPointClickHandler += OnPointClick;
            VSEngine.Instance.OnEventReceiveRoomSyncData += OnReceiveRoomSyncData;
        }

        // 重写OnDestroy方法，在对象销毁时调用
        public override void OnDestroy()
        {
            base.OnDestroy(); // 调用基类的OnDestroy方法
            // 取消订阅点击事件和房间同步数据接收事件
            VSEngine.Instance.OnEventPointClickHandler -= OnPointClick;
            VSEngine.Instance.OnEventReceiveRoomSyncData -= OnReceiveRoomSyncData;
        }

        // 点击事件处理方法
        private void OnPointClick(GameObject obj)
        {
            if (obj.name.Contains("Cube")) // 检查点击的对象名称是否包含"Cube"
            {
                HandlePointedObject(obj); // 如果包含，处理该对象
            }
        }

        // 处理被点击的对象
        private void HandlePointedObject(GameObject obj)
        {
            Transform transform = obj.GetComponent<Transform>(); // 获取对象的Transform组件
            bool isZoomIn = transform.localScale.x <= 1.5f; // 判断对象当前是否处于缩放状态
            Color randomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value); // 随机生成颜色

            // 创建并发送房间同步数据
            RoomSycnData roomSycnData = new RoomSycnData()
            {
                a = "GameObjectScaleChange",
                b = obj.name,
                c = isZoomIn.ToString(),
                d = ColorUtility.ToHtmlStringRGB(randomColor) // 将颜色转换为HTML字符串格式
            };
            VSEngine.Instance.SendRoomSyncData(roomSycnData);

            // 显示房间日志信息
            string clickedName = $"obj:{obj.name} has clicked! isZoominfo : {isZoomIn}";
            VSEngine.Instance.ShowRoomMarqueeLog(clickedName, InfoColor.green, 5, false);
        }

        // 房间同步数据接收事件处理方法
        private void OnReceiveRoomSyncData(RoomSycnData data)
        {
            if (data.a == "GameObjectScaleChange" && GameObject.Find(data.b) != null)
            {
                HandleScaleChangeAndColor(data); // 处理缩放和颜色变化
            }
        }

        // 处理缩放和颜色变化
        private void HandleScaleChangeAndColor(RoomSycnData data)
        {
            GameObject gameObject = GameObject.Find(data.b); // 找到对应的游戏对象
            if (gameObject != null && bool.TryParse(data.c, out bool isZoomIn)) // 检查对象存在且解析缩放信息成功
            {
                Transform transform = gameObject.GetComponent<Transform>(); // 获取对象的Transform组件
                Vector3 targetScale = isZoomIn ? Vector3.one * 2 : Vector3.one; // 根据缩放信息设置目标缩放值

                // 使用DOTween进行缩放动画
                transform.DOScale(targetScale, 0.5f).SetAutoKill(true);

                // 解析颜色信息并应用新的材质和颜色
                if (ColorUtility.TryParseHtmlString("#" + data.d, out Color color))
                {
                    ApplyNewMaterialWithColor(gameObject, color);
                }
            }
        }

        // 应用新的材质和颜色
        private void ApplyNewMaterialWithColor(GameObject gameObject, Color color)
        {
            // 获取游戏对象的Renderer组件
            Renderer renderer = gameObject.GetComponent<Renderer>();
            // 如果Renderer组件存在
            if (renderer!= null)
            {
                // 如果字典中不包含该游戏对象的名称
                if (!originalMaterials.ContainsKey(gameObject.name))
                {
                    // 将原始材质保存到字典中
                    originalMaterials[gameObject.name] = renderer.material;
                    // 创建一个新的材质实例
                    Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    // 设置新材质的颜色
                    material.color = color;
                    // 将新材质应用到游戏对象上
                    renderer.material = material;
                }
                else
                {
                    // 如果字典中已包含该游戏对象的名称，则直接修改现有材质的颜色
                    renderer.material.color = color;
                }
            }
        }

    }
}
