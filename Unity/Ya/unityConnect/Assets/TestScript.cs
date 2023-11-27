using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        GameManager.AddAllStates();
    }

    void Update()
    {
      
        tex.LoadImage(GameData.idMap);
        img.texture = tex;
   
        
    }
}
