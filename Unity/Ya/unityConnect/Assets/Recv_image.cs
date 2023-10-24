// Server
/*
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.IO;

public class Recv_image: MonoBehaviour
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
        port = 6666;
        print("TCP Initialized");
        IPEndPoint anyIP = new (IPAddress.Parse("127.0.0.1"), port);
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
                NetworkStream stream = new NetworkStream(client.Client);
                StreamReader sr = new StreamReader(stream);
                
                string jsonData = sr.ReadToEnd();
                Debug.Log("Received Data: " + jsonData);

                Data _imgData = JsonUtility.FromJson<Data>(jsonData);
                imageDatas = _imgData.image;
                
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
    }

    private void FixedUpdate()  // Unity無法在接收方法中直接設定 texture
    {
        tex.LoadImage(imageDatas);
        img.texture = tex;  // 在 RawImage 上顯示圖片

    }
}
*/