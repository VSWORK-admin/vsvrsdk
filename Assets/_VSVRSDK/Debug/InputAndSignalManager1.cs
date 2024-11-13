using com.ootii.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAndSignalManager1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject VRPointEnterObj;
    public GameObject ClickGameObj;

    private void Awake()
    {
        InitData();
    }
    void Start()
    {
    }
    private void OnEnable()
    {
        MessageDispatcher.AddListener(WsMessageType.SendChangeObj.ToString(), OnSendChangeObj);
        MessageDispatcher.AddListener(WsMessageType.SendCChangeObj.ToString(), OnSendCChangeObj);
    }
    private void OnDisable()
    {
        MessageDispatcher.RemoveListener(WsMessageType.SendChangeObj.ToString(), OnSendChangeObj);
        MessageDispatcher.RemoveListener(WsMessageType.SendCChangeObj.ToString(), OnSendCChangeObj);
    }
    //替代式转发，真正转发在客户端内
    private void OnSendChangeObj(IMessage msg)
    {
        WsChangeInfo changeInfo = msg.Data as WsChangeInfo;

        MessageDispatcher.SendMessage(this, WsMessageType.RecieveChangeObj.ToString(), changeInfo, 0);

    }
    private void OnSendCChangeObj(IMessage msg)
    {
        WsCChangeInfo changeInfo = msg.Data as WsCChangeInfo;

        MessageDispatcher.SendMessage(this, WsMessageType.RecieveCChangeObj.ToString(), changeInfo, 0);

    }

    private void InitData()
    {
        //初始数据
        WsAvatarFrame test1 = new WsAvatarFrame();
        test1.id = "test1";
        test1.e = true;
        WsAvatarFrameJian jian1 = test1.ToJian();

        WsAvatarFrame test2 = new WsAvatarFrame();
        test2.id = "test2";
        test2.e = true;
        WsAvatarFrameJian jian2 = test2.ToJian();

        mStaticThings.AllStaticAvatarsDic.Add(mStaticThings.I.mAvatarID, test1);
        mStaticThings.AllStaticAvatarsDic.Add("test2", test1);

        mStaticThings.AllStaticAvatarList.Add(mStaticThings.I.mAvatarID);
        mStaticThings.AllStaticAvatarList.Add("test2");

        mStaticThings.AllActiveAvatarList.Add(mStaticThings.I.mAvatarID);
        mStaticThings.AllActiveAvatarList.Add("test2");


        mStaticThings.DynClientAvatarsDic.Add(mStaticThings.I.mAvatarID, jian1);

        mStaticThings.DynClientAvatarsDic.Add("test2", jian2);

    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            TestEventVRLaserChange();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            TestVR_Right_GrabDown();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SendPlaceInfo();
        }
        if (Input.GetMouseButtonDown(0))
        {
            var posX = Input.mousePosition.x;
            var posY = Input.mousePosition.y;
            
            var world = Camera.main.ScreenToWorldPoint(new Vector3(posX, posY, 10));
            Debug.Log(world);

            ClickGameObj.transform.position = world;
            TestClickChooseObject(posX, posY);
        }
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    SendAllPlaceTo();
        //}
    }

    private void TestClickChooseObject(float pointx,float pointy)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(pointx, pointy, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointClick.ToString(), hit.collider.gameObject, 0);
        }
    }

    //测试信号EventVRLaserChange
    private void TestEventVRLaserChange()
    {
        MessageDispatcher.SendMessage(this, VRPointObjEventType.VRPointEnter.ToString(), VRPointEnterObj, 0);
    }

    private void TestVR_Right_GrabDown()
    {
        MessageDispatcher.SendMessage(this, CommonVREventType.VR_Right_GrabDown.ToString(), null, 0);
    }

    //新加位置信息
    private void SendPlaceInfo()
    {
        //位置信息
        WsPlaceMarkList newwpmlist = new WsPlaceMarkList();

        newwpmlist.kind = WsPlaycePortKind.all;
        newwpmlist.id = "test2";
        newwpmlist.gname = "G1";
        newwpmlist.marks = new List<WsPlaceMark>{
            new WsPlaceMark{ dname = "PlayceDot1",id = "test1"},
            new WsPlaceMark{ dname = "PlayceDot1 (1)",id = "test2"},
        };

        MessageDispatcher.SendMessage(this, WsMessageType.RecievePlaceMark.ToString(), newwpmlist, 0);
    }

    private void SendAllPlaceTo()
    {
        MessageDispatcher.SendMessage(true, VrDispMessageType.AllPlaceTo.ToString(), "G1", 0);
    }
}
