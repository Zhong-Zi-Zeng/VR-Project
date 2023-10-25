// Server
///*
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.IO;

public class Recv_image_text: MonoBehaviour
{
    Thread receiveThread;
    TcpClient client;
    TcpListener listener;
    int port;

    public RawImage img;
    private byte[] imageDatas = new byte[0];
    private Texture2D tex;

    private void Start()
    {
        InitTcp();
        tex = new Texture2D(1280, 720);
    }

    void InitTcp()
    {
        port = 7777;
        print("TCP Initialized");
        IPEndPoint anyIP = new(IPAddress.Parse("127.0.0.1"), port);
        listener = new TcpListener(anyIP);
        listener.Start();

        receiveThread = new(new ThreadStart(ReceiveData))
        {
            IsBackground = true
        };
        receiveThread.Start();
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
                NetworkStream stream = new(client.Client);
                StreamReader sr = new(stream);

                string jsonData = sr.ReadToEnd();
                Debug.Log("Received Data: " + jsonData);

                // 解析 json 
                Data data = JsonUtility.FromJson<Data>(jsonData);

                // 顯示 image 和 text
                imageDatas = data.image;
                Debug.Log("image: " + jsonData);
                Debug.Log("Received text: " + data.text);


            }
        }
        catch (Exception e)
        {
            print(e);
        }
    }

    public class Data
    {
        public byte[] image;
        public string text;
    }

    private void FixedUpdate()  
    {
        tex.LoadImage(imageDatas);
        img.texture = tex;
    }
}
//*/