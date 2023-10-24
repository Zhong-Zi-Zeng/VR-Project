// Server
/*
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.IO;

public class Recv_sceen : MonoBehaviour
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
        port = 5555;
        print("TCP Initialized");
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        listener = new TcpListener(anyIP);
        listener.Start();

        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
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

                print(sr.ReadToEnd());//
                string jsonData = sr.ReadLine();
                Debug.Log("Received Data: " + jsonData);

                // Ū���Ϥ�
                Data _imgData = JsonUtility.FromJson<Data>(jsonData);
                imageDatas = _imgData.image;

                // �d�ݮɶ�
                // DateTime currentTime = DateTime.Now;
                // string result = currentTime.ToString("yyyy-MM-dd hh:mm:ss.fff tt");
                // Debug.Log("time: " + result);
            }
        }
        catch(Exception e)
        {
            print(e);
        }
    }

    public class Data
    {
        public byte[] image;
    }

    private void FixedUpdate()  // Unity�L�k�b������k�������]�w texture
    {
        tex.LoadImage(imageDatas);
        img.texture = tex;  // �b RawImage �W��ܹϤ�
    }
}
*/