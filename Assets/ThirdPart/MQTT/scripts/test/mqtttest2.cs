using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.Threading;
using System;

public class mqtttest2 : MonoBehaviour
{
    private MqttClient client;
    Thread mqttthread;
    int _TryCount = 0;
    // Use this for initialization
    void Start()
    {
        _TryContinueConnect();
    }

    private void _TryContinueConnect()
    {
        Thread retryThread = new Thread(new ThreadStart(delegate
        {
            while (client == null || !client.IsConnected)
            {
                if (client == null)
                {
                    Debug.LogWarning("building client");
                    _BuildClient();
                    Thread.Sleep(1000);
                    continue;
                }

                try
                {
                    _TryCount++;
                    Debug.LogWarning("tryconnecting :" + _TryCount);
                    _Connect();
                }
                catch (Exception ce)
                {
                    Debug.LogWarning("re connect exception:" + ce.Message);
                }

            // 如果还没连接不符合结束条件则睡2秒
            if (!client.IsConnected)
                {
                    Thread.Sleep(1000);
                }else{
                    Debug.LogWarning("connected");
                }
            }
        }));

        retryThread.Start();
    }

    void _BuildClient()
    {
        try
        {
            client = new MqttClient(IPAddress.Parse("192.168.0.118"), 1883, false, null);
        }
        catch (Exception e)
        {
            Debug.LogWarning("build client error:" + e.Message);
            return;
        }

        Debug.LogWarning("builded client");
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        client.MqttMsgDisconnected += client_MqttMsgDisconnected;
    }

    void _Connect()
    {
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId, "client", "client");
        // subscribe to the topic "/home/temperature" with QoS 2 
        client.Subscribe(new string[] { "hello/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        
        
    }

    void OnApplicationQuit()
    {
        Debug.LogWarning("mqtt abort");
    }

    void client_MqttMsgDisconnected(object sender, EventArgs e)
    {
        Debug.Log("Disconnect: " + e.ToString());
        _TryContinueConnect();
        //client.MqttMsgPublishReceived -= client_MqttMsgPublishReceived;
        //client.MqttMsgDisconnected -= client_MqttMsgDisconnected;
        //client.Unsubscribe(new string[] { "hello/world" });
    }


    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received topic *" +e.Topic + "* : " + System.Text.Encoding.UTF8.GetString(e.Message));
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 40, 80, 20), "Level 1"))
        {
            Debug.Log("sending...");
            client.Publish("hello/world", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("sent");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(client != null){
            if(client.IsConnected){
                client.Publish("hello/world", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                client.Publish("hello/cccc", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D2!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                client.Publish("hello/dddd", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D3!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            }
        }
        
    }
}
