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

    //send
    private Thread sendThread;
    private TcpListener reader;
    private TcpClient client_2;

    public RawImage img;
    private byte[] imageDatas = new byte[0];
    private Texture2D tex;
    public Button yourButton;

    void Start()
    {
        InitTcp();

        //receive
        tex = new Texture2D(4096, 2048);    //自動調整圖片大小
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        //send
        sendThread = new Thread(new ThreadStart(SendData));
        sendThread.IsBackground = true;
        sendThread.Start();

        //Button btn = yourButton.GetComponent<Button>();
        //btn.onClick.AddListener(this.SendData);
    }


    void InitTcp()
    {
        print("TCP Initialized");

        IPEndPoint receiveIp = new(IPAddress.Parse("127.0.0.1"), 7777);
        listener = new TcpListener(receiveIp);
        listener.Start();

        IPEndPoint sendIp = new(IPAddress.Parse("127.0.0.1"), 6666);
        reader = new TcpListener(sendIp);
        reader.Start();
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
                
                string jsonData  = sr.ReadToEnd();

                // 解析 json 
                SendDataStruct data = JsonUtility.FromJson<SendDataStruct>(jsonData);

                imageDatas = data.image;
                Debug.Log("Received image: " + data.image);
                //Debug.Log("Received H: " + data.h);
                //Debug.Log("Received W: " + data.w);
                //Debug.Log("Received text: " + data.text);



            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SendData()
    {
        while (true)
        {
            Debug.Log("SendData");
            Thread.Sleep(5000);

            client_2 = reader.AcceptTcpClient();
            NetworkStream sendStream = client_2.GetStream();
            StreamWriter sw = new(sendStream);

            RecvDataStruct sendData = new()
            {
                parameter = 123,  // 要傳的參數
                text = "generate"
            };
            string jsonToSend = JsonUtility.ToJson(sendData);
            sw.Write(jsonToSend);
            sw.Flush();
            Debug.Log("Sended");
        }
    }
    //receive
    [SerializeField]
    public class SendDataStruct
    {
        public List<string> id_map;
        public string h;
        public string w;
        public byte[] image;
        public string text;
    }

    //send
    [SerializeField]
    public class RecvDataStruct
    {
        public int parameter;
        public string text;
    }

    private void FixedUpdate()  
    {
        tex.LoadImage(imageDatas);
        img.texture = tex;
        //Debug.Log(tex.height);
        //Debug.Log(tex.width);
    }
}
