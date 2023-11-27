using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class TestScript : MonoBehaviour
{
    private unityConnect trans_api;

    public RawImage img_1;
    public RawImage img_2;
    Texture2D tex_1;
    Texture2D tex_2;
    private bool flag = false;

    void Start()
    {
        // 傳輸使用的接口
        trans_api = new unityConnect();
        tex_1 = new Texture2D(4096, 2048);
        tex_2 = new Texture2D(4096, 2048);
    }

    void Update()
    {
        
        if (GameData.panoramaList.Count > 0) {
            tex_1.LoadImage(GameData.panoramaList[0]);
            img_1.texture = tex_1;

            tex_2.LoadImage(GameData.panoramaList[1]);
            img_2.texture = tex_2;
        }

        if (Input.GetKey(KeyCode.Q) && flag == false)
        {
            flag = true;
            trans_api.SendData("Generate", "panorama2.jpg");
        }

        Debug.Log(GameData.panoramaWithMaskList.Count);
    }
}
