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


//接收的資料型態 
[SerializeField]
public class RecvDataStruct
{
    public byte[] panoramaWithMask = new byte[0]; // 帶有mask的panorama
    public byte[] panorama = new byte[0]; // 一般的panorama
    public byte[] indexMap = new byte[0]; // 用來儲存每個pixel對應到哪一張mask
    public byte[] idMap = new byte[0]; // 用來儲存每個pixel對應到哪一個物件
    public int progress; // 進度條使用
    public string text; 
}

//傳輸的資料型態
[SerializeField]
public class SendDataStruct 
{
    public string task; // 將指定任務傳給Python
    public string imageName; // User指定的照片名稱
}

public class unityConnect
{
    //receive
    private Thread receiveThread;
    private TcpClient listenClient;
    private TcpListener listener;

    //send
    private TcpClient readClient;
    private TcpListener reader;


    public unityConnect()
    {
        Debug.Log("TCP Initialized");

        // 建立連線
        IPEndPoint receiveIp = new(IPAddress.Parse("127.0.0.1"), 7777);
        listener = new TcpListener(receiveIp);
        listener.Start();
        //192.168.1.10
        IPEndPoint sendIp = new(IPAddress.Parse("127.0.0.1"), 6666);
        reader = new TcpListener(sendIp);
        reader.Start();

        //receive
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }


    public void ReceiveData()
    {
        try
        {
            while (true)
            {
                listenClient = listener.AcceptTcpClient();
                NetworkStream recvStream = listenClient.GetStream();
                StreamReader sr = new(recvStream);
                Debug.Log("Receive something...");

                string jsonData  = sr.ReadToEnd();

                // 解析 json 
                RecvDataStruct data = JsonUtility.FromJson<RecvDataStruct>(jsonData);

                Debug.Log("id"+data.idMap.Length);
                Debug.Log("index"+data.indexMap.Length);
                  
                
                // 將收到的data放到GameData中
                GameData.panoramaWithMaskList.Add(data.panoramaWithMask);
                GameData.panoramaList.Add(data.panorama);
                GameData.indexMap = data.indexMap.Length != 0 ? data.indexMap : GameData.indexMap;
                GameData.idMap = data.idMap.Length != 0 ? data.idMap : GameData.idMap;
                GameData.progress = data.progress;
                GameData.text = data.text;

                //Debug.Log("Received panoramaWithMask: " + data.panoramaWithMask);
                //Debug.Log("Received panorama: " + data.panorama);
                //Debug.Log("Received idMap: " + data.idMap);
                //Debug.Log("Received progress: " + data.progress);
                //Debug.Log("Received text: " + data.text);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SendData(string task, string imageName="")
    {        
        Debug.Log("Send data");

        SendDataStruct sendData = new SendDataStruct
        {
            task = task,
            imageName = imageName
        };
        
        readClient = reader.AcceptTcpClient();
        NetworkStream sendStream = readClient.GetStream();
        StreamWriter sw = new(sendStream);

        // 傳送給python
        string jsonToSend = JsonUtility.ToJson(sendData);
        sw.Write(jsonToSend);
        sw.Flush();
        Debug.Log("Sended");        
    }
}
