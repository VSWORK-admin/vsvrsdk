using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.Threading;
using System;

public class mqttTest : MonoBehaviour
{
    private MqttClient client;
    Thread mqttthread;
    // Use this for initialization
    void Start()
    {
        //InvokeRepeating("Connectmqtt",1f,1f);
        //Connectmqtt();
        mqttthread = new Thread(Connectmqtt);
        mqttthread.Start();//开启线程1
    }



    void Connectmqtt()
    {
        // create client instance 
        client = new MqttClient(IPAddress.Parse("192.168.0.118"), 1883, false, null);
		

         while (!client.IsConnected)
        {
            try
            {
                Debug.LogWarning("connecting");
                // register to message received 
                string clientId = Guid.NewGuid().ToString();
                client.Connect(clientId, "client", "client");
                // subscribe to the topic "/home/temperature" with QoS 2 
                client.Subscribe(new string[] { "hello/world" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                client.MqttMsgDisconnected += client_MqttMsgDisconnected;
            }
            catch
            {
                Debug.LogWarning("connect false");
                mqttthread.Abort();
                Thread.Sleep(1000);
                Debug.LogWarning("reconnect");
                mqttthread.Start();//开启线程1
            }
        }
    }

    void OnApplicationQuit()
    {
		Debug.LogWarning("mqtt abort");
    }

    void client_MqttMsgDisconnected(object sender, EventArgs e)
    {
        Debug.Log("Disconnect: " + e.ToString());
        client.MqttMsgPublishReceived -= client_MqttMsgPublishReceived;
        client.MqttMsgDisconnected -= client_MqttMsgDisconnected;
    }


    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
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



    }
}
