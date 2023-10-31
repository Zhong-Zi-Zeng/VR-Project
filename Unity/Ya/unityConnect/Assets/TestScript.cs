using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TestScript : MonoBehaviour
{
    private unityConnect trans_api;

    public RawImage img;
    Texture2D tex;

    void Start()
    {
        // 傳輸使用的接口
        trans_api = new unityConnect();
        tex = new Texture2D(4096, 2048);

    }

    void FixedUpdate()
    {
        tex.LoadImage(GameData.idMap);
        img.texture = tex;
        Debug.Log(GameData.panoramaList.Count);
    }
}
