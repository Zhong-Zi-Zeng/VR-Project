using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TestScript : MonoBehaviour
{
    public RawImage rawImage;
    Texture2D tex;

    void Start()
    {
        // �ǿ�ϥΪ����f
        //trans_api = new unityConnect();
        tex = new Texture2D(4096, 2048);
        
    }

    void FixedUpdate()
    {
        tex.LoadImage(GameData.indexMap);
        rawImage.texture = tex;        
    }
}
