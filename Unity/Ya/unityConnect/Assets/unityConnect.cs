// Server
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.IO;
using System.Collections.Generic;
using Unity.VisualScripting;

public class unityConnect : MonoBehaviour
{
    //receive
    private Thread receiveThread;
    private TcpClient client;
    private TcpListener listener;

    public RawImage img;
    private byte[] imageDatas = new byte[0];
    private Texture2D tex;

    //send
    private Thread sendThread;

    private void Start()
    {
        InitTcp();

        //receive
        tex = new Texture2D(4096, 2048);    //自動調整圖片大小
        receiveThread = new(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        //send
        sendThread = new Thread(new ThreadStart(SendData));
        sendThread.IsBackground = true;
        sendThread.Start();
    }


    void InitTcp()
    {
        print("TCP Initialized");
        IPEndPoint anyIP = new(IPAddress.Parse("127.0.0.1"), 7777);
        listener = new TcpListener(anyIP);
        listener.Start();
    }

    private void OnDestroy()
    {
        receiveThread.Abort();
    }

    private void ReceiveData()
    {
        print("received something...");
        try
        {
            while (true)
            {
                client = listener.AcceptTcpClient();
                NetworkStream recvStream = client.GetStream();
                StreamReader sr = new(recvStream);
                
                string jsonData = sr.ReadToEnd();
                Debug.Log("Received Data: " + jsonData);

                // 解析 json 
                SendDataStruct data = JsonUtility.FromJson<SendDataStruct>(jsonData);
                
                // 顯示
                Debug.Log("Received list length: " + data.list.Count);  // 2048 * 4096 = 8388608
                Debug.Log("H: " + data.H);
                Debug.Log("W: " + data.W);

                Debug.Log("Received image: " + imageDatas);
                imageDatas = data.image;

                Debug.Log("Received text: " + data.text);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void SendData()
    {
        try
        {
            while (true)
            {
                client = listener.AcceptTcpClient();
                NetworkStream sendStream = client.GetStream();
                StreamWriter sw = new(sendStream);

                RecvDataStruct sendData = new()
                {
                   parameter = 123  // 要傳的參數
                };
                string jsonToSend = JsonUtility.ToJson(sendData);     
                sw.Write(jsonToSend);
                sw.Flush();

                Thread.Sleep(1000);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    //receive
    [SerializeField]
    public class SendDataStruct
    {
        public List<string> list;
        public string H;
        public string W;
        public byte[] image;
        public string text;
    }

    //send
    [SerializeField]
    public class RecvDataStruct
    {
        public int parameter;
    }

    private void FixedUpdate()  
    {
        tex.LoadImage(imageDatas);
        img.texture = tex;
        //Debug.Log(tex.height);
        //Debug.Log(tex.width);
    }
}
