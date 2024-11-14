using DG.Tweening; // 引入DG.Tweening库，用于动画处理
using System; // 引入系统命名空间
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

        // 重写Init方法，进行初始化操作
        public override void Init()
        {
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

            // 创建并发送房间同步数据
            RoomSycnData roomSycnData = new RoomSycnData()
            {
                a = "GameObjectScaleChange",
                b = obj.name,
                c = isZoomIn.ToString()
            };
            VSEngine.Instance.SendRoomSyncData(roomSycnData);

            // 显示房间日志信息
            string clickedName = $"{obj.name} has clicked! isZoominfo : {isZoomIn}";
            VSEngine.Instance.ShowRoomMarqueeLog(clickedName, InfoColor.green, 5, false);
        }

        // 房间同步数据接收事件处理方法
        private void OnReceiveRoomSyncData(RoomSycnData data)
        {
            if (data.a == "GameObjectScaleChange" && GameObject.Find(data.b) != null)
            {
                HandleScaleChange(data); // 处理缩放变化
            }
        }

        // 处理缩放变化
        private void HandleScaleChange(RoomSycnData data)
        {
            GameObject gameObject = GameObject.Find(data.b); // 找到对应的游戏对象
            if (gameObject != null && bool.TryParse(data.c, out bool isZoomIn)) // 检查对象存在且解析缩放信息成功
            {
                Transform transform = gameObject.GetComponent<Transform>(); // 获取对象的Transform组件
                Vector3 targetScale = isZoomIn ? Vector3.one * 2 : Vector3.one; // 根据缩放信息设置目标缩放值

                // 使用DOTween进行缩放动画
                transform.DOScale(targetScale, 0.5f).SetAutoKill(true);
            }
        }
    }
}
